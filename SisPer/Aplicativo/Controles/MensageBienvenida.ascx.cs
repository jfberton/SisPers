using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class MensageBienvenida : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = (Agente)Session["UsuarioLogueado"];
                CargarDatos(ag);
            }
        }

        private void CargarDatos(Agente agente)
        {
            bool validoMail = Convert.ToBoolean(Session["ValidoMail"]);
            if (agente != null)
            {
                var cxt = new Model1Container();

                lbl_ApyNom.Text = agente.ApellidoYNombre;
                if (validoMail)
                {
                    div_valido_mail.Visible = true;
                    div_correo_pendiente_de_validacion.Visible = false;
                    lbl_correo.Text = agente.Legajo_datos_laborales.Email;
                }
                else
                {
                    div_valido_mail.Visible = false;

                    Validacion_email vm = cxt.Validaciones_email.FirstOrDefault(vmail => vmail.AgenteId == agente.Id);
                    if (vm == null || vm.Fecha_validado == null)
                    {
                        div_correo_pendiente_de_validacion.Visible = true;
                    }
                    else
                    {
                        div_correo_pendiente_de_validacion.Visible = false;
                    }
                }

                

                int idUltimaSession = cxt.Sesiones.Where(ss=>ss.AgenteId == agente.Id).Max(s=>s.Id);//ultimo inicio de sesion incluyendo este acceso asi que tengo que descartar este id
                idUltimaSession = cxt.Sesiones.Where(ss => ss.AgenteId == agente.Id && ss.Id != idUltimaSession).Count() > 0 ? cxt.Sesiones.Where(ss => ss.AgenteId == agente.Id && ss.Id != idUltimaSession).Max(s => s.Id) : 0; //ultimo inicio de sesion sin este acceso, SI NO ENCUENTRA NINGUN ACCESO ANTERIOR SETEO EL ID = 0;
                Sesion ultimo_acceso = cxt.Sesiones.FirstOrDefault(ss=>ss.Id == idUltimaSession);
                if (ultimo_acceso != null)
                {
                    if (ultimo_acceso.FechaHoraInicio.Date == DateTime.Today)
                    {
                        lbl_ultimo_acceso.Text = "Tu último acceso fue hoy, a las " + ultimo_acceso.FechaHoraInicio.ToLongTimeString();
                    }
                    else
                    {
                        lbl_ultimo_acceso.Text = "Tu último acceso fue el " + ultimo_acceso.FechaHoraInicio.ToLongDateString() + ", a las " + ultimo_acceso.FechaHoraInicio.ToLongTimeString();
                    }
                }
                else
                {
                    lbl_ultimo_acceso.Text = "Este es tu primer acceso al sitio, espero sea de tu agrado y nos visites más seguido!";
                }

                
                //mensajes
                var mensajesSinLeer = cxt.Destinatarios.Where(m => m.AgenteId == agente.Id && m.FechaLeido == null).ToList();
                var mensajes = (from mm in mensajesSinLeer
                                select new
                                {
                                    fecha = mm.Mensaje.FechaEnvio,
                                    emisor = mm.Mensaje.Agente.ApellidoYNombre,
                                    asunto = mm.Mensaje.Asunto
                                }).ToList();
                gv_mensajes.DataSource = mensajes;
                gv_mensajes.DataBind();
                panel_mensajes.Visible = mensajesSinLeer.Count() > 0;
                lbl_mensajes.Text = mensajesSinLeer.Count().ToString();




                //solicitudes de documentacion
                var notificacionesSinLeer = (from ne in cxt.Notificaciones
                                             where ne.AgenteId == agente.Id && ne.HistorialEstadosNotificacion.FirstOrDefault(e => e.Estado.Estado == "Notificada") == null
                                             select ne);
                var notificaciones = ( from nn in notificacionesSinLeer
                                       select new
                                       {
                                           id = nn.Id,
                                           descripcion = nn.Descripcion
                                       }).ToList();
                gv_solicitudes.DataSource = notificaciones;
                gv_solicitudes.DataBind();
                panel_solicitudes.Visible = notificacionesSinLeer.Count() > 0;
                lbl_solicitudes.Text = notificacionesSinLeer.Count().ToString();


                //solicitudes de aprobacion 1214
                int solicitudes214 = 0;
                if (agente.Perfil == PerfilUsuario.Personal)
                {
                    solicitudes214 = cxt.Agentes1214.Count(aa => aa.Estado == EstadoAgente1214.Solicitado);
                }
                else
                {
                    solicitudes214 = cxt.Agentes1214.Count(aa => aa.Estado == EstadoAgente1214.Solicitado && aa.Id_Jefe == agente.Id);
                }

                CargarPendientes();

                panel_solicitudes_1214.Visible = solicitudes214 > 0;
                lbl_solicitudes_1214.Text = solicitudes214.ToString();



                lbl_tiene_algo_por_revisar.Visible = (mensajesSinLeer.Count() + notificacionesSinLeer.Count() + solicitudes214) > 0;
            }
        }

        private void CargarPendientes()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            using (var cxt = new Model1Container())
            {
                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    var items = (from aa in cxt.Agentes1214
                                 where aa.Estado == EstadoAgente1214.Solicitado
                                 select new
                                 {
                                     agente214_id = aa.Id,
                                     area = aa.Agente.Area.Nombre,
                                     f1214_id = aa.Formulario1214Id,
                                     agente = aa.Agente.ApellidoYNombre,
                                     destino = aa.Formulario1214.Destino,
                                     desde = aa.Formulario1214.Desde,
                                     hasta = aa.Formulario1214.Hasta,
                                     jefe_comision = aa.Formulario1214.Nomina.FirstOrDefault(nn => nn.JefeComicion).Agente.ApellidoYNombre,
                                     tareas = aa.Formulario1214.TareasACumplir
                                 }).ToList();

                    var itemsFormateados = (from item in items
                                            select new
                                            {
                                                agente214_id = item.agente214_id,
                                                area = item.area,
                                                f1214_id = Cadena.CompletarConCeros(6, item.f1214_id),
                                                agente = item.agente,
                                                destino = item.destino,
                                                desde = item.desde,
                                                hasta = item.hasta,
                                                desde_long_str = item.desde.ToLongDateString(),
                                                hasta_long_str = item.hasta.ToLongDateString(),
                                                jefe_comision = item.jefe_comision,
                                                dias = ((item.hasta - item.desde).Days + 1).ToString(),
                                                tareas = item.tareas
                                            }).ToList();



                    gv_pendientes.DataSource = itemsFormateados;
                    gv_pendientes.DataBind();
                }
                else
                {
                    var items = (from aa in cxt.Agentes1214
                                 where aa.Estado == EstadoAgente1214.Solicitado && aa.Id_Jefe == usuarioLogueado.Id
                                 select new
                                 {
                                     agente214_id = aa.Id,
                                     area = aa.Agente.Area.Nombre,
                                     f1214_id = aa.Formulario1214Id,
                                     agente = aa.Agente.ApellidoYNombre,
                                     destino = aa.Formulario1214.Destino,
                                     desde = aa.Formulario1214.Desde,
                                     hasta = aa.Formulario1214.Hasta,
                                     jefe_comision = aa.Formulario1214.Nomina.FirstOrDefault(nn => nn.JefeComicion).Agente.ApellidoYNombre,
                                     tareas = aa.Formulario1214.TareasACumplir
                                 }).ToList();

                    var itemsFormateados = (from item in items
                                            select new
                                            {
                                                agente214_id = item.agente214_id,
                                                area = item.area,
                                                f1214_id = Cadena.CompletarConCeros(6, item.f1214_id),
                                                agente = item.agente,
                                                destino = item.destino,
                                                desde = item.desde,
                                                hasta = item.hasta,
                                                desde_long_str = item.desde.ToLongDateString(),
                                                hasta_long_str = item.hasta.ToLongDateString(),
                                                jefe_comision = item.jefe_comision,
                                                dias = ((item.hasta - item.desde).Days + 1).ToString(),
                                                tareas = item.tareas
                                            }).ToList();

                    gv_pendientes.Columns[ObtenerColumna("Area")].Visible = false;

                    gv_pendientes.DataSource = itemsFormateados;
                    gv_pendientes.DataBind();
                }

            }
        }

        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_pendientes.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_pendientes.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }


        public void Show()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "mostrarModalBienvenida();", true);
        }
    }
}