<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Card_Agente.ascx.cs" Inherits="SisPer.Aplicativo.Controles.Card_Agente" %>
<%@ Register Src="~/Aplicativo/Controles/ImagenAgente.ascx" TagPrefix="uc1" TagName="ImagenAgente" %>
<div class="panel panel-primary">
    <div class="panel-heading">
        <h3 class="panel-title">Agente
            <asp:Label Text="" ID="lbl_Id" runat="server" Visible="false" /><asp:Label Text="" ID="lbl_ApyNom" runat="server" /></h3>
    </div>
    <table class="table-condensed">
        <tr>
            <td style="vertical-align: top;">
                <table class="table-condensed">
                    <tr>
                        <td>
                            <strong>Legajo</strong>
                        </td>
                        <td>
                            <asp:Label Text="" ID="lbl_Legajo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>DNI</strong>
                        </td>
                        <td>
                            <asp:Label Text="" ID="lbl_DNI" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Fecha ingreso</strong>
                        </td>
                        <td>
                            <asp:Label Text="" ID="lbl_FechIngreso" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Fecha nacimiento</strong>
                        </td>
                        <td>
                            <asp:Label Text="" ID="lbl_FechNac" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>E-Mail
                            </strong>
                        </td>
                        <td>
                            <asp:Label Text="" ID="lbl_Email" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <uc1:ImagenAgente runat="server" ID="ImagenAgente1" Width="140" Height="140" />
            </td>
        </tr>
    </table>
</div>
