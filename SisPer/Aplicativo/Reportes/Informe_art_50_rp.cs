﻿using iText.Kernel.Pdf;
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
    public class Informe_art_50_rp: Informe_Personal<Datos_informe_desde_hasta<ListadoSalidas_DS>>
    {
        public Informe_art_50_rp(Datos_informe_desde_hasta<ListadoSalidas_DS> datos, Agente ag_informe) : base(datos, ag_informe, "SOLICITUDES ART 50 INC 4")
        {
        }

        protected override stream_informe Generar_cuerpo_informe(Datos_informe_desde_hasta<ListadoSalidas_DS> datos, stream_informe informe)
        {
            Document document = informe.document;

            foreach (ListadoSalidas_DS.GeneralRow dr in datos.datos.General)
            {
                Paragraph desde_hasta = new Paragraph(String.Format("Art. 50 inc. 4 de agentes de {2}, desde el {0} al {1}", datos.desde.ToShortDateString(), datos.hasta.ToShortDateString(), dr.Sector))
                   .SetTextAlignment(TextAlignment.LEFT);


                #region datos generales
                Table tabla_detalle = new Table(UnitValue.CreatePercentArray(new float[] { 20, 80, 20, 40 })).UseAllAvailableWidth().SetFontSize(10).SetKeepTogether(true);

                #region Encabezado tabla detalle
                
                Cell cell = new Cell(1, 4).Add(desde_hasta).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("LEGAJO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("FECHA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);
                cell = new Cell(1, 1).Add(new Paragraph("MOTIVO")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                tabla_detalle.AddCell(cell);

                #endregion

                #region Carga de valores detalle
                ///Recorro los valores asociados al area y voy cargando en la tabla
                foreach (ListadoSalidas_DS.SalidasRow item in datos.datos.Salidas.Where(ss => ss.Sector == dr.Sector))
                {
                    cell = new Cell(1, 1).Add(new Paragraph(item.Legajo));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Agente));
                    tabla_detalle.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(item.Fecha));
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
