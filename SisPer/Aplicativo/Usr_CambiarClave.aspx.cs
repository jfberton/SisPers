using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Usr_CambiarClave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string usr = Session["AgentePantallaPropia"] as string;
                Session["AgentePantallaPropia"] = null;
                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;

                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
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

                Agente ag = cxt.Agentes.First(u => u.Usr == usr);
                Session["AgentePP"] = ag;
                mensaje.Visible = false;
            }
        }

        protected void btn_Cambiar_Click(object sender, EventArgs e)
        {
            mensaje.Visible = false;
            Page.Validate();
            if (Page.IsValid)
            {
                Agente ag = Session["AgentePP"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agentecxt = cxt.Agentes.First(a => a.Id == ag.Id);
                    agentecxt.Pass = Cripto.Encriptar(tb_Clave.Value);
                    cxt.SaveChanges();
                    mensaje.Visible = true;
                }
            }
        }

        protected void cv_ClavesCorrectas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_Clave.Value.Length >= 6;
        }

        protected void cv_ClavesIguales_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_Clave.Value == tb_ConfirmaClave.Value;
        }
    }
}