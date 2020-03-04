using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Inbox : System.Web.UI.Page
    {
        private struct AgrupacionDeRecibidos
        {
            public string Nombre { get; set; }
            public List<Destinatario> Mensajes { get; set; }
        }

        private struct AgrupacionDeEnvios
        {
            public string Nombre { get; set; }
            public List<Mensaje> MensajesEnviados { get; set; }
        }

        private struct EstadoAgrupacion
        {
            public int IndexAgrupacion { get; set; }
            public bool Abierto { get; set; }
        }

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
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }


                if (MenuAgente.Visible) { MenuAgente.ActualizarNotificacionesMensajes(); }
                if (MenuJefe.Visible) { MenuJefe.ActualizarNotificacionesMensajes(); }
                if (MenuPersonalAgente.Visible) { MenuPersonalAgente.ActualizarNotificacionesMensajes(); }
                if (MenuPersonalJefe.Visible) { MenuPersonalJefe.ActualizarNotificacionesMensajes(); }

                Mensaje mensaje = ((Mensaje)Session["AbrioMensaje"]);
                if (mensaje != null)
                {
                    CuerpoMensaje.Text = mensaje.Encabezado() + mensaje.Cuerpo;
                    bool recibido = Convert.ToBoolean(Session["Recibido"]);
                    if (recibido)
                    {
                        btn_responder.Visible = true;
                    }
                }

                tb_Buscar_Recibidos.Value = Session["Filtro_Recibidos"] != null ? Session["Filtro_Recibidos"].ToString() : String.Empty;

                Session["AgrupacionesMensajesRecibidos"] = new List<AgrupacionDeRecibidos>();
                Session["AgrupacionesMensajesEnviados"] = new List<AgrupacionDeEnvios>();
            }

            CrearBandejas();

        }

        private void CrearBandejas()
        {
            #region Bandeja de entrada

            ObtenerAgrupacionesRecibidos();
            AccordionRecibidos.Controls.Clear();

            List<AgrupacionDeRecibidos> recibidos = Session["AgrupacionesMensajesRecibidos"] as List<AgrupacionDeRecibidos>;

            int index = 0;

            foreach (AgrupacionDeRecibidos grupo in recibidos)
            {

                bool abierto = ObtenerEstadoAgrupacionRecibido(index);
                int mensajesSinLeerDelGrupo = grupo.Mensajes.Where(m => m.FechaLeido == null).Count();
                HtmlGenericControl panelGrupo = new HtmlGenericControl("div"); panelGrupo.Attributes.Add("class", "panel panel-info");

                #region Armado del encabezado

                HtmlGenericControl panelEncabezado = new HtmlGenericControl("div"); panelEncabezado.Attributes.Add("class", "panel-heading"); panelEncabezado.Attributes.Add("role", "tab"); panelEncabezado.Attributes.Add("id", "heading_" + index.ToString());
                HtmlGenericControl h4 = new HtmlGenericControl("h4"); h4.Attributes.Add("class", "panel-title");
                HtmlAnchor a = new HtmlAnchor(); a.Attributes.Add("class", "collapsed"); a.Attributes.Add("data-toggle", "collapse"); a.Attributes.Add("data-target", "#collapse_" + index.ToString()); a.Attributes.Add("href", "#collapse_" + index.ToString()); a.Attributes.Add("aria-expanded", "false"); a.Attributes.Add("aria-controls", "collapse_" + index.ToString());
                if (abierto)
                {
                    a.ID = "idRecibidosAbierto_" + index.ToString();
                }
                else
                {
                    a.ID = "idRecibidosCerrado_" + index.ToString();
                }

                if (mensajesSinLeerDelGrupo > 0)
                {
                    a.InnerHtml = grupo.Nombre + " <span class=\"badge\">" + mensajesSinLeerDelGrupo + "</span>";
                }
                else
                {
                    a.InnerText = grupo.Nombre;
                }

                a.ServerClick += GrupoRecibidos_ServerClick;

                h4.Controls.Add(a);
                panelEncabezado.Controls.Add(h4);

                #endregion

                #region Armado del cuerpo

                HtmlGenericControl panelCollapse = new HtmlGenericControl("div"); panelCollapse.Attributes.Add("id", "collapse_" + index.ToString());
                if (abierto)
                {
                    panelCollapse.Attributes.Add("class", "panel-collapse collapse in");
                }
                else
                {
                    panelCollapse.Attributes.Add("class", "panel-collapse collapse");
                }

                panelCollapse.Attributes.Add("role", "tabpanel"); panelCollapse.Attributes.Add("aria-labelledby", "heading_" + index.ToString());

                HtmlGenericControl panelCuerpo = new HtmlGenericControl("div"); panelCuerpo.Attributes.Add("class", "panel-body");

                if (grupo.Mensajes.Count > 0)
                {
                    foreach (Destinatario item in grupo.Mensajes)
                    {
                        HtmlGenericControl row = new HtmlGenericControl("div");
                        row.Attributes.Add("class", "row");
                        HtmlGenericControl column = new HtmlGenericControl("div");
                        column.Attributes.Add("class", "col-md-12");

                        ItemMensaje mensaje = Page.LoadControl("~/Aplicativo/Controles/ItemMensaje.ascx") as ItemMensaje;
                        mensaje.Mensaje = item.Mensaje;
                        mensaje.Recibido = true;
                        mensaje.AbrioElMensaje += mensaje_AbrioElMensaje;

                        column.Controls.Add(mensaje);
                        row.Controls.Add(column);
                        panelCuerpo.Controls.Add(row);
                    }
                }
                else
                {
                    panelCuerpo.InnerText = "No existen mensajes.-";
                }

                panelCollapse.Controls.Add(panelCuerpo);

                #endregion

                panelGrupo.Controls.Add(panelEncabezado);
                panelGrupo.Controls.Add(panelCollapse);

                AccordionRecibidos.Controls.Add(panelGrupo);

                index++;
            }

            #endregion

            #region Bandeja de salida

            ObtenerAgrupacionesEnviados();
            accordionEnviados.Controls.Clear();

            List<AgrupacionDeEnvios> enviados = Session["AgrupacionesMensajesEnviados"] as List<AgrupacionDeEnvios>;
            index = 0;

            foreach (AgrupacionDeEnvios grupo in enviados)
            {
                HtmlGenericControl panelGrupo = new HtmlGenericControl("div"); panelGrupo.Attributes.Add("class", "panel panel-warning");
                bool abierto = ObtenerEstadoAgrupacionEnviado(index);

                #region Armado del encabezado

                HtmlGenericControl panelEncabezado = new HtmlGenericControl("div"); panelEncabezado.Attributes.Add("class", "panel-heading"); panelEncabezado.Attributes.Add("role", "tab"); panelEncabezado.Attributes.Add("id", "heading_" + index.ToString());
                HtmlGenericControl h4 = new HtmlGenericControl("h4"); h4.Attributes.Add("class", "panel-title");
                HtmlAnchor a = new HtmlAnchor(); a.Attributes.Add("class", "collapsed"); a.Attributes.Add("data-toggle", "collapse"); a.Attributes.Add("data-target", "#collapse_" + index.ToString()); a.Attributes.Add("href", "#collapse_" + index.ToString()); a.Attributes.Add("aria-expanded", "false"); a.Attributes.Add("aria-controls", "collapse_" + index.ToString());
                if (abierto)
                {
                    a.ID = "idEnviadosAbierto_" + index.ToString();
                }
                else
                {
                    a.ID = "idEnviadosCerrado_" + index.ToString();
                }
                a.InnerText = grupo.Nombre;
                a.ServerClick += GrupoEnviados_ServerClick;

                h4.Controls.Add(a);
                panelEncabezado.Controls.Add(h4);

                #endregion

                #region Armado del cuerpo

                HtmlGenericControl panelCollapse = new HtmlGenericControl("div"); panelCollapse.Attributes.Add("id", "collapse_" + index.ToString());
                if (abierto)
                {
                    panelCollapse.Attributes.Add("class", "panel-collapse collapse in");
                }
                else
                {
                    panelCollapse.Attributes.Add("class", "panel-collapse collapse");
                }

                panelCollapse.Attributes.Add("role", "tabpanel"); panelCollapse.Attributes.Add("aria-labelledby", "heading_" + index.ToString());

                HtmlGenericControl panelCuerpo = new HtmlGenericControl("div"); panelCuerpo.Attributes.Add("class", "panel-body");
                if (grupo.MensajesEnviados.Count > 0)
                {
                    foreach (Mensaje item in grupo.MensajesEnviados)
                    {
                        ItemMensaje mensaje = Page.LoadControl("~/Aplicativo/Controles/ItemMensaje.ascx") as ItemMensaje;
                        mensaje.Mensaje = item;
                        mensaje.Recibido = false;
                        mensaje.AbrioElMensaje += mensaje_AbrioElMensaje;
                        panelCuerpo.Controls.Add(mensaje);
                    }
                }
                else
                {
                    panelCuerpo.InnerText = "No existen mensajes.-";
                }

                panelCollapse.Controls.Add(panelCuerpo);

                #endregion

                panelGrupo.Controls.Add(panelEncabezado);
                panelGrupo.Controls.Add(panelCollapse);

                accordionEnviados.Controls.Add(panelGrupo);

                index++;
            }

            #endregion

            #region Tabs

            string tab = Session["Tab"] != null ? Session["Tab"].ToString() : string.Empty;

            if (tab != string.Empty)
            {
                if (tab == "BandejaEntrada")
                {
                    //tab bandeja de entrada
                    tabBandejaEntrada.Attributes.Remove("class"); tabBandejaEntrada.Attributes.Add("class", "active");
                    tabBandejaEnviados.Attributes.Remove("class");

                    bandejaEntradaTab.Attributes.Remove("class"); bandejaEntradaTab.Attributes.Add("class", "tab-pane active");
                    bandejaSalidaTab.Attributes.Remove("class"); bandejaSalidaTab.Attributes.Add("class", "tab-pane");
                }
                else
                {
                    //tab bandeja de salida
                    tabBandejaEntrada.Attributes.Remove("class");
                    tabBandejaEnviados.Attributes.Remove("class"); tabBandejaEnviados.Attributes.Add("class", "active");

                    bandejaEntradaTab.Attributes.Remove("class"); bandejaEntradaTab.Attributes.Add("class", "tab-pane");
                    bandejaSalidaTab.Attributes.Remove("class"); bandejaSalidaTab.Attributes.Add("class", "tab-pane active");
                }
            }
            else
            {
                //tab bandeja de entrada
                tabBandejaEntrada.Attributes.Remove("class"); tabBandejaEntrada.Attributes.Add("class", "active");
                tabBandejaEnviados.Attributes.Remove("class");

                bandejaEntradaTab.Attributes.Remove("class"); bandejaEntradaTab.Attributes.Add("class", "tab-pane active");
                bandejaSalidaTab.Attributes.Remove("class"); bandejaSalidaTab.Attributes.Add("class", "tab-pane");
            }

            #endregion

        }

        private void ObtenerAgrupacionesRecibidos()
        {
            List<AgrupacionDeRecibidos> agrupaciones = new List<AgrupacionDeRecibidos>();

            Agente ag = Session["UsuarioLogueado"] as Agente;

            var cxt = new Model1Container();

            List<Destinatario> para_el_agente = cxt.Destinatarios.Where(d => d.AgenteId == ag.Id).ToList();
            string filtro = tb_Buscar_Recibidos.Value.ToUpper();

            Session["Filtro_Recibidos"] = tb_Buscar_Recibidos.Value;

            List<Destinatario> itemsFiltrados = filtro == string.Empty ?
                (cxt.Destinatarios.Where(d => d.AgenteId == ag.Id).OrderByDescending(m => m.Mensaje.FechaEnvio).Take(300).ToList()) :
                ((from i in para_el_agente
                  where
                    Cadena.Normalizar(i.Mensaje.Agente.ApellidoYNombre.ToUpper()).Contains(filtro)
                  select i).OrderByDescending(m => m.Mensaje.FechaEnvio).ToList());

            List<Destinatario> mensajes = itemsFiltrados;

            List<Destinatario> mensajesDeHoy = mensajes.Where(d => d.Mensaje.FechaEnvio.Date == DateTime.Today.Date).ToList();
            agrupaciones.Add(new AgrupacionDeRecibidos() { Nombre = "Mensajes de hoy", Mensajes = mensajesDeHoy });

            List<Destinatario> mensajesDeAyer = mensajes.Where(d => d.Mensaje.FechaEnvio.Date == DateTime.Today.AddDays(-1).Date).ToList();
            if (mensajesDeAyer.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeRecibidos() { Nombre = "Mensajes de ayer", Mensajes = mensajesDeAyer });
            }

            List<Destinatario> mensajesDeHaceUnaSemana = mensajes.Where(d => d.Mensaje.FechaEnvio.Date > DateTime.Today.AddDays(-9).Date && d.Mensaje.FechaEnvio.Date < DateTime.Today.AddDays(-1).Date).ToList();
            if (mensajesDeHaceUnaSemana.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeRecibidos() { Nombre = "Mensajes de hace una semana", Mensajes = mensajesDeHaceUnaSemana });
            }

            List<Destinatario> mensajesAntiguos = mensajes.Where(d => d.Mensaje.FechaEnvio.Date < DateTime.Today.AddDays(-9).Date).ToList();
            if (mensajesAntiguos.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeRecibidos() { Nombre = "Mensajes antiguos", Mensajes = mensajesAntiguos });
            }

            Session["AgrupacionesMensajesRecibidos"] = agrupaciones;
        }

        private void ObtenerAgrupacionesEnviados()
        {
            List<AgrupacionDeEnvios> agrupaciones = new List<AgrupacionDeEnvios>();

            Agente ag = Session["UsuarioLogueado"] as Agente;

            var cxt = new Model1Container();

            List<Mensaje> mensajes = cxt.Mensajes.Where(m => m.AgenteId == ag.Id).OrderByDescending(m => m.FechaEnvio).Take(300).ToList();

            List<Mensaje> mensajesDeHoy = mensajes.Where(d => d.FechaEnvio.Date == DateTime.Today.Date).ToList();
            agrupaciones.Add(new AgrupacionDeEnvios() { Nombre = "Mensajes de hoy", MensajesEnviados = mensajesDeHoy });

            List<Mensaje> mensajesDeAyer = mensajes.Where(d => d.FechaEnvio.Date == DateTime.Today.AddDays(-1).Date).ToList();
            if (mensajesDeAyer.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeEnvios() { Nombre = "Mensajes de ayer", MensajesEnviados = mensajesDeAyer });
            }

            List<Mensaje> mensajesDeHaceUnaSemana = mensajes.Where(d => d.FechaEnvio.Date > DateTime.Today.AddDays(-9).Date && d.FechaEnvio.Date < DateTime.Today.AddDays(-1).Date).ToList();
            if (mensajesDeHaceUnaSemana.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeEnvios() { Nombre = "Mensajes de hace una semana", MensajesEnviados = mensajesDeHaceUnaSemana });
            }

            List<Mensaje> mensajesAntiguos = mensajes.Where(d => d.FechaEnvio.Date < DateTime.Today.AddDays(-9).Date).ToList();
            if (mensajesAntiguos.Count > 0)
            {
                agrupaciones.Add(new AgrupacionDeEnvios() { Nombre = "Mensajes antiguos", MensajesEnviados = mensajesAntiguos });
            }

            Session["AgrupacionesMensajesEnviados"] = agrupaciones;
        }

        private bool ObtenerEstadoAgrupacionRecibido(int index)
        {
            List<EstadoAgrupacion> eeaa = Session["EstadoAgrupacionRecibidos"] as List<EstadoAgrupacion>;
            if (eeaa != null)
            {
                EstadoAgrupacion ea = new EstadoAgrupacion();
                bool encontro = false;
                foreach (EstadoAgrupacion item in eeaa)
                {
                    if (item.IndexAgrupacion == index)
                    {
                        ea = item;
                        encontro = true;
                    }
                }

                if (encontro)
                {
                    return ea.Abierto;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private bool ObtenerEstadoAgrupacionEnviado(int index)
        {
            List<EstadoAgrupacion> eeaa = Session["EstadoAgrupacionEnviados"] as List<EstadoAgrupacion>;
            if (eeaa != null)
            {
                EstadoAgrupacion ea = new EstadoAgrupacion();
                bool encontro = false;
                foreach (EstadoAgrupacion item in eeaa)
                {
                    if (item.IndexAgrupacion == index)
                    {
                        ea = item;
                        encontro = true;
                    }
                }

                if (encontro)
                {
                    return ea.Abierto;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void GrupoEnviados_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor a = sender as HtmlAnchor;
            List<EstadoAgrupacion> estadosAgrupaciones = Session["EstadoAgrupacionEnviados"] as List<EstadoAgrupacion>;

            if (estadosAgrupaciones == null)
            {
                estadosAgrupaciones = new List<EstadoAgrupacion>();
            }

            int indexGrupo = Convert.ToInt32(a.ID.Split('_')[1]);
            int indexAEliminar = -1;
            EstadoAgrupacion ea = new EstadoAgrupacion()
            {
                IndexAgrupacion = indexGrupo,
                Abierto = !a.ID.Contains("Abierto")
            };

            foreach (EstadoAgrupacion item in estadosAgrupaciones)
            {
                if (item.IndexAgrupacion == indexGrupo)
                {
                    indexAEliminar = estadosAgrupaciones.IndexOf(item);
                }
            }

            if (indexAEliminar != -1)
            {
                estadosAgrupaciones.RemoveAt(indexAEliminar);
            }

            estadosAgrupaciones.Add(ea);

            Session["EstadoAgrupacionEnviados"] = estadosAgrupaciones;

            Response.Redirect(Request.RawUrl);
        }

        private void GrupoRecibidos_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor a = sender as HtmlAnchor;
            List<EstadoAgrupacion> estadosAgrupaciones = Session["EstadoAgrupacionRecibidos"] as List<EstadoAgrupacion>;

            if (estadosAgrupaciones == null)
            {
                estadosAgrupaciones = new List<EstadoAgrupacion>();
            }

            int indexGrupo = Convert.ToInt32(a.ID.Split('_')[1]);
            int indexAEliminar = -1;
            EstadoAgrupacion ea = new EstadoAgrupacion()
            {
                IndexAgrupacion = indexGrupo,
                Abierto = !a.ID.Contains("Abierto")
            };

            foreach (EstadoAgrupacion item in estadosAgrupaciones)
            {
                if (item.IndexAgrupacion == indexGrupo)
                {
                    indexAEliminar = estadosAgrupaciones.IndexOf(item);
                }
            }

            if (indexAEliminar != -1)
            {
                estadosAgrupaciones.RemoveAt(indexAEliminar);
            }

            estadosAgrupaciones.Add(ea);

            Session["EstadoAgrupacionRecibidos"] = estadosAgrupaciones;

            Response.Redirect(Request.RawUrl);
        }

        private void mensaje_AbrioElMensaje(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btn_NuevoMensaje_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Mensajes.aspx");
        }

        protected void btn_responder_Click(object sender, EventArgs e)
        {
            Mensaje mensaje = Session["AbrioMensaje"] as Mensaje;

            if (mensaje != null)
            {
                var CXT = new Model1Container();
                mensaje = CXT.Mensajes.FirstOrDefault(M => M.Id == mensaje.Id);
                Session["ResponderA"] = mensaje.Agente;
                Session["MensajeOriginal"] = "<em><hr />" + mensaje.Encabezado().Replace("<hr />", "") + mensaje.Cuerpo + "<hr /><p></p></em>";
            }

            Response.Redirect("~/Aplicativo/Mensajes.aspx");

        }

        protected void Recibidos_ServerClick(object sender, EventArgs e)
        {
            Session["Tab"] = "BandejaEntrada";
            Response.Redirect(Request.RawUrl);
        }

        protected void Enviados_ServerClick(object sender, EventArgs e)
        {
            Session["Tab"] = "BandejaSalida";
            Response.Redirect(Request.RawUrl);
        }


        protected void btn_Buscar_Recibidos_ServerClick(object sender, EventArgs e)
        {
            //Response.Redirect(Request.RawUrl);
        }

      
    }
}