using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class Licencia : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RefrescarValores();
        }

        public void RefrescarValores()
        {
            lbl_Tipo.Text = Tipo;
            lbl_DiasOtorgados.Text = DiasOtorgados.ToString();
            lbl_DiasUsufructuados.Text = DiasUsufructuados.ToString();
            lbl_Saldo.Text = (DiasOtorgados - DiasUsufructuados).ToString();

            btn_Edit_DiasOtorgados.Visible = HabilitarEdicion;
            btn_Edit_DiasUsufructuados.Visible = HabilitarEdicion;
        }

        
        public int DiasOtorgados
        {
            get
            {
                return Convert.ToInt32(lbl_DiasOtorgados.Text);
            }
            set
            {
                lbl_DiasOtorgados.Text = value.ToString();
            }
        }
        public int DiasUsufructuados
        {
            get
            {
                return Convert.ToInt32(lbl_DiasUsufructuados.Text);
            }
            set
            {
                lbl_DiasUsufructuados.Text = value.ToString();
            }
        }
        public string Tipo
        {
            get
            {
                return lbl_Tipo.Text;
            }
            set
            {
                lbl_Tipo.Text = value;
            }
        }
        public bool HabilitarEdicion { get; set; }

        private void OcultarControlesEdicion()
        {
            btn_Accept_DiasOtorgados.Visible = false;
            btn_Cancel_DiasOtorgados.Visible = false;

            btn_Accept_DiasUsufructuados.Visible = false;
            btn_Cancel_DiasUsufructuados.Visible = false;

            lbl_DiasOtorgados.Visible = true;
            lbl_DiasUsufructuados.Visible = true;

            tb_DiasOtorgados.Visible = false;
            tb_DiasUsufructuados.Visible = false;
            tb_DiasUsufructuados.Text = string.Empty;

            RefrescarValores();
        }

        public event EventHandler ModificoValor;

        #region Días disponibles

        protected void btn_Accept_DiasOtorgados_Click(object sender, EventArgs e)
        {
            Page.Validate("DiasOtorgados");

            if (Page.IsValid)
            {
                OcultarControlesEdicion();

                if (ModificoValor != null)
                    ModificoValor(sender, e);
            }
        }

        protected void btn_Cancel_DiasOtorgados_Click(object sender, EventArgs e)
        {
            OcultarControlesEdicion();
        }

        protected void btn_Edit_DiasOtorgados_Click(object sender, EventArgs e)
        {
            btn_Accept_DiasOtorgados.Visible = true;
            btn_Cancel_DiasOtorgados.Visible = true;

            lbl_DiasOtorgados.Visible = false;
            tb_DiasOtorgados.Text = lbl_DiasOtorgados.Text;
            tb_DiasOtorgados.Visible = true;

            btn_Edit_DiasOtorgados.Visible = false;
        }

        protected void cv_DiasOtorgados_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int dias = 0;

            bool ok = int.TryParse(tb_DiasOtorgados.Text, out dias);

            if (ok)
            {
                lbl_DiasOtorgados.Text = dias.ToString();
            }

            args.IsValid = true;

        }

        #endregion

        #region Días usufructuados

        protected void btn_Accept_DiasUsufructuados_Click(object sender, EventArgs e)
        {
            Page.Validate("DiasUsufructuados");

            if (Page.IsValid)
            {
                OcultarControlesEdicion();

                if (ModificoValor != null)
                    ModificoValor(sender, e);
            }
        }

        protected void btn_Cancel_DiasUsufructuados_Click(object sender, EventArgs e)
        {
            OcultarControlesEdicion();
        }

        protected void btn_Edit_DiasUsufructuados_Click(object sender, EventArgs e)
        {
            btn_Accept_DiasUsufructuados.Visible = true;
            btn_Cancel_DiasUsufructuados.Visible = true;

            lbl_DiasUsufructuados.Visible = false;
            tb_DiasUsufructuados.Text = lbl_DiasUsufructuados.Text;
            tb_DiasUsufructuados.Visible = true;
            btn_Edit_DiasUsufructuados.Visible = false;
        }

        protected void cv_DiasUsufructuados_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int dias = 0;

            bool ok = int.TryParse(tb_DiasUsufructuados.Text, out dias);

            if (ok)
            {
                lbl_DiasUsufructuados.Text = dias.ToString();
            }

            args.IsValid = true;
        }

        #endregion





    }
}