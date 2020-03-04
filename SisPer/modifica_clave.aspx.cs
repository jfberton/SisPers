using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo;
using System.Web.Security;
using SisPer.Aplicativo.Controles;

namespace SisPer
{
    public partial class modifica_clave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Guid guid;
                using (var cxt = new Model1Container())
                {
                    if (Guid.TryParse(Request.QueryString["q"], out guid))
                    {
                        CambioClave cc = cxt.CambiosDeClave.FirstOrDefault(ccc => ccc.Guid == guid && ccc.FechaAceptacion.HasValue == false);
                        if (cc != null)
                        {
                            cc.FechaAceptacion = DateTime.Now;
                            Agente ag = cxt.Agentes.First(a => a.Id == cc.AgenteId);

                            cxt.SaveChanges();

                            Session["UsuarioLogueado"] = ag;
                            Session["Cambiar_clave_al_ingresar"] = true;
                            FormsAuthentication.RedirectFromLoginPage(ag.ApellidoYNombre, false);
                        }
                        else
                        {
                            MessageBox.Show(this.Page, "El link ya fue utilizado, genere uno nuevo para continuar.", MessageBox.Tipo_MessageBox.Danger, "Error de clave", "dbatp.chaco.gov.ar/SisPersonal");
                        }
                    }
                }
            }
        }
    }
}