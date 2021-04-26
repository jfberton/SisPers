<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="MainPandemia.aspx.cs" Inherits="SisPer.Aplicativo.MainPandemia" %>


<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc3" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>
<%@ Register Src="~/Aplicativo/Controles/AdministrarDiaAgente.ascx" TagPrefix="uc1" TagName="AdministrarDiaAgente" %>
<%@ Register Src="~/Aplicativo/Controles/VisualizarDiaAgente.ascx" TagPrefix="uc1" TagName="VisualizarDiaAgente" %>

<script runat="server" type="text/c#">
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc3:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc2:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container">
        <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
        <uc1:DatosAgente ID="DatosAgente1" runat="server" />
         <h1>Atención!</h1>
         <p>Ante la situación de público conocimiento respecto al coronavirus (COVID-19), y de acuerdo con los lineamientos de asistencia, se torna inviable la carga y control de movimientos diarios generados fuera del sistema, por lo que el mismo se verá reducido en sus funcionalidades originales. </p>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
