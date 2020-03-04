using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Informe_Bonificaciones : System.Web.UI.Page
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

                    CargarValores();
                }
            }
        }

        private void CargarValores()
        {
            ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;
            CargarDDLAnio();
        }

        private void CargarDDLAnio()
        {
            for (int i = 2013; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarResultadoBusqueda();
        }

        private Reportes.CumplimientoBonificacion_DS Buscar()
        {
            using (var cxt = new Model1Container())
            {
                Reportes.CumplimientoBonificacion_DS ds = new Reportes.CumplimientoBonificacion_DS();
                int mes = ddl_Mes.SelectedIndex + 1;
                int año = Convert.ToInt32(ddl_Anio.Text);
                var bonificaciones = cxt.BonificacionesOtorgadas.Where(b => b.Anio == año && b.Mes == mes).OrderBy(d => d.Agente.Legajo);

                string mesStr = new DateTime(año, mes, 1).ToString("MMMM yyyy");
                Reportes.CumplimientoBonificacion_DS.GeneralRow gr = ds.General.NewGeneralRow();
                gr.Mes = mesStr;
                gr.Pie = "Listado general";
                ds.General.AddGeneralRow(gr);

                foreach (BonificacionOtorgada item in bonificaciones)
                {
                    Reportes.CumplimientoBonificacion_DS.DetalleRow dr = ds.Detalle.NewDetalleRow();

                    dr.Legajo = item.Agente.Legajo;
                    dr.ApellidoyNombre = item.Agente.ApellidoYNombre;
                    dr.HorasOtorgadas = item.HorasOtorgadas;
                    dr.HorasPorCumplir = item.HorasAdeudadas.Replace("-", "").Replace("000:00", "-");
                    dr.DiasLicencia = item.Agente.EstadosPorDiaAgente.Where(ea => ea.Dia.Month == mes && ea.Dia.Year == año && ea.TipoEstado.Estado.Contains("Licencia")).Count().ToString();
                    ds.Detalle.AddDetalleRow(dr);
                }
                return ds;
            }
        }

        private void CargarResultadoBusqueda()
        {
            Reportes.CumplimientoBonificacion_DS ds = Buscar();
            gv_bonificaciones.DataSource = ds.Detalle;
            gv_bonificaciones.DataBind();
            lbl_titulo_grilla.Visible = ds.Detalle.Rows.Count > 0;
            btn_Imprimir.Visible = ds.Detalle.Rows.Count > 0;
            btn_ImprimirAgentesDeudores.Visible = ds.Detalle.Rows.Count > 0;
        }

        /// <summary>
        /// Imprime el reporte con los datos generados
        /// </summary>
        /// <param name="todosLosAgentes">
        ///     Verdadero: imprime el reporte general con todos los agentes
        ///     Falso: imprime el reporte con los agentes que adeudan horas unicamente.
        /// </param>
        private void RenderReport(bool todosLosAgentes)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/CumplimientoBonificacion_r.rdlc");
            Reportes.CumplimientoBonificacion_DS ds = Buscar();

            //si no son todos los agentes elimino aquellos que no adeuden horas
            if (!todosLosAgentes)
            {
                List<Reportes.CumplimientoBonificacion_DS.DetalleRow> filasporeliminar = new List<Reportes.CumplimientoBonificacion_DS.DetalleRow>();
                foreach (Reportes.CumplimientoBonificacion_DS.DetalleRow dr in ds.Detalle.Rows)
                {
                    if (dr.HorasPorCumplir == "-")
                    {
                        filasporeliminar.Add(dr);
                    }
                }

                foreach (Reportes.CumplimientoBonificacion_DS.DetalleRow item in filasporeliminar)
                {
                    ds.Detalle.RemoveDetalleRow(item);
                }

                ds.Detalle.AcceptChanges();
                ds.General.Rows[0][1] = "Agentes deudores";
            }

            if (ds.Detalle.Rows.Count > 0)
            {
                ReportDataSource general = new ReportDataSource("General", ds.General.Rows);
                ReportDataSource detalle = new ReportDataSource("Detalle", ds.Detalle.Rows);

                viewer.LocalReport.DataSources.Add(general);
                viewer.LocalReport.DataSources.Add(detalle);

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
            else
            {
                Controles.MessageBox.Show(this, "La búsqueda realizada no arrojó resultados", Controles.MessageBox.Tipo_MessageBox.Info);
            }
        }

        private void RegistrarImpresionReporte()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "INFORME BONIFICACIONES", DateTime.Now, nombreMaquina, localIP);
        }

        protected void btn_VerDetalle_Click(object sender, ImageClickEventArgs e)
        {
            int legajo = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            int mes = ddl_Mes.SelectedIndex + 1;
            int año = Convert.ToInt32(ddl_Anio.Text);
            using (var cxt = new Model1Container())
            {
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo);
                lbl_AgenteSeleccionado.Text = ag.ApellidoYNombre;

                if ((ag.HorarioFlexible ?? false) == false)
                {
                    var detalleMovimientos = (from mh in cxt.MovimientosHoras
                                              where
                                                 mh.ResumenDiario.Agente.Legajo == legajo &&
                                                 mh.ResumenDiario.Dia.Month == mes &&
                                                 mh.ResumenDiario.Dia.Year == año &&
                                                 mh.DescontoDeHorasBonificables
                                              select new
                                              {
                                                  Dia = mh.ResumenDiario.Dia,
                                                  Movimiento = mh.Tipo.Tipo,
                                                  Horas = mh.Horas,
                                              }).ToList();

                    var detalleMovimientoFormatoDeHorasCorregidas = (from item in detalleMovimientos
                                                                     select new
                                                                     {
                                                                         Dia = item.Dia,
                                                                         Movimiento = item.Movimiento,
                                                                         Horas = FormatearHora(item.Horas)
                                                                     }).ToList();

                    gv_Detalle.DataSource = detalleMovimientoFormatoDeHorasCorregidas;
                    gv_Detalle.DataBind();
                }
                else
                {
                    var detalleMovimientos = (from rd in cxt.ResumenesDiarios
                                              where
                                              rd.AgenteId == ag.Id &&
                                              rd.Dia.Month == mes &&
                                              rd.Dia.Year == año &&
                                              rd.AcumuloHorasBonificacion != "000:00" && rd.AcumuloHorasBonificacion != "00:00" && rd.AcumuloHorasBonificacion != null
                                              select new
                                              {
                                                  Dia = rd.Dia,
                                                  Movimiento = "Desconto desde horas flexibles",
                                                  Horas = rd.AcumuloHorasBonificacion,
                                              }).ToList();
                    var detalleMovimientoFormatoDeHorasCorregidas = (from item in detalleMovimientos
                                                                     select new
                                                                     {
                                                                         Dia = item.Dia,
                                                                         Movimiento = item.Movimiento,
                                                                         Horas = FormatearHora(item.Horas)
                                                                     }).ToList();
                    gv_Detalle.DataSource = detalleMovimientoFormatoDeHorasCorregidas;
                    gv_Detalle.DataBind();
                }
            }

            gv_Detalle.Visible = true;
            btn_cerrarDetalle.Visible = true;
            lbl_AgenteSeleccionado.Visible = true;
        }

        private string FormatearHora(string p)
        {
            string[] horasminutos = p.Split(':');
            string horas = string.Empty;

            if (horasminutos[0].Length > 2)
            {
                horas = horasminutos[0].Replace("00", "0");
            }
            else
            {
                horas = horasminutos[0];
            }

            return horas + ":" + horasminutos[1];
        }

        protected void btn_cerrarDetalle_Click(object sender, EventArgs e)
        {
            gv_Detalle.Visible = false;
            btn_cerrarDetalle.Visible = false;
            lbl_AgenteSeleccionado.Visible = false;
        }

      
        protected void btn_Imprimir_Click(object sender, EventArgs e)
        {
            RenderReport(true);
        }

        protected void gv_bonificaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_bonificaciones.PageIndex = e.NewPageIndex;
            CargarResultadoBusqueda();
        }

        protected void btn_ImprimirAgentesDeudores_Click(object sender, EventArgs e)
        {
            RenderReport(false);
        }
       
    }
}