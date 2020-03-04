using SisPer.Aplicativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                /*
                 sw.WriteLine("\nDATE: " + System.DateTime.Now);
                     sb.Append("DATE=" + System.DateTime.Now);

                     if (ctx.User.Identity.IsAuthenticated)
                     {
                         sw.WriteLine("\nAGENTE: " + ctx.User.Identity.Name.Replace("Bienvenido ", "").Replace("|", ""));
                         sb.Append("&AGENTE=" + ctx.User.Identity.Name.Replace("Bienvenido ", "").Replace("|", ""));
                     }
                     else
                     {
                         sb.Append("&AGENTE=SinAutenticar");
                     }

                     sw.WriteLine("\nMESSAGE: " + ex.Message);
                     sb.Append("&MESSAGE=" + ex.Message);

                     sw.WriteLine("\nSOURCE: " + ex.Source);
                     sb.Append("&SOURCE=" + ex.Source);
                     sw.WriteLine("\nINSTANCE: " + ex.InnerException);
                     sb.Append("&INSTANCE=" + ex.InnerException);

                     sw.WriteLine("\nDATA: " + ex.Data);
                     sb.Append("&DATA=" + ex.Data);

                     sw.WriteLine("\nURL: " + ctx.Request.Url.ToString());
                     sb.Append("&URL: " + ctx.Request.Url.ToString());
                    
                     sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);
                     sb.Append("&TARGETSITE: " + ex.TargetSite);

                     sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                     sb.Append("&STACKTRACE: " + ex.StackTrace + "\n");
                     sw.W
                
                 */
                string s = Request.QueryString["qs"];
                
                if (s != null)
                {
                    string list = Cripto.Desencriptar(s);
                }

                //lbl_Data.Text = ex.Data.ToString();
                //lbl_Instance.Text = ex.InnerException.ToString();
                //lbl_Message.Text = ex.Message;
                //lbl_Source.Text = ex.Source;
                //lbl_StackTrace.Text = ex.StackTrace;
                //lbl_TargetSite.Text = ex.TargetSite.ToString();
                //lbl_URL.Text = Session["ERRORURL"].ToString();
            }
        }

        protected void btn_VolverAEmpezar_Click(object sender, EventArgs e)
        {
            if (Page.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            Response.Redirect("~/Default.aspx?mode=error");
        }
    }
}