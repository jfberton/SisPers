<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtorgarBonificacion.ascx.cs" Inherits="SisPer.Aplicativo.Controles.OtorgarBonificacion" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<table class="table-condensed" style="background-color: #FFFFFF; border-color: white" border="0">
    <tr style="background-color: #333333; color: #FFFFFF; font-weight: bold">
        <td colspan="2">Otorgar Bonificación</td>
    </tr>
    <tr>
        <td colspan="2">Beneficiario: &nbsp<asp:Label ID="Label1" Text="" runat="server" Font-Bold="True" />
        </td>
    </tr>
    <tr>
        <td>Horas por bonificación</td>
        <td>
            <input type="hidden" id="AgenteId" runat="server" value="valor" />
            <asp:TextBox runat="server" ID="tb_horasABonificar" ClientIDMode="Static" Width="50px" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="Button1" Text="Cancelar" runat="server" /></td>
        <td>
            <asp:Button Text="Aceptar" ID="btn_AceptarBonificacion" OnClick="btn_AceptarBonificacion_Click" runat="server" /></td>
    </tr>
</table>
