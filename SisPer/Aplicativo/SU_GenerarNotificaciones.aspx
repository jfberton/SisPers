<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SU_GenerarNotificaciones.aspx.cs" Inherits="SisPer.Aplicativo.SU_GenerarNotificaciones" %>

<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuSU runat="server" ID="MenuSU" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Generar notificaciones</h2> <span class="small">Ojo! no controla si ya fueron generadas, el ejecutar varias veces esta función duplicará las notificaciones al agente</span>
    <br />
    <br />
    <asp:Button Text="Generar notificaciones pendientes" CssClass="btn btn-lg btn-danger" OnClick="btn_generar_notificaciones_pendientes_Click" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
