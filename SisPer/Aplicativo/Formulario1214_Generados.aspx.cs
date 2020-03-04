using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Formulario1214_Generados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Menues
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                
                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }


                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                #endregion

                CargarF1214();
            }
        }

        private void CargarF1214()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Model1Container cxt = new Model1Container();

            var items = (from ff in cxt.Formularios1214
                         where
                         //ff.Estado == Estado1214.Confeccionado &&
                         (usuarioLogueado.Perfil == PerfilUsuario.Personal || ff.GeneradoPor.Id == usuarioLogueado.Id)
                         select new
                         {
                             Numero = ff.Id,
                             Estado = ff.Estado,
                             Generadopor = ff.GeneradoPor.ApellidoYNombre,
                             Destino = ff.Destino,
                             Desde = ff.Desde,
                             Hasta = ff.Hasta,
                             Jefe = ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado) != null ? ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado).Agente.ApellidoYNombre : string.Empty,
                             Tareas = ff.TareasACumplir,
                             IdF1214 = ff.Id
                         }).ToList();

            gv_form1214.DataSource = items;
            gv_form1214.DataBind();

            if(usuarioLogueado.Perfil != PerfilUsuario.Personal)
            {
                gv_form1214.Columns[ObtenerColumna("Confeccionó")].Visible = false;
            }


        }

        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_form1214.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_form1214.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        protected void gv_1214_generados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_form1214.PageIndex = e.NewPageIndex;
            CargarF1214();
        }

       
        protected void btn_ver_Click(object sender, ImageClickEventArgs e)
        {
            int idf1214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Session["id214"] = idf1214;
            Response.Redirect("~/Aplicativo/Form1214_Nuevo.aspx");
        }
    }
}