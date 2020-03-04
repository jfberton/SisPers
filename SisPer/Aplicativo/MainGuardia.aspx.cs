using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class MainGuardia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente agSession = Session["UsuarioLogueado"] as Agente;

                if (agSession == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }
            }

        }
    }
}