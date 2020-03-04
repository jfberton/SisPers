<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuSU.ascx.cs" Inherits="SisPer.Aplicativo.Menues.MenuSU" %>

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
                    <a class="navbar-brand" href="../Aplicativo/SU_Main.aspx">Inicio</a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li><a href="../Aplicativo/SU_Errores.aspx">Ver log Errores</a></li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Mantenimiento <span class="caret"></span></a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/SU_Mantenimiento.aspx?Mant=1">Poner en Mantenimiento</a></li>
                                <li><a href="../Aplicativo/SU_Mantenimiento.aspx?Mant=0">Volver a Operativo</a></li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Prueba de funciones<span class="caret"></span></a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="../Aplicativo/SU_GenerarNotificaciones.aspx">Generar Notificaciones</a></li>
                                <li><a href="../Aplicativo/SU_CalculoHoras.aspx">Probar cálculo de horas</a></li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton Text="Cerrar sesión" ID="lbl_logout" OnClick="lbl_logout_Click" runat="server" /></li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</div>
