using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class SU_GenerarNotificaciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btn_generar_notificaciones_pendientes_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                var solicitudes = cxt.SolicitudesDeEstado.Where(ssee => ssee.Estado == EstadoSolicitudDeEstado.Solicitado);

                Notificacion_Tipo nt = cxt.Notificacion_Tipos.FirstOrDefault(nntt => nntt.Tipo == "Automática");
                if (nt == null)
                {
                    nt = new Notificacion_Tipo() { Tipo = "Automática" };
                    cxt.Notificacion_Tipos.AddObject(nt);
                }

                Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Generada");
                if (ne == null)
                {
                    ne = new Notificacion_Estado() { Estado = "Generada" };
                    cxt.Notificacion_Estados.AddObject(ne);
                }

                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Legajo == 101);

                foreach (SolicitudDeEstado solicitud in solicitudes)
                {
                    Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == solicitud.Agente.Id);
                    Notificacion notificacion = new Notificacion();

                    notificacion.Descripcion = "Presentar documentación respaldatoria de la solicitud \"" + solicitud.TipoEstadoAgente.Estado + "\" para el/los día/s " + solicitud.FechaDesde.ToString("dd/MM/yyyy") + " al " + solicitud.FechaHasta.ToString("dd/MM/yyyy");
                    notificacion.Destinatario = destinatarioCxt;
                    notificacion.ObservacionPendienteRecibir = string.Empty;

                    notificacion.Tipo = nt;
                    cxt.Notificaciones.AddObject(notificacion);

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = agCxt,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = notificacion
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);
                }

                cxt.SaveChanges();

                Controles.MessageBox.Show(this,"Listo!", Controles.MessageBox.Tipo_MessageBox.Success, "Generación terminada");
            }
        }
        private void MessageBox(string message)
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">alert('" + message + "');</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", script, false);
        }
    }
}