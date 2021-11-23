using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class MainPandemia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente agSession = Session["UsuarioLogueado"] as Agente;

                if (agSession == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                int id = 0;
                string idUsr = Request.QueryString["Usr"];
                if (idUsr != null && idUsr != string.Empty)
                {
                    id = Convert.ToInt32(idUsr);
                }
                else
                {
                    id = agSession.Id;
                }

                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                Session["Agente"] = ag;
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                DatosAgente1.Agente = ag;

                Calendar1.SelectedDate = DateTime.Today;

                CargarMarcacionesFecha();

                bool mostrarMensaje = Convert.ToBoolean(Session["MostrarMensageBienvenida"]);
                Session["MostrarMensageBienvenida"] = false;
                if (mostrarMensaje)
                {
                    MensageBienvenida.Show();
                }

                btn_continuar_main_agente.Visible = ag.Area.Interior == true;
            }
        }

        protected void btn_continuar_main_agente_Click(object sender, EventArgs e)
        {
            div_huellas.Visible = true;
            btn_continuar_main_agente.Visible = false;
        }

        #region Huellas

        public List<Notificacion> Notificaciones_por_enviar { get; set; }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
            }
            else
            {
                if (e.Day.Date <= DateTime.Today)
                {
                    using (var cxt = new Model1Container())
                    {
                        Notificaciones_por_enviar = (from nn in cxt.Notificaciones
                                                     where
                                                     nn.AgenteId == DatosAgente1.Agente.Id &&
                                                     nn.HistorialEstadosNotificacion.FirstOrDefault(nh => nh.Estado.Estado == "Enviada") == null
                                                     select nn).ToList();

                        Notificacion notFecha = Notificaciones_por_enviar.FirstOrDefault(nn => nn.HistorialEstadosNotificacion.First(nh => nh.Estado.Estado == "Generada").Fecha.Date == e.Day.Date);

                        if (notFecha != null)
                        {
                            e.Cell.BackColor = Color.OrangeRed;
                            e.Cell.ToolTip = "Debe envio de notificación N° " + notFecha.Id.ToString();
                        }
                        else
                        {
                            DS_Marcaciones ds = ProcesosGlobales.ObtenerMarcaciones(e.Day.Date, DatosAgente1.Agente.Legajo.ToString());
                            if (ds.Marcacion.Rows.Count == 0)
                            {
                                if (DatosAgente1.Agente.ObtenerEstadoAgenteParaElDia(e.Day.Date) == null)
                                {
                                    e.Cell.BackColor = Color.Red;
                                    e.Cell.ToolTip = "Figura ausente";
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            CargarMarcacionesFecha();
        }

        private void CargarMarcacionesFecha()
        {
            using (var cxt = new SisPer.Aplicativo.ClockCardEntities())
            {
                Agente ag = Session["Agente"] as Agente;
                DateTime fechaSeleccionada = Calendar1.SelectedDate;
                DS_Marcaciones marcaciones = ProcesosGlobales.ObtenerMarcacionesConProvisorias(fechaSeleccionada, ag.Legajo.ToString());

                var itemsGrillaMarcaciones = (from m in marcaciones.Marcacion
                                              select new
                                              {
                                                  Legajo = m.Legajo,
                                                  Fecha = m.Fecha,
                                                  Hora = m.Hora,
                                                  MarcaManual = m.MarcaManual,
                                                  EsDefinitivo = m.EsDefinitivo
                                              }).ToList();

                gv_Huellas.DataSource = itemsGrillaMarcaciones.OrderBy(i => i.Hora).ToList();
                gv_Huellas.DataBind();

                EntradaSalida es = ag.EntradasSalidas.FirstOrDefault(eess => eess.Fecha == Calendar1.SelectedDate);
                Area area = ag.Area;

                if ((area.Interior ?? false) == true)
                {
                    if ((es != null && !es.CerradoPersonal && (es.Enviado ?? false) == false) || es == null)
                    {
                        div_ES.Visible = true;
                        if (es != null)
                        {
                            h_entrada.Value = es.Entrada;
                            h_salida.Value = es.Salida;
                        }
                        else
                        {
                            h_entrada.Value = string.Empty;
                            h_salida.Value = string.Empty;
                        }
                    }
                    else
                    {
                        div_ES.Visible = false;
                    }
                }
                else
                {
                    div_ES.Visible = false;
                }
            }
        }

        protected void gv_Huellas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Huellas.PageIndex = e.NewPageIndex;
            CargarMarcacionesFecha();
        }

        protected void btn_GuardarMarcacionES_Click(object sender, EventArgs e)
        {
            Page.Validate("MarcacionesES");
            if (IsValid)
            {
                GuardarES();
                div_ES.Visible = false;
            }
        }

        protected void cv_puedemodificar_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;

            List<Agente> agentes = ag.Area.Agentes.Where(aa => !aa.Jefe).ToList();

            bool enviado = false;

            DateTime diaSeleccionado = Calendar1.SelectedDate;

            foreach (Agente item in agentes)
            {
                EntradaSalida es = item.EntradasSalidas.FirstOrDefault(io => io.Fecha == diaSeleccionado);
                if (es != null)
                {
                    enviado = (es.Enviado ?? false) == true;
                    break;
                }
            }

            args.IsValid = !enviado;
        }

        private void GuardarES()
        {
            Model1Container cxt = new Model1Container();
            Agente ag = Session["Agente"] as Agente;
            Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);

            EntradaSalida e_s = agCxt.EntradasSalidas.FirstOrDefault(io => io.Fecha == Calendar1.SelectedDate);

            if (e_s == null)
            {
                e_s = new EntradaSalida();
                cxt.EntradasSalidas.AddObject(e_s);
            }

            string hora_Entrada = h_entrada.Value.Length == 4 ? "0" + h_entrada.Value : h_entrada.Value;
            string Hora_Salida = h_salida.Value.Length == 4 ? "0" + h_salida.Value : h_salida.Value;

            h_entrada.Value = string.Empty;
            h_salida.Value = string.Empty;

            if (!e_s.CerradoPersonal)
            {
                e_s.Fecha = Calendar1.SelectedDate;
                e_s.Entrada = hora_Entrada;
                e_s.Salida = Hora_Salida;
                e_s.AgenteId = ag.Id;
                e_s.AgenteId1 = ag.Id;
                e_s.Enviado = false;
                e_s.CerradoPersonal = false;
            }

            cxt.SaveChanges();

            Session["Agente"] = cxt.Agentes.FirstOrDefault(a => a.Id == ag.Id);
        }

        #endregion
    }
}