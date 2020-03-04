<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualizarDiaAgente.ascx.cs" Inherits="SisPer.Aplicativo.Controles.VisualizarDiaAgente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Aplicativo/Controles/Ddl_TipoMovimientoHora.ascx" TagPrefix="uc1" TagName="Ddl_TipoMovimientoHora" %>

<input type="hidden" id="inputAgenteBuscado" runat="server" />
<input type="hidden" id="inputDiaBuscado" runat="server" />
<input type="hidden" id="inputResumenDiarioBuscado" runat="server" />
<input type="hidden" id="inputReadOnly" runat="server" />

<div runat="server" id="panelDia" class="panel panel-success">
    <div class="panel-heading">
        <h2 class="panel-title">
            <asp:Label Text="" ID="lbl_Dia" runat="server" />
            "<asp:Label ID="lbl_Estado" Font-Bold="true" Text="" runat="server" />" 
            <asp:Label ID="lb_cerrado_por" runat="server" />
        </h2>
    </div>
    <div class="panel-body">
        <div runat="server" id="DivMovimiento" class="alert alert-warning">
            <asp:Label Text="" ID="lbl_Movimiento" runat="server" />
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">Marcaciones</h2>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td valign="top">
                                    <asp:GridView ID="gv_Marcaciones" runat="server" ForeColor="#717171"
                                        EmptyDataText="El agente no posee marcaciones registradas."
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                        OnPageIndexChanging="gv_Marcaciones_PageIndexChanging"
                                        OnRowDataBound="gv_Marcaciones_RowDataBound"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Hora" HeaderText="Hora" ReadOnly="true" SortExpression="Hora" />
                                            <asp:CheckBoxField DataField="Manual" HeaderText="Manual" ReadOnly="true" SortExpression="Manual" />
                                            <asp:TemplateField HeaderText="Anulada" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_anulada" Enabled="false" ToolTip="Anulada" Checked='<%#Eval("Anulada")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">Entrada/Salida</h2>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td>Entrada
                                <asp:Label Text="" ID="lbl_hEntrada" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Salida
                                <asp:Label Text="" ID="lbl_hSalida" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">Movimientos agendados</h2>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td valign="top">
                                    <asp:GridView ID="GridView1" runat="server" EmptyDataText="El agente no tiene movimientos de horas en la fecha seleccionada." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                            <asp:BoundField DataField="Operador" HeaderText="Operador" ReadOnly="true" SortExpression="Operador" />
                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ReadOnly="true" SortExpression="Observaciones" />
                                           <%-- <asp:TemplateField HeaderText="Desc. Año Anterior" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("Horasanioanterior")%>' runat="server" Height="16"
                                                        Width="16" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desc. Bonific." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("Horasbonific")%>' runat="server" Height="16"
                                                        Width="16" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" ImageUrl="~/Imagenes/report_user.png" ToolTip='<%# Eval("AgendadoPor")%>' runat="server" Height="16"
                                                        Width="16" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <p />
                                    <u><b>
                                        <asp:Label Text="" runat="server" ID="lbl_totalHorasFechaSeleccionada" /></b></u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="div_horarios_vespertinos">
                                        <br />
                                        <h2 class="panel-title small">Horarios vespertinos</h2>
                                        <asp:GridView ID="gv_HV" runat="server" ForeColor="#717171"
                                            EmptyDataText="El agente no posee horarios vespertinos registrados."
                                            AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                                <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                                <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                                    DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Desde" HeaderText="Hora desde" ReadOnly="true" SortExpression="Desde" />
                                                <asp:BoundField DataField="Hasta" HeaderText="Hora hasta" ReadOnly="true" SortExpression="Hasta" />
                                                <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="img_Motivo" ImageUrl="../../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                                            runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <div runat="server" id="div_solcitudes_pendientes_dia">
                                        <br />
                                        <h2 class="panel-title">Solicitud pendiente de aprobacion para el día</h2>
                                        <asp:GridView ID="GridView_solicitudes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                            AutoGenerateColumns="False" GridLines="None"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="AgendadoPor" HeaderText="Agendado por" ReadOnly="true"
                                                    SortExpression="AgendadoPor" />
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                                <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" DataFormatString="{0:d}"
                                                    SortExpression="Dia" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="div_estados_agendados">
                                        <br />
                                        <h2 class="panel-title">Estados agendado para el día</h2>
                                        <asp:GridView ID="GridViewEstados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                            AutoGenerateColumns="False" GridLines="None"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="AgendadoPor" HeaderText="Agendado por" ReadOnly="true"
                                                    SortExpression="AgendadoPor" />
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                                <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" DataFormatString="{0:d}"
                                                    SortExpression="Dia" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <br />
                        <asp:Panel runat="server" ID="Panel_DistribucionHoras" Visible="false">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <u><b>
                                            <asp:Label Text="Distribución de horas" runat="server" /></b></u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Descontó de bonificación</td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_DescontoBonificacion" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Descontó año anterior</td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_DescontoAAnt" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Descontó año actual</td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_DescontoAAct" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Acumuló mes</td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_AcumuloMes" runat="server" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<input type="hidden" id="muestra_collapseMarcacion" runat="server" value="0" />
<input type="hidden" id="muestra_collapseMov" runat="server" value="0" />
<input type="hidden" id="muestra_collapseHoras" runat="server" value="0" />

<script>
    $('#collapseMarcacion').on('hidden.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseMarcacion.ClientID %>");
        collapse.value = 0;
    })

    $('#collapseMarcacion').on('shown.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseMarcacion.ClientID %>");
        collapse.value = 1;
    })

    $('#collapseMov').on('hidden.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseMov.ClientID %>");
        collapse.value = 0;
    })

    $('#collapseMov').on('shown.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseMov.ClientID %>");
        collapse.value = 1;
    })

    $('#collapseHoras').on('hidden.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseHoras.ClientID %>");
        collapse.value = 0;
    })

    $('#collapseHoras').on('shown.bs.collapse', function () {
        var collapse = document.getElementById("<%= muestra_collapseHoras.ClientID %>");
        collapse.value = 1;
    })



    $(function () {
        var v_collapseMarcacion = document.getElementById("<%= muestra_collapseMarcacion.ClientID %>");
        var v_collapseMov = document.getElementById("<%= muestra_collapseMov.ClientID %>");
        var v_collapseHoras = document.getElementById("<%= muestra_collapseHoras.ClientID %>");

        v_collapseMarcacion.value == 0 ? $('#collapseMarcacion').collapse('hide') : $('#collapseMarcacion').collapse('show');
        v_collapseMov.value == 0 ? $('#collapseMov').collapse('hide') : $('#collapseMov').collapse('show');
        v_collapseHoras.value == 0 ? $('#collapseHoras').collapse('hide') : $('#collapseHoras').collapse('show');
    });
</script>


