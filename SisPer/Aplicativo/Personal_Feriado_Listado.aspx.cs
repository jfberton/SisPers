using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace SisPer.Aplicativo
{
    public partial class Personal_Feriado_Listado : System.Web.UI.Page
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

                    CargarFeriados();
                }
            }
        }

        private void CargarFeriados()
        {
            Model1Container cxt = new Model1Container();
            var feriados = (from f in cxt.Feriados
                            select new
                            {
                                Id = f.Id,
                                Fecha = f.Dia,
                                Motivo = f.Motivo
                            }
                            ).OrderByDescending(i => i.Fecha);
                           
            var asuetos = (from ap in cxt.AsuetosParciales
                           group ap by new {ap.Dia, ap.HorarioQueModifica, ap.Hora} into grupo
                           select new
                           {
                               MasDatos = grupo.Key.HorarioQueModifica == "Entrada" ? "hasta las " + grupo.Key.Hora : "desde las " + grupo.Key.Hora,
                               Fecha = grupo.Key.Dia,
                               Motivo = grupo.FirstOrDefault()!=null? grupo.FirstOrDefault().Observacion:""
                           }).OrderByDescending(i => i.Fecha);
            
            GridView1.DataSource = feriados;
            GridView1.DataBind();

            GridView2.DataSource = asuetos;
            GridView2.DataBind();

            Agente ag = Session["UsuarioLogueado"] as Agente;
            btn_Agregar.Visible = ag.Perfil == PerfilUsuario.Personal;
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Feriado_Nuevo.aspx");
        }

        protected void btn_Editar_Click(object sender, ImageClickEventArgs e)
        {
            Session["Tipo"] = "Feriado";
            Session["Id"] = ((ImageButton)sender).CommandArgument;
            Response.Redirect("~/Aplicativo/Personal_Feriado_Nuevo.aspx");
        }

        protected void btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            string tipo = ((ImageButton)sender).CommandArgument.Split('_')[0];
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument.Split('_')[1]);
            Model1Container cxt = new Model1Container();
            try
            {
                if (tipo == "Feriado")
                {
                    Feriado f = cxt.Feriados.First(fer => fer.Id == id);
                    cxt.Feriados.DeleteObject(f);
                }
                else
                {
                    AsuetoParcial f = cxt.AsuetosParciales.First(ap => ap.Id == id);
                    cxt.AsuetosParciales.DeleteObject(f);
                }

                cxt.SaveChanges();

                CargarFeriados();
            }
            catch { }
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarFeriados();
        }

        protected void btn_editar_asueto_Click(object sender, ImageClickEventArgs e)
        {
            string valores = ((ImageButton)sender).CommandArgument;

            Session["Tipo"] = "Asueto";
            Session["Id"] = ((ImageButton)sender).CommandArgument;
            Response.Redirect("~/Aplicativo/Personal_Feriado_Nuevo.aspx");
           

        }

        protected void btn_eliminar_asueto_Click(object sender, ImageClickEventArgs e)
        {
            string idStr = ((ImageButton)sender).CommandArgument;
            DateTime dia = Convert.ToDateTime(idStr.Split('_')[0]);
            string hora = idStr.Split('_')[1].Replace("desde las ", "").Replace("hasta las ", "");
            string entrada_salida = idStr.Contains("desde") ? "Salida" : "Entrada";
            string observacion = idStr.Split('_')[2];

            using (var cxt = new Model1Container())
            {
                cxt.AsuetosParciales.Where(ap => ap.Dia == dia && ap.Hora == hora && ap.HorarioQueModifica == entrada_salida && ap.Observacion == observacion).ToList().ForEach(cxt.DeleteObject);
                cxt.SaveChanges();
            }

            CargarFeriados();
        }
    }
}