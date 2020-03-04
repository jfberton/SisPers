using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Form1214_Nuevo_ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //unicamente para limpiar la variable de sesion
            Session["Form214"] = null;
            Response.Redirect("~/Aplicativo/Form1214_Nuevo.aspx");
        }
    }
}