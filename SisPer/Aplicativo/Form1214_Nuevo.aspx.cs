using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Form1214_Nuevo : System.Web.UI.Page
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
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                #endregion

                #region Entro por primera vez a la pagina

                //obtengo el id pasado por session y luego lo anulo si es la segunda ves que entro ya esta nullo
                int id214 = Session["id214"] != null ? Convert.ToInt32(Session["id214"].ToString()) : 0;
                Session["id214"] = null;

                if (id214 != 0)
                {
                    //si existe un id cargo el formulario con ese id, recordar que la segunda ves que entra ya el id no existe mas
                    using (var cxt = new Model1Container())
                    {
                        Session["Form214"] = cxt.Formularios1214.First(f1214 => f1214.Id == id214);
                    }
                }
                else
                {
                    //si no viene id se verifica la session si ya tiene cargado un formulario se usa ese sino se crea uno nuevo.
                    //al guardar o cancelar se elimina esta variable de session
                    if (Session["Form214"] == null)
                    {
                        Formulario1214 f1214 = new Formulario1214();
                        f1214.AgenteId = usuarioLogueado.Id;
                        f1214.Estado = Estado1214.Confeccionado;
                        Session["Form214"] = f1214;
                        //List<PosAgente> nomina = new List<PosAgente>();
                    }
                    else
                    {
                        //sigo con el formulario 1214 que tengo en session
                    }

                }

                #endregion

                if (Session["Form214"] != null)
                {
                    CargarDatos214();
                }

                CargarAgentesDisponibles();
            }
        }

        private void ActualizarF1214ConDatosDeControles(Formulario1214 f1214)
        {
            f1214.Destino = tb_destino.Text;
            f1214.Fuera_provincia = dentro_fuera.Value == "2";
            f1214.Usa_chofer = ddl_con_chofer.SelectedValue == "0";
            f1214.TareasACumplir = tb_tareas.Text;
        }

        private void CargarDatos214()
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            Model1Container cxt = new Model1Container();
            Agente aglog = Session["UsuarioLogueado"] as Agente;
            Agente usuarioLogueado = cxt.Agentes.First(aa => aa.Id == aglog.Id);
            //armo una nómina con los agentes que no fueron cancelados
            List<PosAgente> nomina = new List<PosAgente>();

            if (f1214.Id == 0)
            {
                //No esta guardado habilitar confeccion
                btn_Confeccionar.Visible = true;
                btn_Cancelar.Visible = true;

                btn_Enviar.Visible = false;
                btn_Anular.Visible = false;
                btn_Imprimir.Visible = false;
                btn_aprobar.Visible = false;

                lbl_encabezado1214.Text = "Nuevo formulario 1214";
            }
            else
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);

                lbl_encabezado1214.Text = "Formulario 1214 N° " + Cadena.CompletarConCeros(6, f1214.Id);

                switch (f1214.Estado)
                {
                    case Estado1214.Confeccionado:
                        main_panel.Attributes["class"] = "panel panel-default";
                        break;
                    case Estado1214.Enviado:
                        main_panel.Attributes["class"] = "panel panel-success";
                        lbl_encabezado1214.Text = lbl_encabezado1214.Text + " - Enviado";
                        break;
                    case Estado1214.Anulado:
                        main_panel.Attributes["class"] = "panel panel-danger";
                        lbl_encabezado1214.Text = lbl_encabezado1214.Text + " - Anulado";
                        break;
                    default:
                        break;
                }

                btn_Confeccionar.Visible = false;
                btn_Cancelar.Text = "Volver";
                btn_Cancelar.Visible = true;

                btn_Enviar.Visible = f1214.PuedeEnviar();
                btn_Anular.Visible = f1214.Estado != Estado1214.Anulado && usuarioLogueado.Perfil == PerfilUsuario.Personal;
                btn_Imprimir.Visible = f1214.Estado == Estado1214.Enviado;
                btn_aprobar.Visible = f1214.Estado == Estado1214.Enviado && (usuarioLogueado.Perfil == PerfilUsuario.Personal || usuarioLogueado.Area.Nombre == "Despacho");
            }

            #region fechas, destino, tareas

            if (f1214.Desde != DateTime.MinValue && f1214.Hasta != DateTime.MinValue)
            {
                int dias = (f1214.Hasta - f1214.Desde).Days + 1;
                lbl_diascorridos.Text = "Comisión de servicios por " + Numalet.ToCardinal(dias) + " (" + dias.ToString() + ") días corridos a partir del " + f1214.Desde.ToString("dd 'de' MMMM 'de' yyyy") + " al " + f1214.Hasta.ToString("dd 'de' MMMM 'de' yyyy") + " inclusive.-";
                div_selecciona_fechas.Visible = false;
                div_muestra_fechas.Visible = true;

                btn_confirmar_fechas.Visible = false;
                btn_cambiar_fechas.Visible = f1214.Id == 0;//Muestro el boton para cambiar las fechas mientras el formulario no haya sido guardado, de ser asi deberá anular y volver a generar
            }
            else
            {
                lbl_diascorridos.Text = string.Empty;
                div_selecciona_fechas.Visible = true;
                div_muestra_fechas.Visible = false;

                btn_confirmar_fechas.Visible = true;
                btn_cambiar_fechas.Visible = false;
            }

            tb_desde.Value = f1214.Desde.ToShortDateString();
            tb_hasta.Value = f1214.Hasta.ToShortDateString();

            tb_destino.Text = f1214.Destino;
            dentro_fuera.Value = f1214.Fuera_provincia ? "2" : "1";
            tb_tareas.Text = f1214.TareasACumplir;
            tb_destino.Enabled = tb_tareas.Enabled = ddl_con_chofer.Enabled = ddl_movilidad.Enabled = txt_dominio_vehiculo_oficial.Enabled = f1214.Id == 0;
            input_group_destino.Disabled = f1214.Id != 0;

            #endregion

            #region Nomina

            int lugar = 2;

            foreach (Agente1214 item in f1214.Nomina)
            {
                if (item.Estado != EstadoAgente1214.Cancelado && !item.Chofer)
                {
                    PosAgente pa = new PosAgente();

                    pa.Agente = cxt.Agentes.First(aa => aa.Id == item.Id_Agente);

                    if (item.Estado == EstadoAgente1214.Solicitado)
                    {
                        pa.Autorizado = null;
                    }
                    else
                    {
                        pa.Autorizado = item.Estado == EstadoAgente1214.Aprobado ? true : false;
                    }
                    if (!item.JefeComicion)
                    {
                        pa.Lugar = lugar;
                        lugar++;
                    }
                    else
                    {
                        pa.Lugar = 1;
                    }

                    nomina.Add(pa);
                }
            }

            MostrarNomina(nomina);

            Agente1214 chofer = f1214.Nomina.FirstOrDefault(aa => aa.Chofer && aa.Estado != EstadoAgente1214.Cancelado);

            MostrarChofer(chofer);

            #endregion

            #region gastos
            if (f1214.Id > 0)
            {
                ddl_movilidad.SelectedIndex = Convert.ToInt32(f1214.Movilidad);
                ddl_movilidad.Enabled = false;

                switch (ddl_movilidad.SelectedValue)
                {
                    case "1":
                        lbl_monto_anticipo.InnerText = "Gastos vehículo: nafta, otros.";
                        break;
                    case "2":
                        lbl_monto_anticipo.InnerText = "Gastos vehículo: nafta, otros.";
                        break;
                    case "3":
                        lbl_monto_anticipo.InnerText = "Pasajes";
                        break;
                    default:
                        break;
                }

                tb_monto_anticipo.Value = f1214.MontoAnticipo.ToString();
                if (f1214.Estado != Estado1214.Confeccionado)
                {
                    tb_monto_anticipo.Attributes["disabled"] = "disabled";
                }
            }
            #endregion

        }

        private void MostrarChofer(Agente1214 chofer)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            if (chofer != null)
            {
                using (var cxt = new Model1Container())
                {
                    Agente ag = cxt.Agentes.FirstOrDefault(aa => aa.Id == chofer.Id_Agente);

                    if (f1214.Id > 0)
                    {
                        btn_del_agente_chofer.Visible = false;
                    }

                    btn_del_agente_chofer.CommandArgument = ag.Id.ToString();

                    txt_chofer.Attributes.Clear();
                    txt_chofer.Attributes.Add("class", "form-control alert-success");
                    txt_chofer.Attributes.Add("style", "background-color:yellowgreen");
                    txt_chofer.Text = ag.ApellidoYNombre;

                    btn_chofer.Visible = false;
                    group_chofer.Visible = true;
                }
            }
            else
            {
                if (f1214.Estado == Estado1214.Confeccionado)
                {
                    btn_del_agente_chofer.CommandArgument = string.Empty;
                    btn_chofer.Visible = true;
                    group_chofer.Visible = false;
                    txt_chofer.Text = string.Empty;
                }
                else
                {
                    btn_del_agente_chofer.CommandArgument = string.Empty;
                    txt_chofer.Text = string.Empty;
                    btn_chofer.Visible = false;
                    group_chofer.Visible = false;
                }
            }

        }

        private void MostrarNomina(List<PosAgente> Nomina)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            for (int i = 1; i <= 10; i++)
            {
                TextBox txt_agente = (TextBox)ControlExtensions.FindControlRecursive(this, "txt_agente_" + i.ToString());
                HtmlButton btn_agente = (HtmlButton)ControlExtensions.FindControlRecursive(this, "btn_agente_" + i.ToString());
                HtmlGenericControl group_agente = (HtmlGenericControl)ControlExtensions.FindControlRecursive(this, "group_agente_" + i.ToString());
                Button btn_eliminar_agente = (Button)ControlExtensions.FindControlRecursive(this, "btn_del_agente_" + i.ToString());
                btn_eliminar_agente.Visible = f1214.Estado == Estado1214.Confeccionado;

                if (Nomina.Exists(aann => aann.Lugar == i))
                {
                    PosAgente agNomina = Nomina.First(aann => aann.Lugar == i);

                    btn_eliminar_agente.CommandArgument = agNomina.Agente.Id.ToString();

                    if (i == 1 && f1214.Id > 0)
                    {
                        btn_eliminar_agente.Visible = false;
                    }

                    txt_agente.Attributes.Clear();

                    if (agNomina.Autorizado == null)
                    {
                        txt_agente.Attributes.Add("class", "form-control alert-warning");
                        txt_agente.Attributes.Add("style", "background-color:navajowhite");
                        txt_agente.Attributes.Add("title", "Pendiente de autorización por parte del Jefe");
                    }
                    else
                    {
                        if (agNomina.Autorizado.Value == true)
                        {
                            txt_agente.Attributes.Add("class", "form-control alert-success");
                            txt_agente.Attributes.Add("style", "background-color:yellowgreen");
                            txt_agente.Attributes.Add("title", "Autorizado por el Jefe");
                        }
                        else
                        {
                            //txt_agente.Attributes["class"] = "form-control alert-danger";
                            txt_agente.Attributes.Add("class", "form-control alert-danger");
                            txt_agente.Attributes.Add("style", "background-color:lightpink");
                            txt_agente.Attributes.Add("title", "Agente no autorizado por el Jefe");
                        }
                    }

                    btn_agente.Visible = false;
                    group_agente.Visible = true;
                    txt_agente.Text = agNomina.Agente.ApellidoYNombre;
                }
                else
                {
                    if (f1214.Estado == Estado1214.Confeccionado)
                    {
                        btn_eliminar_agente.CommandArgument = string.Empty;
                        btn_agente.Visible = true;
                        group_agente.Visible = false;
                        txt_agente.Text = string.Empty;
                    }
                    else
                    {
                        btn_eliminar_agente.CommandArgument = string.Empty;
                        txt_agente.Text = string.Empty;
                        btn_agente.Visible = false;
                        group_agente.Visible = false;
                    }
                }
            }
        }

        private struct PosAgente
        {
            public int Lugar { get; set; }
            public Agente Agente { get; set; }
            /// <summary>
            /// True: Autorizado
            /// False: No autorizado
            /// Null: pendiente de autorización
            /// </summary>
            public Nullable<bool> Autorizado { get; set; }
        }

        private void CargarAgentesDisponibles()
        {
            using (var cxt = new Model1Container())
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                var agentes = (from aa in cxt.Agentes
                               where aa.FechaBaja == null
                               select new
                               {
                                   IdAgente = aa.Id,
                                   Legajo = aa.Legajo,
                                   Agente = aa.ApellidoYNombre,
                                   Area = aa.Area.Nombre
                               }).OrderBy(s => s.Agente).ToList();

                if (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal)
                {
                    gv_agentes_para_1.DataSource = agentes;
                    gv_agentes_para_1.DataBind();
                }
                else
                {
                    gv_agentes_para_1.DataSource = agentes.Where(aa => aa.IdAgente == usuarioLogueado.Id).ToList();
                    gv_agentes_para_1.DataBind();
                }

                for (int i = 2; i <= 11; i++)
                {
                    if (i < 11)
                    {
                        //cargar gridviews para agentes de la nomina
                        GridView gv_agentes_para = (GridView)ControlExtensions.FindControlRecursive(this, "gv_agentes_para_" + i.ToString());
                        gv_agentes_para.DataSource = agentes;
                        gv_agentes_para.DataBind();
                    }
                    else
                    {
                        //cargar gridview para seleccion de chofer
                        gv_agentes_para_chofer.DataSource = agentes;
                        gv_agentes_para_chofer.DataBind();
                    }
                }


            }
        }

        protected void btn_confirmar_fechas_Click(object sender, EventArgs e)
        {
            Page.Validate("fecha");
            if (IsValid)
            {
                DateTime desde;
                DateTime hasta;
                DateTime.TryParse(tb_desde.Value, out desde);
                DateTime.TryParse(tb_hasta.Value, out hasta);
                Formulario1214 f1214 = Session["Form214"] as Formulario1214;
                f1214.Desde = desde;
                f1214.Hasta = hasta;
                ActualizarF1214ConDatosDeControles(f1214);
                Session["Form214"] = f1214;
                CargarDatos214();
            }
        }

        protected void btn_cambiar_fechas_Click(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            f1214.Desde = DateTime.MinValue;
            f1214.Hasta = DateTime.MinValue;

            ActualizarF1214ConDatosDeControles(f1214);

            Session["Form214"] = f1214;

            LimpiarNomina();

            CargarDatos214();
        }

        private void LimpiarNomina()
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            if (f1214.Id > 0)
            {
                using (var cxt = new Model1Container())
                {
                    f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);

                    while (f1214.Nomina.Count() > 0)
                    {
                        Agente1214 agNomina = f1214.Nomina.First();
                        if (agNomina.Estado == EstadoAgente1214.Aprobado)
                        {
                            //El agente fue aprobado para la comision en las fechas que se estan por cambiar
                            #region envio notificación de que el agente bajo del listado porque cambiaron las fechas en las que se le habia aprobado

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

                            Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == agNomina.Id_Jefe);
                            Notificacion notificacion = new Notificacion();

                            notificacion.Descripcion = "Por la presente se notifica que el agente " + agNomina.Agente.ApellidoYNombre + " fue removido de la comición aprobada por usted para las fechas " + f1214.Desde.ToShortDateString() + " a " + f1214.Hasta.ToShortDateString() + " ya que dicha comisión ha sufrico cambios en las fechas mencionadas.-";
                            notificacion.Destinatario = destinatarioCxt;
                            notificacion.ObservacionPendienteRecibir = string.Empty;

                            notificacion.Tipo = nt;
                            cxt.Notificaciones.AddObject(notificacion);

                            Notificacion_Historial notHist = new Notificacion_Historial()
                            {
                                Agente = agNomina.Agente,
                                Estado = ne,
                                Fecha = DateTime.Now,
                                Notificacion = notificacion
                            };

                            cxt.Notificacion_Historiales.AddObject(notHist);

                            #endregion
                        }

                        agNomina.Estado = EstadoAgente1214.Cancelado;
                    }

                    cxt.SaveChanges();
                }
            }
            else
            {
                while (f1214.Nomina.Count() > 0)
                {
                    //elimino todos los agentes de la nomina
                    f1214.Nomina.Remove(f1214.Nomina.First());
                }
            }

            Session["Form214"] = f1214;
        }

        #region Agregar agente nómina

        private bool ChequearAgente(Agente ag)
        {
            Session["ErrorInclusion"] = string.Empty;
            Session["Autorizado"] = null;
            bool ret = true;

            if (div_selecciona_fechas.Visible) //No selecciono las fechas!
            {
                ret = false;
                Session["ErrorInclusion"] = "Error - Primero debe confirmar las fechas en las que se realizará la comisión.-";
            }
            else
            {
                //ya selecciono las fechas, debo verificar si el agente ya no tiene comisiones asignadas o estados entre esas fechas.-
                DateTime desde;
                DateTime hasta;
                DateTime.TryParse(tb_desde.Value, out desde);
                DateTime.TryParse(tb_hasta.Value, out hasta);
                string error = string.Empty;
                for (DateTime dia = desde; dia <= hasta; dia = dia.AddDays(1))
                {
                    EstadoAgente ea = ag.ObtenerEstadoAgenteParaElDia(dia);
                    if (ea != null)
                    {
                        if (!(
                            ea.TipoEstado.Estado == "Marca Planilla" ||
                            ea.TipoEstado.Estado == "Fin de semana" ||
                            ea.TipoEstado.Estado == "Natalicio"
                            ))
                        {
                            ret = false;
                            error = error == string.Empty ? dia.ToShortDateString() + " por " + ea.TipoEstado.Estado : error + ", " + dia.ToShortDateString() + " por " + ea.TipoEstado.Estado;
                        }
                    }
                }

                if (error != string.Empty)
                {
                    Session["ErrorInclusion"] = "Error - El agente no puede asistir las siguientes fechas: " + error;
                }
            }

            if (ret)//Si esta en condiciones de agregar verifico si tiene o no que solicitar permiso del jefe
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                List<Agente> subordinados = usuarioLogueado.ObtenerAgentesSubordinadosDirectos();
                if ((subordinados.Exists(s => s.Id == ag.Id) && (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal)) ||
                    (usuarioLogueado.Id == ag.Id && (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal)))
                {
                    Session["Autorizado"] = true;
                }
            }

            return ret;
        }

        public void AgregarAgenteNomina(Agente ag, int lugar)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            var cxt = new Model1Container();
            bool yaExiste = false;

            ActualizarF1214ConDatosDeControles(f1214);


            if (f1214.Id > 0)
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
                int existencia = f1214.Nomina.Where(nn => nn.Agente.Id == ag.Id && (nn.Estado == EstadoAgente1214.Aprobado || nn.Estado == EstadoAgente1214.Solicitado)).Count();
                if (existencia > 0)
                {
                    yaExiste = true;
                }
            }
            else
            {
                yaExiste = f1214.Nomina.Where(nn => nn.Id_Agente == ag.Id && (nn.Estado == EstadoAgente1214.Aprobado || nn.Estado == EstadoAgente1214.Solicitado)).Count() > 0;
            }

            if (yaExiste)
            {
                Controles.MessageBox.Show(this, "El agente que quiere agregar ya se encuentra entre los seleccionados.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (ChequearAgente(ag))
                {
                    Nullable<bool> autorizado = null;
                    Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                    if (Session["Autorizado"] != null)
                    {
                        autorizado = Convert.ToBoolean(Session["Autorizado"]);
                    }

                    Agente1214 agNomina = new Agente1214();
                    agNomina.Id_Agente = ag.Id;
                    agNomina.JefeComicion = lugar == 1;
                    agNomina.Chofer = false;
                    agNomina.Id_Jefe = ag.Area.Agentes.Where(aa => (aa.Jefe || aa.JefeTemporal) && aa.FechaBaja == null).First().Id;

                    if (autorizado == null)
                    {
                        agNomina.Estado = EstadoAgente1214.Solicitado;
                    }
                    else
                    {
                        if (autorizado.Value == true)
                        {
                            agNomina.Estado = EstadoAgente1214.Aprobado;
                            agNomina.Id_Jefe = usuarioLogueado.Id;
                            agNomina.FechaAprobacion = DateTime.Now;
                        }
                    }

                    f1214.Nomina.Add(agNomina);

                    if (f1214.Id > 0)
                    {
                        cxt.SaveChanges();
                    }

                    Session["Form214"] = f1214;
                    CargarDatos214();
                }
                else
                {
                    string error = Session["ErrorInclusion"].ToString();
                    Controles.MessageBox.Show(this, error, Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        protected void Agregarchofer(Agente ag)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            var cxt = new Model1Container();
            bool yaExiste = false;

            ActualizarF1214ConDatosDeControles(f1214);


            if (f1214.Id > 0)
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
            }

            yaExiste = f1214.Nomina.Count(chof => chof.Chofer && chof.Estado != EstadoAgente1214.Cancelado) > 0;


            if (yaExiste)
            {
                Controles.MessageBox.Show(this, "Ya se encuentra seleccionado un chofer para esta comisión.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (ChequearAgente(ag))
                {
                    Nullable<bool> autorizado = null;
                    Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                    if (Session["Autorizado"] != null)
                    {
                        autorizado = Convert.ToBoolean(Session["Autorizado"]);
                    }

                    Agente1214 agNomina = new Agente1214();
                    agNomina.Id_Agente = ag.Id;
                    agNomina.JefeComicion = false;
                    agNomina.Chofer = true;
                    agNomina.Id_Jefe = ag.Area.Agentes.Where(aa => (aa.Jefe || aa.JefeTemporal) && aa.FechaBaja == null).First().Id;

                    agNomina.Estado = EstadoAgente1214.Aprobado;
                    agNomina.Id_Jefe = usuarioLogueado.Id;
                    agNomina.FechaAprobacion = DateTime.Now;

                    f1214.Nomina.Add(agNomina);

                    if (f1214.Id > 0)
                    {
                        cxt.SaveChanges();
                    }

                    Session["Form214"] = f1214;
                    CargarDatos214();
                }
                else
                {
                    string error = Session["ErrorInclusion"].ToString();
                    Controles.MessageBox.Show(this, error, Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        protected void btn_agente_x_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int lugar = Convert.ToInt32(((Button)sender).ID.Replace("btn_agente_", ""));
                int idAgente = Convert.ToInt32(((Button)sender).CommandArgument);
                Agente ag = cxt.Agentes.FirstOrDefault(aa => aa.Id == idAgente);
                AgregarAgenteNomina(ag, lugar);
            }
        }

        protected void btn_agente_chofer_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int idAgente = Convert.ToInt32(((Button)sender).CommandArgument);
                Agente ag = cxt.Agentes.FirstOrDefault(aa => aa.Id == idAgente);
                Agregarchofer(ag);
            }
        }

        #endregion

        #region Eliminar agente nómina

        protected void btn_del_agente_x_ServerClick(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            var cxt = new Model1Container();
            if (f1214.Id > 0)
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
            }

            ActualizarF1214ConDatosDeControles(f1214);

            int idAgente = Convert.ToInt32(((Button)sender).CommandArgument);
            Agente1214 agNomina = f1214.Nomina.FirstOrDefault(aa => aa.Id_Agente == idAgente && aa.Estado != EstadoAgente1214.Cancelado);
            EstadoAgente1214 estado_antes_de_cancelar = agNomina.Estado;
            if (agNomina != null)
            {
                if (agNomina.Estado != EstadoAgente1214.Rechazado)
                {
                    Agente ag = Session["UsuarioLogueado"] as Agente;
                    agNomina.Estado = EstadoAgente1214.Cancelado;
                    agNomina.Rechazado_por_agente_id = ag.Id;
                    agNomina.FechaRechazo = DateTime.Now;
                }
                else
                { //si ya fue rechazado por el jefe o personal no toco esos valores
                    agNomina.Estado = EstadoAgente1214.Cancelado;
                }


                if (f1214.Id > 0)
                {
                    if (estado_antes_de_cancelar == EstadoAgente1214.Aprobado)
                    {
                        if (f1214.Estado == Estado1214.Aprobada)
                        {//si esta aprobada se generaron los estados de comision, hay que eliminarle los estados
                            for (DateTime dia = f1214.Desde; dia <= f1214.Hasta; dia = dia.AddDays(1))
                            {
                                cxt.EstadosAgente.DeleteObject(cxt.EstadosAgente.First(eeaa => eeaa.TipoEstado.Estado == "Comisión" && eeaa.AgenteId == agNomina.Agente.Id && eeaa.Dia == dia));
                            }
                        }

                        Mensaje mensaje = new Mensaje();
                        Agente agente = f1214.GeneradoPor;
                        Agente agCxt = cxt.Agentes.First(a => a.Id == agente.Id);
                        mensaje.Asunto = "Agente eliminado de la comisión N°" + Cadena.CompletarConCeros(6, f1214.Id);
                        mensaje.Cuerpo = "Se informa por medio de la presente, que el agente <b>" + agNomina.Agente.ApellidoYNombre + "</b>, aprobado por usted oportunamente, fue eliminado de la comisión N° <b>" + Cadena.CompletarConCeros(6, f1214.Id) + "</b> con destino a <b>" + f1214.Destino + "</b>, por lo que no participará de la misma.-";
                        mensaje.Agente = agCxt;
                        mensaje.FechaEnvio = DateTime.Now;

                        Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == agNomina.Jefe.Id);
                        Destinatario dest = new Destinatario();
                        dest.Agente = destinatarioCxt;
                        mensaje.Destinatarios.Add(dest);

                        cxt.Mensajes.AddObject(mensaje);
                    }

                    cxt.SaveChanges();
                }
            }

            Session["Form214"] = f1214;

            CargarDatos214();
        }

        protected void btn_del_agente_chofer_ServerClick(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            var cxt = new Model1Container();
            if (f1214.Id > 0)
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
            }

            ActualizarF1214ConDatosDeControles(f1214);

            int idAgente = Convert.ToInt32(((Button)sender).CommandArgument);
            Agente1214 agNomina = f1214.Nomina.FirstOrDefault(aa => aa.Id_Agente == idAgente && aa.Estado != EstadoAgente1214.Cancelado && aa.Chofer);

            if (agNomina != null)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;
                agNomina.Estado = EstadoAgente1214.Cancelado;
                agNomina.Rechazado_por_agente_id = ag.Id;
                agNomina.FechaRechazo = DateTime.Now;

                if (f1214.Id > 0)
                {
                    cxt.SaveChanges();
                }
            }

            Session["Form214"] = f1214;

            CargarDatos214();
        }

        #endregion

        #region Confeccion, Confirmación, Anulacion del 214, impresion
        /// <summary>
        /// Confecciona el 214 y envia la solicitudes necesarias de aprobacion para los agentes
        /// </summary>
        protected void btn_Confeccionar_Click(object sender, EventArgs e)
        {
            Page.Validate("general_214");
            if (IsValid)
            {
                Formulario1214 f1214 = Session["Form214"] as Formulario1214;
                using (var cxt = new Model1Container())
                {
                    f1214.Estado = Estado1214.Confeccionado;
                    f1214.Movilidad = ((Movilidad1214)(Convert.ToInt32(ddl_movilidad.SelectedValue)));
                    f1214.Anticipo = f1214.Movilidad == Movilidad1214.Transporte_publico ? Anticipo1214.Pasajes : Anticipo1214.Gastos_vehiculo;
                    f1214.MontoAnticipo = Convert.ToDecimal(tb_monto_anticipo.Value);
                    cxt.Formularios1214.AddObject(f1214);
                    cxt.SaveChanges();

                    Session["Form214"] = null;

                    Response.Redirect("~/Aplicativo/Formulario1214_Generados.aspx");
                }
            }
        }

        /// <summary>
        /// Cancela la confeccion del 214
        /// </summary>
        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Session["Form214"] = null;
            Response.Redirect("~/Aplicativo/Formulario1214_Generados.aspx");
        }

        /// <summary>
        /// Confirma el 214 con los agentes aprobados, el 214 pasa para su aprobacion al Administrador, desde aca ya permite la impresión del mismo
        /// </summary>
        protected void btn_Enviar_Click(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            using (var cxt = new Model1Container())
            {
                f1214 = cxt.Formularios1214.FirstOrDefault(ff => ff.Id == f1214.Id);

                if (f1214.Nomina.Where(aa => aa.Estado == EstadoAgente1214.Rechazado).Count() > 0)
                {
                    Controles.MessageBox.Show(this, "Para continuar debe eliminar los agentes RECHAZADOS (rojo) de la nómina.", Controles.MessageBox.Tipo_MessageBox.Info);
                }
                else
                {
                    f1214.Estado = Estado1214.Enviado;
                    cxt.SaveChanges();
                    Session["Form214"] = null;
                    Response.Redirect("~/Aplicativo/Formulario1214_Generados.aspx");
                }
            }
        }

        protected void btn_aprobar_Click(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            using (var cxt = new Model1Container())
            {
                f1214 = cxt.Formularios1214.FirstOrDefault(ff => ff.Id == f1214.Id);
                f1214.Estado = Estado1214.Aprobada;

                foreach (Agente1214 item in f1214.Nomina)
                {
                    if (item.Estado == EstadoAgente1214.Aprobado)
                    {
                        for (DateTime dia = f1214.Desde; dia <= f1214.Hasta; dia = dia.AddDays(1))
                        {
                            Agente agendadoPor = item.Agente;
                            Agente agentecxt = item.Agente;
                            TipoEstadoAgente te = cxt.TiposEstadoAgente.First(tea => tea.Estado == "Comisión");
                            ProcesosGlobales.AgendarEstadoDiaAgente(agendadoPor, agentecxt, dia, te);
                        }
                    }
                }

                cxt.SaveChanges();
                Session["Form214"] = null;
                Response.Redirect("~/Aplicativo/Formulario1214_Generados.aspx");
            }
        }

        /// <summary>
        /// Anula el 214 en cualquier momento después de confeccionado.
        /// </summary>
        protected void btn_Anular_Click(object sender, EventArgs e)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            using (var cxt = new Model1Container())
            {
                f1214 = cxt.Formularios1214.FirstOrDefault(ff => ff.Id == f1214.Id);

                //tengo que eliminar los estados de los dias de cada uno de los agentes
                foreach (Agente1214 item in f1214.Nomina)
                {
                    if (item.Estado == EstadoAgente1214.Aprobado)
                    {
                        if (f1214.Estado == Estado1214.Aprobada)
                        {
                            for (DateTime dia = f1214.Desde; dia <= f1214.Hasta; dia = dia.AddDays(1))
                            {
                                cxt.EstadosAgente.DeleteObject(cxt.EstadosAgente.First(eeaa => eeaa.TipoEstado.Estado == "Comisión" && eeaa.AgenteId == item.Agente.Id && eeaa.Dia == dia));
                            }
                        }

                        Mensaje mensaje = new Mensaje();
                        Agente agente = f1214.GeneradoPor;
                        Agente agCxt = cxt.Agentes.First(a => a.Id == agente.Id);
                        mensaje.Asunto = "Comisión " + Cadena.CompletarConCeros(6, f1214.Id) + " anulada.";
                        mensaje.Cuerpo = "Se informa por medio de la presente, que el agente <b>" + item.Agente.ApellidoYNombre + "</b> quien fue aprobado oportunamente por usted para la comisón N° <b>" + Cadena.CompletarConCeros(6, f1214.Id) + "</b> con destino a <b>" + f1214.Destino + "</b>, no participará de la misma debido a que fue anulada.-";
                        mensaje.Agente = agCxt;
                        mensaje.FechaEnvio = DateTime.Now;

                        Agente destinatarioCxt = cxt.Agentes.First(a => a.Id == item.Jefe.Id);
                        Destinatario dest = new Destinatario();
                        dest.Agente = destinatarioCxt;
                        mensaje.Destinatarios.Add(dest);

                        cxt.Mensajes.AddObject(mensaje);
                    }

                    item.Estado = EstadoAgente1214.Cancelado;
                }

                f1214.Estado = Estado1214.Anulado;
                cxt.SaveChanges();
                Session["Form214"] = null;
                Response.Redirect("~/Aplicativo/Formulario1214_Generados.aspx");
            }
        }

        protected void btn_Imprimir_Click(object sender, EventArgs e)
        {
            RenderReport();
        }

        private void RenderReport()
        {
            //Frente
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Form1214_r.rdlc");
            Reportes.Form1214_ds ds = GenerarDSF1214();

            ReportDataSource general = new ReportDataSource("F1214", ds.f214.Rows);
            ReportDataSource detalle = new ReportDataSource("Nomina", ds.Nomina.Rows);

            viewer.LocalReport.DataSources.Add(general);
            viewer.LocalReport.DataSources.Add(detalle);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;
            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Session["BytesFrente"] = bytes;


            //Reverso
            ReportViewer viewerreverso = new ReportViewer();
            viewerreverso.ProcessingMode = ProcessingMode.Local;
            viewerreverso.LocalReport.EnableExternalImages = true;
            viewerreverso.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Form1214_reverso_r.rdlc");

            ReportDataSource general_reverso = new ReportDataSource("General", ds.f214.Rows);

            viewerreverso.LocalReport.DataSources.Add(general_reverso);

            bytes = viewerreverso.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Session["BytesReverso"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF_combinado.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        private Reportes.Form1214_ds GenerarDSF1214()
        {
            var cxt = new Model1Container();
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            if (f1214.Id > 0)
            {
                f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
            }

            Reportes.Form1214_ds ret = new Reportes.Form1214_ds();

            Reportes.Form1214_ds.f214Row drGeneral = ret.f214.Newf214Row();

            drGeneral.Desde = f1214.Desde;
            drGeneral.Hasta = f1214.Hasta;
            drGeneral.Destino = f1214.Destino;
            drGeneral.Dias = ((f1214.Hasta - f1214.Desde).Days + 1);
            drGeneral.Anticipo = lbl_monto_anticipo.InnerText;
            drGeneral.MontoAnticipo = f1214.MontoAnticipo;
            drGeneral.Movilidad = f1214.Movilidad.ToString().Replace('_', ' ');
            drGeneral.Numero = Cadena.CompletarConCeros(6, f1214.Id);
            drGeneral.Tareas = f1214.TareasACumplir;

            ret.f214.Rows.Add(drGeneral);

            List<Agente1214> nomina = new List<Agente1214>();
            nomina.Add(f1214.Nomina.First(aa => aa.JefeComicion && aa.Estado == EstadoAgente1214.Aprobado));
            foreach (Agente1214 item in f1214.Nomina)
            {
                if (item.Estado == EstadoAgente1214.Aprobado && !item.JefeComicion)
                {
                    nomina.Add(item);
                }
            }

            for (int i = 1; i < 11; i++)
            {
                Reportes.Form1214_ds.NominaRow dr = ret.Nomina.NewNominaRow();
                dr.Numero214 = Cadena.CompletarConCeros(6, f1214.Id);
                dr.PosicionStr = Cadena.CompletarConCeros(2, i);
                dr.Posicion = i;

                if (nomina.Count() >= i)
                {
                    if (i == 1)
                    {
                        dr.Agente = "Jefe de comisión: " + nomina[i - 1].Agente.ApellidoYNombre;
                    }
                    else
                    {
                        dr.Agente = nomina[i - 1].Agente.ApellidoYNombre;
                    }
                }
                else
                {
                    dr.Agente = "/////////////////////////////";
                }

                ret.Nomina.Rows.Add(dr);
            }

            return ret;
        }

        #endregion

        #region Validaciones

        protected void cv_desde_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out desde) && desde >= DateTime.Today;
        }

        protected void cv_dentro_fuera_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = dentro_fuera.Value != "0";
        }

        protected void cv_fechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            DateTime hasta;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out desde) && DateTime.TryParse(tb_hasta.Value, out hasta) && desde <= hasta;
        }

        protected void cv_jefe_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool ret;
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;

            if (f1214.Id > 0)
            {
                using (var cxt = new Model1Container())
                {
                    f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
                    ret = f1214.Nomina.Count(aa => aa.JefeComicion) == 1;
                }
            }
            else
            {
                //EN ESTA INSTANCIA NO PUEDE ESTAR RECHAZADO, ESTA CONFECCIONANDO  EL FORM POR ESO PREGUNTO UNICAMENTE POR CANCELADO
                ret = f1214.Nomina.Count(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado) == 1;

            }

            args.IsValid = ret;
        }

        protected void cv_anticipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddl_movilidad.SelectedValue != "0";
        }

        protected void cv_monto_anticipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddl_movilidad.SelectedValue != "0")
            {
                decimal d;
                args.IsValid = decimal.TryParse(tb_monto_anticipo.Value, out d);
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cv_agentes_rechazados_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Formulario1214 f1214 = Session["Form214"] as Formulario1214;
            bool ret = false;
            if (f1214.Id > 0)
            {
                using (var cxt = new Model1Container())
                {
                    f1214 = cxt.Formularios1214.First(ff => ff.Id == f1214.Id);
                    ret = f1214.Nomina.Count(aa => aa.FechaRechazo != null && aa.Estado != EstadoAgente1214.Cancelado) == 0;
                }
            }
            else
            {
                ret = f1214.Nomina.Count(aa => aa.FechaRechazo != null && aa.Estado != EstadoAgente1214.Cancelado) == 0;
            }

            args.IsValid = ret;
        }

        #endregion

       
    }
}

public static class ControlExtensions
{
    /// <summary>
    /// recursively finds a child control of the specified parent.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Control FindControlRecursive(this Control control, string id)
    {
        if (control == null) return null;
        //try to find the control at the current level
        Control ctrl = control.FindControl(id);

        if (ctrl == null)
        {
            //search the children
            foreach (Control child in control.Controls)
            {
                ctrl = FindControlRecursive(child, id);

                if (ctrl != null) break;
            }
        }
        return ctrl;
    }
}
