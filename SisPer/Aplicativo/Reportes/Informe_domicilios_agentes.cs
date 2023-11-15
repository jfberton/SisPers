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
    public class Informe_domicilios_agentes:Informe_Personal<List<Agente>>
    {
        public Informe_domicilios_agentes(List<Agente> datos, Agente ag_informe) : base(datos, ag_informe, "DOMICILIOS DE AGENTES")
        {
        }

        protected override stream_informe Generar_cuerpo_informe(List<Agente> datos, stream_informe informe)
        {
            Document document = informe.document;
            

            var nombresAreas = datos.Select(a => a.Area.Nombre).Distinct();

            foreach (string nombreArea in nombresAreas)
            {
                #region datos generales
                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 80, 80, 60 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true).SetMarginBottom(15);

                #region Encabezado tabla detalle
                Cell cell = new Cell(1, 4).Add(new Paragraph("Agentes de " + nombreArea)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
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
                foreach (Agente item in datos.Where(x => x.Area.Nombre == nombreArea))
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

            }

            informe.document = document;

            return informe;
        }
    }
}
