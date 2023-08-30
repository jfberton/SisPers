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
using iText.Kernel.Pdf.Canvas.Draw;

namespace SisPer.Aplicativo.Reportes
{
    public class Informe_solicitud_medico
    {
        public SolicitudDeEstado solicitud { get; set; }
        public string actuacion_electronica { get; set; }

        //Obtiene el turno del agente que efectua la solicitud, si el horario de entrada es menor a las 13:00 hs es MATUTINO sino VESPERTINO
        private string ObtenerTurno()
        {
            string turno = "MATUTINO";
            if (HorasString.AMayorQueB(solicitud.Agente.HoraEntrada, "13:00"))
            {
                turno = "VESPERTINO";
            }
            return turno;
        }

        private string ObtenerSanatorioInternacion()
        {
            string lugar = solicitud.Lugar;
            string[] lugares = lugar.Replace("Sanatorio ", "").Replace(" habitación ", ";").Split(';');

            return lugares[0];
        }
        private string ObtenerHabitacionInternacion()
        {
            string lugar = solicitud.Lugar;
            string[] lugares = lugar.Replace("Sanatorio ", "").Replace(" habitación ", ";").Split(';');

            return lugares[1];
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
                float margenSuperior = ((float)(0.5 * puntos_por_centimetro));
                float margenInferior = ((float)(2 * puntos_por_centimetro));
                float margenDerecho = ((float)(1 * puntos_por_centimetro));
                float margenIzquierdo = ((float)(3 * puntos_por_centimetro));
                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                document.SetFontSize(10);
                #region Hoja ATP

                #region encabezado reporte

                Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                document.Add(membrete);


                Table tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] { 5, 40, 30, 40, 5 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5).SetMarginTop(5);

