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

public class Informe_remotos_autorizados
{
    public Informe_remotos_autorizados(ListadoSalidas_DS _datos, DateTime _desde, DateTime _hasta)
    {
        datos = _datos;
        desde = _desde;
        hasta = _hasta;
    }
    private ListadoSalidas_DS datos;
    private DateTime desde;
    private DateTime hasta;

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

            foreach (ListadoSalidas_DS.GeneralRow dr in datos.General)
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

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph("INFORME TRABAJO REMOTO AUTORIZADO")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontSize(12);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph(dr.Sector.ToUpper()))
                                    .SetFontSize(10)
                                    .SetBold()
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                document.Add(tableEncabezado);

                #endregion

                Paragraph desde_hasta = new Paragraph(String.Format("Datos desde el {0} al {1}", desde.ToShortDateString(), hasta.ToShortDateString()))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(8);
                document.Add(desde_hasta);


                #region datos generales
                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 80, 20 })).UseAllAvailableWidth().SetFontSize(10);

                #region Encabezado tabla detalle

                Cell cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("FECHA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);

                #endregion

                #region Carga de valores detalle
                ///Recorro los valores asociados al area y voy cargando en la tabla
                foreach (ListadoSalidas_DS.SalidasRow item in datos.Salidas.Where(ss => ss.Sector == dr.Sector))
                {
                    cell = new Cell(1, 1).Add(new Paragraph(item.Legajo));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Agente));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Fecha));
                    tabla_detalle.AddCell(cell);
                }

                document.Add(tabla_detalle);

                #endregion

                #endregion

                corrida++;
            }

            document.Flush();

            document.Close();

            res = ms.ToArray();
        }

        return res;
    }
}