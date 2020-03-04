<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SU_Main.aspx.cs" Inherits="SisPer.Aplicativo.SU_Main" %>

<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuSU runat="server" id="MenuSU" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Pantalla Super Usuario</h2>
    
</asp:Content>
