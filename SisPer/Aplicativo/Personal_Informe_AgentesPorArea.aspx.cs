using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Informe_AgentesPorArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

            if (usuariologueado == null)
            {
                Response.Redirect("~/Default.aspx?mode=session_end");
            }

            if (usuariologueado.Perfil != PerfilUsuario.Personal)
            {
                Response.Redirect("../default.aspx?mode=trucho");
            }
            else
            {
                MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
            }
        }
    }
}