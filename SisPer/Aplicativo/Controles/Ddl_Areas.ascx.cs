using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class Ddl_Areas : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarDatos();
            }
        }

        private string textoItemNulo = " Ninguno";

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
                var items = from pp in cxt.Areas
                            select new { Id = pp.Id, Valor = pp.Nombre };

                foreach (var i in items)
                {
                    lista.Add(new ItemList(i.Id, i.Valor));
                }
                ddlAreas.DataValueField = "Id";
                ddlAreas.DataTextField = "Valor";
                ddlAreas.DataSource = lista.OrderBy(a=>a.Valor);
                ddlAreas.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Obtiene o establece el area seleccionada. Devuelve null si esta seleccionado "Ninguno"
        /// </summary>
        public Area AreaSeleccionado
        {
            get
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                int id = Convert.ToInt32(ddlAreas.SelectedValue);
                return cxt.Areas.FirstOrDefault(p => p.Id == id);
            }
            set
            {
                try
                {
                    ddlAreas.SelectedValue = value.Id.ToString();
                }
                catch {
                    ddlAreas.SelectedValue = "0";
                }
            }
        }

        /// <summary>
        ///String Id del area seleccionado
        /// </summary>
        public string SeleccionarArea
        {
            set
            {
                try
                {
                    ddlAreas.SelectedValue = value;
                }
                catch { }
            }
        }

        /// <summary>
        /// Habilita o deshabilita el control
        /// </summary>
        public bool Enabled
        {
            get
            {
                return ddlAreas.Enabled;
            }
            set
            {
                ddlAreas.Enabled = value;
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

        protected void ddlAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SeleccionoOtroItem != null)
                this.SeleccionoOtroItem(this, new EventArgs());
        }
        
    }
}