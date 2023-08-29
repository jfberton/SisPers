using Microsoft.Reporting.WebForms;
using Microsoft.SqlServer.Server;
using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SisPer.Aplicativo
{
    public partial class Personal_Informe_Domicilios_Agente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                Ddl_Areas.Enabled = false;
                chk_Dependencias.Enabled = false;

                Panel_Legajo.Attributes.Add("class", "alert alert-info");
            }
        }

        protected void rb_CheckedAgenteChanged(object sender, EventArgs e)
        {
            if (rb_Legajo.Checked)
            {//seleccionado la busqueda por agente
                Panel_Legajo.Attributes.Add("class", "alert alert-info");
                Panel_Area.Attributes.Clear();

                tb_Legajo.Enabled = true;
                Ddl_Areas.Enabled = false;
                chk_Dependencias.Enabled = false;
            }
            else
            {//seleccionado la busqueda por sector
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                Panel_Legajo.Attributes.Clear();
                Panel_Area.Attributes.Add("class", "alert alert-info");

                tb_Legajo.Text = string.Empty; tb_Legajo.Enabled = false;
                if (usuarioLogueado.Perfil != PerfilUsuario.Personal)
                {
                    Ddl_Areas.Enabled = false;
                    Ddl_Areas.SeleccionarArea = usuarioLogueado.AreaId.ToString();
                }
                else
                {// perfil personal, puede ver el area que quiera.
                    Ddl_Areas.Enabled = true;
                }

                chk_Dependencias.Enabled = true;

            }
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (rb_Legajo.Checked == true)
            {
                if (ControlarSiPuedeVerAlAgente())
                {
                    Generar_informe_cierres_mensuales();
                }
                else
                {
                    Controles.MessageBox.Show(this, "Existe un error en el legajo ingresado o el agente al que esta intentando acceder no depende de usted.", Controles.MessageBox.Tipo_MessageBox.Danger);
                }
            }
            else
            {
                Generar_informe_cierres_mensuales();
            }

        }

        private bool ControlarSiPuedeVerAlAgente()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            if (usuarioLogueado.Perfil != PerfilUsuario.Personal)
            {
                //el usuario puede ver unicamente al agente que dependa de el
                List<Agente> agentes = usuarioLogueado.ObtenerAgentesSubordinadosCascada();
                if (agentes.FirstOrDefault(a => a.Legajo.ToString() == tb_Legajo.Text) != null || tb_Legajo.Text == usuarioLogueado.Legajo.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //el usuario esta logueado como personal puede ver a quien se le antoje
                return true;
            }
        }


        /// <summary>
        /// Aqui se ecuentran las funciones que generan los distintos tipos de informes 
        /// </summary>
        #region Generacion de informes

        private void IncluirAgentes(Area area)
        {
            List<Agente> agentes = Session["AgentesInforme"] as List<Agente>;

            foreach (Agente ag in area.Agentes.Where(a => a.FechaBaja == null))
            {
                agentes.Add(ag);
            }

            if (chk_Dependencias.Checked)
            {
                foreach (Area areaSubordinada in area.Subordinados)
                {
                    IncluirAgentes(areaSubordinada);
                }
            }

            Session["AgentesInforme"] = agentes;
        }

        #region Informe de cierres mensuales

        public struct Informe_cierres_agente
        {
            public string Nombre { get; set; }
            public string Legajo { get; set; }
            public string Area { get; set; }
            public string Periodo_str { get; set; }
            public string Horas_bonificcion { get; set; }
            public int Dias_sin_cerrar { get; set; }
            public string Horas_acumuladas { get; set; }
            public int Periodo_int { get; set; }
            public string Hora_mes { get; set; }
            public string Hora_año_ant { get; set; }
            public string Hora_año_act { get; set; }
            public string Fecha_actualizacion { get; set; }
        }

        private void Generar_informe_cierres_mensuales()
        {
            Validate();
            if (IsValid)
            {

                byte[] bytes = null;

                #region Obtener datos cargar el DataSet
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                List<Agente> agentesBuscados = new List<Agente>();
                Session["AgentesInforme"] = agentesBuscados;
                Area area; int legajo;

                if (rb_Legajo.Checked)
                {//seleccionado la busqueda por agente
                    legajo = Convert.ToInt32(tb_Legajo.Text);
                    Agente ag = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo);
                    if (ag != null)
                    {
                        agentesBuscados.Add(ag);
                    }
                }
                else
                {//seleccionado la busqueda por sector
                    area = Ddl_Areas.AreaSeleccionado;
                    IncluirAgentes(area);
                    agentesBuscados = Session["AgentesInforme"] as List<Agente>;
                }

                #endregion

                if (agentesBuscados.Count > 0)
                {
                    Informe_domicilios_agentes reporte = new Informe_domicilios_agentes(agentesBuscados);
                    bytes = reporte.Generar_informe();
                }

                if (bytes != null)
                {
                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                else
                {
                    Controles.MessageBox.Show(this, "La búsqueda realizada no arrojó resultados", Controles.MessageBox.Tipo_MessageBox.Info);
                }
            }
        }

        #endregion

        protected void cv_Legajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int legajo = 0;
            args.IsValid = rb_Legajo.Checked ? int.TryParse(tb_Legajo.Text, out legajo) : true;
        }

        #endregion

    }
}