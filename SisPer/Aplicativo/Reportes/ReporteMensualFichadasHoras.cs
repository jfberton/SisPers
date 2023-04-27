using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.IO.Image;
using Image = iText.Layout.Element.Image;
using System.Configuration;
using iText.Kernel.Pdf.Canvas;
using System.IO;
using System.Web;

namespace SisPer.Aplicativo.Reportes
{
    public class ReporteMensualFichadasHoras
    {
        public List<Agente> Agentes { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }

        public ReporteMensualFichadasHoras(List<Agente> _agentes, int _mes, int _anio)
        {
            Agentes = _agentes;
            Mes = _mes;
            Anio = _anio;
        }

        public Byte[] GenerarPDFAsistenciaMensual()
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
                DateTime primer_dia_mes = new DateTime(Anio, Mes, 1);
                DateTime ultimo_dia_mes = new DateTime(Anio, Mes, 1).AddMonths(1).AddDays(-1);

                foreach (Agente agente in Agentes)
                {
                    if (corrida > 0)//no es el primer agente
                    {
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                    }

                    #region encabezado reporte

                    Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                    document.Add(membrete);

                    Table tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] {15, 80, 15 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5);

                    Cell celldaEncabezado = new Cell(2,1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    celldaEncabezado = new Cell(1, 1).Add(new Paragraph("PLANILLA DE ASISTENCIA")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontSize(12);
                    tableEncabezado.AddCell(celldaEncabezado);
                    
                    celldaEncabezado = new Cell(2, 1).Add(new Paragraph(primer_dia_mes.ToString("MMMM").ToUpper() + " " + primer_dia_mes.ToString("yyyy")))
                                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                        .SetFontSize(10)
                                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    celldaEncabezado = new Cell(1, 1).Add(new Paragraph(agente.Area.Nombre.ToString().ToUpper()))
                                        .SetFontSize(10)
                                        .SetBold()
                                        .SetTextAlignment(TextAlignment.CENTER)
                                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tableEncabezado.AddCell(celldaEncabezado);

                    

                    document.Add(tableEncabezado);

                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 95, 20, 35 })).UseAllAvailableWidth().SetFontSize(10).SetMarginBottom(5);

                    Cell cell = new Cell(1,1).Add(new Paragraph("LEGAJO")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(agente.Legajo.ToString())).SetFontSize(10).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("AGENTE")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(cell);
                    cell = new Cell(1, 3).Add(new Paragraph(agente.ApellidoYNombre.ToString().ToUpper())).SetFontSize(10).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    table.AddCell(cell);

                    document.Add(table);



                    #endregion

                    #region Cuerpo del reporte

                    #region Encabezado tabla horas

