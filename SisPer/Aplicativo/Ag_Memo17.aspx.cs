using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisPer.Aplicativo.Reportes;

namespace SisPer.Aplicativo
{
    public partial class Ag_Memo17 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                Memo_17_DDJJ memo = cxt.Memo_17_DDJJs.FirstOrDefault(x => x.AgenteId == ag.Id);
                if (memo == null)
                {
                    memo = new Memo_17_DDJJ();
                    memo.AgenteId = ag.Id;
                }

                Session["Memo17"] = memo;

                Mostrar_valores_memo();
            }
        }

        private void Mostrar_valores_memo()
        {
            Memo_17_DDJJ memo = Session["Memo17"] as Memo_17_DDJJ;

            #region Datos personales
            tb_dp_apellido_y_nombre.Text = memo.dp_apellido_y_nombre;
            tb_dp_nacionalidad.Text = memo.dp_nacionalidad;
            ddl_dp_nativo_naturalizado.SelectedValue = memo.dp_nativo_naturalizado;
            tb_dp_nacimiento_lugar.Text = memo.dp_nacimiento_lugar;
            tb_dp_nacimiento_fecha.Value = memo.dp_nacimiento_fecha.ToString("dd/MM/yyyy");
            tb_dp_clase.Text = memo.dp_clase;
            tb_dp_dni.Text = memo.dp_dni;
            tb_dp_estado_civil.Text = memo.dp_estado_civil;
            tb_dp_domicilio_direccion.Text = memo.dp_domicilio_direccion;
            tb_dp_domicilio_barrio.Text = memo.dp_domicilio_barrio;
            tb_dp_domicilio_localidad.Text = memo.dp_domicilio_localidad;
            tb_dp_domicilio_provincia.Text = memo.dp_domicilio_provincia;
            tb_dp_domicilio_codpost.Text = memo.dp_domicilio_codpost;
            tb_dp_profesion.Text = memo.dp_profesion;
            tb_dp_instruccion.Text = memo.dp_instruccion;
            tb_dp_conyugue_apellido_y_nombre.Text = memo.dp_conyugue_apellido_y_nombre;
            tb_dp_conyugue_nacionalidad.Text = memo.dp_conyugue_nacionalidad;
            tb_dp_padre_apellido_y_nombre.Text = memo.dp_padre_apellido_y_nombre;
            tb_dp_padre_nacionalidad.Text = memo.dp_padre_nacionalidad;
            ddl_dp_padre_vive.SelectedValue = memo.dp_padre_vive ? "Si" : "No";
            tb_dp_madre_apellido_y_nombre.Text = memo.dp_madre_apellido_y_nombre;
            tb_dp_madre_nacionalidad.Text = memo.dp_madre_nacionalidad;
            ddl_dp_madre_vive.SelectedValue = memo.dp_madre_vive ? "Si" : "No";

            gv_hijos.DataSource = memo.Memo_17_DDJJ_Hijos;
            gv_hijos.DataBind();

            #endregion

            #region Antecedentes administrativos

            tb_aa_cargo.Text = memo.aa_cargo;
            tb_aa_funcion.Text = memo.aa_funcion;
            tb_aa_contrato_obra_ingreso.Value = memo.aa_contrato_obra_ingreso.ToString("dd/MM/yyyy");
            tb_aa_contrato_serv_ingreso.Value = memo.aa_contrato_serv_ingreso.ToString("dd/MM/yyyy");
            tb_aa_nombramiento_ingreso.Value = memo.aa_nombramiento_ingreso.ToString("dd/MM/yyyy");
            tb_aa_contrato_obra_instrumento.Text = memo.aa_contrato_obra_instrumento;
            tb_aa_contrato_serv_instrumento.Text = memo.aa_contrato_serv_instrumento;
            tb_aa_nombramiento_instrumento.Text = memo.aa_nombramiento_instrumento;

            #endregion

            #region Cargos desempeñados anteriormente

            tb_ca_en_admin_nacional.Text = memo.ca_en_admin_nacional;
            tb_ca_en_admin_provincial.Text = memo.ca_en_admin_provincial;
            tb_ca_privados.Text = memo.ca_privados;

            #endregion

            #region Otros antecedentes

            tb_ca_otros_antecedentes.Text = memo.ca_otros_antecedentes;

            #endregion

        }

        private void MostrarPopUp(string id_pop_up)
        {
            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#" + id_pop_up + "').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "ShowPopUp", script, false);
        }

        /// <summary>
        /// Guarda los valores en la variable de session sin persistirlos en la base de datos
        /// </summary>
        private void Guardar_valores_memo_session()
        {
            Memo_17_DDJJ memo = Session["Memo17"] as Memo_17_DDJJ;

            #region Datos Personales

            memo.dp_apellido_y_nombre = tb_dp_apellido_y_nombre.Text;
            memo.dp_nacionalidad = tb_dp_nacionalidad.Text;
            memo.dp_nativo_naturalizado = ddl_dp_nativo_naturalizado.SelectedValue;
            memo.dp_nacimiento_lugar = tb_dp_nacimiento_lugar.Text;
            memo.dp_nacimiento_fecha = Convert.ToDateTime(tb_dp_nacimiento_fecha.Value);
            memo.dp_clase = tb_dp_clase.Text;
            memo.dp_dni = tb_dp_dni.Text;
            memo.dp_estado_civil = tb_dp_estado_civil.Text;
            memo.dp_domicilio_direccion = tb_dp_domicilio_direccion.Text;
            memo.dp_domicilio_barrio = tb_dp_domicilio_barrio.Text;
            memo.dp_domicilio_localidad = tb_dp_domicilio_localidad.Text;
            memo.dp_domicilio_provincia = tb_dp_domicilio_provincia.Text;
            memo.dp_domicilio_codpost = tb_dp_domicilio_codpost.Text;
            memo.dp_profesion = tb_dp_profesion.Text;
            memo.dp_instruccion = tb_dp_instruccion.Text;
            memo.dp_conyugue_apellido_y_nombre = tb_dp_conyugue_apellido_y_nombre.Text;
            memo.dp_conyugue_nacionalidad = tb_dp_conyugue_nacionalidad.Text;
            memo.dp_padre_apellido_y_nombre = tb_dp_padre_apellido_y_nombre.Text;
            memo.dp_padre_nacionalidad = tb_dp_padre_nacionalidad.Text;
            memo.dp_padre_vive = ddl_dp_padre_vive.SelectedValue == "Si" ? true : false;
            memo.dp_madre_apellido_y_nombre = tb_dp_madre_apellido_y_nombre.Text;
            memo.dp_madre_nacionalidad = tb_dp_madre_nacionalidad.Text;
            memo.dp_madre_vive = ddl_dp_madre_vive.SelectedValue == "Si" ? true : false;

            #endregion

            #region Antecedentes administrativos

            memo.aa_cargo = tb_aa_cargo.Text;
            memo.aa_funcion = tb_aa_funcion.Text;
            memo.aa_contrato_obra_ingreso = Convert.ToDateTime(tb_aa_contrato_obra_ingreso.Value);
            memo.aa_contrato_obra_instrumento = tb_aa_contrato_obra_instrumento.Text;
            memo.aa_contrato_serv_ingreso = Convert.ToDateTime(tb_aa_contrato_serv_ingreso.Value);
            memo.aa_contrato_serv_instrumento = tb_aa_contrato_serv_instrumento.Text;
            memo.aa_nombramiento_ingreso = Convert.ToDateTime(tb_aa_nombramiento_ingreso.Value);
            memo.aa_nombramiento_instrumento = tb_aa_nombramiento_instrumento.Text;

            #endregion

            #region Cargos desempeñados anteriormente

            memo.ca_en_admin_nacional = tb_ca_en_admin_nacional.Text;
            memo.ca_en_admin_provincial = tb_ca_en_admin_provincial.Text;
            memo.ca_privados = tb_ca_privados.Text;

            #endregion

            #region Otros antecedentes

            memo.ca_otros_antecedentes = tb_ca_otros_antecedentes.Text;

            #endregion

            Session["Memo17"] = memo;
            Mostrar_valores_memo();
        }

        private bool Guardar_valores_memo()
        {
            bool ret = true;
            using (var cxt = new Model1Container())
            {
                #region Obtener y crear el memo

                Memo_17_DDJJ memo_session = Session["Memo17"] as Memo_17_DDJJ;
                Memo_17_DDJJ memo_cxt = null;

                if (memo_session.Id > 0)
                {
                    memo_cxt = cxt.Memo_17_DDJJs.FirstOrDefault(x => x.Id == memo_session.Id);
                }

                if (memo_cxt == null)
                {
                    memo_cxt = new Memo_17_DDJJ();
                    memo_cxt.AgenteId = memo_session.AgenteId;
                    cxt.Memo_17_DDJJs.AddObject(memo_cxt);
                }

                memo_cxt.fecha_modificacion = DateTime.Now;

                #endregion

                try
                {
                    #region Datos Personales

                    memo_cxt.dp_apellido_y_nombre = tb_dp_apellido_y_nombre.Text;
                    memo_cxt.dp_nacionalidad = tb_dp_nacionalidad.Text;
                    memo_cxt.dp_nativo_naturalizado = ddl_dp_nativo_naturalizado.SelectedValue;
                    memo_cxt.dp_nacimiento_lugar = tb_dp_nacimiento_lugar.Text;
                    memo_cxt.dp_nacimiento_fecha = Convert.ToDateTime(tb_dp_nacimiento_fecha.Value) == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1800") : Convert.ToDateTime(tb_dp_nacimiento_fecha.Value);
                    memo_cxt.dp_clase = tb_dp_clase.Text;
                    memo_cxt.dp_dni = tb_dp_dni.Text;
                    memo_cxt.dp_estado_civil = tb_dp_estado_civil.Text;
                    memo_cxt.dp_domicilio_direccion = tb_dp_domicilio_direccion.Text;
                    memo_cxt.dp_domicilio_barrio = tb_dp_domicilio_barrio.Text;
                    memo_cxt.dp_domicilio_localidad = tb_dp_domicilio_localidad.Text;
                    memo_cxt.dp_domicilio_provincia = tb_dp_domicilio_provincia.Text;
                    memo_cxt.dp_domicilio_codpost = tb_dp_domicilio_codpost.Text;
                    memo_cxt.dp_profesion = tb_dp_profesion.Text;
                    memo_cxt.dp_instruccion = tb_dp_instruccion.Text;
                    memo_cxt.dp_conyugue_apellido_y_nombre = tb_dp_conyugue_apellido_y_nombre.Text;
                    memo_cxt.dp_conyugue_nacionalidad = tb_dp_conyugue_nacionalidad.Text;
                    memo_cxt.dp_padre_apellido_y_nombre = tb_dp_padre_apellido_y_nombre.Text;
                    memo_cxt.dp_padre_nacionalidad = tb_dp_padre_nacionalidad.Text;
                    memo_cxt.dp_padre_vive = ddl_dp_padre_vive.SelectedValue == "Si" ? true : false;
                    memo_cxt.dp_madre_apellido_y_nombre = tb_dp_madre_apellido_y_nombre.Text;
                    memo_cxt.dp_madre_nacionalidad = tb_dp_madre_nacionalidad.Text;
                    memo_cxt.dp_madre_vive = ddl_dp_madre_vive.SelectedValue == "Si" ? true : false;

                    #endregion

                    #region Antecedentes administrativos

                    memo_cxt.aa_cargo = tb_aa_cargo.Text;
                    memo_cxt.aa_funcion = tb_aa_funcion.Text;
                    memo_cxt.aa_contrato_obra_ingreso = Convert.ToDateTime(tb_aa_contrato_obra_ingreso.Value) == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1800") : Convert.ToDateTime(tb_aa_contrato_obra_ingreso.Value);
                    memo_cxt.aa_contrato_obra_instrumento = tb_aa_contrato_obra_instrumento.Text;
                    memo_cxt.aa_contrato_serv_ingreso = Convert.ToDateTime(tb_aa_contrato_serv_ingreso.Value) == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1800") : Convert.ToDateTime(tb_aa_contrato_serv_ingreso.Value);
                    memo_cxt.aa_contrato_serv_instrumento = tb_aa_contrato_serv_instrumento.Text;
                    memo_cxt.aa_nombramiento_ingreso = Convert.ToDateTime(tb_aa_nombramiento_ingreso.Value) == Convert.ToDateTime("01/01/0001") ? Convert.ToDateTime("01/01/1800") : Convert.ToDateTime(tb_aa_nombramiento_ingreso.Value);
                    memo_cxt.aa_nombramiento_instrumento = tb_aa_nombramiento_instrumento.Text;

                    #endregion

                    #region Cargos desempeñados anteriormente

                    memo_cxt.ca_en_admin_nacional = tb_ca_en_admin_nacional.Text;
                    memo_cxt.ca_en_admin_provincial = tb_ca_en_admin_provincial.Text;
                    memo_cxt.ca_privados = tb_ca_privados.Text;

                    #endregion

                    #region Otros antecedentes

                    memo_cxt.ca_otros_antecedentes = tb_ca_otros_antecedentes.Text;

                    #endregion

                    #region Hijos

                    int hijos = memo_cxt.Memo_17_DDJJ_Hijos.Count();

                    for (int i = 0; i <= hijos - 1; i++)
                    {
                        Memo_17_DDJJ_Hijo hijo = memo_cxt.Memo_17_DDJJ_Hijos.First();
                        cxt.Memo_17_DDJJ_Hijos.DeleteObject(hijo);
                    }

                    cxt.SaveChanges();

                    foreach (Memo_17_DDJJ_Hijo hijo in memo_session.Memo_17_DDJJ_Hijos)
                    {
                        memo_cxt.Memo_17_DDJJ_Hijos.Add(
                            new Memo_17_DDJJ_Hijo()
                            {
                                dp_hijo_apellido_y_nombre = hijo.dp_hijo_apellido_y_nombre,
                                dp_hijo_fecha_nacimiento = hijo.dp_hijo_fecha_nacimiento
                            });
                    }

                    #endregion  

                    cxt.SaveChanges();
                }
                catch (Exception ex)
                {
                    ret = false;
                }
            }
            return ret;
        }

        protected void btn_eliminar_hijo_Click(object sender, ImageClickEventArgs e)
        {
            Guardar_valores_memo_session();
            Memo_17_DDJJ memo = Session["Memo17"] as Memo_17_DDJJ;
            string nombre = ((ImageButton)sender).CommandArgument;
            Memo_17_DDJJ_Hijo memo_hijo = memo.Memo_17_DDJJ_Hijos.First(aa => aa.dp_hijo_apellido_y_nombre == nombre);
            if (memo_hijo != null)
            {
                memo.Memo_17_DDJJ_Hijos.Remove(memo_hijo);
            }

            Session["Memo17"] = memo;
            Mostrar_valores_memo();

        }

        protected void btn_aceptar_hijo_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            if (DateTime.TryParse(tb_fecha_nacimiento_hijo.Value, out fecha) && tb_nombre_hijo.Value != "")
            {
                Guardar_valores_memo_session();

                Memo_17_DDJJ memo = Session["Memo17"] as Memo_17_DDJJ;

                memo.Memo_17_DDJJ_Hijos.Add(new Memo_17_DDJJ_Hijo() { dp_hijo_apellido_y_nombre = tb_nombre_hijo.Value, dp_hijo_fecha_nacimiento = fecha });

                Session["Memo17"] = memo;

                Mostrar_valores_memo();
            }
            else
            {
                MessageBox.Show(this, "Todos los datos son obligatorios", Controles.MessageBox.Tipo_MessageBox.Warning);
            }

        }

        protected void btn_agregar_hijo_ServerClick(object sender, EventArgs e)
        {
            Guardar_valores_memo_session();

            MostrarPopUp("agregar_hijo");
        }

        protected void btn_guardar_e_imprimir_ServerClick(object sender, EventArgs e)
        {
            Guardar_valores_memo_session();
            if (Guardar_valores_memo())
            {
                memo17_ds ds = GenerarDataSet();
                RenderReport(ds);
            }
            else
            {
                MessageBox.Show(this, "Ocurrio un error al guardar los datos ingresados. ", MessageBox.Tipo_MessageBox.Danger);
            }
        }

        private memo17_ds GenerarDataSet()
        {
            Memo_17_DDJJ memo = Session["Memo17"] as Memo_17_DDJJ;
            memo17_ds ret = new memo17_ds();
            string legajo = "-";
            using (Model1Container cxt = new Model1Container())
            {
                Agente aa = cxt.Agentes.FirstOrDefault(x => x.Id == memo.AgenteId);
                if (aa != null)
                {
                    legajo = aa.Legajo.ToString();
                }
            }

            memo17_ds.Datos_memoRow dr = ret.Datos_memo.NewDatos_memoRow();

            dr.legajo = legajo;
            dr.aa_cargo = memo.aa_cargo == "" ? "Sin datos" : memo.aa_cargo;
            dr.aa_contrato_obra_ingreso = memo.aa_contrato_obra_ingreso.ToString("dd/MM/yyyy") != "01/01/1800" ? memo.aa_contrato_obra_ingreso.ToShortDateString() : "Sin datos";
            dr.aa_contrato_obra_instrumento = memo.aa_contrato_obra_instrumento == "" ? "Sin datos" : memo.aa_contrato_obra_instrumento;
            dr.aa_contrato_serv_ingreso = memo.aa_contrato_serv_ingreso.ToString("dd/MM/yyyy") != "01/01/1800" ? memo.aa_contrato_serv_ingreso.ToShortDateString() : "Sin datos";
            dr.aa_contrato_serv_instrumento = memo.aa_contrato_serv_instrumento == "" ? "Sin datos" : memo.aa_contrato_serv_instrumento;
            dr.aa_funcion = memo.aa_funcion == "" ? "Sin datos" : memo.aa_funcion;
            dr.aa_nombramiento_ingreso = memo.aa_nombramiento_ingreso.ToString("dd/MM/yyyy") != "01/01/1800" ? memo.aa_nombramiento_ingreso.ToShortDateString() : "Sin datos";
            dr.aa_nombramiento_instrumento = memo.aa_nombramiento_instrumento == "" ? "Sin datos" : memo.aa_nombramiento_instrumento;
            dr.ca_en_admin_nacional = memo.ca_en_admin_nacional == "" ? "Sin datos" : memo.ca_en_admin_nacional;
            dr.ca_en_admin_provincial = memo.ca_en_admin_provincial == "" ? "Sin datos" : memo.ca_en_admin_provincial;
            dr.ca_otros_antecedentes = memo.ca_otros_antecedentes == "" ? "Sin datos" : memo.ca_otros_antecedentes;
            dr.ca_privados = memo.ca_privados == "" ? "Sin datos" : memo.ca_privados;
            dr.dp_apellido_y_nombre = memo.dp_apellido_y_nombre == "" ? "Sin datos" : memo.dp_apellido_y_nombre;
            dr.dp_clase = memo.dp_clase == "" ? "Sin datos" : memo.dp_clase;
            dr.dp_conyugue_apellido_y_nombre = memo.dp_conyugue_apellido_y_nombre == "" ? "Sin datos" : memo.dp_conyugue_apellido_y_nombre;
            dr.dp_conyugue_nacionalidad = memo.dp_conyugue_nacionalidad == "" ? "Sin datos" : memo.dp_conyugue_nacionalidad;
            dr.dp_dni = memo.dp_dni == "" ? "Sin datos" : memo.dp_dni;
            dr.dp_domicilio_barrio = memo.dp_domicilio_barrio == "" ? "Sin datos" : memo.dp_domicilio_barrio;
            dr.dp_domicilio_codpost = memo.dp_domicilio_codpost == "" ? "Sin datos" : memo.dp_domicilio_codpost;
            dr.dp_domicilio_direccion = memo.dp_domicilio_direccion == "" ? "Sin datos" : memo.dp_domicilio_direccion;
            dr.dp_domicilio_localidad = memo.dp_domicilio_localidad == "" ? "Sin datos" : memo.dp_domicilio_localidad;
            dr.dp_domicilio_provincia = memo.dp_domicilio_provincia == "" ? "Sin datos" : memo.dp_domicilio_provincia;
            dr.dp_estado_civil = memo.dp_estado_civil == "" ? "Sin datos" : memo.dp_estado_civil;
            dr.dp_instruccion = memo.dp_instruccion == "" ? "Sin datos" : memo.dp_instruccion;
            dr.dp_madre_apellido_y_nombre = memo.dp_madre_apellido_y_nombre == "" ? "Sin datos" : memo.dp_madre_apellido_y_nombre;
            dr.dp_madre_nacionalidad = memo.dp_madre_nacionalidad == "" ? "Sin datos" : memo.dp_madre_nacionalidad;
            dr.dp_madre_vive = memo.dp_madre_vive ? "Si" : "No";
            dr.dp_nacimiento_fecha = memo.dp_nacimiento_fecha.ToString("dd/MM/yyyy") != "01/01/1800" ? memo.dp_nacimiento_fecha.ToShortDateString() : "Sin datos";
            dr.dp_nacimiento_lugar = memo.dp_nacimiento_lugar == "" ? "Sin datos" : memo.dp_nacimiento_lugar;
            dr.dp_nacionalidad = memo.dp_nacionalidad == "" ? "Sin datos" : memo.dp_nacionalidad;
            dr.dp_nativo_naturalizado = memo.dp_nativo_naturalizado == "" ? "Sin datos" : memo.dp_nativo_naturalizado;
            dr.dp_padre_apellido_y_nombre = memo.dp_padre_apellido_y_nombre == "" ? "Sin datos" : memo.dp_padre_apellido_y_nombre;
            dr.dp_padre_nacionalidad = memo.dp_padre_nacionalidad == "" ? "Sin datos" : memo.dp_padre_nacionalidad;
            dr.dp_padre_vive = memo.dp_padre_vive ? "Si" : "No";
            dr.dp_profesion = memo.dp_profesion == "" ? "Sin datos" : memo.dp_profesion;
            dr.fecha_modicacion = memo.fecha_modificacion.ToString("dd/MM/yyyy hh:mm:ss");
            dr.tiene_hijos = memo.Memo_17_DDJJ_Hijos.Count() == 0 ? "No tiene hijos" : "";
            ret.Datos_memo.Rows.Add(dr);

            foreach (Memo_17_DDJJ_Hijo hijo in memo.Memo_17_DDJJ_Hijos)
            {
                memo17_ds.Datos_memo_hijosRow hr = ret.Datos_memo_hijos.NewDatos_memo_hijosRow();
                hr.legajo = legajo;
                hr.dp_hijo_apellido_y_nombre = hijo.dp_hijo_apellido_y_nombre;
                hr.dp_hijo_fecha_nacimiento = hijo.dp_hijo_fecha_nacimiento.ToShortDateString();
                ret.Datos_memo_hijos.Rows.Add(hr);
            }

            return ret;
        }

        private void RenderReport(memo17_ds ds)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/memo17_r.rdlc");

            ReportDataSource datos = new ReportDataSource("datos", ds.Datos_memo.Rows);
            ReportDataSource hijos = new ReportDataSource("hijos", ds.Datos_memo_hijos.Rows);

            viewer.LocalReport.DataSources.Add(datos);
            viewer.LocalReport.DataSources.Add(hijos);

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


            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }

        protected void btn_imprimir_solamente_ServerClick(object sender, EventArgs e)
        {
            Memo_17_DDJJ memo_session = Session["Memo17"] as Memo_17_DDJJ;
            using (var cxt = new Model1Container())
            {
                Memo_17_DDJJ memo = cxt.Memo_17_DDJJs.FirstOrDefault(m => m.AgenteId == memo_session.AgenteId);

                if (memo != null)
                {
                    Session["Memo17"] = memo;
                    memo17_ds ds = GenerarDataSet();
                    RenderReport(ds);
                }
                else
                {
                    Controles.MessageBox.Show(this, "Aún no realizó ninguna declaración de datos, debe guardar antes de imprimir.");
                }
            }

        }
    }
}