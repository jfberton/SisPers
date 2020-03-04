<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SU_Mantenimiento.aspx.cs" Inherits="SisPer.Aplicativo.SU_Mantenimiento" %>

<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuSU runat="server" id="MenuSU1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div runat="server" id="EnMantenimiento"><h2>La página se encuentra ahora en modo MANTENIMIENTO</h2></div>
    <div runat="server" id="Operativa"><h2>La página se encuentra ahora en modo OPERATIVO</h2></div>
</asp:Content>
