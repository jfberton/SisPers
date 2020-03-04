using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Reportes
{
    public partial class ReportePDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                byte[] bytes = Session["Bytes"] as byte[];
                if (bytes != null)
                {
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
            }
        }
    }
}