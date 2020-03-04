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
    public partial class Notificaciones_Inbox : System.Web.UI.Page
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
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    CargarGrillaNotificacionesAgente();
                }
            }
        }

        private void CargarGrillaNotificacionesPersonal()
        {
            CargarGrillaNotificacionesAgente();

            p_personal.Visible = true;
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            using (var cxt = new Model1Container())
            {


                var notificacionesGeneradas = (from n in cxt.Notificaciones
                                               where n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") != null
                                               select n).ToList();

                var itemsGrillaNotificacionesEnviadas = (from n in notificacionesGeneradas
                                                         select new
                                              {
                                                  Id = n.Id,
                                                  Agente = n.Destinatario.ApellidoYNombre,
                                                  Descripcion = n.Descripcion.Length > 20 ? n.Descripcion.Substring(0, 20) : n.Descripcion,
                                                  Generada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Fecha.ToString("dd/MM/yyyy") : "",
                                                  Notificada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada").Fecha.ToString("dd/MM/yyyy") : "",
                                                  Enviada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada").Fecha.ToString("dd/MM/yyyy") : "",
                                                  Recibida = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida").Fecha.ToString("dd/MM/yyyy") : "",
                                                  PathImagenObservada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada") != null ? "~/Imagenes/WarningHeader.gif" : "~/Imagenes/invisible.png",
                                                  Observaciones = n.ObservacionPendienteRecibir
                                              }).ToList();

                gv_NotificacionesEnviadas.DataSource = itemsGrillaNotificacionesEnviadas;
                gv_NotificacionesEnviadas.DataBind();
            }
        }

        private void CargarGrillaNotificacionesAgente()
        {
            p_agente.Visible = true;
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            using (var cxt = new Model1Container())
            {
                var notificacionesRecibidas = (from n in cxt.Notificaciones
                                               where n.AgenteId == usuarioLogueado.Id
                                               select n).ToList();

                var itemsGrillaNotificacionesRecibidasPersonal = (from n in notificacionesRecibidas
                                                                  select new
                                                                  {
                                                                      Id = n.Id,
                                                                      Descripcion = n.Descripcion.Length > 20 ? n.Descripcion.Substring(0, 20) : n.Descripcion,
                                                                      Generada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Fecha.ToString("dd/MM/yyyy") : "",
                                                                      Notificada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada").Fecha.ToString("dd/MM/yyyy") : "",
                                                                      Enviada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada").Fecha.ToString("dd/MM/yyyy") : "",
                                                                      Recibida = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida") != null ? n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida").Fecha.ToString("dd/MM/yyyy") : "",
                                                                      PathImagenObservada = n.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada") != null ? "~/Imagenes/WarningHeader.gif" : "~/Imagenes/invisible.png",
                                                                      Observaciones = n.ObservacionPendienteRecibir
                                                                  }).ToList();

                gv_Notificaciones.DataSource = itemsGrillaNotificacionesRecibidasPersonal;
                gv_Notificaciones.DataBind();
            }
        }

        protected void gv_Notificaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Notificaciones.PageIndex = e.NewPageIndex;
            CargarGrillaNotificacionesAgente();
        }

        protected void btn_VerNotificacionAgente_Click(object sender, ImageClickEventArgs e)
        {
            int idNotificacion = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            p_DescripcionNotifEnviada.Visible = false;
            using (var cxt = new Model1Container())
            {
                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == agenteLogueado.Id);
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);
                if (not != null && not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Notificada") == null)
                {
                    Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Notificada");
                    if (ne == null)
                    {
                        ne = new Notificacion_Estado() { Estado = "Notificada" };
                        cxt.Notificacion_Estados.AddObject(ne);
                    }

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = agCxt,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = not
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);
                    cxt.SaveChanges();
                }

                //Mostrar Notificacion Seleccionada
                bool enviada = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") != null;
                btn_EnviarNotifAg.Visible = !enviada;
                lbl_DeNotifAg.Text = not.Tipo.Tipo == "Automática" ? " - " : not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Agente.ApellidoYNombre : "";
                lbl_NumNotifAg.Text = not.Id.ToString();
                lbl_TipoNotifAg.Text = not.Tipo.Tipo;

                if (not.Vencimiento != null)
                {
                    div_vto.Visible = true;
                    div_vto.Attributes["class"] = "row alert alert-success";
                    lbl_vto.Text = not.Vencimiento.Value.ToString("dd/MM/yyyy");
                    double dias_antes_del_vencimiento = (not.Vencimiento.Value - DateTime.Today).TotalDays;
                    if (enviada)
                    {
                        div_vto.Attributes["class"] = "row alert alert-success";
                        lbl_diasalvto.Text = " envio realizado.-";
                    }
                    else
                    {

                        if (dias_antes_del_vencimiento > 0)
                        {
                            if (dias_antes_del_vencimiento < 5)
                            {
                                div_vto.Attributes["class"] = "row alert alert-warning";
                            }

                            lbl_diasalvto.Text = "quedan " + dias_antes_del_vencimiento.ToString() + " días";
                        }
                        else
                        {
                            div_vto.Attributes["class"] = "row alert alert-danger";
                            lbl_diasalvto.Text = " - VENCIDO - ";
                        }
                    }
                }
                else
                {
                    div_vto.Visible = false;
                }

                tb_descripcion.InnerText = not.Descripcion;

                tb_observada.InnerHtml = not.ObservacionPendienteRecibir.Length > 0 ? not.ObservacionPendienteRecibir : " - No posee observaciones.";
                p_DescripcionNotifAg.Visible = true;
            }

            if (MenuAgente.Visible) { MenuAgente.ActualizarNotificacionesMensajes(); }
            if (MenuJefe.Visible) { MenuJefe.ActualizarNotificacionesMensajes(); }
            if (MenuPersonalAgente.Visible) { MenuPersonalAgente.ActualizarNotificacionesMensajes(); }
            if (MenuPersonalJefe.Visible) { MenuPersonalJefe.ActualizarNotificacionesMensajes(); }
        }

        protected void btn_CancelarNotifAg_Click(object sender, EventArgs e)
        {
            lbl_DeNotifAg.Text = string.Empty;
            lbl_NumNotifAg.Text = string.Empty;
            lbl_TipoNotifAg.Text = string.Empty;
            tb_descripcion.InnerText = string.Empty;
            p_DescripcionNotifAg.Visible = false;

            CargarGrillaNotificacionesAgente();
        }

        protected void btn_EnviarNotifAg_Click(object sender, EventArgs e)
        {
            int idNofificacion = Convert.ToInt32(lbl_NumNotifAg.Text);

            using (var cxt = new Model1Container())
            {
                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == agenteLogueado.Id);
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNofificacion);
                if (not != null && not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada") == null)
                {
                    Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Enviada");
                    if (ne == null)
                    {
                        ne = new Notificacion_Estado() { Estado = "Enviada" };
                        cxt.Notificacion_Estados.AddObject(ne);
                    }

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = agCxt,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = not
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);
                    cxt.SaveChanges();
                }

                lbl_DeNotifAg.Text = string.Empty;
                lbl_NumNotifAg.Text = string.Empty;
                lbl_TipoNotifAg.Text = string.Empty;
                tb_descripcion.InnerText = string.Empty;
                p_DescripcionNotifAg.Visible = false;

                CargarGrillaNotificacionesAgente();
            }
        }

        protected void btn_VerNotificacionEnviada_Click(object sender, ImageClickEventArgs e)
        {
            p_DescripcionNotifAg.Visible = false;
            int idNotificacion = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);

                //Mostrar Notificacion Seleccionada
                lbl_DeNotifEnviada.Text = not.Tipo.Tipo == "Automática" ? " - " : not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada") != null ? not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Generada").Agente.ApellidoYNombre : "";
                lbl_ParaNotifEnviada.Text = not.Destinatario.ApellidoYNombre;
                lbl_NumNotifEnviada.Text = not.Id.ToString();
                lbl_TipoNotifEnviada.Text = not.Tipo.Tipo;
                tb_observaciones.InnerHtml = not.ObservacionPendienteRecibir;
                tb_observaciones.Disabled = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada") != null;
                tb_descripcionEnviada.InnerText = not.Descripcion;

                p_DescripcionNotifEnviada.Visible = true;

                if (not.Vencimiento != null)
                {
                    div_vtoEnviado.Visible = true;
                    div_vtoEnviado.Attributes["class"] = "row alert alert-success";
                    lbl_fechaVtoEnviado.Text = not.Vencimiento.Value.ToString("dd/MM/yyyy");
                    Notificacion_Historial nh = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Enviada");
                    if (nh != null)
                    {
                        DateTime fechaenvio = not.HistorialEstadosNotificacion.First(he => he.Estado.Estado == "Enviada").Fecha;

                        double enviado_dias_Antes_Vto = (not.Vencimiento.Value - fechaenvio.Date).TotalDays;
                        if (enviado_dias_Antes_Vto < 0)
                        {
                            div_vtoEnviado.Attributes["class"] = "row alert alert-danger";
                            lbl_diasAVencerEnviado.Text = " - El envío fue posterior al vencimiento.";
                        }
                        else
                        {
                            lbl_diasAVencerEnviado.Text = " - Enviado a tiempo.";
                        }
                    }
                    else
                    {//No deberia entrar aca ya que aquí solamente muestro las enviadas
                        if (not.Vencimiento.Value < DateTime.Today)
                        {
                            div_vtoEnviado.Attributes["class"] = "row alert alert-danger";
                            lbl_diasAVencerEnviado.Text = " - Notificación vencida.";
                        }
                    }
                }
                else
                {
                    div_vtoEnviado.Visible = false;
                }

                btn_Recibir.Visible = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida") == null;
                btn_PosponerRecepcion.Visible = not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada") == null && not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida") == null;


            }
        }

        protected void gv_NotificacionesEnviadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_NotificacionesEnviadas.PageIndex = e.NewPageIndex;
            CargarGrillaNotificacionesPersonal();
        }

        protected void btn_Recibir_Click(object sender, EventArgs e)
        {
            int idNotificacion = Convert.ToInt32(lbl_NumNotifEnviada.Text);

            using (var cxt = new Model1Container())
            {
                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == agenteLogueado.Id);
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);
                if (not != null && not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Recibida") == null)
                {
                    Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Recibida");
                    if (ne == null)
                    {
                        ne = new Notificacion_Estado() { Estado = "Recibida" };
                        cxt.Notificacion_Estados.AddObject(ne);
                    }

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = agCxt,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = not
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);
                    cxt.SaveChanges();
                }

                btn_Recibir.Visible = false;
                btn_PosponerRecepcion.Visible = false;
            }

            CargarGrillaNotificacionesPersonal();
        }

        protected void btn_PosponerRecepcion_Click(object sender, EventArgs e)
        {
            int idNotificacion = Convert.ToInt32(lbl_NumNotifEnviada.Text);

            using (var cxt = new Model1Container())
            {
                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == agenteLogueado.Id);
                Notificacion not = cxt.Notificaciones.FirstOrDefault(n => n.Id == idNotificacion);
                if (not != null && not.HistorialEstadosNotificacion.FirstOrDefault(he => he.Estado.Estado == "Observada") == null)
                {
                    Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Observada");
                    if (ne == null)
                    {
                        ne = new Notificacion_Estado() { Estado = "Observada" };
                        cxt.Notificacion_Estados.AddObject(ne);
                    }

                    not.ObservacionPendienteRecibir = tb_observaciones.InnerText;

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = agCxt,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = not
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);
                    cxt.SaveChanges();
                }

                btn_Recibir.Visible = false;
                btn_PosponerRecepcion.Visible = false;
            }

            CargarGrillaNotificacionesPersonal();
        }

        protected void btn_volver_Click(object sender, EventArgs e)
        {
            p_DescripcionNotifEnviada.Visible = false;

            CargarGrillaNotificacionesPersonal();
        }

        

        protected void btn_ImprimirNotifAg_Click(object sender, EventArgs e)
        {
            int idNotificacion = Convert.ToInt32(lbl_NumNotifAg.Text);
            ImprimirNotificacionIndividual(idNotificacion);
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
    }
}