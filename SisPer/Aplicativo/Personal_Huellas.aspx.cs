using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace SisPer.Aplicativo
{
    public partial class Personal_Huellas : System.Web.UI.Page
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
                    Calendar1.Text = DateTime.Today.ToLongDateString();
                    CargarDDLAnio();

                    MenuPersonalJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                    btn_GenTXT.Enabled = false;
                }
            }
        }

        private void CargarDDLAnio()
        {
            for (int i = 2013; i <= DateTime.Today.Year; i++)
            {
                ddl_Anio.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddl_Anio.SelectedIndex = 0;
            ddl_Mes.SelectedIndex = 0;
        }

        private void CargarMarcacionesFecha()
        {

            int legajo = 0;
            int.TryParse(tb_Legajo.Value, out legajo);
            DS_Marcaciones ds = new DS_Marcaciones();
            DateTime fechaSeleccionada = Convert.ToDateTime(Calendar1.Text);

            lbl_tituloGrilla.Text = "Marcaciones del " + fechaSeleccionada.ToShortDateString();

            if (legajo != 0)
            {
                using (var cxt = new Model1Container())
                {
                    Agente ag = cxt.Agentes.FirstOrDefault(aagg => aagg.Legajo == legajo && aagg.FechaBaja == null);
                    cxt.sp_obtener_resumen_diario_agente_fecha(ag.Id, fechaSeleccionada.ToShortDateString()).First();
                }
                    
                ds = ProcesosGlobales.ObtenerMarcaciones(fechaSeleccionada, legajo.ToString());
                lbl_tituloGrilla.Text += " - Legajo " + legajo.ToString();
            }
            else
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(fechaSeleccionada);
            }

            var marcaciones = (from h in ds.Marcacion
                               select new
                               {
                                   h.Legajo,
                                   h.Fecha,
                                   h.Hora
                               }
                                   ).ToList();

            GridView1.DataSource = marcaciones;
            GridView1.DataBind();

            if (GridView1.Rows.Count > 0)
            {
                btn_GenTXT.Enabled = true;
            }
        }

        private void CargarMarcacionesMes()
        {
            int mesSeleccionado = Convert.ToInt32(ddl_Mes.SelectedValue);
            int anioSeleccionado = Convert.ToInt32(ddl_Anio.SelectedValue);

             

            int legajo = 0;
            int.TryParse(tb_Legajo.Value, out legajo);
            DS_Marcaciones ds = new DS_Marcaciones();
            DateTime desde = new DateTime(anioSeleccionado, mesSeleccionado, 1);
            DateTime hasta = new DateTime(anioSeleccionado, mesSeleccionado, DateTime.DaysInMonth(anioSeleccionado, mesSeleccionado));

            lbl_tituloGrilla.Text = "Marcaciones del mes de " + ddl_Mes.SelectedItem.Text + " de " + desde.Year.ToString();

            if (legajo != 0)
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(desde, hasta, legajo.ToString());
                lbl_tituloGrilla.Text += " - Legajo " + legajo.ToString();
            }
            else
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(desde, hasta);
            }


            var marcaciones = (from h in ds.Marcacion
                               select new
                               {
                                   h.Legajo,
                                   h.Fecha,
                                   h.Hora
                               }
                                   ).ToList();

            GridView1.DataSource = marcaciones;
            GridView1.DataBind();

            if (GridView1.Rows.Count > 0)
            {
                btn_GenTXT.Enabled = true;
            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            if (rb_Diadio.Checked)
            {
                if(Convert.ToDateTime(Calendar1.Text) > DateTime.Today)
                {
                    Calendar1.Text = DateTime.Today.ToLongDateString();
                    Controles.MessageBox.Show(this, "No puede cargar una fecha posterior a la actual!", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
                else
                { 
                    CargarMarcacionesFecha();
                }
            }
            else
            {
                CargarMarcacionesMes();
            }
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            if (rb_Diadio.Checked)
            {
                CargarMarcacionesFecha();
            }
            else
            {
                CargarMarcacionesMes();
            }
        }

        protected void btn_GenTXT_Click(object sender, EventArgs e)
        {
            if (rb_Diadio.Checked)
            {
                GenerarDiario();
            }
            else
            {
                GenerarMensual();
            }
        }

        private void GenerarMensual()
        {
            StringWriter sr = new StringWriter();
            string FilePath = Server.MapPath("~/Temp/");

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            string FileName = Convert.ToDateTime(Calendar1.Text).Year.ToString() + Convert.ToDateTime(Calendar1.Text).Month.ToString() + Convert.ToDateTime(Calendar1.Text).Day.ToString() + ".CRO";

            int mesSeleccionado = Convert.ToInt32(ddl_Mes.SelectedValue);
            int anioSeleccionado = Convert.ToInt32(ddl_Anio.SelectedValue);

            int legajo = 0;
            int.TryParse(tb_Legajo.Value, out legajo);
            DS_Marcaciones ds = new DS_Marcaciones();
            DateTime desde = new DateTime(anioSeleccionado, mesSeleccionado, 1);
            DateTime hasta = new DateTime(anioSeleccionado, mesSeleccionado, DateTime.DaysInMonth(anioSeleccionado, mesSeleccionado));

            if (legajo != 0)
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(desde, hasta, legajo.ToString());
            }
            else
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(desde, hasta);
            }


            var marcaciones = (from h in ds.Marcacion
                               select new
                               {
                                   h.Legajo,
                                   h.Fecha,
                                   h.Hora
                               }
                                   ).ToList();

            foreach (var marcacion in marcaciones)
            {
                sr.WriteLine(marcacion.Legajo.ToString().PadLeft(5, '0') + " " + marcacion.Fecha.ToShortDateString() + " " + marcacion.Hora + " 01 20");
            }



            // Creates the file on server
            File.WriteAllText(FilePath + FileName, sr.ToString());

            // Prompts user to save file
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(FilePath + FileName);
            response.Flush();

            // Deletes the file on server
            File.Delete(FilePath + FileName);

            response.End();
        }

        private void GenerarDiario()
        {
            StringWriter sr = new StringWriter();
            string FilePath = Server.MapPath("~/Temp/");

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            string FileName = Convert.ToDateTime(Calendar1.Text).Year.ToString() + Convert.ToDateTime(Calendar1.Text).Month.ToString() + Convert.ToDateTime(Calendar1.Text).Day.ToString() + ".CRO";

            int legajo = 0;
            int.TryParse(tb_Legajo.Value, out legajo);
            DS_Marcaciones ds = new DS_Marcaciones();
            DateTime fechaSeleccionada = Convert.ToDateTime(Calendar1.Text);

            if (legajo != 0)
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(fechaSeleccionada, legajo.ToString());
            }
            else
            {
                ds = ProcesosGlobales.ObtenerMarcaciones(fechaSeleccionada);
            }

            var marcaciones = (from h in ds.Marcacion
                               select new
                               {
                                   h.Legajo,
                                   h.Fecha,
                                   h.Hora
                               }
                                   ).ToList();

            foreach (var marcacion in marcaciones)
            {
                sr.WriteLine(marcacion.Legajo.ToString().PadLeft(5, '0') + " " + marcacion.Fecha.ToShortDateString() + " " + marcacion.Hora + " 01 20");
            }


            // Creates the file on server
            File.WriteAllText(FilePath + FileName, sr.ToString());

            // Prompts user to save file
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(FilePath + FileName);
            response.Flush();

            // Deletes the file on server
            File.Delete(FilePath + FileName);

            response.End();
        }

        protected void rb_Diadio_CheckedChanged(object sender, EventArgs e)
        {
            PanelDiario.Visible = rb_Diadio.Checked;
            PanelMensual.Visible = rb_Mensual.Checked;
        }

        protected void btn_Procesar_Click(object sender, EventArgs e)
        {
            bool proceso_Correctamente = Procesar();

            if (proceso_Correctamente)
            {
                Model1Container cxt = new Model1Container();
                DiaProcesado dp = new DiaProcesado();
                dp.Dia = Convert.ToDateTime(Calendar1.Text);
                dp.Cerrado = false;
                if (cxt.DiasProcesados.Where(d => d.Dia == dp.Dia).Count() == 0)
                {
                    cxt.DiasProcesados.AddObject(dp);
                    cxt.SaveChanges();
                }
                Response.Redirect("~/Aplicativo/Personal_Marcaciones_Procesar.aspx?d=" + dp.Dia.ToString("dd") + "&m=" + dp.Dia.ToString("MM") + "&a=" + dp.Dia.ToString("yyyy"));
            }

        }

        private bool Procesar()
        {
            bool procesoCorrectamente = true;
            try
            {
                using (Model1Container cxt = new Model1Container())
                {
                    foreach (Agente ag in cxt.Agentes.Where(a => a.FechaBaja == null))
                    {
                        ResumenDiario rdAgente = ag.ObtenerResumenDiario(Convert.ToDateTime(Calendar1.Text));
                        ResumenDiario rd = rdAgente != null ? cxt.ResumenesDiarios.First(r => r.Id == rdAgente.Id) : null;
                        DS_Marcaciones marcaciones = new DS_Marcaciones();

                        if (rd != null && (rd.Cerrado ?? false) == false)
                        {
                            marcaciones = ProcesosGlobales.ObtenerMarcaciones(Convert.ToDateTime(Calendar1.Text), ag.Legajo.ToString());
                            foreach (DS_Marcaciones.MarcacionRow item in marcaciones.Marcacion.Rows)
                            {
                                Marcacion marca = rd.Marcaciones.FirstOrDefault(m => m.Hora == item.Hora);
                                if (marca == null)
                                {
                                    //no posee marca, entonces la agrego
                                    rd.Marcaciones.Add(new Marcacion()
                                    {
                                        Hora = item.Hora,
                                        Manual = item.MarcaManual,
                                        Anulada = false,
                                    });
                                }
                            }
                        }
                        else
                        {
                            if (rd == null)
                            {
                                marcaciones = ProcesosGlobales.ObtenerMarcaciones(Convert.ToDateTime(Calendar1.Text), ag.Legajo.ToString());
                                //el agente no tiene resumen diario debo crear uno nuevo 
                                rd = new ResumenDiario();
                                ///////////////
                                rd.Dia = Convert.ToDateTime(Calendar1.Text);
                                rd.HEntrada = "000:00";
                                rd.HSalida = "000:00";
                                rd.HVEnt = "000:00";
                                rd.HVSal = "000:00";
                                rd.Horas = "000:00";
                                rd.AgenteId = ag.Id;
                                rd.Inconsistente = false;
                                rd.MarcoTardanza = false;
                                rd.MarcoProlongJornada = false;
                                rd.ObservacionInconsistente = "";

                                cxt.ResumenesDiarios.AddObject(rd);

                                if (marcaciones.Marcacion.Rows.Count > 0)
                                {//agrego las marcaciones del dia al resumen diario
                                    foreach (DS_Marcaciones.MarcacionRow marca in marcaciones.Marcacion.Rows)
                                    {
                                        rd.Marcaciones.Add(new Marcacion()
                                        {
                                            Hora = marca.Hora,
                                            Manual = marca.MarcaManual,
                                            Anulada = false,
                                        });
                                    }
                                }
                                else
                                {
                                    rd.Inconsistente = true;
                                    rd.ObservacionInconsistente = "El agente no tiene marcaciones este día";
                                }
                            }
                        }
                    }
                    cxt.SaveChanges();
                }
            }
            catch
            {
                procesoCorrectamente = false;
            }

            return procesoCorrectamente;
        }

        //el día ya esta procesado y cerrado. 
        protected void btn_Ver_Proceso_Click(object sender, EventArgs e)
        {
            DateTime dp = Convert.ToDateTime(Calendar1.Text);
            Response.Redirect("~/Aplicativo/Personal_Marcaciones_Procesar.aspx?d=" + dp.ToString("dd") + "&m=" + dp.ToString("MM") + "&a=" + dp.ToString("yyyy"));
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime d = e.Day.Date;
            Model1Container cxt = new Model1Container();
            DiaProcesado dp = cxt.DiasProcesados.FirstOrDefault(dia => dia.Dia == d);
            Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == d);
            if (dp != null)
            {
                if (dp.Cerrado)
                {
                    e.Cell.BackColor = Color.DarkGreen;
                    e.Cell.ForeColor = Color.Azure;
                    e.Cell.Font.Bold = true;
                    e.Cell.ToolTip = "Procesado y cerrado";
                }
                else
                {
                    e.Cell.BackColor = Color.DarkGoldenrod;
                    e.Cell.ForeColor = ColorTranslator.FromHtml("#333333");
                    e.Cell.Font.Bold = true;
                    e.Cell.ToolTip = "Procesado";
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