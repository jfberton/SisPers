using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo;

namespace SisPer
{
    public partial class Dispatcher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //throw new Exception("Maldicion");
            if (!IsPostBack)
            {
                Agente u = ((Agente)Session["UsuarioLogueado"]);
                bool cambiar_clave_al_inicio = Convert.ToBoolean(Session["Cambiar_clave_al_ingresar"]);

                if (!Convert.ToBoolean(Session["RedireccionarAPantallaPrincipal"]))
                {
                    Session["MostrarMensageBienvenida"] = true;
                }

                if (cambiar_clave_al_inicio)
                {
                    Session["AgentePantallaPropia"] = u.Usr;
                    Response.Redirect("~/Aplicativo/Usr_CambiarClave.aspx");
                }
                else
                {
                    switch (u.Perfil)
                    {
                        case PerfilUsuario.Ninguno:
                            Response.Redirect("~/Default.aspx?mode=trucho");
                            break;
                        case PerfilUsuario.Agente:
                            /* MODIFICADO POR PANDEMIA UNICAMENTE SE UTILIZA PARA GESTION DE  COMISIONES*/
                            //Response.Redirect("~/Aplicativo/MainPandemia.aspx");
                            if (u.Jefe == true || u.JefeTemporal)
                            {
                                ProcesosGlobales.VerificarYAgregarNotificacionJefe(u.Id);
                                Response.Redirect("~/Aplicativo/MainJefe.aspx");
                            }
                            else
                            {
                                Session["IdAg"] = u.Id.ToString();
                                Response.Redirect("~/Aplicativo/MainAgente.aspx");
                            }
                            

                            break;
                        case PerfilUsuario.Personal:

                            ProcesosGlobales.RegenerarBonificaciones();
                            if (DateTime.Today.Month == 7)
                            {
                                ProcesosGlobales.EliminarHorasAñoAnterior();
                            }

                            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
                            break;
                        case PerfilUsuario.Administrador:
                            //El perfil Administrador no tiene definido ninguna funcionalidad aún, lo rechazo.
                            Response.Redirect("~/Default.aspx?mode=trucho");
                            break;
                        case PerfilUsuario.Desarrollador:
                            Response.Redirect("~/Aplicativo/SU_Main.aspx");
                            break;
                        case PerfilUsuario.Guardia:
                            Response.Redirect("~/Aplicativo/MainGuardia.aspx");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}