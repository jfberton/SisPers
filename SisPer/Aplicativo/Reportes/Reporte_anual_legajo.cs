using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SisPer.Aplicativo.Reportes
{
    public class Reporte_anual_legajo
    {
        public List<Agente> agentes { get; set; }
        public int year { get; set; }

        public Reporte_anual_legajo(List<Agente> _agentes, int _anio)
        {
            agentes = _agentes;
            year = _anio;
        }

        public Byte[] Generar_pdf_legajo_anual()
        {
            Byte[] res = null;

            using (MemoryStream ms = new MemoryStream())
            {
                //Genero el PDF en memoria para ir agregando las partes
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);

                //PageOrientationsEventHandler eventHandler = new PageOrientationsEventHandler();
                //pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);
                Document document = new Document(pdf, PageSize.LEGAL.Rotate(), false);

                float puntos_por_centimetro = (float)20;

                float margenSuperior = ((float)(1 * puntos_por_centimetro));
                float margenInferior = ((float)(1 * puntos_por_centimetro));
                float margenDerecho = ((float)(1.5 * puntos_por_centimetro));
                float margenIzquierdo = ((float)(1.5 * puntos_por_centimetro));

                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                document.SetFontSize(8);


                int corrida = 0;

                foreach (Agente agente in agentes)
                {
                    if (corrida > 0)//no es el primer agente
                    {
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                        document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                    }
                    Agente _agente;
                    using (var cxt = new Model1Container())
                    {
                        _agente = cxt.Agentes.Include("Legajo_datos_personales").Include("Legajo_datos_laborales").FirstOrDefault(aa => aa.Id == agente.Id);
                    }

                    #region encabezado reporte

                    int tamaño_texto_celdas_encabezado = 10;

                    Table tabla_encabezado = new Table(UnitValue.CreatePercentArray(new float[] { 45, 15, 30, 10 })).UseAllAvailableWidth().SetMarginBottom(0);
                    tabla_encabezado.SetRotationAngle(Math.PI);

                    Cell celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    Paragraph p = new Paragraph();
                    Text t1 = new Text("Jurisdicción: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado + 4);
                    p.Add(t1);
                    Text t2 = new Text("20 ADMINISTRACIÓN TRIBUTARIA PROVINCIAL");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado + 4).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    p = new Paragraph();
                    t1 = new Text("AÑO " + year.ToString());
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD))
                    t1.SetFontSize(25).SetBold();
                    p.Add(t1);
                    celda_encabezado = new Cell(5, 1).Add(p).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("L E G A J O  N° : ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado + 5).SetBold();
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo.ToString());
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado + 5).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p).SetTextAlignment(TextAlignment.CENTER);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Apellido y Nombre: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.ApellidoYNombre);
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);


                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Cargo: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text("..................................................................");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado - 1).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Apartado: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text("..................");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado - 1).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Clase: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_personales!= null? _agente.Legajo_datos_personales.FechaNacimiento.Year.ToString():"........");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    t1 = new Text(" DNI: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_personales != null ? _agente.Legajo_datos_personales.DNI : "..................");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Fecha de ingreso a la repartición: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_laborales.FechaIngresoATP.ToString("dd 'de' MMMM 'de' yyyy"));
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);


                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Domicilio: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_personales != null ? _agente.Legajo_datos_personales.Domicilio + " - " + _agente.Legajo_datos_personales.Domicilio_localidad : "........................");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);


                    celda_encabezado = new Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER) ;
                    p = new Paragraph();
                    t1 = new Text("Fecha de ingreso a la administración pública: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_laborales.FechaIngresoAminPub.ToString("dd 'de' MMMM 'de' yyyy"));
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);


                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Teléfono: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text("............................");
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);


                    celda_encabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    p = new Paragraph();
                    t1 = new Text("Ficha médica: ");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 1);
                    p.Add(t1);
                    t2 = new Text(_agente.Legajo_datos_laborales.FichaMedica);
                    //t2.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD));
                    t2.SetFontSize(tamaño_texto_celdas_encabezado).SetBold();
                    p.Add(t2);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    celda_encabezado = new Cell(1, 1);
                    p = new Paragraph();
                    t1 = new Text("LICENCIA P/ENFERMEDAD");
                    //t1.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA));
                    t1.SetFontSize(tamaño_texto_celdas_encabezado - 3);
                    p.Add(t1);
                    celda_encabezado.Add(p);
                    tabla_encabezado.AddCell(celda_encabezado);

                    document.Add(tabla_encabezado);

                    #endregion


                    #region tabla central

                    Table tabla_central = new Table(UnitValue.CreatePercentArray(new float[] { 20,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,20,20,20 })).UseAllAvailableWidth().SetMarginBottom(0);
                    Cell tr = new Cell(1, 1).Add(new Paragraph("MESES").SetBold().SetFontSize(tamaño_texto_celdas_encabezado-1)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tabla_central.AddCell(tr);

                    for (int i = 1; i <= 31; i++)
                    {
                        tr = new Cell(1, 1).Add(new Paragraph(i.ToString()).SetBold().SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                        tabla_central.AddCell(tr);
                    }

                    tr = new Cell(1, 1).Add(new Paragraph("TOTAL DIAS COMPUT.").SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("Común y/o familiar").SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("Tratamiento Prolongado").SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tabla_central.AddCell(tr);

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        tr = new Cell(1, 1).Add(new Paragraph(Mes(mes)).SetBold().SetFontSize(tamaño_texto_celdas_encabezado + 3)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                        tabla_central.AddCell(tr);

                        for (int dia = 1; dia <= 31; dia++)
                        {
                            DateTime fecha = DateTime.MinValue;

                            try
                            {
                                fecha = new DateTime(year, mes, dia);
                            }
                            catch 
                            {
                                //nada, aca no hago nada, si tiró error queda con la fecha minima
                            }

                            if (fecha != DateTime.MinValue)
                            {
                                if (fecha.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    bool feriado = BuscarFeriado(fecha);
                                    if (feriado)
                                    {
                                        tr = new Cell(1, 1).Add(new Paragraph("SF").SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                        tabla_central.AddCell(tr);
                                    }
                                    else
                                    {
                                        tr = new Cell(1, 1).Add(new Paragraph("S").SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                        tabla_central.AddCell(tr);
                                    }
                                }
                                else
                                {
                                    if (fecha.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        bool feriado = BuscarFeriado(fecha);
                                        if (feriado)
                                        {
                                            tr = new Cell(1, 1).Add(new Paragraph("DF").SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                            tabla_central.AddCell(tr);
                                        }
                                        else
                                        {
                                            tr = new Cell(1, 1).Add(new Paragraph("D").SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                            tabla_central.AddCell(tr);
                                        }
                                    }
                                    else
                                    {
                                        bool feriado = BuscarFeriado(fecha);
                                        if (feriado)
                                        {
                                            tr = new Cell(1, 1).Add(new Paragraph("F").SetFontSize(tamaño_texto_celdas_encabezado)).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                                            tabla_central.AddCell(tr);
                                        }
                                        else
                                        {
                                            tr = new Cell(1, 1);
                                            tabla_central.AddCell(tr);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                tr = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                                tabla_central.AddCell(tr);
                            }
                        }

                        //TOTAL DIAS COMPUT.
                        tr = new Cell(1, 1);
                        tabla_central.AddCell(tr);

                        //Común y/o familiar
                        tr = new Cell(1, 1);
                        tabla_central.AddCell(tr);

                        //Tratamiento Prolongado
                        tr = new Cell(1, 1);
                        tabla_central.AddCell(tr);
                    }

                    tr = new Cell(1, 25).Add(new Paragraph("SIGLAS DE INASISTENCIAS E IMPUNTUALIDADES, INFRACCIONES Y SANCIONES").SetFontSize(12)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT).SetVerticalAlignment(VerticalAlignment.BOTTOM);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 7).Add(new Paragraph("TOTAL ACUMULADO").SetFontSize(12)).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 1);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 1);
                    tabla_central.AddCell(tr);

                    tr = new Cell(1, 1);
                    tabla_central.AddCell(tr);

                    document.Add(tabla_central);

                    #endregion

                    #region Tabla siglas

                    Table tabla_siglas = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 20, 20 })).UseAllAvailableWidth().SetMarginBottom(0);
                    
                    tr = new Cell(1, 1).Add(new Paragraph("1) L/A - Licencia Anual").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("6) IMP - Impuntualidad").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("11) CASAM - Casamiento").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("16) F/C - Franco Comp.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("21) A/A/C - Act. Art. y Cul.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("2) E/C - Enfermedad Común").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("7) L/EX - Licencia p/examen").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("12) EXTR - Lic. Raz. Part.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("17) R/P - Raz. Part. Rep. Hs.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("22) A/D - Act. Deportiva").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("3) E/F - Enfermedad Familiar").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("8) L/MAT - Licencia p/Matrimonio").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("13) S.M. - Servicio Militar").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("18) E/I/G Est. Int. General").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("23) R/G Represent. Gremial").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("4) TR/PR - Tratamiento Prolongado").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("9) Nac. - Por Nacimiento Hijo").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("14) SUSP - Suspensión").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("19) E/I/P - Est. Int. Partic.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("24) D/S - Donación Sangre").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    tr = new Cell(1, 1).Add(new Paragraph("5) INAS - Inasistencias (1)").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("10) MAT. - Matrim. Hijo Agente").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("15) F - Feriado").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 1).Add(new Paragraph("20) E/P - Examen Psicofisiológico").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(2, 1).Add(new Paragraph("25) L/E/P - s/g/haberes - Enf. Prolong. sin goce de haberes").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);


                    tr = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 3).Add(new Paragraph("Consignar en el día respectivo la sigla que coresponda. Cuando exista juatificativo agregar \"j\".").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    tr = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);
                    tr = new Cell(1, 4).Add(new Paragraph("En caso de licencia consignar el año de las mismas. En impuntualidad poner además el tiempo. (1) - En todos los casos sin goce de haberes.").SetFontSize(tamaño_texto_celdas_encabezado - 1)).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tabla_siglas.AddCell(tr);

                    document.Add(tabla_siglas);

                    #endregion


                    corrida++;
                }

                document.Flush();

                document.Close();

                res = ms.ToArray();
            }

            return res;
        }

        private bool BuscarFeriado(DateTime fecha)
        {
            bool ret = false;
            using (var cxt = new Model1Container())
            {
                ret = cxt.Feriados.FirstOrDefault(ff => ff.Dia == fecha) != null;
            }

            return ret;
        }

        public string Mes(int nroMes)
        {
            string ret = "No existe mes";
            switch (nroMes)
            {
                case 1:
                    ret = "Enero";
                    break;
                case 2:
                    ret = "Febrero";
                    break;
                case 3:
                    ret = "Marzo";
                    break;
                case 4:
                    ret = "Abril";
                    break;
                case 5:
                    ret = "Mayo";
                    break;
                case 6:
                    ret = "Junio";
                    break;
                case 7:
                    ret = "Julio";
                    break;
                case 8:
                    ret = "Agosto";
                    break;
                case 9:
                    ret = "Septiembre";
                    break;
                case 10:
                    ret = "Octubre";
                    break;
                case 11:
                    ret = "Noviembre";
                    break;
                case 12:
                    ret = "Diciembre";
                    break;
                default:
                    break;
            }

            return ret;
        }

    }
}

