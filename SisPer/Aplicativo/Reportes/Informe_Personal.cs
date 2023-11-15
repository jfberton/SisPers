using System;
using System.IO;
using System.Linq;
using System.Web;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.IO.Image;
using Image = iText.Layout.Element.Image;
using System.Configuration;
using iText.Kernel.Pdf.Canvas;

namespace SisPer.Aplicativo.Reportes
{
    public struct Datos_informe_desde_hasta<T>
    {
        public T datos { get; set; }
        public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
    }

    public abstract class Informe_Personal<T>
    {
        private T datos;
        private Agente ag_informe;
        private string titulo_informe;
        private readonly float puntos_por_centimetro = (float)20;
        protected readonly float margenSuperior;
        protected readonly float margenInferior;
        protected readonly float margenDerecho;
        protected readonly float margenIzquierdo;
        private stream_informe informe;

        public struct stream_informe
        {
            public iText.Layout.Document document { get; set; }
            public MemoryStream ms { get; set; }
        }

        public Informe_Personal(T datos, Agente ag_informe, string titulo)
        {
            this.datos = datos;
            this.ag_informe = ag_informe;
            this.titulo_informe = titulo;

            //genero el stream del informe con los margenes
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4, false);

            margenSuperior = ((float)(6 * puntos_por_centimetro));
            margenInferior = ((float)(2 * puntos_por_centimetro));
            margenDerecho = ((float)(1 * puntos_por_centimetro));
            margenIzquierdo = ((float)(3 * puntos_por_centimetro));

            document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
            document.SetFontSize(10);

            informe = new stream_informe();
            informe.ms = ms;
            informe.document = document;

        }

        public Byte[] Generar_informe()
        {
            stream_informe cuerpoInforme = Generar_cuerpo_informe(datos, informe);
            Byte[] informeConEncabezadoYPie = Agregar_encabezado_y_pie(cuerpoInforme);



            return informeConEncabezadoYPie;
        }

        protected abstract stream_informe Generar_cuerpo_informe(T datos, stream_informe informe);

        protected Byte[] Agregar_encabezado_y_pie(stream_informe informe)
        {
            Byte[] bytesOutput;

            using (MemoryStream ms = informe.ms)
            {
                PdfDocument _pdfDoc = informe.document.GetPdfDocument();
                Document document = informe.document;

                document.Flush();

                #region Agrego cabecera y pie

                Text t_leyenda = new Text(ConfigurationManager.AppSettings["Leyenda"]);

                Paragraph leyenda = new Paragraph().Add(t_leyenda)
                    .SetFontSize(8);

                Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);

                float margenIzquierdo = ((float)(3 * puntos_por_centimetro));

                int pagina = 1;

                var pdfDoc = document.GetPdfDocument();
                int paginas_doc = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= paginas_doc; i++)
                {
                    PdfPage page = pdfDoc.GetPage(i);
                    Rectangle area = page.GetPageSize().ApplyMargins(25, 15, 20, 15, false);
                    float x = area.GetWidth() + 10;
                    float y = area.GetTop() + 5;

                    #region encabezados

                    //comun a todos membrete y leyenda
                    document.ShowTextAligned(leyenda, x, y, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);

                    PdfCanvas aboveCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);

                    new Canvas(aboveCanvas, pdfDoc, area).Add(membrete);

                    document.ShowTextAligned(new Paragraph(titulo_informe).SetBold().SetFontSize(12).SetUnderline(), x/2, y - 85, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
                    document.ShowTextAligned(new Paragraph("agente: ").Add(ag_informe.ApellidoYNombre).SetFontSize(8).SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY), x, y - 80, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                    document.ShowTextAligned(new Paragraph("impreso: ").Add(DateTime.Now.ToString("g")).SetFontSize(8).SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY), x, y - 90, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);

                    #endregion

                    #region pie de pagina

                    document.ShowTextAligned(new Paragraph("SISTEMA PERSONAL").SetFontSize(9).SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY), 15, area.GetBottom(), i, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
                    document.ShowTextAligned(new Paragraph(titulo_informe).Add(" - hoja ").Add(pagina.ToString()).Add(" de ").Add(paginas_doc.ToString()).SetFontSize(9).SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY), x, area.GetBottom(), i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                    pagina++;

                    #endregion
                }

                #endregion

                //Cierro el documento
                document.Close();

                bytesOutput = ms.ToArray();
            }

            return bytesOutput;

        }


    }
}