using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class MainPandemia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente agSession = Session["UsuarioLogueado"] as Agente;

                if (agSession == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                int id = 0;
                string idUsr = Request.QueryString["Usr"];
                if (idUsr != null && idUsr != string.Empty)
                {
                    id = Convert.ToInt32(idUsr);
                }
                else
                {
                    id = agSession.Id;
                }

                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                Session["Agente"] = ag;
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

                DatosAgente1.Agente = ag;

                bool mostrarMensaje = Convert.ToBoolean(Session["MostrarMensageBienvenida"]);
                Session["MostrarMensageBienvenida"] = false;
                if (mostrarMensaje)
                {
                    MensageBienvenida.Show();
                }
            }
        }
    }
}