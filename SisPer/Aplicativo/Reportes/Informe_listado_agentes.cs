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
using iText.Kernel.Pdf.Canvas.Draw;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Reportes
{
    internal class Informe_listado_agentes : Informe_Personal<ListadoAgentes_DS>
    {
        public Informe_listado_agentes(ListadoAgentes_DS datos, Agente ag_informe) : base(datos, ag_informe, titulo: "LISTADO DE AGENTES")
        {
        }

        protected override stream_informe Generar_cuerpo_informe(ListadoAgentes_DS datos, stream_informe i)
        {
            //Genero el PDF en memoria para ir agregando las partes
            Document document = i.document;

            int corrida = 0;

            List<string> areas = datos.Area.Select(c => c.Nombre).Distinct().ToList();
            List<ListadoAgentes_DS.AgentesRow> agentes_area = new List<ListadoAgentes_DS.AgentesRow>();

            LineSeparator linea = new LineSeparator(new SolidLine(1f));
            linea.SetMarginTop(1);
            linea.SetMarginBottom(2);

            foreach (string area in areas)
            {
                #region Encabezado Area

                int dependencias = datos.Area.First(aa => aa.Nombre == area).CantidadAreasSubordinadas;
                int agentes = datos.Area.First(aa => aa.Nombre == area).CantidadAgentes;
                string dependeDe = datos.Area.First(aa => aa.Nombre == area).Depende_de;
                agentes_area = datos.Agentes.Where(a => a.Area == area).ToList();
                iText.Layout.Element.Table table_area = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 20, 80 }));
                table_area.SetKeepTogether(true);

                Text t_agentes = new Text("Agentes: ").SetBold();
                Text t_agentes_valor = new Text(agentes.ToString());
                Text t_dependencias = new Text("Dependencias: ").SetBold();
                Text t_dependencias_valor = new Text(dependencias.ToString());
                Text t_depende_de = new Text("Depende de: ").SetBold();
                Text t_depende_de_valor = new Text(dependeDe);

                Cell cell_area = new Cell(1, 2).Add(new Paragraph(area)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1));
                table_area.AddCell(cell_area);

                cell_area = new Cell(1, 2).Add(new Paragraph(t_agentes).Add(t_agentes_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_area.AddCell(cell_area);

                cell_area = new Cell(1, 2).Add(new Paragraph(t_dependencias).Add(t_dependencias_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_area.AddCell(cell_area);

                cell_area = new Cell(1, 2).Add(new Paragraph(t_depende_de).Add(t_depende_de_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_area.AddCell(cell_area);

                #endregion

                #region Encabezado tabla

                iText.Layout.Element.Table table_agente = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 30, 15, 30, 80 })).UseAllAvailableWidth().SetFontSize(10).SetMarginBottom(10);

                Cell cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_agente.AddCell(cell);

                cell = new Cell(1, 1).Add(new Paragraph("Legajo").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_agente.AddCell(cell);

                cell = new Cell(1, 1).Add(new Paragraph("D.N.I.").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_agente.AddCell(cell);

                cell = new Cell(1, 1).Add(new Paragraph("Apellido y nombre").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_agente.AddCell(cell);

                #endregion

                foreach (var agente in agentes_area)
                {
                    if (agente.Jefe)
                    {
                        cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.Legajo).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.DNI).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.Nombre).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);
                    }
                    else
                    {
                        cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.Legajo)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.DNI)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);

                        cell = new Cell(1, 1).Add(new Paragraph(agente.Nombre)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table_agente.AddCell(cell);
                    }

                }

                cell_area = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_area.AddCell(cell_area);

                cell_area = new Cell(1, 1).Add(table_agente).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                table_area.AddCell(cell_area);

                document.Add(table_area);
                corrida++;
            }

            i.document = document;

            //document.Flush();

            //document.Close();

            return i;
        }
    }

    /*
      public ListadoAgentes_DS datos { get; set; }
        public Agente ag { get; set; }

        public Informe_listado_agentes(ListadoAgentes_DS _datos, Agente _agente_impresion)
        {
            datos = _datos;
            ag = _agente_impresion;
        }

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

                float margenSuperior = ((float)(5 * puntos_por_centimetro));
                float margenInferior = ((float)(2 * puntos_por_centimetro));
                float margenDerecho = ((float)(1 * puntos_por_centimetro));
                float margenIzquierdo = ((float)(3 * puntos_por_centimetro));

                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                document.SetFontSize(12);

                int corrida = 0;

                List<string> areas = datos.Area.Select(c => c.Nombre).Distinct().ToList();
                List<ListadoAgentes_DS.AgentesRow> agentes_area = new List<ListadoAgentes_DS.AgentesRow>();


              
                 
                    //#region encabezado reporte

                    //    Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                    //    document.Add(membrete);

                    //    Paragraph fecha_hora_impresion = new Paragraph().Add(new Text(String.Format("Fecha hora impresión: {0}", DateTime.Now)).SetFontSize(7)).SetMarginBottom(0);
                    //    document.Add(fecha_hora_impresion.SetTextAlignment(TextAlignment.RIGHT));

                    //    Paragraph agente_impresion = new Paragraph().Add(new Text(String.Format("Por: {1} ({0})", ag.Legajo, ag.ApellidoYNombre)).SetFontSize(7));
                    //    document.Add(agente_impresion.SetTextAlignment(TextAlignment.RIGHT));

                    //    Table tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] { 15, 80, 15 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5);

                    //    Cell celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    //    tableEncabezado.AddCell(celldaEncabezado);

                    //    celldaEncabezado = new Cell(1, 1).Add(new Paragraph("LISTADO DE AGENTE POR UNIDAD ORGANIZATIVA")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontSize(12);
                    //    tableEncabezado.AddCell(celldaEncabezado);

                    //    celldaEncabezado = new Cell(2, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    //    tableEncabezado.AddCell(celldaEncabezado);

                    //    celldaEncabezado = new Cell(1, 1).Add(new Paragraph(area.ToUpper()))
                    //                        .SetFontSize(10)
                    //                        .SetBold()
                    //                        .SetTextAlignment(TextAlignment.CENTER)
                    //                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    //    tableEncabezado.AddCell(celldaEncabezado);

                    //    document.Add(tableEncabezado);

                    //#endregion
                 
          


    LineSeparator linea = new LineSeparator(new SolidLine(1f));
    linea.SetMarginTop(1);
                linea.SetMarginBottom(2);

                foreach (string area in areas)
                {
                    #region Encabezado Area

                    int dependencias = datos.Area.First(aa => aa.Nombre == area).CantidadAreasSubordinadas;
    int agentes = datos.Area.First(aa => aa.Nombre == area).CantidadAgentes;
    string dependeDe = datos.Area.First(aa => aa.Nombre == area).Depende_de;
    agentes_area = datos.Agentes.Where(a => a.Area == area).ToList();
    iText.Layout.Element.Table table_area = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 20, 80 }));
    table_area.SetKeepTogether(true);
                    //document.Add(new Paragraph(area).SetMarginBottom(1));
                    //document.Add(linea);

                    Text t_agentes = new Text("Agentes: ").SetBold();
    Text t_agentes_valor = new Text(agentes.ToString());
    Text t_dependencias = new Text("Dependencias: ").SetBold();
    Text t_dependencias_valor = new Text(dependencias.ToString());
    Text t_depende_de = new Text("Depende de: ").SetBold();
    Text t_depende_de_valor = new Text(dependeDe);

    Cell cell_area = new Cell(1, 2).Add(new Paragraph(area)).SetBorderBottom(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 2));
    table_area.AddCell(cell_area);

                    cell_area = new Cell(1, 2).Add(new Paragraph(t_agentes).Add(t_agentes_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_area.AddCell(cell_area);

                    cell_area = new Cell(1, 2).Add(new Paragraph(t_dependencias).Add(t_dependencias_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_area.AddCell(cell_area);

                    cell_area = new Cell(1, 2).Add(new Paragraph(t_depende_de).Add(t_depende_de_valor)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_area.AddCell(cell_area);

                    #endregion

                    #region Encabezado tabla

                    iText.Layout.Element.Table table_agente = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 30, 15, 30, 80 })).UseAllAvailableWidth().SetFontSize(10).SetMarginBottom(10);

    Cell cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph("Legajo").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph("D.N.I.").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph("Apellido y nombre").SetBold()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                    #endregion

                    foreach (var agente in agentes_area)
                    {
                        if (agente.Jefe)
                        {
                            cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                            cell = new Cell(1, 1).Add(new Paragraph(agente.Legajo).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                            cell = new Cell(1, 1).Add(new Paragraph(agente.DNI).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

                            cell = new Cell(1, 1).Add(new Paragraph(agente.Nombre).SetBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);
                        }
                        else
{
    cell = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

    cell = new Cell(1, 1).Add(new Paragraph(agente.Legajo)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

    cell = new Cell(1, 1).Add(new Paragraph(agente.DNI)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);

    cell = new Cell(1, 1).Add(new Paragraph(agente.Nombre)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
    table_agente.AddCell(cell);
}

                    }

                    cell_area = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
table_area.AddCell(cell_area);

cell_area = new Cell(1, 1).Add(table_agente).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
table_area.AddCell(cell_area);

document.Add(table_area);
corrida++;
                }

                document.Flush();

document.Close();

res = ms.ToArray();
            }

            return res;
        }

     
     */
}