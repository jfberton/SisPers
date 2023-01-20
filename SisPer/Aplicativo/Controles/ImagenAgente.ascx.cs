using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class ImagenAgente : System.Web.UI.UserControl
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        Agente agente = null;

        public int Height
        {
            set
            {
                img_acceso.Height = value;
                img_cuenta.Height = value;
                img_cumple.Height = value;
            }
        }

        public int Width
        {
            set
            {
                img_acceso.Width = value;
                img_cuenta.Width = value;
                img_cumple.Width = value;
            }
        }

        public Agente Agente
        {
            set
            {
                Model1Container cxt = new Model1Container();
                agente = cxt.Agentes.FirstOrDefault(a => a.Id == value.Id);
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            if (agente != null)
            {
                EstadoAgente estado = agente.ObtenerEstadoAgenteParaElDia(DateTime.Now, true);
                if (Directory.Exists(pathImagenesDisco + agente.Legajo))
                {
                    if (File.Exists(pathImagenesDisco + agente.Legajo + "\\Original.jpg"))
                    {
                        img_cuenta.ImageUrl = "~/Imagenes/" + agente.Legajo + "/Original.jpg";
                    }
                    else
                    {
                        img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                    }
                }
                else
                {
                    img_cuenta.ImageUrl = "~/Imagenes/UsrDefault.jpg";
                }

                if (agente.EstadoActual == "Natalicio")
                {
                    img_cumple.ImageUrl = "~/Imagenes/FrameCumple.png";
                }

                Agente u = ((Agente)Session["UsuarioLogueado"]);

                if (u.Perfil == PerfilUsuario.Guardia)
                {
                    if (agente.Legajo != u.Legajo)
                    {
                        if (ObtenerAutorizacion())
                        {
                            img_acceso.Width = 50;
                            img_acceso.Height = 50;
                            img_acceso.ImageUrl = "~/Imagenes/ok.png";
                        }
                        else
                        {
                            img_acceso.ImageUrl = "~/Imagenes/dont.png";
                        }
                    }
                }

                
            }
        }

        private bool ObtenerAutorizacion()
        {
            if (DateTime.Now.Hour > 13)
            {
                //TAMBIEN DEBERIA VERIFICAR QUE SE ENCUENTRE APROBADO EL HORARIO VESPERTINO
                HorarioVespertino hv = agente.HorariosVespertinos.FirstOrDefault(h => h.Dia == DateTime.Today);
                if (hv == null)
                {
                    if (Convert.ToInt32(agente.ObtenerHoraSalidaLaboral(DateTime.Today).Split(':')[0]) > DateTime.Now.Hour ||
                        (Convert.ToInt32(agente.ObtenerHoraSalidaLaboral(DateTime.Today).Split(':')[0]) == DateTime.Now.Hour &&
                        Convert.ToInt32(agente.ObtenerHoraSalidaLaboral(DateTime.Today).Split(':')[1]) > DateTime.Now.Minute)
                        )
                    { //el agente tiene horario de salida mayor al horario en el que esta tratando de entrar
                        return true;
                    }
                    else
                    {//el agente no tiene horario vespertino y no trabaja a la tarde
                        return false;
                    }
                }
                else
                {
                    if (hv.Estado == EstadosHorarioVespertino.Aprobado || hv.Estado == EstadosHorarioVespertino.Terminado)
                    {//el agente tiene un horario vespertino aprobado por el jefe
                        return true;
                    }
                    else
                    {//el agente no tiene horario vespertino aprobado.
                        return false;
                    }
                }
            }
            else
            {//es de mañana
                return true;
            }

        }


        public void Refrescar()
        {
            this.Agente = agente;
        }
    }
}