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
using iText.IO.Font.Otf;

namespace SisPer.Aplicativo.Reportes
{
    public class Informe_domicilios_agentes
    {
        public Informe_domicilios_agentes(List<Agente> _datos)
        {
            datos = _datos;
        }

        private List<Agente> datos;

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


                var nombresAreas = datos.Select(a => a.Area.Nombre).Distinct();



                foreach (string nombreArea in nombresAreas)
                {
                    if (corrida > 0)//no es el primer area que recorro hago un break
                    {
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                    }

                    #region encabezado reporte

                    Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                    document.Add(membrete);

                    Paragraph fecha_hora_impresion = new Paragraph().Add(new Text(String.Format("Fecha hora impresión {0}", DateTime.Now)).SetFontSize(7));
                    document.Add(fecha_hora_impresion.SetTextAlignment(TextAlignment.RIGHT));

                    Table tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] { 15, 80, 15 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5);

                    Cell celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    celldaEncabezado = new Cell(1, 1).Add(new Paragraph("INFORME DOMICILIOS DE LOS AGENTES")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontSize(12);
                    tableEncabezado.AddCell(celldaEncabezado);

                    celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    celldaEncabezado = new Cell(1, 1).Add(new Paragraph(nombreArea))
                                        .SetFontSize(10)
                                        .SetBold()
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    document.Add(tableEncabezado);

                    #endregion

                    #region datos generales
                    Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 80, 80, 60 })).UseAllAvailableWidth().SetFontSize(10);

                    #region Encabezado tabla detalle

                    Cell cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("DOMICILIO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("LOCALIDAD")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle.AddCell(cell);

                    #endregion

                    #region Carga de valores detalle
                    ///Recorro los valores asociados al area y voy cargando en la tabla
                    foreach (Agente item in datos.Where(x=>x.Area.Nombre == nombreArea))
                    {
                        cell = new Cell(1, 1).Add(new Paragraph(item.Legajo.ToString()));
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(item.ApellidoYNombre));
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(item.Legajo_datos_personales.Domicilio));
                        tabla_detalle.AddCell(cell);
                        cell = new Cell(1, 1).Add(new Paragraph(item.Legajo_datos_personales.Domicilio_localidad));
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
}
