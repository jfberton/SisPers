using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Area_Listado : System.Web.UI.Page
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
                    MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                    CargarGrilla();
                    Session["Filtro"] = string.Empty;
                }
            }
        }

        private void CargarGrilla()
        {
            Session["CXT"] = new Model1Container();
            Model1Container cxt = Session["CXT"] as Model1Container;
            var items = (from a in cxt.Areas
                        select new
                        {
                           Id = a.Id,
                           Nombre = a.Nombre
                        }).ToList();

            string filtro = Session["Filtro"] != null ? Session["Filtro"].ToString() : String.Empty;

            //if (Page.Form != null)
            //{
            //    Page.Form.DefaultButton = btn_Buscar.UniqueID;
            //}

            var itemsFiltrados = (from i in items
                                  where Cadena.Normalizar(i.Nombre.ToUpper()).Contains(filtro)
                                  select i).ToList();
            
            GridView1.DataSource = itemsFiltrados;
            GridView1.DataBind();
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Area_Nuevo.aspx");
        }

        protected void btn_Editar_Click(object sender, ImageClickEventArgs e)
        {
            Session["IdArea"] = ((ImageButton)sender).CommandArgument;
            Response.Redirect("~/Aplicativo/Personal_Area_Nuevo.aspx");
        }

        protected void btn_Detalle_Click(object sender, ImageClickEventArgs e)
        {
            Session["IdArea"] = ((ImageButton)sender).CommandArgument;
            Response.Redirect("~/Aplicativo/Personal_Area_Detalle.aspx");
        }

        protected void btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idArea = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Area area = cxt.Areas.FirstOrDefault(a => a.Id == idArea);
            if (area != null && (area.Agentes.Where(a => a.FechaBaja == null).Count() > 0 || area.Subordinados.Count > 0))
            {
                Controles.MessageBox.Show(this, "El area que desea eliminar posee agentes o areas subordinadas, no se puede eliminar", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (area != null)
                {
                    cxt.Areas.DeleteObject(area);
                    cxt.SaveChanges();
                    Controles.MessageBox.Show(this, "EL area se eliminó correctamente", Controles.MessageBox.Tipo_MessageBox.Success);

                }
            }
            
            CargarGrilla();
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            Session["Filtro"] = Cadena.Normalizar(tb_Busqueda.Value.ToUpper());
            CargarGrilla();
        }
    }
}