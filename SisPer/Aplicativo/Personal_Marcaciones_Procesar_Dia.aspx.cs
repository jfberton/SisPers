using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Marcaciones_Procesar_Dia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente.Visible = !(ag.Jefe || ag.JefeTemporal);

                    int idAgente = Convert.ToInt32(Session["Id"]);
                    DateTime diaBuscado = Convert.ToDateTime(Session["d"]);

                    Model1Container cxt = new Model1Container();
                    Agente agenteBuscado = cxt.Agentes.FirstOrDefault(a => a.Id == idAgente);

                    AdministrarDiaAgente.DiaBuscado = diaBuscado;
                    AdministrarDiaAgente.AgenteBuscado = agenteBuscado;
                    AdministrarDiaAgente.ResumenDiarioBuscado = agenteBuscado.ObtenerResumenDiario(diaBuscado);
                    AdministrarDiaAgente.CargarValores();
                    AdministrarDiaAgente.Visible = true;
                }
            }
        }

        protected void AdministrarDiaAgente_CerroElDia(object sender, EventArgs e)
        {
            DateTime dia = AdministrarDiaAgente.DiaBuscado;
            Session["RD"] = AdministrarDiaAgente.ResumenDiarioBuscado;
            Response.Redirect("~/Aplicativo/Personal_Marcaciones_Procesar.aspx?d=" + dia.ToString("dd") + "&m=" + dia.ToString("MM") + "&a=" + dia.ToString("yyyy") + "&check=1");
        }

        protected void AdministrarDiaAgente_PrecionoVolver(object sender, EventArgs e)
        {
            DateTime dia = AdministrarDiaAgente.DiaBuscado;
            Session["RD"] = AdministrarDiaAgente.ResumenDiarioBuscado;
            Response.Redirect("~/Aplicativo/Personal_Marcaciones_Procesar.aspx?d=" + dia.ToString("dd") + "&m=" + dia.ToString("MM") + "&a=" + dia.ToString("yyyy") + "&check=1");
        }




    }
}