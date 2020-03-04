using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class VisualizarDiaAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Agente ag = Session["UsuarioLogueado"] as Agente;
                //if (ag.Perfil != PerfilUsuario.Personal)
                //{
                //    Response.Redirect("../default.aspx?mode=trucho");
                //}
                //else
                //{
                    Model1Container cxt = new Model1Container();
                //}
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


        public void CargarValores()
        {
            if (AgenteBuscado != null)
            {
                Agente agenteBuscado = AgenteBuscado;
                DateTime diaBuscado = DiaBuscado;

                lbl_hEntrada.Text = resumendiariobuscado.HEntrada;
                lbl_hSalida.Text = resumendiariobuscado.HSalida;

                lbl_Dia.Text = diaBuscado.ToLongDateString();
                EstadoAgente ea = agenteBuscado.ObtenerEstadoAgenteParaElDia(diaBuscado);

                lbl_Movimiento.Text = ea != null ? "Movimiento agendado para la fecha: " + ea.TipoEstado.Estado : "";
                DivMovimiento.Visible = lbl_Movimiento.Text.Length > 0;
               
                CargarGrillaMarcaciones();
                CargarValoresResumenDiario();
                CargarHVS();
                CargarGrillaDeHorasFechaSeleccionada();
                CargarGrillaSolicitudesEstados();
                CargarGrillaEstados();
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
            Model1Container cxt = new Model1Container();
            DateTime d = DiaBuscado;
            Agente agBuscado = AgenteBuscado;
            ResumenDiario horasDia = cxt.ResumenesDiarios.FirstOrDefault(hd => hd.AgenteId == agBuscado.Id && hd.Dia == d);

            if (horasDia != null)
            {
                lbl_AcumuloMes.Text = horasDia.AcumuloHorasMes;
                lbl_DescontoAAct.Text = horasDia.AcumuloHorasAnioActual;
                lbl_DescontoAAnt.Text = horasDia.AcumuloHorasAnioAnterior;
                lbl_DescontoBonificacion.Text = horasDia.AcumuloHorasBonificacion;
                Panel_DistribucionHoras.Visible = true;

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

        #region Administrar marcaciones

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
           
        }

        protected void gv_Marcaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Marcaciones.PageIndex = e.NewPageIndex;
            CargarGrillaMarcaciones();
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

            Model1Container cxt = new Model1Container();
            ResumenDiario rdCxt = cxt.ResumenesDiarios.First(r => r.Id == ResumenDiarioBuscado.Id);

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

            if ((rdCxt.Cerrado ?? false) == true && rdCxt.AgenteId1 != null && agLogueado.Perfil == PerfilUsuario.Personal && (agLogueado.Jefe || agLogueado.JefeTemporal))
            {
                //el resumen diario esta cerrado, tiene un agente que lo cerró y el usuario logueado es un jefe de personal
                Agente cerradoPor = cxt.Agentes.FirstOrDefault(ag => ag.Id == rdCxt.AgenteId1);
                lb_cerrado_por.Text = " - Cerrado por: " + cerradoPor!= null? cerradoPor.ApellidoYNombre: " Proceso automático";
            }
            else
            {
                lb_cerrado_por.Text = string.Empty;
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


        #endregion

        #region Mostrar solicitudes pendientes estado dia
        //GridView_solicitudes
        private void CargarGrillaSolicitudesEstados()
        {
            Model1Container cxt = new Model1Container();

            var estados = (from sea in cxt.SolicitudesDeEstado
                           where sea.AgenteId == AgenteBuscado.Id && sea.FechaDesde <= DiaBuscado && sea.FechaHasta >= DiaBuscado && sea.Estado == EstadoSolicitudDeEstado.Solicitado
                           select new
                           {
                               Id = sea.Id,
                               AgendadoPor = sea.SolicitadoPor.ApellidoYNombre,
                               Estado = sea.TipoEstadoAgente.Estado,
                               TipoEnfermedad = sea.TipoEnfermedad,
                               Dia = diabuscado
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

    }
}