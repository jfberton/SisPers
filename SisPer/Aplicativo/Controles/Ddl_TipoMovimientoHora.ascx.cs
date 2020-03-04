using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class Ddl_TipoMovimientoHora : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarDatos();
            }
        }

        private string textoItemNulo = "Ninguno";
        private bool muestraManuales = false;

        struct ItemList
        {
            private int id;
            public int Id
            {
                set { id = value; }
                get { return id; }
            }

            private string valor;
            public string Valor
            {
                set { valor = value; }
                get { return valor; }
            }
            /// <summary>
            /// Instancia un nuevo itemlist
            /// </summary>
            /// <param name="p">Id</param>
            /// <param name="q">Valor</param>
            public ItemList(int p, string q)
            {
                id = p;
                valor = q;
            }
        }

        private void CargarDatos()
        {
            try
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                List<ItemList> lista = new List<ItemList>();
                ItemList item = new ItemList(0, textoItemNulo);
                lista.Add(item);
                var items = from pp in cxt.TiposMovimientosHora
                            where (pp.Manual == muestraManuales || muestraManuales == false)
                            select new { Id = pp.Id, Valor = pp.Tipo };

                foreach (var i in items)
                {
                    lista.Add(new ItemList(i.Id, i.Valor));
                }
                DropDownList1.DataValueField = "Id";
                DropDownList1.DataTextField = "Valor";
                DropDownList1.DataSource = lista;
                DropDownList1.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Obtiene o establece el TipoMovimiento seleccionada. Devuelve null si esta seleccionado "Ninguno"
        /// </summary>
        public TipoMovimientoHora TipoMovimientoHoraSeleccionado
        {
            get
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                int id = Convert.ToInt32(DropDownList1.SelectedValue);
                return cxt.TiposMovimientosHora.FirstOrDefault(p => p.Id == id);
            }
            set
            {
                try
                {
                    DropDownList1.SelectedValue = value.Id.ToString();
                }
                catch
                {
                    DropDownList1.SelectedValue = "0";
                }
            }
        }

        /// <summary>
        ///String Id del TipoMovimientoHora seleccionado
        /// </summary>
        public string SeleccionarTipoMovimientoHora
        {
            set
            {
                try
                {
                    DropDownList1.SelectedValue = value;
                }
                catch { }
            }
        }

        /// <summary>
        /// Determina si el dropdown muestra únicamente los tipos de movimientos de horas que se pueden setear manualmente
        /// </summary>
        public bool MuestraManuales
        {
            get { return muestraManuales; }
            set { muestraManuales = value; CargarDatos(); }
        }

        /// <summary>
        /// Habilita o deshabilita el control
        /// </summary>
        public bool Enabled
        {
            get
            {
                return DropDownList1.Enabled;
            }
            set
            {
                DropDownList1.Enabled = value;
            }
        }

        /// <summary>
        /// Valor que determina cual será el texto del item nulo
        /// </summary>
        public string SetearTextoItemNullo
        {
            get
            {
                return textoItemNulo;
            }
            set
            {
                textoItemNulo = value;
                CargarDatos();
            }
        }

        public event System.EventHandler SeleccionoOtroItem;

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SeleccionoOtroItem != null)
                this.SeleccionoOtroItem(this, new EventArgs());
        }

    }
}