                    table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2 })).UseAllAvailableWidth();
                    table.SetFontSize(8);

                    cell = new Cell(2, 1).Add(new Paragraph("DÍA")).SetBold().SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 2).Add(new Paragraph("JORNADA LABORAL")).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 2).Add(new Paragraph("HORARIO VESPERTINO")).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 4).Add(new Paragraph("HORAS DÍA")).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);


                    cell = new Cell(1, 1).Add(new Paragraph("ENTRADA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("SALIDA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("ENTRADA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("SALIDA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("TARDANZA")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("H TRABAJ")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("H DE MÁS")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("H DE MENOS")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(cell);

                    #endregion

                    #region Variables para totales mensuales agente

                    string total_periodo_tardanzas = "000:00";
                    string total_periodo_trabajadas = "000:00";
                    string total_periodo_horas_mas = "000:00";
                    string total_periodo_horas_menos = "000:00";
                    string total_periodo_horas = "000:00";

                    int cantidad_dias_licencia_anual_saldo = 0;
                    int cantidad_dias_licencia_anual = 0;
                    int cantidad_dias_licencia_enfermedad = 0;
                    int cantidad_dias_licencia_invierno = 0;

                    #endregion

                    #region Recorro los dias del mes
                    using (var cxt = new Model1Container())
                    {
                        for (DateTime dia = primer_dia_mes; dia <= ultimo_dia_mes; dia = dia.AddDays(1))
                        {
                            table.AddCell(dia.ToString("ddd dd")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetTextAlignment(TextAlignment.LEFT);

                            cxt.sp_cerrar_dia(agente.Id, dia);

                            ResumenDiario rd = cxt.ResumenesDiarios.Include("Marcaciones").FirstOrDefault(x=>x.AgenteId == agente.Id && x.Dia == dia);

                            if (rd != null && rd.Cerrado == true)
                            {
                                if (rd.Marcaciones != null && rd.Marcaciones.Count == 0)
                                {
                                    if (rd.ObservacionInconsistente == "" || rd.ObservacionInconsistente == null)
                                    {
                                        cell = new Cell(1, 8).Add(new Paragraph("-- NO SE REGISTRAN MOVIMIENTOS --")).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        EstadoAgente ea = agente.ObtenerEstadoAgenteParaElDia(dia, true);
                                        string estado = "";
                                        if (ea != null)
                                        {
                                            estado = ea.TipoEstado.Estado;
                                        }
                                        else
                                        {
                                            estado = rd.ObservacionInconsistente;
                                        }

                                        if (estado != "Franco compensatorio" && estado != "Razones particulares")
                                        {
                                            cell = new Cell(1, 8).Add(new Paragraph(estado)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                                            table.AddCell(cell);
                                        }
                                        else
                                        {
                                            if (estado == "Franco compensatorio")
                                            {
                                                cell = new Cell(1, 7).Add(new Paragraph(estado)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                                                table.AddCell(cell);
                                                table.AddCell("07:00");
                                                total_periodo_horas_menos = HorasString.SumarHoras(new string[] { total_periodo_horas_menos, "07:00" });
                                                total_periodo_horas = HorasString.SumarHoras(new string[] { total_periodo_horas, "-07:00" });
                                            }
                                            else
                                            {
                                                cell = new Cell(1, 7).Add(new Paragraph(estado)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                                                table.AddCell(cell);
                                                table.AddCell("06:30");
                                                total_periodo_horas_menos = HorasString.SumarHoras(new string[] { total_periodo_horas_menos, "06:30" });
                                                total_periodo_horas = HorasString.SumarHoras(new string[] { total_periodo_horas, "-06:30" });
                                            }
                                        }
                                        

                                        switch (estado)
                                        {
                                            case "Licencia Anual":
                                                cantidad_dias_licencia_anual++;
                                                break;
                                            case "Licencia Anual (Saldo)":
                                                cantidad_dias_licencia_anual_saldo++;
                                                break;
                                            case "Enfermedad común":
                                                cantidad_dias_licencia_enfermedad++;
                                                break;
                                            case "Enfermedad familiar":
                                                cantidad_dias_licencia_enfermedad++;
                                                break;
                                            case "Licencia especial invierno":
                                                cantidad_dias_licencia_invierno++;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    table.AddCell(rd.HEntrada);
                                    table.AddCell(rd.HSalida);
                                    table.AddCell(rd.HVEnt);
                                    table.AddCell(rd.HVSal);

                                    MovimientoHora tardanza = cxt.MovimientosHoras.FirstOrDefault(mh => mh.AgenteId == agente.Id && mh.ResumenDiario.Dia == dia && mh.TipoMovimientoHoraId == 1);
                                    MovimientoHora horas_trabajadas = cxt.MovimientosHoras.FirstOrDefault(mh => mh.AgenteId == agente.Id && mh.ResumenDiario.Dia == dia && mh.TipoMovimientoHoraId == 10);
                                    MovimientoHora prolongacion_de_jornada = cxt.MovimientosHoras.FirstOrDefault(mh => mh.AgenteId == agente.Id && mh.ResumenDiario.Dia == dia && mh.TipoMovimientoHoraId == 2);
                                    HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hhvv=>hhvv.AgenteId == agente.Id && hhvv.Dia == dia);

                                    string tardanzaStr = tardanza != null ? tardanza.Horas : "00:00";
                                    string horas_trabajadasStr = HorasString.SumarHoras(new string[] { 
                                                                                horas_trabajadas != null ? horas_trabajadas.Horas : "00:00", //si tiene horas trabajadas del dia
                                                                                (hv != null && hv.Estado == EstadosHorarioVespertino.Terminado) ? hv.Horas : "00:00", //si tiene un HV y este esta terminado lo contabilizo tambien
                                                                                prolongacion_de_jornada != null ? prolongacion_de_jornada.Horas : "00:00", //si tiene prolongación de jornada en el día
                                                                                });
                                    string horas_menosStr = "00:00";
                                    string horas_masStr = "00:00";

                                    if (rd.Horas.Contains("-"))
                                    {
                                        horas_menosStr = rd.Horas.Replace("-", "");
                                    }
                                    else
                                    {
                                        horas_masStr = rd.Horas;
                                    }

                                    total_periodo_horas = HorasString.SumarHoras(new string[] { total_periodo_horas, rd.Horas });

                                    total_periodo_tardanzas = HorasString.SumarHoras(new string[] { total_periodo_tardanzas, tardanzaStr });
                                    total_periodo_trabajadas = HorasString.SumarHoras(new string[] { total_periodo_trabajadas, horas_trabajadasStr });
                                    total_periodo_horas_mas = HorasString.SumarHoras(new string[] { total_periodo_horas_mas, horas_masStr });
                                    total_periodo_horas_menos = HorasString.SumarHoras(new string[] { total_periodo_horas_menos, horas_menosStr });

                                    table.AddCell(tardanzaStr);
                                    table.AddCell(horas_trabajadasStr);
                                    table.AddCell(horas_masStr);
                                    table.AddCell(horas_menosStr);
                                }
                            }
                            else
                            {
                                if (rd != null && rd.Dia <= DateTime.Today)
                                {
                                    

                                    string textoCelda = "DÍA SIN CERRAR: ";
                                    switch (rd.ObservacionInconsistente)
                                    {
                                        case "Ausente.":
                                            SolicitudDeEstado se = cxt.SolicitudesDeEstado.FirstOrDefault(x => x.AgenteId == rd.AgenteId && x.FechaDesde <= rd.Dia && x.FechaHasta >= rd.Dia);
                                            textoCelda += "Ausente. - ";
                                            if (se != null)
                                            {
                                                textoCelda += "SOLICITUD PENDIENTE: " + se.TipoEstadoAgente.Estado;
                                            }
                                            break;

                                        case "Marcaciones impares":
                                            textoCelda += "Marcaciones impares. - MARCACIONES REGISTRADAS: " + rd.Marcaciones.Where(m=>!m.Anulada).Count();
                                            break;

                                        default:
                                            textoCelda += rd.ObservacionInconsistente;
                                            break;
                                    }




                                    cell = new Cell(1, 8).Add(new Paragraph(textoCelda));
                                    table.AddCell(cell);
                                }
                                else
                                {
                                    if (rd != null)
                                    {
                                        cell = new Cell(1, 8);//no muestro nada porque todavia no ocurrio el dia.
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        cell = new Cell(1, 8).Add(new Paragraph("-- NO SE ENCUENTRA EL PROCESO DEL DÍA --"));
                                        table.AddCell(cell);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Agrego el pie de la tabla

                    cell = new Cell(1, 5); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Tot. tardanza").SetBold()); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Tot trabaj.").SetBold()); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Tot. horas +").SetBold()); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph("Tot. horas -").SetBold()); table.AddCell(cell);


                    cell = new Cell(1, 5).Add(new Paragraph("TOTALES DEL PERIODO - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - :")); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(total_periodo_tardanzas)); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(total_periodo_trabajadas)); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(total_periodo_horas_mas)); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(total_periodo_horas_menos)); table.AddCell(cell);
                    cell = new Cell(1, 8).Add(new Paragraph("TOTAL DE HORAS - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - :").SetBold()); table.AddCell(cell);
                    cell = new Cell(1, 1).Add(new Paragraph(total_periodo_horas).SetBold()); table.AddCell(cell);

                    cell = new Cell(1, 9).SetBorder(iText.Layout.Borders.Border.NO_BORDER); table.AddCell(cell);
                    cell = new Cell(1, 9).Add(new Paragraph(String.Format("Días licencia anual: {0}", cantidad_dias_licencia_anual.ToString()))).SetBorder(iText.Layout.Borders.Border.NO_BORDER); table.AddCell (cell);
                    cell = new Cell(1, 9).Add(new Paragraph(String.Format("Días licencia anual (SALDO): {0}", cantidad_dias_licencia_anual_saldo.ToString()))).SetBorder(iText.Layout.Borders.Border.NO_BORDER); table.AddCell(cell);
                    cell = new Cell(1, 9).Add(new Paragraph(String.Format("Días licencia especial invierno: {0}", cantidad_dias_licencia_invierno.ToString()))).SetBorder(iText.Layout.Borders.Border.NO_BORDER); table.AddCell(cell);
                    cell = new Cell(1, 9).Add(new Paragraph(String.Format("Días licencia enfermedad: {0}", cantidad_dias_licencia_enfermedad.ToString()))).SetBorder(iText.Layout.Borders.Border.NO_BORDER); table.AddCell(cell);


                    #endregion

                    document.Add(table);

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
