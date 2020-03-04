<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemMensaje.ascx.cs" Inherits="SisPer.Aplicativo.Controles.ItemMensaje" %>

<input type="hidden" runat="server" id="divCuerpo" />
<input type="hidden" runat="server" id="CuerpoMensaje" />
<div class="row">
    <div class="col-md-12">
        <a runat="server" onserverclick="lbl_Asunto_Click">
            <div id="row_mensaje" runat="server" class="row btn-default btn-block well-mio">
                <div class="col-md-12">
                    <asp:Label Text="Enviado Por" ID="lbl_EnviadoPor" Font-Size="11" runat="server" />
                </div>
                <div class="col-md-7">
                    <asp:Label runat="server" ID="lbl_Asunto" Font-Size="9" ForeColor="Gray" />
                </div>
                <div class="col-md-5">
                    <asp:Label Text="dd/mm/yyyy" ID="lbl_fechaEnvio" CssClass="text-right" runat="server" Font-Size="8" />
                </div>

                <div class="col-md-12">
                    <span class="small">
                        <asp:Label ID="lbl_mensaje" runat="server" /></span>
                </div>
            </div>
        </a>

    </div>
</div>

