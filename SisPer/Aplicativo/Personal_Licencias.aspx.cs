using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Licencias : System.Web.UI.Page
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

                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                }

                GridViewAgentes.Columns[2].HeaderText = (DateTime.Today.Year - 1).ToString();
                GridViewAgentes.Columns[3].HeaderText = DateTime.Today.Year.ToString();

                Session["calculo_antiguedad"] = false;
                Session["corte_licencia"] = false;

                ddl_corte_anio.Items.Clear();
                ddl_corte_anio.Items.Add(new ListItem { Text = (DateTime.Today.Year - 1).ToString() });
                ddl_corte_anio.Items.Add(new ListItem { Text = DateTime.Today.Year.ToString() });
            }
        }

        private List<ItemGrilla> itemsGrilla = new List<ItemGrilla>();
        private struct ItemGrilla
        {
            public int AgenteId { get; set; }
            public int Legajo { get; set; }
            public string Agente { get; set; }
            public int DiasAnioAnterior { get; set; }
            public int DiasAnioActual { get; set; }
        }

        protected void btn_AdministrarLicencias_Click(object sender, ImageClickEventArgs e)
        {
            int idAg = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            CargarValores(idAg);
        }

        private void CargarValores(int idAg)
        {
            using (var cxt = new Model1Container())
            {
                Agente ag = cxt.Agentes.First(a => a.Id == idAg);
                DatosAgente.Agente = ag;

                OcultarEdicion();

                TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                LicenciaAgente licAgAnioAct = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                LicenciaAgente licAgAnioanterior = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                LicenciaActual.Tipo = tli.Tipo + " - " + DateTime.Today.Year.ToString();
                LicenciaActual.DiasOtorgados = licAgAnioAct.DiasOtorgados;
                LicenciaActual.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licAgAnioAct);
                LicenciaActual.RefrescarValores();

                LicenciaAnterior.Tipo = tli.Tipo + " - " + (DateTime.Today.Year - 1).ToString();
                LicenciaAnterior.DiasOtorgados = licAgAnioanterior.DiasOtorgados;
                LicenciaAnterior.DiasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licAgAnioanterior);
                LicenciaAnterior.RefrescarValores();

                p_datosLicencia.Visible = true;

                string anioanterior = DateTime.Today.AddYears(-1).Year.ToString();
                string anioactual = DateTime.Today.Year.ToString();

                var cortes = (from cc in cxt.CorteLicencia
                              where cc.AgenteId == DatosAgente.Agente.Id && (cc.Anio == anioanterior || cc.Anio == anioactual)
                              select new
                              {
                                  Instrumento = cc.NumInstrumento,
                                  Tipo = cc.Tipo,
                                  Anio = cc.Anio,
                                  Desde = cc.Desde
                              }
                                  );
                gv_cortes.DataSource = cortes;
                gv_cortes.DataBind();
            }
        }

        protected void GridViewAgentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAgentes.PageIndex = e.NewPageIndex;
            CargarAgentes();
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarAgentes();
        }

        private void CargarAgentes()
        {
            int legajoDesde = 0;
            int legajoHasta = 0;
            if (int.TryParse(tb_desde.Text, out legajoDesde) && int.TryParse(tb_hasta.Text, out legajoHasta))
            {
                itemsGrilla.Clear();

                using (var cxt = new Model1Container())
                {
                    var agentes = cxt.Agentes.Where(a => a.Legajo >= legajoDesde && a.Legajo <= legajoHasta && a.FechaBaja == null).OrderBy(i => i.Legajo);
                    TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");

                    foreach (var agente in agentes)
                    {
                        LicenciaAgente licAgAnioAct = agente.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                        LicenciaAgente licAgAnioanterior = agente.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                        if (licAgAnioAct == null)
                        {
                            licAgAnioAct = new LicenciaAgente
                            {
                                TipoLicenciaId = tli.Id,
                                Anio = DateTime.Today.Year,
                                AgenteId = agente.Id,
                                DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(agente, DateTime.Today.Year),
                                DiasUsufructuadosIniciales = 0,
                            };

                            cxt.LicenciasAgentes.AddObject(licAgAnioAct);
                        }

                        if (licAgAnioanterior == null)
                        {
                            licAgAnioanterior = new LicenciaAgente
                            {
                                TipoLicenciaId = tli.Id,
                                Anio = DateTime.Today.Year - 1,
                                AgenteId = agente.Id,
                                DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(agente, DateTime.Today.Year - 1),
                                DiasUsufructuadosIniciales = 0,
                            };

                            cxt.LicenciasAgentes.AddObject(licAgAnioanterior);
                        }

                        itemsGrilla.Add(new ItemGrilla()
                        {
                            AgenteId = agente.Id,
                            Legajo = agente.Legajo,
                            Agente = agente.ApellidoYNombre,
                            DiasAnioActual = licAgAnioAct.DiasOtorgados,
                            DiasAnioAnterior = licAgAnioanterior.DiasOtorgados
                        });
                    }

                    cxt.SaveChanges();
                    GridViewAgentes.DataSource = null;
                    GridViewAgentes.DataSource = itemsGrilla;
                    GridViewAgentes.DataBind();
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Verifique los legajos ingresados, deben ser numéricos sin puntos ni comas.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        private void MessageBox(string message)
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">alert('" + message + "');</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", script, false);
        }

        #region Editar fechas de ingreso y años reconocidos en otras partes

        protected void cv_IngresoAdmPub_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_IngresoAdmPub.Text, out d);
        }

        protected void cv_IngresoAPlanta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_IngresoAPlanta.Text, out d);
        }

        protected void cv_AntiguedadAnios_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int i = 0;
            args.IsValid = int.TryParse(tb_AntiguedadAnios.Text, out i);
        }

        protected void cv_AntiguedadMeses_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int i = 0;
            args.IsValid = int.TryParse(tb_AntiguedadMeses.Text, out i);
        }

        private void OcultarEdicion()
        {
            using (var cxt = new Model1Container())
            {
                int legajo = DatosAgente.Agente.Legajo;
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);

                lbl_IngresoAPlanta.Text = ag.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                lbl_IngresoAdmPub.Text = ag.Legajo_datos_laborales.FechaIngresoAminPub.ToShortDateString();
                lbl_AniosEnOtrasPartes.Text = ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes.ToString() + " años;" + ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes.ToString() + " meses";
                lbl_AñosTotalesAntiguedad.Text = Math.Round((((DateTime.Today - ag.Legajo_datos_laborales.FechaIngresoAminPub).TotalDays / 365) + ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes + (ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes / 12)), 2).ToString();
            }

            lbl_IngresoAPlanta.Visible = true;
            lbl_IngresoAdmPub.Visible = true;
            lbl_AniosEnOtrasPartes.Visible = true;

            btn_Accept_AniosEnOtrasPartes.Visible = false;
            btn_Accept_IngresoAdmPub.Visible = false;
            btn_Accept_IngresoAPlanta.Visible = false;

            btn_Cancel_AniosEnOtrasPartes.Visible = false;
            btn_Cancel_IngresoAdmPub.Visible = false;
            btn_Cancel_IngresoAPlanta.Visible = false;

            lbl_labelAños.Visible = false;
            lbl_labelMeses.Visible = false;
            tb_AntiguedadAnios.Visible = false;
            tb_AntiguedadMeses.Visible = false;

            tb_IngresoAdmPub.Visible = false;
            tb_IngresoAPlanta.Visible = false;


            btn_Edit_AniosEnOtrasPartes.Visible = true;
            btn_Edit_IngresoAdmPub.Visible = true;
            btn_Edit_IngresoAPlanta.Visible = true;

        }

        private void RefrescarDiasOtorgadosLicencia()
        {
            using (var cxt = new Model1Container())
            {
                int legajo = DatosAgente.Agente.Legajo;
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);
                TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                LicenciaAgente licAgAnioAct = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                LicenciaAgente licAgAnioanterior = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                licAgAnioAct.DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(ag, DateTime.Today.Year);
                licAgAnioanterior.DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(ag, DateTime.Today.Year - 1);

                cxt.SaveChanges();
                CargarValores(ag.Id);
            }
        }

        protected void btn_Accept_IngresoAdmPub_Click(object sender, EventArgs e)
        {
            Page.Validate("IngresoAdmPub");

            if (Page.IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    int legajo = DatosAgente.Agente.Legajo;
                    Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);

                    ag.Legajo_datos_laborales.FechaIngresoAminPub = Convert.ToDateTime(tb_IngresoAdmPub.Text);
                    lbl_IngresoAdmPub.Text = ag.Legajo_datos_laborales.FechaIngresoAminPub.ToShortDateString();

                    cxt.SaveChanges();
                    RefrescarDiasOtorgadosLicencia();
                }
            }
        }

        protected void btn_Cancel_IngresoAdmPub_Click(object sender, EventArgs e)
        {
            OcultarEdicion();
        }

        protected void btn_Edit_IngresoAdmPub_Click(object sender, EventArgs e)
        {
            lbl_IngresoAdmPub.Visible = false;
            tb_IngresoAdmPub.Text = lbl_IngresoAdmPub.Text;
            tb_IngresoAdmPub.Visible = true;
            btn_Accept_IngresoAdmPub.Visible = true;
            btn_Cancel_IngresoAdmPub.Visible = true;
            btn_Edit_IngresoAdmPub.Visible = false;
        }

        protected void btn_Accept_IngresoAPlanta_Click(object sender, EventArgs e)
        {
            Page.Validate("IngresoAPlanta");

            if (Page.IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    int legajo = DatosAgente.Agente.Legajo;
                    Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);

                    ag.Legajo_datos_laborales.FechaIngresoATP = Convert.ToDateTime(tb_IngresoAPlanta.Text);
                    lbl_IngresoAPlanta.Text = ag.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();

                    cxt.SaveChanges();
                    RefrescarDiasOtorgadosLicencia();
                }

            }
        }

        protected void btn_Cancel_IngresoAPlanta_Click(object sender, EventArgs e)
        {
            OcultarEdicion();
        }

        protected void btn_Edit_IngresoAPlanta_Click(object sender, EventArgs e)
        {
            lbl_IngresoAPlanta.Visible = false;
            tb_IngresoAPlanta.Text = lbl_IngresoAPlanta.Text;
            tb_IngresoAPlanta.Visible = true;
            btn_Accept_IngresoAPlanta.Visible = true;
            btn_Cancel_IngresoAPlanta.Visible = true;
            btn_Edit_IngresoAPlanta.Visible = false;
        }

        protected void btn_Accept_AniosEnOtrasPartes_Click(object sender, EventArgs e)
        {
            Page.Validate("AniosEnOtrasPartes");

            if (Page.IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    int legajo = DatosAgente.Agente.Legajo;
                    Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);

                    ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes = Convert.ToInt16(tb_AntiguedadAnios.Text);
                    ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes = Convert.ToInt16(tb_AntiguedadMeses.Text);
                    lbl_AniosEnOtrasPartes.Text = ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes.ToString() + " años;" + ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes.ToString() + " meses";

                    cxt.SaveChanges();
                    RefrescarDiasOtorgadosLicencia();
                }
            }
        }

        protected void btn_Cancel_AniosEnOtrasPartes_Click(object sender, EventArgs e)
        {
            OcultarEdicion();
        }

        protected void btn_Edit_AniosEnOtrasPartes_Click(object sender, EventArgs e)
        {
            lbl_AniosEnOtrasPartes.Visible = false;

            tb_AntiguedadAnios.Text = lbl_AniosEnOtrasPartes.Text.Split(';')[0].Replace(" años", "");
            tb_AntiguedadMeses.Text = lbl_AniosEnOtrasPartes.Text.Split(';')[1].Replace(" meses", "");

            lbl_labelAños.Visible = true;
            lbl_labelMeses.Visible = true;
            tb_AntiguedadAnios.Visible = true;
            tb_AntiguedadMeses.Visible = true;

            btn_Accept_AniosEnOtrasPartes.Visible = true;
            btn_Cancel_AniosEnOtrasPartes.Visible = true;
            btn_Edit_AniosEnOtrasPartes.Visible = false;
        }

        #endregion

        protected void LicenciaActual_ModificoValor(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int legajo = DatosAgente.Agente.Legajo;
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo);
                TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                LicenciaAgente licAgAnioAct = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);

                licAgAnioAct.DiasOtorgados = LicenciaActual.DiasOtorgados;

                int dias = LicenciaActual.DiasUsufructuados;
                int diasAgendados = (from du in ag.DiasUsufructados
                                     where du.Anio == licAgAnioAct.Anio
                                     select du).Count();
                licAgAnioAct.DiasUsufructuadosIniciales = dias - diasAgendados;

                cxt.SaveChanges();

                CargarValores(ag.Id);
            }
        }

        protected void LicenciaAnterior_ModificoValor(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int legajo = DatosAgente.Agente.Legajo;
                Agente ag = cxt.Agentes.First(a => a.Legajo == legajo && a.FechaBaja == null);
                TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                LicenciaAgente licAgAnioanterior = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                licAgAnioanterior.DiasOtorgados = LicenciaAnterior.DiasOtorgados;

                int dias = LicenciaAnterior.DiasUsufructuados;
                int diasAgendados = (from du in ag.DiasUsufructados
                                     where du.Anio == licAgAnioanterior.Anio
                                     select du).Count();
                licAgAnioanterior.DiasUsufructuadosIniciales = dias - diasAgendados;

                cxt.SaveChanges();
                CargarValores(ag.Id);
            }
        }

        protected void lnk_mostrar_datos_calculo_antiguedad_Click(object sender, EventArgs e)
        {
            bool muestra = !Convert.ToBoolean(Session["calculo_antiguedad"]);
            Session["calculo_antiguedad"] = muestra;
            if (muestra)
            {
                collapseAntiguedadLicencia.Attributes["class"] = "collapse in";
            }
            else
            {
                collapseAntiguedadLicencia.Attributes["class"] = "collapse";
            }
        }

        protected void lnk_corte_licencia_Click(object sender, EventArgs e)
        {
            bool muestra = !Convert.ToBoolean(Session["corte_licencia"]);
            Session["corte_licencia"] = muestra;
            if (muestra)
            {
                collapseCorteLicencia.Attributes["class"] = "collapse in";
            }
            else
            {
                collapseCorteLicencia.Attributes["class"] = "collapse";
            }
        }

        protected void btn_Visualizar_Corte_Click(object sender, EventArgs e)
        {
            int diasEliminados = 0;

            using (var cxt = new Model1Container())
            {
                DateTime desde = Convert.ToDateTime(tb_corte_desde.Text);
                DateTime hasta = cxt.EstadosAgente.Where(eeaa => eeaa.AgenteId == DatosAgente.Agente.Id).Max(eeaa => eeaa.Dia);
                for (DateTime i = desde; i <= hasta; i = i.AddDays(1))
                {
                    if (DebeEliminariaDia(i))
                    {
                        diasEliminados = diasEliminados + 1;
                    }
                }

            }

            lbl_Visualizarcorte.Text = "El corte eliminará " + diasEliminados + " días de licencia " + ddl_corteTipoLicencia.Text.ToLower() + " agendados.-";
            lbl_Visualizarcorte.Visible = true;
            btn_Visualizar_Corte.Enabled = false;
            btn_cancelar_corte.Visible = true;
            btn_Cargar_Corte.Visible = true;
        }

        /// <summary>
        /// Proceso que verifica el dia buscado y determina si se eliminaría o no la licencia en caso del corte
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool DebeEliminariaDia(DateTime d)
        {
            bool ret = false;

            using (var cxt = new Model1Container())
            {
                EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(eeaa => eeaa.AgenteId == DatosAgente.Agente.Id && eeaa.Dia == d && eeaa.TipoEstado.Estado != "Fin de semana");
                if (ea != null)
                {
                    //tiene un estado agendado para ese día, si es del tipo de licencia que estoy queriendo eliminar sigo analizando sino salgo
                    if (ddl_corteTipoLicencia.Text == "Anual" && ea.TipoEstado.Estado == "Licencia Anual")
                    {
                        //Si existe entre los dias usufructuados verifico que sea del mismo año sino lo obvio.
                        DiaUsufructado du = cxt.DiasUsufructuados.FirstOrDefault(dduu => dduu.AgenteId == DatosAgente.Agente.Id && dduu.Dia == d);
                        if (du != null)
                        {
                            int year = 0;
                            int.TryParse(ddl_corte_anio.Text, out year);
                            if (du.Anio == year)
                            {
                                ret = true;
                            }
                        }
                        else
                        {
                            ret = true;
                        }

                    }

                    if (ddl_corteTipoLicencia.Text == "Invierno" && ea.TipoEstado.Estado == "Licencia especial invierno")
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        private void EliminarEstadoDiaDia(DateTime d)
        {
            using (var cxt = new Model1Container())
            {
                EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(eeaa => eeaa.AgenteId == DatosAgente.Agente.Id && eeaa.Dia == d && eeaa.TipoEstado.Estado != "Fin de semana");
                if (ea != null)
                {
                    //tiene un estado agendado para ese día, si es del tipo de licencia que estoy queriendo eliminar sigo analizando sino salgo
                    if (ddl_corteTipoLicencia.Text == "Anual" && ea.TipoEstado.Estado == "Licencia Anual")
                    {
                        //Si existe entre los dias usufructuados verifico que sea del mismo año sino lo obvio.
                        DiaUsufructado du = cxt.DiasUsufructuados.FirstOrDefault(dduu => dduu.AgenteId == DatosAgente.Agente.Id && dduu.Dia == d);
                        if (du != null)
                        {
                            int year = 0;
                            int.TryParse(ddl_corte_anio.Text, out year);
                            if (du.Anio == year)
                            {
                                cxt.DiasUsufructuados.DeleteObject(du);
                                cxt.EstadosAgente.DeleteObject(ea);
                                cxt.SaveChanges();
                            }
                        }
                        else
                        {
                            cxt.EstadosAgente.DeleteObject(ea);
                            cxt.SaveChanges();
                        }

                    }

                    if (ddl_corteTipoLicencia.Text == "Invierno" && ea.TipoEstado.Estado == "Licencia especial invierno")
                    {
                        cxt.EstadosAgente.DeleteObject(ea);
                        cxt.SaveChanges();
                    }
                }
            }
        }

        protected void btn_Cargar_Corte_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                DateTime desde = Convert.ToDateTime(tb_corte_desde.Text);
                DateTime hasta = cxt.EstadosAgente.Where(eeaa => eeaa.AgenteId == DatosAgente.Agente.Id).Max(eeaa => eeaa.Dia);
                for (DateTime i = desde; i <= hasta; i = i.AddDays(1))
                {
                    EliminarEstadoDiaDia(i);
                }

                Agente ag = Session["UsuarioLogueado"] as Agente;

                CorteLicenciaAnual cla = new CorteLicenciaAnual()
                {
                    AgenteId = DatosAgente.Agente.Id,
                    AgenteId1 = ag.Id,
                    Anio = ddl_corte_anio.Text,
                    Desde = Convert.ToDateTime(tb_corte_desde.Text),
                    NumInstrumento = tb_corte_Instrumento.Text,
                    Tipo = ddl_corteTipoLicencia.Text
                };

                cxt.CorteLicencia.AddObject(cla);
                cxt.SaveChanges();
            }

            CerrarControlCorteLicencia();

            CargarValores(DatosAgente.Agente.Id);
        }

        protected void btn_cancelar_corte_Click(object sender, EventArgs e)
        {
            CerrarControlCorteLicencia();
        }

        private void CerrarControlCorteLicencia()
        {
            lbl_Visualizarcorte.Visible = false;
            tb_corte_desde.Text = string.Empty;
            tb_corte_Instrumento.Text = string.Empty;
            btn_Cargar_Corte.Visible = false;
            btn_cancelar_corte.Visible = false;

            bool muestra = !Convert.ToBoolean(Session["corte_licencia"]);
            Session["corte_licencia"] = muestra;
            if (muestra)
            {
                collapseCorteLicencia.Attributes["class"] = "collapse in";
            }
            else
            {
                collapseCorteLicencia.Attributes["class"] = "collapse";
            }
        }

    }
}