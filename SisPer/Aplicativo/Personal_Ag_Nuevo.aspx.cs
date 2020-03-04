using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Ag_Nuevo : System.Web.UI.Page
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

                if (usuariologueado.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                    Model1Container cxt = new Model1Container();
                    Session["CXT"] = cxt;
                    CargarEmun();
                    if (Session["IdAg"] != null)
                    {
                        int id = Convert.ToInt32(Session["IdAg"]);
                        Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                        Session["IdAg"] = null;
                        Session["Agente"] = ag;
                        CargarValoresAgente();
                    }
                    else
                    {
                        Session["Agente"] = null;
                    }
                }
            }
        }

        private void CargarEmun()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(PerfilUsuario)))
            {
                if (value != 4 && value != 3) //Desarrollador solo mio)
                {
                    dictionary.Add(value, Enum.GetName(typeof(PerfilUsuario), value));
                }
            }

            ddl_Perfiles.DataSource = dictionary;
            ddl_Perfiles.DataTextField = "Value";
            ddl_Perfiles.DataValueField = "Key";
            ddl_Perfiles.DataBind();
        }
        

        private void CargarValoresAgente()
        {
            Agente ag = Session["Agente"] as Agente;
            //Datos agente
            tb_NomYApAgente.Text = ag.ApellidoYNombre;
            tb_legajo.Text = ag.Legajo.ToString();
            tb_Mail.Text = ag.Legajo_datos_laborales.Email;
            tb_Nacimiento.Text = ag.Legajo_datos_personales.FechaNacimiento.ToLongDateString();
            tb_IngresoAPlanta.Text = ag.Legajo_datos_laborales.FechaIngresoATP.ToLongDateString();
            tb_DNI.Text = ag.Legajo_datos_personales.DNI;
            tb_CUIT.Text = ag.Legajo_datos_laborales.CUIT;
            tb_AntiguedadAnios.Text = ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes.ToString();
            tb_AntiguedadMeses.Text = ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes.ToString();
            tb_FichaMedica.Text = ag.Legajo_datos_laborales.FichaMedica;
            tb_HoraDesde.Text = ag.HoraEntrada;
            tb_HoraHasta.Text = ag.HoraSalida;
            tb_IngresoAdmPub.Text = ag.Legajo_datos_laborales.FechaIngresoAminPub.ToLongDateString();
            chk_HorarioFlexible.Checked = (ag.HorarioFlexible ?? false) == true;

            //Datos usuario
            tb_Login.Text = ag.Usr;
            ddl_Perfiles.SelectedValue = Convert.ToInt32(ag.Perfil).ToString();

            //Datos area
            Ddl_Areas1.AreaSeleccionado = ag.Area;
            Ddl_Areas1.Enabled = false;
            chk_Jefe.Checked = ag.Jefe;
            chk_JefeTemporal.Checked = ag.JefeTemporal;
            if (ag.JefeTemporal)
            {
                tb_JefetemporalHasta.Text = ag.JefeTemporalHasta.Value.ToShortDateString();
                tb_JefetemporalHasta.Visible = true;
                lbl_JefeHasta.Visible = true;
            }

            chk_HabilitarClave.Checked = false;
            MostrarCamposClave.Visible = false;
            MostrarHabilitarClavePanel.Visible = true;
        }
      
        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Ag_Listado.aspx");
        }

        protected void chk_HabilitarClave_CheckedChanged(object sender, EventArgs e)
        {
            MostrarCamposClave.Visible = chk_HabilitarClave.Checked;
        }

        protected void Validator_Usr_YaExiste_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (tb_Login.Enabled)
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                Agente ag = Session["Agente"] as Agente;
                if (ag != null)
                {
                    //controlo que no exista otro agente con un usuario igual al de este 
                    //por eso el id debe ser distinto al del agente en cuestion
                    args.IsValid = (cxt.Agentes.Where(u => u.Usr == tb_Login.Text && u.Id != ag.Id).Count() == 0 && tb_Login.Text!="SU");
                }
                else
                {
                    args.IsValid = (cxt.Agentes.Where(u => u.Usr == tb_Login.Text).Count() == 0 && tb_Login.Text!= "SU");
                }
            }
            else
                args.IsValid = true;
        }

        protected void ValidarPerfil_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddl_Perfiles.SelectedItem.Text != "Ninguno";
        }

        protected void ValidarPerfil2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            args.IsValid = ddl_Perfiles.SelectedItem.Text == "Personal" ? (ag.Perfil == PerfilUsuario.Personal && (ag.Jefe || ag.JefeTemporal)) ? true : false : true;
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = tb_Clave.Text.Length >= 6;
        }

        protected void btn_Aceptar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                Model1Container cxt = Session["CXT"] as Model1Container;
                Agente ag = Session["Agente"] as Agente;
                if (ag == null)
                {
                    ag = new Agente();
                    cxt.Agentes.AddObject(ag);
                    ag.HorasAcumuladasAnioActual = "000:00";
                    ag.HorasAcumuladasAnioAnterior = "000:00";
                    if (Ddl_Areas1.AreaSeleccionado != null)
                    {
                        ag.Reasignaciones.Add(new Reasignacion() { Area = Ddl_Areas1.AreaSeleccionado, Desde = DateTime.Today });
                    }
                }

                //Datos Agente
                ag.ApellidoYNombre = tb_NomYApAgente.Text;
                ag.Legajo = Convert.ToInt32(tb_legajo.Text);
                if (ag.Legajo_datos_laborales == null)
                {
                    ag.Legajo_datos_laborales = new Legajo_datos_laborales();
                }
                ag.Legajo_datos_laborales.CUIT = tb_CUIT.Text;
                ag.HoraEntrada = tb_HoraDesde.Text;
                ag.HoraSalida = tb_HoraHasta.Text;
                if (ag.Legajo_datos_personales == null)
                {
                    ag.Legajo_datos_personales = new Legajo_datos_personales();
                }
                ag.Legajo_datos_personales.DNI = tb_DNI.Text;
                ag.Legajo_datos_laborales.Email = tb_Mail.Text;
                ag.Legajo_datos_laborales.FechaIngresoAminPub = Convert.ToDateTime(tb_IngresoAdmPub.Text);
                ag.Legajo_datos_personales.FechaNacimiento = Convert.ToDateTime(tb_Nacimiento.Text);
                ag.Legajo_datos_laborales.FechaIngresoATP = Convert.ToDateTime(tb_IngresoAPlanta.Text);
                ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes = Convert.ToInt16(tb_AntiguedadAnios.Text);
                ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes = Convert.ToInt16(tb_AntiguedadMeses.Text);
                ag.Legajo_datos_laborales.FichaMedica = tb_FichaMedica.Text;
                ag.Legajo_datos_laborales.Grupo = " ";
                ag.Legajo_datos_laborales.Apartado = " ";
                ag.Legajo_datos_laborales.Cargo = " ";
                ag.Legajo_datos_laborales.Situacion_de_revista = " ";
                ag.HorarioFlexible = chk_HorarioFlexible.Checked;

                //Datos del area
                ag.Area = Ddl_Areas1.AreaSeleccionado;

                #region Registro modificaciones realizadas por agente de personal sobre algun agente

                if (ag.Jefe == false && chk_Jefe.Checked)
                {
                    RegistrarMovimientoSobreAgente(ag,"Asigno como jefe");
                }
                
                if (ag.JefeTemporal == false && chk_JefeTemporal.Checked)
                {
                    RegistrarMovimientoSobreAgente(ag, "Asigno como jefe temporal hasta " + tb_JefetemporalHasta.Text);
                }

                if ((ag.Jefe || ag.JefeTemporal) && chk_Agente.Checked)
                { 
                    //quito marca de jefe o de jefe temporal
                    if (ag.Jefe)
                    {
                        RegistrarMovimientoSobreAgente(ag, "Quito marca de Jefe");
                    }
                    else
                    {
                        RegistrarMovimientoSobreAgente(ag, "Quito marca de Jefe temporal");
                    }
                }
            
                #endregion

                ag.Jefe = chk_Jefe.Checked;

                if (chk_JefeTemporal.Checked)
                {
                    ag.JefeTemporal = true;
                    ag.JefeTemporalHasta = Convert.ToDateTime(tb_JefetemporalHasta.Text);
                }
                else
                {
                    ag.JefeTemporal = false;
                    ag.JefeTemporalHasta = null;
                }

                //Datos Usuario
                ag.Usr = tb_Login.Text;
                ag.Perfil = ((PerfilUsuario)Convert.ToInt32(ddl_Perfiles.SelectedValue));
                if (MostrarCamposClave.Visible)
                {
                    string pass = System.Web.Security.Membership.GeneratePassword(8, 0).ToUpper();
                    pass = Regex.Replace(pass, @"[^a-zA-Z0-9]", m => "9");
                    ag.Pass = Cripto.Encriptar(pass);
                    Session["NuevaClave"] = pass;
                }

                cxt.SaveChanges();

                Session["Agente"] = ag;

                if (MostrarCamposClave.Visible)
                {
                    RenderReport();
                }
                else
                {
                    Controles.MessageBox.Show(this, "Los cambios se registraron correctamente.", Controles.MessageBox.Tipo_MessageBox.Success, "Genial!", "Personal_Ag_Listado.aspx");
                }
            }
        }

        private void RegistrarMovimientoSobreAgente(Agente ag, string p)
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarMovimientoSobreAgente(ag, usuarioLogueado, DateTime.Now, nombreMaquina, localIP, p);
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_Nacimiento.Text, out d);
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_IngresoAPlanta.Text, out d);
        }

        protected void cv_Legajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int legajo = 0;
            args.IsValid = int.TryParse(tb_legajo.Text, out legajo);
        }

        protected void tb_AntiguedadAnios_RegularExpressionValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int var = 0;
            args.IsValid = int.TryParse(tb_AntiguedadAnios.Text, out var);
        }

        protected void tb_AntiguedadMesesRegularExpressionValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int var = 0;
            args.IsValid = int.TryParse(tb_AntiguedadMeses.Text, out var) && var<12;
        }

        #region Informe Entrega Nueva Clave

        private OtorgoClave_DS ObtenerDS()
        {
            Model1Container cxt = new Model1Container();

            OtorgoClave_DS ds = new OtorgoClave_DS();

            Agente personal = Session["UsuarioLogueado"] as Agente;
            Agente ag = Session["Agente"] as Agente;


            OtorgoClave_DS.DatosRow dr = ds.Datos.NewDatosRow();

            dr.AgentePersonal = personal.ApellidoYNombre;
            dr.NombreAgente = ag.ApellidoYNombre;
            dr.Usuario = ag.Usr;
            dr.Departamento = ag.Area.Nombre;
            dr.Clave = Session["NuevaClave"].ToString();
            dr.Fecha = "Resistencia, " + DateTime.Today.ToLongDateString();
            
            ds.Datos.Rows.Add(dr);

            return ds;
        }

        private void RenderReport()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/OtorgoClave.rdlc");

            OtorgoClave_DS ds = ObtenerDS();
            ReportDataSource maestro = new ReportDataSource("DS", ds.Datos.Rows);

            viewer.LocalReport.DataSources.Add(maestro);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }
       
        #endregion

        protected void cv_legajo1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool ret = true;
            int legajo = 0;
            ret= int.TryParse(tb_legajo.Text, out legajo);
            Agente ag = Session["Agente"] as Agente;
            if (ag == null)
            {//esta por dar de alta un agente
                if (ret)
                {
                    Model1Container cxt = new Model1Container();
                    ret = cxt.Agentes.FirstOrDefault(a => a.Legajo == legajo) == null;
                }
            }

            args.IsValid = ret;
        }

        protected void chk_JefeTemporal_CheckedChanged(object sender, EventArgs e)
        {
            lbl_JefeHasta.Visible = chk_JefeTemporal.Checked;
            tb_JefetemporalHasta.Visible = chk_JefeTemporal.Checked;
        }

        protected void CV_JefeTemporal_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_JefetemporalHasta.Text, out d) || tb_JefetemporalHasta.Visible == false; 
        }

        protected void CV_JefeTemporalFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (tb_JefetemporalHasta.Visible && tb_JefetemporalHasta.Text.Length > 0) || tb_JefetemporalHasta.Visible == false;
        }
    }
}