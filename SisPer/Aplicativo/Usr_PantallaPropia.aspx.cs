using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SisPer.Aplicativo
{
    public partial class Usr_PantallaPropia : System.Web.UI.Page
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string usr = Session["AgentePantallaPropia"] as string;
                Session["AgentePantallaPropia"] = null;
                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;

                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                Agente ag = cxt.Agentes.First(u => u.Usr == usr);
                Session["AgentePP"] = ag;

                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            Agente ag = Session["AgentePP"] as Agente;
            if (ag.CambioPendiente == null)
            {
                using (var cxt = new Model1Container())
                {
                    ag = cxt.Agentes.FirstOrDefault(aa => aa.Id == ag.Id);

                    tb_ApyNom.Text = ag.ApellidoYNombre;
                    tb_DNI.Text = ag.Legajo_datos_personales.DNI;
                    tb_CUIL.Text = ag.Legajo_datos_laborales.CUIT;
                    tb_Email.Text = ag.Legajo_datos_laborales.Email;
                    tb_FechaNacimiento.Value = ag.Legajo_datos_personales.FechaNacimiento.ToShortDateString();
                    tb_ficha_medica.Text = ag.Legajo_datos_laborales.FichaMedica;
                    tb_direccion.Text = ag.Legajo_datos_personales.Domicilio;
                    tb_localidad.Text = ag.Legajo_datos_personales.Domicilio_localidad;
                    tb_aclaracion_domicilio.Text = ag.Legajo_datos_personales.DomicilioObservaciones;

                    if (Directory.Exists(pathImagenesDisco + ag.Legajo.ToString()))
                    {
                        if (File.Exists(pathImagenesDisco + ag.Legajo + "\\Original.jpg"))
                        {//Si no existe la imagen temporal pero si la original, la cargo
                            img_cuenta.ImageUrl = "~/Imagenes/" + ag.Legajo + "/Original.jpg";
                        }
                        else
                        {//Si no existe la imagen temporal y tampoco la original, cargo la default
                            img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                        }
                    }
                    else
                    {//Si no existe la carpeta del usuario directamente cargo la imagen de default
                        img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                    }
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Usted posee cambios por confirmar", Controles.MessageBox.Tipo_MessageBox.Info, "ATENCION!");

                tb_ApyNom.Text = ag.CambioPendiente.ApyNom;
                tb_DNI.Text = ag.CambioPendiente.DNI;
                tb_Email.Text = ag.CambioPendiente.Mail;
                tb_FechaNacimiento.Value = ag.CambioPendiente.FechaNacimiento.ToShortDateString();
                tb_CUIL.Text = ag.CambioPendiente.CUIL;
                tb_ficha_medica.Text = ag.CambioPendiente.FichaMedica;
                tb_direccion.Text = ag.CambioPendiente.Dom_direccion;
                tb_localidad.Text = ag.CambioPendiente.Dom_localidad;
                tb_aclaracion_domicilio.Text = ag.CambioPendiente.Dom_aclaraciones;
                
                if (Directory.Exists(pathImagenesDisco + ag.Legajo))
                {
                    if (File.Exists(pathImagenesDisco + ag.Legajo + "\\Temp.jpg"))
                    {//Si existe la imagen temporal, la cargo y remarco la imagen
                        img_cuenta.ImageUrl = "~/Imagenes/" + ag.Legajo + "/Temp.jpg";
                    }
                    else
                    {
                        if (File.Exists(pathImagenesDisco + ag.Legajo + "\\Original.jpg"))
                        {//Si no existe la imagen temporal pero si la original, la cargo
                            img_cuenta.ImageUrl = "~/Imagenes/" + ag.Legajo + "/Original.jpg";
                        }
                        else
                        {//Si no existe la imagen temporal y tampoco la original, cargo la default
                            img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                        }
                    }
                }
                else
                {//Si no existe la carpeta del usuario directamente cargo la imagen de default
                    img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                }
            }
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_FechaNacimiento.Value, out d);
        }

        protected void btn_SolicitarCambios_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                Agente ag = Session["AgentePP"] as Agente;
                if (ag.CambioPendiente == null)
                {
                    ag.CambioPendiente = new CambioPendiente()
                    {
                        ApyNom = tb_ApyNom.Text,
                        DNI = tb_DNI.Text,
                        CUIL = tb_CUIL.Text,
                        FechaNacimiento = Convert.ToDateTime(tb_FechaNacimiento.Value),
                        FichaMedica = tb_ficha_medica.Text,
                        Mail = tb_Email.Text,
                        Dom_direccion = tb_direccion.Text,
                        Dom_aclaraciones = tb_aclaracion_domicilio.Text,
                        Dom_localidad = tb_localidad.Text
                    };
                }
                else
                {
                    ag.CambioPendiente.ApyNom = tb_ApyNom.Text;
                    ag.CambioPendiente.DNI = tb_DNI.Text;
                    ag.CambioPendiente.CUIL = tb_CUIL.Text;
                    ag.CambioPendiente.FechaNacimiento = Convert.ToDateTime(tb_FechaNacimiento.Value);
                    ag.CambioPendiente.Mail = tb_Email.Text;
                    ag.CambioPendiente.FichaMedica = tb_ficha_medica.Text;
                    ag.CambioPendiente.Dom_aclaraciones = tb_aclaracion_domicilio.Text;
                    ag.CambioPendiente.Dom_direccion = tb_direccion.Text;
                    ag.CambioPendiente.Dom_localidad = tb_localidad.Text;
                }

                cxt.SaveChanges();
                Session["ImagenTemp"] = null;
                VolverAPaginaInicial();
            }
        }

        private void VolverAPaginaInicial()
        {
            Agente u = ((Agente)Session["UsuarioLogueado"]);

            if (u.Perfil == PerfilUsuario.Agente)
            {
                if (u.Jefe == true || u.JefeTemporal)
                {
                    Response.Redirect("~/Aplicativo/MainJefe.aspx");
                }
                else
                {
                    Session["IdAg"] = u.Id.ToString();
                    Response.Redirect("~/Aplicativo/MainAgente.aspx");
                }
            }
            else
            {
                if (u.Perfil == PerfilUsuario.Personal)
                {
                    Response.Redirect("~/Aplicativo/MainPersonal.aspx");
                }
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Model1Container cxt = Session["CXT"] as Model1Container;
            Agente ag = Session["AgentePP"] as Agente;
            HttpPostedFile file = archivo_imagen.PostedFile;
            if (file != null && file.ContentLength > 0)
            {

                string path = pathImagenesDisco + ag.Legajo;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "\\Temp.jpg"))
                {
                    File.Delete(path + "\\Temp.jpg");
                }

                file.SaveAs(path + "\\Temp.jpg");

                img_cuenta.ImageUrl = "~/Imagenes/" + ag.Legajo + "/Temp.jpg";

            }
        }

        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            VolverAPaginaInicial();
        }

        protected void btn_cambiar_clave_ServerClick(object sender, EventArgs e)
        {
            Agente ag = (Agente)Session["UsuarioLogueado"];
            Session["AgentePantallaPropia"] = ag.Usr;
            Response.Redirect("~/Aplicativo/Usr_CambiarClave.aspx");
        }

        protected void btn_cambiar_email_ServerClick(object sender, EventArgs e)
        {
            Agente ag = (Agente)Session["UsuarioLogueado"];
            Session["AgentePantallaPropia"] = ag.Usr;
            Response.Redirect("~/Aplicativo/usr_modifica_mail.aspx");
        }

    }
}