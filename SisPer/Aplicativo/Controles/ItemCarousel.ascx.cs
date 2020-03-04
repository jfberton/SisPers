using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class ItemCarousel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.ComponentModel.Description("Dirección en donde se encuentra el video.")]
        [System.ComponentModel.Browsable(true)]
        public string Href
        {
            get { return video.HRef; }
            set
            {
                video.HRef = value;
                
            }
        }

        public string Titulo
        {
            get { return lbl_titulo.Text; }
            set { lbl_titulo.Text = value; }
        }

        public string Subtitulo
        {
            get { return lbl_subtitulo.Text; }
            set { lbl_subtitulo.Text = value; }
        }

        public string Subtitulo2
        {
            get { return lbl_Subtitulo2.Text; }
            set { lbl_Subtitulo2.Text = value; }
        }

        public string Descripcion
        {
            get { return lbl_descrip.Text; }
            set { lbl_descrip.Text = value; }
        }
    }
}