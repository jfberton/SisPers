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

public class Informe_cierres_mensuales
{
    public Informe_cierres_mensuales(List<Informe_cierres_agente> _datos)
    {
        datos = _datos;
    }

    private List<Informe_cierres_agente> datos;

    public Byte[] Generar_informe()
    {
        Byte[] res = null;

        using (MemoryStream ms = new MemoryStream())
        {
            //Genero el PDF en memoria para ir agregando las partes
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4, false);

            float puntos_por_centimetro = (float)20;

            float margenSuperior = ((float)(0.5 * puntos_por_centimetro));
            float margenInferior = ((float)(2 * puntos_por_centimetro));
            float margenDerecho = ((float)(1 * puntos_por_centimetro));
            float margenIzquierdo = ((float)(3 * puntos_por_centimetro));

            document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
            document.SetFontSize(12);

            int corrida = 0;

            List<string> areas = datos.Select(c => c.Area).Distinct().ToList();

            List<string> agentes = new List<string>();
            

            foreach (string area in areas)
            {
                if (corrida > 0)//no es el primer area que recorro hago un break
                {
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                }

                #region encabezado reporte

                Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                document.Add(membrete);

                Table tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] { 15, 80, 15 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5);

                Cell celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph("CIERRES MENSUALES")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontSize(12);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph(area.ToUpper()))
                                    .SetFontSize(10)
                                    .SetBold()
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                document.Add(tableEncabezado);

                #endregion


                agentes = datos.Where(c => c.Area == area).Select(c => c.Legajo).Distinct().ToList();

                foreach (var legajo in agentes)
                {

                    string nombre = datos.Where(c => c.Legajo == legajo).Select(c => c.Nombre).FirstOrDefault();

                    #region datos generales

                    Paragraph desde_hasta = new Paragraph(String.Format("Datos del agente {0} - {1}", legajo, nombre.ToUpperInvariant()));
                    document.Add(desde_hasta);

                    Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 10, 10, 10, 10, 10, 10, 20 })).UseAllAvailableWidth().SetFontSize(10);

                    #region Encabezado tabla detalle

                    Cell cell = new Cell(1, 1).Add(new Paragraph("Período")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Horas mes")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Horas bonificación")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Horas acumuladas")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Días sin cerrar")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Horas año ant")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Horas año act")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Fecha actualicación")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);

                    #endregion

                    #region Carga de valores detalle
                    ///Recorro los valores asociados al area y voy cargando en la tabla
                    ///
                    string horas_anio_ant = "00:00";
                    string horas_anio_act = "00:00";
                    
                    foreach(Informe_cierres_agente cierre in datos.Where(c => c.Legajo == legajo).OrderBy(c=>c.Periodo_int))
                    {
                        cell = new Cell(1, 1).Add(new Paragraph(cierre.Periodo_str)).SetTextAlignment(TextAlignment.RIGHT);
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(cierre.Hora_mes)).SetTextAlignment(TextAlignment.RIGHT);
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(cierre.Horas_bonificcion)).SetTextAlignment(TextAlignment.RIGHT);
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(cierre.Horas_acumuladas)).SetTextAlignment(TextAlignment.RIGHT);
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

            document.Flush();

            document.Close();

            res = ms.ToArray();
        }

        return res;
    }
}