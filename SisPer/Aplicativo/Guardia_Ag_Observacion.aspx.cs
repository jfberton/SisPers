using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Guardia_Ag_Observacion : System.Web.UI.Page
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

                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;
                
                if (Session["IdAg"] != null)
                {
                    int id = Convert.ToInt32(Session["IdAg"]);
                    Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                    Session["IdAg"] = null;
                    Session["Agente"] = ag;
                    CargarValoresAgente();
                }
                else
                {
                    Session["Agente"] = null;
                }
            }
        }

        private void CargarValoresAgente()
        {
            Agente ag = Session["Agente"] as Agente;
            DatosAgente1.Agente = ag;
            var observaciones = from obs in ag.ObservacionesGuardia
                                select new { obs.Id, obs.Fecha };
            GridView1.DataSource = observaciones.OrderByDescending(a => a.Fecha).ToList();
            GridView1.DataBind();
            tb_Observacion.Text = string.Empty;
            tb_Observacion.ReadOnly = true;
            btn_GuardarObs.Enabled = false;
        }

        protected void btn_Ver_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Model1Container cxt = new Model1Container();
            ObservacionGuardia obs = cxt.ObservacionesGuardia.FirstOrDefault(og=>og.Id == id);
            tb_Observacion.Text = obs != null ? obs.Observacion : "";
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarValoresAgente();
        }

        protected void btn_NuevaObs_Click(object sender, EventArgs e)
        {
            tb_Observacion.Text = string.Empty;
            tb_Observacion.ReadOnly = false;
            btn_GuardarObs.Enabled = true;
        }

        protected void btn_GuardarObs_Click(object sender, EventArgs e)
        {
            if (tb_Observacion.Text.Length > 0)
            {
                btn_GuardarObs.Enabled = false;
                Model1Container cxt = Session["CXT"] as Model1Container;
                Agente ag = Session["Agente"] as Agente;
                ObservacionGuardia obs = new ObservacionGuardia()
                {
                    Fecha = DateTime.Now,
                    Observacion = tb_Observacion.Text,
                    AgenteId = ag.Id
                };
                cxt.ObservacionesGuardia.AddObject(obs);
                cxt.SaveChanges();
                Controles.MessageBox.Show(this, "La observación se guardó exitosamente!", Controles.MessageBox.Tipo_MessageBox.Success,"Genial!");
                CargarValoresAgente();
                
            }
            else 
            {
                Controles.MessageBox.Show(this,"No existe observación por guardar!", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }
    }
}