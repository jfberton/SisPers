using System;
using System.Collections.Generic;
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
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Pdf.Canvas;

namespace SisPer.Aplicativo
{
    public partial class Formulario1214
    {
        public bool PuedeEnviar()
        {
            bool ret = true;

            foreach (Agente1214 agente in Nomina)
            {
                if (agente.Estado == EstadoAgente1214.Solicitado)
                {
                    ret = false;
                }
            }

            ret = ret && Estado == Estado1214.Confeccionado;

            return ret;
        }

        [Obsolete]
        public Byte[] GenerarPDFSolicitud()
        {
            Byte[] res = null;

            using (MemoryStream ms = new MemoryStream())
            {
                //Genero el PDF en memoria para ir agregando las partes
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.LEGAL, false);

                string letra_3168 = "";

                float puntos_por_centimetro = (float)20;

                float margenSuperior = ((float)(6 * puntos_por_centimetro));
                float margenInferior = ((float)(3 * puntos_por_centimetro));
                float margenDerecho = ((float)(2 * puntos_por_centimetro));
                float margenIzquierdo = ((float)(4 * puntos_por_centimetro));

                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);
                document.SetFontSize(12);

                #region Solicitud F - 3168

                //Creo el titulo
                string texto_titulo = string.Empty;
                switch (Movilidad)
                {
                    case Movilidad1214.Vehiculo_oficial:
                        if (AnticipoMovilidad > 0)
                        {
                            if (AnticipoViaticos > 0)
                            {
                                texto_titulo = "FORMULARIO A.T. Nº 3168-A";
                                letra_3168 = "A";
                            }
                            else
                            {
                                texto_titulo = "FORMULARIO A.T. Nº 3168-D"; //Sin viaticos
                                letra_3168 = "D";
                            }
                        }
                        else
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-E"; //Sin viaticos y sin movilidad
                            letra_3168 = "E";
                        }

                        break;

                    case Movilidad1214.Vehiculo_particular:

                        if (AnticipoViaticos > 0)
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-B";
                            letra_3168 = "B";
                        }
                        else
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-F"; //Sin viaticos
                            letra_3168 = "F";
                        }
                        break;

                    case Movilidad1214.Transporte_publico:
                        if (AnticipoViaticos > 0)
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-C";
                            letra_3168 = "C";
                        }
                        else
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-G"; //Sin viaticos
                            letra_3168 = "G";
                        }
                        break;

                    case Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion:
                        if (AnticipoViaticos > 0)
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-H";
                            letra_3168 = "H";
                        }
                        else
                        {
                            texto_titulo = "FORMULARIO A.T. Nº 3168-I"; //Sin viaticos
                            letra_3168 = "I";
                        }
                        break;

                    default:
                        break;
                }

                Paragraph titulo = new Paragraph(texto_titulo)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetBold()
                   .SetUnderline()
                   .SetMargins(
                        /*top*/0
                        ,/*right*/0
                        , /*bottom*/10
                        , /*left*/0);
                document.Add(titulo);

                Paragraph subtitulo = new Paragraph("SOLICITUD DE AUTORIZACIÓN DE COMISIÓN DE SERVICIOS Nº " + Cadena.CompletarConCeros(6, Id))
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetBold()
                   .SetUnderline()
                   .SetMargins(
                        /*top*/15
                        ,/*right*/0
                        , /*bottom*/10
                        , /*left*/40);
                document.Add(subtitulo);


                //Creo lugar y fecha
                Text t_lugarYFecha = new Text(String.Format("Resistencia, {0}", this.Fecha_confeccion.Value.ToLongDateString().Replace(",", "")));
                Paragraph lugar_y_fecha = new Paragraph().Add(t_lugarYFecha)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetFontSize(12);
                document.Add(lugar_y_fecha);


                //Creo el primer texto
                Text t_comunico = new Text(String.Format("Se solicita autorización de la Administración General, para la Comisión de Servicios que, ésta Dirección/Departamento tiene dispuesta por {0} ({1}) días a partir del día {2} al día {3}, integrada por los agentes consignados a continuación:"
                    , Numalet.ToCardinal(((this.Hasta - this.Desde).Days + 1))
                    , ((this.Hasta - this.Desde).Days + 1).ToString()
                    , this.Desde.ToLongDateString()
                    , this.Hasta.ToLongDateString()
                    ));

                Paragraph comunico = new Paragraph()
                    .Add(t_comunico).SetMarginTop(15).SetMarginBottom(10).SetMarginLeft(40)
                    .SetTextAlignment(TextAlignment.JUSTIFIED);
                document.Add(comunico);

                //Creo el jefe de comision
                Text t_jefe = new Text("Jefe de Comisión de Servicios").SetUnderline();
                Paragraph titulo_jefe = new Paragraph().Add("1. ").Add(t_jefe).Add(":").SetMarginTop(15).SetMarginBottom(5).SetMarginLeft(40).SetBold();

                var nom_jefe = this.Nomina.First(x => x.JefeComicion && x.Estado == EstadoAgente1214.Aprobado).Agente;
                var chofer = this.Nomina.FirstOrDefault(x => x.Chofer && x.Estado == EstadoAgente1214.Aprobado);
                var chofer_agente = chofer != null ? chofer.Agente : nom_jefe;

                Paragraph jefe = new Paragraph().Add("    - ").Add(String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", nom_jefe.ApellidoYNombre, nom_jefe.Legajo, nom_jefe.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato))
                    .SetMarginLeft(55).SetMarginLeft(45);
                document.Add(titulo_jefe);
                document.Add(jefe);

                //Trabajo con la nomina de agentes
                Text t_otros = new Text("Otros agentes afectados a la Comisión de Servicios").SetUnderline();
                Paragraph titulo_otros = new Paragraph().Add("2. ").Add(t_otros).Add(":").SetMarginTop(15).SetMarginBottom(5).SetMarginLeft(40).SetBold();
                document.Add(titulo_otros);

                var nomina = this.Nomina.Where(x => !x.JefeComicion && !x.Chofer && x.Estado == EstadoAgente1214.Aprobado).ToList();
                if (nomina.Count > 0)
                {
                    foreach (Agente1214 item in nomina)
                    {
                        Paragraph otro = new Paragraph().Add("    - ").Add(String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", item.Agente.ApellidoYNombre, item.Agente.Legajo.ToString(), item.Agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato))
                            .SetMarginLeft(55).SetMarginLeft(45);
                        document.Add(otro);
                    }
                }
                else
                {
                    Paragraph otro = new Paragraph().Add("    - ").Add("Sin otros agentes afectados afectados a la Comisión de Servicios").SetMarginLeft(55).SetMarginLeft(45);
                    document.Add(otro);
                }

                Text t_destino = new Text(String.Format("{0}", this.Destino)).SetBold();

                Text t_tareas = new Text(String.Format("{0}", this.TareasACumplir)).SetBold();

                Paragraph destino = new Paragraph()
                    .Add("La localidad donde se realizará la Comisión de servicio es ")
                    .Add(t_destino)
                    .Add(" y las tareas a realizar por orden de mi superior son: ")
                    .Add(t_tareas).SetMarginTop(15).SetMarginBottom(10).SetMarginLeft(40)
                    .SetTextAlignment(TextAlignment.JUSTIFIED);
                document.Add(destino);

                Paragraph p_para_la_presente = new Paragraph("Para la presente comisión estimaré disponer, por donde corresponda, se provea:")
                    .SetMargins(
                        /*top*/15
                        ,/*right*/0
                        , /*bottom*/0
                        , /*left*/40);

                document.Add(p_para_la_presente);

                Text t_a = new Text("a. Medio de transporte utilizado:").SetBold();

                string medio = string.Empty;

                switch (Movilidad)
                {
                    case Movilidad1214.Vehiculo_oficial:
                        medio = "Vehículo oficial";
                        break;
                    case Movilidad1214.Vehiculo_particular:
                        medio = "Vehículo particular";
                        break;
                    case Movilidad1214.Transporte_publico:
                        medio = "Transporte público";
                        break;
                    case Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion:
                        medio = String.Format("Vehiculo autorizado en D.A. N° {0}", MovilidadAsociadaADispo.Value.ToString().Insert(MovilidadAsociadaADispo.Value.ToString().Length - 4, "/"));
                        break;
                    default:
                        break;
                }

                Paragraph p_a = new Paragraph().Add(t_a).Add(String.Format(" {0}", medio))
                           .SetMargins(
                               /*top*/0
                               ,/*right*/0
                               , /*bottom*/0
                               , /*left*/40);
                document.Add(p_a);


                Text t_b = new Text("b. Anticipo de viáticos:").SetBold();
                Paragraph p_b = new Paragraph().Add(t_b).Add(String.Format(" {0}", AnticipoViaticos > 0 ? AnticipoViaticos.ToString("C") : "No se solicita"))
                    .SetMargins(
                        /*top*/0
                        ,/*right*/0
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_b);


                string anticipo_otros_gastos = string.Empty;

                switch (Anticipo)
                {
                    case Anticipo1214.Gastos_vehiculo:
                        anticipo_otros_gastos = "Gastos vehículo";
                        break;
                    case Anticipo1214.Pasajes:
                        anticipo_otros_gastos = "Gastos pasajes";
                        break;
                    default:
                        break;
                }

                Text t_c = new Text("c. Anticipo para otros gastos: ").SetBold();

                Paragraph p_c = new Paragraph().Add(t_c).Add(String.Format(" {0} {1}", (AnticipoMovilidad > 0) ? anticipo_otros_gastos : "", AnticipoMovilidad > 0 ? AnticipoMovilidad.ToString("C") : "No se solicita"))
                    .SetMargins(
                        /*top*/0
                        ,/*right*/0
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_c);


                Paragraph p_linea_firma = new Paragraph("....................................                                                    ........................................")
                    .SetMargins(
                        /*top*/70
                        ,/*right*/0
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_linea_firma);

                Paragraph p_firma = new Paragraph("Firma del Jefe de comisión                                                                         Autorización Superior inmediato")
                    .SetFontSize(9)
                    .SetMargins(
                        /*top*/0
                        ,/*right*/0
                        , /*bottom*/0
                        , /*left*/45);
                document.Add(p_firma);

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));


                Text t_medio = new Text("Medio de Transporte: ").SetBold();

                switch (this.Movilidad)
                {
                    case Movilidad1214.Transporte_publico:

                        break;
                    case Movilidad1214.Vehiculo_oficial:
                        Paragraph medio_transporte_oficial = new Paragraph().Add(t_medio).Add(String.Format(" Vehículo oficial"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(medio_transporte_oficial);

                        Text oficial_dominio = new Text(String.Format(" {0}", this.Vehiculo_dominio)).SetBold();
                        Paragraph p_medio_oficial_texto = new Paragraph();

                        if (AnticipoMovilidad > 0)
                        {
                            p_medio_oficial_texto = new Paragraph(String.Format("Para el cumplimiento de la comisión se debe proveer de Pesos {0} ({1}) en concepto de gastos de movilidad y afectar al vehículo oficial dominio ", Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"))).Add(oficial_dominio);
                        }
                        else
                        {
                            p_medio_oficial_texto = new Paragraph("Para el cumplimiento de la comisión se debe afectar al vehículo oficial dominio ").Add(oficial_dominio);
                        }


                        Text t_chofer = new Text(String.Format(", conducido por {0}", String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", chofer_agente.ApellidoYNombre, chofer_agente.Legajo, chofer_agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato)));
                        p_medio_oficial_texto.Add(t_chofer);

                        p_medio_oficial_texto.SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/40
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                        document.Add(p_medio_oficial_texto);

                        document = AgregarFirmaMantenimiento(document);

                        break;

                    case Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion:
                        Paragraph medio_transporte_afectado_a_otra_dispo = new Paragraph().Add(t_medio).Add(String.Format(" Vehículo autorizado en otra disposición"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(medio_transporte_afectado_a_otra_dispo);

                        Text oficial_afectado_a_otra_dispo = new Text(String.Format(" {0}", this.Vehiculo_dominio)).SetBold();

                        Paragraph p_medio_oficial_afectado_a_otra_dispo = new Paragraph(String.Format("Para el cumplimiento de la comisión se debe afectar el vehículo oficial, dominio:")).Add(oficial_afectado_a_otra_dispo);

                        Text t_chofer_afectado_a_otra_dispo = new Text(String.Format(", conducido por {0}", String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}, autorizados en Disposición Administrativa N° {4}", chofer_agente.ApellidoYNombre, chofer_agente.Legajo, chofer_agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato, MovilidadAsociadaADispo.Value.ToString().Insert(MovilidadAsociadaADispo.Value.ToString().Length - 4, "/"))));
                        p_medio_oficial_afectado_a_otra_dispo.Add(t_chofer_afectado_a_otra_dispo);

                        p_medio_oficial_afectado_a_otra_dispo.SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/40
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                        document.Add(p_medio_oficial_afectado_a_otra_dispo);

                        document = AgregarFirmaMantenimiento(document);

                        break;

                    case Movilidad1214.Vehiculo_particular:
                        Paragraph medio_transporte_particular = new Paragraph().Add(t_medio).Add(String.Format(" Vehículo particular"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(medio_transporte_particular);

                        Text particular_dominio = new Text(String.Format(" {0}", this.Vehiculo_dominio)).SetBold();
                        Text particular_titular = new Text(String.Format(" {0}", this.Vehiculo_particular_titular)).SetBold();
                        Text particular_tipo_combustible = new Text(String.Format(" {0}", this.Vehiculo_particular_tipo_combustible)).SetBold();
                        Text particular_poliza = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_nro)).SetBold();
                        Text particular_poliza_vigencia = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_vigencia)).SetBold();
                        Text particular_poliza_cobertura = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_cobertura)).SetBold();

                        Paragraph p_medio_particular_texto = new Paragraph(String.Format("Para el cumplimiento de la comisión se dede proveer de Pesos {0} ({1}) en concepto de gastos de movilidad. Datos del vehículo particular para el que se solicita autorización: dominio ", Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"))).Add(particular_dominio);
                        p_medio_particular_texto.Add(", perteneciente a ").Add(particular_titular).Add(", tipo de combustible ").Add(particular_tipo_combustible).Add(", número de póliza ").Add(particular_poliza).Add(" vigente hasta ").Add(particular_poliza_vigencia).Add(" tipo de cobertura ").Add(particular_poliza_cobertura);
                        p_medio_particular_texto.SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/40
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                        document.Add(p_medio_particular_texto);

                        document = AgregarFirmaMantenimiento(document);

                        break;

                }


                //nuevo
                Text negrita = new Text("Notificación:").SetBold().SetUnderline();
                Text normal = new Text(" En caso de incumplimiento, de las obligaciones referidas a la rendición del presente anticipo, será aplicable lo estipulado por el Memorándum Nº 50/2014 de Contaduría General de la Provincia del Chaco…");
                Text ital = new Text("” Si el subresponsable no efectuare la rendición y/o reintegro del excedente del presente anticipo dentro del plazo reglamentario, autoriza expresamente a retener de sus haberes los importes recibidos y/o no reintegrados”").SetItalic();
                Paragraph p_notificacion = new Paragraph().Add(negrita).Add(normal).Add(ital)
                    .SetMargins(
                        /*top*/90
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40).SetTextAlignment(TextAlignment.JUSTIFIED);
                document.Add(p_notificacion);

                p_firma = new Paragraph(".............................................................")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMargins(
                            /*top*/90
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                document.Add(p_firma);

                p_firma = new Paragraph("Firma Jefe comisión de servicios")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMargins(
                            /*top*/0
                            ,/*right*/20
                            , /*bottom*/0
                            , /*left*/40);
                document.Add(p_firma);


                Table t = new Table(1);

                negrita = new Text("IMPORTANTE: ").SetBold();
                normal = new Text("Se recuerda que los anticipos a los cuales se refiere el presente Formulario AT 3168, deberán ser rendidos INDEFECTIBLEMENTE en el plazo de tres (3) días hábiles posteriores al regreso de la comisión de servicios, según lo establecido por Decreto Nº 1324/78 (t.v.) y Memorándum Nº 50/14 de la Contaduría General de la Provincia.");

                Cell celda = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.JUSTIFIED)
                   .Add(new Paragraph().Add(negrita).Add(normal));

                t.AddCell(celda);

                Paragraph p_tabla = new Paragraph().Add(t)
                    .SetMargins(
                            /*top*/20
                            ,/*right*/20
                            , /*bottom*/0
                            , /*left*/40);

                document.Add(p_tabla);

                #endregion

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                //actualizo el margen superior para dejar espacio para agregar el titulo DISPOCICION N°_____
                margenSuperior = ((float)(7.5 * puntos_por_centimetro));
                document.SetMargins(margenSuperior, margenDerecho, margenInferior, margenIzquierdo);

                #region Disposicion

                //document.Add(leyenda);
                //document.Add(membrete);

                //Creo el titulo
                titulo = new Paragraph("DISPOSICIÓN ADMINISTRATIVA Nº ________________")
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetBold()
                   .SetFontSize(12)
                   .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40);
                //document.Add(titulo); <-- no lo agrego aca porque lo agrego luego en la parte que agrego encabezados y pies de pagina ya que va en todas las hojas de la disposición

                document.Add(lugar_y_fecha.SetMarginRight(15).SetMarginTop(20));

                negrita = new Text("VISTO Y CONSIDERANDO:").SetBold();
                Paragraph p = new Paragraph().Add(negrita)
                    .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40)
                    .SetFontSize(12);

                document.Add(p);

                normal = new Text("La solicitud de autorización de comisión de servicios según Formulario AT Nº 3168-" + letra_3168 + " “Solicitud autorización Comisión de Servicios N° " + Cadena.CompletarConCeros(6, Id) + "” que se adjunta;");

                p = new Paragraph().Add(normal)
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetFirstLineIndent(40);

                document.Add(p);

                string cadena = string.Empty;

                if (AnticipoViaticos > 0)
                {
                    cadena = "Que consecuentemente es necesario autorizar y  liquidar, en concepto de anticipo de viáticos a las/los agentes " +
                    nom_jefe.ApellidoYNombre;

                    var _chofer = this.Nomina.FirstOrDefault(x => x.Chofer && x.Estado == EstadoAgente1214.Aprobado);

                    foreach (Agente1214 item in nomina)
                    {
                        cadena += ", " + item.Agente.ApellidoYNombre;
                    }

                    if (_chofer != null && Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion)
                    {
                        cadena += ", " + _chofer.Agente.ApellidoYNombre;
                    }

                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion)
                    {
                        cadena += String.Format(", por la suma de pesos {0} ({1}) y para otros gastos de comisión la suma de pesos {2} ({3});", Numalet.ToCardinal(this.AnticipoViaticos), this.AnticipoViaticos.ToString("C"), Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"));
                    }
                    else
                    {
                        cadena += String.Format(", por la suma de pesos {0} ({1});", Numalet.ToCardinal(this.AnticipoViaticos), this.AnticipoViaticos.ToString("C"));
                    }

                    normal = new Text(cadena);

                    p = new Paragraph().Add(normal)
                        .SetMargins(
                            /*top*/0
                            ,/*right*/15
                            , /*bottom*/10
                            , /*left*/40)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetFirstLineIndent(40);

                    document.Add(p);
                }

                if (AnticipoMovilidad > 0 && AnticipoViaticos == 0)
                {
                    cadena = String.Format("Que consecuentemente es necesario autorizar y liquidar, en concepto de anticipo para otros gastos de comisión a la/el agente {0} - CUIL {1} - Legajo {2}, por la suma de pesos {3} ({4});", nom_jefe.ApellidoYNombre, nom_jefe.Legajo_datos_laborales.CUIT, nom_jefe.Legajo, Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad);

                    normal = new Text(cadena);

                    p = new Paragraph().Add(normal)
                        .SetMargins(
                            /*top*/0
                            ,/*right*/15
                            , /*bottom*/10
                            , /*left*/40)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetFirstLineIndent(40);

                    document.Add(p);

                }

                if (AnticipoViaticos > 0 || AnticipoMovilidad > 0)
                {
                    normal = new Text("Que, una vez concluida la Comisión de servicios autorizada, cada uno/a de los/las agentes afectados/as a la misma, deberán realizar la correspondiente rendición de viáticos y gastos, según corresponda, en el marco de las normas vigentes, Decreto Provincial Nº 1324/78, Memorándum Nº 50/2014 de Contaduría General de la Provincia, Memorándums Nº 03/2014, Nº 01/2015 y Circular General Nº 06/2015 de la Administración Tributaria, entre otros instrumentos legales.");

                    p = new Paragraph().Add(normal)
                        .SetMargins(
                            /*top*/0
                            ,/*right*/15
                            , /*bottom*/10
                            , /*left*/40)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetFirstLineIndent(40);

                    document.Add(p);
                }

                switch (this.Movilidad)
                {
                    case Movilidad1214.Vehiculo_oficial:

                        normal = new Text(String.Format("Que para la realización de la comisión es necesario proveer de medio de movilidad afectándose el vehículo {0}, el cual será conducido por {1} – {2} – {3} - {4}, contándose con el visto bueno del Jefe de Departamento Mantenimiento,", Vehiculo_dominio, chofer_agente.ApellidoYNombre, chofer_agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato, chofer_agente.Legajo));

                        p = new Paragraph().Add(normal)
                       .SetMargins(
                           /*top*/0
                           ,/*right*/15
                           , /*bottom*/10
                           , /*left*/40)
                       .SetFontSize(12)
                       .SetTextAlignment(TextAlignment.JUSTIFIED)
                       .SetFirstLineIndent(40);

                        document.Add(p);

                        break;
                    case Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion:

                        normal = new Text(String.Format("Que para la realización de la comisión es necesario proveer de medio de movilidad afectándose el vehículo {0} autorizado por Disposición Administrativa N° {1}, el cual será conducido por {2} – CUIT {3} – Estrato {4} - Legajo {5},", Vehiculo_dominio, MovilidadAsociadaADispo.Value.ToString().Insert(MovilidadAsociadaADispo.Value.ToString().Length - 4, "/"), chofer_agente.ApellidoYNombre, chofer_agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato, chofer_agente.Legajo));

                        p = new Paragraph().Add(normal)
                       .SetMargins(
                           /*top*/0
                           ,/*right*/15
                           , /*bottom*/10
                           , /*left*/40)
                       .SetFontSize(12)
                       .SetTextAlignment(TextAlignment.JUSTIFIED)
                       .SetFirstLineIndent(40);

                        document.Add(p);

                        break;
                    default:
                        break;
                }

                normal = new Text("Por ello:");

                p = new Paragraph().Add(normal)
                      .SetMargins(
                          /*top*/0
                          ,/*right*/15
                          , /*bottom*/10
                          , /*left*/40)
                      .SetFontSize(12)
                      .SetTextAlignment(TextAlignment.JUSTIFIED)
                      .SetFirstLineIndent(40);

                document.Add(p);

                normal = new Text("LA ADMINISTRACIÓN TRIBUTARIA DE LA PROVINCIA DEL CHACO DISPONE:").SetBold();
                p = new Paragraph().Add(normal)
                      .SetMargins(
                          /*top*/15
                          ,/*right*/15
                          , /*bottom*/10
                          , /*left*/40)
                      .SetFontSize(12)
                      .SetTextAlignment(TextAlignment.CENTER);

                document.Add(p);

                negrita = new Text("ARTÍCULO 1º: AUTORIZAR ").SetBold();
                normal = new Text(String.Format("la comisión de servicios según Formulario AT Nº 3168-{0}  “Solicitud autorización Comisión de Servicios N° {1} ” que se adjunta a la presente. ", letra_3168, Cadena.CompletarConCeros(6, Id)));

                p = new Paragraph().Add(negrita).Add(normal)
                      .SetMargins(
                          /*top*/0
                          ,/*right*/15
                          , /*bottom*/10
                          , /*left*/40)
                      .SetFontSize(12)
                      .SetTextAlignment(TextAlignment.JUSTIFIED)
                      .SetKeepTogether(true)
                      .SetKeepWithNext(false);

                document.Add(p);


                //SIN ANTICIPO Y MOVILIDAD PERO DISTINTO DE ASOCIADO A OTRA DISPOSICION YA QUE ESTE TAMBIEN TIENE ANTICIPO MOVILIDAD EN CERO
                if (AnticipoViaticos == 0 && AnticipoMovilidad == 0)
                {
                    negrita = new Text("ARTÍCULO 2º: AFECTAR ").SetBold();

                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion)
                    {
                        cadena = String.Format("el vehículo {0} que será conducido por el/la agente {1} – CUIL {2} – Estrato {3} - Legajo {4}, el cual será responsable del cumplimiento de las normas vigentes en materia de conservación y cuidado de automotores del Estado Provincial.", Vehiculo_dominio, chofer_agente.ApellidoYNombre, chofer_agente.Legajo_datos_laborales.CUIT, Estrato1214.Estrato, chofer_agente.Legajo);
                    }
                    else
                    {
                        cadena = String.Format(" para el traslado de los agentes, incluidos en la Comisión de Servicios mencionada en el artículo 1° de la presente, el vehículo  {0}, conducido por {1} Estrato:{2} - Legajo: {3} - CUIL: {4}, autorizados en Disposición Administrativa N° {5}"
                            , this.Vehiculo_dominio
                            , chofer_agente.ApellidoYNombre
                            , this.Estrato1214.Estrato
                            , chofer_agente.Legajo
                            , chofer_agente.Legajo_datos_laborales.CUIT
                            , MovilidadAsociadaADispo.Value.ToString().Insert(MovilidadAsociadaADispo.Value.ToString().Length - 4, " /"));
                    }

                    normal = new Text(cadena);

                    p = new Paragraph().Add(negrita).Add(normal)
                          .SetMargins(
                              /*top*/0
                              ,/*right*/15
                              , /*bottom*/10
                              , /*left*/40)
                          .SetFontSize(12)
                          .SetTextAlignment(TextAlignment.JUSTIFIED)
                          .SetKeepTogether(true)
                          .SetKeepWithNext(false);

                    document.Add(p);

                    negrita = new Text("ARTÍCULO 3º: NOTIFÍQUESE ").SetBold();
                    normal = new Text("al Tribunal de Cuentas. ");

                    p = new Paragraph().Add(negrita).Add(normal)
                         .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/10
                                , /*left*/40)
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.JUSTIFIED)
                            .SetKeepTogether(true)
                            .SetKeepWithNext(false);

                    document.Add(p);

                    negrita = new Text("ARTÍCULO 4º: TOME ").SetBold();


                    normal = new Text("razón Despacho. ");

                    p = new Paragraph().Add(negrita).Add(normal);

                    negrita = new Text("REGÍSTRESE. ").SetBold();
                    normal = new Text("Comuníquese a la Dirección Administración y a los integrantes de la Comisión de Servicios. Cumplido, ");

                    p.Add(negrita).Add(normal);

                    negrita = new Text("ARCHÍVESE.").SetBold();

                    p.Add(negrita)
                         .SetMargins(
                              /*top*/0
                              ,/*right*/15
                              , /*bottom*/10
                              , /*left*/40)
                          .SetFontSize(12)
                          .SetTextAlignment(TextAlignment.JUSTIFIED)
                          .SetKeepTogether(true)
                          .SetKeepWithNext(false);

                    document.Add(p);

                }
                else
                {
                    if (AnticipoViaticos > 0)
                    {
                        negrita = new Text("ARTÍCULO 2º: AUTORIZAR ").SetBold();

                        cadena = String.Format("a la Dirección de Administración a liquidar y abonar, mediante depósito/transferencia en el NUEVO BANCO DEL CHACO S.A, Caja de Ahorro sueldo de los agentes: {0} CUIL {1} Legajo {2} ", nom_jefe.ApellidoYNombre, nom_jefe.Legajo_datos_laborales.CUIT, nom_jefe.Legajo);

                        foreach (Agente1214 item in nomina)
                        {
                            cadena += ", " + item.Agente.ApellidoYNombre + " CUIT " + item.Agente.Legajo_datos_laborales.CUIT + " Legajo " + item.Agente.Legajo.ToString();
                        }

                        if (chofer != null && Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion)
                        {
                            cadena += ", " + chofer_agente.ApellidoYNombre + " CUIT " + chofer_agente.Legajo_datos_laborales.CUIT + " Legajo " + chofer_agente.Legajo.ToString();
                        }

                        cadena += String.Format(", la suma total de pesos {0} ({1}), a cada uno de los agentes mencionados, en concepto de anticipo de viático para cumplir con las tareas que se indican en Formulario AT Nº 3168-" + letra_3168 + " “Solicitud autorización Comisión de Servicios N° {2}.” que se adjunta a la presente.;", Numalet.ToCardinal(this.AnticipoViaticos), this.AnticipoViaticos.ToString("C"), Cadena.CompletarConCeros(6, Id));

                        normal = new Text(cadena);

                        p = new Paragraph().Add(negrita).Add(normal)
                              .SetMargins(
                                  /*top*/0
                                  ,/*right*/15
                                  , /*bottom*/10
                                  , /*left*/40)
                              .SetFontSize(12)
                              .SetTextAlignment(TextAlignment.JUSTIFIED)
                              .SetKeepTogether(true)
                              .SetKeepWithNext(false);

                        document.Add(p);

                    }

                    if (AnticipoViaticos > 0)
                    {
                        if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion)
                        {
                            negrita = new Text("ARTÍCULO 3º: AUTORIZAR ").SetBold();

                            cadena = String.Format("a la Dirección de Administración de la Administración Tributaria Provincial a liquidar y abonar, mediante depósito/transferencia en el NUEVO BANCO DEL CHACO S.A , Caja de Ahorro sueldo a la/el agente: {0} CUIL {1} Legajo {2} la suma total de pesos {3} ({4}) para otros gastos que resulten necesarios a fin de cumplir con las tareas que se indican en Formulario AT Nº 3168-" + letra_3168 + " “Solicitud autorización Comisión de Servicios N° {5} ”. que se adjunta a la presente.  ", nom_jefe.ApellidoYNombre, nom_jefe.Legajo_datos_laborales.CUIT, nom_jefe.Legajo, Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"), Cadena.CompletarConCeros(6, Id));

                            normal = new Text(cadena);

                            p = new Paragraph().Add(negrita).Add(normal)
                                  .SetMargins(
                                      /*top*/0
                                      ,/*right*/15
                                      , /*bottom*/10
                                      , /*left*/40)
                                  .SetFontSize(12)
                                  .SetTextAlignment(TextAlignment.JUSTIFIED)
                                  .SetKeepTogether(true)
                                  .SetKeepWithNext(false);

                            document.Add(p);
                        }
                    }
                    else
                    {
                        negrita = new Text("ARTÍCULO 2º: AUTORIZAR ").SetBold();

                        cadena = String.Format("a la Dirección de Administración de la Administración Tributaria Provincial a liquidar y abonar, mediante depósito/transferencia en el NUEVO BANCO DEL CHACO S.A , Caja de Ahorro sueldo a la/el agente: {0} CUIL {1} Legajo {2} la suma total de pesos {3} ({4}) para otros gastos que resulten necesarios a fin de cumplir con las tareas que se indican en Formulario AT Nº 3168-{5} “Solicitud autorización Comisión de Servicios N° {6} ”. que se adjunta a la presente.  ", nom_jefe.ApellidoYNombre, nom_jefe.Legajo_datos_laborales.CUIT, nom_jefe.Legajo, Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"), letra_3168, Cadena.CompletarConCeros(6, Id));

                        normal = new Text(cadena);

                        p = new Paragraph().Add(negrita).Add(normal)
                              .SetMargins(
                                  /*top*/0
                                  ,/*right*/15
                                  , /*bottom*/10
                                  , /*left*/40)
                              .SetFontSize(12)
                              .SetTextAlignment(TextAlignment.JUSTIFIED)
                              .SetKeepTogether(true)
                              .SetKeepWithNext(false);

                        document.Add(p);

                    }

                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion && AnticipoViaticos > 0)
                    {
                        negrita = new Text("ARTÍCULO 4º: ESTABLECER ").SetBold();
                    }
                    else
                    {
                        negrita = new Text("ARTÍCULO 3º: ESTABLECER ").SetBold();
                    }

                    if (AnticipoViaticos > 0)
                    {
                        cadena = String.Format("que los montos depositados en concepto de anticipo de viáticos y anticipo para otros gastos de comisión deberán ser rendidos, por cada uno de los/las agentes responsables, al finalizar la comisión y dentro de los tres (3) días hábiles posteriores al regreso, reintegrando los saldos excedentes, si los hubiere.");
                    }
                    else
                    {
                        cadena = String.Format("que los montos depositados en concepto de anticipo de viáticos y anticipo para otros gastos de comisión deberán ser rendidos, por el/la agente responsable, al finalizar la comisión y dentro de los tres (3) días hábiles posteriores al regreso, reintegrando los saldos excedentes, si los hubiere.");
                    }


                    normal = new Text(cadena);

                    p = new Paragraph().Add(negrita).Add(normal)
                          .SetMargins(
                              /*top*/0
                              ,/*right*/15
                              , /*bottom*/10
                              , /*left*/40)
                          .SetFontSize(12)
                          .SetTextAlignment(TextAlignment.JUSTIFIED)
                          .SetKeepTogether(true)
                          .SetKeepWithNext(false);

                    document.Add(p);

                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion && AnticipoViaticos > 0)
                    {
                        negrita = new Text("ARTÍCULO 5º: ESTABLECER ").SetBold();
                    }
                    else
                    {
                        negrita = new Text("ARTÍCULO 4º: ESTABLECER ").SetBold();
                    }

                    cadena = String.Format("que, en caso de incumplimiento, de las obligaciones referidas en el artículo precedente, será aplicable lo estipulado por el Memorándum Nº 50/2014 de Contaduría General de la Provincia del Chaco ");

                    normal = new Text(cadena);

                    ital = new Text("…” Si el subresponsable no efectuare la rendición y/o reintegro del excedente del presente anticipo dentro del plazo reglamentario, autoriza expresamente a retener de sus haberes los importes recibidos y/o no reintegrados”. ").SetItalic();

                    p = new Paragraph().Add(negrita).Add(normal).Add(ital)
                          .SetMargins(
                              /*top*/0
                              ,/*right*/15
                              , /*bottom*/10
                              , /*left*/40)
                          .SetFontSize(12)
                          .SetTextAlignment(TextAlignment.JUSTIFIED)
                          .SetKeepTogether(true)
                          .SetKeepWithNext(false);

                    document.Add(p);

                    switch (Movilidad)
                    {
                        case Movilidad1214.Vehiculo_oficial:
                            if (AnticipoViaticos > 0)
                            {
                                negrita = new Text("ARTÍCULO 6º: AFECTAR ").SetBold();
                            }
                            else
                            {
                                negrita = new Text("ARTÍCULO 5º: AFECTAR ").SetBold();
                            }


                            cadena = String.Format("el vehículo {0} que será conducido por el/la agente {1} – CUIL {2} – Estrato {3} - Legajo {4}, el cual será responsable del cumplimiento de las normas vigentes en materia de conservación y cuidado de automotores del Estado Provincial.", Vehiculo_dominio, chofer_agente.ApellidoYNombre, chofer_agente.Legajo_datos_laborales.CUIT, Estrato1214.Estrato, chofer_agente.Legajo);

                            normal = new Text(cadena);

                            p = new Paragraph().Add(negrita).Add(normal)
                                  .SetMargins(
                                      /*top*/0
                                      ,/*right*/15
                                      , /*bottom*/10
                                      , /*left*/40)
                                  .SetFontSize(12)
                                  .SetTextAlignment(TextAlignment.JUSTIFIED)
                                  .SetKeepTogether(true)
                                  .SetKeepWithNext(false);

                            document.Add(p);

                            break;
                        case Movilidad1214.Vehiculo_particular:
                            if (AnticipoViaticos > 0)
                            {
                                negrita = new Text("ARTÍCULO 6º: AFECTAR ").SetBold();
                            }
                            else
                            {
                                negrita = new Text("ARTÍCULO 5º: AFECTAR ").SetBold();
                            }


                            Text particular_dominio = new Text(String.Format(" {0}", this.Vehiculo_dominio));
                            Text particular_titular = new Text(String.Format(" {0}", this.Vehiculo_particular_titular));
                            Text particular_tipo_combustible = new Text(String.Format(" {0}", this.Vehiculo_particular_tipo_combustible));
                            Text particular_poliza = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_nro));
                            Text particular_poliza_vigencia = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_vigencia));
                            Text particular_poliza_cobertura = new Text(String.Format(" {0}", this.Vehiculo_particular_poliza_cobertura));

                            Paragraph p_medio_particular_texto = new Paragraph(negrita).Add(String.Format("el vehículo particular dominio ")).Add(particular_dominio);
                            p_medio_particular_texto.Add(", perteneciente a ").Add(particular_titular).Add(", tipo de combustible ").Add(particular_tipo_combustible).Add(", número de póliza ").Add(particular_poliza).Add(" vigente hasta ").Add(particular_poliza_vigencia).Add(" tipo de cobertura ").Add(particular_poliza_cobertura);
                            p_medio_particular_texto.SetFontSize(12)
                                .SetMargins(
                                    /*top*/0
                                    ,/*right*/15
                                    , /*bottom*/10
                                    , /*left*/40)
                                .SetFontSize(12)
                                .SetTextAlignment(TextAlignment.JUSTIFIED)
                                .SetKeepTogether(true)
                                .SetKeepWithNext(false);
                            document.Add(p_medio_particular_texto);

                            break;
                        case Movilidad1214.Transporte_publico:
                            if (AnticipoViaticos > 0)
                            {
                                negrita = new Text("ARTÍCULO 6º: AUTORIZAR ").SetBold();
                                normal = new Text("el traslado de los/las agentes integrantes de la presente comisión de servicios en transporte público, debiendo presentar y rendir, en los términos del ARTÍCULO 4°, los pasajes y/o documentos que justifiquen el traslado.");
                            }
                            else
                            {
                                negrita = new Text("ARTÍCULO 5º: AUTORIZAR ").SetBold();
                                normal = new Text("el traslado de los/las agentes integrantes de la presente comisión de servicios en transporte público, debiendo presentar y rendir, en los términos del ARTÍCULO 3°, los pasajes y/o documentos que justifiquen el traslado.");
                            }

                            p = new Paragraph().Add(negrita).Add(normal)
                                  .SetMargins(
                                      /*top*/0
                                      ,/*right*/15
                                      , /*bottom*/10
                                      , /*left*/40)
                                  .SetFontSize(12)
                                  .SetTextAlignment(TextAlignment.JUSTIFIED)
                                  .SetKeepTogether(true)
                                  .SetKeepWithNext(false);

                            document.Add(p);
                            break;

                        case Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion:
                            negrita = new Text("ARTÍCULO 5º: AFECTAR ").SetBold();
                            normal = new Text(String.Format("para el traslado de los agentes mencionados en el artículo 2° de la presente, el vehículo {0} que será conducido por el/la agente {1} – CUIL {2} – Estrato {3} - Legajo {4}, autorizados por la Disposición Administrativa N° {5}", Vehiculo_dominio, chofer_agente.ApellidoYNombre, chofer_agente.Legajo_datos_laborales.CUIT, Estrato1214.Estrato, chofer_agente.Legajo, MovilidadAsociadaADispo.Value.ToString().Insert(MovilidadAsociadaADispo.Value.ToString().Length - 4, "/")));

                            p = new Paragraph().Add(negrita).Add(normal)
                                  .SetMargins(
                                      /*top*/0
                                      ,/*right*/15
                                      , /*bottom*/10
                                      , /*left*/40)
                                  .SetFontSize(12)
                                  .SetTextAlignment(TextAlignment.JUSTIFIED)
                                  .SetKeepTogether(true)
                                  .SetKeepWithNext(false);

                            document.Add(p);

                            break;

                        default:
                            break;
                    }


                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion && AnticipoViaticos > 0)
                    {
                        negrita = new Text("ARTÍCULO 7º: NOTIFÍQUESE ").SetBold();
                    }
                    else
                    {
                        negrita = new Text("ARTÍCULO 6º: NOTIFÍQUESE ").SetBold();
                    }


                    normal = new Text("al Tribunal de Cuentas. ");

                    p = new Paragraph().Add(negrita).Add(normal)
                         .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/10
                                , /*left*/40)
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.JUSTIFIED)
                            .SetKeepTogether(true)
                            .SetKeepWithNext(false);

                    document.Add(p);

                    if (Movilidad != Movilidad1214.Vehiculo_oficial_autorizado_por_disposicion && AnticipoViaticos > 0)
                    {
                        negrita = new Text("ARTÍCULO 8º: TOME ").SetBold();
                    }
                    else
                    {
                        negrita = new Text("ARTÍCULO 7º: TOME ").SetBold();
                    }


                    normal = new Text("razón Despacho. ");

                    p = new Paragraph().Add(negrita).Add(normal);

                    negrita = new Text("REGÍSTRESE. ").SetBold();
                    normal = new Text("Comuníquese a la Dirección Administración y a los integrantes de la Comisión de Servicios. Cumplido, ");

                    p.Add(negrita).Add(normal);

                    negrita = new Text("ARCHÍVESE.").SetBold();

                    p.Add(negrita)
                         .SetMargins(
                              /*top*/0
                              ,/*right*/15
                              , /*bottom*/10
                              , /*left*/40)
                          .SetFontSize(12)
                          .SetTextAlignment(TextAlignment.JUSTIFIED)
                          .SetKeepTogether(true)
                          .SetKeepWithNext(false);

                    document.Add(p);
                }

                negrita = new Text("ADMINISTRACIÓN TRIBUTARIA PROVINCIAL.").SetBold();

                p = new Paragraph().Add(negrita)
                     .SetMargins(
                          /*top*/30
                          ,/*right*/15
                          , /*bottom*/10
                          , /*left*/40)
                      .SetFontSize(12)
                      .SetTextAlignment(TextAlignment.LEFT)
                      .SetKeepTogether(true)
                      .SetKeepWithNext(false);

                document.Add(p);

                #endregion

                document.Flush();

                #region Agrego cabecera y pie

                Text t_leyenda = new Text(ConfigurationManager.AppSettings["Leyenda"]);

                Paragraph leyenda = new Paragraph().Add(t_leyenda)
                    .SetFontSize(9);

                Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);

                //Actualizo nuevamente el margen que voy a utilizar como punto para agregar la linea de titulo
                margenSuperior = ((float)5 * puntos_por_centimetro);


                int pagina = 1;

                var pdfDoc = document.GetPdfDocument();

                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    PdfPage page = pdfDoc.GetPage(i);
                    Rectangle area = page.GetPageSize().ApplyMargins(40, 28, 28, 28, false);
                    float x = area.GetWidth() + 10; // 2;
                    float y = area.GetTop() + 5;

                    #region encabezados

                    //comun a todos membrete y leyenda
                    document.ShowTextAligned(leyenda, x, y, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);

                    PdfCanvas aboveCanvas = new PdfCanvas(page.NewContentStreamAfter(),
                                        page.GetResources(), pdfDoc);

                    new Canvas(aboveCanvas, pdfDoc, area).Add(membrete);

                    if (i > 2) //disposicion
                    {
                        document.ShowTextAligned(titulo, x, y - margenSuperior, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                    }

                    #endregion

                    #region pie de pagina

                    if (i <= 2) //formulario
                    {
                        document.ShowTextAligned(new Paragraph(texto_titulo).Add(" - hoja ").Add(pagina.ToString()).SetFontSize(9).SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY), x, area.GetBottom(), i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                        pagina++;
                    }
                    else //disposicion
                    {
                        pagina = i == 3 ? 1 : pagina;

                        document.ShowTextAligned(new Paragraph(pagina.ToString()), x, area.GetBottom(), i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                        pagina++;
                    }

                    #endregion
                }

                #endregion

                //Cierro el documento
                document.Close();

                res = ms.ToArray();
            }

            return res;
        }

        private Document AgregarFirmaMantenimiento(Document document)
        {
            Paragraph p_depto_manteminiento = new Paragraph(String.Format("Departamento de Mantenimiento y Bienes Patrimoniales, {0}", this.Fecha_confeccion.Value.ToShortDateString()))
                      .SetMargins(
                             /*top*/20
                             ,/*right*/15
                             , /*bottom*/0
                             , /*left*/40);
            document.Add(p_depto_manteminiento);

            Paragraph firma_responsable = new Paragraph(".............................................................")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMargins(
                        /*top*/90
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);
            document.Add(firma_responsable);

            Paragraph firma_responsable_texto = new Paragraph("Firma y Sello del responsable a/c")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMargins(
                        /*top*/0
                        ,/*right*/20
                        , /*bottom*/0
                        , /*left*/40);
            document.Add(firma_responsable_texto);

            return document;
        }

    }
}
