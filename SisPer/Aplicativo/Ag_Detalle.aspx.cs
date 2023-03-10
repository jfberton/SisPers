using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;

namespace SisPer.Aplicativo
{
    public partial class Ag_Detalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                
                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                int id = 0;
                if (Session["IdAg"] != null)
                {
                    id = Convert.ToInt32(Session["IdAg"]);
                    Session["IdAg"] = null;
                }
                else
                {
                    id = usuarioLogueado.Id;
                }

                //Menues
                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                Model1Container cxt = new Model1Container();
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                DatosAgente1.Agente = ag;
                Calendar1.SelectedDate = DateTime.Today;
                VerificarEstadoAgente();
                CargarGrillaDeHorasFechaSeleccionada();
                CargarHorasTotalesMes();
                CargarLicencias();
            }
        }

        private void CargarLicencias()
        {
            Agente ag = DatosAgente1.Agente;
            Model1Container cxt = new Model1Container();

            #region Licencias anuales
            TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
            LicenciaAgente licAgAnioAct = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
            LicenciaAgente licAgAnioanterior = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

            if (licAgAnioAct == null)
            {
                licAgAnioAct = new LicenciaAgente
                {
                    TipoLicenciaId = tli.Id,
                    Anio = DateTime.Today.Year,
                    AgenteId = ag.Id,
                    DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(ag, DateTime.Today.Year),
                    DiasUsufructuadosIniciales = 0,
                };

                cxt.LicenciasAgentes.AddObject(licAgAnioAct);
                cxt.SaveChanges();
            }

            if (licAgAnioanterior == null)
            {
                licAgAnioanterior = new LicenciaAgente
                {
                    TipoLicenciaId = tli.Id,
                    Anio = DateTime.Today.Year - 1,
                    AgenteId = ag.Id,
                    DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(ag, DateTime.Today.Year - 1),
                    DiasUsufructuadosIniciales = 0,
                };

                cxt.LicenciasAgentes.AddObject(licAgAnioanterior);
                cxt.SaveChanges();
            }

            C_LicAnioEnCurso.DiasOtorgados = licAgAnioAct.DiasOtorgados;
            C_LicAnioEnCurso.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licAgAnioAct);
            C_LicAnioEnCurso.Tipo = tli.Tipo + " " + DateTime.Today.Year.ToString();

            C_LicAnioAnterior.DiasOtorgados = licAgAnioanterior.DiasOtorgados;
            C_LicAnioAnterior.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licAgAnioanterior);
            C_LicAnioAnterior.Tipo = tli.Tipo + " " + (DateTime.Today.Year - 1).ToString();
            #endregion

            #region Licencia especial de invierno

            tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia especial invierno");
            LicenciaAgente licEspInv = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);

            if (licEspInv == null)
            {
                licEspInv = new LicenciaAgente
                {
                    TipoLicenciaId = tli.Id,
                    Anio = DateTime.Today.Year,
                    AgenteId = ag.Id,
                    DiasOtorgados = 10,
                    DiasUsufructuadosIniciales = 0,
                };

                cxt.LicenciasAgentes.AddObject(licEspInv);
                cxt.SaveChanges();
            }

            C_LicEspInvierno.DiasOtorgados = licEspInv.DiasOtorgados;
            C_LicEspInvierno.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licEspInv);
            C_LicEspInvierno.Tipo = tli.Tipo;

            #endregion

            #region Licencia por enfermedad

            tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia enfermedad común");
            LicenciaAgente licEnfComun = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);

            if (licEnfComun == null)
            {
                licEnfComun = new LicenciaAgente
                {
                    TipoLicenciaId = tli.Id,
                    Anio = DateTime.Today.Year,
                    AgenteId = ag.Id,
                    DiasOtorgados = 45,
                    DiasUsufructuadosIniciales = 0,
                };

                cxt.LicenciasAgentes.AddObject(licEnfComun);
                cxt.SaveChanges();
            }

            C_LicEnfComun.DiasOtorgados = licEnfComun.DiasOtorgados;
            C_LicEnfComun.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licEnfComun);
            C_LicEnfComun.Tipo = "Enfermedad común";

            tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia enfermedad familiar");
            LicenciaAgente licEnfFamiliar = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);

            if (licEnfFamiliar == null)
            {
                licEnfFamiliar = new LicenciaAgente
                {
                    TipoLicenciaId = tli.Id,
                    Anio = DateTime.Today.Year,
                    AgenteId = ag.Id,
                    DiasOtorgados = 30,
                    DiasUsufructuadosIniciales = 0,
                };

                cxt.LicenciasAgentes.AddObject(licEnfFamiliar);
                cxt.SaveChanges();
            }

            C_LicEnfFamiliar.DiasOtorgados = licEnfFamiliar.DiasOtorgados;
            C_LicEnfFamiliar.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licEnfFamiliar);
            C_LicEnfFamiliar.Tipo = "Enfermedad familiar";

            int diasenfermedad = C_LicEnfComun.DiasUsufructuados + C_LicEnfFamiliar.DiasUsufructuados;

            if (diasenfermedad <= 20)
            {
                lbl_EnfermedadCGFE.Text = diasenfermedad.ToString();
                lbl_EnfermedadSGFE.Text = "0";
            }
            else
            {
                lbl_EnfermedadCGFE.Text = "20";
                lbl_EnfermedadSGFE.Text = (diasenfermedad - 20).ToString();
            }

            #endregion
        }

        private void VerificarEstadoAgente()
        {
            Agente ag = DatosAgente1.Agente;
            string estado_agente = ag.EstadoActual;
            if (estado_agente != "Activo")
            {
                if (estado_agente == "Sin datos")
                {
                    lbl_EstadoFuera.Text = "No se encuentran datos de la marcación de entrada";
                }
                else
                {
                    lbl_EstadoFuera.Text = "El agente no se encuentra presente por: " + estado_agente.Replace("_", " ");
                }
            }
            else
            {
                lbl_EstadoFuera.Visible = false;
            }
        }

        private void CargarGrillaDeHorasFechaSeleccionada()
        {
            Model1Container cxt = new Model1Container();
            DateTime d = Calendar1.SelectedDate;
            lbl_Dia.Text = "Detalle del " + d.ToLongDateString();
            ResumenDiario horasDia = cxt.ResumenesDiarios.FirstOrDefault(hd => hd.AgenteId == DatosAgente1.Agente.Id && hd.Dia == d);
            if (horasDia != null)
            {
                lbl_totalHorasFechaSeleccionada.Text = "Total acumulado " + horasDia.Horas + " hs.";
                var items = (from mhd in horasDia.MovimientosHoras
                             select new
                             {
                                 Id = mhd.Id,
                                 Movimiento = mhd.Tipo.Tipo,
                                 Operador = mhd.Tipo.Suma ? "(+)" : "(-)",
                                 Horas = mhd.Horas,
                                 //Horasanioanterior = mhd.DescontoDeAcumuladoAnioAnterior ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 //Horasbonific = mhd.DescontoDeHorasBonificables ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 Descripcion = mhd.Descripcion,
                                 AgendadoPor = mhd.AgendadoPor.ApellidoYNombre
                             }).ToList();

                GridViewHorasDia.DataSource = items;
                GridViewHorasDia.DataBind();
            }
            else
            {
                lbl_totalHorasFechaSeleccionada.Text = "Total acumulado 000:00 hs.";
                GridViewHorasDia.DataSource = null;
                GridViewHorasDia.DataBind();
            }
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
            }
            else
            {
                Model1Container cxt = new Model1Container();

                List<Notificacion> notSinEnviar = (from nn in cxt.Notificaciones
                                                   where
                                                   nn.AgenteId == DatosAgente1.Agente.Id &&
                                                   nn.HistorialEstadosNotificacion.FirstOrDefault(nh => nh.Estado.Estado == "Enviada") == null
                                                   select nn).ToList();

                Notificacion notFecha = notSinEnviar.FirstOrDefault(nn => nn.HistorialEstadosNotificacion.First(nh => nh.Estado.Estado == "Generada").Fecha.Date == e.Day.Date);

                if (notFecha != null)
                {
                    e.Cell.BackColor = Color.OrangeRed;
                    e.Cell.ToolTip = "Debe envio de notificación N° " + notFecha.Id.ToString();
                }
                else
                {

                    DateTime d = e.Day.Date;

                    ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(hd => hd.AgenteId == DatosAgente1.Agente.Id && hd.Dia == d);
                    EstadoAgente ea = DatosAgente1.Agente.ObtenerEstadoAgenteParaElDia(d, true);

                    Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == d);

                    if (rd != null)
                    {
                        if (rd.MovimientosHoras.Count() > 0)
                        {
                            e.Cell.BackColor = Color.DarkGreen;
                            e.Cell.ForeColor = Color.Azure;
                            e.Cell.Font.Bold = true;
                            e.Cell.ToolTip = "Resumen de horas del día " + rd.Horas;
                        }
                    }
                    else
                    {
                        e.Cell.Enabled = false;
                    }

                    if (ea != null)
                    {
                        if (ea.TipoEstado.Estado == "Fin de semana")
                        {
                        }
                        else
                        {
                            if (ea.TipoEstado.Estado == "Feriado" && feriado != null)
                            {
                                e.Cell.BackColor = Color.DarkRed;
                                e.Cell.ForeColor = Color.Azure;
                                e.Cell.Font.Bold = true;
                                e.Cell.ToolTip = "Feriado: " + feriado.Motivo;
                            }
                            else
                            {
                                e.Cell.BackColor = Color.DarkGoldenrod;
                                e.Cell.ForeColor = ColorTranslator.FromHtml("#333333");
                                e.Cell.Font.Bold = true;
                                e.Cell.ToolTip = ea.TipoEstado.Estado;
                            }
                        }
                    }
                    else
                    {
                        if (feriado != null)
                        {

                            e.Cell.BackColor = Color.DarkRed;
                            e.Cell.ForeColor = Color.Azure;
                            e.Cell.Font.Bold = true;
                            e.Cell.ToolTip = "Feriado: " + feriado.Motivo;
                        }
                        else
                        {
                            e.Cell.ToolTip = "";
                        }
                    }
                }
            }
        }

        protected void btn_DetalleCierreMes_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            CierreMensual cm = cxt.CierreMensual.First(c => c.Id == id);
            Calendar1.SelectedDate = new DateTime(cm.Anio, cm.Mes, 1); ;
            Calendar1.VisibleDate = Calendar1.SelectedDate;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            CargarGrillaDeHorasFechaSeleccionada();
        }

        protected void btn_VerMesEnCurso_Click(object sender, EventArgs e)
        {
            Calendar1.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Calendar1.VisibleDate = Calendar1.SelectedDate;
            CargarGrillaDeHorasFechaSeleccionada();
        }

        protected void GridViewCierredeMes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GridViewCierredeMes.PageIndex = e.NewPageIndex;
            //CargarGrillaCierresDeMes();
        }

        protected void GridViewHorasDia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHorasDia.PageIndex = e.NewPageIndex;
            CargarGrillaDeHorasFechaSeleccionada();
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendar1.SelectedDate = e.NewDate;
            CargarGrillaDeHorasFechaSeleccionada();
            CargarHorasTotalesMes();
        }

        private void CargarHorasTotalesMes()
        {
            DateTime primerDia = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            using (var cxt = new Model1Container())
            {
                var horaspos = (from mmhh in cxt.MovimientosHoras
                                where
                                  mmhh.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                                  mmhh.ResumenDiario.Dia >= primerDia &&
                                  mmhh.ResumenDiario.Dia <= ultimoDia &&
                                  mmhh.Tipo.Suma &&
                                  !mmhh.DescontoDeHorasBonificables
                                select new { mmhh.Horas }).ToList();

                var horasneg = (from mmhh in cxt.MovimientosHoras
                                where
                                  mmhh.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                                  mmhh.ResumenDiario.Dia >= primerDia &&
                                  mmhh.ResumenDiario.Dia <= ultimoDia &&
                                  !mmhh.Tipo.Suma
                                select new { mmhh.Horas }).ToList();

                List<string> horasPositivasMes = new List<string>();
                List<string> horasNegativasMes = new List<string>();

                foreach (var item in horaspos)
                {
                    horasPositivasMes.Add(item.Horas);
                }

                foreach (var item in horasneg)
                {
                    horasNegativasMes.Add(item.Horas);
                }

                string horasPositivas = HorasString.SumarHoras(horasPositivasMes);
                string horasNegativas = HorasString.SumarHoras(horasNegativasMes);

                string horastotalesMes = HorasString.RestarHoras(horasPositivas, horasNegativas);

                /*cargo el detalle de los movimientos del mes*/
                var items = (from mhd in cxt.MovimientosHoras
                             where
                                mhd.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                                mhd.ResumenDiario.Dia >= primerDia &&
                                mhd.ResumenDiario.Dia <= ultimoDia
                             select new
                             {
                                 Id = mhd.Id,
                                 Fecha = mhd.ResumenDiario.Dia,
                                 Movimiento = mhd.Tipo.Tipo,
                                 Operador = mhd.Tipo.Suma ? "(+)" : "(-)",
                                 Horas = mhd.Horas,
                                 //Horasanioanterior = mhd.DescontoDeAcumuladoAnioAnterior ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 Horasbonific = mhd.DescontoDeHorasBonificables ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 Descripcion = mhd.Descripcion != "agendado automáticamente" ? mhd.Descripcion : "",
                                 AgendadoPor = mhd.AgendadoPor.ApellidoYNombre
                             }).OrderBy(ii => ii.Fecha).ToList();

                gv_detalle_movimiento.DataSource = items;
                gv_detalle_movimiento.DataBind();

                /*cargo tardanzas del mes*/
                var itemstardanza = (from mhd in cxt.MovimientosHoras
                             where
                                mhd.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                                mhd.ResumenDiario.Dia >= primerDia &&
                                mhd.ResumenDiario.Dia <= ultimoDia &&
                                mhd.Tipo.Tipo == "Tardanza"
                             select new
                             {
                                 Id = mhd.Id,
                                 Fecha = mhd.ResumenDiario.Dia,
                                 Movimiento = mhd.Tipo.Tipo,
                                 Horas = mhd.Horas
                             }).OrderBy(ii=>ii.Fecha).ToList();

                gv_tardanzas_mes.DataSource = itemstardanza;
                gv_tardanzas_mes.DataBind();

                //Cargo valores del cierre de mes
                lbl_mes.Text = "mes de " + Calendar1.SelectedDate.ToString("MMMM 'de' yyyy");

                int mes = Calendar1.SelectedDate.Month;
                int anio = Calendar1.SelectedDate.Year;
                CierreMensual cierre = cxt.CierreMensual.FirstOrDefault(cc => cc.Mes == mes && cc.Anio == anio && cc.AgenteId == DatosAgente1.Agente.Id);
                lbl_cierre_mes_cerrado.Text = Calendar1.SelectedDate.ToString("MMMM 'de' yyyy");
                if (cierre != null)
                {
                    lbl_cierre_fecha_cierre.Text = cierre.FechaCierre.ToShortDateString();
                    lbl_cierre_horas_anio_actual.Text = cierre.HorasAnioActual;
                    lbl_cierre_horas_anio_anterior.Text = cierre.HorasAnioAnterior;
                    lbl_cierre_tardanzas.Text = ObtenerTardanzasMes();
                    lbl_cierre_acumulado_mes.Text = horastotalesMes;

                    if (cierre.Modificaciones.Count() > 1)
                    {
                        div_modificaciones.Visible = true;
                        var modificaciones = (from mm in cierre.Modificaciones
                                              select new
                                              {
                                                  fecha = mm.Fecha,
                                                  horas_anio_anterior = mm.HoraAnioAnterior,
                                                  horas_anio_actual = mm.HoraAnioActual,
                                                  horas_mes = mm.HorasMes,
                                                  agente = mm.Agente.ApellidoYNombre
                                              }).ToList();

                        gv_modificaciones.DataSource = modificaciones;
                        gv_modificaciones.DataBind();
                    }
                    else
                    {
                        div_modificaciones.Visible = false;
                    }
                }
                else
                {
                    lbl_cierre_fecha_cierre.Text = "Actualmente sin cerrar";
                    lbl_cierre_horas_anio_actual.Text = "Actualmente sin cerrar";
                    lbl_cierre_horas_anio_anterior.Text = "Actualmente sin cerrar";
                    lbl_cierre_tardanzas.Text = ObtenerTardanzasMes();
                    lbl_cierre_acumulado_mes.Text = horastotalesMes.Replace("-000:00","000:00").Replace("000:00","00:00");
                    div_modificaciones.Visible = false;
                }
            }
        }

        private string ObtenerTardanzasMes()
        {
            DateTime primerDia = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            using (var cxt = new Model1Container())
            {
                var horasTardanzas = (from mmhh in cxt.MovimientosHoras
                                      where
                                        mmhh.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                                        mmhh.ResumenDiario.Dia >= primerDia &&
                                        mmhh.ResumenDiario.Dia <= ultimoDia &&
                                        mmhh.Tipo.Tipo == "Tardanza"
                                      select new { mmhh.Horas }).ToList();

                List<string> horasTardanzasMes = new List<string>();
                
                foreach (var item in horasTardanzas)
                {
                    horasTardanzasMes.Add(item.Horas);
                }

                return HorasString.SumarHoras(horasTardanzasMes);
            }
        }

        protected void btn_imprimir_detalle_mes_ServerClick(object sender, EventArgs e)
        {
            Reportes.detalle_movimientos_horas_mes_agente_ds ds = ObtenerDetalleMesAgente();

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/detalle_movimientos_horas_mes_agente_r.rdlc");

            ReportDataSource encabezado = new ReportDataSource("General", ds.General.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle.Rows);

            viewer.LocalReport.DataSources.Add(encabezado);
            viewer.LocalReport.DataSources.Add(detalle);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private Reportes.detalle_movimientos_horas_mes_agente_ds ObtenerDetalleMesAgente()
        {
            Reportes.detalle_movimientos_horas_mes_agente_ds ret = new Reportes.detalle_movimientos_horas_mes_agente_ds();

            DateTime primerDia = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            using (var cxt = new Model1Container())
            {
                var items = (from mhd in cxt.MovimientosHoras
                            where
                            mhd.ResumenDiario.AgenteId == DatosAgente1.Agente.Id &&
                            mhd.ResumenDiario.Dia >= primerDia &&
                            mhd.ResumenDiario.Dia <= ultimoDia
                            select new
                            {
                                Id = mhd.Id,
                                Fecha = mhd.ResumenDiario.Dia,
                                Movimiento = mhd.Tipo.Tipo,
                                Operador = mhd.Tipo.Suma ? "(+)" : "(-)",
                                Horas = mhd.Horas,
                                //Horasanioanterior = mhd.DescontoDeAcumuladoAnioAnterior ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                Horasbonific = mhd.DescontoDeHorasBonificables ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                Descripcion = mhd.Descripcion != "agendado automáticamente" ? mhd.Descripcion : "",
                                AgendadoPor = mhd.AgendadoPor.ApellidoYNombre
                            }).OrderBy(ii => ii.Fecha).ToList();

                Reportes.detalle_movimientos_horas_mes_agente_ds.GeneralRow gr = ret.General.NewGeneralRow();
                gr.Agente = DatosAgente1.Agente.ApellidoYNombre;
                gr.Mes = primerDia.ToString(" MMMM 'de' yyyy");
                ret.General.Rows.Add(gr);

                foreach (var item in items)
                {
                    Reportes.detalle_movimientos_horas_mes_agente_ds.DetalleRow dr = ret.Detalle.NewDetalleRow();
                    dr.AgendadoPor = item.AgendadoPor;
                    dr.Fecha = item.Fecha.ToShortDateString();
                    dr.DescontoBonif = item.Horasbonific == "~/Imagenes/accept.png" ? "SI" : "NO";
                    dr.Horas = item.Operador + item.Horas;
                    dr.Movimiento = item.Movimiento;
                    ret.Detalle.Rows.Add(dr);
                }
            }

            return ret;
        }

        protected void ImprirPlanilla_Click(object sender, EventArgs e)
        {
            Agente ag = DatosAgente1.Agente;
            List<Agente> agentes = new List<Agente>();
            agentes.Add(ag);
            ReporteMensualFichadasHoras rp = new ReporteMensualFichadasHoras(agentes, Calendar1.SelectedDate.Month, Calendar1.SelectedDate.Year);

            byte[] pdf = rp.GenerarPDFAsistenciaMensual();
            Session["Bytes"] = pdf;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);

        }
    }
}