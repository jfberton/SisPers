using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Area_Nuevo : System.Web.UI.Page
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
                        //A editar
                        int id = Convert.ToInt32(Session["IdArea"]);
                        Session["IdArea"] = null;
                        Area area = cxt.Areas.FirstOrDefault(a => a.Id == id);
                        Session["Area"] = area;
                        CargarValoresArea();
                    }
                    else
                    {
                        Session["Area"] = null;
                    }
                }
            }
        }

        private void CargarValoresArea()
        {
            Area area = Session["Area"] as Area;
            Ddl_Areas1.AreaSeleccionado = area.DependeDe;
            tb_Nombre.Text = area.Nombre;
            chk_Interior.Checked = area.Interior == true;
        }

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                Area area = Session["Area"] as Area;
                if (area == null)
                { 
                    area = new Area();
                    cxt.Areas.AddObject(area);
                }

                area.DependeDe = Ddl_Areas1.AreaSeleccionado;
                area.Nombre = tb_Nombre.Text;
                area.Interior = chk_Interior.Checked;
                if (!area.Recursivo())
                {
                    cxt.SaveChanges();
                    Session["Area"] = null;
                    Response.Redirect("~/Aplicativo/Personal_Area_Listado.aspx");
                }
                else
                {
                    Controles.MessageBox.Show(this, "Verifique porque se produce un bucle entre con el area de donde depende el mismo", Controles.MessageBox.Tipo_MessageBox.Danger);
                }
            }
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Area_Listado.aspx");
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Ddl_Areas1.AreaSeleccionado != null;
        }
    }
}