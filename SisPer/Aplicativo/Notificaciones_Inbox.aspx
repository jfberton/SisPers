<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Notificaciones_Inbox.aspx.cs" Inherits="SisPer.Aplicativo.Notificaciones_Inbox" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div runat="server" id="p_agente" visible="false">
        <div class="row">
            <div class="col-md-8">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Notificaciones del agente.</h3>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gv_Notificaciones" Width="100%" runat="server" EmptyDataText="No existen notificaciones para mostrar."
                            AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gv_Notificaciones_PageIndexChanging"
                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Número" ReadOnly="true" SortExpression="Id" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="Descripcion" />
                                <asp:BoundField DataField="Generada" HeaderText="Generada" ReadOnly="true" SortExpression="Generada" />
                                <asp:BoundField DataField="Notificada" HeaderText="Notificada" ReadOnly="true" SortExpression="Notificada" />
                                <asp:BoundField DataField="Enviada" HeaderText="Enviada" ReadOnly="true" SortExpression="Enviada" />
                                <asp:TemplateField HeaderText="Observ.">
                                    <ItemTemplate>
                                        <asp:Image ImageUrl='<%#Eval("PathImagenObservada")%>' Width="16px" Height="16px" ToolTip='<%#Eval("Observaciones")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Recibida" HeaderText="Recibida" ReadOnly="true" SortExpression="Recibida" />
                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_VerNotificacionAgente" runat="server" CommandArgument='<%#Eval("Id")%>' ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_VerNotificacionAgente_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-default" id="p_DescripcionNotifAg" runat="server" visible="false">
                    <div class="panel-heading">
                        <h3>Descripción de la notificación</h3>
                    </div>
                    <div class="panel-body">
                        <div runat="server" class="row " id="div_vto">
                            <div class="col-md-12">
                                <b>Vence el</b>
                                <asp:Label Text="" ID="lbl_vto" runat="server" />
                                <span class="small">
                                    <asp:Label Text="" ID="lbl_diasalvto" runat="server" /></span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <b>Número</b>
                            </div>
                            <div class="col-me-8">
                                <asp:Label Text="" ID="lbl_NumNotifAg" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <b>Tipo</b>
                            </div>
                            <div class="col-me-8">
                                <asp:Label Text="" ID="lbl_TipoNotifAg" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <b>De</b>
                            </div>
                            <div class="col-me-8">
                                <asp:Label Text="" ID="lbl_DeNotifAg" runat="server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <b>Descripción:</b>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <textarea class="well-sm" runat="server" style="width: 100%;" rows="3" id="tb_descripcion" disabled="disabled"></textarea>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <b>Observaciones:</b>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <textarea class="well-sm" style="width: 100%;" rows="3" runat="server" id="tb_observada" disabled="disabled"></textarea>
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button Text="Enviar" ID="btn_EnviarNotifAg" OnClick="btn_EnviarNotifAg_Click" OnClientClick="if (!confirm('Esta por marcar como ENVIADA la notificación, desea continuar?')) return false;" CssClass="btn btn-success" runat="server" />
                                <asp:Button Text="Volver" ID="btn_CancelarNotifAg" OnClick="btn_CancelarNotifAg_Click" CssClass="btn btn-default" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div runat="server" id="p_personal" visible="false">

        <%--NOTIFICACIONES ENVIADAS--%>
        <div class="row">
            <div class="col-md-8">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Notificaciones al Dpto. Personal por recibir </h3>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gv_NotificacionesEnviadas" runat="server" EmptyDataText="No existen notificaciones para mostrar."
                            AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gv_NotificacionesEnviadas_PageIndexChanging"
                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Número" ReadOnly="true" SortExpression="Id" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="Descripcion" />
                                <asp:BoundField DataField="Generada" HeaderText="Generada" ReadOnly="true" SortExpression="Generada" />
                                <asp:BoundField DataField="Notificada" HeaderText="Notificada" ReadOnly="true" SortExpression="Notificada" />
                                <asp:BoundField DataField="Enviada" HeaderText="Enviada" ReadOnly="true" SortExpression="Enviada" />
                                <asp:TemplateField HeaderText="Observ.">
                                    <ItemTemplate>
                                        <asp:Image ImageUrl='<%#Eval("PathImagenObservada")%>' Width="16px" Height="16px" ToolTip='<%#Eval("Observaciones")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ver">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_VerNotificacionEnviada" runat="server" CommandArgument='<%#Eval("Id")%>' ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_VerNotificacionEnviada_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-default" id="p_DescripcionNotifEnviada" runat="server" visible="false">
                    <div class="panel-heading">
                        <h3>Descripción de la notificación</h3>
                    </div>
                    <div class="panel-body">
                        <div runat="server" class="row " id="div_vtoEnviado">
                            <div class="col-md-12">
                                <b>Vence el</b>
                                <asp:Label Text="" ID="lbl_fechaVtoEnviado" runat="server" />
                                <span class="small">
                                    <asp:Label Text="" ID="lbl_diasAVencerEnviado" runat="server" /></span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <b>Número</b>
                            </div>
                            <div class="col-me-3">
                                <asp:Label Text="" ID="lbl_NumNotifEnviada" runat="server" />
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <b>Tipo</b>
                            </div>
                            <div class="col-me-9">
                                <asp:Label Text="" ID="lbl_TipoNotifEnviada" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <b>De</b>
                            </div>
                            <div class="col-me-9">
                                <asp:Label Text="" ID="lbl_DeNotifEnviada" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <b>Para</b>
                            </div>
                            <div class="col-me-9">
                                <asp:Label Text="" ID="lbl_ParaNotifEnviada" runat="server" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <b>Descripción</b>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <textarea class="well-sm" runat="server" style="width: 100%;" rows="3" id="tb_descripcionEnviada" disabled="disabled"></textarea>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <a class="btn btn-primary btn-warning" data-toggle="collapse" href="#collapseObservaciones" aria-expanded="false" aria-controls="collapseExample">Observación
                                </a>
                                <asp:Button Text="Recibir" ID="btn_Recibir" OnClick="btn_Recibir_Click" OnClientClick="if (!confirm('Esta por marcar como RECIBIDA la notificación, desea continuar?')) return false;" CssClass="btn btn-success" runat="server" />
                                <asp:Button Text="Volver" ID="btn_volver" OnClick="btn_volver_Click" CssClass="btn btn-default" runat="server" />
                                <asp:Button Text="Imprimir" ID="btn_Imprimir_Enviado" OnClick="btn_Imprimir_Enviado_Click" CssClass="btn btn-default" runat="server" />
                            </div>
                        </div>
                        <div class="collapse" id="collapseObservaciones">
                            <div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <br />
                                        <b>Observaciones</b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <textarea id="tb_observaciones" class="well-sm" runat="server" style="width: 100%;" rows="3" onkeyup="HabilitarBoton()"></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button Text="Posponer" ID="btn_PosponerRecepcion" Enabled="false" OnClick="btn_PosponerRecepcion_Click" CssClass="btn btn-danger" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>

        function HabilitarBoton() {
            var observacion = document.getElementById('<%=tb_observaciones.ClientID%>').value;
            if (observacion.length > 0) {
                document.getElementById('<%=btn_PosponerRecepcion.ClientID%>').disabled = false;
            }
            else {
                document.getElementById('<%=btn_PosponerRecepcion.ClientID%>').disabled = true;
            }
        }
    </script>
</asp:Content>
