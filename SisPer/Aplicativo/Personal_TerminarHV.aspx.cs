using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_TerminarHV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;
                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                string idstr = Session["Id"] != null ? Session["Id"].ToString() : null;
                if (idstr != null)
                {
                    if (usuariologueado.Perfil == PerfilUsuario.Personal)
                    {
                        MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
                        MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    }
                    else
                    {
                        MenuJefe.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    }

                    Model1Container cxt = new Model1Container();
                    int id = Convert.ToInt32(idstr);
                    HorarioVespertino hv = cxt.HorariosVespertinos.First(hvr => hvr.Id == id);
                    Session["HV"] = hv;
                    if (usuariologueado.Perfil == PerfilUsuario.Personal && hv.Agente.Area.Interior == true)
                    {
                        lblInterior.Text = "ATENCIÓN! Este horario vespertino pertenece a " + hv.Agente.Area.Nombre + " y lo debería terminar el agente que se encuentra a cargo.";
                    }
                    lbl_Agente.Text = hv.Agente.ApellidoYNombre;
                    lbl_Descripcion.Text = hv.Motivo;
                    lbl_Dia.Text = hv.Dia.ToLongDateString();
                    lbl_HoraDesde.Text = hv.HoraInicio;
                    lbl_HoraHasta.Text = hv.HoraFin;
                    CargarComboboxes();
                }
                else
                {
                    Response.Redirect("~/Aplicativo/MainPersonal.aspx");
                }
            }
        }

        private void CargarComboboxes()
        {
            HorarioVespertino hv = Session["HV"] as HorarioVespertino;
            using (var cxt = new ClockCardEntities())
            {
                Agente ag = hv.Agente;
                DateTime fechaSeleccionada = hv.Dia;
                int legajo = ag.Legajo;
                Nullable<int> leg = null;
                if (legajo != 0)
                {
                    leg = legajo;
                }

                var marcaciones = (from h in cxt.FICHADA
                                   where h.FIC_FECHA == fechaSeleccionada &&
                                   (h.LEG_LEGAJO == leg || leg == null)
                                   select new
                                   {
                                       Legajo = h.LEG_LEGAJO,
                                       Fecha = h.FIC_FECHA,
                                       Hora = h.FIC_HORA
                                   }).ToList();

                if (marcaciones.Count() == 0)
                {
                    List<ListItem> list = new List<ListItem>();
                    list.Add(new ListItem("No hay registros.", "0"));
                    list = list.OrderBy(a => a.Text).ToList();
                    tb_HoraDesde.DataSource = list;
                    tb_HoraDesde.DataTextField = "Text";
                    tb_HoraDesde.DataValueField = "Value";
                    tb_HoraDesde.DataBind();

                    tb_HoraHasta.DataSource = list;
                    tb_HoraHasta.DataTextField = "Text";
                    tb_HoraHasta.DataValueField = "Value";
                    tb_HoraHasta.DataBind(); 
                }
                else
                {
                    tb_HoraDesde.DataSource = marcaciones;
                    tb_HoraDesde.DataTextField = "Hora";
                    tb_HoraDesde.DataValueField = "Legajo";
                    tb_HoraDesde.DataBind();

                    tb_HoraHasta.DataSource = marcaciones;
                    tb_HoraHasta.DataTextField = "Hora";
                    tb_HoraHasta.DataValueField = "Legajo";
                    tb_HoraHasta.DataBind();
                }
                
            }

        }

        protected void btn_Terminar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                int id = (Session["HV"] as HorarioVespertino).Id;
                Model1Container cxt = new Model1Container();
                HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hvs => hvs.Id == id);
                hv.HoraInicio = tb_HoraDesde.Text;
                hv.HoraFin = tb_HoraHasta.Text;
                cxt.SaveChanges();
                ProcesosGlobales.ModificarEstadoHV(id, EstadosHorarioVespertino.Terminado, Session["UsuarioLogueado"] as Agente);
                TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(tmhs => tmhs.Tipo == "Horario Vespertino");
                ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(hv.Dia, hv.Agente, Session["UsuarioLogueado"] as Agente, HorasString.RestarHoras(hv.HoraFin, hv.HoraInicio), tmh, hv.Motivo);
                //ListadoAgentesParaGrilla.ActualizarPropiedad(hv.AgenteId, ListadoAgentesParaGrilla.PropiedadPorActualizar.HoraBonificacion, "");
                cxt = new Model1Container();
                Agente ag = cxt.Agentes.First(a => a.Id == hv.AgenteId);
                ResumenDiario rd = ag.ObtenerResumenDiario(hv.Dia);
                if (rd != null)
                {
                    Model1Container cxt1 = new Model1Container();
                    ResumenDiario rdCxt = cxt1.ResumenesDiarios.First(r => r.Id == rd.Id);

                    rdCxt.HVEnt = hv.HoraInicio;
                    rdCxt.HVSal = hv.HoraFin;
                    rdCxt.Marcaciones.Add(new Marcacion() { Manual = true, Hora = hv.HoraInicio, Anulada = false });
                    rdCxt.Marcaciones.Add(new Marcacion() { Manual = true, Hora = hv.HoraFin, Anulada = false });

                    cxt.SaveChanges();
                }
                else
                {
                    rd = new ResumenDiario();

                    rd.AgenteId = ag.Id;
                    rd.Dia = hv.Dia;
                    rd.HEntrada = "000:00";
                    rd.HSalida = "000:00";
                    rd.HVEnt = hv.HoraInicio;
                    rd.HVSal = hv.HoraFin;
                    rd.Horas = "000:00";
                    rd.Inconsistente = false;
                    rd.MarcoTardanza = false;
                    rd.MarcoProlongJornada = false;
                    rd.ObservacionInconsistente = "";

                    rd.Marcaciones.Add(new Marcacion() { Manual = true, Hora = hv.HoraInicio, Anulada = false });
                    rd.Marcaciones.Add(new Marcacion() { Manual = true, Hora = hv.HoraFin, Anulada = false });

                    cxt.ResumenesDiarios.AddObject(rd);
                }

                cxt.SaveChanges();

                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;
                if (usuariologueado.Perfil == PerfilUsuario.Personal)
                {
                    Response.Redirect("~/Aplicativo/MainPersonal.aspx");
                }
                else
                {
                    Response.Redirect("~/Aplicativo/MainJefe.aspx");
                }
            }
        }

        protected void btn_Rechazar_Click(object sender, EventArgs e)
        {
            int id = (Session["HV"] as HorarioVespertino).Id;
            Model1Container cxt = new Model1Container();
            HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hvs => hvs.Id == id);
            ProcesosGlobales.ModificarEstadoHV(id, EstadosHorarioVespertino.Cancelado, Session["UsuarioLogueado"] as Agente);

            Agente usuariologueado = Session["UsuarioLogueado"] as Agente;
            if (usuariologueado.Perfil == PerfilUsuario.Personal)
            {
                Response.Redirect("~/Aplicativo/MainPersonal.aspx");
            }
            else
            {
                Response.Redirect("~/Aplicativo/MainJefe.aspx");
            }
        }

        protected void btn_Volver_Click(object sender, EventArgs e)
        {
            Agente usuariologueado = Session["UsuarioLogueado"] as Agente;
            if (usuariologueado.Perfil == PerfilUsuario.Personal)
            {
                Response.Redirect("~/Aplicativo/MainPersonal.aspx");
            }
            else
            {
                Response.Redirect("~/Aplicativo/MainJefe.aspx");
            }
        }

        protected void CustomFieldValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_HoraDesde.Text.Length > 0;
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = !(HorasString.RestarHoras(tb_HoraHasta.Text, tb_HoraDesde.Text).Contains("-"));
            }
            catch
            {
                //los formatos de horas no corresponden por lo tanto deberia de saltar por el otro customValidator 
                //asi que este no lo muestro
                args.IsValid = true;
            }
        }

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_HoraHasta.Text.Length > 0;
        }

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try {
                Regex rex = new Regex("(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]");
                Match match = rex.Match(tb_HoraDesde.Text);
                args.IsValid = match.Success;
            }
            catch {
                args.IsValid = false;
            }
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                Regex rex = new Regex("(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]");
                Match match = rex.Match(tb_HoraHasta.Text);
                args.IsValid = match.Success;
            }
            catch
            {
                args.IsValid = false;
            }
        }
       
    }
}