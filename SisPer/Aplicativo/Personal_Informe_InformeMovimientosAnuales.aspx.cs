using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Informe_InformeMovimientosAnuales : System.Web.UI.Page
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

                    for (int i = 2014; i <= DateTime.Today.Year; i++)
                    {
                        ddl_anio.Items.Add(i.ToString());
                    }

                    rb_Legajo.Checked = true;
                    Panel_Legajo.Attributes.Clear();
                    Panel_Area.Attributes.Clear();
                    Panel_Legajo.Attributes.Add("class", "alert alert-info");

                    tb_Legajo.Enabled = true;
                    Ddl_Areas.Enabled = false;
                    chk_Dependencias.Enabled = false;
                }
            }
        }

        protected void btn_movimientosAnuales_Click(object sender, EventArgs e)
        {
            MovimientosAnuales_DS ds = ObtenerDatosReporte();
            
            Session["DS_MOVIMIENTOS"] = ds;

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/ListadoReporteMovimientosAnuales_r.rdlc");

            ReportDataSource encabezado = new ReportDataSource("Encabezado", ds.Encabezado.Rows);

            viewer.LocalReport.DataSources.Add(encabezado);

            viewer.LocalReport.SubreportProcessing += LocalReportSalidas_SubreportProcessing;

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            RegistrarImpresionReporte();
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private MovimientosAnuales_DS ObtenerDatosReporte()
        {
            int legajo;
            Area area;
            List<Agente> agentesBuscados = new List<Agente>();
            Session["AgentesInforme"] = agentesBuscados;
            MovimientosAnuales_DS ds = new MovimientosAnuales_DS();

            using (var cxt = new Model1Container())
            {
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
            }

            foreach (Agente ag in agentesBuscados)
            {
                ds = CargarValores(ds, ag);
            }

            return ds;
        }

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
        }

        protected void rb_CheckedAgenteChanged(object sender, EventArgs e)
        {
            if (rb_Legajo.Checked)
            {//seleccionado la busqueda por agente
                Panel_Legajo.Attributes.Add("class","alert alert-info");
                Panel_Area.Attributes.Clear();

                tb_Legajo.Enabled = true;
                Ddl_Areas.Enabled = false;
                chk_Dependencias.Enabled = false;
            }
            else
            {//seleccionado la busqueda por sector
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                Panel_Legajo.Attributes.Clear();
                Panel_Area.Attributes.Add("class","alert alert-info");

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

        private void LocalReportSalidas_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.MovimientosAnuales_DS ds = ((Reportes.MovimientosAnuales_DS)Session["DS_MOVIMIENTOS"]);

            e.DataSources.Add(new ReportDataSource("Encabezado", ds.Encabezado.Rows));
            e.DataSources.Add(new ReportDataSource("Movimientos", ds.MovimientosMes.Rows));
        }

        private MovimientosAnuales_DS CargarValores(MovimientosAnuales_DS ret, Agente ag)
        {
            using (var cxt = new Model1Container())
            {
                ag = cxt.Agentes.FirstOrDefault(x => x.Id == ag.Id);
                MovimientosAnuales_DS.EncabezadoRow dre = ret.Encabezado.NewEncabezadoRow();

                dre.Anio = ddl_anio.SelectedValue;
                dre.Apartartado = ag.Legajo_datos_laborales.Apartado.Length > 0 ? ag.Legajo_datos_laborales.Apartado : " - ";
                dre.ApellidoyNombre = ag.ApellidoYNombre;
                dre.Clases = ag.Legajo_datos_personales.FechaNacimiento.Year.ToString();
                dre.Domicilio = ag.Legajo_datos_personales.Domicilio.Length > 0 ? ag.Legajo_datos_personales.Domicilio : " - ";
                dre.FechaIngreso = ag.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                dre.Grupo = ag.Legajo_datos_laborales.Grupo.Length > 0 ? ag.Legajo_datos_laborales.Grupo : " - ";
                dre.Legajo = ag.Legajo.ToString();
                dre.MI = ag.Legajo_datos_personales.DNI.Length > 0 ? ag.Legajo_datos_personales.DNI : " - ";
                dre.Puntaje = ag.Legajo_datos_laborales.Cargo.Length > 0 ? ag.Legajo_datos_laborales.Cargo : " - ";
                ret.Encabezado.Rows.Add(dre);

                for (int i = 1; i <= 12; i++)
                {
                    MovimientosAnuales_DS.MovimientosMesRow dr = ret.MovimientosMes.NewMovimientosMesRow();
                    dr[0] = new DateTime(DateTime.Today.Year, i, 1).ToString("MMMM");
                    int totalDias = 0;
                    for (int j = 1; j <= 31; j++)
                    {
                        DateTime dia = new DateTime();
                        if (DateTime.TryParse(j.ToString() + "/" + i.ToString() + "/" + DateTime.Today.Year.ToString(), out dia))
                        {
                            EstadoAgente ea = ag.ObtenerEstadoAgenteParaElDia(dia);
                            dr[j] = (ea != null && ea.TipoEstado.Codigo.HasValue) ? ea.TipoEstado.Codigo.Value.ToString() : "";
                            if (!((ea != null && ea.TipoEstado.Codigo.HasValue) &&
                                   (ea.TipoEstado.Codigo.Value == 25 ||
                                    ea.TipoEstado.Codigo.Value == 5 ||
                                    ea.TipoEstado.Codigo.Value == 12 ||
                                    ea.TipoEstado.Codigo.Value == 14)))
                            {
                                totalDias = totalDias + 1;
                            }
                        }
                        else
                        {
                            dr[j] = "X";
                        }
                    }

                    dr[32] = totalDias.ToString();
                    dr[33] = ag.Legajo.ToString();

                    ret.MovimientosMes.Rows.Add(dr);
                }
            }

            return ret;
        }

        private MovimientosAnuales_DS CargarValoresPrueba(MovimientosAnuales_DS ret, string legajo)
        {
            Random r = new Random((int)DateTime.Now.Ticks);

            MovimientosAnuales_DS.EncabezadoRow dre = ret.Encabezado.NewEncabezadoRow();
            dre.Anio = DateTime.Now.Year.ToString();
            dre.Apartartado = r.Next(30, 150).ToString();
            dre.ApellidoyNombre = "asldjasldasldjaslkdjslkd";
            dre.Clases = "1990";
            dre.Domicilio = "sdfjdlfkajld asldkjasld asd11";
            dre.FechaIngreso = "15/15/15";
            dre.Grupo = "A - " + r.Next(1, 20).ToString();
            dre.Legajo = legajo;
            dre.MI = r.Next(20000000, 40000000).ToString();
            dre.Puntaje = r.Next(10, 100).ToString();
            ret.Encabezado.Rows.Add(dre);

            for (int i = 1; i <= 12; i++)
            {
                MovimientosAnuales_DS.MovimientosMesRow dr = ret.MovimientosMes.NewMovimientosMesRow();
                dr[0] = new DateTime(DateTime.Today.Year, i, 1).ToString("MMMM");
                int totalDias = 0;
                for (int j = 1; j <= 31; j++)
                {
                    r = new Random(j * (int)DateTime.Now.Ticks);
                    DateTime dia = new DateTime();
                    if (DateTime.TryParse(j.ToString() + "/" + i.ToString() + "/" + DateTime.Today.Year.ToString(), out dia))
                    {
                        int inasistencia = r.Next(1, 100);
                        dr[j] = inasistencia < 26 ? inasistencia.ToString() : "";
                        if (inasistencia != 14 && inasistencia != 25)
                        {
                            totalDias = totalDias + 1;
                        }
                    }
                    else
                    {
                        dr[j] = "X";
                    }
                }

                dr[32] = totalDias.ToString();
                dr[33] = legajo;

                ret.MovimientosMes.Rows.Add(dr);
            }

            return ret;
        }

        private void RegistrarImpresionReporte()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "MOVIMIENTOS ANUALES", DateTime.Now, nombreMaquina, localIP);
        }

        protected void cv_Legajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int legajo = 0;
            args.IsValid = rb_Legajo.Checked ? int.TryParse(tb_Legajo.Text, out legajo) : true;
        }
    }
}