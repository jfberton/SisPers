using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo;
using System.Web.Security;
using System.IO;
using SisPer.Aplicativo.Reportes;
using Microsoft.Reporting.WebForms;
using System.Web.Services;

namespace SisPer
{
    public partial class Site2 : System.Web.UI.MasterPage
    {
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    if (CheckForSessionTimeout())
        //    {
        //        if (Page.Request.IsAuthenticated)
        //        {
        //            FormsAuthentication.SignOut();
        //        }

        //        Response.Redirect("~/Default.aspx?mode=timeout");
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbl_AñoActual.Text = DateTime.Today.Year.ToString();
                lbl_usuarios_logueados.Text = Global.CurrentNumberOfUsers.ToString();
                using (var cxt = new Model1Container())
                {
                    lbl_db.Text = cxt.Connection.DataSource.ToString() + " - " + ((System.Data.SqlClient.SqlConnection)((System.Data.EntityClient.EntityConnection)cxt.Connection).StoreConnection).Database;
                }
            }
        }

        //public bool CheckForSessionTimeout()
        //{
        //    if (Context.Session != null && Context.Session.IsNewSession)
        //    {
        //        string cookieHeader = Page.Request.Headers["Cookie"];
        //        if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}

        protected void btn_modo_dev_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx?mode=Dev");
        }
      
    }
}