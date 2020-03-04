using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System.IO;

namespace SisPer.Aplicativo
{
    public partial class Personal_AprobarFranco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;
                MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                CargarPagina();
            }
        }

        private void CargarPagina()
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
                if (Session["IdFranco"] != null)
                {
                    CargarDatosFranco();
                }
            }
        }

        private void CargarDatosFranco()
        {
            int id = Convert.ToInt32(Session["IdFranco"]);
            Session["IdFranco"] = null;
            Model1Container cxt = new Model1Container();
            Session["YaImprimio"] = "No";
            Franco f = cxt.Francos.First(fr => fr.Id == id);
            Session["Franco"] = f;
            lbl_Anio.Text = f.DiasFranco.Select(d => d.Dia).Min().Year.ToString();
            lbl_Mes.Text = f.DiasFranco.Select(d => d.Dia).Min().ToString("MMMM");
            lbl_Dias.Text = StringDiasFranco(f);
            lbl_FechaFirmaAgente.Text = f.FechaSolicitud.ToShortDateString();
            lbl_Legajo.Text = f.Agente.Legajo.ToString();
            lbl_NomyAp.Text = f.Agente.ApellidoYNombre;
            lbl_Area.Text = f.Agente.Area.Nombre;

            lbl_anioActual.Text = f.Agente.HorasAcumuladasAnioActual;
            lbl_AnioAnterior.Text = f.Agente.HorasAcumuladasAnioAnterior;
            lbl_MediadoDeEsteAnio.Text = "01/07/" + DateTime.Today.Year.ToString();

            MovimientoFranco mf = ObtenerMovimientoAprobadoJefe(f);
            lbl_legajoJefe.Text = mf.Agente.Legajo.ToString();
            lbl_NomyApJefe.Text = mf.Agente.ApellidoYNombre;
            lbl_AreaJefe.Text = mf.Agente.Area.Nombre;
            lbl_FechaFirmaJefe.Text = mf.Fecha.ToShortDateString();
        }

        private MovimientoFranco ObtenerMovimientoAprobadoJefe(Franco f)
        {
            return f.MovimientosFranco.First(mf => mf.Estado == EstadosFrancos.AprobadoJefe);
        }

        private string StringDiasFranco(Franco f)
        {
            string ret = string.Empty;

            foreach (DiaFranco item in f.DiasFranco)
            {
                ret += item.Dia.ToString("dddd dd") + ", ";
            }

            ret = ret.Remove(ret.Length - 2);

            return ret;
        }

        protected void btn_Aprobar_Click(object sender, EventArgs e)
        {
            if (Session["YaImprimio"].ToString() != "Si")
            {
                btn_Aprobar.Enabled = false;
                btn_Rechazar.Enabled = false;
                Agente ag = Session["UsuarioLogueado"] as Agente;
                Franco f = Session["Franco"] as Franco;
                ProcesosGlobales.ModificarEstadoFranco(f.Id, EstadosFrancos.AprobadoPersonal, ag);
                Model1Container cxt = new Model1Container();
                Session["Franco"] = cxt.Francos.First(fra => fra.Id == f.Id);
                RenderReport();
            }
        }

        private FrancoCompensatorio ObtenerDS()
        {
            Model1Container cxt = new Model1Container();

            FrancoCompensatorio ds = new FrancoCompensatorio();

            Agente ag = Session["UsuarioLogueado"] as Agente;
            Franco f = Session["Franco"] as Franco;

            FrancoCompensatorio.DatosRow dr = ds.Datos.NewDatosRow();

            dr.Anio = f.DiasFranco.Select(d => d.Dia).Min().Year.ToString();
            dr.ApellidoyNombre = f.Agente.ApellidoYNombre;
            dr.CantidadFrancosAnio = cxt.Francos.Where(fra => fra.AgenteId == f.AgenteId && fra.DiasFranco.Select(d => d.Dia).Min().Year == DateTime.Today.Year && fra.Estado == EstadosFrancos.Aprobado).Count().ToString();
            dr.DepartamentoAgente = f.Agente.Area.Nombre;
            dr.FechaFirmaAgente = f.MovimientosFranco.First(mfra => mfra.Estado ==  EstadosFrancos.Solicitado).Fecha.ToShortDateString();
            dr.FechaFirmaJefe = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoJefe).Fecha.ToShortDateString();
            dr.FechaFirmaPersonal = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoPersonal).Fecha.ToShortDateString();
            dr.Dias = StringDiasFranco(f);
            dr.HorasSaldoAnioAnterior = f.Agente.HorasAcumuladasAnioAnterior;
            dr.HorasSaldoAnioCurso = f.Agente.HorasAcumuladasAnioActual;
            dr.Legajo = f.Agente.Legajo.ToString();
            dr.Mes = f.DiasFranco.Select(d => d.Dia).Min().ToString("MMMM");
            dr.NomyApJefe = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoJefe).Agente.ApellidoYNombre;
            dr.NomyApPersonal = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoPersonal).Agente.ApellidoYNombre;
            dr.LegajoJefe = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoJefe).Agente.Legajo.ToString();
            dr.LegajoPersonal = f.MovimientosFranco.First(mfra => mfra.Estado == EstadosFrancos.AprobadoPersonal).Agente.Legajo.ToString();
            dr.FirmaAgente = ObtenerFirma(dr.Legajo.ToString());
            dr.FirmaJefe = ObtenerFirma(dr.LegajoJefe.ToString());
            dr.FirmaPersonal = ObtenerFirma(dr.LegajoPersonal.ToString());
            ds.Datos.Rows.Add(dr);

            return ds;
        }

        private byte[] ObtenerFirma(string legajo)
        { 
           DirectoryInfo di1 = new DirectoryInfo(Server.MapPath("../Imagenes/"));

           if(File.Exists(di1.FullName + "\\" + legajo + "\\Firma.jpg"))
           {
               return imageToByteArray(System.Drawing.Image.FromFile(di1.FullName + "\\" + legajo + "\\Firma.jpg"));
           }
           else
           {
               return imageToByteArray(System.Drawing.Image.FromFile(di1.FullName + "\\firmaBlanco.jpg"));
           }

        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void RenderReport()
        {
            Session["YaImprimio"] = "Si";
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/FrancoCompensatorio_r.rdlc");

            FrancoCompensatorio ds = ObtenerDS();
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

        protected void btn_Rechazar_Click(object sender, EventArgs e)
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;
            Franco f = Session["Franco"] as Franco;
            ProcesosGlobales.ModificarEstadoFranco(f.Id, EstadosFrancos.Cancelado, ag);
            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
        }

        protected void btn_Volver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/MainPersonal.aspx");
        }
       
    }
}