using Microsoft.Reporting.WebForms;
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
    public partial class Personal_Informe_SalidasVarias : System.Web.UI.Page
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
                    ddl_TiposDeInforme.Items.Add("Tardanzas mensuales");
                    ddl_TiposDeInforme.Items.Add("Fichadas mensuales");
                    ddl_TiposDeInforme.Items.Add("Listado de ausentes");
                    ddl_TiposDeInforme.Items.Add("Listado de art 50 inc 4");
                    ddl_TiposDeInforme.Items.Add("Listado de francos compensatorios");
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                ddl_Mes.Enabled = false;
                ddl_Anio.Enabled = false;
                tb_Desde.Enabled = false;
                tb_Hasta.Enabled = false;

                Ddl_Areas.Enabled = false;
                chk_Dependencias.Enabled = false;

                ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;

                CargarDDLAnio();

                Panel_dia.Attributes.Add("class", "alert alert-info");
                Panel_Legajo.Attributes.Add("class", "alert alert-info");
            }
        }

        private void CargarDDLAnio()
        {
            for (int i = 2022; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

        protected void rb_CheckedFechaChanged(object sender, EventArgs e)
        {
            if (rb_Dia.Checked)
            {//esta chequeado el dia, anular el resto
                Panel_dia.Attributes.Add("class", "alert alert-info");
                Panel_Mes.Attributes.Clear();
                Panel_DesdeHasta.Attributes.Clear();

                ddl_Mes.Enabled = false;
                ddl_Anio.Enabled = false;
                tb_Desde.Enabled = false;
                tb_Hasta.Enabled = false;
                tb_dia.Enabled = true;
            }
            else
            {
                if (rb_Mes.Checked)
                {//esta chequeado el mes, anular el resto
                    Panel_dia.Attributes.Clear();
                    Panel_Mes.Attributes.Add("class", "alert alert-info");
                    Panel_DesdeHasta.Attributes.Clear();

                    ddl_Mes.Enabled = true;
                    ddl_Anio.Enabled = true;
                    tb_Desde.Enabled = false;
                    tb_Hasta.Enabled = false;
                    tb_dia.Enabled = false;
                }
                else
                {//esta chequeado desde hasta, anular el resto
                    Panel_dia.Attributes.Clear();
                    Panel_Mes.Attributes.Clear();
                    Panel_DesdeHasta.Attributes.Add("class", "alert alert-info");

                    ddl_Mes.Enabled = false;
                    ddl_Anio.Enabled = false;
                    tb_Desde.Enabled = true;
                    tb_Hasta.Enabled = true;
                    tb_dia.Enabled = false;
                }
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
                    GenerarInforme();
                }
                else
                {
                    Controles.MessageBox.Show(this, "Existe un error en el legajo ingresado o el agente al que esta intentando acceder no depende de usted.", Controles.MessageBox.Tipo_MessageBox.Danger);
                }
            }
            else
            {
                GenerarInforme();
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

        private void GenerarInforme()
        {
            switch (ddl_TiposDeInforme.Text)
            {
                case "Salidas diarias":
                    GenerarInformeSalidas();
                    break;
                case "Horarios vespertinos":
                    GenerarInformeHorariosVespertinos();
                    break;
                case "Tardanzas mensuales":
                    GenerarInformeTardanzas();
                    break;
                case "Fichadas mensuales":
                    GenerarInformeFichadasMes();
                    break;
                case "Listado de ausentes":
                    GenerarInformeListadoAusentes();
                    break;
                case "Listado de art 50 inc 4":
                    GenerarInformeSolicitudesArt50Inc4();
                        break;
                case "Listado de francos compensatorios":
                    GenerarInformeFrancos();
                    break;

                default:
                    break;
            }
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

            Session["AgentesInforme"] = agentes;
        }

        #region Informe Salidas

        private void GenerarInformeSalidas()
        {
            Validate();
            if (IsValid)
            {
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                DateTime desde; DateTime hasta; Area area; int legajo;
                string labelDiaReporte = string.Empty;
                List<Agente> agentesBuscados = new List<Agente>();
                List<Salida> salidas = new List<Salida>();
                Session["AgentesInforme"] = agentesBuscados;

                if (rb_Dia.Checked)
                {//esta chequeado el dia
                    desde = Convert.ToDateTime(tb_dia.Text);
                    hasta = Convert.ToDateTime(tb_dia.Text);
                    labelDiaReporte = "DIA: " + tb_dia.Text;
                }
                else
                {
                    if (rb_Mes.Checked)
                    {//esta chequeado el mes
                        desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                        hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));
                        labelDiaReporte = "MES DE " + ddl_Mes.SelectedItem.Text + " DE " + ddl_Anio.Text;
                    }
                    else
                    {//esta chequeado desde hasta
                        desde = Convert.ToDateTime(tb_Desde.Text);
                        hasta = Convert.ToDateTime(tb_Hasta.Text);
                        labelDiaReporte = "DESDE " + tb_Desde.Text + " HASTA " + tb_Hasta.Text;
                    }
                }

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

                foreach (Agente ag in agentesBuscados)
                {
                    foreach (Salida salida in ag.Salidas.Where(s => s.Dia >= desde && s.Dia <= hasta))
                    {
                        salidas.Add(salida);
                    }
                }

                ///Armo el dataset
                var items = (from s in salidas
                             select new
                             {
                                 Area = s.Agente.Area.Nombre,
                                 Jefe = cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre) != null ?
                                       cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre).ApellidoYNombre : "El area no tiene jefe asignado",
                                 Agente = s.Agente.ApellidoYNombre,
                                 Legajo = s.Agente.Legajo,
                                 Tipo = s.Tipo,
                                 Desde = s.HoraDesde,
                                 Hasta = s.HoraHasta != null ? s.HoraHasta : " no vuelve aún ",
                                 MarcoJefe = s.Jefe != null ? s.Jefe.ApellidoYNombre : " - ",
                                 Destino = s.Destino,
                                 Fecha = s.Dia
                             }).ToList().OrderBy(i => i.Legajo).ThenBy(i => i.Fecha).ThenBy(i => i.Hasta).ToList();

                var itemsGroup = (from i in items
                                  group i by i.Area into SalidasArea
                                  select new
                                  {
                                      Area = SalidasArea.Key,
                                      Salidas = SalidasArea.ToList()
                                  }).ToList();

                ListadoSalidas_DS ret = new ListadoSalidas_DS();

                foreach (var grupo in itemsGroup)
                {//agrego el sector
                    ListadoSalidas_DS.GeneralRow dr = ret.General.NewGeneralRow();
                    dr.Dia = labelDiaReporte;
                    dr.Jefe = grupo.Salidas.First().Jefe;
                    dr.Sector = grupo.Area;
                    ret.General.AddGeneralRow(dr);

                    foreach (var salida in grupo.Salidas)
                    {//agrego las salidas del sector
                        ListadoSalidas_DS.SalidasRow sr = ret.Salidas.NewSalidasRow();
                        sr.Agente = salida.Agente;
                        sr.Desde = salida.Desde;
                        sr.Hasta = salida.Hasta;
                        sr.Legajo = salida.Legajo.ToString();
                        sr.Tipo = salida.Tipo.ToString();
                        sr.Destino = salida.Destino;
                        sr.Fecha = salida.Fecha.ToShortDateString();
                        sr.Sector = grupo.Area;

                        ret.Salidas.AddSalidasRow(sr);
                    }
                }

                Session["DS_SALIDAS"] = ret;

                ///Configuro el reporte
                ///
                RenderReportSalidas();
            }
        }

        private void RenderReportSalidas()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/ListadoSalidas_Generico_r.rdlc");
            Reportes.ListadoSalidas_DS ds = ((Reportes.ListadoSalidas_DS)Session["DS_SALIDAS"]);

            if (ds.Salidas.Rows.Count > 0)
            {
                ReportDataSource general = new ReportDataSource("General", ds.General.Rows);

                viewer.LocalReport.DataSources.Add(general);

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
                RegistrarImpresionReporteSalidas();
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

        private void RegistrarImpresionReporteSalidas()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "SALIDAS DIARIAS", DateTime.Now, nombreMaquina, localIP);
        }

        private void LocalReportSalidas_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.ListadoSalidas_DS ds = ((Reportes.ListadoSalidas_DS)Session["DS_SALIDAS"]);

            e.DataSources.Add(new ReportDataSource("Detalle_DS", ds.Salidas.Rows));
        }

        #endregion

        #region Informe Horarios Vespertinos

        private void GenerarInformeHorariosVespertinos()
        {
            ///Obtengo los datos
            Model1Container cxt = new Model1Container();
            DateTime desde; DateTime hasta; Area area; int legajo;
            string labelDiaReporte = string.Empty;
            List<Agente> agentesBuscados = new List<Agente>();
            List<HorarioVespertino> horariosVespertinos = new List<HorarioVespertino>();
            Session["AgentesInforme"] = agentesBuscados;

            if (rb_Dia.Checked)
            {//esta chequeado el dia
                desde = Convert.ToDateTime(tb_dia.Text);
                hasta = Convert.ToDateTime(tb_dia.Text);
                labelDiaReporte = "DIA: " + tb_dia.Text;
            }
            else
            {
                if (rb_Mes.Checked)
                {//esta chequeado el mes
                    desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                    hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));
                    labelDiaReporte = "MES DE " + ddl_Mes.SelectedItem.Text + " DE " + ddl_Anio.Text;
                }
                else
                {//esta chequeado desde hasta
                    desde = Convert.ToDateTime(tb_Desde.Text);
                    hasta = Convert.ToDateTime(tb_Hasta.Text);
                    labelDiaReporte = "DESDE " + tb_Desde.Text + " HASTA " + tb_Hasta.Text;
                }
            }

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

            foreach (Agente ag in agentesBuscados)
            {
                foreach (HorarioVespertino hv in ag.HorariosVespertinos.Where(h => h.Dia >= desde && h.Dia <= hasta && h.Estado == EstadosHorarioVespertino.Terminado))
                {
                    horariosVespertinos.Add(hv);
                }
            }



            ///Armo el dataset
            var items = (from hvesp in horariosVespertinos
                         select new
                         {
                             Area = hvesp.Agente.Area.Nombre,
                             Jefe = cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == hvesp.Agente.Area.Nombre) != null ?
                                    cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == hvesp.Agente.Area.Nombre).ApellidoYNombre : "El area no tiene jefe asignado",
                             Legajo = hvesp.Agente.Legajo,
                             Agente = hvesp.Agente.ApellidoYNombre,
                             Desde = hvesp.HoraInicio,
                             Hasta = hvesp.HoraFin,
                             Motivo = hvesp.Motivo,
                             Fecha = hvesp.Dia
                         }).ToList().OrderBy(i => i.Legajo).ThenBy(i => i.Fecha).ThenBy(i => i.Hasta).ToList();

            var itemsGroup = (from i in items
                              group i by i.Area into HorariosArea
                              select new
                              {
                                  Area = HorariosArea.Key,
                                  Horarios = HorariosArea.ToList()
                              }).ToList();

            HorariosVespertinos_DS ret = new HorariosVespertinos_DS();

            foreach (var grupo in itemsGroup)
            {//agrego el sector
                HorariosVespertinos_DS.GeneralRow dr = ret.General.NewGeneralRow();
                dr.RangoFecha = labelDiaReporte;
                dr.Jefe = grupo.Horarios.First().Jefe;
                dr.Sector = grupo.Area;
                ret.General.AddGeneralRow(dr);

                foreach (var horario in grupo.Horarios)
                {//agrego las salidas del sector
                    HorariosVespertinos_DS.DetalleRow hvr = ret.Detalle.NewDetalleRow();
                    hvr.Agente = horario.Agente;
                    hvr.Dia = horario.Fecha;
                    hvr.HoraDesde = horario.Desde;
                    hvr.HoraHasta = horario.Hasta;
                    hvr.Motivo = horario.Motivo;
                    hvr.Legajo = horario.Legajo.ToString();

                    hvr.Sector = grupo.Area;

                    ret.Detalle.AddDetalleRow(hvr);
                }
            }

            Session["DS_HorariosVespertinos"] = ret;

            ///Configuro el reporte
            ///
            RenderReportHorariosVespertinos();
        }

        private void RenderReportHorariosVespertinos()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/HorariosVespertinos_Generico_r.rdlc");
            Reportes.HorariosVespertinos_DS ds = ((Reportes.HorariosVespertinos_DS)Session["DS_HorariosVespertinos"]);

            if (ds.Detalle.Rows.Count > 0)
            {
                ReportDataSource general = new ReportDataSource("General", ds.General.Rows);

                viewer.LocalReport.DataSources.Add(general);

                viewer.LocalReport.SubreportProcessing += LocalReportHorariosVespertinos_SubreportProcessing;

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

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "HORARIOS VESPERTINOS", DateTime.Now, nombreMaquina, localIP);
        }

        private void LocalReportHorariosVespertinos_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.HorariosVespertinos_DS ds = ((Reportes.HorariosVespertinos_DS)Session["DS_HorariosVespertinos"]);

            e.DataSources.Add(new ReportDataSource("Detalle", ds.Detalle.Rows));
        }

        #endregion

        #region Informe Tardanza

        private void GenerarInformeTardanzas()
        {
            Validate();
            if (IsValid)
            {
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                DateTime desde; DateTime hasta; Area area; int legajo;
                string labelDiaReporte = string.Empty;
                List<Agente> agentesBuscados = new List<Agente>();
                TardanzasMes_DS ds = new TardanzasMes_DS();
                Session["AgentesInforme"] = agentesBuscados;

                if (rb_Dia.Checked)
                {//esta chequeado el dia
                    desde = Convert.ToDateTime(tb_dia.Text);
                    hasta = Convert.ToDateTime(tb_dia.Text);
                    labelDiaReporte = "DIA: " + tb_dia.Text;
                }
                else
                {
                    if (rb_Mes.Checked)
                    {//esta chequeado el mes
                        desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                        hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));
                        labelDiaReporte = "MES DE " + ddl_Mes.SelectedItem.Text + " DE " + ddl_Anio.Text;
                    }
                    else
                    {//esta chequeado desde hasta
                        desde = Convert.ToDateTime(tb_Desde.Text);
                        hasta = Convert.ToDateTime(tb_Hasta.Text);
                        labelDiaReporte = "DESDE " + tb_Desde.Text + " HASTA " + tb_Hasta.Text;
                    }
                }

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

                TardanzasMes_DS.GeneralRow gr = ds.General.NewGeneralRow();
                gr.Desde = desde;
                gr.Hasta = hasta;
                ds.General.AddGeneralRow(gr);

                foreach (Agente ag in agentesBuscados)
                {
                    var resumenes = (from rd in ag.ResumenesDiarios
                                     where
                                         rd.Dia >= desde && rd.Dia <= hasta &&
                                         rd.MovimientosHoras.Where(rrdd => rrdd.Tipo.Tipo == "Tardanza").Count() > 0
                                     select rd).OrderBy(r => r.Dia);

                    if (resumenes.Count() > 0)
                    {
                        string horasTotalesTardanza = "00:00";
                        int totalesTardanza = 0;
                        bool pasolos120porprimeravez = false;
                        int mesencurso = 0;

                        string _horasTotalesExcedidas = "00:00";
                        string _horasTotalesTardanza = "00:00";
                        int _totalesTardanza = 0;
                        bool _pasolos120porprimeravez = false;

                        foreach (ResumenDiario item in resumenes)
                        {
                            if (mesencurso != item.Dia.Month)
                            {
                                //Acumulo los totales generales del agente y pongo a cero los valores para el nuevo mes
                                if (mesencurso != 0)
                                {
                                    if (pasolos120porprimeravez)
                                    {
                                        _horasTotalesExcedidas = HorasString.SumarHoras(new string[] { HorasString.RestarHoras(horasTotalesTardanza, "02:00"), _horasTotalesExcedidas });
                                    }

                                    _horasTotalesTardanza = HorasString.SumarHoras(new string[] { horasTotalesTardanza, _horasTotalesTardanza });
                                    _totalesTardanza = _totalesTardanza + totalesTardanza;
                                    _pasolos120porprimeravez = _pasolos120porprimeravez || pasolos120porprimeravez;
                                }

                                horasTotalesTardanza = "00:00";
                                totalesTardanza = 0;
                                pasolos120porprimeravez = false;
                                mesencurso = item.Dia.Month;
                            }

                            TardanzasMes_DS.TardanzaRow tr = ds.Tardanza.NewTardanzaRow();
                            tr.Legajo = ag.Legajo;
                            tr.Fecha = item.Dia;
                            tr.HoraEntrada = (item.Marcaciones != null && item.Marcaciones.Where(m => !m.Anulada).Count()>0) ? item.Marcaciones.Where(m => !m.Anulada).OrderBy(m => m.Hora).First().Hora : "00:00";
                            tr.TiempoTardanza = item.MovimientosHoras.First(rrdd => rrdd.Tipo.Tipo == "Tardanza").Horas;

                            horasTotalesTardanza = HorasString.SumarHoras(new string[] { horasTotalesTardanza, tr.TiempoTardanza });
                            int horastardanzaAcumulada = 0;
                            int minutostardanzaAcumulado = 0;
                            int.TryParse(horasTotalesTardanza.Split(':')[0], out horastardanzaAcumulada);
                            int.TryParse(horasTotalesTardanza.Split(':')[1], out minutostardanzaAcumulado);

                            if ((horastardanzaAcumulada == 2 && minutostardanzaAcumulado > 0) || horastardanzaAcumulada > 2)
                            {
                                if (!pasolos120porprimeravez)
                                {
                                    tr.PrimerSupero = true;
                                    pasolos120porprimeravez = true;
                                }
                                else
                                {
                                    tr.PrimerSupero = false;
                                }

                                tr.TiempoSuperado = HorasString.RestarHoras(horasTotalesTardanza, "02:00");
                            }
                            else
                            {
                                tr.PrimerSupero = false;
                                tr.TiempoSuperado = string.Empty;
                            }

                            ds.Tardanza.AddTardanzaRow(tr);
                            totalesTardanza++;
                        }

                        if (pasolos120porprimeravez)
                        {
                            _horasTotalesExcedidas = HorasString.SumarHoras(new string[] { HorasString.RestarHoras(horasTotalesTardanza, "02:00"), _horasTotalesExcedidas });

                        }
                        _horasTotalesTardanza = HorasString.SumarHoras(new string[] { horasTotalesTardanza, _horasTotalesTardanza });
                        _totalesTardanza = _totalesTardanza + totalesTardanza;
                        _pasolos120porprimeravez = _pasolos120porprimeravez || pasolos120porprimeravez;

                        TardanzasMes_DS.AgenteRow ar = ds.Agente.NewAgenteRow();
                        ar.Legajo = ag.Legajo;
                        ar.NombreYApellido = ag.ApellidoYNombre;
                        ar.Departamento = ag.Area.Nombre;
                        ar.TotalTardanzas = _totalesTardanza;
                        ar.TotalHorasExedidas = _pasolos120porprimeravez ? _horasTotalesExcedidas : "00:00";
                        ar.TotalHorasTardanza = _horasTotalesTardanza;
                        ds.Agente.AddAgenteRow(ar);
                    }

                }

                Session["DS_TARDANZA"] = ds;

                ///Configuro el reporte
                ///
                RenderReportTardanzas();
            }
        }

        private void RenderReportTardanzas()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/TardanzasMes_r.rdlc");
            Reportes.TardanzasMes_DS ds = ((Reportes.TardanzasMes_DS)Session["DS_TARDANZA"]);

            if (ds.Tardanza.Rows.Count > 0)
            {
                ReportDataSource general = new ReportDataSource("General", ds.General.Rows);
                ReportDataSource agente = new ReportDataSource("Agente", ds.Agente.Rows);

                viewer.LocalReport.DataSources.Add(general);
                viewer.LocalReport.DataSources.Add(agente);

                viewer.LocalReport.SubreportProcessing += LocalReportTardanzas_SubreportProcessing;

                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = null;
                string encoding = null;
                string extension = null;
                string deviceInfo = null;
                byte[] bytes = null;

                deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //Render the report
                RegistrarImpresionReporteTardanza();
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

        private void RegistrarImpresionReporteTardanza()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "REPORTE TARDANZAS", DateTime.Now, nombreMaquina, localIP);
        }

        private void LocalReportTardanzas_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.TardanzasMes_DS ds = ((Reportes.TardanzasMes_DS)Session["DS_TARDANZA"]);

            e.DataSources.Add(new ReportDataSource("Tardanza", ds.Tardanza.Rows));
        }

        #endregion

        #region Informe fichadas mes

        private void GenerarInformeFichadasMes()
        {
            ///Obtengo los datos
            Model1Container cxt = new Model1Container();
            DateTime desde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime hasta = DateTime.Today;
            Area area; int legajo;
            string labelDiaReporte = string.Empty;
            List<Agente> agentesBuscados = new List<Agente>();
            List<HorarioVespertino> horariosVespertinos = new List<HorarioVespertino>();
            Session["AgentesInforme"] = agentesBuscados;

            if (rb_Dia.Checked)
            {//esta chequeado el dia
                MessageBox.Show(this, "El informe requerido es mensual unicamente, cambie la seleccion de fecha");
                return;
            }
            else
            {
                if (rb_Mes.Checked)
                {//esta chequeado el mes
                    desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                }
                else
                {//esta chequeado desde hasta
                    MessageBox.Show(this, "El informe requerido es mensual unicamente, cambie la seleccion de fecha");
                    return;
                }
            }

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
                Session["AgentesInforme"] = new List<Agente>();
                IncluirAgentes(area);
                agentesBuscados = Session["AgentesInforme"] as List<Agente>;
            }


            if (agentesBuscados.Count > 0 && desde != null)
            {
                RegistrarImpresionReporteFichadasMensuales();

                ReporteMensualFichadasHoras rp = new ReporteMensualFichadasHoras(agentesBuscados, desde.Month, desde.Year);

                byte[] pdf = rp.GenerarPDFAsistenciaMensual();
                Session["Bytes"] = pdf;

                string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
            }
            
        }

        private void RegistrarImpresionReporteFichadasMensuales()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "REPORTE FICHADAS MENSUALES", DateTime.Now, nombreMaquina, localIP);
        }

        #endregion

        #region Informe listado ausencias

        private void GenerarInformeListadoAusentes()
        {
            Validate();
            if (IsValid)
            {
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                DateTime desde; DateTime hasta; Area area; int legajo;
                string agentesInforme = string.Empty;
                List<Agente> agentesBuscados = new List<Agente>();
                List<Salida> salidas = new List<Salida>();
                Session["AgentesInforme"] = agentesBuscados;

                if (rb_Dia.Checked)
                {//esta chequeado el dia
                    desde = Convert.ToDateTime(tb_dia.Text);
                    hasta = Convert.ToDateTime(tb_dia.Text);

                }
                else
                {
                    if (rb_Mes.Checked)
                    {//esta chequeado el mes
                        desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                        hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));

                    }
                    else
                    {//esta chequeado desde hasta
                        desde = Convert.ToDateTime(tb_Desde.Text);
                        hasta = Convert.ToDateTime(tb_Hasta.Text);

                    }
                }

                if (rb_Legajo.Checked)
                {//seleccionado la busqueda por agente
                    legajo = Convert.ToInt32(tb_Legajo.Text);
                    Agente ag = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo);
                    if (ag != null)
                    {
                        agentesBuscados.Add(ag);
                        agentesInforme = "EL AGENTE " + ag.ApellidoYNombre;
                    }
                }
                else
                {//seleccionado la busqueda por sector
                    area = Ddl_Areas.AreaSeleccionado;
                    IncluirAgentes(area);
                    agentesBuscados = Session["AgentesInforme"] as List<Agente>;
                    agentesInforme = "LOS AGENTES DEL AREA " + area.Nombre + (chk_Dependencias.Checked ? " INCLUYENDO DEPENDENCIAS" : "");
                }

                ///Armo el dataset
                listado_ausentes_ds ds = new listado_ausentes_ds();

                listado_ausentes_ds.EncabezadoRow er = ds.Encabezado.NewEncabezadoRow();
                er.Desde = desde.ToShortDateString();
                er.Hasta = hasta.ToShortDateString();
                er.Filtro = agentesInforme;

                ds.Encabezado.Rows.Add(er);

                for (DateTime i = desde; i <= hasta; i = i.AddDays(1))
                {
                    if (i.DayOfWeek != DayOfWeek.Saturday && i.DayOfWeek != DayOfWeek.Sunday)
                    {
                        Feriado f = cxt.Feriados.FirstOrDefault(ff => ff.Dia == i);

                        foreach (Agente ag in agentesBuscados)
                        {
                            listado_ausentes_ds.DetalleRow dr = ds.Detalle.NewDetalleRow();

                            if (f != null)
                            {
                                dr.Fecha = i.ToShortDateString();
                                dr.Legajo = ag.Legajo.ToString();
                                dr.Agente = ag.ApellidoYNombre;
                                dr.Motivo = f.Motivo;
                                ds.Detalle.Rows.Add(dr);
                            }
                            else
                            {
                                EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(eeaa => eeaa.AgenteId == ag.Id && eeaa.Dia == i);
                                if (ea != null)
                                {
                                    dr.Fecha = i.ToShortDateString();
                                    dr.Legajo = ag.Legajo.ToString();
                                    dr.Agente = ag.ApellidoYNombre;
                                    dr.Motivo = ea.TipoEstado.Estado;
                                    ds.Detalle.Rows.Add(dr);
                                }
                                else
                                {
                                    DS_Marcaciones marcaciones = ProcesosGlobales.ObtenerMarcaciones(i, ag.Legajo.ToString());
                                    if (marcaciones.Marcacion.Rows.Count == 0)
                                    {
                                        //no tiene marcaciones 
                                        dr.Fecha = i.ToShortDateString();
                                        dr.Legajo = ag.Legajo.ToString();
                                        dr.Agente = ag.ApellidoYNombre;
                                        dr.Motivo = "Sin motivo";
                                        ds.Detalle.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }

                Session["ds_ausentes"] = ds;

                ///Configuro el reporte
                RenderReportListadoAusentes();
            }
        }

        private void RenderReportListadoAusentes()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/listado_ausentes_r.rdlc");
            listado_ausentes_ds ds = ((listado_ausentes_ds)Session["ds_ausentes"]);

            if (ds.Detalle.Rows.Count > 0)
            {
                ReportDataSource encabezado = new ReportDataSource("encabezado", ds.Encabezado.Rows);
                ReportDataSource detalle = new ReportDataSource("detalle", ds.Detalle.Rows);

                viewer.LocalReport.DataSources.Add(encabezado);
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

                RegistrarImpresionReporteAusencias();

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

        private void RegistrarImpresionReporteAusencias()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "REPORTE AUSENCIAS", DateTime.Now, nombreMaquina, localIP);
        }

        #endregion

      

        #region Informe de solicitudes de art 50 inc 4 (ex art 51)

        private void GenerarInformeSolicitudesArt50Inc4()
        {
            Validate();
            if (IsValid)
            {
                byte[] bytes = null;

                #region Obtener datos cargar el DataSet
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                DateTime desde; DateTime hasta; Area area; int legajo;
                string labelDiaReporte = string.Empty;
                List<Agente> agentesBuscados = new List<Agente>();
                List<Salida> salidas_art_50 = new List<Salida>();
                Session["AgentesInforme"] = agentesBuscados;

                if (rb_Dia.Checked)
                {//esta chequeado el dia
                    desde = Convert.ToDateTime(tb_dia.Text);
                    hasta = Convert.ToDateTime(tb_dia.Text);
                    labelDiaReporte = "DIA: " + tb_dia.Text;
                }
                else
                {
                    if (rb_Mes.Checked)
                    {//esta chequeado el mes
                        desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                        hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));
                        labelDiaReporte = "MES DE " + ddl_Mes.SelectedItem.Text + " DE " + ddl_Anio.Text;
                    }
                    else
                    {//esta chequeado desde hasta
                        desde = Convert.ToDateTime(tb_Desde.Text);
                        hasta = Convert.ToDateTime(tb_Hasta.Text);
                        labelDiaReporte = "DESDE " + tb_Desde.Text + " HASTA " + tb_Hasta.Text;
                    }
                }

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

                foreach (Agente ag in agentesBuscados)
                {
                    foreach (Salida salida in ag.Salidas.Where(s => s.Dia >= desde &&
                                                                    s.Dia <= hasta &&
                                                                    s.Destino == "Art. 50 R.P." &&
                                                                    s.HoraDesde == "06:30" &&
                                                                    s.HoraHasta == "13:00"))
                    {
                        salidas_art_50.Add(salida);
                    }
                }

                ///Armo el dataset
                var items = (from s in salidas_art_50
                             select new
                             {
                                 Area = s.Agente.Area.Nombre,
                                 Jefe = cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre) != null ?
                                       cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre).ApellidoYNombre : "El area no tiene jefe asignado",
                                 Agente = s.Agente.ApellidoYNombre,
                                 Legajo = s.Agente.Legajo,
                                 Tipo = s.Tipo,
                                 Desde = s.HoraDesde,
                                 Hasta = s.HoraHasta != null ? s.HoraHasta : " no vuelve aún ",
                                 MarcoJefe = s.Jefe != null ? s.Jefe.ApellidoYNombre : " - ",
                                 Destino = s.Destino,
                                 Fecha = s.Dia.ToString("dd/MM/yyyy")
                             }).ToList().OrderBy(i => i.Legajo).ThenBy(i => i.Fecha).ThenBy(i => i.Hasta).ToList();

                var itemsGroup = (from i in items
                                  group i by i.Area into SalidasArea
                                  select new
                                  {
                                      Area = SalidasArea.Key,
                                      Salidas = SalidasArea.ToList()
                                  }).ToList();

                ListadoSalidas_DS ret = new ListadoSalidas_DS();

                foreach (var grupo in itemsGroup)
                {//agrego el sector
                    ListadoSalidas_DS.GeneralRow dr = ret.General.NewGeneralRow();
                    dr.Dia = labelDiaReporte;
                    dr.Jefe = grupo.Salidas.First().Jefe;
                    dr.Sector = grupo.Area;
                    ret.General.AddGeneralRow(dr);

                    foreach (var salida in grupo.Salidas)
                    {//agrego las salidas del sector
                        ListadoSalidas_DS.SalidasRow sr = ret.Salidas.NewSalidasRow();
                        sr.Agente = salida.Agente;
                        sr.Desde = salida.Desde;
                        sr.Hasta = salida.Hasta;
                        sr.Legajo = salida.Legajo.ToString();
                        sr.Tipo = salida.Tipo.ToString();
                        sr.Destino = salida.Destino;
                        sr.Fecha = salida.Fecha;
                        sr.Sector = grupo.Area;

                        ret.Salidas.AddSalidasRow(sr);
                    }
                }

                #endregion

                if (ret.General.Count > 0)
                {
                    Informe_art_50_rp reporte = new Informe_art_50_rp(ret, desde, hasta);
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

        #region Informe de Francos compensatorios

        private void GenerarInformeFrancos()
        {
            Validate();
            if (IsValid)
            {
                byte[] bytes = null;

                #region Obtener datos cargar el DataSet
                ///Obtengo los datos
                Model1Container cxt = new Model1Container();
                DateTime desde; DateTime hasta; Area area; int legajo;
                string labelDiaReporte = string.Empty;
                List<Agente> agentesBuscados = new List<Agente>();
                List<Franco> francos = new List<Franco>();
                Session["AgentesInforme"] = agentesBuscados;

                if (rb_Dia.Checked)
                {//esta chequeado el dia
                    desde = Convert.ToDateTime(tb_dia.Text);
                    hasta = Convert.ToDateTime(tb_dia.Text);
                    labelDiaReporte = "DIA: " + tb_dia.Text;
                }
                else
                {
                    if (rb_Mes.Checked)
                    {//esta chequeado el mes
                        desde = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), 1);
                        hasta = new DateTime(Convert.ToInt16(ddl_Anio.Text), Convert.ToInt16(ddl_Mes.SelectedValue), DateTime.DaysInMonth(desde.Year, desde.Month));
                        labelDiaReporte = "MES DE " + ddl_Mes.SelectedItem.Text + " DE " + ddl_Anio.Text;
                    }
                    else
                    {//esta chequeado desde hasta
                        desde = Convert.ToDateTime(tb_Desde.Text);
                        hasta = Convert.ToDateTime(tb_Hasta.Text);
                        labelDiaReporte = "DESDE " + tb_Desde.Text + " HASTA " + tb_Hasta.Text;
                    }
                }

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

                foreach (Agente ag in agentesBuscados)
                {
                    List<Franco> francos_agente = (from franco in ag.Francos
                                          where
                                           franco.DiasFranco.Any(dia => dia.Dia >= desde && dia.Dia <= hasta) && franco.Estado == EstadosFrancos.Aprobado
                                          select franco).ToList();

                    foreach(Franco franco in francos_agente)
                    {
                        francos.Add(franco);
                    }
                }

                ///Armo el dataset
                var items = (from s in francos
                             select new
                             {
                                 Area = s.Agente.Area.Nombre,
                                 Jefe = cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre) != null ?
                                       cxt.Agentes.FirstOrDefault(a => a.Jefe && a.Area.Nombre == s.Agente.Area.Nombre).ApellidoYNombre : "El area no tiene jefe asignado",
                                 Agente = s.Agente.ApellidoYNombre,
                                 Legajo = s.Agente.Legajo,
                                 Tipo = "Franco compensatorio",
                                 Desde = "",
                                 Hasta = "",
                                 MarcoJefe = "",
                                 Destino = "",
                                 Fecha = s.DiasFranco.First().Dia.ToString("dd/MM/yyyy")
                             }).ToList().OrderBy(i => i.Legajo).ThenBy(i => i.Fecha).ThenBy(i => i.Hasta).ToList();

                var itemsGroup = (from i in items
                                  group i by i.Area into SalidasArea
                                  select new
                                  {
                                      Area = SalidasArea.Key,
                                      Salidas = SalidasArea.ToList()
                                  }).ToList();

                ListadoSalidas_DS ret = new ListadoSalidas_DS();

                foreach (var grupo in itemsGroup)
                {//agrego el sector
                    ListadoSalidas_DS.GeneralRow dr = ret.General.NewGeneralRow();
                    dr.Dia = labelDiaReporte;
                    dr.Jefe = grupo.Salidas.First().Jefe;
                    dr.Sector = grupo.Area;
                    ret.General.AddGeneralRow(dr);

                    foreach (var salida in grupo.Salidas)
                    {//agrego las salidas del sector
                        ListadoSalidas_DS.SalidasRow sr = ret.Salidas.NewSalidasRow();
                        sr.Agente = salida.Agente;
                        sr.Desde = salida.Desde;
                        sr.Hasta = salida.Hasta;
                        sr.Legajo = salida.Legajo.ToString();
                        sr.Tipo = salida.Tipo.ToString();
                        sr.Destino = salida.Destino;
                        sr.Fecha = salida.Fecha;
                        sr.Sector = grupo.Area;

                        ret.Salidas.AddSalidasRow(sr);
                    }
                }

                #endregion

                if (ret.General.Count > 0)
                {
                    Informe_francos_compensatorios reporte = new Informe_francos_compensatorios(ret, desde, hasta);
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

        #region Informe de trabajo remoto
        #endregion

        #region Informe de cambio de turno
        #endregion

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = rb_Dia.Checked ? DateTime.TryParse(tb_dia.Text, out d) : true;
        }

        protected void cv_Legajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int legajo = 0;
            args.IsValid = rb_Legajo.Checked ? int.TryParse(tb_Legajo.Text, out legajo) : true;
        }

        #endregion

    }
}