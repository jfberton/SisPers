<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Marcaciones_Procesar_Dia.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Marcaciones_Procesar_Dia" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Aplicativo/Controles/AdministrarDiaAgente.ascx" TagPrefix="uc1" TagName="AdministrarDiaAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:AdministrarDiaAgente runat="server" ID="AdministrarDiaAgente" Visible="false" OnCerroElDia="AdministrarDiaAgente_CerroElDia" OnPrecionoVolver="AdministrarDiaAgente_PrecionoVolver" />
</asp:Content>
