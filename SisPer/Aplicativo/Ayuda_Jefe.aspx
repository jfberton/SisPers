<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Ayuda_Jefe.aspx.cs" Inherits="SisPer.Aplicativo.Ayuda_Jefe" %>

<%@ Register Src="~/Aplicativo/Controles/ItemCarousel.ascx" TagPrefix="uc1" TagName="ItemCarousel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            font-size: medium;
        }
        .auto-style2 {
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">

    <script src="../Slider/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="../Slider/js/jquery.min.js"><\/script>')</script>

    <link rel="stylesheet" href="../Slider/css/anythingslider.css" />
    <script src="../Slider/js/jquery.anythingslider.js"></script>

    <script>
        // Set up Sliders
        // **************
        $(function () {

            $('#slider2').anythingSlider({
                theme: 'default',
                mode: 'f',   // fade mode - new in v1.8!
                resizeContents: false,
                buildNavigation: true,
                navigationSize: 15,
                startText: '',
                enableStartStop: false
            });

            // tooltips for first demo
            $.jatt();

        });
    </script>

    <%--<script type="text/javascript" src="../Player/jwplayer.js"></script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table width="900px">
        <tr>
            <td align="center">
                <h2>Videos ayuda Jefe</h2>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <ul id="slider2">
                        <li>
                            <table width="700px">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label runat="server" ID="lbl_titulo" Style="padding: 5px; font-size: large; font-weight: bold; color: #000080">Listado de videos:</asp:Label>
                                            <ul style="padding-left:20px;list-style:none">
                                                <li><strong><span class="auto-style1"><span class="auto-style2">Menú</span>
                                                    </span></strong>
                                                    <ul style="padding-left:20px;list-style:none">
                                                        <li class="auto-style1"><strong>02 - Inicio</strong></li>
                                                        <li class="auto-style1"><strong>03 - Mis Horas</strong></li>
                                                        <li class="auto-style1"><strong>04 - Marcar entrada salida</strong></li>
                                                        <li class="auto-style1"><strong>05 - Últimos movimientos</strong></li>
                                                        <li class="auto-style1"><strong>06 - Salidas diarias</strong></li>
                                                    </ul>
                                                </li>
                                                <li><strong><span class="auto-style1"><span class="auto-style2">Listado de agentes</span>
                                                     </span></strong>
                                                     <ul style=" padding-left: 20px;list-style:none">
                                                         <li class="auto-style1"><strong>07 - Detalle</strong></li>
                                                         <li><strong><span class="auto-style1">08 - Administrar
                                                            </span></strong>
                                                            <ul style=" padding-left: 20px;list-style:none">
                                                                <li class="auto-style1"><strong>09 - Pantalla principal agente</strong></li>
                                                                <li class="auto-style1"><strong>10 - Salidas diarias</strong></li>
                                                                <li class="auto-style1"><strong>11 - Horarios vespertinos</strong></li>
                                                                <li class="auto-style1"><strong>12 - Francos compensatorios</strong></li>
                                                            </ul>
                                                         </li>
                                                         <li class="auto-style1"><strong>13 - Solicitar</strong></li>
                                                     </ul>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel"
                                Titulo="Menú"
                                Subtitulo=" Inicio"
                                Subtitulo2="Pantalla principal"
                                Descripcion="Descripción de secciones y funcionalidades."
                                Href="../../Imagenes/Videos/Menu Jefe - Inicio.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel1"
                                Titulo="Menú"
                                Subtitulo="Mis Horas"
                                Descripcion="Descripción de los datos que se visualizan en cada una de las secciones."
                                Href="../../Imagenes/Videos/Menu Jefe - Mis Horas.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel2"
                                Titulo="Menú"
                                Subtitulo="Marcar entrada salida"
                                Descripcion="Funcionalidad disponible únicamente para Jefes de receptorias, ya que sus agentes no poseen sismtema de marcación de tarjetas ni huellas digitales"
                                Href="../../Imagenes/Videos/Menu Jefe - Marcar entrada salida.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel10"
                                Titulo="Menú"
                                Subtitulo="Últimos movimientos"
                                Descripcion="Salidas del dia, feriados, horarios vespertinos, y francos compensatorios por venir."
                                Href="../../Imagenes/Videos/Menu Jefe - Ultimos movimientos.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel11"
                                Titulo="Menú"
                                Subtitulo="Salidas diarias"
                                Descripcion="Muestra como generar el informe de salidas diarias que se presenta a Personal."
                                Href="../../Imagenes/Videos/Menu Jefe - Salidas diarias.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel3"
                                Titulo="Listado de agentes"
                                Subtitulo="Detalle"
                                Descripcion="Pantalla 'Mis Horas' pero con los datos del agente seleccionado"
                                Href="../../Imagenes/Videos/Listado agentes - Detalle.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel4"
                                Titulo="Listado de agentes"
                                Subtitulo="Administrar"
                                Descripcion="<p>Nos lleva a la pantalla principal del agente.</p>"
                                Href="../../Imagenes/Videos/Listado agentes - Administrar.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel5"
                                Titulo="Listado de agentes"
                                Subtitulo="Administrar"
                                Subtitulo2="PANTALLA PRINCIPAL DEL AGENTE"
                                Descripcion="<p>Desde aquí podremos:</p> <ul><li>• Iniciar o terminar salidas diarias</li><li>• Solicitar horarios vespertinos</li><li>• Solicitar francos compensatorios</li><li>• Verificar marcaciones de entrada salida</li></ul>"
                                Href="../../Imagenes/Videos/Agente pantalla principal.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel6"
                                Titulo="Listado de agentes"
                                Subtitulo="Administrar"
                                Subtitulo2="SALIDAS DIARIAS"
                                Descripcion="<p>Descripción de como iniciar las salidas diarias.</p><p>Las salidas no terminadas se terminan automáticamente al dia siguiente de la siguiente manera:<p/><ul><li>• Si fue iniciada a la mañana se finaliza con hora 13:00</li><li>• Si fue iniciada durante la tarde se finaliza con hora 20:00</li></ul>"
                                Href="../../Imagenes/Videos/Agente salidas diarias.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel7"
                                Titulo="Listado de agentes"
                                Subtitulo="Administrar"
                                Subtitulo2="HORARIOS VESPERTINOS"
                                Descripcion="Descripción de como solicitar horarios vespertinos"
                                Href="../../Imagenes/Videos/Agente horarios Vespertinos.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel8"
                                Titulo="Listado de agentes"
                                Subtitulo="Administrar"
                                Subtitulo2="FRANCOS COMPENSATORIOS"
                                Descripcion="Descripción de como solicitar francos compensatorios"
                                Href="../../Imagenes/Videos/Agente francos compensatorios.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel9"
                                Titulo="Listado de agentes"
                                Subtitulo="Solicitar"
                                Descripcion="<p>Desde aquí podremos solicitar:</p> <ul><li>• Enfermedad común</li><li>• Atención a familiar</li><li>• Licencia por exámen</li><li>• Donación de sangre</li><li>• Comisión</li></ul>"
                                Href="../../Imagenes/Videos/Listado agentes - Solicitar.mp4" />
                        </li>
                    </ul>
                </div>
            </td>
        </tr>
    </table>

    <script src="../Player/flowplayer-3.2.12.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        flowplayer("a.player", "../Player/flowplayer-3.2.16.swf", {
            plugins: {
                pseudo: { url: "../Player/flowplayer.pseudostreaming-3.2.12.swf" }
            },
            clip: { provider: 'pseudo', autoPlay: false },
        });
    </script>

</asp:Content>
