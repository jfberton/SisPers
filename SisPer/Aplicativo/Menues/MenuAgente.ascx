<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuAgente.ascx.cs" Inherits="SisPer.Aplicativo.Menues.MenuAgente" %>
<%@ Register Src="~/Aplicativo/Controles/ImagenAgente.ascx" TagPrefix="uc1" TagName="ImagenAgente" %>

<script type="text/javascript">
    function actualizarFechaHora() {
        var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        var fecha = new Date();
        var h = fecha.getHours();
        var m = fecha.getMinutes();
        var s = fecha.getSeconds();
        m = checkTime(m);
        s = checkTime(s);
        document.getElementById('<%= lbl_fecha_hora.ClientID %>').innerHTML = fecha.getDate() + " de " + meses[fecha.getMonth()] + " de " + fecha.getFullYear() + ", " + h + ":" + m + ":" + s;
        var t = setTimeout(function () { actualizarFechaHora() }, 500);
    }

    function checkTime(i) {
        if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
        return i;
    }
</script>

<div class="navbar-wrapper">
    <div class="container">
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#">Sistema Personal</a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li><a href="../Aplicativo/MainPandemia.aspx">Inicio</a></li>
                        <%--<li><a href="../Aplicativo/MainAgente.aspx">Inicio</a></li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Mis horas
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Ag_Detalle.aspx">Mis Horas</a></li>
                                <li><a href="../Aplicativo/ag_dias_mes.aspx">Detalle mensual</a></li>
                            </ul>
                        </li>
                        <li><a href="../Aplicativo/Ag_Memo17.aspx">Memo 17</a></li>
                        <li><a href="../Legislacion.aspx" target="_blank">Legislación</a></li>--%>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Form AT Nº 3168
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Form1214_Nuevo_.aspx">Nuevo AT Nº 3168</a></li>
                                <li><a href="../Aplicativo/Formulario1214_Generados.aspx">Generados</a></li>
                            </ul>
                        </li>
                        <%--<li><a href="../Aplicativo/Ayuda_Agente.aspx" target="_blank">Ayuda</a></li>--%>
                    </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li>
                            <a href="../Aplicativo/Notificaciones_Inbox.aspx">
                                <span class="glyphicon glyphicon-exclamation-sign" data-toggle="tooltip" data-placement="left" title="Notificaciones">
                                    <span class="badge" runat="server" id="notificaciones">
                                        <small>
                                            <asp:Label Text="" ID="lbl_notificacionesNuevas" runat="server" /></small></span>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="../Aplicativo/Inbox.aspx">
                                <span class="glyphicon glyphicon-envelope" data-toggle="tooltip" data-placement="left" title="Mensajes">
                                    <span class="badge" runat="server" id="mensajes">
                                        <small>
                                            <asp:Label Text="" ID="lbl_mensajesNuevos" runat="server" /></small></span>
                                </span>
                            </a>
                        </li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false" onclick="actualizarFechaHora();">
                                <span class="glyphicon glyphicon-user" data-toggle="tooltip" data-placement="left" title="Administrar cuenta"></span>
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li class="dropdown-header">
                                    <asp:Label Text="" ID="lbl_fecha_hora" runat="server" /></li>
                                <li class="divider"></li>
                                <li class="dropdown-header">
                                    <asp:Label Text="" ID="lbl_ApyNom" runat="server" />
                                    <uc1:ImagenAgente runat="server" ID="ImagenAgente1" />
                                </li>
                                <li>
                                    <asp:LinkButton Text="Ver datos legajo" ID="lbl_datos_legajo" CausesValidation="false" OnClick="lbl_datos_legajo_Click" runat="server" />
                                </li>
                                <li>
                                    <asp:LinkButton Text="Editar datos" ID="lbl_Editar" CausesValidation="false" OnClick="lbl_Editar_Click" runat="server" />
                                </li>
                                <li>
                                    <asp:LinkButton Text="Cambiar clave" ID="lbl_CambiarClave" CausesValidation="false" OnClick="lbl_CambiarClave_Click" runat="server" />
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton Text="Cerrar sesión" ID="lbl_logout" CausesValidation="false" OnClick="lbl_logout_Click" runat="server" /></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</div>


