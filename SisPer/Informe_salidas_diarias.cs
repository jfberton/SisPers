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
using SisPer.Aplicativo.Reportes;
using SisPer.Aplicativo;

namespace SisPer
{
    public class Informe_salidas_diarias : Informe_Personal<Datos_informe_desde_hasta<ListadoSalidas_DS>>
    {
        public Informe_salidas_diarias(Datos_informe_desde_hasta<ListadoSalidas_DS> datos, Agente ag_informe) : base(datos, ag_informe, "SALIDAS DIARIAS")
        {
        }

        protected override stream_informe Generar_cuerpo_informe(Datos_informe_desde_hasta<ListadoSalidas_DS> datos, stream_informe informe)
        {
            Document document = informe.document;

            foreach (ListadoSalidas_DS.GeneralRow dr in datos.datos.General)
            {
                Paragraph desde_hasta = new Paragraph(String.Format("Agentes de {2}, desde el {0} al {1}", datos.desde.ToShortDateString(), datos.hasta.ToShortDateString(), dr.Sector))
                   .SetTextAlignment(TextAlignment.LEFT);

                #region datos generales
                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 60, 20, 20, 40, 60 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true).SetMarginBottom(15);

                #region Encabezado tabla detalle

                Cell cell = new Cell(1, 7).Add(desde_hasta).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_detalle.AddCell(cell);

                cell = new Cell(1, 1).Add(new Paragraph("FECHA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("DESDE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("HASTA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("TIPO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("DESTINO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);

                #endregion

                #region Carga de valores detalle
                ///Recorro los valores asociados al area y voy cargando en la tabla
                foreach (ListadoSalidas_DS.SalidasRow item in datos.datos.Salidas.Where(ss => ss.Sector == dr.Sector))
                {
                    cell = new Cell(1, 1).Add(new Paragraph(item.Fecha));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Legajo));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Agente));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Desde));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Hasta));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Tipo));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Destino));
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