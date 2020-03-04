using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Reportes
{
    public partial class Calendario : System.Web.UI.Page
    {
        internal class DiaCalendario
        {
            internal int Dia { get; set; }
            internal string Titulo { get; set; }
            internal string Detalle { get; set; }

            internal DiaCalendario()
            {
                Dia = 0;
                Titulo = "  ";
                Detalle = "  ";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            RecorrerYGenerar(new DateTime(2013, 05, 01));
        }

        private void RecorrerYGenerar(DateTime cualquierFechaDelMesAProcesar)
        {
            DateTime primerDiaPrueba = new DateTime(cualquierFechaDelMesAProcesar.Year, cualquierFechaDelMesAProcesar.Month, 1);
            DiaCalendario[,] diasMes = new DiaCalendario[6, 7];
            for (int s = 0; s < 6; s++)
            {
                for (int d = 0; d < 7; d++)
                {
                    diasMes[s, d] = new DiaCalendario();
                }
            }

            int dia = 0;
            int semana = 0;
            int numeroDia = 1;
            int ultimoNumeroDiaMes = DateTime.DaysInMonth(primerDiaPrueba.Year, primerDiaPrueba.Month);

            switch (primerDiaPrueba.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dia = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dia = 1;
                    break;
                case DayOfWeek.Wednesday:
                    dia = 2;
                    break;
                case DayOfWeek.Thursday:
                    dia = 3;
                    break;
                case DayOfWeek.Friday:
                    dia = 4;
                    break;
                case DayOfWeek.Saturday:
                    dia = 5;
                    break;
                case DayOfWeek.Sunday:
                    dia = 6;
                    break;
            }

            while (semana <= 5 && numeroDia <= ultimoNumeroDiaMes)
            {
                while (dia <= 6 && numeroDia <= ultimoNumeroDiaMes)
                {

                    diasMes[semana, dia] = ObtenerDatos(new DateTime(cualquierFechaDelMesAProcesar.Year, cualquierFechaDelMesAProcesar.Month, numeroDia));
                    numeroDia++;
                    dia++;
                }
                dia = 0;
                semana++;
            }
            Calendario_ds ds = LlenarDS(diasMes,cualquierFechaDelMesAProcesar);
            RenderReport(ds);
        }

        private DiaCalendario ObtenerDatos(DateTime fecha)
        {
            DiaCalendario dia = new DiaCalendario();
            dia.Dia = fecha.Day;

            using (var cxt = new Model1Container())
            {
                Feriado fe = cxt.Feriados.FirstOrDefault(f => f.Dia == fecha);
                if (fe != null)
                {
                    dia.Detalle = fe.Motivo;
                }
            }

            return dia;
        }

        private Calendario_ds LlenarDS(DiaCalendario[,] dm, DateTime d)
        {
            Calendario_ds ds = new Calendario_ds();
            Calendario_ds.CalendarioRow cr = ds.Calendario.NewCalendarioRow();
            #region Semana 0
            cr.Dia = dm[0, 0].Dia;
            cr.Titulo = dm[0, 0].Titulo;
            cr.Detalle = dm[0, 0].Detalle;

            cr.Dia1 = dm[0, 1].Dia;
            cr.Titulo1 = dm[0, 1].Titulo;
            cr.Detalle1 = dm[0, 1].Detalle;

            cr.Dia2 = dm[0, 2].Dia;
            cr.Titulo2 = dm[0, 2].Titulo;
            cr.Detalle2 = dm[0, 2].Detalle;

            cr.Dia3 = dm[0, 3].Dia;
            cr.Titulo3 = dm[0, 3].Titulo;
            cr.Detalle3 = dm[0, 3].Detalle;

            cr.Dia4 = dm[0, 4].Dia;
            cr.Titulo4 = dm[0, 4].Titulo;
            cr.Detalle4 = dm[0, 4].Detalle;

            cr.Dia5 = dm[0, 5].Dia;
            cr.Titulo5 = dm[0, 5].Titulo;
            cr.Detalle5 = dm[0, 5].Detalle;

            cr.Dia6 = dm[0, 6].Dia;
            cr.Titulo6 = dm[0, 6].Titulo;
            cr.Detalle6 = dm[0, 6].Detalle;
            #endregion
            #region Semana 1
            cr.Dia7 = dm[1, 0].Dia;
            cr.Titulo7 = dm[1, 0].Titulo;
            cr.Detalle7 = dm[1, 0].Detalle;

            cr.Dia8 = dm[1, 1].Dia;
            cr.Titulo8 = dm[1, 1].Titulo;
            cr.Detalle8 = dm[1, 1].Detalle;

            cr.Dia9 = dm[1, 2].Dia;
            cr.Titulo9 = dm[1, 2].Titulo;
            cr.Detalle9 = dm[1, 2].Detalle;

            cr.Dia10 = dm[1, 3].Dia;
            cr.Titulo10 = dm[1, 3].Titulo;
            cr.Detalle10 = dm[1, 3].Detalle;

            cr.Dia11 = dm[1, 4].Dia;
            cr.Titulo11 = dm[1, 4].Titulo;
            cr.Detalle11 = dm[1, 4].Detalle;

            cr.Dia12 = dm[1, 5].Dia;
            cr.Titulo12 = dm[1, 5].Titulo;
            cr.Detalle12 = dm[1, 5].Detalle;

            cr.Dia13 = dm[1, 6].Dia;
            cr.Titulo13 = dm[1, 6].Titulo;
            cr.Detalle13 = dm[1, 6].Detalle;
            #endregion
            #region Semana 2
            cr.Dia14 = dm[2, 0].Dia;
            cr.Titulo14 = dm[2, 0].Titulo;
            cr.Detalle14 = dm[2, 0].Detalle;

            cr.Dia15 = dm[2, 1].Dia;
            cr.Titulo15 = dm[2, 1].Titulo;
            cr.Detalle15 = dm[2, 1].Detalle;

            cr.Dia16 = dm[2, 2].Dia;
            cr.Titulo16 = dm[2, 2].Titulo;
            cr.Detalle16 = dm[2, 2].Detalle;

            cr.Dia17 = dm[2, 3].Dia;
            cr.Titulo17 = dm[2, 3].Titulo;
            cr.Detalle17 = dm[2, 3].Detalle;

            cr.Dia18 = dm[2, 4].Dia;
            cr.Titulo18 = dm[2, 4].Titulo;
            cr.Detalle18 = dm[2, 4].Detalle;

            cr.Dia19 = dm[2, 5].Dia;
            cr.Titulo19 = dm[2, 5].Titulo;
            cr.Detalle19 = dm[2, 5].Detalle;

            cr.Dia20 = dm[2, 6].Dia;
            cr.Titulo20 = dm[2, 6].Titulo;
            cr.Detalle20 = dm[2, 6].Detalle;
            #endregion
            #region Semana 3
            cr.Dia21 = dm[3, 0].Dia;
            cr.Titulo21 = dm[3, 0].Titulo;
            cr.Detalle21 = dm[3, 0].Detalle;

            cr.Dia22 = dm[3, 1].Dia;
            cr.Titulo22 = dm[3, 1].Titulo;
            cr.Detalle22 = dm[3, 1].Detalle;

            cr.Dia23 = dm[3, 2].Dia;
            cr.Titulo23 = dm[3, 2].Titulo;
            cr.Detalle23 = dm[3, 2].Detalle;

            cr.Dia24 = dm[3, 3].Dia;
            cr.Titulo24 = dm[3, 3].Titulo;
            cr.Detalle24 = dm[3, 3].Detalle;

            cr.Dia25 = dm[3, 4].Dia;
            cr.Titulo25 = dm[3, 4].Titulo;
            cr.Detalle25 = dm[3, 4].Detalle;

            cr.Dia26 = dm[3, 5].Dia;
            cr.Titulo26 = dm[3, 5].Titulo;
            cr.Detalle26 = dm[3, 5].Detalle;

            cr.Dia27 = dm[3, 6].Dia;
            cr.Titulo27 = dm[3, 6].Titulo;
            cr.Detalle27 = dm[3, 6].Detalle;
            #endregion
            #region Semana 4
            cr.Dia28 = dm[4, 0].Dia;
            cr.Titulo28 = dm[4, 0].Titulo;
            cr.Detalle28 = dm[4, 0].Detalle;

            cr.Dia29 = dm[4, 1].Dia;
            cr.Titulo29 = dm[4, 1].Titulo;
            cr.Detalle29 = dm[4, 1].Detalle;

            cr.Dia30 = dm[4, 2].Dia;
            cr.Titulo30 = dm[4, 2].Titulo;
            cr.Detalle30 = dm[4, 2].Detalle;

            cr.Dia31 = dm[4, 3].Dia;
            cr.Titulo31 = dm[4, 3].Titulo;
            cr.Detalle31 = dm[4, 3].Detalle;

            cr.Dia32 = dm[4, 4].Dia;
            cr.Titulo32 = dm[4, 4].Titulo;
            cr.Detalle32 = dm[4, 4].Detalle;

            cr.Dia33 = dm[4, 5].Dia;
            cr.Titulo33 = dm[4, 5].Titulo;
            cr.Detalle33 = dm[4, 5].Detalle;

            cr.Dia34 = dm[4, 6].Dia;
            cr.Titulo34 = dm[4, 6].Titulo;
            cr.Detalle34 = dm[4, 6].Detalle;
            #endregion
            #region Semana 5
            cr.Dia35 = dm[5, 0].Dia;
            cr.Titulo35 = dm[5, 0].Titulo;
            cr.Detalle35 = dm[5, 0].Detalle;

            cr.Dia36 = dm[5, 1].Dia;
            cr.Titulo36 = dm[5, 1].Titulo;
            cr.Detalle36 = dm[5, 1].Detalle;

            cr.Dia37 = dm[5, 2].Dia;
            cr.Titulo37 = dm[5, 2].Titulo;
            cr.Detalle37 = dm[5, 2].Detalle;

            cr.Dia38 = dm[5, 3].Dia;
            cr.Titulo38 = dm[5, 3].Titulo;
            cr.Detalle38 = dm[5, 3].Detalle;

            cr.Dia39 = dm[5, 4].Dia;
            cr.Titulo39 = dm[5, 4].Titulo;
            cr.Detalle39 = dm[5, 4].Detalle;

            cr.Dia40 = dm[5, 5].Dia;
            cr.Titulo40 = dm[5, 5].Titulo;
            cr.Detalle40 = dm[5, 5].Detalle;

            cr.Dia41 = dm[5, 6].Dia;
            cr.Titulo41 = dm[5, 6].Titulo;
            cr.Detalle41 = dm[5, 6].Detalle;
            #endregion
            ds.Calendario.AddCalendarioRow(cr);
            ds.General.AddGeneralRow("Feriados", d.ToString("MMMM"));
            return ds;
        }

        private void RenderReport(Calendario_ds ds)
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Calendario_r.rdlc");

            ReportDataSource detalle = new ReportDataSource("DataSet1", ds.Calendario.Rows);
            ReportDataSource maestro = new ReportDataSource("DataSet2", ds.General.Rows);

            viewer.LocalReport.DataSources.Add(maestro);
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
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('ReportePDF.aspx');</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
        }
    }
}