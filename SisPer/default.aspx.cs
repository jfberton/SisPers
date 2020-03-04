using SisPer.Aplicativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_AñoActual.Text = DateTime.Today.Year.ToString();
            lbl_usuarios_logueados.Text = Global.CurrentNumberOfUsers.ToString();

            if (!IsPostBack)
            {
                Session.Clear();

                if (EstaEnMantenimiento())
                {
                    p_EnMantenimiento.Visible = true;
                    p_Normal.Visible = false;
                    p_Ambiente_prueba.Visible = false;
                }
                else
                {
                    if (EsAmbienteDePrueba())
                    {
                        CargarGrillaAgentes();
                        p_EnMantenimiento.Visible = false;
                        p_Normal.Visible = false;
                        p_Ambiente_prueba.Visible = true;
                    }
                    else
                    {
                        p_EnMantenimiento.Visible = false;
                        p_Normal.Visible = true;
                        p_Ambiente_prueba.Visible = false;
                    }
                }

                switch (Request.QueryString["mode"])
                {
                    case "session_end":
                        //Response.Write("<script>alert('Su sesión a expirado.');</script>");

                        Response.Write("<script>" +
                            "alert('Su sesión a expirado.');" +
                            "location.href='Default.aspx';" + "</script>");
                        break;
                    case "trucho":
                        Response.Write("<script>" +
                          "alert('Su perfil tiene definidos los permisos para ingresar en ese directorio.');" +
                          "location.href='Default.aspx';" + "</script>");
                        break;
                    case "Dev":
                        p_EnMantenimiento.Visible = false;
                        p_Ambiente_prueba.Visible = false;
                        p_Normal.Visible = true;
                        tb_Usuario.Value = "SU";
                        tb_Usuario.Disabled = true;
                        tb_Contraseña.Focus();
                        btn_cancelar_dev.Visible = true;
                        break;
                    default:
                        Session["CXT"] = new Model1Container();
                        Session["UsuarioLogueado"] = null;
                        tb_Usuario.Focus();
                        break;
                }
            }

        }

        protected void btn_modo_dev_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx?mode=Dev");
        }

        private void CargarGrillaAgentes()
        {
            using (var cxt = new Model1Container())
            {
                var agentes = (from a in cxt.Agentes
                               where a.FechaBaja == null
                               select new
                               {
                                   IdAgente = a.Id,
                                   Legajo = a.Legajo,
                                   Agente = a.ApellidoYNombre,
                                   Area = a.Area.Nombre
                               }).ToList();



                gv_Agentes.DataSource = agentes;
                gv_Agentes.DataBind();
            }
        }

        private bool EsAmbienteDePrueba()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            return url.ToUpper().Contains("SISPERSONALPRUEBA");
        }

        public static string GetVisitorIPAddress(bool GetLan = false)
        {
            string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
            {
                GetLan = true;
                visitorIPAddress = string.Empty;
            }

            if (GetLan && string.IsNullOrEmpty(visitorIPAddress))
            {
                //This is for Local(LAN) Connected ID Address
                string stringHostName = Dns.GetHostName();
                //Get Ip Host Entry
                IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                //Get Ip Address From The Ip Host Entry Address List
                IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                try
                {
                    visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                }
                catch
                {
                    try
                    {
                        visitorIPAddress = arrIpAddress[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            arrIpAddress = Dns.GetHostAddresses(stringHostName);
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            visitorIPAddress = "127.0.0.1";
                        }
                    }
                }

            }

            return visitorIPAddress;
        }

        private bool EstaEnMantenimiento()
        {
            Model1Container cxt = new Model1Container();
            return cxt.VariablesGlobales.First().EnMantenimiento;
        }

        private void MessageBox(string message)
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">alert('" + message + "');</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", script, false);
        }

        protected void IngresarButtton_Click(object sender, EventArgs e)
        {
            Login();
            Agente u = ((Agente)Session["UsuarioLogueado"]);

            if (u != null && u.Pass == Cripto.Encriptar(tb_Contraseña.Value))
            {
                FormsAuthentication.RedirectFromLoginPage(u.ApellidoYNombre, false);
            }
            else
            {
                Aplicativo.Controles.MessageBox.Show(this, "Usuario no encontrado o contraseña inválida.", Aplicativo.Controles.MessageBox.Tipo_MessageBox.Default, "Acceso incorrecto");
                tb_Usuario.Value = string.Empty;
                tb_Usuario.Focus();
            }
        }

        protected void CancelarButtton_Click(object sender, EventArgs e)
        {
            Response.Write("<script> location.href='Default.aspx'; </script>");
        }

        private void Login()
        {
            string name = tb_Usuario.Value.ToLower();
            string pass = tb_Contraseña.Value;
            Agente u = new Agente();
            Model1Container cxt = new Model1Container();
            if (name == "su" && pass == "Sistema")
            {
                u.ApellidoYNombre = "Super Usuario";
                u.Perfil = PerfilUsuario.Desarrollador;
                u.Usr = "SU";
                u.Pass = Cripto.Encriptar("Sistema");
            }
            else
            {
                pass = Cripto.Encriptar(pass);

                u = cxt.Agentes.FirstOrDefault(usr => usr.Usr == name && usr.Pass == pass);
            }

            //registro el inicio de sesion
            if (u != null && u.Usr != "SU")
            {
                Sesion s = new Sesion()
                {
                    AgenteId = u.Id,
                    FechaHoraInicio = DateTime.Now,
                    Ip = GetVisitorIPAddress(),
                    Maquina = Request.UserHostName
                };

                cxt.Sesiones.AddObject(s);
                cxt.SaveChanges();
            }

            //Verifico si esta como jefe temporal y si la fecha esta vigente aun
            if (u != null && u.JefeTemporal)
            {
                if (u.JefeTemporalHasta < DateTime.Today)
                {
                    u.JefeTemporal = false;
                    u.JefeTemporalHasta = null;
                    cxt.SaveChanges();
                }
            }

            Session["UsuarioLogueado"] = u;
        }

        /// <summary>
        /// Aqui llega unicamente en ambiente de pruebas
        /// </summary>
        protected void btn_ingresar_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idAgente = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            Agente u = cxt.Agentes.First(a => a.Id == idAgente);
            Session["UsuarioLogueado"] = u;

            if (u != null && u.Usr != "SU")
            {
                Sesion s = new Sesion()
                {
                    AgenteId = u.Id,
                    FechaHoraInicio = DateTime.Now,
                    Ip = GetVisitorIPAddress(),
                    Maquina = Request.UserHostName
                };

                cxt.Sesiones.AddObject(s);
                cxt.SaveChanges();
            }

            FormsAuthentication.RedirectFromLoginPage(u.ApellidoYNombre, false);
        }

        protected void btn_enviar_solicitud_ServerClick(object sender, EventArgs e)
        {
            hidTAB.Value = "profile";



            using (var cxt = new Model1Container())
            {
                Validacion_email vm = cxt.Validaciones_email.FirstOrDefault(vvmm => vvmm.Mail == tb_mail.Value && vvmm.Fecha_validado.HasValue);
                if (vm != null)
                {
                    Guid guid = Guid.NewGuid();
                    CambioClave cc = new CambioClave();
                    cc.AgenteId = vm.AgenteId;
                    cc.Guid = guid;
                    cc.FechaSolicitud = DateTime.Now;
                    cxt.CambiosDeClave.AddObject(cc);
                    cxt.SaveChanges();

                    MailMessage mail = new MailMessage("atp.jfbertoncini@chaco.gov.ar", tb_mail.Value);
                    mail.IsBodyHtml = true;
                    mail.AlternateViews.Add(Cuerpo_de_mensaje(vm.Agente, guid.ToString()));
                    mail.Subject = "Sistema Personal - Solicitud de cambio  de clave.";

                    SmtpClient clienteSmtp = new SmtpClient("mail.chaco.gov.ar");
                    clienteSmtp.Credentials = new NetworkCredential("atp.jfbertoncini", "28162815");
                    clienteSmtp.Send(mail);

                    lbl_noexistecorreo.Visible = false;
                    lbl_se_envio_mail.Visible = true;
                }
                else
                {
                    lbl_noexistecorreo.Visible = true;
                    lbl_se_envio_mail.Visible = false;
                }
            }
        }

        private AlternateView Cuerpo_de_mensaje(Agente ag, string clave)
        {
            string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\nuevo_logo.jpg";
            LinkedResource inline = new LinkedResource(pathImagenesDisco);
            inline.ContentId = Guid.NewGuid().ToString();

            string path_devolucion = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "/SisPersonal/modifica_clave.aspx");
            
            if (path_devolucion.Contains("?"))
            {
                path_devolucion = path_devolucion.Substring(0, path_devolucion.LastIndexOf("?")) + "?q=" + clave;
            }
            else
            {
                path_devolucion = path_devolucion + "?q=" + clave;
            }
            

            StringBuilder sb = new StringBuilder();

            #region cuerpo html

            sb.AppendLine("<!DOCTYPE html \"-//w3c//dtd xhtml 1.0 transitional //en\" \"http://www.w3.org/tr/xhtml1/dtd/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head>");
            sb.AppendLine("    <!--[if gte mso 9]><xml>");
            sb.AppendLine("     <o:OfficeDocumentSettings>");
            sb.AppendLine("      <o:AllowPNG/>");
            sb.AppendLine("      <o:PixelsPerInch>96</o:PixelsPerInch>");
            sb.AppendLine("     </o:OfficeDocumentSettings>");
            sb.AppendLine("    </xml><![endif]-->");
            sb.AppendLine("    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width\">");
            sb.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=9; IE=8; IE=7; IE=EDGE\">");
            sb.AppendLine("    <title>Template Base</title>");
            sb.AppendLine("    ");
            sb.AppendLine("</head>");
            sb.AppendLine("<body style=\"width: 100% !important;min-width: 100%;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100% !important;margin: 0;padding: 0;background-color: #FFFFFF\">");
            sb.AppendLine("  <style id=\"media-query\">");
            sb.AppendLine("      /* Client-specific Styles & Reset */");
            sb.AppendLine("      #outlook a {");
            sb.AppendLine("          padding: 0;");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("      /* .ExternalClass applies to Outlook.com (the artist formerly known as Hotmail) */");
            sb.AppendLine("      .ExternalClass {");
            sb.AppendLine("          width: 100%;");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("          .ExternalClass,");
            sb.AppendLine("          .ExternalClass p,");
            sb.AppendLine("          .ExternalClass span,");
            sb.AppendLine("          .ExternalClass font,");
            sb.AppendLine("          .ExternalClass td,");
            sb.AppendLine("          .ExternalClass div {");
            sb.AppendLine("              line-height: 100%;");
            sb.AppendLine("          }");
            sb.AppendLine("");
            sb.AppendLine("      #backgroundTable {");
            sb.AppendLine("          margin: 0;");
            sb.AppendLine("          padding: 0;");
            sb.AppendLine("          width: 100% !important;");
            sb.AppendLine("          line-height: 100% !important;");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("      /* Buttons */");
            sb.AppendLine("      .button a {");
            sb.AppendLine("          display: inline-block;");
            sb.AppendLine("          text-decoration: none;");
            sb.AppendLine("          -webkit-text-size-adjust: none;");
            sb.AppendLine("          text-align: center;");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("          .button a div {");
            sb.AppendLine("              text-align: center !important;");
            sb.AppendLine("          }");
            sb.AppendLine("");
            sb.AppendLine("      /* Outlook First */");
            sb.AppendLine("      body.outlook p {");
            sb.AppendLine("          display: inline !important;");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("      /*  Media Queries */");
            sb.AppendLine("@media only screen and (max-width: 500px) {");
            sb.AppendLine("  table[class=\"body\"] img {");
            sb.AppendLine("    height: auto !important;");
            sb.AppendLine("    width: 100% !important; }");
            sb.AppendLine("  table[class=\"body\"] img.fullwidth {");
            sb.AppendLine("    width: 100% !important; }");
            sb.AppendLine("  table[class=\"body\"] center {");
            sb.AppendLine("    min-width: 0 !important; }");
            sb.AppendLine("  table[class=\"body\"] .container {");
            sb.AppendLine("    width: 95% !important; }");
            sb.AppendLine("  table[class=\"body\"] .row {");
            sb.AppendLine("    width: 100% !important;");
            sb.AppendLine("    display: block !important; }");
            sb.AppendLine("  table[class=\"body\"] .wrapper {");
            sb.AppendLine("    display: block !important;");
            sb.AppendLine("    padding-right: 0 !important; }");
            sb.AppendLine("  table[class=\"body\"] .columns, table[class=\"body\"] .column {");
            sb.AppendLine("    table-layout: fixed !important;");
            sb.AppendLine("    float: none !important;");
            sb.AppendLine("    width: 100% !important;");
            sb.AppendLine("    padding-right: 0px !important;");
            sb.AppendLine("    padding-left: 0px !important;");
            sb.AppendLine("    display: block !important; }");
            sb.AppendLine("  table[class=\"body\"] .wrapper.first .columns, table[class=\"body\"] .wrapper.first .column {");
            sb.AppendLine("    display: table !important; }");
            sb.AppendLine("  table[class=\"body\"] table.columns td, table[class=\"body\"] table.column td, .col {");
            sb.AppendLine("    width: 100% !important; }");
            sb.AppendLine("  table[class=\"body\"] table.columns td.expander {");
            sb.AppendLine("    width: 1px !important; }");
            sb.AppendLine("  table[class=\"body\"] .right-text-pad, table[class=\"body\"] .text-pad-right {");
            sb.AppendLine("    padding-left: 10px !important; }");
            sb.AppendLine("  table[class=\"body\"] .left-text-pad, table[class=\"body\"] .text-pad-left {");
            sb.AppendLine("    padding-right: 10px !important; }");
            sb.AppendLine("  table[class=\"body\"] .hide-for-small, table[class=\"body\"] .show-for-desktop {");
            sb.AppendLine("    display: none !important; }");
            sb.AppendLine("  table[class=\"body\"] .show-for-small, table[class=\"body\"] .hide-for-desktop {");
            sb.AppendLine("    display: inherit !important; }");
            sb.AppendLine("  .mixed-two-up .col {");
            sb.AppendLine("    width: 100% !important; } }");
            sb.AppendLine(" @media screen and (max-width: 500px) {");
            sb.AppendLine("          div[class=\"col\"] {");
            sb.AppendLine("              width: 100% !important;");
            sb.AppendLine("          }");
            sb.AppendLine("      }");
            sb.AppendLine("");
            sb.AppendLine("      @media screen and (min-width: 501px) {");
            sb.AppendLine("          table[class=\"container\"] {");
            sb.AppendLine("              width: 500px !important;");
            sb.AppendLine("          }");
            sb.AppendLine("      }");
            sb.AppendLine("  </style>");
            sb.AppendLine("  <table class=\"body\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;height: 100%;width: 100%;table-layout: fixed\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">");
            sb.AppendLine("      <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("          <td class=\"center\" style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;background-color: #FFFFFF\" align=\"center\" valign=\"top\">");
            sb.AppendLine("              <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\">");
            sb.AppendLine("                <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("                  <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                    <!--[if gte mso 9]>");
            sb.AppendLine("                    <table id=\"outlookholder\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tr><td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    <table width='500' align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.AppendLine("                        <tr>");
            sb.AppendLine("                            <td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <table class=\"container\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;max-width: 500px;margin: 0 auto;text-align: inherit\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                        <table class=\"block-grid mixed-two-up\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;width: 100%;max-width: 500px;color: #333;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"transparent\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;font-size: 0\"><!--[if (gte mso 9)|(IE)]><table width=\"100%\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='167'><![endif]--><div class=\"col num4\" style=\"display: inline-block;vertical-align: top;text-align: center;width: 167px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 15px;padding-right: 10px;padding-bottom: 15px;padding-left: 10px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">");
            sb.AppendLine("    <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("        <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;width: 100%;padding-top: 0px;padding-right: 0px;padding-bottom: 0px;padding-left: 0px\" align=\"center\">");
            sb.AppendLine("            <div align=\"center\">");
            sb.AppendLine("");
            sb.AppendLine("                <img class=\"center\" style=\"outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: block;border: 0;height: auto;line-height: 100%;margin: 0 auto;float: none;width: 100px;max-width: 100px\" align=\"center\" border=\"0\" src=\"cid:" + inline.ContentId + "\" alt=\"Image\" title=\"Image\" width=\"100\">");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </td>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("                        </td></tr></tbody></table></div><!--[if (gte mso 9)|(IE)]></td><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='333'><![endif]--><div class=\"col num8\" style=\"display: inline-block;vertical-align: top;text-align: center;width: 333px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 15px;padding-right: 0px;padding-bottom: 15px;padding-left: 0px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#555555;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:12px;line-height:14px;color:#555555;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:18px; line-height:22px;\"><strong>Solicitud de cambio de clave</strong></span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("                        </td></tr></tbody></table></div><!--[if (gte mso 9)|(IE)]></td><![endif]--><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr></tbody></table></td></tr></tbody></table>");
            sb.AppendLine("                    <!--[if mso]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                  </td>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("              </tbody></table>");
            sb.AppendLine("              <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\">");
            sb.AppendLine("                <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("                  <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                    <!--[if gte mso 9]>");
            sb.AppendLine("                    <table id=\"outlookholder\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tr><td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    <table width='500' align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.AppendLine("                        <tr>");
            sb.AppendLine("                            <td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <table class=\"container\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;max-width: 500px;margin: 0 auto;text-align: inherit\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                        <table class=\"block-grid\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;width: 100%;max-width: 500px;color: #333;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"transparent\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;font-size: 0\"><!--[if (gte mso 9)|(IE)]><table width=\"100%\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='500'><![endif]--><div class=\"col num12\" style=\"display: inline-block;vertical-align: top;width: 500px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 30px;padding-right: 0px;padding-bottom: 30px;padding-left: 0px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 0px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#555555;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#555555;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:24px; line-height:29px;\" mce-data-marked=\"1\"><strong><span style=\"font-family: arial, helvetica, sans-serif; line-height: 28px; font-size: 24px;\" mce-data-marked=\"1\">Estimado " + ag.ApellidoYNombre + ":</span></strong></span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" align=\"center\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.AppendLine("    <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("        <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\" align=\"center\">");
            sb.AppendLine("            <div style=\"height: 1px;\">");
            sb.AppendLine("                <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;border-top: 1px solid #BBBBBB;width: 100%\" align=\"center\" border=\"0\" cellspacing=\"0\">");
            sb.AppendLine("                    <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("                        <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" align=\"center\"></td>");
            sb.AppendLine("                    </tr>");
            sb.AppendLine("                </tbody></table>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </td>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("</tbody></table><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 5px;padding-right: 10px;padding-bottom: 5px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#777777;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:12px;line-height:14px;color:#777777;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:14px; line-height:17px;\">Haga&nbsp;<a style=\"font-size: 14px; line-height: 16px;;color:#0000FF\" title=\"cambiar clave\" href=\"" + path_devolucion + "\" target=\"_blank\">click aqu&#237;</a>&nbsp;para modificar su contrase&#241;a</span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 15px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#aaaaaa;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#aaaaaa;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><p style=\"margin: 0;font-size: 14px;line-height: 17px\">SI no has solicitado el cambio de correo no hagas nada y seguir&#225; siendo todo como siempre. El sistema jam&#225;s solicitar&#225; datos por este medio.</p></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("                        </td></tr></tbody></table></div><!--[if (gte mso 9)|(IE)]></td><![endif]--><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr></tbody></table></td></tr></tbody></table>");
            sb.AppendLine("                    <!--[if mso]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                  </td>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("              </tbody></table>");
            sb.AppendLine("              <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;background-color: #444444\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\">");
            sb.AppendLine("                <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("                  <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                    <!--[if gte mso 9]>");
            sb.AppendLine("                    <table id=\"outlookholder\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tr><td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    <table width='500' align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.AppendLine("                        <tr>");
            sb.AppendLine("                            <td>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <table class=\"container\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;max-width: 500px;margin: 0 auto;text-align: inherit\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top\" width=\"100%\">");
            sb.AppendLine("                        <table class=\"block-grid\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;width: 100%;max-width: 500px;color: #333;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"transparent\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;font-size: 0\"><!--[if (gte mso 9)|(IE)]><table width=\"100%\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='500'><![endif]--><div class=\"col num12\" style=\"display: inline-block;vertical-align: top;width: 500px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 25px;padding-right: 0px;padding-bottom: 25px;padding-left: 0px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#bbbbbb;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:18px;line-height:22px;text-align:center;color:#bbbbbb;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\"><span style=\"font-size:16px; line-height:22px;\">- Sistema de Personal de la Administraci&#243;n Tributaria Provincial -</span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("                        </td></tr></tbody></table></div><!--[if (gte mso 9)|(IE)]></td><![endif]--><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr></tbody></table></td></tr></tbody></table>");
            sb.AppendLine("                    <!--[if mso]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                    <!--[if (IE)]>");
            sb.AppendLine("                    </td></tr></table>");
            sb.AppendLine("                    <![endif]-->");
            sb.AppendLine("                  </td>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("              </tbody></table>");
            sb.AppendLine("          </td>");
            sb.AppendLine("      </tr>");
            sb.AppendLine("  </tbody></table>");
            sb.AppendLine("");
            sb.AppendLine("</body></html>");

            #endregion

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(sb.ToString(), null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);
            return alternateView;
        }
    }
}