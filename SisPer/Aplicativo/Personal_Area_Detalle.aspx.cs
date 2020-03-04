using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Area_Detalle : System.Web.UI.Page
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

                    Model1Container cxt = new Model1Container();
                    Session["CXT"] = cxt;

                    if (Session["IdArea"] != null)
                    {
                        int id = Convert.ToInt32(Session["IdArea"]);
                        Session["IdArea"] = null;
                        Area area = cxt.Areas.FirstOrDefault(a => a.Id == id);
                        Session["Area"] = area;
                        CargarValoresArea();
                    }
                    else
                    {
                        Session["Area"] = null;
                        Response.Redirect("~/Aplicativo/Personal_Area_Listado.aspx");
                    }
                }
            }
        }

        private void CargarValoresArea()
        {
            Area area = Session["Area"] as Area;
            lbl_DependeDe.Text = area.DependeDe != null ? area.DependeDe.Nombre : "Ninguno";
            lbl_NombreUnidadOrg.Text = area.Nombre;
            lbl_InteriorExterior.Text = area.Interior == true ? "La unidad funciona fuera del edificio central" : "";
            alertInterior.Visible = area.Interior == true ? true : false;
            lbl_CantAgentes.Text = area.Agentes.Where(a => a.FechaBaja == null).Count().ToString();
            lbl_CantidadSubordinadas.Text = area.Subordinados.Count.ToString();
            CargarGrillaAgentes();
            CargarGrillaSubordinados();
        }

        private void CargarGrillaAgentes()
        {
            Area area = Session["Area"] as Area;
            var items = (from ag in area.Agentes
                         where ag.FechaBaja==null
                         select new
                         {
                             Jefe = ag.Jefe ? "Jefe" : ag.JefeTemporal ? "Temporario" : "",
                             Nombre = ag.ApellidoYNombre,
                             Legajo = ag.Legajo
                         }).OrderBy(i=>i.Legajo).ToList();
            GridViewAgentes.DataSource = items;
            GridViewAgentes.DataBind();
        }

        private void CargarGrillaSubordinados()
        {
            Area area = Session["Area"] as Area;
            Model1Container cxt = Session["CXT"] as Model1Container;
            var items = (from ar in area.Subordinados
                        select new
                        {
                            Nombre = ar.Nombre
                        }).ToList();
            GridViewAreas.DataSource = items;
            GridViewAreas.DataBind();
        }

        protected void gridViewAgente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAgentes.PageIndex = e.NewPageIndex;
            CargarGrillaAgentes();
        }

        protected void gridViewArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAreas.PageIndex = e.NewPageIndex;
            CargarGrillaSubordinados();
        }
    }
}