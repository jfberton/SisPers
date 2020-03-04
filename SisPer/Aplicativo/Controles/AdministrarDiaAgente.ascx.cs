using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class AdministrarDiaAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;
                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    Model1Container cxt = new Model1Container();

                    ddl_Estados.DataValueField = "Id";
                    ddl_Estados.DataTextField = "Estado";
                    ddl_Estados.DataSource = cxt.TiposEstadoAgente.Where(tea => tea.MarcaPersonal).ToList();
                    ddl_Estados.DataBind();
                }
            }
        }

        private Agente agentebuscado;
        public Agente AgenteBuscado
        {
            get
            {
                if (agentebuscado == null)
                {
                    int id = 0;
                    if (int.TryParse(inputAgenteBuscado.Value, out id))
                    {
                        var cxt = new Model1Container();
                        agentebuscado = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                    }
                }
                return agentebuscado;
            }
            set
            {
                agentebuscado = value;
                inputAgenteBuscado.Value = agentebuscado != null ? value.Id.ToString() : "0";
            }
        }

        private DateTime diabuscado;
        public DateTime DiaBuscado
        {
            get
            {
                diabuscado = Convert.ToDateTime(inputDiaBuscado.Value);
                return diabuscado;
            }
            set
            {
                diabuscado = value;
                inputDiaBuscado.Value = diabuscado.ToString();
            }
        }

        private ResumenDiario resumendiariobuscado;
        public ResumenDiario ResumenDiarioBuscado
        {
            get
            {
                if (resumendiariobuscado == null)
                {
                    int id = 0;
                    if (int.TryParse(inputResumenDiarioBuscado.Value, out id))
                    {
                        var cxt = new Model1Container();
                        resumendiariobuscado = cxt.ResumenesDiarios.FirstOrDefault(a => a.Id == id);
                    }
                }
                return resumendiariobuscado;
            }
            set
            {
                resumendiariobuscado = value;
                inputResumenDiarioBuscado.Value = resumendiariobuscado != null ? value.Id.ToString() : "0";
            }
        }

        private bool _readonly = false;

        public bool ReadOnly
        {
            get
            {
                _readonly = inputReadOnly.Value != string.Empty ? Convert.ToBoolean(inputReadOnly.Value) : _readonly;
                return _readonly;
            }
            set
            {
                _readonly = value;
                inputReadOnly.Value = _readonly.ToString();
            }
        }

        public event System.EventHandler PrecionoVolver;

        public event System.EventHandler CerroElDia;

        public event System.EventHandler AbrioElDia;


        public void CargarValores()
        {
            if (AgenteBuscado != null)
            {
                Agente agenteBuscado = AgenteBuscado;
                DateTime diaBuscado = DiaBuscado;

                lbl_hEntrada.Text = " (" + agenteBuscado.ObtenerHoraEntradaLaboral(diabuscado) + ")";
                lbl_hSalida.Text = " (" + agenteBuscado.ObtenerHoraSalidaLaboral(diabuscado) + ")";

                lbl_Dia.Text = diaBuscado.ToLongDateString();
                EstadoAgente ea = agenteBuscado.ObtenerEstadoAgenteParaElDia(diaBuscado);

                lbl_Movimiento.Text = ea != null ? "Movimiento agendado para la fecha: " + ea.TipoEstado.Estado : "";
                DivMovimiento.Visible = lbl_Movimiento.Text.Length > 0;

                panel_movimiento.Visible = !ReadOnly;
                //btn_CerrarDia.Visible = !ReadOnly;
                //btn_abrir_dia.Visible = !ReadOnly;
                gv_Marcaciones.Enabled = !ReadOnly;
               
                CargarGrillaMarcaciones();
                CargarValoresResumenDiario();
                CargarHVS();
                CargarGrillaDeHorasFechaSeleccionada();
                CargarGrillaEstados();
                CargarGrillaSolicitudesEstados();
                CargarDropDownList();
                if ((resumendiariobuscado.Cerrado ?? false) == true)
                {
                    panel_estado_dia.Visible = false;
                    Panel_AgendarHoras.Visible = false;
                    Panel_DistribucionHoras.Visible = true;
                }
                else
                {
                    panel_estado_dia.Visible = true;
                    Panel_AgendarHoras.Visible = true;

                    ddl_Estados.Enabled = true;
                    ddl_anio_estado_licencia.Enabled = true;
                    btn_GuardarComoCorrecto.Enabled = true;
                }
            }
        }

        struct ItemList
        {
            private int id;
            public int Id
            {
                set { id = value; }
                get { return id; }
            }

            private string valor;
            public string Valor
            {
                set { valor = value; }
                get { return valor; }
            }
            /// <summary>
            /// Instancia un nuevo itemlist
            /// </summary>
            /// <param name="p">Id</param>
            /// <param name="q">Valor</param>
            public ItemList(int p, string q)
            {
                id = p;
                valor = q;
            }
        }

        private void CargarDropDownList()
        {
            try
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                List<ItemList> lista = new List<ItemList>();
                ItemList item = new ItemList(0, "Ninguno");
                lista.Add(item);
                var items = from pp in cxt.TiposMovimientosHora
                            where (pp.Manual == true)
                            select new { Id = pp.Id, Valor = pp.Tipo };

                foreach (var i in items)
                {
                    lista.Add(new ItemList(i.Id, i.Valor));
                }
                DropDownList1.DataValueField = "Id";
                DropDownList1.DataTextField = "Valor";
                DropDownList1.DataSource = lista;
                DropDownList1.DataBind();
            }
            catch { }
        }

        private void CargarHVS()
        {
            Model1Container cxt = new Model1Container();
            DateTime d = DiaBuscado;
            Agente agBuscado = AgenteBuscado;

            var hvsSolicitados = (from hv in cxt.HorariosVespertinos
                                  where
                                     hv.Estado == EstadosHorarioVespertino.Terminado &&
                                     hv.AgenteId == agBuscado.Id &&
                                     hv.Dia == d
                                  select new
                                  {
                                      Id = hv.Id,
                                      Legajo = hv.Agente.Legajo,
                                      Agente = hv.Agente.ApellidoYNombre,
                                      Dia = hv.Dia,
                                      Desde = hv.HoraInicio,
                                      Hasta = hv.HoraFin,
                                      Motivo = hv.Motivo
                                  }).OrderBy(l => l.Legajo).ToList();

            if (hvsSolicitados.Count > 0)
            {
                gv_HV.DataSource = hvsSolicitados;
                gv_HV.DataBind();
                div_horarios_vespertinos.Visible = true;
            }
            else
            {
                div_horarios_vespertinos.Visible = false;
            }

        }

        #region Marcaciones de horas agendadas

        private void CargarGrillaDeHorasFechaSeleccionada()
        {
            //bool considera_prolongacion = chk_considera_prolongacion.Checked;
            Model1Container cxt = new Model1Container();
            DateTime d = DiaBuscado;
            Agente agBuscado = AgenteBuscado;
            ResumenDiario horasDia = cxt.ResumenesDiarios.FirstOrDefault(hd => hd.AgenteId == agBuscado.Id && hd.Dia == d);

            if (horasDia != null)
            {
                if (agBuscado.ObtenerEstadoAgenteParaElDia(d) == null)
                {
                    //Agrego los movimientos que se van a guardar tardanzas, prolongaciones de jornada, salidas antes de hora.
                    if (horasDia.Horas != " - " && (agBuscado.HorarioFlexible ?? false) == false && !ObtenerDiaProcesado().Cerrado)
                    {//buscar tardanzas, prolongacion de jornada, salida antes de hora.

                        //Tardanzas
                        if (HorasString.AMayorQueB(horasDia.HEntrada, HorasString.SumarHoras(new string[] { agBuscado.ObtenerHoraEntradaLaboral(d), "00:05" })))
                        {
                            TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Tardanza");
                            horasDia.MovimientosHoras.Add(new MovimientoHora()
                            {
                                Horas = Recortar000(HorasString.RestarHoras(horasDia.HEntrada, HorasString.SumarHoras(new string[] { agBuscado.ObtenerHoraEntradaLaboral(d), "000:05" })).Replace("-", "")),
                                Tipo = tmh,
                                AgenteId = agBuscado.Id
                            });

                        }

                        //Salidas despues de hora Prolongacion de jornada
                        if (HorasString.AMayorQueB(horasDia.HSalida, agBuscado.ObtenerHoraSalidaLaboral(d)) /*&& considera_prolongacion*/)
                        {
                            string horasDeMas = HorasString.RestarHoras(horasDia.HSalida, agBuscado.ObtenerHoraSalidaLaboral(d));
                            //Permito que se agende hasta una hora como prolongacion de jornada, desde ahi en mas debe hacerce
                            //un horario vespertino.
                            TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Prolongación de jornada");

                            if (HorasString.AMayorQueB(horasDeMas, "000:30"))
                            {
                                horasDia.MovimientosHoras.Add(new MovimientoHora()
                                {
                                    Horas = "00:30",
                                    Tipo = tmh,
                                    AgenteId = agBuscado.Id
                                });
                            }
                            else
                            {
                                horasDia.MovimientosHoras.Add(new MovimientoHora()
                                {
                                    Horas = Recortar000(horasDeMas),
                                    Tipo = tmh,
                                    AgenteId = agBuscado.Id
                                });
                            }
                        }

                        //Salidas antes de hora 
                        //Controlo que la marcacion de salida es antes de la hora en la que debe salir y tambien
                        //que no tenga ninguna salida que termino a las 13
                        if (HorasString.AMayorQueB(agBuscado.ObtenerHoraSalidaLaboral(d), horasDia.HSalida))
                        {
                            TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Salida antes de hora");

                            if (agBuscado.Salidas.Where(s => s.Dia == d && s.HoraHasta == "13:00").Count() == 0)
                            {
                                horasDia.MovimientosHoras.Add(new MovimientoHora()
                                {
                                    Horas = Recortar000(HorasString.RestarHoras(agBuscado.ObtenerHoraSalidaLaboral(d), horasDia.HSalida)),
                                    Tipo = tmh,
                                    AgenteId = agBuscado.Id
                                });
                            }
                        }
                    }

                    if ((agBuscado.HorarioFlexible ?? false) == true && !ObtenerDiaProcesado().Cerrado)
                    {
                        //El agente posee horario flexible y no se cerro el dia, tengo que calcular las horas trabajadas y sumarlas 
                        TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Horas trabajadas");
                        horasDia.HEntrada = ddl_HoraEntrada.Text != null && ddl_HoraEntrada.Text.Length == 5 ? ddl_HoraEntrada.Text : "00:00";
                        horasDia.HSalida = ddl_HoraSalida.Text != null && ddl_HoraEntrada.Text.Length == 5 ? ddl_HoraSalida.Text : "00:00";
                        bool toma630 = HorasString.AMayorQueB("06:30", horasDia.HEntrada);

                        if (horasDia.HEntrada != "00:00")
                        {
                            horasDia.MovimientosHoras.Add(new MovimientoHora()
                            {
                                Horas = toma630 ? Recortar000(HorasString.RestarHoras(horasDia.HSalida, "06:30")) : Recortar000(HorasString.RestarHoras(horasDia.HSalida, horasDia.HEntrada)),
                                Tipo = tmh,
                                AgenteId = agBuscado.Id
                            });
                        }
                    }


                }

                if ((agBuscado.HorarioFlexible ?? false) == true && ObtenerDiaProcesado().Cerrado)
                {
                    //El agente posee horario flexible y se cerro el dia, tengo que ocultar el panel agendar movimientoHora y mostrar el de distribuir hora
                    Panel_AgendarHoras.Visible = false;

                    lbl_AcumuloMes.Text = horasDia.AcumuloHorasMes;
                    lbl_DescontoAAct.Text = horasDia.AcumuloHorasAnioActual;
                    lbl_DescontoAAnt.Text = horasDia.AcumuloHorasAnioAnterior;
                    lbl_DescontoBonificacion.Text = horasDia.AcumuloHorasBonificacion;

                    Panel_DistribucionHoras.Visible = true;
                }
                else
                {
                    Panel_AgendarHoras.Visible = true;
                    Panel_DistribucionHoras.Visible = false;
                }

                lbl_totalHorasFechaSeleccionada.Text = "Total acumulado " + horasDia.HorasConMovimientosSinCerrar() + " hs.";
                var items = (from mhd in horasDia.MovimientosHoras
                             select new
                             {
                                 Id = mhd.Id,
                                 AgregarAlCerrar = mhd.Id == 0 ? "~/Imagenes/add.png" : "~/Imagenes/invisible.png",
                                 Movimiento = mhd.Tipo.Tipo,
                                 Operador = mhd.Tipo.Suma ? "(+)" : "(-)",
                                 Horas = mhd.Horas,
                                 Observaciones = mhd.Descripcion,
                                 Horasanioanterior = mhd.DescontoDeAcumuladoAnioAnterior ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 Horasbonific = mhd.DescontoDeHorasBonificables ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 AgendadoPor = mhd.AgendadoPor.Id == mhd.ResumenDiario.AgenteId ? "Agendado automáticamente" : "Agendado por: " + mhd.AgendadoPor.ApellidoYNombre
                             }).ToList();

                GridView1.DataSource = items;
                GridView1.DataBind();

            }
            else
            {
                lbl_totalHorasFechaSeleccionada.Text = "";
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        private string Recortar000(string p)
        {
            if (p.Split(':')[0].Substring(0, 1) == "0")
            {
                p = p.Substring(1, 5);
            }

            return p;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarGrillaDeHorasFechaSeleccionada();
        }

        #endregion

        #region agendar movimientos horas

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            Page.Validate("MovimientoHora");
            if (Page.IsValid)
            {
                DateTime d = DiaBuscado;
                Agente ag = AgenteBuscado;
                Agente agendador = Session["UsuarioLogueado"] as Agente;
                string horas = tb_AgendarHoras.Text;

                TipoMovimientoHora tmh = MovimientoSeleccionado();
                string descripcion = tb_descripcion.Text;
                Model1Container cxt = new Model1Container();
                ResumenDiario rD = cxt.ResumenesDiarios.FirstOrDefault(rd => rd.AgenteId == AgenteBuscado.Id && rd.Dia == d);
                if ((rD == null) ||
                    (rD != null &&
                        rD.MovimientosHoras.Where(mh =>
                            mh.AgendadoPor.Id == agendador.Id &&
                            mh.Tipo.Id == tmh.Id &&
                            mh.Horas == horas).Count() == 0
                        )
                    )
                {
                    if (rD != null)
                    {
                        if (rD.MovimientosHoras.Where(mh => mh.Tipo.Id == tmh.Id).Count() > 0 &&
                            (MovimientoSeleccionado().Tipo == "Tardanza" ||
                            MovimientoSeleccionado().Tipo == "Prolongación de jornada"))
                        {
                            Controles.MessageBox.Show(this.Page, "Los movimientos de tardanza y prolongación de jornada se pueden agendar uno por día unicamente.", Controles.MessageBox.Tipo_MessageBox.Warning, "Movimiento duplicado");
                        }
                        else
                        {
                            ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, agendador, horas, tmh, descripcion);
                        }
                    }
                    else
                    {
                        ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, agendador, horas, tmh, descripcion);
                    }

                }
                else
                {
                    MessageBox.Show(this.Page, "El movimiento que esta intentando agendar ya fue agendado.", Controles.MessageBox.Tipo_MessageBox.Warning, "Movimiento duplicado");
                }

                tb_descripcion.Text = "";
                tb_Horas.Text = "";

                Panel_AgendarHoras.Visible = true;
                CargarValores();
            }
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            tb_descripcion.Text = "";
            tb_Horas.Text = "";

            Panel_AgendarHoras.Visible = true;
        }

        private TipoMovimientoHora MovimientoSeleccionado()
        {
            Model1Container cxt = Session["CXT"] as Model1Container;
            int id = Convert.ToInt32(DropDownList1.SelectedValue);
            return cxt.TiposMovimientosHora.FirstOrDefault(p => p.Id == id);
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = MovimientoSeleccionado() != null;
        }

        #endregion

        #region Administrar marcaciones

        protected void btn_AgregarMarcacion_Click(object sender, EventArgs e)
        {
            Page.Validate("Marcacion");
            if (Page.IsValid)
            {

                ResumenDiario rd = ResumenDiarioBuscado;
                using (Model1Container cxt = new Model1Container())
                {
                    ResumenDiario rdiario = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);
                    rdiario.Marcaciones.Add(new Marcacion()
                    {
                        Hora = tb_Horas.Text,
                        Manual = true,
                        Anulada = false
                    });
                    cxt.SaveChanges();
                    ResumenDiarioBuscado = rdiario;

                    CargarValores();
                }


            }
        }

        private struct ItemGrillaMarcaciones
        {
            public int Id { get; set; }
            public string Hora { get; set; }
            public bool Manual { get; set; }
            public bool Anulada { get; set; }
            public string UrlImagen { get; set; }
            public string ToolTip { get; set; }
        }

        private void CargarGrillaMarcaciones()
        {
            ResumenDiario rd = ResumenDiarioBuscado;

            List<ItemGrillaMarcaciones> items = new List<ItemGrillaMarcaciones>();
            bool diaCerrado = ObtenerDiaProcesado().Cerrado;

            foreach (Marcacion item in rd.Marcaciones)
            {
                items.Add(new ItemGrillaMarcaciones()
                {
                    Id = item.Id,
                    Hora = item.Hora,
                    Manual = item.Manual,
                    Anulada = item.Anulada,
                    UrlImagen = item.Anulada ? "~/Imagenes/accept.png" : "~/Imagenes/cancel.png",
                    ToolTip = item.Anulada ? "Activar" : "Anular"
                });
            }

            gv_Marcaciones.DataSource = items.OrderBy(i => i.Hora).ToList();
            gv_Marcaciones.DataBind();

            if (diaCerrado)
            {
                gv_Marcaciones.Columns[ObtenerColumna("Anul./Act.")].Visible = false;
            }
            else
            {
                gv_Marcaciones.Columns[ObtenerColumna("Anul./Act.")].Visible = true;
            }

            btn_AgregarMarcacion.Enabled = !diaCerrado;
        }

        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_Marcaciones.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_Marcaciones.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        protected void gv_Marcaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Marcaciones.PageIndex = e.NewPageIndex;
            CargarGrillaMarcaciones();
        }

        protected void btn_AnularActivar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton boton = ((ImageButton)sender);

            int idMarcacion = Convert.ToInt32(boton.CommandArgument);

            Model1Container cxt = new Model1Container();

            Marcacion marc = cxt.Marcaciones.FirstOrDefault(m => m.Id == idMarcacion);

            if (marc != null && !ObtenerDiaProcesado().Cerrado)
            {
                marc.Anulada = !marc.Anulada;

                cxt.SaveChanges();
                //Actualizo las marcaciones de la variable de sesion para poder 
                //mostrarla correctamente en el cargargrilla
                ResumenDiario rd = ResumenDiarioBuscado;
                ResumenDiario rdcxt = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);
                //anulo lo que puede ser una entrada o una salida asi que tengo que corregir el resumen diario.
                if (marc.Anulada)
                {
                    if (rdcxt.HEntrada == marc.Hora || rdcxt.HSalida == marc.Hora)
                    {
                        var marcacionesConLaMismaHora = rdcxt.Marcaciones.Where(m => m.Hora == marc.Hora);
                        bool anularLaEntradaOLaSalida = true;
                        foreach (Marcacion item in marcacionesConLaMismaHora)
                        {
                            anularLaEntradaOLaSalida = anularLaEntradaOLaSalida && item.Anulada;
                        }

                        if (anularLaEntradaOLaSalida && rdcxt.HEntrada == marc.Hora)
                        {
                            rdcxt.HEntrada = "000:00";
                        }
                        else
                        {
                            if (anularLaEntradaOLaSalida)
                            {
                                rdcxt.HSalida = "000:00";
                            }
                        }

                        cxt.SaveChanges();
                    }
                }

                ResumenDiarioBuscado = rdcxt;

                CargarValores();
            }
        }

        protected void gv_Marcaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CheckBox cb = ((CheckBox)e.Row.FindControl("chk_anulada"));

            if (cb != null && cb.Checked)
            {
                e.Row.Style.Value = "text-decoration:line-through;";
            }
        }

        #endregion

        #region Administrar resumen diario

        private void CargarValoresResumenDiario()
        {
            Agente agLogueado = Session["UsuarioLogueado"] as Agente;

            ResumenDiario rd = ProcesosGlobales.AnalizarResumenDiario(ResumenDiarioBuscado);

            div_considera_prolongacion_de_jornada.Visible = !(rd.Cerrado ?? false) == true;

            DiaProcesado dp = ObtenerDiaProcesado();

            Model1Container cxt = new Model1Container();
            ResumenDiario rdCxt = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);

            panelDia.Attributes.Clear();

            if (rdCxt.Inconsistente)
            {
                panelDia.Attributes.Add("class", "panel panel-danger");
            }
            else
            {
                panelDia.Attributes.Add("class", "panel panel-success");
            }

            lbl_Estado.Text = rdCxt.Inconsistente ? "Inconsistente (" + rdCxt.ObservacionInconsistente + ")" : "Correcto";
            lbl_Estado.ForeColor = rdCxt.Inconsistente ? Color.DarkRed : Color.DarkGreen;

            if ((rdCxt.Cerrado ?? false) == true && rd.AgenteId1 != null && agLogueado.Perfil == PerfilUsuario.Personal && (agLogueado.Jefe || agLogueado.JefeTemporal))
            {
                //el resumen diario esta cerrado, tiene un agente que lo cerró y el usuario logueado es un jefe de personal
                Agente cerradoPor = cxt.Agentes.First(ag => ag.Id == rd.AgenteId1);
                lb_cerrado_por.Text = " - Cerrado por: " + cerradoPor.ApellidoYNombre;
            }
            else
            {
                lb_cerrado_por.Text = string.Empty;
            }

            List<string> itemsParaDropDownList = new List<string>();

            foreach (Marcacion marca in rdCxt.Marcaciones)
            {
                if (!marca.Anulada)
                {
                    itemsParaDropDownList.Add(marca.Hora);
                }
            }

            ddl_HoraEntrada.DataSource = itemsParaDropDownList.OrderBy(i => i).ToList();
            ddl_HoraEntrada.DataBind();

            ddl_HoraEntrada.Enabled = !dp.Cerrado;
            ddl_HoraSalida.Enabled = !dp.Cerrado;

            if (HorasString.HoraNoNula(rdCxt.HEntrada))
            {
                try
                {
                    ddl_HoraEntrada.Items.FindByValue(rdCxt.HEntrada).Selected = true;
                }
                catch { }
            }

            ddl_HoraSalida.DataSource = itemsParaDropDownList.OrderBy(i => i).ToList();
            ddl_HoraSalida.DataBind();

            if (HorasString.HoraNoNula(rdCxt.HSalida))
            {
                try
                {
                    ddl_HoraSalida.Items.FindByValue(rdCxt.HSalida).Selected = true;
                }
                catch { }
            }

            fs_AgrenarMarcacion.Visible = !dp.Cerrado;

            //btn_CerrarDia.Visible = !dp.Cerrado && !rdCxt.Inconsistente;

            //btn_abrir_dia.Visible = dp.Cerrado && agLogueado.Perfil == PerfilUsuario.Personal && (agLogueado.Jefe || agLogueado.JefeTemporal);

            panel_movimiento.Visible = (rdCxt.Inconsistente && !dp.Cerrado) || ((rdCxt.Agente.HorarioFlexible ?? false) == true && rdCxt.Marcaciones.Count == 0);

            ResumenDiarioBuscado = rdCxt;
        }

        protected void ddl_HoraEntrada_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResumenDiario rd = ResumenDiarioBuscado;
            if (ddl_HoraEntrada.SelectedValue != rd.HEntrada)
            {
                Model1Container cxt = new Model1Container();
                ResumenDiario rdcxt = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);

                rdcxt.HEntrada = ddl_HoraEntrada.Text;
                cxt.SaveChanges();

                ResumenDiarioBuscado = rdcxt;

                CargarValores();
            }
        }

        protected void ddl_HoraSalida_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResumenDiario rd = ResumenDiarioBuscado;
            if (ddl_HoraSalida.SelectedValue != rd.HSalida)
            {
                Model1Container cxt = new Model1Container();
                ResumenDiario rdcxt = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);

                rdcxt.HSalida = ddl_HoraSalida.Text;
                cxt.SaveChanges();

                ResumenDiarioBuscado = rdcxt;

                CargarValores();
            }
        }

        protected void btn_MarcoTardanza_Click(object sender, EventArgs e)
        {
            using (Model1Container cxt = new Model1Container())
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                DateTime d = DiaBuscado;
                Agente ag = AgenteBuscado;
                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(t => t.Tipo == "Tardanza");

                string hsTardanza = ((Button)sender).ToolTip.Split('=')[1].Replace(" hs.", "");
                ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, usuarioLogueado, hsTardanza, tmh, "");

                CargarValores();
            }
        }

        protected void btn_MarcoProlJor_Click(object sender, EventArgs e)
        {
            using (Model1Container cxt = new Model1Container())
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                DateTime d = DiaBuscado;
                Agente ag = AgenteBuscado;
                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(t => t.Tipo == "Prolongación de jornada");

                string hsProlongacion = ((Button)sender).ToolTip.Split('=')[1].Replace(" hs.", "");
                ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, usuarioLogueado, hsProlongacion, tmh, "");

                CargarValores();
            }
        }

        protected void btn_GuardarComoCorrecto_Click(object sender, EventArgs e)
        {
            using (Model1Container cxt = new Model1Container())
            {
                int IDtea = Convert.ToInt32(ddl_Estados.SelectedValue);
                TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(te => te.Id == IDtea);

                ResumenDiario rd = ResumenDiarioBuscado;
                Agente ag = Session["UsuarioLogueado"] as Agente;
                DateTime dia = DiaBuscado;

                Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);
                ResumenDiario rdCxt = cxt.ResumenesDiarios.First(r => r.Id == rd.Id);

                if (tea != null && tea.Estado == "Franco compensatorio")
                {
                    ProcesosGlobales.CrearFrancoAprobado(rdCxt.Agente, agCxt, dia);
                }
                else
                {
                    if (tea != null)
                    {
                        int anio = ddl_anio_estado_licencia.Visible ? Convert.ToInt32(ddl_anio_estado_licencia.SelectedItem.Text) : 0;
                        ProcesosGlobales.AgendarEstadoDiaAgente(agCxt, rdCxt.Agente, dia, tea, anio);
                        rdCxt.Inconsistente = false;
                        cxt.SaveChanges();
                    }
                }

                panel_movimiento.Visible = false;

                CargarValores();
            }
        }

        #endregion

        #region Mostrar y eliminar estados dia.

        private void CargarGrillaEstados()
        {
            Model1Container cxt = new Model1Container();

            var estados = (from ea in cxt.EstadosAgente
                           where ea.AgenteId == AgenteBuscado.Id && ea.Dia == DiaBuscado
                           select new
                           {
                               Id = ea.Id,
                               AgendadoPor = ea.AgendadoPor.ApellidoYNombre,
                               Estado = ea.TipoEstado.Estado,
                               Dia = ea.Dia
                           }).OrderBy(a => a.Dia).ToList();

            if (estados.Count > 0)
            {
                GridViewEstados.DataSource = estados;
                GridViewEstados.DataBind();
                div_estados_agendados.Visible = true;
            }
            else
            {
                div_estados_agendados.Visible = false;
            }
        }

        protected void btn_EliminarEstado_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idEstado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            EstadoAgente ea = cxt.EstadosAgente.First(est => est.Id == idEstado);
            if (ea.TipoEstado.Estado != "Franco compensatorio p/aprobar")
            {
                if (ea.TipoEstado.Estado == "Franco compensatorio")
                {
                    Agente ag = ea.Agente;
                    Franco fra = ag.Francos.FirstOrDefault(f => f.DiasFranco.Where(d => d.Dia == ea.Dia).Count() > 0 && f.Estado == EstadosFrancos.Aprobado);
                    if (fra != null && fra.MovimientosFranco.Count > 1)
                    {
                        MessageBox.Show(this.Page, "No se puede eliminar un franco compensatorio que no haya sido agendado desde esta funcionalidad (El franco que quiere eliminar fue solicitado por el agente y aprobado por todos sus superiores). Si cree que debe eliminarce comuníquese con Sistemas.-", MessageBox.Tipo_MessageBox.Danger);
                    }
                    else
                    {
                        ProcesosGlobales.EliminarFrancoYMovimientos(fra);
                        ResumenDiarioBuscado.Cerrado = false;
                    }
                }
                else
                {
                    //Pongo el resumen diaio como no cerrado
                    ResumenDiario rd = cxt.ResumenesDiarios.First(rrdd => rrdd.Id == ResumenDiarioBuscado.Id);
                    rd.Cerrado = false;

                    //elimino el estado del dia
                    cxt.EstadosAgente.DeleteObject(ea);

                    cxt.SaveChanges();

                    AgenteBuscado = cxt.Agentes.First(a => a.Id == AgenteBuscado.Id);
                    ResumenDiarioBuscado = rd;
                }
            }
            else
            {
                MessageBox.Show(this.Page, "Desde aqui no se puede eliminar un Franco compensatorio pendiente de aprobación.", MessageBox.Tipo_MessageBox.Danger);
            }

            CargarValores();
        }

        #endregion

        #region Mostrar solicitudes pendientes estado dia
        //GridView_solicitudes
        private void CargarGrillaSolicitudesEstados()
        {
            Model1Container cxt = new Model1Container();

            var estados = (from sea in cxt.SolicitudesDeEstado
                           where 
                            sea.AgenteId == AgenteBuscado.Id 
                            && sea.Estado == EstadoSolicitudDeEstado.Solicitado
                            && sea.FechaDesde <= diabuscado
                            && sea.FechaHasta >= diabuscado
                           select new
                           {
                               Id = sea.Id,
                               AgendadoPor = sea.SolicitadoPor.ApellidoYNombre,
                               Estado = sea.TipoEstadoAgente.Estado,
                               TipoEnfermedad = sea.TipoEnfermedad,
                               desde = sea.FechaDesde,
                               hasta = sea.FechaHasta,
                               Dia = DiaBuscado
                           }).OrderBy(a => a.Dia).ToList();

            var estados_con_descripcion_año = (from estado in estados
                                               select new
                                               {
                                                   Id = estado.Id,
                                                   AgendadoPor = estado.AgendadoPor,
                                                   Dia = estado.Dia,
                                                   Estado = (estado.Estado.Contains("Licencia") && !estado.Estado.Contains("Invierno")) ? estado.Estado + " - " + estado.TipoEnfermedad.ToString() : estado.Estado
                                               }).OrderBy(a => a.Dia).ToList();


            if (estados.Count > 0)
            {
                GridView_solicitudes.DataSource = estados_con_descripcion_año;
                GridView_solicitudes.DataBind();
                div_solcitudes_pendientes_dia.Visible = true;
            }
            else
            {
                div_solcitudes_pendientes_dia.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// Devuelve el objeto dia procesado, el mismo puede estar cerrado porque ya se encuentra cerrado el dia o porque tiene como cerrado el resumen diario del agente.
        /// </summary>
        /// <returns></returns>
        private DiaProcesado ObtenerDiaProcesado()
        {
            ResumenDiario rd = ResumenDiarioBuscado;
            DiaProcesado dproc = new DiaProcesado();
            dproc.Cerrado = rd.Cerrado == true;
            return dproc;
        }

        //protected void btn_CerrarDia_Click(object sender, EventArgs e)
        //{
        //    ResumenDiario horasDia = ResumenDiarioBuscado;
        //    Model1Container cxt = new Model1Container();
        //    DateTime dia = DiaBuscado;
        //    Agente ag = AgenteBuscado;
        //    bool considera_prolongacion = chk_considera_prolongacion.Checked;

        //    if (ag.ObtenerEstadoAgenteParaElDia(dia) == null)
        //    {
        //        //Agrego los movimientos que se van a guardar tardanzas, prolongaciones de jornada, salidas antes de hora.
        //        if (horasDia.Horas != " - " && ((horasDia.Agente.HorarioFlexible ?? false) == false) && !ObtenerDiaProcesado().Cerrado)
        //        {//buscar tardanzas, prolongacion de jornada, salida antes de hora.

        //            //Tardanzas
        //            if (HorasString.AMayorQueB(horasDia.HEntrada, HorasString.SumarHoras(new string[] { horasDia.Agente.ObtenerHoraEntradaLaboral(dia), "00:05" })))
        //            {
        //                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Tardanza");
        //                ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(horasDia.Dia, horasDia.Agente, horasDia.Agente, Recortar000(HorasString.RestarHoras(horasDia.HEntrada, HorasString.SumarHoras(new string[] { horasDia.Agente.ObtenerHoraEntradaLaboral(dia), "000:05" })).Replace("-", "")), tmh, "agendado automáticamente");

        //            }

        //            //Salidas despues de hora Prolongacion de jornada
        //            if (HorasString.AMayorQueB(horasDia.HSalida, horasDia.Agente.ObtenerHoraSalidaLaboral(dia)) && considera_prolongacion)
        //            {
        //                string horasDeMas = HorasString.RestarHoras(horasDia.HSalida, horasDia.Agente.ObtenerHoraSalidaLaboral(dia));
        //                //Permito que se agende hasta una hora como prolongacion de jornada, desde ahi en mas debe hacerce
        //                //un horario vespertino.
        //                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Prolongación de jornada");

        //                if (HorasString.AMayorQueB(horasDeMas, "000:30"))
        //                {
        //                    ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(horasDia.Dia, horasDia.Agente, horasDia.Agente, "00:30", tmh, "agendado automáticamente");
        //                }
        //                else
        //                {
        //                    ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(horasDia.Dia, horasDia.Agente, horasDia.Agente, Recortar000(horasDeMas), tmh, "agendado automáticamente");
        //                }
        //            }

        //            //Salidas antes de hora 
        //            //Controlo que la marcacion de salida es antes de la hora en la que debe salir y tambien
        //            //que no tenga ninguna salida que termino a las 13 y que no sea fin de semana (sabado ni domingo)
        //            if (HorasString.AMayorQueB(horasDia.Agente.ObtenerHoraSalidaLaboral(dia), horasDia.HSalida))
        //            {
        //                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Salida antes de hora");

        //                if (horasDia.Agente.Salidas.Where(s => s.Dia == horasDia.Dia && s.HoraHasta == "13:00").Count() == 0 && horasDia.Dia.DayOfWeek != DayOfWeek.Saturday && horasDia.Dia.DayOfWeek != DayOfWeek.Sunday)
        //                {
        //                    ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(horasDia.Dia, horasDia.Agente, horasDia.Agente, Recortar000(HorasString.RestarHoras(horasDia.Agente.ObtenerHoraSalidaLaboral(dia), horasDia.HSalida)), tmh, "agendado automáticamente");
        //                }
        //            }
        //        }

        //        if ((horasDia.Agente.HorarioFlexible ?? false) == true && !ObtenerDiaProcesado().Cerrado && ag.ObtenerEstadoAgenteParaElDia(dia) == null)
        //        {
        //            //El agente posee horario flexible y no se cerro el dia, tengo que calcular las horas trabajadas y sumarlas 
        //            TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Horas trabajadas");

        //            horasDia.HEntrada = ddl_HoraEntrada.Text != null ? ddl_HoraEntrada.Text : "00:00";
        //            horasDia.HSalida = ddl_HoraSalida.Text != null ? ddl_HoraSalida.Text : "00:00";

        //            bool toma630 = HorasString.AMayorQueB("06:30", horasDia.HEntrada);

        //            ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(horasDia.Dia, horasDia.Agente, horasDia.Agente, toma630 ? Recortar000(HorasString.RestarHoras(horasDia.HSalida, "06:30")) : Recortar000(HorasString.RestarHoras(horasDia.HSalida, horasDia.HEntrada)), tmh, "agendado automáticamente");
        //            ProcesosGlobales.DistribuirHorasCierreDiaHorarioFlexible(horasDia);
        //        }

        //    }

        //    Agente agLogueado = Session["UsuarioLogueado"] as Agente;
        //    ResumenDiario rdcxt = cxt.ResumenesDiarios.FirstOrDefault(x => x.Id == horasDia.Id);
        //    Agente agcxt = cxt.Agentes.First(a => a.Id == agLogueado.Id);

        //    rdcxt.CerradoPor = agcxt;
        //    rdcxt.Cerrado = true;

        //    cxt.SaveChanges();

        //    ResumenDiarioBuscado = rdcxt;

        //    CargarValores();

        //    //ListadoAgentesParaGrilla.ActualizarPropiedad(rdcxt.AgenteId, ListadoAgentesParaGrilla.PropiedadPorActualizar.HoraBonificacion, "");

        //    if (this.CerroElDia != null)
        //        this.CerroElDia(this, new EventArgs());

        //}

        protected void btn_Volver_Click(object sender, EventArgs e)
        {
            DateTime dia = DiaBuscado;

            if (this.PrecionoVolver != null)
                this.PrecionoVolver(this, new EventArgs());

        }

        protected void btn_abrir_dia_Click(object sender, EventArgs e)
        {
            Agente responsable = Session["UsuarioLogueado"] as Agente;
            if (ProcesosGlobales.AbrirDiaCerrado(AgenteBuscado.Id, DiaBuscado, responsable.Id))
            {
                //btn_abrir_dia.Visible = false;
                if (this.AbrioElDia != null)
                    this.AbrioElDia(this, new EventArgs());
            }
            else
            {
                MessageBox.Show(this.Page, "Ocurrió un error al intentar abrir el día, consulte con Sistemas.", MessageBox.Tipo_MessageBox.Danger);
            }
        }

        protected void ddl_Estados_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(ddl_Estados.SelectedItem.Value);

            using (var cxt = new Model1Container())
            {
                var tea = cxt.TiposEstadoAgente.FirstOrDefault(tt => tt.Id == id);

                if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
                {
                    lbl_anio.Visible = true;
                    ddl_anio_estado_licencia.Items.Clear();
                    ddl_anio_estado_licencia.Items.Add(new ListItem { Text = (DateTime.Today.Year - 1).ToString() });
                    ddl_anio_estado_licencia.Items.Add(new ListItem { Text = DateTime.Today.Year.ToString() });
                    ddl_anio_estado_licencia.Visible = true;
                }
                else
                {
                    lbl_anio.Visible = false;
                    ddl_anio_estado_licencia.Visible = false;
                }
            }
        }

        protected void ddl_Estados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(ddl_Estados.SelectedItem.Value);

            using (var cxt = new Model1Container())
            {
                var tea = cxt.TiposEstadoAgente.FirstOrDefault(tt => tt.Id == id);

                if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
                {
                    lbl_anio.Visible = true;
                    ddl_anio_estado_licencia.Items.Clear();
                    ddl_anio_estado_licencia.Items.Add(new ListItem { Text = (DateTime.Today.Year - 1).ToString() });
                    ddl_anio_estado_licencia.Items.Add(new ListItem { Text = DateTime.Today.Year.ToString() });
                    ddl_anio_estado_licencia.Visible = true;
                }
                else
                {
                    lbl_anio.Visible = false;
                    ddl_anio_estado_licencia.Visible = false;
                }
            }
        }

        protected void chk_considera_prolongacion_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrillaDeHorasFechaSeleccionada();
        }
    }
}