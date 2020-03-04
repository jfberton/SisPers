﻿using Microsoft.Reporting.WebForms;
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
            for (int i = 2013; i <= DateTime.Now.Year; i++)
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

        #region informe fichadas mes

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
                //mostrar error
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
                    //mostrar error
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

            Reportes.HorarioIngresoEgresoMensual ret = new Reportes.HorarioIngresoEgresoMensual();

            foreach (Agente agente in agentesBuscados)
            {
                string total_horas_trabajadas = "00:00";
                string total_horas_mas = "00:00";
                string total_horas_menos = "00:00";
                string total_horas_tardanza = "00:00";

                for (DateTime i = desde; i <= hasta; i = i.AddDays(1))
                {
                    DateTime dia = new DateTime();

                    try
                    {
                        dia = i;
                        Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ObtenerDetalle(agente, dia, ret);
                        ret.DetalleDia.Rows.Add(dr);

                        total_horas_mas = HorasString.SumarHoras(new string[] { total_horas_mas, dr.HorasMas });
                        total_horas_menos = HorasString.SumarHoras(new string[] { total_horas_menos, dr.HorasMenos });
                        total_horas_trabajadas = HorasString.SumarHoras(new string[] { total_horas_trabajadas, dr.HorasTrabajadas });
                        total_horas_tardanza = HorasString.SumarHoras(new string[] { total_horas_tardanza, dr.Tardanzas });
                    }
                    catch
                    {
                        Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ret.DetalleDia.NewDetalleDiaRow();
                        dr.Dia = i.Day;
                        dr.AgenteId = agente.Id;
                        dr.IngresoMan = "-";
                        dr.EgresoMan = "-";
                        dr.IngresoTar = "-";
                        dr.EngresoTar = "-";
                        dr.Tardanzas = "-";
                        dr.HorasMas = "-";
                        dr.HorasMenos = "-";
                        dr.HorasTrabajadas = "-";
                        dr.Observacion = "---";
                        ret.DetalleDia.Rows.Add(dr);
                    }
                }

                Reportes.HorarioIngresoEgresoMensual.AgenteRow ar = ret.Agente.NewAgenteRow();
                ar.Id = agente.Id;
                ar.Legajo = agente.Legajo.ToString();
                ar.Nombre = agente.ApellidoYNombre;
                ar.Area = agente.Area.Nombre;
                ar.Total_horas_mas = total_horas_mas;
                ar.Total_horas_menos = total_horas_menos;
                ar.Total_horas_trabajadas = total_horas_trabajadas;
                ar.Total_horas_tardanza = total_horas_tardanza;
                ar.Total_horas = HorasString.RestarHoras(total_horas_mas, total_horas_menos);
                ret.Agente.Rows.Add(ar);

            }

            Reportes.HorarioIngresoEgresoMensual.GeneralRow gr = ret.General.NewGeneralRow();
            gr.Anio = ddl_Anio.SelectedItem.Text;
            gr.Mes = ddl_Mes.SelectedItem.Text;

            ret.General.Rows.Add(gr);

            RenderReportInformeFichadasMensual(ret);
        }

        private Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow ObtenerDetalle(Agente a, DateTime dia, Reportes.HorarioIngresoEgresoMensual ds)
        {
            Reportes.HorarioIngresoEgresoMensual.DetalleDiaRow dr = ds.DetalleDia.NewDetalleDiaRow();

            using (var cxt = new Model1Container())
            {
                dr.Dia = dia.Day;
                dr.AgenteId = a.Id;
                dr.IngresoMan = "00:00";
                dr.EgresoMan = "00:00";
                dr.IngresoTar = "00:00";
                dr.EngresoTar = "00:00";
                dr.Tardanzas = "00:00";
                dr.HorasMas = "00:00";
                dr.HorasMenos = "00:00";
                dr.HorasTrabajadas = "00:00";
                dr.Observacion = "";

                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                {
                    Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == dia);
                    if (feriado != null)
                    {
                        dr.Observacion = "FERIADO: " + feriado.Motivo;
                    }
                    else
                    {
                        EstadoAgente estado = a.EstadosPorDiaAgente.FirstOrDefault(e => e.Dia == dia);
                        if (estado != null)
                        {
                            dr.Observacion = estado.TipoEstado.Estado;
                        }
                        else
                        {
                            Franco franco = a.Francos.FirstOrDefault(f => f.DiasFranco.FirstOrDefault(d => d.Dia == dia) != null);
                            if (franco != null && franco.Estado == EstadosFrancos.Aprobado)
                            {
                                dr.Observacion = "FRANCO COMPENSATORIO";
                            }
                            else
                            {
                                ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Dia == dia && rrdd.AgenteId == a.Id);
                                if (rd == null || !(rd.Cerrado ?? false))
                                {
                                    dr.Observacion = "Día sin cerrar";
                                }
                                else
                                {
                                    string horas_trabajadas = "00:00";
                                    string horas_tardanza = "00:00";
                                    string horas_mas = "00:00";
                                    string horas_menos = "00:00";

                                    MovimientoHora tardanza = cxt.MovimientosHoras.FirstOrDefault(mh => mh.AgenteId == a.Id && mh.ResumenDiario.Dia == dia && mh.TipoMovimientoHoraId == 1);
                                    if (tardanza != null)
                                    {
                                        horas_tardanza = tardanza.Horas;
                                    }

                                    //EntradaSalida Es = cxt.EntradasSalidas.FirstOrDefault(es => es.Agente.Id == a.Id && es.Fecha == dia);//e=> a.EntradasSalidasMarcadas.FirstOrDefault(es => es.Fecha == dia);


                                    dr.IngresoMan = rd.HEntrada;
                                    string hora_entrada_para_calculo = HorasString.AMayorQueB(rd.HEntrada, a.HoraEntrada) ? rd.HEntrada : a.HoraEntrada;
                                    dr.EgresoMan = rd.HSalida;
                                    horas_trabajadas = HorasString.SumarHoras(new string[] { horas_trabajadas, HorasString.RestarHoras(rd.HSalida, hora_entrada_para_calculo) });

                                    HorarioVespertino hv = a.HorariosVespertinos.FirstOrDefault(h => h.Dia == dia);
                                    if (hv != null && hv.Estado == EstadosHorarioVespertino.Terminado)
                                    {
                                        dr.IngresoTar = hv.HoraInicio;
                                        dr.EngresoTar = hv.HoraFin;
                                        horas_trabajadas = HorasString.SumarHoras(new string[] { horas_trabajadas, hv.Horas });
                                    }
                                    else
                                    {
                                        dr.IngresoTar = "00:00";
                                        dr.EngresoTar = "00:00";
                                    }

                                    string diferencia_horas = HorasString.RestarHoras(horas_trabajadas, "06:30");
                                    if (diferencia_horas.Contains("-"))
                                    {
                                        horas_mas = "00:00";
                                        horas_menos = diferencia_horas.Replace("-", "");
                                    }
                                    else
                                    {
                                        horas_mas = diferencia_horas;
                                        horas_menos = "00:00";
                                    }

                                    dr.HorasMas = horas_mas;
                                    dr.HorasMenos = horas_menos;
                                    dr.HorasTrabajadas = horas_trabajadas;
                                    dr.Tardanzas = horas_tardanza;
                                }
                            }
                        }
                    }
                }
                else
                {
                    dr.Observacion = dia.DayOfWeek == DayOfWeek.Saturday ? "SABADO" : "DOMINGO";
                }
            }

            return dr;
        }

        private void RenderReportInformeFichadasMensual(Reportes.HorarioIngresoEgresoMensual fichadas_ds)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/HorariosMensualesAgentesArea.rdlc");
            Session["DS"] = fichadas_ds;
            Reportes.HorarioIngresoEgresoMensual ds = ((Reportes.HorarioIngresoEgresoMensual)Session["DS"]);
            ReportDataSource general = new ReportDataSource("ds_General", ds.General.Rows);
            ReportDataSource agentes = new ReportDataSource("ds_Agentes", ds.Agente.Rows);

            viewer.LocalReport.DataSources.Add(general);
            viewer.LocalReport.DataSources.Add(agentes);

            viewer.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            RegistrarImpresionReporteFichadasMensuales();
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            Reportes.HorarioIngresoEgresoMensual ds = ((Reportes.HorarioIngresoEgresoMensual)Session["DS"]);
            e.DataSources.Add(new ReportDataSource("ds_Detalle", ds.DetalleDia.Rows));
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