                Cell celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 3).Add(new Paragraph().Add(new Text(String.Format("Actuación Electrónica {0}", actuacion_electronica)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph().Add(new Text(String.Format("FICHA MEDICA N° {0}", solicitud.Agente.Legajo_datos_laborales.FichaMedica)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph().Add(new Text(String.Format("LEGAJO N° {0}", solicitud.Agente.Legajo)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                document.Add(tableEncabezado);

                SolidLine line = new SolidLine();
                document.Add(new LineSeparator(line).SetMarginTop(5).SetMarginBottom(5));

                Paragraph reparticion = new Paragraph();
                reparticion.Add(new Text("REPARTICIÓN: ").SetFontSize(10));
                reparticion.Add(new Text("JUR 20 - ADMINISTRACIÓN TRIBUTARIA PROVINCIAL").SetBold().SetUnderline().SetFontSize(12));
                document.Add(reparticion);

                #endregion

                #region Cuerpo del informe

                Paragraph fecha_impresion = new Paragraph().SetMarginTop(7).SetTextAlignment(TextAlignment.RIGHT);
                fecha_impresion.Add(new Text(string.Format("RESISTENCIA, {0}", DateTime.Today.ToLongDateString())).SetFontSize(12));
                document.Add(fecha_impresion);

                Paragraph contenido = new Paragraph().SetFirstLineIndent(40);
                contenido.Add(new Text("Solicito que practique reconocimiento médico a ").SetFontSize(10));
                contenido.Add(new Text(string.Format("{0}, DNI N° {1}", solicitud.Agente.ApellidoYNombre, solicitud.Agente.Legajo_datos_personales.DNI)).SetBold().SetFontSize(10));
                contenido.Add(new Text(", que presta servicios en el horario ").SetFontSize(10));
                contenido.Add(new Text(string.Format("{0}", ObtenerTurno())).SetBold().SetFontSize(10));
                contenido.Add(new Text(". Con domicilio en ").SetFontSize(10));
                contenido.Add(new Text(string.Format("{0} - {1}", solicitud.Agente.Legajo_datos_personales.Domicilio, solicitud.Agente.Legajo_datos_personales.Domicilio_localidad)).SetBold().SetFontSize(10));
                contenido.Add(new Text(" – Chaco, e inasiste desde el  ").SetFontSize(10));
                contenido.Add(new Text(string.Format("{0}", solicitud.FechaDesde.ToString("dd/MM/yyyy"))).SetBold().SetFontSize(10));

                document.Add(contenido);

                Paragraph tipo = new Paragraph().SetMarginTop(5).SetFirstLineIndent(40);
                tipo.Add(new Text("El pedido es a "));
                switch ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad)
                {
                    case TipoMovimientoEnfermedad.Consultorio:
                        tipo.Add(new Text(string.Format("{0}.", ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad).ToString())).SetBold());
                        document.Add(tipo);
                        break;
                    case TipoMovimientoEnfermedad.Domicilio:
                        tipo.Add(new Text(string.Format("{0}.", ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad).ToString())).SetBold());
                        document.Add(tipo);
                        break;
                    case TipoMovimientoEnfermedad.Internacion:
                        tipo.Add(new Text(string.Format("Internación:")).SetBold());
                        document.Add(tipo);
                        
                        Paragraph p_sanatorio = new Paragraph();
                        p_sanatorio.Add(new Text("Sanatorio: "));
                        p_sanatorio.Add(new Text(string.Format("{0}", ObtenerSanatorioInternacion())).SetBold());

                        Paragraph p_habitacion = new Paragraph();
                        p_habitacion.Add(new Text("Habitación: "));
                        p_habitacion.Add(new Text(string.Format("{0}", ObtenerHabitacionInternacion())).SetBold());
                        
                        Table table_internacion = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth().SetFontSize(10).SetMarginBottom(5).SetMarginTop(5).SetMarginLeft(40);
                        Cell celda_sanatorio = new Cell(1, 1).Add(p_sanatorio).SetMargin(5);
                        Cell celda_habitacion = new Cell(1, 1).Add(p_habitacion).SetMargin(5);

                        table_internacion.AddCell(celda_sanatorio);
                        table_internacion.AddCell(celda_habitacion);
                        
                        document.Add(table_internacion);
                        
                        break;
                    case TipoMovimientoEnfermedad.Consultorio_a_su_regreso_de:
                        tipo.Add(new Text("Consultorio a su regreso de: "));
                        tipo.Add(new Text(string.Format("{0}", solicitud.Lugar)).SetBold());
                        document.Add(tipo);
                        break;
                    default:
                        break;
                }

                if (solicitud.TipoEstadoAgente.Estado == "Enfermedad familiar")
                { 
                    Paragraph familiar = new Paragraph().SetMarginTop(5).SetFirstLineIndent(40);
                    familiar.Add(new Text("Familiar enfermo: "));
                    document.Add(familiar);

                    Table t_familiar = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth().SetFontSize(10).SetMarginBottom(5).SetMarginTop(5).SetMarginLeft(40);
                    
                    Cell cell = new Cell(1, 1).Add(new Paragraph().Add(new Text("Apellido y Nombre").SetFontSize(10)));
                    t_familiar.AddCell(cell);
                   
                    cell = new Cell(1, 1).Add(new Paragraph().Add(new Text(string.Format("{0}", solicitud.Fam_NomyAp)).SetBold().SetFontSize(10)));
                    t_familiar.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph().Add(new Text("Parentesco con el agente")));
                    t_familiar.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph().Add(new Text(string.Format("{0}", solicitud.Fam_Parentesco)).SetBold().SetFontSize(10)));
                    t_familiar.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph().Add(new Text("Domicilio en")));
                    t_familiar.AddCell(cell);

                    cell = new Cell(1, 1).Add(new Paragraph().Add(new Text(string.Format("{0} - {1}", solicitud.Agente.Legajo_datos_personales.Domicilio, solicitud.Agente.Legajo_datos_personales.Domicilio_localidad)).SetBold().SetFontSize(10)));
                    t_familiar.AddCell(cell);

                    document.Add(t_familiar);
                }   

                Paragraph encuadres = new Paragraph();
                encuadres.Add(new Text("LICENCIAS DIAS  y MOTIVOS:").SetFontSize(10)).SetMarginTop(10);
                document.Add(encuadres);

                encuadres = new Paragraph();
                encuadres.Add(new Text("14 “A”   E/C    14 “B”  Tto Prolong     14 “C”  Acid Trab    14 “D”  Atenc Fliar    14 “E”  Enf Grave Fliar   "));
                document.Add(encuadres);

                document.Add(new LineSeparator(line).SetMarginTop(5).SetMarginBottom(5));

                Image sello = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/sello.jpg"))).ScaleAbsolute(100, 100).SetMarginTop(20);
                document.Add(sello.SetHorizontalAlignment(HorizontalAlignment.CENTER));

                
                

                #endregion

                #endregion

                #region Hoja salud

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);

                #region encabezado reporte

                membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);
                document.Add(membrete);


                tableEncabezado = new Table(UnitValue.CreatePercentArray(new float[] { 5, 40, 30, 40, 5 })).UseAllAvailableWidth().SetFontSize(8).SetMarginBottom(5).SetMarginTop(5);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 3).Add(new Paragraph().Add(new Text(String.Format("Actuación Electrónica {0}", actuacion_electronica)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph().Add(new Text(String.Format("FICHA MEDICA N° {0}", solicitud.Agente.Legajo_datos_laborales.FichaMedica)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).Add(new Paragraph().Add(new Text(String.Format("LEGAJO N° {0}", solicitud.Agente.Legajo)).SetFontSize(12).SetBold())).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                celldaEncabezado = new Cell(1, 1).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                tableEncabezado.AddCell(celldaEncabezado);

                document.Add(tableEncabezado);

                line = new SolidLine();
                document.Add(new LineSeparator(line).SetMarginTop(5).SetMarginBottom(5));

                reparticion = new Paragraph();
                reparticion.Add(new Text("REPARTICIÓN: ").SetFontSize(10));
                reparticion.Add(new Text("JUR 20 - ADMINISTRACIÓN TRIBUTARIA PROVINCIAL").SetBold().SetUnderline().SetFontSize(12));
                document.Add(reparticion);

                #endregion

                #region cuerpo reporte

                fecha_impresion = new Paragraph().SetMarginTop(7).SetTextAlignment(TextAlignment.RIGHT);
                fecha_impresion.Add(new Text(string.Format("RESISTENCIA, _________________________________")).SetFontSize(12));
                document.Add(fecha_impresion);

                Paragraph contenido_salud = new Paragraph();
                contenido_salud.Add(new Text("En la fecha SI se recibe Certificado Médico Particular a "));
                contenido_salud.Add(new Text(solicitud.Agente.ApellidoYNombre).SetBold());
                contenido_salud.Add(new Text(" D.N.I. Nº "));
                contenido_salud.Add(new Text(solicitud.Agente.Legajo_datos_personales.DNI).SetBold());
                document.Add(contenido_salud);

                contenido_salud = new Paragraph().SetMultipliedLeading(2);
                contenido_salud.Add(new Text("Y "));
                contenido_salud.Add(new Text("SI – NO").SetBold());
                contenido_salud.Add(new Text(" se justifica licencia por Art. _______ inc ________ desde el ____/____/______ hasta el ____/____/______ por _______________________________ ( ______ ) días corridos, "));
                contenido_salud.Add(new Text("SI - NO").SetBold());
                contenido_salud.Add(new Text(" SE JUSTIFICA por _________________________________________________________________________________________.-"));
                document.Add(contenido_salud);


                contenido_salud = new Paragraph().SetMultipliedLeading(2);
                contenido_salud.Add(new Text("Personal con Horario "));
                contenido_salud.Add(new Text(ObtenerTurno()).SetBold());
                document.Add(contenido_salud);


                contenido_salud = new Paragraph().SetMultipliedLeading(2);
                contenido_salud.Add(new Text("El pedido es a "));
                switch ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad)
                {
                    case TipoMovimientoEnfermedad.Consultorio:
                        contenido_salud.Add(new Text(string.Format("{0}.", ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad).ToString())).SetBold());
                        document.Add(contenido_salud);
                        break;
                    case TipoMovimientoEnfermedad.Domicilio:
                        contenido_salud.Add(new Text(string.Format("{0}.", ((TipoMovimientoEnfermedad)solicitud.TipoEnfermedad).ToString())).SetBold());
                        document.Add(contenido_salud);
                        break;
                    case TipoMovimientoEnfermedad.Internacion:
                        contenido_salud.Add(new Text(string.Format("Internación:")).SetBold());
                        document.Add(contenido_salud);
                        break;
                    case TipoMovimientoEnfermedad.Consultorio_a_su_regreso_de:
                        contenido_salud.Add(new Text("Consultorio a su regreso de: ")).SetBold(); ;
                        contenido_salud.Add(new Text(string.Format("{0}", solicitud.Lugar)).SetBold());
                        document.Add(contenido_salud);
                        break;
                    default:
                        break;
                }


                contenido_salud = new Paragraph().SetMultipliedLeading(2);
                contenido_salud.Add(new Text("Inasiste desde "));
                contenido_salud.Add(new Text(solicitud.FechaDesde.ToString("dd/MM/yyyy")).SetBold());
                document.Add(contenido_salud);

                #endregion

                #endregion


                document.Flush();

                document.Close();

                res = ms.ToArray();
            }

            return res;
        }
    }
}