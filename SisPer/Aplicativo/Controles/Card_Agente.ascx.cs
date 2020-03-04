using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class Card_Agente : System.Web.UI.UserControl
    {
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        Agente agente = null;

        public Agente Agente
        {
            get
            {
                Model1Container cxt = new Model1Container();
                int id = lbl_Id.Text != null ? Convert.ToInt32(lbl_Id.Text) : 0;
                agente = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                return agente;
            }
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
                lbl_Id.Text = agente.Id.ToString();
                lbl_ApyNom.Text = agente.ApellidoYNombre;
                lbl_Legajo.Text = agente.Legajo.ToString();
                lbl_Email.Text = agente.Legajo_datos_laborales.Email;
                lbl_DNI.Text = agente.Legajo_datos_personales.DNI;
                lbl_FechIngreso.Text = agente.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                lbl_FechNac.Text = agente.Legajo_datos_personales.FechaNacimiento.ToShortDateString();
                ImagenAgente1.Agente = agente;
            }
        }

        public void Refrescar()
        {
            this.Agente = agente;
        }
    }
}