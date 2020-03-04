using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class ItemMensaje : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            lbl_mensaje.Text = Recortar(StripTagsRegex(Mensaje.Cuerpo), 40);
            lbl_fechaEnvio.Text = Mensaje.FechaEnvio.ToString("dd/MM/yyyy");
            lbl_EnviadoPor.Text = Recortar(Mensaje.Agente.ApellidoYNombre, 30);
            lbl_EnviadoPor.ToolTip = Mensaje.Agente.ApellidoYNombre;
            lbl_Asunto.Text = Recortar(Mensaje.Asunto, 20);
            if (Recibido)
            {
                if (Mensaje.Destinatarios.First(d => d.AgenteId == ag.Id).FechaLeido == null)
                {
                    lbl_Asunto.Font.Bold = true;
                    row_mensaje.Style.Add("background-color", "gainsboro");
                }
            }
        }

        public string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Mensaje que se muestra
        /// </summary>
        public Mensaje Mensaje { get; set; }

        /// <summary>
        /// determina si el mensaje en cuestion fue recibido o enviado
        /// </summary>
        public bool Recibido { get; set; }

        public event EventHandler AbrioElMensaje;

        /// <summary>
        /// Recorta el string de entrada y agrega "..." al final
        /// </summary>
        /// <param name="entrada">Texto de entrada</param>
        /// <param name="caracteres">Caracteres que tiene que tener al final</param>
        /// <returns></returns>
        private string Recortar(string entrada, int caracteres)
        {
            string ret = entrada;
            if (entrada != null)
            {
                if (caracteres > 0)
                {
                    if (caracteres - 3 > 0)
                    {
                        if (entrada.Length > caracteres - 3)
                        {
                            ret = entrada.Substring(0, caracteres-3) + "...";
                        }
                    }
                    else
                    {
                        if (entrada.Length > caracteres)
                        {
                            ret = entrada.Substring(0, caracteres);
                        }
                    }
                }
            }

            return ret;
        }

        protected void lbl_Asunto_Click(object sender, EventArgs e)
        {
            if (Recibido)
            {
                using (var cxt = new Model1Container())
                {
                    Agente ag = Session["UsuarioLogueado"] as Agente;
                    Mensaje msg = cxt.Mensajes.First(m => m.Id == Mensaje.Id);
                    Destinatario dest = msg.Destinatarios.First(d => d.AgenteId == ag.Id);

                    if (dest.FechaLeido == null)
                    {
                        dest.FechaLeido = DateTime.Now;
                        cxt.SaveChanges();
                    }
                }
            }

            Session["AbrioMensaje"] = Mensaje;
            Session["Recibido"] = Recibido;

            if (this.AbrioElMensaje != null)
                this.AbrioElMensaje(this, new EventArgs());

        }

    }
}
