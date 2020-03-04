using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo.Reportes;
using Microsoft.Reporting.WebForms;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

namespace SisPer.Aplicativo
{
    public partial class Jefe_UltimosMovimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (
                   ag.Perfil != PerfilUsuario.Personal &&
                   !ag.Jefe && !ag.JefeTemporal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = (ag.Perfil == PerfilUsuario.Personal);
                    MenuJefe1.Visible = !(ag.Perfil == PerfilUsuario.Personal);

                    CargarValores();
                }
            }
        }

        private void CargarValores()
        {
            //Aca tengo que cargar todas las salidas y los francos y los horarios vespertinos
            Model1Container cxt = new Model1Container();
            Agente ag = Session["UsuarioLogueado"] as Agente;
            ag = cxt.Agentes.First(a => a.Id == ag.Id);
            List<Agente> agentes = ag.ObtenerAgentesSubordinadosCascada(true);
            Session["AgentesUltimosMovimientos"] = agentes;

            CargarSalidas();
            CargarHV();
            CargarFrancos();
            CargarLicencias();
            CargarMovimientosGrilla();
        }

        #region Salidas

        private void CargarSalidas()
        {
            List<Agente> agentes = Session["AgentesUltimosMovimientos"] as List<Agente>;
            List<Salida> salidas = new List<Salida>();
            foreach (Agente agente in agentes)
            {
                foreach (Salida sal in agente.Salidas.Where(fs => fs.Dia == DateTime.Today))
                {
                    salidas.Add(sal);
                }
            }

            var items = (from s in salidas
                         select new
                         {
                             Agente = s.Agente.ApellidoYNombre,
                             Tipo = s.Tipo,
                             Destino = s.Destino,
                             Desde = s.HoraDesde,
                             Hasta = s.HoraHasta != null ? s.HoraHasta : " no vuelve aún ",
                             MarcoJefe = s.Jefe != null ? s.Jefe.ApellidoYNombre : " - "
                         }).ToList().OrderBy(i => i.Hasta).ToList();

            GridViewSalidas.DataSource = items;
            GridViewSalidas.DataBind();
        }

        protected void gridViewSalidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSalidas.PageIndex = e.NewPageIndex;
            CargarSalidas();
        }

        protected void btn_ListadoSalidasDiarias_Click(object sender, EventArgs e)
        {
            RenderReport();
        }

        private ListadoSalidas_DS ObtenerDS()
        {

            ///Ver porque el jefe debe informar UNICAMENTE
            ///los de su area no de las areas que dependan de el
            List<Agente> agentes = Session["AgentesUltimosMovimientos"] as List<Agente>;
            List<Salida> salidas = new List<Salida>();
            DateTime diaBuscado = DateTime.Today;

            foreach (Agente agente in agentes)
            {
                foreach (Salida sal in agente.Salidas.Where(fs => fs.Dia.Date == diaBuscado.Date))
                {
                    salidas.Add(sal);
                }
            }

            var items = (from s in salidas
                         select new
                         {
                             Agente = s.Agente.ApellidoYNombre,
                             Legajo = s.Agente.Legajo,
                             Tipo = s.Tipo,
                             Destino = s.Destino,
                             Desde = s.HoraDesde,
                             Hasta = s.HoraHasta != null ? s.HoraHasta : " no vuelve aún ",
                             MarcoJefe = s.Jefe != null ? s.Jefe.ApellidoYNombre : " - ",
                             Fecha = s.Dia
                         }).ToList().OrderBy(i => i.Hasta).ToList();

            ListadoSalidas_DS ret = new ListadoSalidas_DS();
            ListadoSalidas_DS.GeneralRow dr = ret.General.NewGeneralRow();
            dr.Dia = "DIA: " + DateTime.Today.ToLongDateString();
            dr.Jefe = ((Agente)Session["UsuarioLogueado"]).ApellidoYNombre;
            dr.Sector = ((Agente)Session["UsuarioLogueado"]).Area.Nombre;

            ret.General.AddGeneralRow(dr);

            foreach (var item in items)
            {
                ListadoSalidas_DS.SalidasRow sr = ret.Salidas.NewSalidasRow();
                sr.Agente = item.Agente;
                sr.Desde = item.Desde;
                sr.Hasta = item.Hasta;
                sr.Legajo = item.Legajo.ToString();
                sr.Tipo = item.Tipo.ToString();
                sr.Destino = item.Destino;
                sr.Fecha = item.Fecha.ToShortDateString();
                sr.Sector = ((Agente)Session["UsuarioLogueado"]).Area.Nombre;

                ret.Salidas.AddSalidasRow(sr);
            }

            return ret;
        }

        private void RenderReport()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/ListadoSalidas_r.rdlc");

            ListadoSalidas_DS ds = ObtenerDS();
            ReportDataSource maestro = new ReportDataSource("Encabezado_DS", ds.General.Rows);
            ReportDataSource detalle = new ReportDataSource("Detalle_DS", ds.Salidas.Rows);

            viewer.LocalReport.DataSources.Add(maestro);
            viewer.LocalReport.DataSources.Add(detalle);

            ReportParameter rp_Sect = new ReportParameter("Sector", ((Agente)Session["UsuarioLogueado"]).Area.Nombre);
            ReportParameter rp_Jef = new ReportParameter("Jefe", ((Agente)Session["UsuarioLogueado"]).ApellidoYNombre);
            ReportParameter rp_Dia = new ReportParameter("Dia", "DIA: " + DateTime.Today.ToLongDateString());

            viewer.LocalReport.SetParameters(new ReportParameter[] { rp_Sect, rp_Jef, rp_Dia });

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

        private void RegistrarImpresionReporte()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "SALIDAS DIARIAS", DateTime.Now, nombreMaquina, localIP);
        }


        #endregion

        #region Cargar Movimientos

        private void CargarFrancos()
        {
            List<Agente> agentes = Session["AgentesUltimosMovimientos"] as List<Agente>;
            List<Franco> francos = new List<Franco>();
            foreach (Agente agente in agentes)
            {
                foreach (Franco franco in agente.Francos.Where(fc => fc.DiasFranco.Where(df => df.Dia >= DateTime.Today).Count() > 0 && (
                                                                                                                                        fc.Estado != EstadosFrancos.Cancelado && 
                                                                                                                                        fc.Estado != EstadosFrancos.CanceladoAutomatico &&
                                                                                                                                        fc.Estado != EstadosFrancos.Aprobado //porque una vez que se aprueba entra como licencia del tipo Franco compensatorio
                                                                                                                                        )))
                {
                    francos.Add(franco);
                }
            }

            List<itemListado> items = (from fc in francos
                         select new itemListado
                         {
                             Agente = fc.Agente.ApellidoYNombre,
                             Estado = fc.Estado.ToString(),
                             Tipo = "Franco compensatorio",
                             Fecha = fc.FechaSolicitud,
                            
                         }).ToList();

            Session["FC"] = items;

        }

        private void CargarHV()
        {
            List<Agente> agentes = Session["AgentesUltimosMovimientos"] as List<Agente>;
            List<HorarioVespertino> hvs = new List<HorarioVespertino>();
            foreach (Agente agente in agentes)
            {
                foreach (HorarioVespertino hv in agente.HorariosVespertinos.Where(hv => hv.Dia >= DateTime.Today))
                {
                    hvs.Add(hv);
                }
            }

            List<itemListado> items = (from hv in hvs
                         select new itemListado
                         {
                             Agente = hv.Agente.ApellidoYNombre,
                             Estado = ((EstadosHorarioVespertino)hv.Estado).ToString(),
                             Tipo = "Horario vespertino",
                             Fecha = hv.Dia,
                         }).ToList();

            Session["HV"] = items;

        }

        private void CargarLicencias()
        {
            List<Agente> agentes = Session["AgentesUltimosMovimientos"] as List<Agente>;
            List<EstadoAgente> estados = new List<EstadoAgente>();
            foreach (Agente agente in agentes)
            {
                foreach (EstadoAgente ea in agente.EstadosPorDiaAgente.Where(eag => eag.Dia >= DateTime.Today))
                {
                    if (ea.TipoEstado.Estado != "Fin de semana")
                    estados.Add(ea);
                }
            }

            List<itemListado> items = (from e in estados
                         select new itemListado
                         {
                             Agente = e.Agente.ApellidoYNombre,
                             Estado = e.TipoEstado.Estado,
                             Tipo = "Licencia",
                             Fecha = e.Dia
                         }).ToList();

            Session["Lic"] = items;
        }

        #endregion

        #region Movimientos por venir

        struct itemListado
        {
            internal string Agente;
            internal string Estado;
            internal string Tipo;
            internal DateTime Fecha;
        }

        private List<itemListado> ObtenerMovimientos()
        {
            List<itemListado> items = new List<itemListado>();
            List<itemListado> fc = Session["FC"] as List<itemListado>;
            List<itemListado> hv = Session["HV"] as List<itemListado>;
            List<itemListado> lic = Session["Lic"] as List<itemListado>;

            Model1Container cxt = new Model1Container();
            Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia >= DateTime.Today);

            items.AddRange(hv);
            items.AddRange(fc);
            items.AddRange(lic);
            if (feriado != null)
            {
                items.Add(new itemListado() { Agente = "-", Estado = feriado.Motivo, Fecha = feriado.Dia, Tipo = "Feriado" });
            }

            return items;
        }

        protected void GVDetalleMov_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDetalleMov.PageIndex = e.NewPageIndex;
            CargarMovimientosGrilla();
        }

        private void CargarMovimientosGrilla()
        {
            List<itemListado> items = ObtenerMovimientos();
            var gridItems = (from item in items
                             select new { Agente = item.Agente, Tipo = item.Tipo, Estado = item.Estado, Fecha = item.Fecha }).ToList();

            GvDetalleMov.DataSource = gridItems.OrderBy(f=>f.Fecha).ToList();
            GvDetalleMov.DataBind();
        }

        #endregion

    }
}