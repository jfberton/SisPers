using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Reportes
{
    public partial class AgentesPorAreaUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarGrilla();
                Session["Filtro"] = string.Empty;
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

            var itemsFiltrados = (from i in items
                                  where Cadena.Normalizar(i.Nombre.ToUpper()).Contains(filtro)
                                  select i).ToList().OrderBy(n=>n.Nombre);

            //GridView1.DataSource = itemsFiltrados;
            //GridView1.DataBind();
            ddl_Areas.DataTextField = "Nombre";
            ddl_Areas.DataValueField = "Id";
            ddl_Areas.DataSource = itemsFiltrados;
            ddl_Areas.DataBind();
        }

       
        protected void btn_Obtener_Click(object sender, EventArgs e)
        {
            IdUO.Value = ddl_Areas.SelectedValue;
            if (IdUO.Value != "0")
            {
                RenderReport();
            }
        }

        private void RenderReport()
        {
            #region anterior
            //ReportViewer viewer = new ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.EnableExternalImages = true;
            //viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/ListadoAgentesUO_rp.rdlc");

            //GenerarDS();

            //Reportes.ListadoAgentes_DS ds = Session["DS"] as Reportes.ListadoAgentes_DS;

            //ReportDataSource areas = new ReportDataSource("Areas", ds.Area.Rows);

            //viewer.LocalReport.DataSources.Add(areas);

            //viewer.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

            //Microsoft.Reporting.WebForms.Warning[] warnings = null;
            //string[] streamids = null;
            //string mimeType = null;
            //string encoding = null;
            //string extension = null;
            //string deviceInfo = null;
            //byte[] bytes = null;

            //deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            ////Render the report
            //RegistrarImpresionReporte();
            //bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            //Session["Bytes"] = bytes;

            //string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
            #endregion

            #region Actual
            byte[] bytes = null;

            GenerarDS();

            ListadoAgentes_DS ds = Session["DS"] as Reportes.ListadoAgentes_DS;

            if (ds.Area.Count > 0)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                Informe_listado_agentes reporte = new Informe_listado_agentes(ds, usuarioLogueado);
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
                Controles.MessageBox.Show(this.Page, "La búsqueda realizada no arrojó resultados", Controles.MessageBox.Tipo_MessageBox.Info);
            }
            #endregion
        }

        private void RegistrarImpresionReporte()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "AGENTES POR AREA", DateTime.Now, nombreMaquina, localIP);
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.ListadoAgentes_DS ds = Session["DS"] as Reportes.ListadoAgentes_DS;
            e.DataSources.Add(new ReportDataSource("Agentes", ds.Agentes.Rows));
        }

        private void GenerarDS()
        {
            int idUO = Convert.ToInt32(IdUO.Value);
            using (var cxt = new Model1Container())
            {
                Area area = cxt.Areas.FirstOrDefault(uo => uo.Id == idUO);
                if (area != null)
                {
                    Reportes.ListadoAgentes_DS ds = new Reportes.ListadoAgentes_DS();
                    ds = AgregarAgentesDelArea(area, ds);
                    Session["DS"] = ds;
                }
                else
                {
                    Session["DS"] = new Reportes.ListadoAgentes_DS();
                }
            }
        }

        private ListadoAgentes_DS AgregarAgentesDelArea(Area area, ListadoAgentes_DS ds)
        {
            Reportes.ListadoAgentes_DS.AreaRow dr = ds.Area.NewAreaRow();
            dr.Nombre = area.Nombre.ToUpper();
            dr.Depende_de = area.DependeDe != null ? area.DependeDe.Nombre : string.Empty;
            dr.CantidadAgentes = area.Agentes.Where(a => a.FechaBaja == null).Count();
            dr.CantidadAreasSubordinadas = area.Subordinados.Count;
            ds.Area.AddAreaRow(dr);

            var agentesJefesDelArea = from a in area.Agentes
                                      where (a.Jefe || a.JefeTemporal) && a.FechaBaja == null
                                      select a;

            foreach (Agente ag in agentesJefesDelArea)
            {
                Reportes.ListadoAgentes_DS.AgentesRow drag = ds.Agentes.NewAgentesRow();
                drag.Area = area.Nombre.ToUpper();
                drag.DNI = ag.Legajo_datos_personales.DNI;
                drag.Legajo = ag.Legajo.ToString();
                drag.Nombre = ag.ApellidoYNombre;
                drag.Jefe = true;
                ds.Agentes.AddAgentesRow(drag);
            }

            var restoDeLosAgentesDelArea = from a in area.Agentes
                                           where !a.Jefe && !a.JefeTemporal && a.FechaBaja == null
                                           select a;

            foreach (Agente ag in restoDeLosAgentesDelArea)
            {
                Reportes.ListadoAgentes_DS.AgentesRow drag = ds.Agentes.NewAgentesRow();
                drag.Area = area.Nombre.ToUpper();
                drag.DNI = ag.Legajo_datos_personales.DNI;
                drag.Legajo = ag.Legajo.ToString();
                drag.Nombre = ag.ApellidoYNombre;
                drag.Jefe = false;
                ds.Agentes.AddAgentesRow(drag);
            }

            if (chk_IncluyeDependencias.Checked)
            {
                foreach (Area item in area.Subordinados)
                {
                    AgregarAgentesDelArea(item, ds);
                }
            }

            return ds;
        }
    }
}