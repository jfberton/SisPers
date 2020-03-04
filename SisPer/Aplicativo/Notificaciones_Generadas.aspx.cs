using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Notificaciones_Generadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    CargarGrillaNotificacionesPersonal();
                }
                else
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
            }
        }

        private void CargarGrillaNotificacionesPersonal()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            using (var cxt = new Model1Container())
            {


                var notificacionesGeneradas = (from n in cxt.Notificaciones
                                               where n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") == null
                                               select n).ToList();

                var itemsGrillaNotificacionesEnviadas = (from n in notificacionesGeneradas
                                                         select new
                                                         {
                                                             Id = n.Id,
                                                             Agente = n.Destinatario.ApellidoYNombre,
                                                             Descripcion = n.Descripcion.Length > 20 ? n.Descripcion.Substring(0, 20) : n.Descripcion,
                                                             Generada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Fecha.ToString("dd/MM/yyyy") : "",
                                                             Notificada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada").Fecha.ToString("dd/MM/yyyy") : ""
                                                         }).ToList();

                gv_NotificacionesEnviadas.DataSource = itemsGrillaNotificacionesEnviadas;
                gv_NotificacionesEnviadas.DataBind();
            }
        }

        protected void btn_VerNotificacionEnviada_Click(object sender, ImageClickEventArgs e)
        {
            int idNotificacion = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);

                //Mostrar Notificacion Seleccionada
                lbl_DeNotifEnviada.Text = not.Tipo.Tipo == "Automática" ? " - " : not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Agente.ApellidoYNombre : "";
                lbl_ParaNotifEnviada.Text = not.Destinatario.ApellidoYNombre;
                lbl_NumNotifEnviada.Text = not.Id.ToString();
                lbl_TipoNotifEnviada.Text = not.Tipo.Tipo;
                tb_descripcionEnviada.InnerText = not.Descripcion;

                p_DescripcionNotifEnviada.Visible = true;

                if (not.Vencimiento != null)
                {
                    div_vtoEnviado.Visible = true;
                    div_vtoEnviado.Attributes["class"] = "row alert alert-success";
                    lbl_fechaVtoEnviado.Text = not.Vencimiento.Value.ToString("dd/MM/yyyy");

                    if (not.Vencimiento.Value < DateTime.Today)
                    {
                        div_vtoEnviado.Attributes["class"] = "row alert alert-danger";
                        lbl_diasAVencerEnviado.Text = " - Notificación vencida.";
                    }
                }
                else
                {
                    div_vtoEnviado.Visible = false;
                }
            }
        }

        protected void btn_volver_Click(object sender, EventArgs e)
        {
            p_DescripcionNotifEnviada.Visible = false;
        }

        protected void gv_NotificacionesEnviadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_NotificacionesEnviadas.PageIndex = e.NewPageIndex;
            CargarGrillaNotificacionesPersonal();
        }

        protected void btn_Imprimir_Enviado_Click(object sender, EventArgs e)
        {
            int idNotificacion = Convert.ToInt32(lbl_NumNotifEnviada.Text);
            ImprimirNotificacionIndividual(idNotificacion);
        }

        private void ImprimirNotificacionIndividual(int idNotificacion)
        {
            using (var cxt = new Model1Container())
            {
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);

                Notificacion_ds ds = new Notificacion_ds();
                Notificacion_ds.NotificacionRow dr = ds.Notificacion.NewNotificacionRow();
                Notificacion_Historial generada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada");
                Notificacion_Historial notificada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada");
                Notificacion_Historial enviada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada");
                Notificacion_Historial observada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada");
                Notificacion_Historial recibida = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida");

                dr.Id = not.Id.ToString();
                dr.De = not.Tipo.Tipo == "Automática" ? "el sistema" : generada != null ? generada.Agente.ApellidoYNombre : "Sin generar";//no se puede dar nunca que no este generada pero por ahi pueden "crear" desde DB y olvidarse de generar historial.
                dr.Para = not.Destinatario.ApellidoYNombre;
                dr.Tipo = not.Tipo.Tipo;
                dr.FchGenerada = generada != null ? generada.Fecha.ToShortDateString() : "Sin generar";
                dr.FchNotificada = notificada != null ? notificada.Fecha.ToShortDateString() : "Sin notificar";
                dr.FchObservada = observada != null ? observada.Fecha.ToShortDateString() : "Sin observar";
                dr.FchRecibida = recibida != null ? recibida.Fecha.ToShortDateString() : "Sin recibir";
                dr.FchEnviada = enviada != null ? enviada.Fecha.ToShortDateString() : "Sin enviar";
                dr.Descripcion = not.Descripcion;
                dr.AgenteObservacion = observada != null ? observada.Agente.ApellidoYNombre : "Sin observar";
                dr.Observacion = observada != null ? not.ObservacionPendienteRecibir : "Sin observar";
                dr.Vencimiento = not.Vencimiento != null ? not.Vencimiento.Value.ToShortDateString() : "Sin vencimiento";

                ds.Notificacion.Rows.Add(dr);

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Notificacion_r.rdlc");

                ReportDataSource notificacion = new ReportDataSource("Notificacion", ds.Notificacion.Rows);

                viewer.LocalReport.DataSources.Add(notificacion);

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
        }

        protected void btn_refrescar_Click(object sender, EventArgs e)
        {
            CargarGrillaNotificacionesPersonal();
        }

        protected void btn_Imprimir_Listado_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                var notificacionesGeneradas = (from n in cxt.Notificaciones
                                               where n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") == null
                                               select n).ToList();
                
                Notificacion_ds ds = new Notificacion_ds();

                foreach (Notificacion not in notificacionesGeneradas)
                {
                    Notificacion_ds.NotificacionRow dr = ds.Notificacion.NewNotificacionRow();
                    
                    Notificacion_Historial generada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada");
                    Notificacion_Historial notificada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada");
                    Notificacion_Historial enviada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada");
                    Notificacion_Historial observada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada");
                    Notificacion_Historial recibida = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida");

                    dr.Id = not.Id.ToString();
                    dr.De = not.Tipo.Tipo == "Automática" ? " - " : generada != null ? generada.Agente.ApellidoYNombre : "Sin generar";//no se puede dar nunca que no este generada pero por ahi pueden "crear" desde DB y olvidarse de generar historial.
                    dr.Para = not.Destinatario.ApellidoYNombre;
                    dr.Tipo = not.Tipo.Tipo;
                    dr.FchGenerada = generada != null ? generada.Fecha.ToShortDateString() : "Sin generar";
                    dr.FchNotificada = notificada != null ? notificada.Fecha.ToShortDateString() : " - ";
                    dr.FchObservada = observada != null ? observada.Fecha.ToShortDateString() : "Sin observar";
                    dr.FchRecibida = recibida != null ? recibida.Fecha.ToShortDateString() : "Sin recibir";
                    dr.FchEnviada = enviada != null ? enviada.Fecha.ToShortDateString() : "Sin enviar";
                    dr.Descripcion = not.Descripcion;
                    dr.AgenteObservacion = observada != null ? observada.Agente.ApellidoYNombre : "Sin observar";
                    dr.Observacion = observada != null ? not.ObservacionPendienteRecibir : "Sin observar";
                    dr.Vencimiento = not.Vencimiento != null ? not.Vencimiento.Value.ToShortDateString() : "Sin vencimiento";

                    ds.Notificacion.Rows.Add(dr);
                }

                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Notificacion_Listado_r.rdlc");

                ReportDataSource notificacion = new ReportDataSource("Notificaciones", ds.Notificacion.Rows);

                viewer.LocalReport.DataSources.Add(notificacion);

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


        }
    }
}