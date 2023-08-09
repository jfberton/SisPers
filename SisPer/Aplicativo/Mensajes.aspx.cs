using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Mensajes : System.Web.UI.Page
    {
        private struct ItemGridAgentes
        {
            public int IdAgente { get; set; }
            public string Agente { get; set; }
            public string Area { get; set; }
            public int Legajo { get; set; }
            public bool Seleccionado { get; set; }
        }

        private struct ItemGridArea
        {
            public int IdArea { get; set; }
            public string Nombre { get; set; }
            public bool Seleccionado { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
                    btn_Todos.Visible = true;
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    btn_Todos.Visible = false;
                }

                CargarAgentes();
                CargarAreas();
                CuerpoMensaje.Height = 500;

                Agente agenteRespuesta = Session["ResponderA"] as Agente;
                string mensajeoriginal = Session["MensajeOriginal"] != null ? Session["MensajeOriginal"].ToString() : string.Empty;

                Session["MensajeOriginal"] = null;
                Session["ResponderA"] = null;

                List<Agente> listaDeDestinatarios = new List<Agente>();
                
                if (agenteRespuesta != null)
                {
                    listaDeDestinatarios.Add(agenteRespuesta);
                    CuerpoMensaje.Text = "<p></p><p></p>" + mensajeoriginal;
                }

                Session["Destinatarios"] = listaDeDestinatarios;
                Session["Todos"] = false;
            }
         
            ActualizarDestinatarios();
        }

        private void CargarAgentes()
        {
            using (var cxt = new Model1Container())
            {
                List<Agente> destinatarios = Session["Destinatarios"] as List<Agente>;
                List<ItemGridAgentes> items = new List<ItemGridAgentes>();

                var agentes = from a in cxt.Agentes
                              where a.FechaBaja == null
                              select new
                              {
                                  IdAgente = a.Id,
                                  Legajo = a.Legajo,
                                  Agente = a.ApellidoYNombre,
                                  Area = a.Area.Nombre,
                                  Seleccionado = false
                              };

                foreach (var a in agentes)
                {
                    items.Add(new ItemGridAgentes()
                               {
                                   IdAgente = a.IdAgente,
                                   Legajo = a.Legajo,
                                   Agente = a.Agente,
                                   Area = a.Area,
                                   Seleccionado = destinatarios != null ? destinatarios.Exists(aa => aa.Id == a.IdAgente) : false
                               });
                }

                gv_Agentes.DataSource = items;
                gv_Agentes.DataBind();
            }
        }

        private void CargarAreas()
        {
            using (var cxt = new Model1Container())
            {
                List<Area> destinatarios = Session["AreasDestino"] as List<Area>;
                List<ItemGridArea> items = new List<ItemGridArea>();

                var areas = from a in cxt.Areas
                            select new
                            {
                                AreaId = a.Id,
                                Nombre = a.Nombre,
                                Seleccionado = false
                            };

                foreach (var a in areas)
                {
                    items.Add(new ItemGridArea()
                    {
                        IdArea = a.AreaId,
                        Nombre = a.Nombre,
                        Seleccionado = destinatarios != null ? destinatarios.Exists(aa => aa.Id == a.AreaId) : false
                    });
                }

                gv_Areas.DataSource = items;
                gv_Areas.DataBind();
            }
        }

        private void ActualizarDestinatarios()
        {
            bool todos = Convert.ToBoolean(Session["Todos"]);
            List<Agente> destinatarios = Session["Destinatarios"] as List<Agente>;
            List<Area> areasDestinatarias = Session["AreasDestino"] as List<Area>;

            div_destinatarios.Controls.Clear();
            div_destinatarios.Attributes.Clear();

            if (!todos)
            {
                if ((destinatarios != null && destinatarios.Count > 0) || (areasDestinatarias != null && areasDestinatarias.Count > 0))
                {
                    #region Agentes

                    if (destinatarios != null && destinatarios.Count > 0)
                    {
                        foreach (var item in destinatarios)
                        {
                            HtmlGenericControl divPrincipal = new HtmlGenericControl("div");
                            divPrincipal.Attributes.Add("class", "btn btn-sm alert-success");

                            Label label = new Label();
                            label.Text = "<strong>" + item.ApellidoYNombre + "</strong> ";

                            LinkButton link = new LinkButton();
                            link.ID = "IDAgente_" + item.Id;
                            link.Attributes.Add("runat", "server");
                            link.Text = "[X]";
                            link.ToolTip = "Quitar";
                            link.CommandArgument = item.Id.ToString();
                            link.Click += new EventHandler(this.QuitarDestinatario_Click);

                            divPrincipal.Controls.Add(label);
                            divPrincipal.Controls.Add(link);

                            div_destinatarios.Controls.Add(divPrincipal);
                        }
                    }

                    #endregion

                    #region Areas

                    if (areasDestinatarias != null && areasDestinatarias.Count > 0)
                    {
                        foreach (var item in areasDestinatarias)
                        {
                            HtmlGenericControl divPrincipal = new HtmlGenericControl("div");
                            divPrincipal.Attributes.Add("class", "btn btn-sm alert-info");

                            Label label = new Label();
                            label.Text = "<strong>Area: </strong> " + item.Nombre + " ";

                            LinkButton link = new LinkButton();
                            link.ID = "IDArea_" + item.Id;
                            link.Attributes.Add("runat", "server");
                            link.Text = "[X]";
                            link.ToolTip = "Quitar";
                            link.CommandArgument = item.Id.ToString();
                            link.Click += new EventHandler(this.QuitarAreaDestinatario_Click);

                            divPrincipal.Controls.Add(label);
                            divPrincipal.Controls.Add(link);

                            div_destinatarios.Controls.Add(divPrincipal);
                        }
                    }

                    #endregion
                }
                else
                {
                    div_destinatarios.Attributes.Add("class", "well well-sm");
                    div_destinatarios.InnerText = "Agregar destinatarios";
                }
            }
            else 
            {
                HtmlGenericControl divPrincipal = new HtmlGenericControl("div");
                divPrincipal.Attributes.Add("class", "btn btn-sm alert-warning");

                Label label = new Label();
                label.Text = "<strong>TODOS LOS AGENTES</strong> ";

                LinkButton link = new LinkButton();
                link.ID = "IDAgente_todos";
                link.Attributes.Add("runat", "server");
                link.Text = "[X]";
                link.ToolTip = "Quitar";
                link.CommandArgument = "Todos";
                link.Click += new EventHandler(this.QuitarDestinatario_Click);

                divPrincipal.Controls.Add(label);
                divPrincipal.Controls.Add(link);

                div_destinatarios.Controls.Add(divPrincipal);
            }
        }

        protected void QuitarDestinatario_Click(object sender, EventArgs e)
        {
            string command = ((LinkButton)sender).CommandArgument;
            int idAEliminar = 0;
            if (int.TryParse(command, out idAEliminar))
            {
                QuitarDestinatario(idAEliminar);
            }
            else
            {
                Session["Todos"] = false;
            }
            ActualizarDestinatarios();
            CargarAgentes();
        }

        protected void QuitarAreaDestinatario_Click(object sender, EventArgs e)
        {
            int idAEliminar = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            QuitarAreaDestinatario(idAEliminar);
            ActualizarDestinatarios();
            CargarAreas();
        }

        private void QuitarAreaDestinatario(int idAreaAEliminar)
        {
            using (var cxt = new Model1Container())
            {
                List<Area> destinatarios = Session["AreasDestino"] as List<Area>;

                if (destinatarios != null)
                {
                    Area areaAEliminar = destinatarios.FirstOrDefault(a => a.Id == idAreaAEliminar);

                    if (areaAEliminar != null)
                    {
                        int index = destinatarios.IndexOf(areaAEliminar);
                        bool encontro = index != -1;

                        if (encontro)
                        {
                            destinatarios.RemoveAt(index);
                            Session["AreasDestino"] = destinatarios;
                        }
                    }
                }
            }
        }

        private void QuitarDestinatario(int idAgente)
        {
            using (var cxt = new Model1Container())
            {
                List<Agente> destinatarios = Session["Destinatarios"] as List<Agente>;

                if (destinatarios != null)
                {
                    Agente agAEliminar = destinatarios.FirstOrDefault(a => a.Id == idAgente);

                    if (agAEliminar != null)
                    {
                        int index = destinatarios.IndexOf(agAEliminar);
                        bool encontro = index != -1;

                        if (encontro)
                        {
                            destinatarios.RemoveAt(index);
                            Session["Destinatarios"] = destinatarios;
                        }
                    }
                }
            }
        }

        private void AgregarDestinatario(Agente ag)
        {
            List<Agente> destinatarios = Session["Destinatarios"] as List<Agente>;
            bool existeEnElListado = destinatarios != null && destinatarios.Exists(a => a.Id == ag.Id);
            //si no existe debo agregarlo
            if (!existeEnElListado)
            {
                if (destinatarios == null)
                {
                    destinatarios = new List<Agente>();
                }

                destinatarios.Add(ag);

                Session["Destinatarios"] = destinatarios;
            }
        }

        private void AgregarAreaDestinatario(Area area)
        {
            List<Area> destinatarios = Session["AreasDestino"] as List<Area>;
            bool existeEnElListado = destinatarios != null && destinatarios.Exists(a => a.Id == area.Id);
            //si no existe debo agregarlo
            if (!existeEnElListado)
            {
                if (destinatarios == null)
                {
                    destinatarios = new List<Area>();
                }

                destinatarios.Add(area);

                Session["AreasDestino"] = destinatarios;
            }
        }

        protected void ControlarChecks(object sender, EventArgs e)
        {
            var cxt = new Model1Container();
            foreach (GridViewRow item in gv_Agentes.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    bool seleccionado = ((CheckBox)item.Cells[0].FindControl("chkbox")).Checked;
                    int legajoAgente = Convert.ToInt32(item.Cells[1].Text);
                    Agente ag = cxt.Agentes.First(a => a.Legajo == legajoAgente);

                    if (ag != null)
                    {
                        if (seleccionado)
                        {
                            AgregarDestinatario(ag);
                        }
                        else
                        {
                            QuitarDestinatario(ag.Id);
                        }
                    }
                }
            }

            foreach (GridViewRow item in gv_Areas.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    bool seleccionado = ((CheckBox)item.Cells[0].FindControl("chkbox")).Checked;
                    int idArea = ((CheckBox)item.Cells[0].FindControl("chkbox")).TabIndex;
                    Area area = cxt.Areas.First(a => a.Id == idArea);

                    if (area != null)
                    {
                        if (seleccionado)
                        {
                            AgregarAreaDestinatario(area);
                        }
                        else
                        {
                            QuitarAreaDestinatario(area.Id);
                        }
                    }
                }
            }
            
            ActualizarDestinatarios();
        }

        protected void btn_Enviar_Click(object sender, EventArgs e)
        {
            List<Agente> destinatarios = Session["Destinatarios"] as List<Agente>;
            List<Area> areasDestinatarias = Session["AreasDestino"] as List<Area>;
            bool todos = Convert.ToBoolean(Session["Todos"]);
            if (((destinatarios != null && destinatarios.Count > 0) 
                || (areasDestinatarias != null && areasDestinatarias.Count > 0) 
                || todos) && 
                ddl_asunto.SelectedItem.Text != "Seleccione el motivo del mensaje:" && 
                CuerpoMensaje.Text.Length > 0 )
            {

                using (var cxt = new Model1Container())
                {

                    Mensaje mensaje = new Mensaje();
                    Agente agenteLogueado = Session["UsuarioLogueado"] as Agente;
                    Agente agCxt = cxt.Agentes.First(a => a.Id == agenteLogueado.Id);
                    mensaje.Asunto = ddl_asunto.SelectedItem.Text;
                    mensaje.Cuerpo = CuerpoMensaje.Text;
                    mensaje.Agente = agCxt;
                    mensaje.FechaEnvio = DateTime.Now;
                    List<Agente> agentesNucleados = new List<Agente>();

                    if (!todos)
                    {
                        if (destinatarios != null)
                        {
                            foreach (Agente agente in destinatarios)
                            {
                                if (agentesNucleados.FirstOrDefault(a => a.Id == agente.Id) == null)
                                {
                                    agentesNucleados.Add(cxt.Agentes.First(a => a.Id == agente.Id));
                                }
                            }
                        }

                        if (areasDestinatarias != null)
                        {
                            foreach (Area area in areasDestinatarias)
                            {
                                foreach (Agente agente in area.Agentes)
                                {
                                    if (agente.FechaBaja == null)
                                    {
                                        if (agentesNucleados.FirstOrDefault(a => a.Id == agente.Id) == null)
                                        {
                                            agentesNucleados.Add(cxt.Agentes.First(a => a.Id == agente.Id));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        agentesNucleados = cxt.Agentes.Where(aa => aa.FechaBaja == null).ToList();
                    }

                    foreach (Agente agente in agentesNucleados)
                    {
                        Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == agente.Id);
                        Destinatario dest = new Destinatario();
                        dest.Agente = destinatarioCxt;
                        mensaje.Destinatarios.Add(dest);
                    }

                    cxt.SaveChanges();

                    Response.Redirect("~/Aplicativo/Inbox.aspx");
                }
            }
            else
            {
                Controles.MessageBox.Show(this,"El mensaje debe tener destinatarios, asunto y cuerpo.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_Todos_Click(object sender, EventArgs e)
        {
            Session["Todos"] = true;
            ActualizarDestinatarios();
        }

    }
}