using Microsoft.SqlServer.Server;
using OfficeOpenXml.ConditionalFormatting;
using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
                        MenuAgente.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    }

                    int id = Convert.ToInt32(idstr);

                    using (var cxt = new Model1Container())
                    {
                        HorarioVespertino hv = cxt.HorariosVespertinos.First(hvr => hvr.Id == id);
                        Session["HV"] = hv;
                        ResumenDiario rd = hv.Agente.ObtenerResumenDiario(hv.Dia);

                        /*
                        AL CARGAR LOS VALORES SI EN EL RD TENGO HVENT U HVSAL Y MARCACION MANUAL REGISTRADA EN ESE HORARIO EL AGENTE SELECCIONO REGISTRAR ENTRADA O SALIDA TOMANDO LA HORA DEL SERVIDOR.
                        DEBO CARGAR ESO COMO INGRESO, EL EGRESO SI ESTA OK LO TOMO COMO BUENO PARA TERMINAR EL HV SI NO (GENERA HORAS EN 00:00 O ALGUN OTRO ERROR) NO DEJO TOMAR ESE REGISTRO DIRECTAMENTE AL MOMENTO DEL REGISTRO

                        VER DE QUE SI REGISTRO ENTRADA Y SALIDA CORRECTAMENTE TERMINAR EL HV AUTOMATICAMENTE

                        */

                        CargarComboboxes();

                        btn_registrar_entrada_hv.Visible = false;
                        btn_registrar_salida_hv.Visible = false;

                        ddl_marcaciones_desde.Visible = false;
                        ddl_marcaciones_hasta.Visible = false;

                        btn_usar_marcacion_entrada.Visible = false;
                        btn_usar_marcacion_salida.Visible = false;

                        tb_entrada_manual.Visible = false;
                        btn_registrar_marcacion_entrada.Visible = false;

                        tb_salida_manual.Visible = false;
                        btn_registrar_marcacion_salida.Visible = false;

                        lbl_Agente.Text = hv.Agente.ApellidoYNombre;
                        lbl_Descripcion.Text = hv.Motivo;
                        lbl_Dia.Text = hv.Dia.ToLongDateString();
                        lbl_HoraDesde.Text = hv.HoraInicio;
                        lbl_HoraHasta.Text = hv.HoraFin;

                        btn_Rechazar.Visible = usuariologueado.Perfil == PerfilUsuario.Personal || (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                        btn_Terminar.Enabled = false;

                        if (rd != null && (rd.HVEnt != "00:00" && rd.HVEnt != "No hay registros." && rd.Marcaciones.Count(x => x.Hora == rd.HVEnt && !x.Anulada) > 0))
                        {
                            //registro entrada 
                            Registrar_Inicio_HV(rd.HVEnt);

                        }

                        if (rd != null && (rd.HVSal != "00:00" && rd.HVSal != "No hay registros." && rd.Marcaciones.Count(x => x.Hora == rd.HVSal && !x.Anulada) > 0))
                        {
                            //registro salida
                            Registrar_Fin_HV(rd.HVSal);
                        }
                    }
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

            //si los ddl no estan enabled es porque al cargar la pagina se detecto que esta seleccionada una marcacion de entrada o salida por lo que no se toca.-

            if (ddl_tipo_marcacion_desde.Enabled) ddl_tipo_marcacion_desde.Items.Add(new ListItem("Seleccione", "0"));
            if (ddl_tipo_marcacion_hasta.Enabled) ddl_tipo_marcacion_hasta.Items.Add(new ListItem("Seleccione", "0"));

            using (var cxt = new ClockCardEntities())
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

                Agente ag = hv.Agente;

                DateTime fechaSeleccionada = hv.Dia;

                //cargo los ddl de tipos de registracion de horas
                //todos deberian poder usar las marcaciones existentes
                if (ddl_tipo_marcacion_desde.Enabled) ddl_tipo_marcacion_desde.Items.Add(new ListItem("Marcaciones existentes", "1"));
                if (ddl_tipo_marcacion_hasta.Enabled) ddl_tipo_marcacion_hasta.Items.Add(new ListItem("Marcaciones existentes", "1"));

                //si es jefe o personal deberia poder agregar marcacion
                if (usuariologueado.EsJefeDe(ag.Id) || (usuariologueado.Perfil == PerfilUsuario.Personal && usuariologueado.Id != ag.Id))
                {
                    if (ddl_tipo_marcacion_desde.Enabled) ddl_tipo_marcacion_desde.Items.Add(new ListItem("Registrar marcación manual", "3"));
                    if (ddl_tipo_marcacion_hasta.Enabled) ddl_tipo_marcacion_hasta.Items.Add(new ListItem("Registrar marcación manual", "3"));
                }
                else
                {
                    //si no es jefe ni personal y esta de remoto o receptoria deberia poder registrar su entrada salida
                    if (ag.MarcaManual(fechaSeleccionada))
                    {
                        if (ddl_tipo_marcacion_desde.Enabled) ddl_tipo_marcacion_desde.Items.Add(new ListItem("Registrar marcación", "2"));
                        if (ddl_tipo_marcacion_hasta.Enabled) ddl_tipo_marcacion_hasta.Items.Add(new ListItem("Registrar marcación", "2"));
                    }
                }

                //cargo los ddl de horas
                var marcaciones = ObtenerMarcacionesResumenDiario(hv.Agente.Id, hv.Dia);

                if (marcaciones.Count() == 0)
                {
                    if (ddl_marcaciones_desde.Enabled) ddl_marcaciones_desde.Items.Add(new ListItem("No hay registros.", "0"));
                    if (ddl_marcaciones_hasta.Enabled) ddl_marcaciones_hasta.Items.Add(new ListItem("No hay registros.", "0"));
                }
                else
                {
                    if (ddl_marcaciones_desde.Enabled) ddl_marcaciones_desde.Items.Add(new ListItem("Seleccionar", "0"));
                    if (ddl_marcaciones_hasta.Enabled) ddl_marcaciones_hasta.Items.Add(new ListItem("Seleccionar", "0"));
                    foreach (var marcacion in marcaciones)
                    {
                        if (ddl_marcaciones_desde.Enabled) ddl_marcaciones_desde.Items.Add(new ListItem() { Text = marcacion.Hora, Value = marcacion.Hora });
                        if (ddl_marcaciones_hasta.Enabled) ddl_marcaciones_hasta.Items.Add(new ListItem() { Text = marcacion.Hora, Value = marcacion.Hora });
                    }
                }
            }
        }
        private List<Marcacion> ObtenerMarcacionesResumenDiario(int idAgente, DateTime dia)
        {
            List<Marcacion> ret = new List<Marcacion>();
            using (var cxt = new Model1Container())
            {
                ResumenDiario rdCxt = cxt.ResumenesDiarios.Include("Marcaciones").Include("Agente").FirstOrDefault(r => r.AgenteId == idAgente && r.Dia == dia);

                using (var cxtClock = new ClockCardEntities())
                {

                    Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == idAgente);
                    var marcaciones = (from h in cxtClock.FICHADA
                                       where h.FIC_FECHA == dia &&
                                       (h.LEG_LEGAJO == ag.Legajo)
                                       select new
                                       {
                                           Legajo = h.LEG_LEGAJO,
                                           Fecha = h.FIC_FECHA,
                                           Hora = h.FIC_HORA
                                       }).ToList();


                    if (rdCxt == null)
                    {
                        rdCxt = new ResumenDiario();

                        rdCxt.AgenteId = idAgente;
                        rdCxt.Dia = dia;
                        rdCxt.HEntrada = "00:00";
                        rdCxt.HSalida = "00:00";
                        rdCxt.HVEnt = "00:00";
                        rdCxt.HVSal = "00:00";
                        rdCxt.Horas = "00:00";
                        rdCxt.Inconsistente = false;
                        rdCxt.MarcoTardanza = false;
                        rdCxt.MarcoProlongJornada = false;
                        rdCxt.ObservacionInconsistente = "";

                        cxt.SaveChanges();
                    }

                    foreach (var marcacion in marcaciones)
                    {
                        if (rdCxt.Marcaciones.FirstOrDefault(x => x.Hora == marcacion.Hora) == null)
                        {
                            rdCxt.Marcaciones.Add(new Marcacion()
                            {
                                Hora = marcacion.Hora
                                                        ,
                                Anulada = false
                                                        ,
                                Manual = false
                            });
                        }
                    }

                    cxt.SaveChanges();

                    ret = rdCxt.Marcaciones.ToList();
                }
            }

            return ret;
        }

        #region Tomar entrada salida dependiendo del tipo de seleccion 

        //selecciona el tipo de ingreso de la marcación
        protected void ddl_tipo_marcacion_desde_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddl_tipo_marcacion_desde.SelectedValue)
            {
                case "0"://Seleccione
                    ddl_marcaciones_desde.Visible = false;
                    btn_usar_marcacion_entrada.Visible = false;
                    btn_registrar_entrada_hv.Visible = false;
                    tb_entrada_manual.Visible = false;
                    btn_registrar_marcacion_entrada.Visible = false;
                    leyenda_marcaciones_faciales_entrada.Text = "";
                    break;
                case "1"://Marcaciones existentes
                    ddl_marcaciones_desde.Visible = true;
                    btn_usar_marcacion_entrada.Visible = true;
                    btn_registrar_entrada_hv.Visible = false;
                    tb_entrada_manual.Visible = false;
                    btn_registrar_marcacion_entrada.Visible = false;
                    leyenda_marcaciones_faciales_entrada.Text = "Se muestran las marcaciones descargadas por Personal de los relojes faciales";
                    break;
                case "2"://Registrar marcación (agente)
                    ddl_marcaciones_desde.Visible = false;
                    btn_usar_marcacion_entrada.Visible = false;
                    btn_registrar_entrada_hv.Visible = true;
                    tb_entrada_manual.Visible = false;
                    btn_registrar_marcacion_entrada.Visible = false;
                    leyenda_marcaciones_faciales_entrada.Text = "";
                    break;
                case "3"://Registrar marcación manual (jefe)
                    ddl_marcaciones_desde.Visible = false;
                    btn_usar_marcacion_entrada.Visible = false;
                    btn_registrar_entrada_hv.Visible = false;
                    tb_entrada_manual.Visible = true;
                    btn_registrar_marcacion_entrada.Visible = true;
                    leyenda_marcaciones_faciales_entrada.Text = "";
                    break;
                default:
                    break;
            }
        }

        protected void ddl_tipo_marcacion_hasta_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddl_tipo_marcacion_hasta.SelectedValue)
            {
                case "0"://Seleccione
                    ddl_marcaciones_hasta.Visible = false;
                    btn_usar_marcacion_salida.Visible = false;
                    btn_registrar_salida_hv.Visible = false;
                    tb_salida_manual.Visible = false;
                    btn_registrar_marcacion_salida.Visible = false;
                    leyenda_marcaciones_faciales_salida.Text = "";
                    break;
                case "1"://Marcaciones existentes
                    ddl_marcaciones_hasta.Visible = true;
                    btn_usar_marcacion_salida.Visible = true;
                    btn_registrar_salida_hv.Visible = false;
                    tb_salida_manual.Visible = false;
                    btn_registrar_marcacion_salida.Visible = false;
                    leyenda_marcaciones_faciales_salida.Text = "Se muestran las marcaciones descargadas por Personal de los relojes faciales";
                    break;
                case "2"://Registrar marcación (agente)
                    ddl_marcaciones_hasta.Visible = false;
                    btn_usar_marcacion_salida.Visible = false;
                    btn_registrar_salida_hv.Visible = true;
                    tb_salida_manual.Visible = false;
                    btn_registrar_marcacion_salida.Visible = false;
                    leyenda_marcaciones_faciales_salida.Text = "";
                    break;
                case "3"://Registrar marcación manual (jefe)
                    ddl_marcaciones_hasta.Visible = false;
                    btn_usar_marcacion_salida.Visible = false;
                    btn_registrar_salida_hv.Visible = false;
                    tb_salida_manual.Visible = true;
                    btn_registrar_marcacion_salida.Visible = true;
                    leyenda_marcaciones_faciales_salida.Text = "";
                    break;
                default:
                    break;
            }
        }

        //registra marcacion de ingreso o egreso como jefe, llama a las validaciones pertinentes
        protected void btn_registrar_marcacion_entrada_Click(object sender, EventArgs e)
        {
            Page.Validate("HVdesde");
            if (Page.IsValid)
            {
                RegistrarMarcacionIngresoHV(tb_entrada_manual.Text, true);
                Registrar_Inicio_HV(tb_entrada_manual.Text);
            }
        }

        protected void btn_registrar_marcacion_salida_Click(object sender, EventArgs e)
        {
            Page.Validate("HVhasta");
            if (Page.IsValid)
            {
                string h_fin = tb_salida_manual.Text;
                string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;

                if (h_inicio != String.Empty && !(HorasString.RestarHoras(h_fin, h_inicio).Contains("-") || HorasString.RestarHoras(h_fin, h_inicio) == "00:00"))
                {
                    RegistrarMarcacionEgresoHV(h_fin, true);
                    Registrar_Fin_HV(h_fin);
                }
                else
                {
                    if (h_inicio == String.Empty)
                    {
                        MessageBox.Show(this, "Primero debe registrar el horario de inicio del horario vespertino", MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        MessageBox.Show(this, "La hora final debe ser mayor o igual a la hora inicial", MessageBox.Tipo_MessageBox.Danger);
                    }
                }

            }
        }

        //toma como marcación de entrada salida alguna de las disponibles
        protected void btn_usar_marcacion_entrada_Click(object sender, EventArgs e)
        {
            if (ddl_marcaciones_desde.SelectedValue != "0")
            {
                string h_inicio = ddl_marcaciones_desde.SelectedItem.Text;
                RegistrarMarcacionIngresoHV(h_inicio, false);
                Registrar_Inicio_HV(h_inicio);
            }
            else
            {
                MessageBox.Show(this, "Debe seleccionar una marcación de entrada", MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_usar_marcacion_salida_Click(object sender, EventArgs e)
        {
            string h_fin = ddl_marcaciones_hasta.SelectedItem.Text;
            string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;

            if (ddl_marcaciones_hasta.SelectedValue != "0")
            {
                if (h_inicio != String.Empty && !(HorasString.RestarHoras(h_fin, h_inicio).Contains("-") || HorasString.RestarHoras(h_fin, h_inicio) == "00:00"))
                {
                    RegistrarMarcacionEgresoHV(h_fin, false);
                    Registrar_Fin_HV(h_fin);
                }
                else
                {
                    if (h_inicio == String.Empty)
                    {
                        MessageBox.Show(this, "Primero debe registrar el horario de inicio del horario vespertino", MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        MessageBox.Show(this, "La hora de salida debe ser mayor o igual a la hora inicial", MessageBox.Tipo_MessageBox.Danger);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Debe seleccionar una marcación de salida", MessageBox.Tipo_MessageBox.Warning);
            }
        }

        //Registra la marcación del servidor para los agentes que pueden registrar marcaciones (remoto - receptorias)
        protected void btn_registrar_entrada_hv_Click(object sender, EventArgs e)
        {
            //Se toma la hora del servidor, este registro lo hace el agente
            HorarioVespertino hv = Session["HV"] as HorarioVespertino;
            if (hv.Dia == DateTime.Today)
            {
                string hora = DateTime.Now.ToString("HH:mm");

                RegistrarMarcacionIngresoHV(hora, true);
                Registrar_Inicio_HV(hora);
            }
            else
            {
                MessageBox.Show(this, "No se puede registrar entrada. Fecha de solicitud distinta a la fecha actual.-", MessageBox.Tipo_MessageBox.Danger);
            }
        }

        protected void btn_registrar_salida_hv_Click(object sender, EventArgs e)
        {
            //Se toma la hora del servidor, este registro lo hace el agente
            HorarioVespertino hv = Session["HV"] as HorarioVespertino;

            if (hv.Dia == DateTime.Today)
            {
                string hora = DateTime.Now.ToString("HH:mm");


                string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;

                if (h_inicio != String.Empty && !(HorasString.RestarHoras(hora, h_inicio).Contains("-") || HorasString.RestarHoras(hora, h_inicio) == "00:00"))
                {
                    RegistrarMarcacionEgresoHV(hora, true);
                    Registrar_Fin_HV(hora);
                }
                else
                {
                    if (h_inicio == String.Empty)
                    {
                        MessageBox.Show(this, "Primero debe registrar el horario de inicio del horario vespertino", MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        MessageBox.Show(this, "La hora final debe ser mayor o igual a la hora inicial", MessageBox.Tipo_MessageBox.Danger);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "No se puede registrar salida. Fecha de solicitud distinta a la fecha actual.-", MessageBox.Tipo_MessageBox.Danger);
            }
        }

        #endregion

        #region Registrar entrada salida 

        /// <summary>
        /// Registra la marcación, si existe no hace nada, de otra manera la crea y setea el campo HVEnt con esa hora
        /// </summary>
        /// <returns></returns>
        public bool RegistrarMarcacionIngresoHV(string hora, bool manual)
        {
            bool ret = true;
            try
            {
                HorarioVespertino hv = Session["HV"] as HorarioVespertino;
                ResumenDiario rd = hv.Agente.ObtenerResumenDiario(hv.Dia);

                using (var cxt = new Model1Container())
                {
                    if (rd != null)
                    {
                        ResumenDiario rdCxt = cxt.ResumenesDiarios.Include("Marcaciones").First(r => r.Id == rd.Id);

                        rdCxt.HVEnt = hora;

                        if (rdCxt.Marcaciones.FirstOrDefault(x => x.Hora == hora && !x.Anulada) == null)
                        {
                            rdCxt.Marcaciones.Add(new Marcacion() { Manual = manual, Hora = hora, Anulada = false });
                        }

                        cxt.SaveChanges();
                    }
                    else
                    {
                        rd = new ResumenDiario();

                        rd.AgenteId = hv.Agente.Id;
                        rd.Dia = hv.Dia;
                        rd.HEntrada = "00:00";
                        rd.HSalida = "00:00";
                        rd.HVEnt = hora;
                        rd.HVSal = "00:00";
                        rd.Horas = "00:00";
                        rd.Inconsistente = false;
                        rd.MarcoTardanza = false;
                        rd.MarcoProlongJornada = false;
                        rd.ObservacionInconsistente = "";

                        rd.Marcaciones.Add(new Marcacion() { Manual = manual, Hora = hora, Anulada = false });

                        cxt.ResumenesDiarios.AddObject(rd);

                        cxt.SaveChanges();
                    }
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Registra la marcación, si existe no hace nada, de otra manera la crea y setea el campo HVSal con esa hora
        /// </summary>
        /// <returns></returns>
        public bool RegistrarMarcacionEgresoHV(string hora, bool manual)
        {
            bool ret = true;
            try
            {
                HorarioVespertino hv = Session["HV"] as HorarioVespertino;
                ResumenDiario rd = hv.Agente.ObtenerResumenDiario(hv.Dia);

                using (var cxt = new Model1Container())
                {
                    if (rd != null)
                    {
                        ResumenDiario rdCxt = cxt.ResumenesDiarios.Include("Marcaciones").First(r => r.Id == rd.Id);

                        rdCxt.HVSal = hora;

                        if (rdCxt.Marcaciones.FirstOrDefault(x => x.Hora == hora && !x.Anulada) == null)
                        {
                            rdCxt.Marcaciones.Add(new Marcacion() { Manual = manual, Hora = hora, Anulada = false });
                        }

                        cxt.SaveChanges();
                    }
                    else
                    {
                        rd = new ResumenDiario();

                        rd.AgenteId = hv.Agente.Id;
                        rd.Dia = hv.Dia;
                        rd.HEntrada = "00:00";
                        rd.HSalida = "00:00";
                        rd.HVEnt = "00:00";
                        rd.HVSal = hora;
                        rd.Horas = "00:00";
                        rd.Inconsistente = false;
                        rd.MarcoTardanza = false;
                        rd.MarcoProlongJornada = false;
                        rd.ObservacionInconsistente = "";

                        rd.Marcaciones.Add(new Marcacion() { Manual = manual, Hora = hora, Anulada = false });

                        cxt.ResumenesDiarios.AddObject(rd);

                        cxt.SaveChanges();
                    }
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Registra en los valores de session el valor correspondiente al ingreso del HV
        /// </summary>
        /// <param name="h_inicio">marcacion guardada</param>
        private void Registrar_Inicio_HV(string h_inicio)
        {
            Session["H_Ingreso_seleccionada"] = h_inicio;

            ddl_tipo_marcacion_desde.Items.Clear();
            ddl_marcaciones_desde.Items.Clear();

            ddl_tipo_marcacion_desde.Items.Add(new ListItem("Marcación registrada", "1"));
            ddl_tipo_marcacion_desde.Enabled = false;

            ddl_marcaciones_desde.Items.Add(new ListItem(h_inicio, "1"));
            ddl_marcaciones_desde.Enabled = false;
            ddl_marcaciones_desde.Visible = true;

            HorarioVespertino hv = Session["HV"] as HorarioVespertino;
            if (HorasString.AMayorQueB(h_inicio, hv.HoraInicio))
            {
                lbl_impacto_hv_entrada.Text = String.Format("Hora registrada: {0}. Hora solicitada: {1}. Se asume el ingreso a partir de la hora registrada {0}", h_inicio, hv.HoraInicio);
                Session["HV_Inicio"] = h_inicio;
            }
            else
            {
                lbl_impacto_hv_entrada.Text = String.Format("Hora registrada: {0}. Hora solicitada: {1}. Se asume el ingreso a partir de la hora solicitada {1}", h_inicio, hv.HoraInicio);
                Session["HV_Inicio"] = hv.HoraInicio;
            }

            ActualizarTitulosResumen();
        }

        /// <summary>
        /// Registra en los valores de session el valor correspondiente al egreso del HV
        /// </summary>
        /// <param name="h_salida">marcacion guardada</param>
        private void Registrar_Fin_HV(string h_salida)
        {
            Session["H_Salida_seleccionada"] = h_salida;
            HorarioVespertino hv = Session["HV"] as HorarioVespertino;
            string hora_salida_HV_con_changui = HorasString.SumarHoras(new string[] { hv.HoraFin, "00:30" });

            ddl_tipo_marcacion_hasta.Items.Clear();
            ddl_marcaciones_hasta.Items.Clear();

            ddl_tipo_marcacion_hasta.Items.Add(new ListItem("Marcación registrada", "1"));
            ddl_tipo_marcacion_hasta.Enabled = false;

            ddl_marcaciones_hasta.Items.Add(new ListItem(h_salida, "1"));
            ddl_marcaciones_hasta.Enabled = false;
            ddl_marcaciones_hasta.Visible = true;

            if (HorasString.AMayorQueB(hora_salida_HV_con_changui, h_salida))
            {
                if (HorasString.AMayorQueB(hv.HoraInicio, h_salida))
                {
                    lbl_impacto_hv_salida.Text = String.Format("Hora registrada: {0}. Hora solicitada (con margen en más de hasta media hora): {1}. Se asume como hora de salida la hora de ingreso solicitada {2}", h_salida, hora_salida_HV_con_changui, hv.HoraInicio);
                    Session["HV_Salida"] = hv.HoraInicio;
                }
                else
                {
                    lbl_impacto_hv_salida.Text = String.Format("Hora registrada: {0}. Hora solicitada (con margen en más de hasta media hora): {1}. Se asume como hora de salida la hora registrada {0}", h_salida, hora_salida_HV_con_changui);
                    Session["HV_Salida"] = h_salida;
                }
            }
            else
            {
                lbl_impacto_hv_salida.Text = String.Format("Hora registrada: {0}. Hora solicitada (con margen en más de hasta media hora): {1}. Se asume como hora de salida la hora registrada {1}", h_salida, hora_salida_HV_con_changui);
                Session["HV_Salida"] = hora_salida_HV_con_changui;
            }

            ActualizarTitulosResumen();
        }

        private void ActualizarTitulosResumen()
        {
            string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;
            string h_fin = Session["HV_Salida"] != null ? Session["HV_Salida"].ToString() : String.Empty;

            Page.Validate("HVHastaGeneral");
            btn_Terminar.Enabled = Page.IsValid;

            if (h_inicio != String.Empty && h_fin != String.Empty)
            {
                lbl_ingreso_registrado.Text = h_inicio;
                lbl_egreso_registrado.Text = h_fin;
                string horas_totales_hv = HorasString.RestarHoras(h_fin, h_inicio);

                if (horas_totales_hv.Contains("-") || horas_totales_hv == "00:00")
                {
                    lbl_resumen_horas_a_aplicar.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lbl_resumen_horas_a_aplicar.ForeColor = System.Drawing.Color.Black;
                }

                lbl_resumen_horas_a_aplicar.Text = String.Format(" - Horas realizadas: {0}", horas_totales_hv);
            }
            else
            {
                if (h_inicio != String.Empty)
                {
                    lbl_ingreso_registrado.Text = h_inicio;
                    lbl_resumen_horas_a_aplicar.Text = String.Format(" - Horas realizadas: {0}", "NE");
                }

                if (h_fin != String.Empty)
                {
                    lbl_egreso_registrado.Text = h_fin;
                    lbl_resumen_horas_a_aplicar.Text = String.Format(" - Horas realizadas: {0}", "NE");
                }
            }
        }

        #endregion

        #region Botones para terminar, rechazar o volver
        protected void btn_Terminar_Click(object sender, EventArgs e)
        {
            Page.Validate("HVHastaGeneral");
            if (Page.IsValid)
            {
                int id = (Session["HV"] as HorarioVespertino).Id;
                string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;
                string h_fin = Session["HV_Salida"] != null ? Session["HV_Salida"].ToString() : String.Empty;

                Model1Container cxt = new Model1Container();
                HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hvs => hvs.Id == id);
                if (hv.Estado != EstadosHorarioVespertino.Terminado)
                {
                    hv.HoraInicio = h_inicio;
                    hv.HoraFin = h_fin;
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
                        if (!(usuariologueado.Jefe || usuariologueado.JefeTemporal))
                        {
                            Response.Redirect("~/Aplicativo/MainAgente.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/Aplicativo/MainJefe.aspx");
                        }

                    }
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
                if (usuariologueado.Jefe || usuariologueado.JefeTemporal)
                {
                    Response.Redirect("~/Aplicativo/MainJefe.aspx");
                }
                else
                {
                    Response.Redirect("~/Aplicativo/MainAgente.aspx");
                }



            }
        }

        #endregion

        #region Validaciones sobre los textboxes de ingreso manual como Jefes o Personal
        protected void CustomFieldValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_entrada_manual.Text.Length > 0;
        }


        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tb_salida_manual.Text.Length > 0;
        }

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                Regex rex = new Regex("(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]");
                Match match = rex.Match(tb_entrada_manual.Text);
                args.IsValid = match.Success;
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                Regex rex = new Regex("(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]");
                Match match = rex.Match(tb_salida_manual.Text);
                args.IsValid = match.Success;
            }
            catch
            {
                args.IsValid = false;
            }
        }

        #endregion

        #region Validación de horas, ingreso - egreso

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                string h_inicio = Session["HV_Inicio"] != null ? Session["HV_Inicio"].ToString() : String.Empty;
                string h_fin = Session["HV_Salida"] != null ? Session["HV_Salida"].ToString() : String.Empty;


                args.IsValid = !(HorasString.RestarHoras(h_fin, h_inicio).Contains("-") || HorasString.RestarHoras(h_fin, h_inicio) == "00:00");
            }
            catch
            {
                //los formatos de horas no corresponden por lo tanto deberia de saltar por el otro customValidator 
                args.IsValid = false;
            }
        }

        #endregion

    }
}