using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using SisPer.Aplicativo.Controles;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace SisPer.Aplicativo
{
    public partial class usr_modifica_mail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string usr = Session["AgentePantallaPropia"] as string;
                Session["AgentePantallaPropia"] = null;
                Model1Container cxt = new Model1Container();

                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                Agente ag = cxt.Agentes.First(u => u.Usr == usr);
                Session["Agentevm"] = ag;

                lbl_mail_actual.Text = ag.Legajo_datos_laborales.Email;
            }
        }

        protected void btn_enviar_validacion_ServerClick(object sender, EventArgs e)
        {
            Agente ag = Session["Agentevm"] as Agente;

            using (var cxt = new Model1Container())
            {

                Validacion_email vm = cxt.Validaciones_email.FirstOrDefault(vmail=>vmail.AgenteId == ag.Id);
                if(vm== null)
                {
                    vm = new Validacion_email();
                    cxt.Validaciones_email.AddObject(vm);
                }
                    
                vm.AgenteId = ag.Id;
                vm.Fecha_envio = DateTime.Now;
                vm.Fecha_validado = null;
                vm.Mail = tb_mail_nuevo.Value;
                vm.Clave = tb_mail_nuevo.Value.GetHashCode().ToString();

                cxt.SaveChanges();
            }

            MailMessage mail = new MailMessage("atp.jfbertoncini@chaco.gov.ar", tb_mail_nuevo.Value);
            mail.IsBodyHtml = true;
            mail.AlternateViews.Add(Cuerpo_de_mensaje(tb_mail_nuevo.Value.GetHashCode().ToString()));
            mail.Subject = "Sistema Personal - Solicitud de validación de correo.";

            SmtpClient clienteSmtp = new SmtpClient("mail.chaco.gov.ar");
            clienteSmtp.Credentials = new NetworkCredential("atp.jfbertoncini", "28162815");
            clienteSmtp.Send(mail);


            MessageBox.Show(this, "Se envió un correo de confirmación a la dirección ingresada. Por favor complete los pasos que le informamos en el mismo para la validación del correo.");
        }

        private AlternateView Cuerpo_de_mensaje(string clave)
        {
            Agente ag = Session["Agentevm"] as Agente;
            string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\nuevo_logo.jpg";
            LinkedResource inline = new LinkedResource(pathImagenesDisco);
            inline.ContentId = Guid.NewGuid().ToString();

            //Request.Url.ToString().Replace("Aplicativo/usr_modifica_mail", "validar_mail") + "?q=" + clave

            string path_devolucion = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "/SisPersonal/validar_mail.aspx");

            if (path_devolucion.Contains("?"))
            {
                path_devolucion = path_devolucion.Substring(0, path_devolucion.LastIndexOf("?")) + "?q=" + clave;
            }
            else
            {
                path_devolucion = path_devolucion + "?q=" + clave;
            }

            StringBuilder sb = new StringBuilder();

            #region escribir html

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
            sb.AppendLine("              <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;background-color: #D9D9D9\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\">");
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
            sb.AppendLine("                        <table class=\"block-grid\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;width: 100%;max-width: 500px;color: #333;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"transparent\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;font-size: 0\"><!--[if (gte mso 9)|(IE)]><table width=\"100%\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='500'><![endif]--><div class=\"col num12\" style=\"display: inline-block;vertical-align: top;width: 500px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 20px;padding-right: 0px;padding-bottom: 20px;padding-left: 0px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 0px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#555555;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:18px;line-height:22px;color:#555555;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:22px; line-height:26px;\" mce-data-marked=\"1\"><strong><span style=\"line-height: 26px; font-size: 22px;\" mce-data-marked=\"1\">Validaci&#243;n de correo</span></strong></span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 5px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#888888;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#888888;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\">Estimado " + ag.ApellidoYNombre + ":<br></div>");
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
            sb.AppendLine("              <table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;background-color: #EDEDED\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\">");
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
            sb.AppendLine("                        <table class=\"block-grid\" style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top;width: 100%;max-width: 500px;color: #000000;background-color: transparent\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"transparent\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;text-align: center;font-size: 0\"><!--[if (gte mso 9)|(IE)]><table width=\"100%\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><![endif]--><!--[if (gte mso 9)|(IE)]><td class='' valign=\"top\" width='500'><![endif]--><div class=\"col num12\" style=\"display: inline-block;vertical-align: top;width: 500px\"><table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\" width=\"100%\" border=\"0\"><tbody><tr style=\"vertical-align: top\"><td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;background-color: transparent;padding-top: 10px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px;border-top: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-left: 0px solid transparent\">");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">");
            sb.AppendLine("    <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("        <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;width: 100%;padding-top: 0px;padding-right: 0px;padding-bottom: 0px;padding-left: 0px\" align=\"center\">");
            sb.AppendLine("            <div align=\"center\">");
            sb.AppendLine("                <a href=\"http://atp.chaco.gob.ar/\" target=\"_blank\">");
            sb.AppendLine("                    <img class=\"center\" style=\"outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: block;border: none;height: auto;line-height: 100%;margin: 0 auto;float: none;width: 151px;max-width: 151px\" align=\"center\" border=\"0\" src=\"cid:" + inline.ContentId + "\" alt=\"ATP\" title=\"ATP\" width=\"151\">");
            sb.AppendLine("                </a>");
            sb.AppendLine("");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </td>");
            sb.AppendLine("    </tr>");
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
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#555555;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:24px; line-height:29px;\" mce-data-marked=\"1\"><strong><span style=\"font-family: arial, helvetica, sans-serif; line-height: 28px; font-size: 24px;\" mce-data-marked=\"1\">Tenga en cuenta que:</span></strong></span></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </td>");
            sb.AppendLine("  </tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<table style=\"border-spacing: 0;border-collapse: collapse;vertical-align: top\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
            sb.AppendLine("  <tbody><tr style=\"vertical-align: top\">");
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 5px;padding-right: 10px;padding-bottom: 5px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#777777;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:12px;line-height:14px;color:#777777;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><font face=\"arial, helvetica, sans-serif\"><span style=\"font-size: 16px; line-height: 19px;\">Esta cuenta ser&#225; el nexo por el cual el sistema informar&#225; de cualquier cambioo situaci&#243;n ante el sistema de personal, asi como tambi&#233;n la recuperaci&#243;n de clave del mismo.</span></font></div>");
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
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 10px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#555555;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#555555;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:14px; line-height:17px;\" id=\"_mce_caret\" data-mce-bogus=\"true\"><span style=\"font-family: inherit; font-size: 14px; line-height: 16px;\">&#65279;</span></span>Haga&nbsp;<a style=\"font-size: 14px; line-height: 16px;;color:#0000FF\" title=\"validar mail\" href=\"" + path_devolucion + "\" target=\"_blank\">clic aqu&#237;</a>&nbsp;para continuar con la validaci&#243;n del correo<br></div>");
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
            sb.AppendLine("    <td style=\"word-break: break-word;-webkit-hyphens: auto;-moz-hyphens: auto;hyphens: auto;border-collapse: collapse !important;vertical-align: top;padding-top: 15px;padding-right: 10px;padding-bottom: 10px;padding-left: 10px\">");
            sb.AppendLine("        <div style=\"color:#aaaaaa;line-height:120%;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;\">            ");
            sb.AppendLine("        	<div style=\"font-size:14px;line-height:17px;color:#aaaaaa;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\">SI no has solicitado el cambio de correo no hagas nada y seguir&#225; siendo todo como siempre. El sistema jam&#225;s solicitar&#225; datos por este medio.<br></div>");
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
            sb.AppendLine("        	<div style=\"font-size:12px;line-height:14px;color:#bbbbbb;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;text-align:left;\"><span style=\"font-size:16px; line-height:19px;\">- Sistema de Personal de la Administraci&#243;n Tributaria Provincial -</span><span style=\"font-size: 12px; line-height: 14px;\" id=\"_mce_caret\" data-mce-bogus=\"true\"><span style=\"font-size: 16px; line-height: 19px;\">&#65279;</span></span></div>");
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