using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Jefe_Memo17 : System.Web.UI.Page
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
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    if (!(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal))
                    {
                        Response.Redirect("~/Default.aspx?mode=trucho");
                    }
                }

                Cargar_Grilla();
            }
        }


        /// <summary>
        /// Carga los agentes subordinados
        /// </summary>
        private void Cargar_Grilla(bool paginoSolamente = false)
        {
            Model1Container cxt = new Model1Container();
            Agente ag = null;
            ag = Session["UsuarioLogueado"] as Agente;

            //Actualizo el usuario de la sesión
            ag = cxt.Agentes.First(a => a.Id == ag.Id);

            List<Agente> agentes = new List<Agente>();


            var items = cxt.sp_obtener_agentes_cascada(ag.Id, ag.Perfil == PerfilUsuario.Personal);

            var items_memo17 = (from x in items
                                select new
                                {
                                    Id = x.id_agente,
                                    x.nombre_area,
                                    x.nivel,
                                    x.legajo,
                                    x.nombre_agente,
                                    ultima_modificacion = ObtenerFechaUltimaModificacion(x.id_agente.Value)
                                }).ToList();

            gv_memo.DataSource = items_memo17;
            gv_memo.DataBind();
        }

        private string ObtenerFechaUltimaModificacion(int id)
        {
            string ret = " - ";

            using (var cxt = new Model1Container())
            {
                if (cxt.Memo_17_DDJJs.Count(x => x.AgenteId == id) > 0)
                {
                    var d = cxt.Memo_17_DDJJs.Where(x => x.AgenteId == id).Max(m => m.fecha_modificacion);
                    ret = d.ToString();
                }
            }

            return ret;
        }

       
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.Cells[1].Text == "Jefe")
                {
                    e.Row.ForeColor = Color.Black;
                    e.Row.Font.Bold = true;
                }

                if (e.Row.Cells[1].Text == "Jefe temporal")
                {
                    e.Row.ForeColor = Color.Blue;
                }
            }
        }

        protected void btn_ver_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((Button)sender).CommandArgument);
            Agente ag = Session["UsuarioLogueado"] as Agente;
            if (id == ag.Id)
            {
                Response.Redirect("~/Aplicativo/Ag_Memo17.aspx");
            }
            else
            {
                using (var cxt = new Model1Container())
                {
                    Memo_17_DDJJ memo = cxt.Memo_17_DDJJs.FirstOrDefault(m => m.AgenteId == id);
                    if (memo != null)
                    {
                        Session["ver_memo"] = memo;
                        memo17_ds ds = GenerarDataSet();
                        RenderReport(ds);
                    }
                    else
                    {
                        Controles.MessageBox.Show(this, "El agente no declaró datos");
                    }
                }
            }
        }

        private memo17_ds GenerarDataSet()
        {
            Memo_17_DDJJ memo = Session["ver_memo"] as Memo_17_DDJJ;
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

        protected void gv_memo_PreRender(object sender, EventArgs e)
        {
            if (gv_memo.Rows.Count > 0)
            {
                gv_memo.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}