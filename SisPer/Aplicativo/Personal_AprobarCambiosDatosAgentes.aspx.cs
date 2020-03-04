using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;

namespace SisPer.Aplicativo
{
    public partial class Personal_AprobarCambiosDatosAgentes : System.Web.UI.Page
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (usuariologueado.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                    string usr = Session["Usr"] as string;
                    Session["Usr"] = null;
                    if (usr != string.Empty)
                    {
                        Model1Container cxt = new Model1Container();
                        Session["CXT"] = cxt;
                        Agente ag = cxt.Agentes.First(a => a.Usr == usr);
                        Session["Usuario"] = ag;
                        CargarDatos();
                    }
                }
            }
        }

        private void CargarDatos()
        {
            Agente ag = Session["Usuario"] as Agente;

            CambioPendiente cp = ag.CambioPendiente;

            var cxt = new Model1Container();
            tb_ApyNom.Text = cp.ApyNom;
            if (cp.ApyNom != ag.ApellidoYNombre)
            {
                RemarcarControl(tb_ApyNom);
            }
            
            tb_CUIL.Text = cp.CUIL;
            if (cp.CUIL != ag.Legajo_datos_laborales.CUIT)
            {
                RemarcarControl(tb_CUIL);
            }
            
            tb_DNI.Text = cp.DNI;
            if (cp.DNI != ag.Legajo_datos_personales.DNI)
            {
                RemarcarControl(tb_DNI);
            }
            
            tb_Email.Text = cp.Mail;
            if (cp.Mail != ag.Legajo_datos_laborales.Email)
            {
                RemarcarControl(tb_Email);
            }

            tb_FechaNacimiento.Text = cp.FechaNacimiento.ToShortDateString();
            if (cp.FechaNacimiento != ag.Legajo_datos_personales.FechaNacimiento)
            {
                RemarcarControl(tb_FechaNacimiento);
            }

            tb_FichaMedica.Text = cp.FichaMedica;
            if (cp.FichaMedica != ag.Legajo_datos_laborales.FichaMedica)
            {
                RemarcarControl(tb_FichaMedica);
            }

            tb_direccion.Text = cp.Dom_direccion;
            if (cp.Dom_direccion != ag.Legajo_datos_personales.Domicilio)
            {
                RemarcarControl(tb_direccion);
            }

            tb_localidad.Text = cp.Dom_localidad;
            if (cp.Dom_localidad != ag.Legajo_datos_personales.Domicilio_localidad)
            {
                RemarcarControl(tb_localidad);
            }

            tb_aclaracion_domicilio.Text = cp.Dom_aclaraciones;
            if (cp.Dom_aclaraciones != ag.Legajo_datos_personales.DomicilioObservaciones)
            {
                RemarcarControl(tb_aclaracion_domicilio);
            }


            //img_cuenta.ImageUrl = "Imagen.aspx?ID=" + ag.Id+ "&Cambio=S";
            if (Directory.Exists(pathImagenesDisco + ag.Legajo))
            {
                if (File.Exists(pathImagenesDisco + ag.Legajo + "\\Temp.jpg"))
                {//Si existe la imagen temporal, la cargo y remarco la imagen
                    img_cuenta.ImageUrl = "~/Imagenes/" + ag.Legajo + "/Temp.jpg";
                    RemarcarControl(img_cuenta);
                }
                else
                {
                    if (File.Exists(pathImagenesDisco + ag.Legajo  + "\\Original.jpg"))
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

        private void RemarcarControl(System.Web.UI.WebControls.Image imagen)
        {
            imagen.BorderStyle = BorderStyle.Solid;
            imagen.BorderWidth = 2;
            imagen.BorderColor = Color.DarkRed;
        }

        private void RemarcarControl(TextBox text)
        {
            text.BorderColor = Color.DarkRed;
            text.BackColor = Color.LightPink;
            text.ForeColor = Color.DimGray;
        }

        protected void btn_Volver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
        }

        protected void btn_Rechazar_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Usuario"] as Agente;
            Model1Container cxt = Session["CXT"] as Model1Container;
            cxt.CambiosPendientes.DeleteObject(ag.CambioPendiente);
            cxt.SaveChanges();
            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
        }

        protected void btn_Aceptar_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Usuario"] as Agente;
            Model1Container cxt = Session["CXT"] as Model1Container;
            CambioPendiente cp = ag.CambioPendiente;

            ag.ApellidoYNombre = cp.ApyNom;
            ag.Legajo_datos_personales.Historial_domicilios.Add(new Legajo_historial_domicilio()
            {
                Domicilio = cp.Dom_direccion + " - Localidad: " + cp.Dom_localidad + " - Observaciones: " + (cp.Dom_aclaraciones.Length > 0 ? cp.Dom_aclaraciones : " Sin observaciones.-"),
                Fecha = DateTime.Now
            });
            //ag.Legajo_datos_personales.DNI = cp.DNI;
            ag.Legajo_datos_laborales.Email = cp.Mail;
            ag.Legajo_datos_personales.FechaNacimiento= cp.FechaNacimiento;
            ag.Legajo_datos_laborales.CUIT = cp.CUIL;
            ag.Legajo_datos_laborales.FichaMedica = cp.FichaMedica;
            //ag.Legajo_datos_laborales.Email = cp.Mail;
            ag.Legajo_datos_personales.Domicilio = cp.Dom_direccion;
            ag.Legajo_datos_personales.Domicilio_localidad = cp.Dom_localidad;
            ag.Legajo_datos_personales.DomicilioObservaciones = cp.Dom_aclaraciones;



            if (File.Exists(pathImagenesDisco + ag.Legajo.ToString() + "\\Temp.jpg"))
            { 
                //el agente cambio la imagen
                string path_Origen = pathImagenesDisco + ag.Legajo.ToString() + "\\Temp.jpg";
                string path_Destino = pathImagenesDisco + ag.Legajo.ToString() + "\\Original.jpg";
                File.Copy(path_Origen, path_Destino, true);
                File.Delete(path_Origen);
            }

            cxt.CambiosPendientes.DeleteObject(ag.CambioPendiente);
            cxt.SaveChanges();
            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
        }
    }
}