using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Menues
{
    public partial class MenuJefe : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = (Agente)Session["UsuarioLogueado"];
                CargarDatos(ag);
            }
        }

        public void CargarDatos(Agente agente)
        {
            if (agente != null)
            {
                lbl_ApyNom.Text = agente.ApellidoYNombre;

                ImagenAgente1.Agente = agente;

                var cxt = new Model1Container();

                int mensajesSinLeer = cxt.Destinatarios.Where(m => m.AgenteId == agente.Id && m.FechaLeido == null).Count();
                int notificacionesSinLeer = (from ne in cxt.Notificaciones
                                             where ne.AgenteId == agente.Id && ne.HistorialEstadosNotificacion.FirstOrDefault(e => e.Estado.Estado == "Notificada") == null
                                             select ne).Count();

                lbl_notificacionesNuevas.Text = notificacionesSinLeer > 0 ? notificacionesSinLeer.ToString() : "";
                notificaciones.Visible = notificacionesSinLeer > 0;

                lbl_mensajesNuevos.Text = mensajesSinLeer > 0 ? mensajesSinLeer.ToString() : "";
                mensajes.Visible = mensajesSinLeer > 0;

                int solicitudes214 = cxt.Agentes1214.Count(aa => aa.Estado == EstadoAgente1214.Solicitado && 
                                (
                                agente.AreaId == aa.Id_Area //depende directamente
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.AreaId == agente.AreaId) //depende en segunda instancia
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.AreaId == agente.AreaId) //depende en tercera instancia
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe.AreaId == agente.AreaId) //depende en cuarta instancia
                                ));
                int solicitudesDeAnticipo = cxt.Agentes1214.Count(aa => agente.Area.Nombre == "Administración"
                                                                        && aa.Formulario1214.Estado == Estado1214.Aprobada
                                                                        && aa.Estado == EstadoAgente1214.Aprobado
                                                                        && aa.NroAnticipo == null);
                int solicitudesTotales = solicitudes214 + solicitudesDeAnticipo;
                lbl_solicitudes.Text = solicitudesTotales.ToString();
                solicitudes.Visible = solicitudesTotales > 0;

                int solicitudes_enviadas_sub = cxt.Formularios1214.Count(ff => ff.Estado == Estado1214.Enviado && agente.Area.Nombre == "Sub-Administración");
                solicitudes_subadministracion.Visible = solicitudes_enviadas_sub > 0;
                lbl_solicitudes_subadministracion.Text = solicitudes_enviadas_sub.ToString();

                int novedades3168Totales = solicitudesTotales + solicitudes_enviadas_sub;
                
                lbl_novedades214.Text = novedades3168Totales.ToString();
                novedades214.Visible = novedades3168Totales > 0;
            }
        }

        public void ActualizarNotificacionesMensajes()
        {
            using (var cxt = new Model1Container())
            {
                Agente ag = (Agente)Session["UsuarioLogueado"];

                int mensajesSinLeer = cxt.Destinatarios.Where(m => m.AgenteId == ag.Id && m.FechaLeido == null).Count();
                int notificacionesSinLeer = (from ne in cxt.Notificaciones
                                             where ne.AgenteId == ag.Id && ne.HistorialEstadosNotificacion.FirstOrDefault(e => e.Estado.Estado == "Notificada") == null
                                             select ne).Count();

                lbl_notificacionesNuevas.Text = notificacionesSinLeer > 0 ? notificacionesSinLeer.ToString() : "";
                notificaciones.Visible = notificacionesSinLeer > 0;

                lbl_mensajesNuevos.Text = mensajesSinLeer > 0 ? mensajesSinLeer.ToString() : "";
                mensajes.Visible = mensajesSinLeer > 0;
            }
        }

        protected void lbl_Editar_Click(object sender, EventArgs e)
        {
            Agente ag = (Agente)Session["UsuarioLogueado"];
            Session["AgentePantallaPropia"] = ag.Usr;
            Response.Redirect("~/Aplicativo/Usr_PantallaPropia.aspx");
        }

        protected void lbl_logout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }

        protected void lbl_CambiarClave_Click(object sender, EventArgs e)
        {
            Agente ag = (Agente)Session["UsuarioLogueado"];
            Session["AgentePantallaPropia"] = ag.Usr;
            Response.Redirect("~/Aplicativo/Usr_CambiarClave.aspx");
        }

        protected void lbl_datos_legajo_Click(object sender, EventArgs e)
        {
            Session["VerLegajo"] = "algo";
            Response.Redirect("~/Aplicativo/Personal_legajo.aspx");
        }

    }
}