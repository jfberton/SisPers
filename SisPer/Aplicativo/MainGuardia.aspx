<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="MainGuardia.aspx.cs" Inherits="SisPer.Aplicativo.MainGuardia" %>

<%--<%@ Register Src="~/Aplicativo/Controles/ListadoAgentes.ascx" TagPrefix="uc1" TagName="ListadoAgentes" %>--%>
<%@ Register Src="~/Aplicativo/Menues/MenuGuardia.ascx" TagPrefix="uc1" TagName="MenuGuardia" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuGuardia runat="server" ID="MenuGuardia" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<uc1:ListadoAgentes runat="server" ID="ListadoAgentes" />--%>
</asp:Content>
