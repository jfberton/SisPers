<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_AgentesPorArea.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_AgentesPorArea" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc2" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Reportes/AgentesPorAreaUC.ascx" TagPrefix="uc3" TagName="AgentesPorAreaUC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe1" />
    <uc2:MenuPersonalAgente runat="server" ID="MenuPersonalAgente1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc3:AgentesPorAreaUC runat="server" ID="AgentesPorAreaUC" />
</asp:Content>
