using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class Ddl_TipoEstadoAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarDatos();
            }
        }

        private string textoItemNulo = "Ninguno";

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
                Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                bool jefe = 
                    agenteLogueado.Perfil ==PerfilUsuario.Agente &&
                    agenteLogueado.Jefe;
                bool personal = agenteLogueado.Perfil == PerfilUsuario.Personal;
                Model1Container cxt = Session["CXT"] as Model1Container;
                List<ItemList> lista = new List<ItemList>();
                ItemList item = new ItemList(0, textoItemNulo);
                lista.Add(item);
                var items = (from pp in cxt.TiposEstadoAgente
                            where 
                            (pp.MarcaJefe && jefe) || 
                            (pp.MarcaPersonal && personal)
                            select new { Id = pp.Id, Valor = pp.Estado }).OrderBy(i=>i.Valor);

                foreach (var i in items)
                {
                    lista.Add(new ItemList(i.Id, i.Valor));
                }

                ddlTipoEstado.DataValueField = "Id";
                ddlTipoEstado.DataTextField = "Valor";
                ddlTipoEstado.DataSource = lista;
                ddlTipoEstado.DataBind();
            }
            catch { }
        }

        /// <summary>
        /// Obtiene o establece el tipo estado agente seleccionado. Devuelve null si esta seleccionado "Ninguno"
        /// </summary>
        public TipoEstadoAgente EstadoSeleccionado
        {
            get
            {
                Model1Container cxt = new Model1Container();
                int id = Convert.ToInt32(ddlTipoEstado.SelectedValue);
                return cxt.TiposEstadoAgente.FirstOrDefault(p => p.Id == id);
            }
            set
            {
                try
                {
                    ddlTipoEstado.SelectedValue = value.Id.ToString();
                }
                catch
                {
                    ddlTipoEstado.SelectedValue = "0";
                }
            }
        }

        /// <summary>
        ///String Id del tipo estado seleccionado
        /// </summary>
        public string SeleccionarTipoEstado
        {
            set
            {
                try
                {
                    ddlTipoEstado.SelectedValue = value;
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
                return ddlTipoEstado.Enabled;
            }
            set
            {
                ddlTipoEstado.Enabled = value;
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

        public event EventHandler SelectedItemChanged;

        protected void ddlTipoEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedItemChanged != null)
                this.SelectedItemChanged(this, e);   
        }
    }
}