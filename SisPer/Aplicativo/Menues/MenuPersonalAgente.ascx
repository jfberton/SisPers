<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuPersonalAgente.ascx.cs" Inherits="SisPer.Aplicativo.Menues.MenuPersonalAgente" %>
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
                        <li><a href="../Aplicativo/MainPersonal.aspx">
                            <span class="glyphicon glyphicon-home" data-toggle="tooltip" data-placement="left" title="Pantalla principal"></span>
                        </a></li>
                        <li><a href="../Aplicativo/Personal_Area_Listado.aspx">Areas</a></li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Agentes
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Personal_Ag_Listado.aspx">Administrar</a></li>
                                <li><a href="../Aplicativo/Personal_Ag_AgregarHoraDia.aspx">Agendar horas</a></li>
                                <li><a href="../Aplicativo/Personal_Ag_AgregarMovimientoDia.aspx">Agendar movimiento</a></li>
                                <li><a href="../Aplicativo/Personal_Ag_Nuevo.aspx">Nuevo agente</a></li>
                                <li><a href="../Aplicativo/personal_ReasignarAgentes.aspx">Reasignaciones</a></li>
                                <li><a href="../Aplicativo/Personal_Licencias.aspx">Licencias</a></li>
                            </ul>
                        </li>

                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Huellas
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Personal_Huellas.aspx">Marcaciones</a></li>
                                <li><a href="../Aplicativo/Personal_Cerrar_EntradaSalida.aspx">Marcaciones manuales</a></li>
                                <li><a href="../Aplicativo/Personal_Marcaciones_Procesar_Agente.aspx">Procesar mes agente</a></li>
                                <li class="divider"></li>
                                <li><a href="../Aplicativo/Personal_Feriado_Listado.aspx">Feriados</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Informes
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Jefe_Memo17.aspx">Memo 17</a></li>
                                <li class="divider"></li>
                                <li><a href="../Aplicativo/Personal_Informe_AgentesPorArea.aspx">Listado de Agentes</a></li>
                                <li><a href="../Aplicativo/Personal_Informe_Bonificaciones.aspx">Bonificaciones</a></li>
                                <li><a href="../Aplicativo/Personal_Informe_SalidasVarias.aspx">Movimientos varios</a></li>
                                <li><a href="../Aplicativo/Personal_Informe_InformeMovimientosAnuales.aspx">Movimientos anuales</a></li>
                                <li class="divider"></li>
                                <li><a href="../Legislacion.aspx" target="_blank">Legislación</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Form 1214
                                <span class="badge" runat="server" id="novedades214">
                                    <small>
                                        <asp:Label Text="" ID="lbl_novedades214" runat="server" /></small></span>
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/Form1214_Nuevo_.aspx">Nuevo 1214</a></li>
                                <li><a href="../Aplicativo/Formulario1214_Solicitudes.aspx">Solicitudes
                                    <span class="badge" runat="server" id="solicitudes">
                                        <small>
                                            <asp:Label Text="" ID="lbl_solicitudes" runat="server" /></small></span>
                                </a></li>
                                <li><a href="../Aplicativo/Formulario1214_Generados.aspx">Generados</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Menu agente
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/MainAgente.aspx">Pantalla Agente</a></li>
                                <li><a href="../Aplicativo/Ag_Detalle.aspx">Mis Horas</a></li>
                                <li><a href="../Aplicativo/ag_dias_mes.aspx">Detalle mensual</a></li>
                            </ul>
                        </li>

                    </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                <span class="glyphicon glyphicon-exclamation-sign" data-toggle="tooltip" data-placement="left" title="Notificaciones">
                                    <span class="badge" runat="server" id="notificaciones">
                                        <small>
                                            <asp:Label Text="" ID="lbl_notificacionesNuevas" runat="server" /></small></span>
                                </span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li class="dropdown-header"></li>
                                <li>
                                    <a href="../Aplicativo/Notificaciones.aspx">Crear nueva</a>
                                </li>
                                <li>
                                    <a href="../Aplicativo/Notificaciones_Inbox.aspx">Recibidas</a>
                                </li>
                                <li>
                                    <a href="../Aplicativo/Notificaciones_Generadas.aspx">Generadas</a>
                                </li>
                            </ul>
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
                        <li><a href="../Manual/AgentePersonal.pdf" target="_blank">
                            <span class="glyphicon glyphicon-question-sign" data-toggle="tooltip" data-placement="left" title="Ayuda"></span>
                        </a></li>
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
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton Text="Editar datos" ID="lbl_Editar" CausesValidation="false" OnClick="lbl_Editar_Click" runat="server" />
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
