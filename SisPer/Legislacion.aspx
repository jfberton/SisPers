<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Legislacion.aspx.cs" Inherits="SisPer.Legislacion" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <asp:Panel runat="server" ID="panelMenuSinIngresar" Visible="false">
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="default.aspx">Sistema Personal <span class="glyphicon glyphicon-home small" data-toggle="tooltip" data-placement="left" title="Pantalla principal"></span></a>
                </div>
                <div id="navbar" class="collapse navbar-collapse">
                    <ul class="nav navbar-nav">
                        <li><a href="Legislacion.aspx">Legislación</a></li>
                        <li><a href="#">Acerca de</a></li>
                    </ul>
                </div>
            </div>
        </nav>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="jumbotron">
            <h1>Legislación <span class="small">Sistema Personal</span></h1>
            <p>En esta sección usted podrá consultar sobre las distintas normativas por las cuales estan regidos los derechos y obligaciones del agente</p>
        </div>
        <h1>Leyes</h1>
        <div class="row">
            <div class="col-lg-4">
                <h3>Ley 2017 <span class="small">Estatuto Emp. Pub.</span></h3>
                <p>ESTATUTO PARA EL PERSONAL DE LA ADMINISTRACION PUBLICA PROVINCIAL - LEY N.2017 (T.A.) - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/1 - Ley 2017  Estatuto Emp Pub.pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>
            <div class="col-lg-4">
                <h3>Ley 2018 <span class="small">Directores</span></h3>
                <p>REGLAMENTA LA FUNCION DE DIRECTOR - LEY N.2018 (T.A.) - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/2 - Ley 2018  Directores.pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>
            <div class="col-lg-4">
                <h3>Ley  3521 <span class="small">Licencias y Permisos</span></h3>
                <p>REGIMEN DE LICENCIAS PARA LA ADMINISTRACION PUBLICA PROVINCIAL - LEY N. 3521 - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/3 - Ley  3521 Licencias y Permisos.pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4">
                <h3>Ley 330 <span class="small">Fondo Estimulo</span></h3>
                <p>LEY ORGANICA DE LA DIRECCION DE RENTAS - LEY N.330 (T.A.) - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/4 - Ley 330  Fondo Estimulo.pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>
            <div class="col-lg-4">
                <h3>Ley 1276 <span class="small">Escalafon</span></h3>
                <p>ESCALAFON PARA LA ADMINISTRACION PUBLICA PROVINCIAL - LEY N. 1276 - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/5 - Ley 1276  Escalafon .pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>
            <%--<div class="col-lg-4">
                <h3>Ley 2943 <span class="small">Incompatinilidad</span></h3>
                <p>REGIMEN DE INCOMPATIBILIDADES PARA LA ADMINISTRACION PUBLICA - LEY N. 2943 - (T.A.) - USO INTERNO</p>
                <p><a class="btn btn-default" href="Normativa/6 - Ley 2943 Incompatinilidad.pdf" target="_blank" role="button">Ver &raquo;</a></p>
            </div>--%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
