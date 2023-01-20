using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SisPer.Aplicativo
{
    public partial class Jefe_Ag_SolicitarEstado : System.Web.UI.Page
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
                    int id = Convert.ToInt32(Request.QueryString["Usr"]);
                    Model1Container cxt = new Model1Container();

                    Agente agente = cxt.Agentes.First(a => a.Id == id);

                    DatosAgente.Agente = agente;

                    tb_desde.Value = DateTime.Today.ToShortDateString();
                    tb_hasta.Value = DateTime.Today.ToShortDateString();

                    CargarTiposMovimientos();
                    CargarGrillaMovimientosSolicitados();
                }
            }
        }

        private void CargarGrillaMovimientosSolicitados()
        {
            int id = Convert.ToInt32(Request.QueryString["Usr"]);
            Model1Container cxt = new Model1Container();
            Agente agente = cxt.Agentes.First(a => a.Id == id);
            DateTime primerDiaDelMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var solicitudes = (from se in agente.SolicitudesDeEstado
                               where se.FechaDesde >= primerDiaDelMes
                               select new
                               {
                                   Id = se.Id,
                                   Tipo = se.TipoEstadoAgente.Estado,
                                   Encuadre = ProcesosGlobales.ObtenerEncuadre(se),
                                   Desde = se.FechaDesde,
                                   Hasta = se.FechaHasta,
                                   Estado = se.Estado
                               }).ToList();

            gv_EstadosSolicitados.DataSource = solicitudes;
            gv_EstadosSolicitados.DataBind();
        }



        private void CargarTiposMovimientos()
        {
            Model1Container cxt = new Model1Container();
            var tiposMovimientos = (from tm in cxt.TiposEstadoAgente.Where(mh => mh.MarcaJefe)
                                    select tm).ToList();
            ddl_TipoMovimiento.DataSource = tiposMovimientos;
            ddl_TipoMovimiento.DataTextField = "Estado";
            ddl_TipoMovimiento.DataValueField = "Id";
            ddl_TipoMovimiento.DataBind();

            Analizar_Solicitar_Datos_Movimiento();
        }

        protected void gv_EstadosSolicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_EstadosSolicitados.PageIndex = e.NewPageIndex;
            CargarGrillaMovimientosSolicitados();
        }

        protected void btn_Solicitar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (IsValid)
            {
                Model1Container cxt = new Model1Container();
                SolicitudDeEstado se = new SolicitudDeEstado();
                int id = Convert.ToInt32(Request.QueryString["Usr"]);
                Agente agente = cxt.Agentes.First(a => a.Id == id);
                Agente ag = Session["UsuarioLogueado"] as Agente;
                Agente jefe = cxt.Agentes.FirstOrDefault(a => a.Id == ag.Id);
                int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
                TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);

                se.Agente = agente;
                se.SolicitadoPor = jefe;
                se.TipoEstadoAgente = tea;
                se.FechaDesde = Convert.ToDateTime(tb_desde.Value);
                se.FechaHasta = Convert.ToDateTime(tb_hasta.Value);
                se.Estado = EstadoSolicitudDeEstado.Solicitado;
                se.FechaHoraSolicitud = DateTime.Now;
                if (p_DatosExtra.Visible)
                {
                    if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
                    {
                        se.TipoEnfermedad = Convert.ToInt32(ddl_Encuadre.Text);
                    }
                    else
                    {
                        se.TipoEnfermedad = ddl_Encuadre.SelectedIndex;
                    }
                }
                se.Lugar = tb_sanatorio.Visible ? "Sanatorio " + tb_sanatorio.Text + " habitación " + tb_habitacion.Text : "";
                se.Fam_NomyAp = tb_fam_nomyap.Visible ? tb_fam_nomyap.Text : "";
                se.Fam_Parentesco = tb_fam_parentesco.Visible ? tb_fam_parentesco.Text : "";


                cxt.SolicitudesDeEstado.AddObject(se);

                #region Agrego notificacion

                Notificacion_Tipo nt = cxt.Notificacion_Tipos.FirstOrDefault(nntt => nntt.Tipo == "Automática");
                if (nt == null)
                {
                    nt = new Notificacion_Tipo() { Tipo = "Automática" };
                    cxt.Notificacion_Tipos.AddObject(nt);
                }

                Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Generada");
                if (ne == null)
                {
                    ne = new Notificacion_Estado() { Estado = "Generada" };
                    cxt.Notificacion_Estados.AddObject(ne);
                }

                Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == agente.Id);
                Notificacion notificacion = new Notificacion();

                notificacion.Descripcion = "Presentar documentación respaldatoria de la solicitud \"" + se.TipoEstadoAgente.Estado + "\" para el/los día/s " + se.FechaDesde.ToString("dd/MM/yyyy") + " al " + se.FechaHasta.ToString("dd/MM/yyyy");
                notificacion.Destinatario = destinatarioCxt;
                notificacion.ObservacionPendienteRecibir = string.Empty;

                notificacion.Tipo = nt;
                cxt.Notificaciones.AddObject(notificacion);

                Notificacion_Historial notHist = new Notificacion_Historial()
                {
                    Agente = se.Agente,
                    Estado = ne,
                    Fecha = DateTime.Now,
                    Notificacion = notificacion
                };

                cxt.Notificacion_Historiales.AddObject(notHist);

                #endregion

                cxt.SaveChanges();
                CargarGrillaMovimientosSolicitados();
            }
        }

        protected void btn_Cancelar_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idSol = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == idSol);
            se.Estado = EstadoSolicitudDeEstado.Cancelado;
            cxt.SaveChanges();
            CargarGrillaMovimientosSolicitados();
        }

        protected void VerificarDesde_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out d);
        }

        protected void VerificarHasta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_hasta.Value, out d);
        }

        protected void VerificarHastaMayorIgualQueDesde_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            DateTime hasta;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out desde) && DateTime.TryParse(tb_hasta.Value, out hasta) && hasta >= desde;
        }

        protected void PoseeSolicitudesEnElRango_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            DateTime hasta;
            if (DateTime.TryParse(tb_desde.Value, out desde) && DateTime.TryParse(tb_hasta.Value, out hasta))
            {
                bool noPoseeMovimientooSolicitud = true;
                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    noPoseeMovimientooSolicitud = noPoseeMovimientooSolicitud && !VerificarMovimientosOSolicitudesAgente(d);
                }

                args.IsValid = noPoseeMovimientooSolicitud;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void DesdeHoyEnAdelante_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out d) && d >= DateTime.Today;
        }

        /// <summary>
        /// Retorna verdadero si el agente posee solicitudes sin cancelar o si tiene movimientos agendados para ese rango
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool VerificarMovimientosOSolicitudesAgente(DateTime d)
        {
            int id = Convert.ToInt32(Request.QueryString["Usr"]);
            Model1Container cxt = new Model1Container();

            bool ret = false;

            Agente agente = cxt.Agentes.First(a => a.Id == id);
            var solicitudesAgente = agente.SolicitudesDeEstado.Where(se =>
                                        se.Estado != EstadoSolicitudDeEstado.Rechazado &&
                                        se.Estado != EstadoSolicitudDeEstado.Cancelado);

            foreach (SolicitudDeEstado se in solicitudesAgente)
            {
                for (DateTime dia = se.FechaDesde; dia <= se.FechaHasta; dia = dia.AddDays(1))
                {
                    if (dia == d)
                    {
                        ret = true;
                    }
                }
            }

            EstadoAgente ea = agente.ObtenerEstadoAgenteParaElDia(d);

            //1 si tiene un estado distinto de Fin de semana o natalicio
            int estadoAgente = (ea != null ?
                                    (
                                        (ea.TipoEstado.Estado != "Fin de semana" &&
                                        ea.TipoEstado.Estado != "Feriado" &&
                                         ea.TipoEstado.Estado != "Natalicio")
                                        ? 1
                                        : 0)
                                    : 0);

            return ret || estadoAgente > 0;
        }

        protected void ddl_TipoMovimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Analizar_Solicitar_Datos_Movimiento();
        }

        private void Analizar_Solicitar_Datos_Movimiento()
        {
            p_DatosExtra.Visible = false;
            ddl_Encuadre.Items.Clear();

            lbl_habitacion.Visible = false;
            tb_habitacion.Visible = false;
            lbl_Sanatorio.Visible = false;
            tb_sanatorio.Visible = false;

            lbl_familiar.Visible = false;
            lbl_fam_nomyAp.Visible = false;
            tb_fam_nomyap.Visible = false;
            lbl_fam_parentesco.Visible = false;
            tb_fam_parentesco.Visible = false;

            lbl_Mensaje.Text = string.Empty;

            Model1Container cxt = new Model1Container();
            int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
            TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);

            int id = Convert.ToInt32(Request.QueryString["Usr"]);

            if (tea.Estado == "Enfermedad común" || tea.Estado == "Enfermedad familiar")
            {
                int diasEnfermo = cxt.Agentes.First(a => a.Id == id).
                                        EstadosPorDiaAgente.Where(eda => eda.Dia.Year == DateTime.Today.Year &&
                                                                (eda.TipoEstado.Estado == "Enfermedad común" ||
                                                                eda.TipoEstado.Estado == "Enfermedad familiar")).Count();
                lbl_Mensaje.Text = "<b>El agente ya lleva tomado " + diasEnfermo.ToString() + " días de licencia entre \"Enfermedad común\" y \"Enfermedad familiar\".</b>\rRecuerde que tiene 20 días al año más 20 días con descuento de Fondo estímulo";

                var tiposMovimientoEnfermedad = Enum.GetValues(typeof(TipoMovimientoEnfermedad));
                ddl_Encuadre.DataSource = tiposMovimientoEnfermedad;
                ddl_Encuadre.DataBind();
                p_DatosExtra.Visible = true;

                if (tea.Estado == "Enfermedad familiar")
                {
                    lbl_familiar.Visible = true;
                    lbl_fam_nomyAp.Visible = true;
                    tb_fam_nomyap.Visible = true;
                    lbl_fam_parentesco.Visible = true;
                    tb_fam_parentesco.Visible = true;
                }

                DatosInternacion();
            }

            if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
            {
                ddl_Encuadre.Items.Add(new ListItem { Text = (DateTime.Today.Year - 2).ToString() });
                ddl_Encuadre.Items.Add(new ListItem { Text = (DateTime.Today.Year - 1).ToString() });
                ddl_Encuadre.Items.Add(new ListItem { Text = DateTime.Today.Year.ToString() });
                ddl_Encuadre.DataBind();

                int year = Convert.ToInt32(ddl_Encuadre.Text);
                TipoLicencia tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");
                LicenciaAgente licencia = cxt.Agentes.First(a => a.Id == id).Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);

                if (licencia == null)
                {
                    licencia = new LicenciaAgente()
                    {
                        AgenteId = id,
                        Anio = year,
                        DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(cxt.Agentes.First(a => a.Id == id), year),
                        DiasUsufructuadosIniciales = 0,
                        TipoLicenciaId = tli.Id
                    };

                    cxt.LicenciasAgentes.AddObject(licencia);
                    cxt.SaveChanges();
                }

                int diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                lbl_Mensaje.Text = "<b>El agente ya lleva tomado " + diasUsufructuados.ToString() + " días de licencia anual del " + ddl_Encuadre.Text + " de los " + licencia.DiasOtorgados.ToString() + " días otorgados. Quedan "+ (licencia.DiasOtorgados  - diasUsufructuados).ToString() + " días disponibles</b>";


                p_DatosExtra.Visible = true;
            }
        }

        protected void ddl_Encuadre_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["Usr"]);
            using (var cxt = new Model1Container())
            {
                int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
                TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);
                if (tea.Estado == "Enfermedad común" || tea.Estado == "Enfermedad familiar")
                {
                    DatosInternacion();
                }

                if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
                {
                    int year = Convert.ToInt32(ddl_Encuadre.Text);
                    TipoLicencia tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");
                    LicenciaAgente licencia = cxt.Agentes.First(a=>a.Id ==id).Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);

                    if (licencia == null)
                    {
                        licencia = new LicenciaAgente()
                        {
                            AgenteId = id,
                            Anio = year,
                            DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(cxt.Agentes.First(a => a.Id == id), year),
                            DiasUsufructuadosIniciales = 0,
                            TipoLicenciaId = tli.Id
                        };

                        cxt.LicenciasAgentes.AddObject(licencia);
                        cxt.SaveChanges();
                    }

                    int diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                    lbl_Mensaje.Text = "<b>El agente ya lleva tomado " + diasUsufructuados.ToString() + " días de licencia anual del " + ddl_Encuadre.Text + " de los " + licencia.DiasOtorgados.ToString() + " días otorgados. Quedan " + (licencia.DiasOtorgados - diasUsufructuados).ToString() + " días disponibles</b>";

                }
            }
        }
        protected void DatosInternacion()
        {
            //el textoseleccionado puede ser años si es de licencia o no si es por enfermedad
            string textoSeleccionado = ddl_Encuadre.Text;
            int year = 0;
            if (!int.TryParse(textoSeleccionado, out year))
            {
                if ((TipoMovimientoEnfermedad)(ddl_Encuadre.SelectedIndex) == TipoMovimientoEnfermedad.Internacion)
                {
                    lbl_habitacion.Visible = true;
                    tb_habitacion.Visible = true;
                    lbl_Sanatorio.Visible = true;
                    tb_sanatorio.Visible = true;
                }
                else
                {
                    lbl_habitacion.Visible = false;
                    tb_habitacion.Visible = false;
                    tb_habitacion.Text = string.Empty;
                    lbl_Sanatorio.Visible = false;
                    tb_sanatorio.Visible = false;
                    tb_sanatorio.Text = string.Empty;
                }
            }
        }

        protected void cv_sanatorio_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
                (p_DatosExtra.Visible == false) ||
                (p_DatosExtra.Visible == true && tb_sanatorio.Visible == false) ||
                (p_DatosExtra.Visible == true && tb_sanatorio.Visible == true && tb_sanatorio.Text.Length > 0);
        }

        protected void cv_habitacion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
               (p_DatosExtra.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_habitacion.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_habitacion.Visible == true && tb_habitacion.Text.Length > 0);
        }

        protected void cv_fam_nomyap_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
               (p_DatosExtra.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_fam_nomyap.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_fam_nomyap.Visible == true && tb_fam_nomyap.Text.Length > 0);
        }

        protected void cv_fam_parentesco_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid =
               (p_DatosExtra.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_fam_parentesco.Visible == false) ||
               (p_DatosExtra.Visible == true && tb_fam_parentesco.Visible == true && tb_fam_parentesco.Text.Length > 0);
        }

        protected void cv_tipoMovimiento_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void cv_VerificarDiasDisponiblesLicencia_ServerValidate(object source, ServerValidateEventArgs args)
        {
            using (var cxt = new Model1Container())
            {
                string textoSeleccionado = ddl_Encuadre.Text;
                int year = 0;
                int id = Convert.ToInt32(Request.QueryString["Usr"]);
                var agente = cxt.Agentes.First(a => a.Id == id);
                DateTime desde = Convert.ToDateTime(tb_desde.Value);
                DateTime hasta = Convert.ToDateTime(tb_hasta.Value);
                int diasSolicitados = Convert.ToInt32((hasta - desde).TotalDays + 1);
                TipoLicencia tli = new TipoLicencia();
                LicenciaAgente licencia = new LicenciaAgente();
                int diasUsufructuados = 0;

                switch (ddl_TipoMovimiento.SelectedItem.Text)
                {
                    case "Licencia especial invierno":
                        tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia especial invierno");
                        licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);
                        if (licencia == null)
                        {
                            licencia = new LicenciaAgente()
                            {
                                AgenteId = id,
                                Anio = year,
                                DiasOtorgados = 10,
                                DiasUsufructuadosIniciales = 0,
                                TipoLicenciaId = tli.Id
                            };

                            cxt.LicenciasAgentes.AddObject(licencia);
                            cxt.SaveChanges();
                        }

                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                        args.IsValid = (licencia.DiasOtorgados - (diasSolicitados + diasUsufructuados)) >= 0;
                        break;

                    case "Enfermedad común":
                        tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia enfermedad común");
                        licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);
                        if (licencia == null)
                        {
                            licencia = new LicenciaAgente()
                            {
                                AgenteId = id,
                                Anio = year,
                                DiasOtorgados = 45,
                                DiasUsufructuadosIniciales = 0,
                                TipoLicenciaId = tli.Id
                            };

                            cxt.LicenciasAgentes.AddObject(licencia);
                            cxt.SaveChanges();
                        }

                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                        args.IsValid = (licencia.DiasOtorgados - (diasSolicitados + diasUsufructuados)) >= 0;
                        break;

                    case "Enfermedad familiar":
                        tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia enfermedad familiar");
                        licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);
                        if (licencia == null)
                        {
                            licencia = new LicenciaAgente()
                            {
                                AgenteId = id,
                                Anio = year,
                                DiasOtorgados = 30,
                                DiasUsufructuadosIniciales = 0,
                                TipoLicenciaId = tli.Id
                            };

                            cxt.LicenciasAgentes.AddObject(licencia);
                            cxt.SaveChanges();
                        }

                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                        args.IsValid = (licencia.DiasOtorgados - (diasSolicitados + diasUsufructuados)) >= 0;
                        break;

                    case "Licencia Anual (Saldo)":
                        year = Convert.ToInt32(ddl_Encuadre.Text);
                        tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");
                        licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);

                        if (licencia == null)
                        {
                            licencia = new LicenciaAgente()
                            {
                                AgenteId = id,
                                Anio = year,
                                DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(agente, year),
                                DiasUsufructuadosIniciales = 0,
                                TipoLicenciaId = tli.Id
                            };

                            cxt.LicenciasAgentes.AddObject(licencia);
                            cxt.SaveChanges();
                        }

                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                        args.IsValid = (licencia.DiasOtorgados - (diasSolicitados + diasUsufructuados)) >= 0;
                        break;

                    case "Licencia Anual (Anticipo)":
                        year = Convert.ToInt32(ddl_Encuadre.Text);
                        tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");
                        licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);
                        if (licencia == null)
                        {
                            licencia = new LicenciaAgente()
                            {
                                AgenteId = id,
                                Anio = year,
                                DiasOtorgados = ProcesosGlobales.ObtenerDiasLicenciaAnualAgente(agente, year),
                                DiasUsufructuadosIniciales = 0,
                                TipoLicenciaId = tli.Id
                            };

                            cxt.LicenciasAgentes.AddObject(licencia);
                            cxt.SaveChanges();
                        }

                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                        args.IsValid = (licencia.DiasOtorgados - (diasSolicitados + diasUsufructuados)) >= 0;
                        break;

                    default:
                        args.IsValid = true;
                        break;
                }
            }
        }

        protected void cv_VerificarSiTieneDiasLicenciaAnteriorAnticipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool valido = true;
            
            //aca controlar licencia anticipo, si tiene saldo licencia año anterior no puede tomar licencia anticipo
            using (var cxt = new Model1Container())
            {
                int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
                TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);
                string textoSeleccionado = ddl_Encuadre.Text;
                int year = 0;
                int id = Convert.ToInt32(Request.QueryString["Usr"]);
                var agente = cxt.Agentes.First(a => a.Id == id);
                DateTime desde = Convert.ToDateTime(tb_desde.Value);
                DateTime hasta = Convert.ToDateTime(tb_hasta.Value);
                int diasSolicitados = Convert.ToInt32((hasta - desde).TotalDays + 1);
                TipoLicencia tli = new TipoLicencia();
                LicenciaAgente licencia = new LicenciaAgente();
                int diasUsufructuados = 0;

                if (tea.Estado == "Licencia Anual (Anticipo)")
                {
                    year = Convert.ToInt32(ddl_Encuadre.Text) - 1;

                    tli = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");
                    licencia = agente.Licencias.FirstOrDefault(l => l.Anio == year && l.Tipo.Id == tli.Id);
                    if (licencia != null)
                    {
                        diasUsufructuados = ProcesosGlobales.ObtenerDiasUsufructuados(licencia);
                    }
                    valido = (licencia == null || diasUsufructuados == 0);
                }
                        
            }

            args.IsValid = valido;
            
        }
    }
}