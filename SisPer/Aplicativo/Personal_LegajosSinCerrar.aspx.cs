using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_LegajosSinCerrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
                    if (usuariologueado.Jefe || usuariologueado.JefeTemporal)
                    {
                        CargarValoresIniciales();
                    }
                    else
                    {
                        Response.Redirect("../default.aspx?mode=trucho");
                    }
                }
            }
        }

        private void CargarValoresIniciales()
        {
            ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;
            CargarDDLAnio();
        }

        private void CargarDDLAnio()
        {
            for (int i = 2013; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

        internal struct ItemGrilla
        {
            public int Legajo { get; set; }
            public string Nombre { get; set; }
            public int DiasPorCerrar { get; set; }
        }

        private void CargarLegajosSinCerrar()
        {
            List<ItemGrilla> items = new List<ItemGrilla>();
            int mes = ddl_Mes.SelectedIndex + 1;
            int año = Convert.ToInt32(ddl_Anio.Text);
            int legajoDesde = 0;
            int legajoHasta = 0;

            int.TryParse(tb_legajoDesde.Text, out legajoDesde);
            int.TryParse(tb_legajoHasta.Text, out legajoHasta);

            using (var cxt = new Model1Container())
            {
                var agentes = (from ag in cxt.Agentes
                               where ag.Legajo >= legajoDesde && ag.Legajo <= legajoHasta && ag.FechaBaja == null
                               select ag).OrderBy(l => l.Legajo);
                
                foreach (Agente ag in agentes)
                {
                    ItemGrilla item = new ItemGrilla();
                    item.Legajo = ag.Legajo;
                    item.Nombre = ag.ApellidoYNombre;
                    int diasPorCerrar = 0;

                    DateTime desde = new DateTime(año, mes, 1);
                    DateTime hasta = new DateTime(año, mes, DateTime.DaysInMonth(año, mes));
                    hasta = (hasta > DateTime.Today) ? DateTime.Today : hasta;

                    for (DateTime dia = desde; dia <= hasta; dia = dia.AddDays(1))
                    {
                        if (ag.ObtenerEstadoAgenteParaElDia(dia) == null)
                        {
                            ResumenDiario rd = ag.ObtenerResumenDiario(dia);
                            diasPorCerrar = diasPorCerrar + ((rd != null && (rd.Cerrado ?? false) == true) ? 0 : 1);
                        }
                    }
                    
                    item.DiasPorCerrar = diasPorCerrar;
                    items.Add(item);
                }
            }

            gv_legajossincerrar.DataSource = items;
            gv_legajossincerrar.DataBind();
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            if (Controlar())
            {
                CargarLegajosSinCerrar();
                tb_legajoDesde.Enabled = false;
                tb_legajoHasta.Enabled = false;
                ddl_Anio.Enabled = false;
                ddl_Mes.Enabled = false;
                p_Grilla.Visible = true;
                btn_buscar.Enabled = false;
                btn_NuevaBusqueda.Visible = true;
            }
            else
            {
                Controles.MessageBox.Show(this,"Hay un error en los legajos ingresados, por favor verifique.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        private bool Controlar()
        {
            int legajoDesde = 0;
            int legajoHasta = 0;

            return int.TryParse(tb_legajoDesde.Text, out legajoDesde) && int.TryParse(tb_legajoHasta.Text, out legajoHasta) && legajoDesde <= legajoHasta;
        }

        protected void btn_NuevaBusqueda_Click(object sender, EventArgs e)
        {
            gv_legajossincerrar.DataSource = null;
            gv_legajossincerrar.DataBind();
            tb_legajoDesde.Enabled = true;
            tb_legajoHasta.Enabled = true;
            ddl_Anio.Enabled = true;
            ddl_Mes.Enabled = true;
            p_Grilla.Visible = false;
            p_AgenteSeleccionado.Visible = false;
            btn_NuevaBusqueda.Visible = false;
            btn_buscar.Enabled = true;
        }

        internal struct itemDiaAgente
        {
            public int NumeroDia { get; set; }
            public DateTime DiasPorCerrar { get; set; }
        }

        protected void btn_verDiasPorCerrar_Click(object sender, ImageClickEventArgs e)
        {
            int mes = ddl_Mes.SelectedIndex + 1;
            int año = Convert.ToInt32(ddl_Anio.Text);
            int legajo = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            List<itemDiaAgente> diasPorCerrar = new List<itemDiaAgente>();
            var cxt = new Model1Container();
            Agente ag = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo);

            lbl_agente.Text = ag.Legajo.ToString() + " - " + ag.ApellidoYNombre;

            DateTime desde = new DateTime(año, mes, 1);
            DateTime hasta = new DateTime(año, mes, DateTime.DaysInMonth(año, mes));
            hasta = (hasta > DateTime.Today) ? DateTime.Today : hasta;
            int numeroDia = 0;
            for (DateTime dia = desde; dia <= hasta; dia = dia.AddDays(1))
            {
                if (ag.ObtenerEstadoAgenteParaElDia(dia) == null)
                {
                    ResumenDiario rd = ag.ObtenerResumenDiario(dia);
                    if(!(rd != null && (rd.Cerrado ?? false) == true))
                    {
                        numeroDia++;
                        diasPorCerrar.Add(new itemDiaAgente() { DiasPorCerrar = dia, NumeroDia = numeroDia });
                    }
                }
            }

            gv_diasAgente.DataSource = diasPorCerrar;
            gv_diasAgente.DataBind();

            p_AgenteSeleccionado.Visible = true;
        }

        protected void gv_legajossincerrar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_legajossincerrar.PageIndex = e.NewPageIndex;
            CargarLegajosSinCerrar();
        }

        protected void btn_volver_Click(object sender, EventArgs e)
        {
            gv_diasAgente.DataSource = null;
            lbl_agente.Text = string.Empty;
            p_AgenteSeleccionado.Visible = false;
        }

        protected void btn_exportar_Click(object sender, EventArgs e)
        {
            if (gv_legajossincerrar.Rows.Count > 0 && gv_legajossincerrar.Visible == true)
            {
                GridViewExportUtil.Export("Dias por cerrar - " + DateTime.Now.ToString("yyyy-MM-dd hhmmss") + ".xls", gv_legajossincerrar, "Legajo desde " + tb_legajoDesde.Text + " legajo hasta " + tb_legajoHasta.Text + " periodo " + ddl_Mes.Text + " de " + ddl_Anio.Text);
            }
        }
    }
}