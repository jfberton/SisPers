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
using System.Security.Cryptography;

namespace SisPer.Aplicativo.Reportes
{
    public class Informe_ausentes : Informe_Personal<Datos_informe_desde_hasta<listado_ausentes_ds>>
    {
        public Informe_ausentes(Datos_informe_desde_hasta<listado_ausentes_ds> datos, Agente ag_informe) : base(datos, ag_informe, "LISTADO DE AUSENCIAS")
        {
        }

        protected override stream_informe Generar_cuerpo_informe(Datos_informe_desde_hasta<listado_ausentes_ds> datos, stream_informe informe)
        {
            Document document = informe.document;

            foreach (listado_ausentes_ds.EncabezadoRow dr in datos.datos.Encabezado)
            {
                Paragraph desde_hasta = new Paragraph(String.Format("{0}, desde {1} al {2}", dr.Filtro, datos.desde.ToShortDateString(), datos.hasta.ToShortDateString()))
                   .SetTextAlignment(TextAlignment.LEFT);


                #region datos generales
                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 80, 40 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true);

                #region Encabezado tabla detalle

                Cell cell = new Cell(1, 4).Add(desde_hasta).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("FECHA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("MOTIVO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);

                #endregion

                #region Carga de valores detalle
                ///Recorro los valores asociados al area y voy cargando en la tabla
                foreach (listado_ausentes_ds.DetalleRow item in datos.datos.Detalle)
                {
                    cell = new Cell(1, 1).Add(new Paragraph(item.Fecha));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Legajo));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Agente));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Motivo));
                    tabla_detalle.AddCell(cell);
                }

                document.Add(tabla_detalle);

                #endregion

                #endregion

            }

            informe.document = document;

            return informe;
        }
    }
}