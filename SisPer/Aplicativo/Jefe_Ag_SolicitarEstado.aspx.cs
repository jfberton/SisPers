using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Menues;
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

        private Model1Container cxt;
        protected void Page_Load(object sender, EventArgs e)
        {
            cxt = new Model1Container();

            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                Cargar_datos_agente_a_solicitar();

                //Menues
                if (ag.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                    MenuPersonalJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuAgente1.Visible = false;
                    MenuJefe1.Visible = false;
                }
                else
                {
                    MenuPersonalAgente1.Visible = false;
                    MenuPersonalJefe1.Visible = false;
                    MenuAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                    MenuJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                }

                tb_desde.Value = DateTime.Today.ToShortDateString();
                tb_hasta.Value = DateTime.Today.ToShortDateString();

                CargarTiposMovimientos();
                CargarGrillaMovimientosSolicitados();
            }
        }

        private void Cargar_datos_agente_a_solicitar()
        {
            Agente agente = Obtener_agente_a_solicitar();
            DatosAgente.Agente = agente;
            tb_domicilio.Text = agente.Legajo_datos_personales.Domicilio;
            tb_localidad.Text = agente.Legajo_datos_personales.Domicilio_localidad;
        }

        private Agente Obtener_agente_a_solicitar()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;

            Agente agente = null;

            if (Request.QueryString["Usr"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["Usr"]);
                agente = cxt.Agentes.Include("Legajo_datos_personales").FirstOrDefault(a => a.Id == id);
            }
            else
            {
                agente = ag;
            }

            return agente;
        }

        private Agente Obtener_Jefe_que_solicita()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;

            Agente agente = ag;

            return agente;
        }

        private void CargarGrillaMovimientosSolicitados()
        {
            Agente agente = Obtener_agente_a_solicitar();

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
            Agente ag = Session["UsuarioLogueado"] as Agente;
            List<TipoEstadoAgente> listado_movimientos_permitidos = new List<TipoEstadoAgente>();

            if (ag.Jefe || ag.JefeTemporal)
            {
                listado_movimientos_permitidos = (from tm in cxt.TiposEstadoAgente.Where(mh => mh.MarcaJefe)
                                                  select tm).ToList();
            }
            else
            {
                listado_movimientos_permitidos = (from tm in cxt.TiposEstadoAgente.Where(
                                                                                            mh => mh.MarcaJefe &&
                                                                                                    (
                                                                                                        mh.Estado.Contains("Enfermedad") ||
                                                                                                        mh.Estado.Contains("Pap, mamografia") ||
                                                                                                        mh.Estado.Contains("Donación de sangre") ||
                                                                                                        mh.Estado.Contains("Fallecimiento Familiar")
                                                                                                    )
                                                                                        )
                                                  select tm).ToList();
            }

            ddl_TipoMovimiento.DataSource = listado_movimientos_permitidos;
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
                SolicitudDeEstado se = new SolicitudDeEstado();
                Agente agente = Obtener_agente_a_solicitar();
                Agente ag = Session["UsuarioLogueado"] as Agente;
                TipoMovimientoEnfermedad tipoMovimientoEnfermedad = (TipoMovimientoEnfermedad)Enum.Parse(typeof(TipoMovimientoEnfermedad), ddl_Encuadre.Text);

                Agente jefe = Obtener_Jefe_que_solicita();

                agente.Legajo_datos_personales.Domicilio = tb_domicilio.Text;
                agente.Legajo_datos_personales.Domicilio_localidad = tb_localidad.Text;

                int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);

                se.AgenteId = agente.Id;
                se.AgenteId1 = jefe.Id;
                se.TipoEstadoAgenteId = tipoEstadoId;
                se.FechaDesde = Convert.ToDateTime(tb_desde.Value);
                se.FechaHasta = hasta_row.Visible? Convert.ToDateTime(tb_hasta.Value) : Convert.ToDateTime(tb_desde.Value);
                se.Estado = EstadoSolicitudDeEstado.Solicitado;
                se.FechaHoraSolicitud = DateTime.Now;
                if (p_DatosExtra.Visible)
                {
                    if (ddl_TipoMovimiento.SelectedItem.Text == "Licencia Anual"
                        || ddl_TipoMovimiento.SelectedItem.Text == "Licencia Anual (Saldo)"
                        || ddl_TipoMovimiento.SelectedItem.Text == "Licencia Anual (Anticipo)")
                    {
                        se.TipoEnfermedad = Convert.ToInt32(ddl_Encuadre.Text);
                    }
                    else
                    {
                        se.TipoEnfermedad = (int)tipoMovimientoEnfermedad;
                    }
                }

                if (tb_sanatorio.Visible || ddl_provincias.Visible)
                {
                    if (tipoMovimientoEnfermedad == TipoMovimientoEnfermedad.Consultorio_a_su_regreso_de)
                        se.Lugar = "Provincia " + tb_sanatorio.Text;

                    if (tipoMovimientoEnfermedad == TipoMovimientoEnfermedad.Internacion)
                        se.Lugar = "Sanatorio " + tb_sanatorio.Text + " habitación " + tb_habitacion.Text;
                }


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
                notificacion.AgenteId = destinatarioCxt.Id;
                notificacion.ObservacionPendienteRecibir = string.Empty;

                notificacion.Tipo = nt;
                cxt.Notificaciones.AddObject(notificacion);

                Notificacion_Historial notHist = new Notificacion_Historial()
                {
                    AgenteId = se.Agente.Id,
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
            bool ret = false;
            Agente agente = Obtener_agente_a_solicitar();
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




            int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
            int id = Obtener_agente_a_solicitar().Id;
            TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);

            if (tea.Estado.Contains("Enfermedad") ||
                tea.Estado.Contains("Pap, mamografia") ||
                tea.Estado.Contains("Donación de sangre") ||
                tea.Estado.Contains("Fallecimiento Familiar"))
            {
                hasta_row.Visible = false;
            }
            else
            { 
                hasta_row.Visible = true;
            }

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
                    ddl_Encuadre.Items.Remove(ddl_Encuadre.Items.FindByValue("Consultorio"));
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
                lbl_Mensaje.Text = "<b>El agente ya lleva tomado " + diasUsufructuados.ToString() + " días de licencia anual del " + ddl_Encuadre.Text + " de los " + licencia.DiasOtorgados.ToString() + " días otorgados. Quedan " + (licencia.DiasOtorgados - diasUsufructuados).ToString() + " días disponibles</b>";


                p_DatosExtra.Visible = true;
            }
        }

        protected void ddl_Encuadre_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Obtener_agente_a_solicitar().Id;

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
                lbl_Mensaje.Text = "<b>El agente ya lleva tomado " + diasUsufructuados.ToString() + " días de licencia anual del " + ddl_Encuadre.Text + " de los " + licencia.DiasOtorgados.ToString() + " días otorgados. Quedan " + (licencia.DiasOtorgados - diasUsufructuados).ToString() + " días disponibles</b>";

            }
        }
        protected void DatosInternacion()
        {
            string tipo_solicitud = ddl_TipoMovimiento.SelectedItem.Text;
            string textoSeleccionado = ddl_Encuadre.Text;

            lbl_habitacion.Visible = false;
            tb_habitacion.Visible = false;
            tb_habitacion.Text = string.Empty;
            lbl_Sanatorio.Visible = false;
            tb_sanatorio.Visible = false;
            tb_sanatorio.Text = string.Empty;
            ddl_provincias.Visible = false;

            TipoMovimientoEnfermedad tipoMovimientoEnfermedad = (TipoMovimientoEnfermedad)Enum.Parse(typeof(TipoMovimientoEnfermedad), ddl_Encuadre.Text);

            if (tipo_solicitud.Contains("Enfermedad"))
            {
                if (tipoMovimientoEnfermedad == TipoMovimientoEnfermedad.Internacion)
                {
                    lbl_habitacion.Visible = true;
                    tb_habitacion.Visible = true;
                    lbl_Sanatorio.Visible = true;
                    lbl_Sanatorio.Text = "Sanatorio";
                    tb_sanatorio.Visible = true;
                }

                if (tipoMovimientoEnfermedad == TipoMovimientoEnfermedad.Consultorio_a_su_regreso_de)
                {
                    lbl_Sanatorio.Visible = true;
                    lbl_Sanatorio.Text = "Provincia";
                    ddl_provincias.Items.Clear();
                    ddl_provincias.Items.Add(new ListItem { Text = "Seleccione una provincia", Value = "0" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Buenos Aires", Value = "1" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Catamarca", Value = "2" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Chaco", Value = "3" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Chubut", Value = "4" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Córdoba", Value = "5" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Corrientes", Value = "6" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Entre Ríos", Value = "7" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Formosa", Value = "8" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Jujuy", Value = "9" });
                    ddl_provincias.Items.Add(new ListItem { Text = "La Pampa", Value = "10" });
                    ddl_provincias.Items.Add(new ListItem { Text = "La Rioja", Value = "11" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Mendoza", Value = "12" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Misiones", Value = "13" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Neuquén", Value = "14" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Río Negro", Value = "15" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Salta", Value = "16" });
                    ddl_provincias.Items.Add(new ListItem { Text = "San Juan", Value = "17" });
                    ddl_provincias.Items.Add(new ListItem { Text = "San Luis", Value = "18" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Santa Cruz", Value = "19" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Santa Fé", Value = "20" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Santiago del Estero", Value = "21" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Tierra del Fuego", Value = "22" });
                    ddl_provincias.Items.Add(new ListItem { Text = "Tucumán", Value = "23" });
                    ddl_provincias.Visible = true;

                    tb_sanatorio.Visible = false;
                }
            }
        }

        protected void ddl_provincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_provincias.SelectedIndex != 0)
            {
                tb_sanatorio.Text = ddl_provincias.SelectedItem.Text;
            }
            else
            {
                tb_sanatorio.Text = string.Empty;
            }

        }

        protected void cv_sanatorio_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tipo_solicitud = ddl_TipoMovimiento.SelectedItem.Text;
            TipoMovimientoEnfermedad tipoMovimientoEnfermedad = (TipoMovimientoEnfermedad)Enum.Parse(typeof(TipoMovimientoEnfermedad), ddl_Encuadre.Text);

            if (tipo_solicitud.Contains("Enfermedad"))
            {
                switch (tipoMovimientoEnfermedad)
                {
                    case TipoMovimientoEnfermedad.Consultorio:
                        args.IsValid = true;
                        break;
                    case TipoMovimientoEnfermedad.Domicilio:
                        args.IsValid = true;
                        break;
                    case TipoMovimientoEnfermedad.Internacion:
                        cv_sanatorio.ErrorMessage = "Debe ingresar el sanatorio donde se encuentra internado.";
                        cv_sanatorio.Text = "<img src='../Imagenes/exclamation.gif' title='Debe ingresar el sanatorio donde se encuentra internado.' />";
                        args.IsValid = tb_sanatorio.Text.Length > 0;
                        break;
                    case TipoMovimientoEnfermedad.Consultorio_a_su_regreso_de:
                        cv_sanatorio.ErrorMessage = "Debe seleccionar la provincia a la cual se dirije el agente para su tratamiento.";
                        cv_sanatorio.Text = "<img src='../Imagenes/exclamation.gif' title='Debe seleccionar la provincia a la cual se dirije el agente para su tratamiento.' />";
                        args.IsValid = tb_sanatorio.Text.Length > 0;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cv_habitacion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string tipo_solicitud = ddl_TipoMovimiento.SelectedItem.Text;
            TipoMovimientoEnfermedad tipoMovimientoEnfermedad = (TipoMovimientoEnfermedad)Enum.Parse(typeof(TipoMovimientoEnfermedad), ddl_Encuadre.Text);

            if (tipo_solicitud.Contains("Enfermedad"))
            {
                if (tipoMovimientoEnfermedad == TipoMovimientoEnfermedad.Internacion)
                {
                    args.IsValid = tb_habitacion.Text.Length > 0;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                args.IsValid = true;
            }
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
            string textoSeleccionado = ddl_Encuadre.Text;
            int year = 0;
            Agente agente = Obtener_agente_a_solicitar();
            int id = agente.Id;
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

        protected void cv_VerificarSiTieneDiasLicenciaAnteriorAnticipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool valido = true;

            //aca controlar licencia anticipo, si tiene saldo licencia año anterior no puede tomar licencia anticipo
            int tipoEstadoId = Convert.ToInt32(ddl_TipoMovimiento.SelectedValue);
            TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(t => t.Id == tipoEstadoId);
            string textoSeleccionado = ddl_Encuadre.Text;
            int year = 0;
            Agente agente = Obtener_agente_a_solicitar();
            int id = agente.Id;
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

            args.IsValid = valido;

        }

        protected void cv_domicilio_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_domicilio.Text.Length > 0;
        }

        protected void cv_localidad_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_localidad.Text.Length > 0;
        }


    }
}