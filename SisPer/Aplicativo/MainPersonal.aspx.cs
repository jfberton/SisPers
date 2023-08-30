using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SisPer.Aplicativo
{
    public partial class MainPersonal : System.Web.UI.Page
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
                    CargarFrancos();
                    CargarHVS();
                    CargarSolicitudesMedico();
                    CargarOtrasSolicitudes();
                    CargarCambiosPendientes();

                    bool mostrarMensaje = Convert.ToBoolean(Session["MostrarMensageBienvenida"]);
                    Session["MostrarMensageBienvenida"] = false;
                    if (mostrarMensaje)
                    {
                        MensageBienvenida.Show();
                    }
                }
            }
        }

        private void CargarFrancos()
        {

            Model1Container cxt = new Model1Container();

            //Cargo la grilla con los francos a aprobar
            var francosPorAprobar = (from fc in cxt.Francos
                                     where fc.Estado == EstadosFrancos.AprobadoJefe
                                     select new
                                     {
                                         Id = fc.Id,
                                         Legajo = fc.Agente.Legajo,
                                         Agente = fc.Agente.ApellidoYNombre,
                                         Estado = fc.Estado,
                                         Dia = fc.FechaSolicitud,
                                         DiaInicial = (from d in fc.DiasFranco select d.Dia).Min(),
                                         CantidadDias = fc.DiasFranco.Count,
                                         Horas = fc.DiasFranco.Count * 7
                                     }).OrderBy(l => l.Legajo).ToList();
            GridViewFrancosPorAprobar.DataSource = francosPorAprobar;
            GridViewFrancosPorAprobar.DataBind();

            //Cargo la grilla con los francos aprobados
            var francosAprobados = (from fc in cxt.Francos
                                    where fc.Estado == EstadosFrancos.AprobadoPersonal
                                    select new
                                    {
                                        Id = fc.Id,
                                        Legajo = fc.Agente.Legajo,
                                        Agente = fc.Agente.ApellidoYNombre,
                                        Estado = fc.Estado,
                                        Dia = fc.FechaSolicitud,
                                        DiaInicial = (from d in fc.DiasFranco select d.Dia).Min(),
                                        CantidadDias = fc.DiasFranco.Count,
                                        Horas = fc.DiasFranco.Count * 7
                                    }).OrderBy(l => l.Legajo).ToList();

            GridViewFrancosAprobados.DataSource = francosAprobados;
            GridViewFrancosAprobados.DataBind();
        }

        private void CargarHVS()
        {
            Model1Container cxt = new Model1Container();
            List<HorarioVespertino> horariosVespertinosAprobados = new List<HorarioVespertino>();
            foreach (var hv in cxt.HorariosVespertinos)
            {
                if (hv.Estado == EstadosHorarioVespertino.Aprobado)
                {
                    horariosVespertinosAprobados.Add(hv);
                }
            }

            var hvsSolicitados = (from hv in horariosVespertinosAprobados
                                  select new
                                  {
                                      Id = hv.Id,
                                      Legajo = hv.Agente.Legajo,
                                      Agente = hv.Agente.ApellidoYNombre,
                                      Dia = hv.Dia,
                                      Desde = hv.HoraInicio,
                                      Hasta = hv.HoraFin,
                                      Motivo = hv.Motivo,
                                      Horas = HorasString.RestarHoras(hv.HoraFin, hv.HoraInicio)
                                  }).OrderBy(l => l.Legajo).ToList();

            GridViewHVPendientesAprobar.DataSource = hvsSolicitados;
            GridViewHVPendientesAprobar.DataBind();
        }

        private void CargarCambiosPendientes()
        {
            Model1Container cxt = new Model1Container();
            var cambios = (from c in cxt.CambiosPendientes.Include("Agente")
                           select new
                           {
                               Usr = c.Agente.Usr,
                               Agente = c.Agente.ApellidoYNombre,
                               Legajo = c.Agente.Legajo
                           }).ToList();
            GridViewCambiosPendientes.DataSource = cambios;
            GridViewCambiosPendientes.DataBind();
        }

        protected void gridViewHVPendientesAprobar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHVPendientesAprobar.PageIndex = e.NewPageIndex;
            CargarHVS();
        }

        protected void btn_AprobarFrancoPorAprobar_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Session["IdFranco"] = id;
            Response.Redirect("~/Aplicativo/Personal_AprobarFranco.aspx");
        }

        protected void btn_AprobarFranco_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            ProcesosGlobales.ModificarEstadoFranco(id, EstadosFrancos.Aprobado, Session["UsuarioLogueado"] as Agente);
            CargarFrancos();
        }

        protected void btn_RechazarFranco_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            ProcesosGlobales.ModificarEstadoFranco(id, EstadosFrancos.Cancelado, Session["UsuarioLogueado"] as Agente);
            CargarFrancos();
        }

        protected void GridViewFrancosPorAprobar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFrancosPorAprobar.PageIndex = e.NewPageIndex;
            CargarFrancos();
        }

        protected void GridViewFrancosAprobados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFrancosAprobados.PageIndex = e.NewPageIndex;
            CargarFrancos();
        }

        protected void gridViewCambiosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewCambiosPendientes.PageIndex = e.NewPageIndex;
            CargarCambiosPendientes();
        }

        protected void btn_AnalizarHV_Click(object sender, ImageClickEventArgs e)
        {
            Session["Id"] = ((ImageButton)sender).CommandArgument;

            Response.Redirect("~/Aplicativo/Personal_TerminarHV.aspx");
        }

        protected void btn_AnalizarCambios_Click(object sender, ImageClickEventArgs e)
        {
            Session["Usr"] = ((ImageButton)sender).CommandArgument;

            Response.Redirect("~/Aplicativo/Personal_AprobarCambiosDatosAgentes.aspx");
        }

        protected void CargarSolicitudesMedico(int legajo = 0)
        {
            Model1Container cxt = new Model1Container();

            var solicitudes = (from se in cxt.SolicitudesDeEstado
                               where
                                   se.Estado == EstadoSolicitudDeEstado.Solicitado &&
                                   se.TipoEstadoAgente.Estado.Contains("Enfermedad") &&
                                   (se.Agente.Legajo == legajo || legajo == 0)
                               select se).AsEnumerable().Select(x => new
                               {
                                   prioridadOrden = x.TipoEstadoAgente.OrdenPrioridad,
                                   Id = x.Id,
                                   Agente = x.Agente.ApellidoYNombre,
                                   Legajo = x.Agente.Legajo,
                                   Tipo = x.TipoEstadoAgente.Estado,
                                   Desde = x.FechaDesde,
                                   Hasta = x.FechaHasta,
                                   Encuadre = ProcesosGlobales.ObtenerEncuadre(x),
                                   Fechahora = x.FechaHoraSolicitud,
                                   Familiar = x.Fam_NomyAp,
                                   Parentesco = x.Fam_Parentesco,
                                   Lugar = x.Lugar
                               }).ToList();

            gv_MedicosSolicitados.DataSource = solicitudes.OrderBy(d => d.prioridadOrden).ThenByDescending(d => d.Fechahora).ToList();
            gv_MedicosSolicitados.DataBind();
        }

        protected void CargarOtrasSolicitudes(int legajo = 0)
        {
            Model1Container cxt = new Model1Container();

            var solicitudes = (from se in cxt.SolicitudesDeEstado
                               where
                                   se.Estado == EstadoSolicitudDeEstado.Solicitado &&
                                   !se.TipoEstadoAgente.Estado.Contains("Enfermedad") &&
                                   (se.Agente.Legajo == legajo || legajo == 0)
                               select se).AsEnumerable().Select(x => new
                               {
                                   prioridadOrden = x.TipoEstadoAgente.OrdenPrioridad,
                                   Id = x.Id,
                                   Agente = x.Agente.ApellidoYNombre,
                                   Legajo = x.Agente.Legajo,
                                   Tipo = x.TipoEstadoAgente.Estado,
                                   Desde = x.FechaDesde,
                                   Hasta = x.FechaHasta,
                                   Encuadre = ProcesosGlobales.ObtenerEncuadre(x),
                                   Fechahora = x.FechaHoraSolicitud,
                                   Familiar = x.Fam_NomyAp,
                                   Parentesco = x.Fam_Parentesco,
                                   Lugar = x.Lugar
                               }).ToList();

            gv_otras_solicitudes.DataSource = solicitudes.OrderBy(d => d.prioridadOrden).ThenByDescending(d => d.Fechahora).ToList();
            gv_otras_solicitudes.DataBind();
        }

        protected void btn_Administrar_Click(object sender, ImageClickEventArgs e)
        {
            int id_solicitud = 0;
            if (int.TryParse(((ImageButton)sender).CommandArgument, out id_solicitud))
            {
                administrar_solicitud_medico(id_solicitud);
            }
        }

        private void administrar_solicitud_medico(int id_solicitud)
        {
            using (var cxt = new Model1Container())
            {
                SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id_solicitud);

                if (se != null)
                {
                    lbl_pre_act_elect.Text = "E20-" + DateTime.Today.Year.ToString() + "-";
                    lbl_agente.Text = "Legajo: " + se.Agente.Legajo + ". Apellido y nombre: " + se.Agente.ApellidoYNombre;
                    lbl_ficha_medica.Text = se.Agente.Legajo_datos_laborales.FichaMedica;
                    lbl_domicilio.Text = se.Agente.Legajo_datos_personales.Domicilio + " - " + se.Agente.Legajo_datos_personales.Domicilio_localidad;
                    lbl_tipo.Text = se.TipoEstadoAgente.Estado;
                    lbl_encuadre.Text = ProcesosGlobales.ObtenerEncuadre(se) + " " + se.Lugar;
                    if (se.Fam_NomyAp != null)
                    {
                        lbl_familiar.Text = se.Fam_NomyAp + " - " + se.Fam_Parentesco;
                        fila_familiar.Visible = true;
                    }
                    else
                    {
                        lbl_familiar.Text = "";
                        fila_familiar.Visible = false;
                    }
                    lbl_fecha_desde.Text = se.FechaDesde.ToShortDateString();
                    lbl_modal_solicitud_Id.Text = se.Id.ToString();
                    lbl_modal_titulo.Text = "Detalle de solicitud de médico";


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "admin_solicitud_medico", "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#modal_solicitud_medico').modal('show')});</script>", false);
                }
            }
        }

        protected void gv_EstadosSolicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_MedicosSolicitados.PageIndex = e.NewPageIndex;
            CargarSolicitudesMedico();
        }


        protected void btn_filtrarSolicitudes_Click(object sender, EventArgs e)
        {
            int legajo = 0;
            if (!int.TryParse(tb_LegajoBuscado.Value, out legajo))
            {
                if (tb_LegajoBuscado.Value != string.Empty)
                {
                    Controles.MessageBox.Show(this, "El legajo ingresado es inválido", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }

            CargarSolicitudesMedico(legajo);
            tb_LegajoBuscado.Value = string.Empty;
        }

        protected void btn_imprimir_solicitud_medico_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_solicitud_Id.Text);
            if(tb_actuacion_electronica.Text != string.Empty)
            {
                Model1Container cxt = new Model1Container();
                SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id);

                Informe_solicitud_medico reporte = new Informe_solicitud_medico();
                reporte.actuacion_electronica = "E20-" + DateTime.Today.Year.ToString() + "-" + tb_actuacion_electronica.Text + "-Ae";
                reporte.solicitud = se;

                byte[] bytes = reporte.Generar_informe();

                if (bytes != null)
                {
                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                else
                {
                    Controles.MessageBox.Show(this, "Ocurrió un error al obtener los datos de la solicitud", Controles.MessageBox.Tipo_MessageBox.Info);
                }
            }
        }

        protected void btn_aprobar_solicitud_medico_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_solicitud_Id.Text);

            Model1Container cxt = new Model1Container();
            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id);
            se.FechaHasta = Convert.ToDateTime(tb_fecha_hasta.Text);
            cxt.SaveChanges();

            if (se != null)
            {
                int year = 0;
                if (se.TipoEstadoAgente.Estado == "Licencia Anual" || se.TipoEstadoAgente.Estado == "Licencia Anual (Saldo)" || se.TipoEstadoAgente.Estado == "Licencia Anual (Anticipo)")
                {
                    year = Convert.ToInt32(se.TipoEnfermedad);
                }

                for (DateTime dia = se.FechaDesde; dia <= se.FechaHasta; dia = dia.AddDays(1))
                {
                    ProcesosGlobales.AgendarEstadoDiaAgente(se.SolicitadoPor, se.Agente, dia, se.TipoEstadoAgente, year);
                }

                se.Estado = EstadoSolicitudDeEstado.Aprobado;
                cxt.SaveChanges();
            }

            CargarSolicitudesMedico();
        }

        protected void btn_rechazar_solicitud_medico_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_solicitud_Id.Text);

            Model1Container cxt = new Model1Container();
            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id);

            if (se != null)
            {
                se.Estado = EstadoSolicitudDeEstado.Rechazado;
                cxt.SaveChanges();
            }

            CargarSolicitudesMedico();
        }

        protected void gv_otras_solicitudes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_otras_solicitudes.PageIndex = e.NewPageIndex;
            CargarOtrasSolicitudes();
        }

        protected void btn_Aprobar_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            Model1Container cxt = new Model1Container();
            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id);

            if (se != null)
            {
                int year = 0;
                if (se.TipoEstadoAgente.Estado == "Licencia Anual" || se.TipoEstadoAgente.Estado == "Licencia Anual (Saldo)" || se.TipoEstadoAgente.Estado == "Licencia Anual (Anticipo)")
                {
                    year = Convert.ToInt32(se.TipoEnfermedad);
                }

                for (DateTime dia = se.FechaDesde; dia <= se.FechaHasta; dia = dia.AddDays(1))
                {
                    ProcesosGlobales.AgendarEstadoDiaAgente(se.SolicitadoPor, se.Agente, dia, se.TipoEstadoAgente, year);
                }

                se.Estado = EstadoSolicitudDeEstado.Aprobado;
                cxt.SaveChanges();
            }

            CargarOtrasSolicitudes();
        }

        protected void btn_Rechazar_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            Model1Container cxt = new Model1Container();
            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(s => s.Id == id);

            if (se != null)
            {
                se.Estado = EstadoSolicitudDeEstado.Rechazado;
                cxt.SaveChanges();
            }

            CargarOtrasSolicitudes();
        }

        protected void btn_filtrarOtrasSolicitudes_Click(object sender, EventArgs e)
        {
            int legajo = 0;
            if (!int.TryParse(tb_legajo_otras_solicitudes.Value, out legajo))
            {
                if (tb_LegajoBuscado.Value != string.Empty)
                {
                    Controles.MessageBox.Show(this, "El legajo ingresado es inválido", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }

            CargarOtrasSolicitudes(legajo);
            tb_legajo_otras_solicitudes.Value = string.Empty;
        }
    }
}