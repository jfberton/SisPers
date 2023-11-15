using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.IO.Image;
using Image = iText.Layout.Element.Image;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SisPer.Aplicativo.Reportes;
using System.Data;
using static SisPer.Aplicativo.Personal_Informe_Cierres_Mensuales;
using System.Runtime.Serialization.Configuration;
using SisPer.Aplicativo;

public class Informe_cierres_mensuales : Informe_Personal<List<Informe_cierres_agente>>
{
    public Informe_cierres_mensuales(List<Informe_cierres_agente> datos, Agente ag_informe) : base(datos, ag_informe, "CIERRES MENSUALES")
    {
    }

    protected override stream_informe Generar_cuerpo_informe(List<Informe_cierres_agente> datos, stream_informe informe)
    {
        int corrida = 0;

        Document document = informe.document;

        List<string> areas = datos.Select(c => c.Area).Distinct().ToList();

        List<string> agentes = new List<string>();


        foreach (string area in areas)
        {
            if (corrida > 0)//no es el primer area que recorro hago un break
            {
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
            }

            agentes = datos.Where(c => c.Area == area).Select(c => c.Legajo).Distinct().ToList();

            foreach (var legajo in agentes)
            {

                string nombre = datos.Where(c => c.Legajo == legajo).Select(c => c.Nombre).FirstOrDefault();

                #region datos generales

                Paragraph desde_hasta = new Paragraph(String.Format("Datos del agente {0} - {1}", legajo, nombre.ToUpperInvariant()));

                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 10, 10, 10, 10, 10, 10, 20 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true).SetMarginBottom(15);

                #region Encabezado tabla detalle

                Cell cell = new Cell(1,8).Add(desde_hasta).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Período")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Hs. mes")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph(
                                                new Text("Hs. bonif.").SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY).SetItalic()
                                                        )
                                            ).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph(
                                            new Text("Hs. desp. bonif.").SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY).SetItalic()
                                                        )
                                            ).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Días sin cerrar")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Hs. año ant.")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Hs. año act.")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Fecha actualicación")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);

                #endregion

                #region Carga de valores detalle
                ///Recorro los valores asociados al area y voy cargando en la tabla
                ///
                string horas_anio_ant = "00:00";
                string horas_anio_act = "00:00";

                foreach (Informe_cierres_agente cierre in datos.Where(c => c.Legajo == legajo).OrderBy(c => c.Periodo_int))
                {
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Periodo_str)).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Hora_mes)).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(new Text(cierre.Horas_bonificcion).SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY).SetItalic())).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(new Text(cierre.Horas_acumuladas).SetFontColor(iText.Kernel.Colors.ColorConstants.DARK_GRAY).SetItalic())).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Dias_sin_cerrar.ToString())).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Hora_año_ant)).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Hora_año_act)).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(cierre.Fecha_actualizacion)).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_detalle.AddCell(cell);

                    horas_anio_ant = cierre.Hora_año_ant;
                    horas_anio_act = cierre.Hora_año_act;
                }

                document.Add(tabla_detalle);



                #endregion
                document.Add(new Paragraph(" "));

                Table tabla_pie_totales = new Table(UnitValue.CreatePercentArray(new float[] { 80, 20 })).SetWidth(300).SetFontSize(10);
                cell = new Cell(1, 1).Add(new Paragraph("Total de horas acumuladas del año anterior")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_pie_totales.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph().Add(new Text(horas_anio_ant).SetBold())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_pie_totales.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("Total de horas acumuladas del año actual")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_pie_totales.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph().Add(new Text(horas_anio_act).SetBold())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_pie_totales.AddCell(cell);

                tabla_pie_totales.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);

                document.Add(tabla_pie_totales);
                #endregion

            }

            corrida++;
        }

        informe.document = document; 

        return informe;
    }
}