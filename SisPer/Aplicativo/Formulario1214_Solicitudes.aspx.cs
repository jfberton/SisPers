using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Formulario1214_Solicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Menues
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                #endregion

                CargarPendientes();
            }
        }

        private void CargarPendientes()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            CargarPendientesAgentesComunes();

            if (usuarioLogueado.Area.Nombre == "Administración")
            {
                CargarParaNroAnticipo();
            }
            else
            {
                panel_anticipos.Attributes["Style"] = "display:none";
                panel_anticipos_otorgados.Attributes["Style"] = "display:none";
            }

            if (!(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal))
            {
                panel_solicitudes.Attributes["Style"] = "display:none";
                panel_solicitudes_aprobadas_rechazadas.Attributes["Style"] = "display:none";
            }
        }

        private void CargarPendientesAgentesComunes()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            using (var cxt = new Model1Container())
            {

                var items = (from aa in cxt.Agentes1214
                             where aa.Estado == EstadoAgente1214.Solicitado &&
                                (
                                usuarioLogueado.AreaId == aa.Agente.Area.Id //depende directamente
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.Id == usuarioLogueado.AreaId) //depende en segunda instancia
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.Id == usuarioLogueado.AreaId) //depende en tercera instancia
                                || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe.Id == usuarioLogueado.AreaId) //depende en cuarta instancia
                                )
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

                var items_otros = (from aa in cxt.Agentes1214
                                   where aa.Estado != EstadoAgente1214.Solicitado &&
                                   (
                                    usuarioLogueado.AreaId == aa.Agente.Area.Id //depende directamente
                                    || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.Id == usuarioLogueado.AreaId) //depende en segunda instancia
                                    || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.Id == usuarioLogueado.AreaId) //depende en tercera instancia
                                    || (aa.Agente.Area.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe != null && aa.Agente.Area.DependeDe.DependeDe.DependeDe.Id == usuarioLogueado.AreaId) //depende en cuarta instancia
                                    )
                                   select new
                                   {
                                       agente214_id = aa.Id,
                                       area = aa.Agente.Area.Nombre,
                                       f1214_id = aa.Formulario1214Id,
                                       estado_1214 = aa.Formulario1214.Estado,
                                       agente = aa.Agente.ApellidoYNombre,
                                       destino = aa.Formulario1214.Destino,
                                       desde = aa.Formulario1214.Desde,
                                       hasta = aa.Formulario1214.Hasta,
                                       jefe_comision = aa.Formulario1214.Nomina.FirstOrDefault(nn => nn.JefeComicion).Agente.ApellidoYNombre,
                                       tareas = aa.Formulario1214.TareasACumplir,
                                       estado_solicitud = aa.Estado
                                   }).ToList();

                var itemsFormateados_otros = (from item in items_otros
                                              select new
                                              {
                                                  agente214_id = item.agente214_id,
                                                  area = item.area,
                                                  f1214_id = Cadena.CompletarConCeros(6, item.f1214_id),
                                                  estado_1214 = item.estado_1214.ToString(),
                                                  agente = item.agente,
                                                  destino = item.destino,
                                                  desde = item.desde,
                                                  hasta = item.hasta,
                                                  desde_long_str = item.desde.ToLongDateString(),
                                                  hasta_long_str = item.hasta.ToLongDateString(),
                                                  jefe_comision = item.jefe_comision,
                                                  dias = ((item.hasta - item.desde).Days + 1).ToString(),
                                                  tareas = item.tareas,
                                                  estado_solicitud = item.estado_solicitud.ToString()
                                              }).ToList();

                gv_pendientes.Columns[ObtenerColumnaOtros("Area")].Visible = false;

                gv_otras_solicitudes.DataSource = itemsFormateados_otros;
                gv_otras_solicitudes.DataBind();
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

        private int ObtenerColumnaOtros(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_otras_solicitudes.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_otras_solicitudes.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        protected void gv_pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_pendientes.PageIndex = e.NewPageIndex;
            CargarPendientes();
        }

        protected void btn_ver_pendiente_Click(object sender, EventArgs e)
        {
            int idSol214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);
                Formulario1214 f214 = cxt.Formularios1214.First(ff => ff.Id == ag214.Formulario1214Id);

                btn_aprobar.CommandArgument = ((ImageButton)sender).CommandArgument;
                btn_rechazar.CommandArgument = ((ImageButton)sender).CommandArgument;


                lbl_f1214_id.Text = Cadena.CompletarConCeros(6, f214.Id);
                lbl_agente.Text = ag214.Agente.ApellidoYNombre;
                lbl_desde.Text = f214.Desde.ToLongDateString();
                lbl_destino.Text = f214.Destino;
                lbl_dias.Text = ((f214.Hasta - f214.Desde).Days + 1).ToString();
                lbl_hasta.Text = f214.Hasta.ToLongDateString();
                lbl_jefe_de_comision.Text = f214.Nomina.First(nn => nn.JefeComicion).Agente.ApellidoYNombre;
                lbl_tareas.Text = f214.TareasACumplir;
            }

            MostrarPopUpAprobarRechazar();
        }

        private void MostrarPopUpAprobarRechazar()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#modal_ve').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_aprobar_Click(object sender, EventArgs e)
        {
            int idSol214 = Convert.ToInt32(((Button)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);
                ag214.FechaAprobacion = DateTime.Now;
                ag214.Aprobado_por_agente_id = ag.Id;
                ag214.Estado = EstadoAgente1214.Aprobado;
                cxt.SaveChanges();
                CargarPendientes();
                RefrescarMenu();
            }
        }

        private void RefrescarMenu()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
            {
                if (!(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal))
                {
                    MenuPersonalAgente.CargarDatos(usuarioLogueado);
                }
                else
                {
                    MenuPersonalJefe.CargarDatos(usuarioLogueado);
                }
            }
            else
            {
                MenuJefe.CargarDatos(usuarioLogueado);
            }
        }

        protected void btn_rechazar_Click(object sender, EventArgs e)
        {
            int idSol214 = Convert.ToInt32(((Button)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);

                ag214.FechaRechazo = DateTime.Now;
                ag214.Rechazado_por_agente_id = ag.Id;
                ag214.Estado = EstadoAgente1214.Rechazado;
                cxt.SaveChanges();
                CargarPendientes();
                RefrescarMenu();
            }
        }

        protected void btn_ver_otros_Click(object sender, ImageClickEventArgs e)
        {
            int idSol214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);
                Formulario1214 f214 = cxt.Formularios1214.First(ff => ff.Id == ag214.Formulario1214Id);

                lbl_encabezado_f1214.Text = "Form 1214 N° " + Cadena.CompletarConCeros(6, f214.Id) + " - Estado: " + f214.Estado.ToString();
                panel_f1214.Attributes.Clear();
                switch (f214.Estado)
                {
                    case Estado1214.Confeccionado:
                        panel_f1214.Attributes.Add("class", "panel panel-warning");
                        break;
                    case Estado1214.Enviado:
                        panel_f1214.Attributes.Add("class", "panel panel-info");
                        break;
                    case Estado1214.Anulado:
                        panel_f1214.Attributes.Add("class", "panel panel-danger");
                        break;
                    case Estado1214.Aprobada:
                        panel_f1214.Attributes.Add("class", "panel panel-success");
                        break;
                    default:
                        break;
                }

                lbl_otra_solicitud_jefe.Text = f214.Nomina.First(nn => nn.JefeComicion).Agente.ApellidoYNombre;
                lbl_otra_solicitud_destino.Text = f214.Destino;
                lbl_otra_solicitud_cantidad_dias.Text = ((f214.Hasta - f214.Desde).Days + 1).ToString();
                lbl_otra_solicitud_desde.Text = f214.Desde.ToLongDateString();
                lbl_otra_solicitud_hasta.Text = f214.Hasta.ToLongDateString();
                lbl_otra_solicitud_tareas.Text = f214.TareasACumplir;

                lbl_encabezado_solicitud.Text = "Estado actual de la solicitud: " + ag214.Estado.ToString();
                panel_solicitud.Attributes.Clear();
                switch (ag214.Estado)
                {
                    case EstadoAgente1214.Solicitado:
                        panel_solicitud.Attributes.Add("class", "panel panel-warning");
                        break;
                    case EstadoAgente1214.Aprobado:
                        panel_solicitud.Attributes.Add("class", "panel panel-success");
                        break;
                    case EstadoAgente1214.Rechazado:
                        panel_solicitud.Attributes.Add("class", "panel panel-danger");
                        break;
                    case EstadoAgente1214.Cancelado:
                        panel_solicitud.Attributes.Add("class", "panel panel-danger");
                        break;
                    default:
                        break;
                }


                lbl_otra_solicitud_agente.Text = ag214.Agente.ApellidoYNombre;

                if (ag214.FechaAprobacion != null && !ag214.Chofer)
                {
                    lbl_otra_solicitud_aprobado_el.Text = ag214.FechaAprobacion.Value.ToShortDateString();
                    lbl_otra_solicitud_aprobado_por.Text = ag214.AprobadoPor.ApellidoYNombre;
                }
                else
                {
                    lbl_otra_solicitud_aprobado_el.Text = " - ";
                    lbl_otra_solicitud_aprobado_por.Text = " - ";
                }

                if (ag214.FechaRechazo != null)
                {
                    lbl_otra_solicitud_rechazado_el.Text = ag214.FechaRechazo.Value.ToShortDateString();
                    lbl_otra_solicitud_rechazado_por.Text = ag214.RechazadoPor.ApellidoYNombre;
                }
                else
                {
                    lbl_otra_solicitud_rechazado_el.Text = " - ";
                    lbl_otra_solicitud_rechazado_por.Text = " - ";
                }


            }

            MostrarPopUpOtraSolicitud();
        }

        private void MostrarPopUpOtraSolicitud()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#ver_otra_solicitud').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUpOtros", script, false);
        }

        #region Nro Anticipos

        private void CargarParaNroAnticipo()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

            using (var cxt = new Model1Container())
            {
                var items = (from aa in cxt.Agentes1214
                             where aa.Formulario1214.Estado == Estado1214.Aprobada
                                    && aa.Estado == EstadoAgente1214.Aprobado
                                    && aa.NroAnticipo == null
                                    && aa.Formulario1214.PorcentajeLiquidacionViatico > 0
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
                                 tareas = aa.Formulario1214.TareasACumplir,
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
                                            tareas = item.tareas,
                                            nro_anticipo = "Por Otorgar"
                                        }).ToList();


                gv_anticipos.DataSource = itemsFormateados;
                gv_anticipos.DataBind();

                var items_anticipos_otorgados = (from aa in cxt.Agentes1214
                                                 where aa.Formulario1214.Estado == Estado1214.Aprobada
                                                        && aa.Estado == EstadoAgente1214.Aprobado
                                                        && aa.NroAnticipo != null
                                                        && aa.Formulario1214.PorcentajeLiquidacionViatico > 0
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
                                                     tareas = aa.Formulario1214.TareasACumplir,
                                                     nro_anticipo = aa.NroAnticipo
                                                 }).ToList();

                var items_anticipos_otorgados_Formateados = (from item in items_anticipos_otorgados
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
                                                                 tareas = item.tareas,
                                                                 nro_anticipo = item.nro_anticipo
                                                             }).ToList();


                gv_anticipos_otorgados.DataSource = items_anticipos_otorgados_Formateados;
                gv_anticipos_otorgados.DataBind();
            }
        }

        protected void btn_ver_otorgar_anticipo_Click(object sender, ImageClickEventArgs e)
        {
            int idSol214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);
                Formulario1214 f214 = cxt.Formularios1214.First(ff => ff.Id == ag214.Formulario1214Id);

                btn_ant_aceptar.CommandArgument = ((ImageButton)sender).CommandArgument;

                lbl_ant_nro_comision.Text = Cadena.CompletarConCeros(6, f214.Id);
                lbl_ant_agente.Text = ag214.Agente.ApellidoYNombre;
                lbl_ant_desde.Text = f214.Desde.ToLongDateString();
                lbl_ant_destino.Text = f214.Destino;
                lbl_ant_dias.Text = ((f214.Hasta - f214.Desde).Days + 1).ToString();
                lbl_ant_hasta.Text = f214.Hasta.ToLongDateString();
                lbl_ant_jefe_comision.Text = f214.Nomina.First(nn => nn.JefeComicion).Agente.ApellidoYNombre;
                lbl_ant_tareas.Text = f214.TareasACumplir;
                tb_ant_nro_anticipo.Text = ag214.NroAnticipo;

                if (ag214.NroAnticipoCargadoPor.HasValue)
                {
                    int idAgente = ag214.NroAnticipoCargadoPor.Value;
                    lbl_ant_otorgado_por.Text = cxt.Agentes.FirstOrDefault(a => a.Id == idAgente).ApellidoYNombre;
                    ant_otorgado_por_PanelRowColumn.Attributes["Style"] = "display:inline";
                }
                else
                {
                    ant_otorgado_por_PanelRowColumn.Attributes["Style"] = "display:none";
                }
            }

            MostrarPopUpOtorgarVerAnticipo();
        }

        private void MostrarPopUpOtorgarVerAnticipo()
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#modal_anticipo').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        protected void btn_ant_aceptar_Click(object sender, EventArgs e)
        {
            int idSol214 = Convert.ToInt32(((Button)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;
                Agente1214 ag214 = cxt.Agentes1214.First(aa => aa.Id == idSol214);

                if (tb_ant_nro_anticipo.Text != "" && tb_ant_nro_anticipo.Text != ag214.NroAnticipo)
                {
                    ag214.NroAnticipo = tb_ant_nro_anticipo.Text;
                    ag214.NroAnticipoCargadoPor = ag.Id;
                    cxt.SaveChanges();

                }

                if (tb_ant_nro_anticipo.Text == "" && ag214.NroAnticipo != null)
                {
                    ag214.NroAnticipo = null;
                    ag214.NroAnticipoCargadoPor = ag.Id;
                    cxt.SaveChanges();
                }

                CargarPendientes();
                RefrescarMenu();
            }
        }

        #endregion

        protected void gv_pendientes_PreRender(object sender, EventArgs e)
        {
            if (gv_pendientes.Rows.Count > 0)
            {
                gv_pendientes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gv_otras_solicitudes_PreRender(object sender, EventArgs e)
        {
            if (gv_otras_solicitudes.Rows.Count > 0)
            {
                gv_otras_solicitudes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gv_anticipos_PreRender(object sender, EventArgs e)
        {
            if (gv_anticipos.Rows.Count > 0)
            {
                gv_anticipos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gv_anticipos_otorgados_PreRender(object sender, EventArgs e)
        {
            if (gv_anticipos_otorgados.Rows.Count > 0)
            {
                gv_anticipos_otorgados.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }

}