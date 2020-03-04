using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Feriado_Nuevo : System.Web.UI.Page
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

                CargarAreas();
                
                Session["todos_asueto_parcial"] = false;

                if (usuariologueado.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    /*
                     preguntar por el tipo y si es asueto hacer esto con el valor que viene en el Id
                      
                     */
                    MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);


                    string tipo = Session["Tipo"] != null ? Session["Tipo"].ToString() : null;
                    Session["Tipo"] = null;
                    string idStr = Session["Id"] != null ? Session["Id"].ToString() : null;
                    Session["Id"] = null;

                    if (tipo != null)
                    {
                        switch (tipo)
                        {
                            case "Feriado":
                                if (idStr != null)
                                {
                                    int id = Convert.ToInt32(idStr);
                                    Model1Container cxt = new Model1Container();
                                    Feriado f = cxt.Feriados.First(fer => fer.Id == id);
                                    tb_Fecha.Value = f.Dia.ToLongDateString();
                                    tb_Motivo.Text = f.Motivo;
                                    Session["Feriado"] = f;
                                }
                                else
                                {
                                    Session["Feriado"] = null;
                                }
                                break;

                            case "Asueto":

                                List<Area> areasDestinatarias = new List<Area>();

                                DateTime dia = Convert.ToDateTime(idStr.Split('_')[0]);
                                string hora = idStr.Split('_')[1].Replace("desde las ", "").Replace("hasta las ", "");
                                string entrada_salida = idStr.Contains("desde") ? "Salida" : "Entrada";
                                string observacion = idStr.Split('_')[2];

                                Session["asueto_dia"] = dia;
                                Session["asueto_hora"] = hora;
                                Session["asueto_entrada_salida"] = entrada_salida;
                                Session["asueto_observacion"] = observacion;

                                tb_fecha_asueto_parcial.Value = dia.ToShortDateString();
                                tb_hora_entrada_salida.Value = hora;
                                tb_observaciones.Text = observacion;
                                ddl_entrada_salida.Text = entrada_salida;

                                using (var cxt = new Model1Container())
                                {
                                    List<AsuetoParcial> aapp = cxt.AsuetosParciales.Where(ap => ap.Dia == dia && ap.Hora == hora && ap.HorarioQueModifica == entrada_salida && ap.Observacion == observacion).ToList();
                                    foreach (AsuetoParcial ap in aapp)
                                    {
                                        areasDestinatarias.Add(ap.Area);
                                    }

                                    Session["listado_de_asuetos_parciales"] = aapp;
                                }

                                Session["areas_asueto_parcial"] = areasDestinatarias;
                                ActualizarDestinatarios();
                                CargarAreas();
                                break;

                            default:
                                break;
                        }

                    }
                }
            }

            ActualizarDestinatarios();
        }

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            Page.Validate("feriado");
            if (Page.IsValid)
            {
                Model1Container cxt = new Model1Container();
                Feriado f = Session["Feriado"] as Feriado;
                if (f == null)
                {
                    f = new Feriado();
                    cxt.Feriados.AddObject(f);
                }

                f.Dia = Convert.ToDateTime(tb_Fecha.Value);
                f.Motivo = tb_Motivo.Text;

                cxt.SaveChanges();

                Response.Redirect("~/Aplicativo/Personal_Feriado_Listado.aspx");
            }
        }

        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Feriado_Listado.aspx");
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_Fecha.Value, out d);
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ObtenerFeriados(tb_Fecha.Value);
        }

        private bool ObtenerFeriados(string dia)
        {
            try
            {
                Model1Container cxt = new Model1Container();
                DateTime d = Convert.ToDateTime(dia);
                return (cxt.Feriados.Count(f => f.Dia == d)) == 0;
            }
            catch
            {
                return false;
            }
        }


        #region Asuetos parciales

        private struct ItemGridArea
        {
            public int IdArea { get; set; }
            public string Nombre { get; set; }
            public bool Seleccionado { get; set; }
        }

        private void CargarAreas()
        {
            using (var cxt = new Model1Container())
            {
                List<Area> destinatarios = Session["areas_asueto_parcial"] as List<Area>;
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
            bool todos = Convert.ToBoolean(Session["todos_asueto_parcial"]);
            List<Area> areasDestinatarias = Session["areas_asueto_parcial"] as List<Area>;

            div_destinatarios.Controls.Clear();
            div_destinatarios.Attributes.Clear();

            if (!todos)
            {
                if ((areasDestinatarias != null && areasDestinatarias.Count > 0))
                {
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
                    HtmlGenericControl input = new HtmlGenericControl("input");
                    input.Attributes["class"] = "form-control";
                    input.Attributes["disabled"] = "true";
                    input.Attributes["placeholder"] = "Agregar areas";
                    div_destinatarios.Controls.Add(input);
                }
            }
            else
            {
                HtmlGenericControl divPrincipal = new HtmlGenericControl("div");
                divPrincipal.Attributes.Add("class", "btn btn-sm alert-warning");

                Label label = new Label();
                label.Text = "<strong>TODOS LAS AREAS</strong> ";

                LinkButton link = new LinkButton();
                link.ID = "IDAgente_todos";
                link.Attributes.Add("runat", "server");
                link.Text = "[X]";
                link.ToolTip = "Quitar";
                link.CommandArgument = "Todos";
                link.Click += new EventHandler(this.QuitarTodos_Click);

                divPrincipal.Controls.Add(label);
                divPrincipal.Controls.Add(link);

                div_destinatarios.Controls.Add(divPrincipal);
            }
        }

        private void QuitarTodos_Click(object sender, EventArgs e)
        {
            Session["todos_asueto_parcial"] = false;
            ActualizarDestinatarios();
        }

        protected void QuitarAreaDestinatario_Click(object sender, EventArgs e)
        {
            int idAEliminar = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            QuitarAreaDestinatario(idAEliminar);
            ActualizarDestinatarios();
            CargarAreas();
        }

        private void AgregarAreaDestinatario(Area area)
        {
            List<Area> destinatarios = Session["areas_asueto_parcial"] as List<Area>;
            bool existeEnElListado = destinatarios != null && destinatarios.Exists(a => a.Id == area.Id);
            //si no existe debo agregarlo
            if (!existeEnElListado)
            {
                if (destinatarios == null)
                {
                    destinatarios = new List<Area>();
                }

                destinatarios.Add(area);

                Session["areas_asueto_parcial"] = destinatarios;
            }
        }

        private void QuitarAreaDestinatario(int idAreaAEliminar)
        {
            using (var cxt = new Model1Container())
            {
                List<Area> destinatarios = Session["areas_asueto_parcial"] as List<Area>;

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
                            Session["areas_asueto_parcial"] = destinatarios;
                        }
                    }
                }
            }
        }

        protected void btn_Todos_Click(object sender, EventArgs e)
        {
            Session["todos_asueto_parcial"] = true;
            ActualizarDestinatarios();
        }

        protected void ControlarChecks(object sender, EventArgs e)
        {
            var cxt = new Model1Container();

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

        protected void cv_fecha_asueto_valida_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_fecha_asueto_parcial.Value, out d);
        }

        protected void cv_dia_libre_asueto_parcial_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ObtenerFeriados(tb_fecha_asueto_parcial.Value);
        }

        protected void cv_al_menos_un_area_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool todos = Convert.ToBoolean(Session["todos_asueto_parcial"]);
            List<Area> areasDestinatarias = Session["areas_asueto_parcial"] as List<Area>;

            if (!todos)
            {
                args.IsValid = areasDestinatarias != null && areasDestinatarias.Count > 0;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void btn_guardar_entrada_salida_Click(object sender, EventArgs e)
        {
            Validate("asueto");

            if (IsValid)
            {
                using (var cxt = new Model1Container())
                {
                    if (Session["listado_de_asuetos_parciales"] != null)
                    {
                        DateTime dia_original = Convert.ToDateTime(Session["asueto_dia"]);
                        string hora_original = Session["asueto_hora"].ToString();
                        string entrada_salida = Session["asueto_entrada_salida"].ToString();
                        string observacion = Session["asueto_observacion"].ToString();
                        //elimino todos los asuetos parciales que fueron creados bajo estos datos (es que asueto que quieren editar) entonces los elimino y los creo de nuevo
                        cxt.AsuetosParciales.Where(ap => ap.Dia == dia_original && ap.Hora == hora_original && ap.HorarioQueModifica == entrada_salida && ap.Observacion == observacion).ToList().ForEach(cxt.DeleteObject);
                        cxt.SaveChanges();
                        Session["listado_de_asuetos_parciales"] = null;
                    }

                    DateTime dia = Convert.ToDateTime(tb_fecha_asueto_parcial.Value);
                    string hora = tb_hora_entrada_salida.Value;
                    string observaciones = tb_observaciones.Text;
                    string entradaSalida = ddl_entrada_salida.Text;

                    bool todos = Convert.ToBoolean(Session["todos_asueto_parcial"]);
                    List<Area> areasDestinatarias = Session["areas_asueto_parcial"] as List<Area>;
                    if (!todos)
                    {
                        foreach (Area item in areasDestinatarias)
                        {
                            AgregarAsueto(dia, hora, item, observaciones, entradaSalida);
                        }
                    }
                    else
                    {
                        foreach (Area item in cxt.Areas)
                        {
                            AgregarAsueto(dia, hora, item, observaciones, entradaSalida);
                        }
                    }
                }

                Response.Redirect("~/Aplicativo/Personal_Feriado_Listado.aspx");
            }
        }

        private void AgregarAsueto(DateTime dia, string hora, Area area, string observacion, string entrada_o_salida)
        {
            using (var cxt = new Model1Container())
            {
                AsuetoParcial ap = new AsuetoParcial()
                {
                    Dia = dia,
                    Hora = hora,
                    HorarioQueModifica = entrada_o_salida,
                    Observacion = observacion,
                    AreaId = area.Id
                };

                cxt.AsuetosParciales.AddObject(ap);

                cxt.SaveChanges();
            }
        }

        #endregion
    }
}