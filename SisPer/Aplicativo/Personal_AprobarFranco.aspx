<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_AprobarFranco.aspx.cs" Inherits="SisPer.Aplicativo.Personal_AprobarFranco" %>


<%@ Register src="Menues/MenuPersonalAgente.ascx" tagname="MenuPersonalAgente" tagprefix="uc1" %>
<%@ Register src="Menues/MenuPersonalJefe.ascx" tagname="MenuPersonalJefe" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc1:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        SOLICITUD DE FRANCO COMPENSATORIO</h2>
    <p />
    Se solicita autorización para hacer uso de las horas a mi favor, debidamente registradas
    y autorizadas, realizadas en horario vespertino. &nbsp;<asp:ValidationSummary ID="ValidationSummary1"
        runat="server" DisplayMode="BulletList" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
    <p />
    <table style="width: 100%">
        <tr>
            <td colspan="3">
                <b><u>01 - FRANCO COMPENSATORIO PARA EL:</u></b>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Días:</strong>
                <asp:Label Text="" ID="lbl_Dias" runat="server" />
            </td>
            <td>
                <strong>Mes:</strong>
                <asp:Label Text="" ID="lbl_Mes" runat="server" />
            </td>
            <td>
                <strong>Año:</strong>
                <asp:Label Text="" ID="lbl_Anio" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <strong>Legajo:</strong>
                <asp:Label Text="" ID="lbl_Legajo" runat="server" />
            </td>
            <td colspan="2">
                <strong>Apellido y Nombre:</strong> <asp:Label Text="" ID="lbl_NomyAp" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>Depto/Area:</strong>
                <asp:Label Text="" ID="lbl_Area" runat="server" />
            </td>
            <td>
                <strong>Fecha solicitud:</strong>
                <asp:Label Text="" ID="lbl_FechaFirmaAgente" runat="server" />
            </td>
        </tr>
         <tr>
        <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <strong><u>02 - TOMA CONOCIMIENTO Y AUTORIZA DE CORRESPONDER:</u></strong>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Legajo:</strong>
                <asp:Label Text="" ID="lbl_legajoJefe" runat="server" />
            </td>
            <td colspan="2">
                <strong>Apellido y Nombre: </strong><asp:Label Text="" ID="lbl_NomyApJefe" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>Depto/Area: </strong>
                <asp:Label Text="" ID="lbl_AreaJefe" runat="server" />
            </td>
            <td>
            <strong>Fecha aprobación: </strong>
                <asp:Label Text="" ID="lbl_FechaFirmaJefe" runat="server" />
            </td>
        </tr>
        <tr>
        <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <b><u>03 - INFORME DEPARTAMENTO PERSONAL:</u></b>
                <br />
                Se verifico que el/la agente regitra horas en:
            </td>
        </tr>
        <tr>
            <td colspan="3">
                A) Saldo año anterior (&nbsp;<asp:Label Text="" ID="lbl_AnioAnterior" 
                    runat="server" style="font-weight: 700" />
                &nbsp;caducan el
                <asp:Label Text="" ID="lbl_MediadoDeEsteAnio" runat="server" />&nbsp;)
                
            </td>
        </tr>
        <tr>
            <td colspan="3">
                B) Prolongación de jornada año (
                <asp:Label Text="" ID="lbl_anioActual" runat="server" 
                    style="font-weight: 700" />&nbsp;)
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="text-align: left">
                <asp:Button Text="Aprobar" ID="btn_Aprobar" runat="server" 
                    onclick="btn_Aprobar_Click" />
                <asp:Button Text="Rechazar" ID="btn_Rechazar" CausesValidation="false" OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;"
                    runat="server" onclick="btn_Rechazar_Click" />
            </td>
            <td style="text-align: right">
                <asp:Button Text="Volver" ID="btn_Volver" runat="server" 
                    CausesValidation="false" onclick="btn_Volver_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
