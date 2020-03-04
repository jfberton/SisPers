using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class ag_dias_mes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MenuAgente.Visible = false;
                MenuJefe.Visible = false;
                MenuPersonalAgente.Visible = false;
                MenuPersonalJefe.Visible = false;

                Agente ag = Session["UsuarioLogueado"] as Agente;
                if (ag.Perfil == PerfilUsuario.Personal)
                {
                    if (ag.Jefe || ag.JefeTemporal)
                    {
                        MenuPersonalJefe.Visible = true;
                    }
                    else
                    {
                        MenuPersonalAgente.Visible = true;
                    }
                }
                else
                {
                    if (ag.Jefe || ag.JefeTemporal)
                    {
                        MenuJefe.Visible = true;
                    }
                    else
                    {
                        MenuAgente.Visible = true;
                    }
                }

                CargarValores();
            }
        }

        private void CargarValores()
        {
            ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;
            CargarGrillaAgentes();
            //lbl_acumulado.Visible = false;
            //AdministrarDiaAgente.Visible = false;
            //panel_horasAgente.Visible = false;
            CargarDDLAnio();
            Session["rd_mes"] = null;
        }

        private void CargarGrillaAgentes()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;

            ddl_agente.Items.Clear();

            //ddl_agente.Items.Add(new ListItem { Text = ag.ApellidoYNombre, Value = ag.Legajo.ToString(), Selected = true });

            using (var cxt = new Model1Container())
            {
                var items = cxt.sp_obtener_agentes_cascada(ag.Id, false).OrderBy(x => x.nivel_para_ordenar).OrderBy(x => x.nombre_agente).ToList();

                foreach (var item in items)
                {
                    ddl_agente.Items.Add(new ListItem { Text = item.nombre_agente, Value = item.legajo.ToString() });
                }

            }
        }

        private void CargarDDLAnio()
        {
            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

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
            using (var cxt = new Model1Container())
            {
                int legajo = Convert.ToInt32(ddl_agente.SelectedItem.Value);
                Agente agcxt = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo && !a.FechaBaja.HasValue);
                bool todos_los_dias_cerrados = true;

                div_datos_cierre_mes.Visible = false;
                VisualizarDiaAgente.Visible = false;

                int mes = ddl_Mes.SelectedIndex + 1;
                int año = Convert.ToInt32(ddl_Anio.Text);

                if (agcxt != null)
                {
                    CierreMensual cierre_mensual = cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == año && cm.Mes == mes && cm.AgenteId == agcxt.Id);
                    //DatosAgenteBuscado.Agente = agcxt;
                    //panel_datos_agente.Visible = true;

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

                    foreach (ResumenDiario rd in rd_mes.OrderBy(t => t.Dia))
                    {
                        //si esta cerrado y tiene horas realizadas las voy acumulando
                        if ((rd.Cerrado ?? false) == true /* && (agcxt.HorarioFlexible ?? false) == true */)
                        {
                            horasBonificacion = HorasString.SumarHoras(new string[] { horasBonificacion, "-" + (rd.AcumuloHorasBonificacion != null ? rd.AcumuloHorasBonificacion : "00:00") });
                            horasAActual = HorasString.SumarHoras(new string[] { horasAActual, (rd.AcumuloHorasAnioActual != null ? rd.AcumuloHorasAnioActual : "00:00") });
                            horasAAnterior = HorasString.SumarHoras(new string[] { horasAAnterior, rd.AcumuloHorasAnioAnterior!= null ? rd.AcumuloHorasAnioAnterior:"00:00" });
                            horasMes = HorasString.SumarHoras(new string[] { horasMes, (rd.AcumuloHorasMes != null ? rd.AcumuloHorasMes : "00:00") });
                        }

                        todos_los_dias_cerrados = todos_los_dias_cerrados && (rd.Cerrado ?? false) == true;

                        ItemGrilla item = new ItemGrilla();

                        item.Dia = rd.Dia;
                        item.Horas = rd != null ? rd.Horas : "00:00";
                        item.PathWarning = rd != null ? rd.Inconsistente ? "~/Imagenes/WarningHeader.gif" : "~/Imagenes/invisible.png" : "~/Imagenes/invisible.png";
                        item.ObservacionInconsistencia = rd != null ? rd.Inconsistente ? rd.ObservacionInconsistente : "" : (rd.Dia.DayOfWeek == DayOfWeek.Saturday || rd.Dia.DayOfWeek == DayOfWeek.Sunday) ? "Sabado - Domingo" : "Dia no procesado";
                        item.Cerrado = rd != null ? (rd.Cerrado ?? false) == true : false;
                        item.CerradoPor = rd != null && (rd.Cerrado ?? false) == true && rd.AgenteId1 != null ? "Cerrado por " + cxt.Agentes.First(a => a.Id == rd.AgenteId1).ApellidoYNombre : "Proceso automático.";
                        item.PathImagenCerrado = rd != null ? ((rd.Cerrado ?? false) == true) ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png" : "~/Imagenes/invisible.png";
                        items.Add(item);
                    }

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

                    }

                    lbl_horasAcumuladasAAct.Text = horasAActual;
                    lbl_horasAcumuladasAAnt.Text = horasAAnterior;
                    lbl_horasAcumuladasMes.Text = horasMes;
                    lbl_horasAdeudadasBonificacion.Text = horasBonificacion;
                    panel_totales_HorarioFlexible.Visible = true;

                    gv_movimientosMes.DataSource = null;
                    gv_movimientosMes.DataSource = items;
                    gv_movimientosMes.DataBind();
                    panelResultadoBusqueda.Visible = true;
                    DesactivarBusqueda(true);
                }
                else
                {
                    Controles.MessageBox.Show(this, "El legajo buscado no existe", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        private void DesactivarBusqueda(bool noPuedeBuscar)
        {
            btn_NuevaBusqueda.Visible = noPuedeBuscar;
            btn_buscar.Enabled = !noPuedeBuscar;
            ddl_agente.Enabled = !noPuedeBuscar;
            ddl_Mes.Enabled = !noPuedeBuscar;
            ddl_Anio.Enabled = !noPuedeBuscar;

            if (!noPuedeBuscar)
            {
                gv_movimientosMes.DataSource = null;
                gv_movimientosMes.DataBind();
                VisualizarDiaAgente.Visible = false;
                //ddl_agente.Text = string.Empty;
            }
        }

        protected void btn_NuevaBusqueda_Click(object sender, EventArgs e)
        {
            DesactivarBusqueda(false);
            panelResultadoBusqueda.Visible = false;
            VisualizarDiaAgente.Visible = false;
            panel_totales_HorarioFlexible.Visible = false;
            //panel_datos_agente.Visible = false;
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
                int legajo = Convert.ToInt32(ddl_agente.SelectedItem.Value);
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo && !a.FechaBaja.HasValue);
                DateTime dia = Convert.ToDateTime(((ImageButton)sender).CommandArgument);
                ResumenDiario rd = cxt.sp_obtener_resumen_diario_agente_fecha(ag.Id, dia.ToShortDateString()).First();

                VisualizarDiaAgente.LoadControl("~/Aplicativo/Controles/AdministrarDiaAgente.ascx");

                VisualizarDiaAgente.AgenteBuscado = ag;
                VisualizarDiaAgente.DiaBuscado = dia;
                VisualizarDiaAgente.ResumenDiarioBuscado = rd;
                VisualizarDiaAgente.CargarValores();

                VisualizarDiaAgente.Visible = true;
            }
        }

    }
}