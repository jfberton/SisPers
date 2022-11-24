<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdministrarDiaAgente.ascx.cs" Inherits="SisPer.Aplicativo.Controles.AdministrarDiaAgente" %>
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
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 class="panel-title">Entrada/Salida - Marcaciones</h2>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td>Entrada
                                <asp:Label Text="" ID="lbl_hEntrada" runat="server" /></td>
                                <td>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_HoraEntrada" OnSelectedIndexChanged="ddl_HoraEntrada_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList></td>
                                &nbsp;
                                <td>Salida
                                <asp:Label Text="" ID="lbl_hSalida" runat="server" /></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_HoraSalida" CssClass="form-control" OnSelectedIndexChanged="ddl_HoraSalida_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
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
                                            <asp:TemplateField HeaderText="Anular" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_anulada" Enabled="false" ToolTip="Anulada" Checked='<%#Eval("Anulada")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Anul./Act." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_AnularActivar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                        ImageUrl='<%#Eval("UrlImagen")%>' ToolTip='<%#Eval("ToolTip")%>' OnClick="btn_AnularActivar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:Panel runat="server" ID="fs_AgrenarMarcacion">
                                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target=".bd-example-modal-sm-marcacion">Agregar marcación</button>

                                        <div class="modal fade bd-example-modal-sm-marcacion" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-sm">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h5 class="modal-title" id="exampleModalLabel">Agregar marcacion</h5>
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                            <span aria-hidden="true">&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="tb_Horas" runat="server" Width="100" CssClass="form-control"></asp:TextBox>
                                                                            <asp:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder=""
                                                                                CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                ClearMaskOnLostFocus="false" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Mask="99:99"
                                                                                Enabled="True" TargetControlID="tb_Horas" ID="tb_Horas_MaskedEditExtender"></asp:MaskedEditExtender>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Horas"
                                                                                ValidationGroup="Marcacion" Text="<img src='../../Imagenes/exclamation.gif' title='Debe ingresar la cantidad de horas a registrar' />"
                                                                                runat="server" ErrorMessage="Debe ingresar la cantidad de horas a registrar"></asp:RequiredFieldValidator>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="Marcacion"
                                                                                Text="<img src='../../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                                                ControlToValidate="tb_Horas" ValidationExpression="[0-9][0-9]:[0-5][0-9]"
                                                                                ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                                                        <asp:Button Text="Agregar" CssClass="btn btn-primary" runat="server" ID="btn_AgregarMarcacion" OnClick="btn_AgregarMarcacion_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <asp:Button id="btn_cerrar" Text="Cerrar día" CssClass="btn btn-success" runat="server" OnClick="btn_cerrar_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div runat="server" class="panel panel-default" id="panel_estado_dia">
                    <div class="panel-heading">
                        <h2 class="panel-title">Estado asociado al día</h2>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td>
                                    <div runat="server" id="div_solcitudes_pendientes_dia">
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
                                    <asp:Panel runat="server" ID="panel_movimiento">
                                        <h2 class="panel-title">Agregar movimiento
                                        </h2>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_Estados" CssClass="form-control" OnSelectedIndexChanged="ddl_Estados_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:Label Text="Año" ID="lbl_anio" runat="server" Visible="false" /></td>
                                <td>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_anio_estado_licencia" Visible="false">
                                    </asp:DropDownList></td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_GuardarComoCorrecto" CssClass="btn btn-default" Text="Guardar como correcto" runat="server" OnClick="btn_GuardarComoCorrecto_Click" />
                                </td>
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
                                    <div runat="server" class="" id="div_considera_prolongacion_de_jornada">
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <%--<div class="input-group">
                                                    <span class="input-group-addon">
                                                        <asp:CheckBox ID="chk_considera_prolongacion" CausesValidation="false" OnCheckedChanged="chk_considera_prolongacion_CheckedChanged" Checked="true" AutoPostBack="true" runat="server" />
                                                    </span>
                                                    <input type="text" readonly="readonly" class="form-control" value="Considerar prolongación de jornada" aria-label="">
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <p />
                                    <asp:GridView ID="GridView1" runat="server" EmptyDataText="El agente no tiene movimientos de horas en la fecha seleccionada." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Nuevo mov." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("AgregarAlCerrar")%>' runat="server" Height="16"
                                                        Width="16" ToolTip="Este movimiento se va a agregar al cerrar el día." />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                            <asp:BoundField DataField="Operador" HeaderText="Operador" ReadOnly="true" SortExpression="Operador" />
                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ReadOnly="true" SortExpression="Observaciones" />
                                            <%--<asp:TemplateField HeaderText="Desc. Año Anterior" ItemStyle-HorizontalAlign="Center">
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
                                    <asp:Panel runat="server" ID="Panel_AgendarHoras">
                                        <div class="panel-group" id="accordionHoras" role="tablist" aria-multiselectable="true">
                                            <div class="panel panel-default">
                                                <div class="panel-heading" role="tab" id="headingThree">
                                                    <h4 class="panel-title">
                                                        <a data-toggle="collapse" data-parent="#accordionHoras" href="#collapseHoras" aria-expanded="true" aria-controls="collapseHoras">Agendar horas en más o en menos
                                                            <span class="caret" />
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapseHoras" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                                    <div class="panel-body">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row">
                                                                    <div class="col-md-3">Tipo Movimiento</div>
                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control" />
                                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Debe seleccionar un tipo de movimiento"
                                                                            ValidationGroup="MovimientoHora" Text="<img src='../../Imagenes/exclamation.gif' title='Debe seleccionar un tipo de movimiento' />"
                                                                            OnServerValidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        Horas
                                                                <asp:Label Text="" ID="lbl_SumaResta" runat="server" />
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <asp:TextBox ID="tb_AgendarHoras" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:MaskedEditExtender runat="server" ClearMaskOnLostFocus="false" Mask="99:99"
                                                                            Enabled="True" TargetControlID="tb_AgendarHoras" ID="MaskedEditExtender1"></asp:MaskedEditExtender>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_AgendarHoras"
                                                                            ValidationGroup="MovimientoHora" Text="<img src='../../Imagenes/exclamation.gif' title='Debe ingresar la cantidad de horas a registrar' />"
                                                                            runat="server" ErrorMessage="Debe ingresar la cantidad de horas a registrar"></asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="MovimientoHora"
                                                                            Text="<img src='../../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                                            ControlToValidate="tb_AgendarHoras" ValidationExpression="[0-9][0-9]:[0-5][0-9]"
                                                                            ErrorMessage="La hora ingresada no es correcta xxx:xx"></asp:RegularExpressionValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-3">Observación</div>
                                                                    <div class="col-md-9">
                                                                        <asp:TextBox ID="tb_descripcion" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <asp:Button ID="btn_Guardar" runat="server" CssClass="btn btn-default" Text="Agendar" OnClick="btn_Guardar_Click" />
                                                                        <asp:Button ID="btn_Cancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btn_Cancelar_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
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
                                                <%--<asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_Eliminar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                            ImageUrl="~/Imagenes/delete.png" OnClick="btn_EliminarEstado_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>--%>
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


