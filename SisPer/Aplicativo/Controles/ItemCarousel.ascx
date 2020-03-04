<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemCarousel.ascx.cs" Inherits="SisPer.Aplicativo.Controles.ItemCarousel" %>

<table style="width: 700px; height: 320px;">
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td>
                        <asp:label runat="server" id="lbl_titulo" style="padding: 5px; font-size: x-large; font-weight: bold; color: #000080"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:label runat="server" id="lbl_subtitulo" style="padding: 5px; font-size: large; font-weight: bold; color: #000080"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:label runat="server" id="lbl_Subtitulo2" style="padding: 5px; font-size: larger; font-weight: bold; color: #000080"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 20px 5px 5px 5px">
                        <h3><asp:Label Text="" ID="lbl_descrip" runat="server" /></h3></td>
                </tr>
            </table>
        </td>
        <td>
            <a runat="server" id="video" class="player" style="height: 300px; width: 400px; display: block" href="" />
        </td>
    </tr>
</table>
