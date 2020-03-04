<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Ayuda_Agente.aspx.cs" Inherits="SisPer.Aplicativo.Ayuda_Agente" %>

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
                <h2>Videos ayuda Agente</h2>
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
                                            <ul style="padding-left: 20px; list-style: none">
                                                <li><strong><span class="auto-style1"><span class="auto-style2">Menú</span>
                                                </span></strong>
                                                    <ul style="padding-left: 20px; list-style: none">
                                                        <li class="auto-style1"><strong>2 - Inicio</strong></li>
                                                        <li class="auto-style1"><strong>3 - Mis Horas</strong></li>
                                                    </ul>
                                                </li>
                                                <li><strong><span class="auto-style1"><span class="auto-style2">Pantalla Principal</span>
                                                </span></strong>
                                                    <ul style="padding-left: 20px; list-style: none">
                                                        <li class="auto-style1"><strong>4 - Horarios vespertinos</strong></li>
                                                        <li class="auto-style1"><strong>5 - Francos compensatorios</strong></li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </li>


                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel5"
                                Titulo="Menú"
                                Subtitulo="Inicio"
                                Subtitulo2="Pantalla principal"
                                Descripcion="<p>Desde aquí podremos:</p> <ul><li>• Terminar salidas diarias</li><li>• Solicitar horarios vespertinos</li><li>• Solicitar francos compensatorios</li><li>• Verificar marcaciones de entrada salida</li></ul>"
                                Href="../../Imagenes/Videos/Agente pantalla principal.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel1"
                                Titulo="Menú"
                                Subtitulo="Mis Horas"
                                Descripcion="Descripción de los datos que se visualizan en cada una de las secciones."
                                Href="../../Imagenes/Videos/Menu Jefe - Mis Horas.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel7"
                                Titulo="Pantalla Principal"
                                Subtitulo="Horarios Vespertinos"
                                Descripcion="Descripción de como solicitar horarios vespertinos"
                                Href="../../Imagenes/Videos/Agente horarios Vespertinos.mp4" />
                        </li>
                        <li>
                            <uc1:ItemCarousel runat="server" ID="ItemCarousel8"
                                Titulo="Pantalla Principal"
                                Subtitulo="Francos compensatorios"
                                Descripcion="Descripción de como solicitar francos compensatorios"
                                Href="../../Imagenes/Videos/Agente francos compensatorios.mp4" />
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
