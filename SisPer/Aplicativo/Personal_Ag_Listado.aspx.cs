using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Ag_Listado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
                    MenuPersonalJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                }

                Cargar_Grilla();
            }
        }

        #region grilla agentes

        private void Cargar_Grilla()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            using (var cxt = new Model1Container())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var items = cxt.sp_obtener_agentes_cascada(ag.Id, true).OrderBy(x => x.nivel_para_ordenar).OrderBy(x => x.nombre_agente).ToList();

                var t1 = sw.Elapsed;

                GridView1.DataSource = items;
                GridView1.DataBind();

                var t2 = sw.Elapsed;
            }
        }

        protected void btn_Administrar_Click(object sender, ImageClickEventArgs e)
        {
            int id_agente = 0;
            if (int.TryParse(((ImageButton)sender).CommandArgument, out id_agente))
            {
                administrar_datos_agente(id_agente);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.Cells[1].Text == "Jefe")
                {
                    e.Row.ForeColor = Color.Black;
                    e.Row.Font.Bold = true;
                }

                if (e.Row.Cells[1].Text == "Jefe temporal")
                {
                    e.Row.ForeColor = Color.Blue;
                }
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        #endregion

        #region administrar agente

        private void administrar_datos_agente(int id_agente)
        {
            using (var cxt = new Model1Container())
            {
                Agente agente = cxt.Agentes.FirstOrDefault(aa => aa.Id == id_agente);

                if (agente != null)
                {
                    lbl_modal_agente_Id.Text = agente.Id.ToString();
                    lbl_modal_titulo.Text = agente.ApellidoYNombre;
                    lbl_Legajo.Text = agente.Legajo.ToString();
                    lbl_Email.Text = agente.Legajo_datos_laborales.Email;
                    lbl_DNI.Text = agente.Legajo_datos_personales.DNI;
                    lbl_FechIngreso.Text = agente.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                    lbl_FechNac.Text = agente.Legajo_datos_personales.FechaNacimiento.ToShortDateString();
                    lbl_estado.Text = cxt.Estado_agente(agente.Id).First();
                    ImagenAgente1.Agente = agente;

                    if (!agente.Jefe && !agente.JefeTemporal)
                    {
                        btn_pantalla_jefe.Disabled = true;
                    }
                    else
                    {
                        btn_pantalla_jefe.Disabled = false;
                    }

                    if (agente.BonificacionesOtorgadas.Count > 0)
                    {
                        string horas = agente.BonificacionesOtorgadas.Last().HorasOtorgadas;
                        tb_horasABonificar.Text = horas;
                    }
                    else
                    {
                        tb_horasABonificar.Text = "005:00";
                    }

                    switch (lbl_estado.Text)
                    {
                        case "Sin datos":
                            modal_title.Attributes["class"] = "modal-content panel-danger";
                            break;
                        case "NATALICIO":
                            modal_title.Attributes["class"] = "modal-content panel-success";
                            break;
                        case "Activo":
                            modal_title.Attributes["class"] = "modal-content panel-success";
                            break;
                        case "Salida Particular":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Salida Oficial":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Indisposición":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Franco compensatorio":
                            modal_title.Attributes["class"] = "modal-content panel-primary";
                            break;
                        case "Franco compensatorio p/aprobar":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        default:
                            modal_title.Attributes["class"] = "modal-content panel-default";
                            break;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "admin_agente", "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#modal_agente').modal('show')});</script>", false);
                }
            }

        }

        protected void btn_AceptarBonificacion_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == id);

            BonificacionOtorgada bo = agCxt.BonificacionesOtorgadas.FirstOrDefault(b => b.Mes == DateTime.Today.Month && b.Anio == DateTime.Today.Year);

            if (bo != null)
            {
                MessageBox.Show(this.Page, "El agente ya posee bonificación otorgada este mes, espere al mes siguiente o consulte con Sistemas.", MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (Verificar())
                {
                    agCxt.PoseeBonificacion = true;
                    agCxt.HorasBonificacionACubrir = "-" + tb_horasABonificar.Text.Replace("-", "");
                    agCxt.BonificacionesOtorgadas.Add(new BonificacionOtorgada() { Anio = DateTime.Today.Year, Mes = DateTime.Today.Month, HorasOtorgadas = tb_horasABonificar.Text, HorasAdeudadas = tb_horasABonificar.Text });
                    cxt.SaveChanges();
                    Cargar_Grilla();
                }
                else
                {
                    MessageBox.Show(this, "Las horas ingresadas para bonificar son incorrectas", MessageBox.Tipo_MessageBox.Danger);
                }
            }
        }

        private bool Verificar()
        {
            string[] HoraMinuto = tb_horasABonificar.Text.Split(':');
            int hora, minuto = 0;
            return (Int32.TryParse(HoraMinuto[0], out hora) && hora > 0) || (Int32.TryParse(HoraMinuto[1], out minuto) && minuto > 0);
        }

        protected void btn_detalle_ServerClick(object sender, EventArgs e)
        {
            Session["IdAg"] = lbl_modal_agente_Id.Text;
            Response.Redirect("~/Aplicativo/Ag_Detalle.aspx");
        }

        protected void btn_pantalla_agente_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/MainAgente.aspx?Usr=" + lbl_modal_agente_Id.Text);
        }

        protected void btn_pantalla_jefe_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == id);

            if (agCxt.Jefe || agCxt.JefeTemporal)
            {
                Response.Redirect("~/Aplicativo/MainJefe.aspx?Usr=" + lbl_modal_agente_Id.Text);
            }
            else
            {
                MessageBox.Show(this.Page, "El agente no tiene perfil de jefe!", MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_solicitar_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Jefe_Ag_SolicitarEstado.aspx?Usr=" + lbl_modal_agente_Id.Text);
        }

        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        private string ObtenerDireccionImagenAgente(Agente u)
        {
            string pathImagen = string.Empty;

            if (Directory.Exists(pathImagenesDisco + u.Legajo))
            {
                if (File.Exists(pathImagenesDisco + u.Legajo + "\\Original.jpg"))
                {//Si no existe la imagen temporal pero si la original, la cargo
                    pathImagen = pathImagenesDisco + u.Legajo + "\\Original.jpg";
                }
                else
                {//Si no existe la imagen temporal y tampoco la original, cargo la default
                    pathImagen = pathImagenesDisco + "UsrDefault.jpg";
                }
            }
            else
            {//Si no existe la carpeta del usuario directamente cargo la imagen de default
                pathImagen = pathImagenesDisco + "UsrDefault.jpg";
            }

            return pathImagen;
        }

        private byte[] ImageToByte(string pathImage)
        {
            try
            {
                System.Drawing.Image imageIn = System.Drawing.Image.FromFile(pathImage);
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            catch
            {
                throw new Exception("CHANNNNNN");
            }
        }

        protected void btn_imprimir_carnet_ServerClick(object sender, EventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idAg = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Agente u = cxt.Agentes.FirstOrDefault(a => a.Id == idAg);

            ///Obtengo los datos
            Carnet_AgenteDS ds = new Carnet_AgenteDS();
            Carnet_AgenteDS.AgenteRow dr = ds.Agente.NewAgenteRow();
            dr.DNI = u.Legajo_datos_personales.DNI;
            dr.Nombre = u.ApellidoYNombre.Split(',')[1].Trim();
            dr.Apellido = u.ApellidoYNombre.Split(',')[0].Trim().ToUpper();
            dr.Foto = ImageToByte(ObtenerDireccionImagenAgente(u));
            ds.Agente.Rows.Add(dr);


            ///Configuro el reporte
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Carnet_Agente.rdlc");

            ReportDataSource general = new ReportDataSource("Agente", ds.Agente.Rows);

            viewer.LocalReport.DataSources.Add(general);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);

        }

        protected void btn_editar_ServerClick(object sender, EventArgs e)
        {
            Session["IdAg"] = lbl_modal_agente_Id.Text;
            Response.Redirect("~/Aplicativo/Personal_legajo.aspx");
        }

        protected void btn_eliminar_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == id);
            agCxt.FechaBaja = DateTime.Today;
            cxt.SaveChanges();
            Cargar_Grilla();
        }



        #endregion

        
    }
}