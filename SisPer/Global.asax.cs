using SisPer.Aplicativo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SisPer
{
    public class Global : System.Web.HttpApplication
    {
        private static int totalNumberOfUsers = 0;
        private static int currentNumberOfUsers = 0;

        public static int TotalNumberOfUsers
        {
            get
            {
                return totalNumberOfUsers;
            }
        }

        public static int CurrentNumberOfUsers
        {
            get
            {
                return currentNumberOfUsers;
            }
        } 

        protected void Application_Start(object sender, EventArgs e)
        {
            //ListadoAgentesParaGrilla.Iniciar();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            totalNumberOfUsers += 1;
             currentNumberOfUsers += 1;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server.GetLastError() != null)
            {
                // At this point we have information about the error
                HttpContext ctx = HttpContext.Current;
                Exception ex = ctx.Server.GetLastError().GetBaseException();
                string querystring = string.Empty;

                //hago esta consulta primero porque siempre me tira un error de que no encuentra
                //una imagen aparentemente pero no puedo determinar que es asi que lo obvio nomas
                if (!(ex.Message == "File does not exist." && ex.Source == "System.Web" && ex.InnerException == null))
                {
                    //guardo en el log.
                    string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
                    StreamWriter sw;
                    StringBuilder date = new StringBuilder();
                    StringBuilder agente = new StringBuilder();
                    StringBuilder message = new StringBuilder();
                    StringBuilder source = new StringBuilder();
                    StringBuilder instance = new StringBuilder();
                    StringBuilder data = new StringBuilder();
                    StringBuilder url = new StringBuilder();
                    StringBuilder targetsite = new StringBuilder();
                    StringBuilder stacktrace = new StringBuilder();

                    sw = File.AppendText(directorioRaiz + "Errores_Log.txt");

                    sw.WriteLine(" \n");
                    sw.WriteLine("******************************************************************");

                    sw.WriteLine("\nDATE: " + System.DateTime.Now);
                    date.Append(System.DateTime.Now);

                    if (ctx.User!=null && ctx.User.Identity.IsAuthenticated)
                    {
                        sw.WriteLine("\nAGENTE: " + ctx.User.Identity.Name.Replace("Bienvenido ", "").Replace("|", ""));
                        agente.Append(ctx.User.Identity.Name.Replace("Bienvenido ", "").Replace("|", ""));
                    }
                    else
                    {
                        agente.Append("SinAutenticar");
                    }

                    sw.WriteLine("\nMESSAGE: " + ex.Message);
                    message.Append(ex.Message);

                    sw.WriteLine("\nSOURCE: " + ex.Source);
                    source.Append(ex.Source);
                    sw.WriteLine("\nINSTANCE: " + ex.InnerException);
                    instance.Append(ex.InnerException);

                    sw.WriteLine("\nDATA: " + ex.Data);
                    data.Append(ex.Data);

                    sw.WriteLine("\nURL: " + ctx.Request.Url.ToString());
                    url.Append(ctx.Request.Url.ToString());
                    
                    sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);
                    targetsite.Append(ex.TargetSite);

                    sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                    stacktrace.Append(ex.StackTrace);
                    sw.WriteLine("\n******************************************************************");
                    sw.Close();

                    querystring = "?" +
                        "qs1=" + Cripto.Encriptar(date.ToString()) +
                        "&qs2=" + Cripto.Encriptar(agente.ToString()) +
                        "&qs3=" + Cripto.Encriptar(message.ToString()) +
                        "&qs4=" + Cripto.Encriptar(source.ToString()) +
                        "&qs5=" + Cripto.Encriptar(instance.ToString()) +
                        "&qs6=" + Cripto.Encriptar(data.ToString()) +
                        "&qs7=" + Cripto.Encriptar(url.ToString()) +
                        "&qs8=" + Cripto.Encriptar(targetsite.ToString()) +
                        "&qs9=" + Cripto.Encriptar(stacktrace.ToString());


                }


                ctx.Server.ClearError();


                if (querystring.Length > 0)
                {
                    Response.Redirect("~/Error.aspx");//+querystring);
                }
                else
                {
                    Response.Redirect("~/Error.aspx");
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            currentNumberOfUsers -= 1;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}