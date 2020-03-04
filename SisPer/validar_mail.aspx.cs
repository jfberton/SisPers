using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo;
using SisPer.Aplicativo.Controles;
using System.Web.Security;

namespace SisPer
{
    public partial class validar_mail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string clave = Request.QueryString["q"].ToString();

            using (var cxt = new Model1Container())
            {
                Validacion_email vm = cxt.Validaciones_email.FirstOrDefault(vmail => vmail.Clave == clave && vmail.Fecha_validado.HasValue == false);

                if (vm != null)
                {
                    vm.Fecha_validado = DateTime.Now;
                    Agente ag = cxt.Agentes.First(a => a.Id == vm.AgenteId);

                    ag.Legajo_datos_laborales.Email = vm.Mail;

                    cxt.SaveChanges();

                    Session["UsuarioLogueado"] = ag;
                    Session["ValidoMail"] = true;
                    FormsAuthentication.RedirectFromLoginPage(ag.ApellidoYNombre, false);
                }
                else
                {
                    MessageBox.Show(this.Page, "La clave de validación es incorrecta, por favor genere una nueva desde la función habilitada en la página", MessageBox.Tipo_MessageBox.Danger, "Error de clave", "dbatp.chaco.gov.ar/SisPersonal");
                }
            }
        }
    }
}