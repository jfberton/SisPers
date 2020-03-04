using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class SU_Mantenimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                var mant = Request.QueryString["Mant"];
                Model1Container cxt = new Model1Container();
                VariableGlobal vg = cxt.VariablesGlobales.First();

                if (mant != null && mant == "0")
                {
                    //Poner la pagina en modo operativo
                    vg.EnMantenimiento = false;
                    EnMantenimiento.Visible = false;
                    Operativa.Visible = true;
                }

                if (mant != null && mant == "1")
                {
                    //Poner la pagina en modo mantenimiento
                    vg.EnMantenimiento = true;
                    EnMantenimiento.Visible = true;
                    Operativa.Visible = false;
                }

                cxt.SaveChanges();
            }
        }
    }
}