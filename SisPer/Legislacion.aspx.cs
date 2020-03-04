using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo;

namespace SisPer
{
    public partial class Legislacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                if(usuarioLogueado==null)
                {
                    panelMenuSinIngresar.Visible = true;
                }
            }
        }
    }
}