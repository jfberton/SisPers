using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Jefe_Ag_EntradaSalida : System.Web.UI.Page
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


                if (
                   ag.Perfil != PerfilUsuario.Personal &&
                   !ag.Jefe && !ag.JefeTemporal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {

                    MenuPersonalJefe1.Visible = (ag.Perfil == PerfilUsuario.Personal);
                    MenuJefe1.Visible = !(ag.Perfil == PerfilUsuario.Personal);

                    if (ag.Area.Interior == true)
                    {
                        div_edificioCentral.Visible = false;
                        div_FueraDelEdificio.Visible = true;

                        Calendario.SelectedDate = DateTime.Today;
                        CargarAgentes();
                        btn_Guardar.Enabled = GridView1.Enabled = (VerificarSiPuedeGuardar() && Calendario.SelectedDate <= DateTime.Today);
                    }
                    else
                    {
                        div_edificioCentral.Visible = true;
                        div_FueraDelEdificio.Visible = false;
                    }
                }
            }
        }

        private struct ItemGrilla
        {
            public int Id { get; set; }
            public string Legajo { get; set; }
            public string Nombre { get; set; }
            public string Hentrada { get; set; }
            public string HSalida { get; set; }

            public bool Enabled { get; set; }

            public string DireccionImagen { get; set; }

            public string Tooltip { get; set; }
        }

        private void CargarAgentes()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            DateTime diaBuscado = Calendario.SelectedDate;

            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.FirstOrDefault(a => a.Id == ag.Id);
            var agentes = agCxt.ObtenerAgentesSubordinadosDirectos();
            List<ItemGrilla> listado = new List<ItemGrilla>();

            EntradaSalida e_s = agCxt.EntradasSalidas.FirstOrDefault(es => es.Fecha == diaBuscado);
            ResumenDiario rd = agCxt.ResumenesDiarios.FirstOrDefault(rd1 => rd1.Dia == diaBuscado);

            listado.Add(new ItemGrilla()
            {
                Id = agCxt.Id,
                Legajo = agCxt.Legajo.ToString(),
                Nombre = agCxt.ApellidoYNombre,
                Hentrada = e_s != null ? e_s.Entrada : "00:00",
                HSalida = e_s != null ? e_s.Salida : "00:00",
                Enabled = rd == null ? true : (rd.Cerrado == null ? true : (rd.Cerrado.Value == false ? true : false)),
                DireccionImagen = rd == null ? "" : (rd.Cerrado == null ? "" : (rd.Cerrado.Value == false ? "" : "~/Imagenes/cancel.png")),
                Tooltip = rd == null ? "" : (rd.Cerrado == null ? "" : (rd.Cerrado.Value == false ? "" : "El agente tiene las marcaciones cerradas para el dia, no se pueden modificar."))
            });

            foreach (Agente item in agentes)
            {
                Agente agente = cxt.Agentes.First(a => a.Id == item.Id);
                ResumenDiario rdag = agente.ResumenesDiarios.FirstOrDefault(rd3 => rd3.Dia == diaBuscado);
                e_s = agente.EntradasSalidas.FirstOrDefault(es => es.Fecha == diaBuscado);
                listado.Add(new ItemGrilla()
                {
                    Id = agente.Id,
                    Legajo = agente.Legajo.ToString(),
                    Nombre = agente.ApellidoYNombre,
                    Hentrada = e_s != null ? e_s.Entrada : "00:00",
                    HSalida = e_s != null ? e_s.Salida : "00:00",
                    Enabled = rdag == null ? true : (rdag.Cerrado == null ? true : (rdag.Cerrado.Value == false ? true : false)),
                    DireccionImagen = rdag == null ? "" : (rdag.Cerrado == null ? "" : (rdag.Cerrado.Value == false ? "" : "~/Imagenes/cancel.png")),
                    Tooltip = rdag == null ? "" : (rdag.Cerrado == null ? "" : (rdag.Cerrado.Value == false ? "" : "El agente tiene las marcaciones cerradas para el dia, no se pueden modificar."))
                });
            }

            GridView1.DataSource = listado.OrderBy(s => s.Nombre).ToList();
            GridView1.DataBind();

            foreach (GridViewRow row in GridView1.Rows)
            {
                string nombre = row.Cells[1].Text;
                string hora_Entrada = ((TextBox)row.FindControl("h_entrada")).Text;
                string Hora_Salida = ((TextBox)row.FindControl("h_salida")).Text;
                if (hora_Entrada == "00:00" || Hora_Salida == "00:00")
                {
                    ((TextBox)row.FindControl("h_entrada")).ForeColor = Color.Red;
                    ((TextBox)row.FindControl("h_salida")).ForeColor = Color.Red;
                }
                else
                {
                    ((TextBox)row.FindControl("h_entrada")).ForeColor = Color.Black;
                    ((TextBox)row.FindControl("h_salida")).ForeColor = Color.Black;
                }
            }

            btn_Guardar.Enabled = VerificarSiPuedeGuardar();
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarAgentes();
        }

        protected void Calendario_SelectionChanged(object sender, EventArgs e)
        {
            CargarAgentes();
            btn_Guardar.Enabled = GridView1.Enabled = (VerificarSiPuedeGuardar() && Calendario.SelectedDate <= DateTime.Today);
        }

        private bool VerificarSiPuedeGuardar()
        {
            bool ret = true;

            Agente ag = Session["UsuarioLogueado"] as Agente;
            ///si el dia esta cerrado o no!!!
            ret = ret && !DiaCerrado();
            ret = ret && (ag.Area.Interior == true);
            ret = ret && (Calendario.SelectedDate.DayOfWeek != DayOfWeek.Saturday && Calendario.SelectedDate.DayOfWeek != DayOfWeek.Sunday);
            return ret;
        }

        /// <summary>
        /// Verfica si el dia esta cerrado por personal.
        /// </summary>
        /// <remarks>Verificando un movimiento de algun agente en el día si esta cerrado ya es suficiente.</remarks>
        /// <returns></returns>
        private bool DiaCerrado(Nullable<DateTime> d = null)
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            //dejo por defecto que el dia esta cerrado asi cuando el jefe 
            //no tenga agentes sale el día cerrado y no habilita el boton guardar
            bool ret = true;
            DateTime diaSeleccionado = d != null ? d.Value : Calendario.SelectedDate;

            List<Agente> agentes = ag.ObtenerAgentesSubordinadosDirectos();
            agentes.Add(ag);

            foreach (Agente item in agentes)
            {
                EntradaSalida es = item.EntradasSalidas.FirstOrDefault(io => io.Fecha == diaSeleccionado);
                if (es != null)
                {
                    ret = es.CerradoPersonal;
                    break;
                }
                {
                    //tiene agentes subordinados pero no existen valores cargados el dia seleccionado
                    //asi que devuelvo false para que el dia no le aparezca como cerrado y pueda guardar valores.
                    ret = false;
                }
            }

            return ret;
        }

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            DateTime fecha = Calendario.SelectedDate;
            if (fecha > DateTime.Today)
            {
                Controles.MessageBox.Show(this, "No se pueden guardar entradas y salidas para días posteriores!", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (ChequearQueLasMarcacionesDeTodosTenganEntradaSalida())
                {
                    Guardar();
                }
                else
                {
                    div_Atencion.Visible = true;
                }
            }

            Calendario.Enabled = true;
            btn_Guardar.Enabled = true;
            GridView1.Enabled = true;
        }

        private bool ChequearQueLasMarcacionesDeTodosTenganEntradaSalida()
        {
            List<ItemGrilla> itemsInvalidos = new List<ItemGrilla>();

            foreach (GridViewRow row in GridView1.Rows)
            {
                string nombre = row.Cells[1].Text;
                string hora_Entrada = ((TextBox)row.FindControl("h_entrada")).Text;
                string Hora_Salida = ((TextBox)row.FindControl("h_salida")).Text;
                if (hora_Entrada == "00:00" || Hora_Salida == "00:00")
                {
                    itemsInvalidos.Add(new ItemGrilla() { Nombre = nombre, Hentrada = hora_Entrada, HSalida = Hora_Salida });
                    row.ForeColor = Color.Red;
                    ((TextBox)row.FindControl("h_entrada")).ForeColor = Color.Red;
                    ((TextBox)row.FindControl("h_salida")).ForeColor = Color.Red;
                }
                else
                {
                    row.ForeColor = Color.Black;
                    ((TextBox)row.FindControl("h_entrada")).ForeColor = Color.Black;
                    ((TextBox)row.FindControl("h_salida")).ForeColor = Color.Black;
                }
            }

            gv_Error.DataSource = itemsInvalidos;
            gv_Error.DataBind();

            return itemsInvalidos.Count == 0;
        }

        private void Guardar()
        {
            Calendario.Enabled = false;
            btn_Guardar.Enabled = false;
            GridView1.Enabled = false;
            DateTime fecha = Calendario.SelectedDate;

            Model1Container cxt = new Model1Container();

            try
            {
                for (int i = 0; i < this.GridView1.PageCount; i++)
                {
                    this.GridView1.PageIndex = i;

                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        int idAgente = Convert.ToInt32(((Label)row.FindControl("lbl_IdAgente")).Text);
                        Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == idAgente);
                        Agente jefe = Session["UsuarioLogueado"] as Agente;
                        EntradaSalida e_s = ag.EntradasSalidas.FirstOrDefault(io => io.Fecha == fecha);

                        if (e_s == null)
                        {
                            e_s = new EntradaSalida();
                            cxt.EntradasSalidas.AddObject(e_s);
                        }

                        string hora_Entrada = ((TextBox)row.FindControl("h_entrada")).Text;
                        string Hora_Salida = ((TextBox)row.FindControl("h_salida")).Text;

                        if (!e_s.CerradoPersonal)
                        {
                            e_s.Fecha = fecha;
                            e_s.Entrada = hora_Entrada;
                            e_s.Salida = Hora_Salida;
                            e_s.AgenteId = ag.Id;
                            e_s.AgenteId1 = jefe.Id;
                            e_s.Enviado = true;
                        }
                    }
                }


                cxt.SaveChanges();
                Controles.MessageBox.Show(this, "Los cambios se guardaron correctamente.", Controles.MessageBox.Tipo_MessageBox.Success, "Genial!");
            }
            catch
            {
                Controles.MessageBox.Show(this, "Ocurrió un error al guardar los horarios de entrada y salida. Si el problema persiste informelo al sector Personal.", Controles.MessageBox.Tipo_MessageBox.Danger, "Error");
            }
        }

        protected void btn_Imprimir_Click(object sender, EventArgs e)
        {
            Reportes.HorarioIngresoEgresoMensual ds = GenerarDS();
            RenderReport();
        }

        private Reportes.HorarioIngresoEgresoMensual GenerarDS()
        {
            Reportes.HorarioIngresoEgresoMensual ret = new Reportes.HorarioIngresoEgresoMensual();

            Agente ag = Session["UsuarioLogueado"] as Agente;
            DateTime diaSeleccionado = Calendario.SelectedDate;

            foreach (Agente agente in ag.Area.Agentes.Where(a => a.FechaBaja == null))
            {
                string total_horas_trabajadas = "00:00";
                string total_horas_mas = "00:00";
                string total_horas_menos = "00:00";
                string total_horas_tardanza = "00:00";

                for (int i = 1; i <= 31; i++)
                {
                    DateTime dia = new DateTime();
                    try
                    {
                        dia = new DateTime(diaSeleccionado.Year, diaSeleccionado.Month, i);
                        Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ObtenerDetalle(agente, dia, ret);
                        ret.DetalleDia.Rows.Add(dr);
                        total_horas_mas = HorasString.SumarHoras(new string[] { total_horas_mas, dr.HorasMas });
                        total_horas_menos = HorasString.SumarHoras(new string[] { total_horas_menos, dr.HorasMenos });
                        total_horas_trabajadas = HorasString.SumarHoras(new string[] { total_horas_trabajadas, dr.HorasTrabajadas });
                        total_horas_tardanza = HorasString.SumarHoras(new string[] { total_horas_tardanza, dr.Tardanzas });
                    }
                    catch
                    {
                        Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ret.DetalleDia.NewDetalleDiaRow();
                        dr.Dia = i;
                        dr.AgenteId = agente.Id;
                        dr.IngresoMan = "-";
                        dr.EgresoMan = "-";
                        dr.IngresoTar = "-";
                        dr.EngresoTar = "-";
                        dr.Tardanzas = "-";
                        dr.HorasMas = "-";
                        dr.HorasMenos = "-";
                        dr.HorasTrabajadas = "-";
                        dr.Observacion = "---";
                        ret.DetalleDia.Rows.Add(dr);
                    }
                }

                Reportes.HorarioIngresoEgresoMensual.AgenteRow ar = ret.Agente.NewAgenteRow();
                ar.Id = agente.Id;
                ar.Legajo = agente.Legajo.ToString();
                ar.Nombre = agente.ApellidoYNombre;
                ar.Area = agente.Area.Nombre;
                ar.Total_horas_mas = total_horas_mas;
                ar.Total_horas_menos = total_horas_menos;
                ar.Total_horas_trabajadas = total_horas_trabajadas;
                ar.Total_horas_tardanza = total_horas_tardanza;
                ar.Total_horas = HorasString.RestarHoras(total_horas_mas, total_horas_menos);
                ret.Agente.Rows.Add(ar);
            }

            Reportes.HorarioIngresoEgresoMensual.GeneralRow gr = ret.General.NewGeneralRow();
            gr.Anio = Calendario.SelectedDate.Year.ToString();
            gr.Mes = Calendario.SelectedDate.ToString("MMMM");

            ret.General.Rows.Add(gr);

            return ret;
        }

        private Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow ObtenerDetalle(Agente a, DateTime dia, Reportes.HorarioIngresoEgresoMensual ds)
        {
            Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ds.DetalleDia.NewDetalleDiaRow();

            using (var cxt = new Model1Container())
            {
                dr.Dia = dia.Day;
                dr.AgenteId = a.Id;
                dr.IngresoMan = "-";
                dr.EgresoMan = "-";
                dr.IngresoTar = "-";
                dr.EngresoTar = "-";
                dr.Tardanzas = "-";
                dr.HorasMas = "-";
                dr.HorasMenos = "-";
                dr.HorasTrabajadas = "-";
                dr.Observacion = "";

                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == dia);
                    if (feriado != null)
                    {
                        dr.Observacion = "FERIADO: " + feriado.Motivo;
                    }
                    else
                    {
                        EstadoAgente estado = a.EstadosPorDiaAgente.FirstOrDefault(e => e.Dia == dia);
                        if (estado != null)
                        {
                            dr.Observacion = estado.TipoEstado.Estado;
                        }
                        else
                        {
                            Franco franco = a.Francos.FirstOrDefault(f => f.DiasFranco.FirstOrDefault(d => d.Dia == dia) != null);
                            if (franco != null && franco.Estado == EstadosFrancos.Aprobado)
                            {
                                dr.Observacion = "FRANCO COMPENSATORIO";
                            }
                            else
                            {
                                string horas_trabajadas = "00:00";
                                string horas_tardanza = "00:00";
                                string horas_mas = "00:00";
                                string horas_menos = "00:00";

                                MovimientoHora tardanza = cxt.MovimientosHoras.FirstOrDefault(mh => mh.AgenteId == a.Id && mh.ResumenDiario.Dia == dia && mh.TipoMovimientoHoraId == 1);
                                if (tardanza != null)
                                {
                                    horas_tardanza = tardanza.Horas;
                                }

                                EntradaSalida Es = cxt.EntradasSalidas.FirstOrDefault(es => es.Agente.Id == a.Id && es.Fecha == dia);//e=> a.EntradasSalidasMarcadas.FirstOrDefault(es => es.Fecha == dia);
                                if (Es != null && DiaCerrado(dia))
                                {
                                    dr.IngresoMan = Es.Entrada;
                                    dr.EgresoMan = Es.Salida;
                                    horas_trabajadas = HorasString.SumarHoras(new string[] { horas_trabajadas, HorasString.RestarHoras(Es.Salida, Es.Entrada) });
                                }
                                else
                                {
                                    dr.IngresoMan = "-";
                                    dr.EgresoMan = "-";
                                }

                                HorarioVespertino hv = a.HorariosVespertinos.FirstOrDefault(h => h.Dia == dia);
                                if (hv != null && hv.Estado == EstadosHorarioVespertino.Terminado)
                                {
                                    dr.IngresoTar = hv.HoraInicio;
                                    dr.EngresoTar = hv.HoraFin;
                                    horas_trabajadas = HorasString.SumarHoras(new string[] { horas_trabajadas, hv.Horas });
                                }
                                else
                                {
                                    dr.IngresoTar = "-";
                                    dr.EngresoTar = "-";
                                }

                                string diferencia_horas = HorasString.RestarHoras(horas_trabajadas, "06:30");
                                if (diferencia_horas.Contains("-"))
                                {
                                    horas_mas = "00:00";
                                    horas_menos = diferencia_horas.Replace("-", "");
                                }
                                else
                                {
                                    horas_mas = diferencia_horas;
                                    horas_menos = "00:00";
                                }

                                dr.HorasMas = horas_mas;
                                dr.HorasMenos = horas_menos;
                                dr.HorasTrabajadas = horas_trabajadas;
                                dr.Tardanzas = horas_tardanza;
                            }
                        }
                    }
                }
                else
                {
                    dr.Observacion = dia.DayOfWeek == DayOfWeek.Saturday ? "SABADO" : "DOMINGO";
                }
            }

            return dr;
        }

        private void RenderReport()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/HorariosMensualesAgentesArea.rdlc");
            Session["DS"] = GenerarDS();
            Reportes.HorarioIngresoEgresoMensual ds = ((Reportes.HorarioIngresoEgresoMensual)Session["DS"]);
            ReportDataSource general = new ReportDataSource("ds_General", ds.General.Rows);
            ReportDataSource agentes = new ReportDataSource("ds_Agentes", ds.Agente.Rows);

            viewer.LocalReport.DataSources.Add(general);
            viewer.LocalReport.DataSources.Add(agentes);

            viewer.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

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

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.HorarioIngresoEgresoMensual ds = ((Reportes.HorarioIngresoEgresoMensual)Session["DS"]);
            e.DataSources.Add(new ReportDataSource("ds_Detalle", ds.DetalleDia.Rows));
        }

        protected void Calendario_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime d = e.Day.Date;
            Model1Container cxt = new Model1Container();
            Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == d);
            bool diaCerrado = DiaCerrado(d);

            if (feriado != null)
            {
                e.Cell.BackColor = Color.DarkRed;
                e.Cell.ForeColor = Color.Azure;
                e.Cell.Font.Bold = true;
                e.Cell.ToolTip = "Feriado: " + feriado.Motivo;
            }
            else
            {
                if (diaCerrado)
                {
                    e.Cell.BackColor = Color.DarkGray;
                    e.Cell.ForeColor = Color.Black;
                    e.Cell.Font.Italic = true;
                    e.Cell.ToolTip = "Día cerrado";
                }
            }

            if (e.Day.Date > DateTime.Today)
            {
                e.Cell.Enabled = false;
            }
        }

        protected void Calendario_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendario.SelectedDate = e.NewDate;
        }

        protected void btn_GuardarDeTodasFormas_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        protected void btn_CancelarGuardado_Click(object sender, EventArgs e)
        {
            div_Atencion.Visible = false;
        }
    }
}