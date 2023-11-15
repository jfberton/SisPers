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
using SisPer.Aplicativo;
using System.Security.Cryptography;

public class Informe_tardanzas : Informe_Personal<Datos_informe_desde_hasta<TardanzasMes_DS>>
{
    public Informe_tardanzas(Datos_informe_desde_hasta<TardanzasMes_DS> datos, Agente ag_informe) : base(datos, ag_informe, "TARDANZAS")
    {
    }

    protected override stream_informe Generar_cuerpo_informe(Datos_informe_desde_hasta<TardanzasMes_DS> datos_informe, stream_informe informe)
    {
        Document document = informe.document;

        List<string> areas = datos_informe.datos.Agente.Select(x => x.Departamento).Distinct().ToList();


        foreach (string area in areas)
        {
            Paragraph desde_hasta = new Paragraph(String.Format("Tardanzas de agentes de {2}, desde el {0} al {1}", datos_informe.desde.ToShortDateString(), datos_informe.hasta.ToShortDateString(), area))
                   .SetTextAlignment(TextAlignment.LEFT);

            #region datos generales
            Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 15, 85 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true).SetMarginBottom(15);

            #region Encabezado tabla detalle

            Cell cell = new Cell(1, 3).Add(desde_hasta).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            tabla_detalle.AddCell(cell);

            cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
            tabla_detalle.AddCell(cell);
            cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
            tabla_detalle.AddCell(cell);

            #endregion

            #region Carga de valores detalle
            ///Recorro los valores asociados al area y voy cargando en la tabla
            foreach (TardanzasMes_DS.AgenteRow item in datos_informe.datos.Agente.Where(ss => ss.Departamento == area))
            {
                iText.Layout.Borders.Border borde_tabla_gris = new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 1);
                iText.Layout.Borders.Border borde_tabla_negro = new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1);

                cell = new Cell(1, 1).Add(new Paragraph(item.Legajo.ToString()));
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph(item.NombreYApellido));
                tabla_detalle.AddCell(cell);

                Table tabla_detalle_interna = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 20, 20 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true).SetMarginBottom(15);
                cell = new Cell(1, 1).SetBorder(borde_tabla_gris).Add(new Paragraph("FECHA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle_interna.AddCell(cell);
                cell = new Cell(1, 1).SetBorder(borde_tabla_gris).Add(new Paragraph("HORA ENTRADA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle_interna.AddCell(cell);
                cell = new Cell(1, 1).SetBorder(borde_tabla_gris).Add(new Paragraph("TARDANZA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle_interna.AddCell(cell);
                cell = new Cell(1, 2).SetBorder(borde_tabla_gris).Add(new Paragraph("TIEMPO SUPERADO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle_interna.AddCell(cell);

                string horas_tardanza = "00:00";
                bool supero = false;
                int elementos = datos_informe.datos.Tardanza.Where(ss => ss.Legajo == item.Legajo).Count();
                int elementos_recorridos = 0;

                foreach (TardanzasMes_DS.TardanzaRow tardanza in datos_informe.datos.Tardanza.Where(ss => ss.Legajo == item.Legajo).OrderBy(t => t.Fecha))
                {
                    elementos_recorridos++;
                    horas_tardanza = HorasString.SumarHoras(new string[] { horas_tardanza, tardanza.TiempoTardanza });
                    iText.Layout.Borders.Border borde_inferior = borde_tabla_gris;
                    
                    if (elementos == elementos_recorridos)
                    {
                        borde_inferior = borde_tabla_negro;
                    }

                    cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph(tardanza.Fecha.ToShortDateString())).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle_interna.AddCell(cell);
                    cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph(tardanza.HoraEntrada)).SetTextAlignment(TextAlignment.CENTER);
                    tabla_detalle_interna.AddCell(cell);
                    if (supero)
                    {
                        cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph(tardanza.TiempoTardanza)).SetTextAlignment(TextAlignment.CENTER).SetBold(); ;
                        tabla_detalle_interna.AddCell(cell);
                    }
                    else
                    {
                        cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph(tardanza.TiempoTardanza)).SetTextAlignment(TextAlignment.CENTER);
                        tabla_detalle_interna.AddCell(cell);
                    }
                   
                    if (tardanza.PrimerSupero)
                    {
                        cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph("<- Sup. 120'")).SetTextAlignment(TextAlignment.CENTER).SetBold();
                        tabla_detalle_interna.AddCell(cell);
                        supero = true;
                    }
                    else
                    {
                        cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior);
                        tabla_detalle_interna.AddCell(cell);
                    }

                    cell = new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderBottom(borde_inferior).Add(new Paragraph(tardanza.TiempoSuperado)).SetTextAlignment(TextAlignment.CENTER).SetBold();
                    tabla_detalle_interna.AddCell(cell);
                }

                tabla_detalle_interna.AddCell(new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderTop(borde_tabla_negro).Add(new Paragraph("Totales periodo")));
                tabla_detalle_interna.AddCell(new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderTop(borde_tabla_negro).Add(new Paragraph(elementos.ToString()).SetTextAlignment(TextAlignment.CENTER)));
                tabla_detalle_interna.AddCell(new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderTop(borde_tabla_negro).Add(new Paragraph(horas_tardanza).SetTextAlignment(TextAlignment.CENTER).SetBold()));
                tabla_detalle_interna.AddCell(new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderTop(borde_tabla_negro).Add(new Paragraph("")));
                string horas_superadas = "";
                if (supero)
                {
                    horas_superadas = HorasString.RestarHoras(horas_tardanza, "02:00");
                }

                tabla_detalle_interna.AddCell(new Cell(1, 1).SetBorder(borde_tabla_gris).SetBorderTop(borde_tabla_negro).Add(new Paragraph(horas_superadas).SetTextAlignment(TextAlignment.CENTER).SetBold()));

                cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER); 
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(tabla_detalle_interna).SetBorder(iText.Layout.Borders.Border.NO_BORDER); 
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