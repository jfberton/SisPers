using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Marcaciones_Procesar_Agente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente.Visible = !(ag.Jefe || ag.JefeTemporal);

                    CargarValores();
                }
            }
        }

        private void CargarValores()
        {
            ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;
            lbl_acumulado.Visible = false;
            AdministrarDiaAgente.Visible = false;
            panel_horasAgente.Visible = false;
            CargarDDLAnio();
            Session["rd_mes"] = null;
        }

        private void CargarDDLAnio()
        {
            for (int i = 2013; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

        #region Valores del mes

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarGrillaMes();
        }

        struct ItemGrilla
        {
            public DateTime Dia { get; set; }
            public string Horas { get; set; }
            public string PathWarning { get; set; }
            public string ObservacionInconsistencia { get; set; }
            public bool Cerrado { get; set; }
            public string PathImagenCerrado { get; set; }
            public string CerradoPor { get; set; }
        }

        public void CargarGrillaMes()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var cxt = new Model1Container())
            {
                int legajo = 0;


                if (int.TryParse(txt_agente.Text, out legajo))
                {
                    Agente agcxt = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo && !a.FechaBaja.HasValue);
                    bool agenteHorarioFlexiblePuedeAcumularMes = true;
                    bool todos_los_dias_cerrados = true;

                    int mes = ddl_Mes.SelectedIndex + 1;
                    int año = Convert.ToInt32(ddl_Anio.Text);
                    

                    if (agcxt != null)
                    {
                        CierreMensual cierre_mensual = cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == año && cm.Mes == mes && cm.AgenteId == agcxt.Id);
                        DatosAgenteBuscado.Agente = agcxt;
                        panel_horasAgente.Visible = true;

                        BonificacionOtorgada bo = agcxt.BonificacionesOtorgadas.FirstOrDefault(b => b.Mes == mes && b.Anio == año);
                        string horasBonificacion = bo != null ? bo.HorasOtorgadas : "00:00";
                        string horasAAnterior = "00:00";
                        string horasAActual = "00:00";
                        string horasMes = "00:00";

                        //rescuento el total de horas descontadas de bonificacion que se encuentran en los movimientos de horas del mes si el agente no era flexible
                        List<string> horasDEscontadasDeBonificacionesPorMovimientosCuandoNoFueFlexible = (from mh in cxt.MovimientosHoras
                                                                                                          where
                                                                                                                mh.ResumenDiario.AgenteId == agcxt.Id &&
                                                                                                                mh.ResumenDiario.Dia.Month == mes &&
                                                                                                                mh.ResumenDiario.Dia.Year == año &&
                                                                                                                mh.DescontoDeHorasBonificables
                                                                                                          select
                                                                                                                mh.Horas).ToList();

                        string totalHorasDescontadas = HorasString.SumarHoras(horasDEscontadasDeBonificacionesPorMovimientosCuandoNoFueFlexible);
                        //actualizo las horas a cumplir por bonificacion despues de descontar las realizadas por movimientos.
                        horasBonificacion = HorasString.RestarHoras(horasBonificacion, totalHorasDescontadas);
                        
                        var desde_el_ingreso_a_la_funcion_hasta_antes_de_la_carga_de_rd = sw.Elapsed;
                        sw.Restart();
                        List<ItemGrilla> items = new List<ItemGrilla>();

                        List<ResumenDiario> rd_mes = new List<ResumenDiario>();

                        if (Session["rd_mes"] == null)
                        {
                            DateTime dia_fin = DateTime.Today;
                            if (mes != DateTime.Today.Month || año != DateTime.Today.Year)
                            {
                                dia_fin = new DateTime(año, mes, DateTime.DaysInMonth(año, mes));
                            }

                            rd_mes = cxt.sp_obtener_resumenes_diarios_agente_mes(agcxt.Id, dia_fin.ToShortDateString()).OrderBy(x => x.Dia).ToList();
                            Session["rd_mes"] = rd_mes;
                        }
                        else
                        {
                            rd_mes = Session["rd_mes"] as List<ResumenDiario>;
                        }

                        var tiempo_de_carga_lista_rd = sw.Elapsed;
                        sw.Restart();

                        foreach (ResumenDiario rd in rd_mes.OrderBy(t=>t.Dia))
                        {
                            //si esta cerrado y tiene horas realizadas las voy acumulando
                            if ((rd.Cerrado ?? false) == true &&
                                (agcxt.HorarioFlexible ?? false) == true &&
                                rd.AcumuloHorasAnioActual != null &&
                                rd.AcumuloHorasAnioAnterior != null &&
                                rd.AcumuloHorasBonificacion != null &&
                                rd.AcumuloHorasMes != null
                                )
                            {
                                horasBonificacion = HorasString.SumarHoras(new string[] { horasBonificacion, "-" + rd.AcumuloHorasBonificacion });
                                horasAActual = HorasString.SumarHoras(new string[] { horasAActual, rd.AcumuloHorasAnioActual });
                                horasAAnterior = HorasString.SumarHoras(new string[] { horasAAnterior, rd.AcumuloHorasAnioAnterior });
                                horasMes = HorasString.SumarHoras(new string[] { horasMes, rd.Horas });
                            }

                            //La variable ya esta en false o el agente no es de horario flexible o el resumen diario no esta cerrado
                            if (!(agenteHorarioFlexiblePuedeAcumularMes && (agcxt.HorarioFlexible ?? false) == true && (rd.Cerrado ?? false) == true))
                            {
                                agenteHorarioFlexiblePuedeAcumularMes = false;
                            }

                            todos_los_dias_cerrados = todos_los_dias_cerrados && (rd.Cerrado ?? false) == true;

                            ItemGrilla item = new ItemGrilla();

                            item.Dia = rd.Dia;
                            item.Horas = rd != null ? rd.Horas : "00:00";
                            item.PathWarning = rd != null ? rd.Inconsistente ? "~/Imagenes/WarningHeader.gif" : "~/Imagenes/invisible.png" : "~/Imagenes/invisible.png";
                            item.ObservacionInconsistencia = rd != null ? rd.Inconsistente ? rd.ObservacionInconsistente : "" : (rd.Dia.DayOfWeek == DayOfWeek.Saturday || rd.Dia.DayOfWeek == DayOfWeek.Sunday) ? "Sabado - Domingo" : "Dia no procesado";
                            item.Cerrado = rd != null ? (rd.Cerrado ?? false) == true : false;
                            item.CerradoPor = rd != null && (rd.Cerrado ?? false) == true && rd.AgenteId1 != null ? "Cerrado por " + cxt.Agentes.First(a => a.Id == rd.AgenteId1).ApellidoYNombre : "No se registró el agente que cerró este día.";
                            item.PathImagenCerrado = rd != null ? ((rd.Cerrado ?? false) == true) ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png" : "~/Imagenes/invisible.png";
                            items.Add(item);
                        }

                        var tiempo_luego_de_procesar_el_listado = sw.Elapsed;
                        sw.Restart();

                        //Todos los dias cerrados y no es horario flexible y no existe el cierre mensual o tiene marca de que hay que modificar
                        if (todos_los_dias_cerrados && (agcxt.HorarioFlexible ?? false) == false && (cierre_mensual == null || cierre_mensual.Tiene_que_modificar))
                        {
                            div_datos_cierre_mes.Visible = false;
                            btn_cerrar_mes.Visible = true;
                        }
                        else
                        {
                            btn_cerrar_mes.Visible = false;
                            div_datos_cierre_mes.Visible = false;

                            if (todos_los_dias_cerrados && cierre_mensual != null && !cierre_mensual.Tiene_que_modificar)
                            {
                                lbl_cierre_mes_cerrado.Text = ddl_Mes.SelectedItem.Text + " del " + ddl_Anio.SelectedItem.Text;
                                lbl_cierre_fecha_cierre.Text = cierre_mensual.FechaCierre.ToShortDateString();
                                lbl_cierre_horas_anio_actual.Text = cierre_mensual.HorasAnioActual;
                                lbl_cierre_horas_anio_anterior.Text = cierre_mensual.HorasAnioAnterior;

                                if (cierre_mensual.Modificaciones.Count() > 1)
                                {
                                    div_modificaciones.Visible = true;
                                    var modificaciones = (from mm in cierre_mensual.Modificaciones
                                                          select new
                                                          {
                                                              fecha = mm.Fecha,
                                                              horas_anio_anterior = mm.HoraAnioAnterior,
                                                              horas_anio_actual = mm.HoraAnioActual,
                                                              agente = mm.Agente.ApellidoYNombre
                                                          }).ToList();

                                    gv_modificaciones.DataSource = modificaciones;
                                    gv_modificaciones.DataBind();
                                }
                                else
                                {
                                    div_modificaciones.Visible = false;
                                }

                                div_datos_cierre_mes.Visible = true;

                                lbl_acumulado.Text = "El mes fué cerrado exitosamente.-";
                            }
                        }

                        if ((agcxt.HorarioFlexible ?? false) == true)
                        {
                            lbl_horasAcumuladasAAct.Text = horasAActual;
                            lbl_horasAcumuladasAAnt.Text = horasAAnterior;
                            lbl_horasAcumuladasMes.Text = horasMes;
                            lbl_horasAdeudadasBonificacion.Text = horasBonificacion;
                            panel_totales_HorarioFlexible.Visible = true;

                            Model1Container cxt_hf = new Model1Container();
                            HorasMesHorarioFlexible hmf = cxt_hf.HorasMesHorarioFlexibles.FirstOrDefault(hhmm => hhmm.Anio == año && hhmm.Mes == mes && hhmm.AgenteId == agcxt.Id);

                            if (hmf != null)
                            {
                                hmf.HorasAcumuladas = horasMes;

                                if ((hmf.AcumuladoEnTotalAnual ?? false) == true)
                                {
                                    cxt_hf.SaveChanges();
                                }

                                btn_acumularTotales.Visible = agenteHorarioFlexiblePuedeAcumularMes && (hmf.AcumuladoEnTotalAnual ?? false) == false;
                                lbl_acumulado.Visible = (hmf.AcumuladoEnTotalAnual ?? false) == true;
                            }

                        }
                        else
                        {
                            panel_totales_HorarioFlexible.Visible = false;
                        }

                        gv_movimientosMes.DataSource = null;
                        gv_movimientosMes.DataSource = items;
                        gv_movimientosMes.DataBind();
                        panelResultadoBusqueda.Visible = true;
                        DesactivarBusqueda(true);
                        var tiempo_luego_de_procesar_todos_los_rd = sw.Elapsed;
                    }
                    else
                    {
                        Controles.MessageBox.Show(this,"El legajo buscado no existe", Controles.MessageBox.Tipo_MessageBox.Warning);
                    }

                }
                else
                {
                    Controles.MessageBox.Show(this, "El legajo buscado es incorrecto", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        private void DesactivarBusqueda(bool noPuedeBuscar)
        {
            btn_NuevaBusqueda.Visible = noPuedeBuscar;
            btn_buscar.Enabled = !noPuedeBuscar;
            txt_agente.Enabled = !noPuedeBuscar;
            ddl_Mes.Enabled = !noPuedeBuscar;
            ddl_Anio.Enabled = !noPuedeBuscar;

            if (!noPuedeBuscar)
            {
                gv_movimientosMes.DataSource = null;
                gv_movimientosMes.DataBind();
                AdministrarDiaAgente.Visible = false;
                txt_agente.Text = string.Empty;
            }
        }

        protected void btn_NuevaBusqueda_Click(object sender, EventArgs e)
        {
            DesactivarBusqueda(false);
            panelResultadoBusqueda.Visible = false;
            AdministrarDiaAgente.Visible = false;
            panel_totales_HorarioFlexible.Visible = false;
            btn_acumularTotales.Visible = false;
            panel_horasAgente.Visible = false;
            lbl_acumulado.Visible = false;
            Session["rd_mes"] = null;
        }

        protected void gv_movimientosMes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_movimientosMes.PageIndex = e.NewPageIndex;
            CargarGrillaMes();
        }

        protected void btn_AnalizarInconsistencia_Click(object sender, ImageClickEventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int legajo = Convert.ToInt32(txt_agente.Text);
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo && !a.FechaBaja.HasValue);
                DateTime dia = Convert.ToDateTime(((ImageButton)sender).CommandArgument);
                //ResumenDiario rd = cxt.sp_obtener_resumen_diario_agente_fecha(ag.Id, dia.ToShortDateString()).First();//ag.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Dia == dia);

                AdministrarDiaAgente.LoadControl("~/Aplicativo/Controles/AdministrarDiaAgente.ascx");

                AdministrarDiaAgente.AgenteBuscado = ag;
                AdministrarDiaAgente.DiaBuscado = dia;
                AdministrarDiaAgente.CargarValores();

                AdministrarDiaAgente.Visible = true;
            }
        }

        #endregion

        protected void AdministrarDiaAgente_CerroElDia(object sender, EventArgs e)
        {
            Actualizar_Resumendiario_en_listado(AdministrarDiaAgente.ResumenDiarioBuscado);
            CargarGrillaMes();
            //AdministrarDiaAgente.Visible = false;
        }

        protected void AdministrarDiaAgente_PrecionoVolver(object sender, EventArgs e)
        {
            Actualizar_Resumendiario_en_listado(AdministrarDiaAgente.ResumenDiarioBuscado);
            CargarGrillaMes();
            //AdministrarDiaAgente.Visible = false;
        }

        private void Actualizar_Resumendiario_en_listado(ResumenDiario resumenDiarioBuscado)
        {
            List<ResumenDiario> rd_mes = Session["rd_mes"] as List<ResumenDiario>;
            int index = -1;

            foreach (ResumenDiario item in rd_mes)
            {
                if (item.Dia == resumenDiarioBuscado.Dia)
                {
                    index = rd_mes.IndexOf(item);
                }
            }

            if (index >= 0)
            {
                rd_mes.RemoveAt(index);
                rd_mes.Add(resumenDiarioBuscado);
                Session["rd_mes"] = rd_mes;
            }
        }

        protected void btn_acumularTotales_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int legajo = 0;
                if (int.TryParse(txt_agente.Text, out legajo))
                {
                    Agente agcxt = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo && !a.FechaBaja.HasValue);
                    int mes = ddl_Mes.SelectedIndex + 1;
                    int año = Convert.ToInt32(ddl_Anio.Text);
                    HorasMesHorarioFlexible hmf = agcxt.HorasMesHorarioFlexibles.FirstOrDefault(hhmm => hhmm.Anio == año && hhmm.Mes == mes);

                    if (hmf != null)
                    {
                        hmf.HorasAcumuladas = lbl_horasAcumuladasMes.Text;
                        hmf.AcumuladoEnTotalAnual = true;
                    }

                    agcxt.HorasAcumuladasAnioActual = HorasString.SumarHoras(new string[] { agcxt.HorasAcumuladasAnioActual, lbl_horasAcumuladasMes.Text });
                    cxt.SaveChanges();
                }
            }

            Crear_Guardar_Cierre_Mensual();

            CargarGrillaMes();
        }

        protected void AdministrarDiaAgente_AbrioElDia(object sender, EventArgs e)
        {
            //Marco el cierre (si existe) como que tiene que modificar
            using (var cxt = new Model1Container())
            {
                int legajo = 0;
                if (int.TryParse(txt_agente.Text, out legajo))
                {
                    Agente ag_cxt = cxt.Agentes.FirstOrDefault(aa => aa.Legajo == legajo);
                    int mes = ddl_Mes.SelectedIndex + 1;
                    int año = Convert.ToInt32(ddl_Anio.Text);
                    CierreMensual cierre = cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == año && cm.Mes == mes && cm.AgenteId == ag_cxt.Id);

                    if (cierre != null)
                    {
                        cierre.Tiene_que_modificar = true;
                    }

                    cxt.SaveChanges();
                }
            }

            Actualizar_Resumendiario_en_listado(AdministrarDiaAgente.ResumenDiarioBuscado);
            CargarGrillaMes();
            //AdministrarDiaAgente.Visible = false;
        }

        protected void btn_cerrar_mes_Click(object sender, EventArgs e)
        {
            Crear_Guardar_Cierre_Mensual();
            CargarGrillaMes();
        }

        private void Crear_Guardar_Cierre_Mensual()
        {
            int legajo = 0;
            using (var cxt = new Model1Container())
            {
                if (int.TryParse(txt_agente.Text, out legajo))
                {
                    Agente usuario_personal = Session["UsuarioLogueado"] as Agente;
                    Agente ag_cxt = cxt.Agentes.FirstOrDefault(aa => aa.Legajo == legajo);
                    if (ag_cxt != null)
                    {
                        int mes = ddl_Mes.SelectedIndex + 1;
                        int año = Convert.ToInt32(ddl_Anio.Text);
                        CierreMensual cierre = cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == año && cm.Mes == mes && cm.AgenteId == ag_cxt.Id);
                        if (cierre == null)
                        {
                            cierre = new CierreMensual()
                            {
                                AgenteId = ag_cxt.Id,
                                Anio = año,
                                Mes = mes,
                                FechaCierre = DateTime.Today,
                                HorasAnioActual = ag_cxt.HorasAcumuladasAnioActual,
                                HorasAnioAnterior = ag_cxt.HorasAcumuladasAnioAnterior,
                                Tiene_que_modificar = false
                            };

                            cxt.CierreMensual.AddObject(cierre);
                        }

                        cierre.FechaCierre = DateTime.Today;
                        cierre.HorasAnioActual = ag_cxt.HorasAcumuladasAnioActual;
                        cierre.HorasAnioAnterior = ag_cxt.HorasAcumuladasAnioAnterior;
                        cierre.Tiene_que_modificar = false;

                        cierre.Modificaciones.Add(new Modificacion_cierre_mes()
                        {
                            AgenteId_modificacion = usuario_personal.Id,
                            Fecha = DateTime.Today,
                            HoraAnioActual = ag_cxt.HorasAcumuladasAnioActual,
                            HoraAnioAnterior = ag_cxt.HorasAcumuladasAnioAnterior
                        });

                        cxt.SaveChanges();
                    }
                }
            }
        }


    }
}