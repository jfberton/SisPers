﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Menues
{
    public partial class MenuAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = (Agente)Session["UsuarioLogueado"];
                CargarDatos(ag);
            }
        }

        private void CargarDatos(Agente agente)
        {
            if (agente != null)
            {
                var cxt = new Model1Container();

                lbl_ApyNom.Text = agente.ApellidoYNombre;
    
                ImagenAgente1.Agente = agente;

                int mensajesSinLeer = cxt.Destinatarios.Where(m => m.AgenteId == agente.Id && m.FechaLeido == null).Count();
                int notificacionesSinLeer = (from ne in cxt.Notificaciones
                                             where ne.AgenteId == agente.Id && ne.HistorialEstadosNotificacion.FirstOrDefault(e => e.Estado.Estado == "Notificada") == null
                                             select ne).Count();

                lbl_notificacionesNuevas.Text = notificacionesSinLeer > 0 ? notificacionesSinLeer.ToString() : "";
                notificaciones.Visible = notificacionesSinLeer > 0;

                lbl_mensajesNuevos.Text = mensajesSinLeer > 0 ? mensajesSinLeer.ToString() : "";
                mensajes.Visible = mensajesSinLeer > 0;

                int solicitudesDeAnticipo = 0;

                if (agente.Area.Nombre == "Administración")
                {
                    solicitudesDeAnticipo = cxt.Agentes1214.Count(aa => aa.Formulario1214.Estado == Estado1214.Aprobada
                                                                            && aa.Estado == EstadoAgente1214.Aprobado
                                                                            && aa.NroAnticipo == null);
                    lbl_solicitudes.Text = solicitudesDeAnticipo.ToString();
                    solicitudes.Visible = solicitudesDeAnticipo > 0;
                }
                else
                {
                    li_solicitudes.Attributes["Style"] = "display:none";
                    solicitudes.Visible = false;
                }

                int solicitudes_enviadas_sub = cxt.Formularios1214.Count(ff => ff.Estado == Estado1214.Enviado && agente.Area.Nombre == "Sub-Administración");
                solicitudes_subadministracion.Visible = solicitudes_enviadas_sub > 0;
                lbl_solicitudes_subadministracion.Text = solicitudes_enviadas_sub.ToString();

                int novedades3168Totales = solicitudesDeAnticipo + solicitudes_enviadas_sub;

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