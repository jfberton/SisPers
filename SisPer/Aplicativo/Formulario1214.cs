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

        public Byte[] GenerarPDFSolicitud()
        {
            Byte[] res = null;

            using (MemoryStream ms = new MemoryStream())
            {
                //Genero el PDF en memoria para ir agregando las partes
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.LEGAL);


                //Creo la leyenda
                Text t_leyenda = new Text(ConfigurationManager.AppSettings["Leyenda"]);

                Paragraph leyenda = new Paragraph().Add(t_leyenda)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(9);
                document.Add(leyenda);

                //Creo la imagen del membrete
                Image membrete = new Image(ImageDataFactory.Create(HttpContext.Current.Server.MapPath("../Imagenes/membrete.png"))).SetAutoScale(true);//.SetTextAlignment(TextAlignment.CENTER);
                document.Add(membrete);

                //Creo el titulo
                Paragraph titulo = new Paragraph("Formulario AT Nº 3168 Solicitud Comisión de Servicios")
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetBold()
                   .SetFontSize(12)
                   .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40);
                document.Add(titulo);

                //Creo lugar y fecha
                Text t_lugarYFecha = new Text(String.Format("Resistencia, {0}", this.Fecha_confeccion.Value.ToLongDateString()));
                Paragraph lugar_y_fecha = new Paragraph().Add(t_lugarYFecha)
                   .SetTextAlignment(TextAlignment.RIGHT)
                   .SetFontSize(12);
                document.Add(lugar_y_fecha);

                //Creo referencia
                Text t_referencia = new Text("Referencia: ").SetBold();
                Text t_referencia_valor = new Text("Autorización Comisión de Servicios");
                Paragraph referencia = new Paragraph()
                    .Add(t_referencia)
                    .Add(" ")
                    .Add(t_referencia_valor)
                    .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40)
                    .SetFontSize(12);
                document.Add(referencia);

                //Creo el primer texto
                Text t_comunico = new Text(String.Format("Comunico al Señor Administrador General que ésta Dirección/Departamento tiene dispuesto una Comisión de Servicios por {0} ({1}) días a partir del día {2} al {3}, integrada por los agentes consignados a continuación: "
                    , Numalet.ToCardinal(((this.Hasta - this.Desde).Days + 1))
                    , ((this.Hasta - this.Desde).Days + 1).ToString()
                    , this.Desde.ToLongDateString()
                    , this.Hasta.ToLongDateString()
                    ));

                Paragraph comunico = new Paragraph()
                    .Add(t_comunico)
                    .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetFontKerning(FontKerning.YES);
                document.Add(comunico);

                //Creo el jefe de comision
                Text t_jefe = new Text("Jefe de Comisión de Servicios").SetUnderline();
                Paragraph titulo_jefe = new Paragraph().Add("1. ").Add(t_jefe).Add(":")
                     .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/5
                        , /*left*/40)
                    .SetFontSize(12)
                    .SetBold();

                var nom_jefe = this.Nomina.First(x => x.JefeComicion && x.Estado == EstadoAgente1214.Aprobado).Agente;

                Paragraph jefe = new Paragraph().Add("    - ").Add(String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", nom_jefe.ApellidoYNombre, nom_jefe.Legajo, nom_jefe.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato))
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/55)
                    .SetFontSize(12);
                document.Add(titulo_jefe);
                document.Add(jefe);

                //Trabajo con la nomina de agentes
                Text t_otros = new Text("Otros agentes afectados a la Comisión de Servicios").SetUnderline();
                Paragraph titulo_otros = new Paragraph().Add("2. ").Add(t_otros).Add(":")
                     .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/5
                        , /*left*/40)
                    .SetFontSize(12)
                    .SetBold();
                document.Add(titulo_otros);

                var nomina = this.Nomina.Where(x => !x.JefeComicion && !x.Chofer && x.Estado == EstadoAgente1214.Aprobado).ToList();

                foreach (Agente1214 item in nomina)
                {
                    Paragraph otro = new Paragraph().Add("    - ").Add(String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", item.Agente.ApellidoYNombre, item.Agente.Legajo, item.Agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato))
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/0
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/55)
                        .SetFontSize(12);
                    document.Add(otro);
                }

                Text t_destino = new Text(String.Format("{0}", this.Destino)).SetBold();

                Text t_tareas = new Text(String.Format("{0}", this.TareasACumplir)).SetBold();


                Paragraph destino = new Paragraph()
                    .Add("La localidad donde se realizará la Comisión de servicio es ")
                    .Add(t_destino)
                    .Add(" y las tareas a realizar por orden de mi superior son: ")
                    .Add(t_tareas)
                    .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/10
                        , /*left*/40)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetFontKerning(FontKerning.YES);
                document.Add(destino);

                Paragraph p_para_la_presente = new Paragraph("Para la presente comisión estimaré disponer, por donde corresponda, se provea:")
                    .SetMargins(
                        /*top*/15
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);

                document.Add(p_para_la_presente);

                Text t_a = new Text("a. Medio de transporte utilizado:").SetBold();
                Paragraph p_a = new Paragraph().Add(t_a).Add(String.Format(" {0}", this.Movilidad.ToString()))
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_a);

                Text t_b = new Text("b. Anticipo de viáticos:").SetBold();
                Paragraph p_b = new Paragraph().Add(t_b).Add(String.Format(" {0}", this.AnticipoViaticos.ToString("C")))
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_b);

                Text t_c = new Text("c. Anticipo para otros gastos: ").SetBold();
                Paragraph p_c = new Paragraph().Add(t_c).Add(String.Format(" {0} {1}", this.Anticipo.ToString(), this.AnticipoMovilidad.ToString("C")))
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_c);


                Paragraph p_linea_firma = new Paragraph(".................................................                                              .................................................")
                    .SetMargins(
                        /*top*/70
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/40);
                document.Add(p_linea_firma);

                Paragraph p_firma = new Paragraph("Firma del Agente                                                                                                Autorización Superior inmediato")
                    .SetFontSize(9)
                    .SetMargins(
                        /*top*/0
                        ,/*right*/15
                        , /*bottom*/0
                        , /*left*/60);
                document.Add(p_firma);

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));


                Text t_medio = new Text("Medio de Transporte: ").SetBold();

                switch (this.Movilidad)
                {
                    case Movilidad1214.Transporte_publico:
                        Paragraph medio_transporte = new Paragraph().Add(t_medio).Add(String.Format(" Transporte público"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/40
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(medio_transporte);

                        Paragraph p_medio_texto_publico = new Paragraph(String.Format("Para el cumplimiento de la comisión se dede proveer de Pesos {0} ({1}) en concepto de gastos de pasajes.-", Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C")))
                            .SetFontSize(12)
                            .SetTextAlignment(TextAlignment.JUSTIFIED)
                            .SetMargins(
                                /*top*/0
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(p_medio_texto_publico);
                        break;
                    case Movilidad1214.Vehiculo_oficial:
                        var chofer = this.Nomina.FirstOrDefault(x => x.Chofer && x.Estado == EstadoAgente1214.Aprobado);
                        //this.Usa_chofer? String.Format("Agente: {0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", chofer.Agente.ApellidoYNombre, chofer.Agente.Legajo, chofer.Agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato) : "No aplica";

                        Paragraph medio_transporte_oficial = new Paragraph().Add(t_medio).Add(String.Format(" Vehículo oficial"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/40
                                ,/*right*/15
                                , /*bottom*/0
                                , /*left*/40);
                        document.Add(medio_transporte_oficial);

                        Text oficial_dominio = new Text(String.Format(" {0}", this.Vehiculo_dominio)).SetBold();

                        Paragraph p_medio_oficial_texto = new Paragraph(String.Format("Para el cumplimiento de la comisión se dede proveer de Pesos {0} ({1}) en concepto de gastos de movilidad y afectar al vehículo oficial dominio ", Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"))).Add(oficial_dominio);

                        if (this.Usa_chofer)
                        {
                            Text t_chofer = new Text(String.Format(", conducido por {0}", String.Format("{0} - Estrato:{3} - Legajo: {1} - CUIL: {2}", chofer.Agente.ApellidoYNombre, chofer.Agente.Legajo, chofer.Agente.Legajo_datos_laborales.CUIT, this.Estrato1214.Estrato))).SetBold();
                            p_medio_oficial_texto.Add(t_chofer);
                        }

                        p_medio_oficial_texto.SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/40
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                        document.Add(p_medio_oficial_texto);
                        break;
                    case Movilidad1214.Vehiculo_particular:
                        Paragraph medio_transporte_particular = new Paragraph().Add(t_medio).Add(String.Format(" Vehículo particular"))
                            .SetFontSize(12)
                            .SetMargins(
                                /*top*/40
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

                        Paragraph p_medio_particular_texto = new Paragraph(String.Format("Para el cumplimiento de la comisión se dede proveer de Pesos {0} ({1}) en concepto de gastos de movilidad. Datos del vehículo particular: dominio ", Numalet.ToCardinal(this.AnticipoMovilidad), this.AnticipoMovilidad.ToString("C"))).Add(particular_dominio);
                        p_medio_particular_texto.Add(", perteneciente a ").Add(particular_titular).Add(", tipo de combustible ").Add(particular_tipo_combustible).Add(", número de póliza ").Add(particular_poliza).Add(" vigente hasta ").Add(particular_poliza_vigencia).Add(" tipo de cobertura ").Add(particular_poliza_cobertura);
                        p_medio_particular_texto.SetFontSize(12)
                        .SetTextAlignment(TextAlignment.JUSTIFIED)
                        .SetMargins(
                            /*top*/40
                            ,/*right*/15
                            , /*bottom*/0
                            , /*left*/40);
                        document.Add(p_medio_particular_texto);
                        break;

                }

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


                //Cierro el documento
                document.Close();

                res = ms.ToArray();
            }

            return res;
        }

    }
}
