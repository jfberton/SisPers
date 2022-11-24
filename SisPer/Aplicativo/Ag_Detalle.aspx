<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Ag_Detalle.aspx.cs" Inherits="SisPer.Aplicativo.Ag_Detalle" %>

<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc2" %>
<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc6" %>
<%@ Register Src="~/Aplicativo/Controles/Licencia.ascx" TagPrefix="uc1" TagName="Licencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc6:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc2:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc1:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <uc4:DatosAgente ID="DatosAgente1" runat="server" />
        <p>
            <asp:Label ID="lbl_EstadoFuera" runat="server" CssClass="alert alert-danger" Visible="false" />
        </p>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Movimientos registrados
                            <asp:Label Text="mes de" ID="lbl_mes" runat="server" /></h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999"
                                    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                                    ForeColor="Black" Height="180px" Width="200px" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged"
                                    OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged">
                                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                    <NextPrevStyle VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#808080" />
                                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                    <SelectorStyle BackColor="#CCCCCC" />
                                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <WeekendDayStyle BackColor="#FFFFCC" />
                                </asp:Calendar>
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="false">
                                    <div class="panel">
                                        <div class="panel-heading" role="tab" id="headingRef">
                                            <h1 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseRef" aria-expanded="false" aria-controls="collapseRef">Referencias
                                    <span class="caret" />
                                                </a>
                                            </h1>
                                        </div>
                                        <div id="collapseRef" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingRef">
                                            <div class="panel-body">
                                                <table class="table-condensed">
                                                    <tr>
                                                        <td>Contiene horas</td>
                                                        <td>
                                                            <asp:Label Text="XX" runat="server" BackColor="DarkGreen" Font-Bold="True" ForeColor="Azure" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Contiene licencias</td>
                                                        <td>
                                                            <asp:Label ID="Label1" Text="XX" runat="server" BackColor="DarkGoldenrod" Font-Bold="True" ForeColor="#333333" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Feriados</td>
                                                        <td>
                                                            <asp:Label Text="XX" runat="server" BackColor="DarkRed" Font-Bold="True"
                                                                ForeColor="Azure" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Adeuda documentación</td>
                                                        <td>
                                                            <asp:Label Text="XX" runat="server" BackColor="OrangeRed" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <h1 class="panel-title">Datos cierre mes de
                                        <asp:Label Text="" ID="lbl_cierre_mes_cerrado" runat="server" /></h1>
                                                    </div>
                                                    <div class="panel-body">
                                                        <div class="row">
                                                            <div class="col-md-6"><b>Fecha cierre</b></div>
                                                            <div class="col-md-6">
                                                                <asp:Label Text="" ID="lbl_cierre_fecha_cierre" runat="server" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-6"><b>Horas año anterior</b></div>
                                                            <div class="col-md-6">
                                                                <asp:Label Text="" ID="lbl_cierre_horas_anio_anterior" runat="server" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-6"><b>Horas año actual</b></div>
                                                            <div class="col-md-6">
                                                                <asp:Label Text="" ID="lbl_cierre_horas_anio_actual" runat="server" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                         <asp:Button Text="Imprimir detalle mensual" ID="ImprirPlanilla" CssClass="btn btn-btn-default" OnClick="ImprirPlanilla_Click" runat="server" />
                                                        <div class="row" style="visibility:hidden">
                                                            <div class="col-md-6"><b>Tardanzas del mes</b></div>
                                                            <div class="col-md-6">
                                                                <asp:Label Text="" ID="lbl_cierre_tardanzas" runat="server" />&nbsp;&nbsp;<a data-toggle="modal" data-target="#ver_tardanzas_mes"><img src="../Imagenes/bullet_go.png" /></a>
                                                            </div>
                                                            <div class="modal fade" id="ver_tardanzas_mes" role="dialog" aria-hidden="true">
                                                                <div class="modal-dialog">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header">
                                                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                            <h4 class="modal-title">Detalle de las tardanas del mes</h4>
                                                                        </div>
                                                                        <div class="modal-body">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:GridView ID="gv_tardanzas_mes" runat="server" ForeColor="#717171"
                                                                                        EmptyDataText="Sin registros" AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                                                                        CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                                                                                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                                                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="modal-footer">
                                                                            <button type="button" class="btn btn-warning" data-dismiss="modal">Cerrar</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row" style="visibility:hidden">
                                                            <div class="col-md-6"><b>Acumulado neto mes</b></div>
                                                            <div class="col-md-6">
                                                                <asp:Label Text="" ID="lbl_cierre_acumulado_mes" runat="server" />&nbsp;&nbsp;<a data-toggle="modal" data-target="#ver_detalle_mes"><img src="../Imagenes/bullet_go.png" /></a>
                                                            </div>
                                                            <div class="modal fade" id="ver_detalle_mes" role="dialog" aria-hidden="true">
                                                                <div class="modal-dialog">
                                                                    <div class="modal-content">
                                                                        <div class="modal-header">
                                                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                            <h4 class="modal-title">Detalle de los movimientos horas menual</h4>
                                                                        </div>
                                                                        <div class="modal-body">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:GridView ID="gv_detalle_movimiento" runat="server" ForeColor="#717171"
                                                                                        EmptyDataText="Sin registros" AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                                                                        CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                                                                                            <asp:TemplateField HeaderText="Operador" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Image ID="Image1" ImageUrl="~/Imagenes/user_gray.png" ToolTip='<%# Eval("AgendadoPor")%>' runat="server" Height="16" Width="16" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                                                                            <asp:BoundField DataField="Operador" HeaderText="" ReadOnly="true" SortExpression="Operador" />
                                                                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                                                                            <asp:TemplateField HeaderText="Desc Bonif" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("Horasbonific")%>' runat="server" ToolTip="Desconto horas bonificables a cumplir" Height="16" Width="16" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                             <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ReadOnly="true"  />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="modal-footer">
                                                                            <button runat="server" id="btn_imprimir_detalle_mes" class="btn btn-default" onserverclick="btn_imprimir_detalle_mes_ServerClick">
                                                                                <span class="glyphicon glyphicon-print" aria-hidden="true"></span> Imprimir
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning" data-dismiss="modal">Cerrar</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_modificaciones">
                                                            <div class="col-md-12">
                                                                <div class="panel panel-default">
                                                                    <div class="panel-heading">
                                                                        <h1 class="panel-title">Modificaciones</h1>
                                                                    </div>
                                                                    <div class="panel-body">
                                                                        <div class="row">
                                                                            <div class="col-md-12">
                                                                                <asp:GridView ID="gv_modificaciones" runat="server" ForeColor="#717171"
                                                                                    EmptyDataText="Sin registros" AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                                                                    CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                                                                                        <asp:BoundField DataField="horas_anio_anterior" HeaderText="Año ant." ReadOnly="true" />
                                                                                        <asp:BoundField DataField="horas_anio_actual" HeaderText="Año act." ReadOnly="true" />
                                                                                        <asp:BoundField DataField="horas_mes" HeaderText="Horas mes" ReadOnly="true" />
                                                                                        <asp:TemplateField HeaderText="Agente" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:Image ID="Image1" ImageUrl="~/Imagenes/user_gray.png" ToolTip='<%# Eval("agente")%>' runat="server" Height="16" Width="16" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <h1 class="panel-title">
                                                            <asp:Label ID="lbl_Dia" Text="" runat="server" /></h1>
                                                    </div>
                                                    <div class="panel-body">
                                                        <asp:GridView ID="GridViewHorasDia" runat="server" EmptyDataText="El agente no tiene movimientos de horas en la fecha seleccionada." ForeColor="#717171"
                                                            AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewHorasDia_PageIndexChanging"
                                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Operador" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" ImageUrl="~/Imagenes/user_gray.png" ToolTip='<%# Eval("AgendadoPor")%>' runat="server" Height="16" Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                                                <asp:BoundField DataField="Operador" HeaderText="" ReadOnly="true" SortExpression="Operador" />
                                                                <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                                                <%-- <asp:TemplateField HeaderText="DAA" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" ImageUrl='<%# Eval("Horasanioanterior")%>' ToolTip="Desconto de horas año anterior" runat="server" Height="16" Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="DB" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" ImageUrl='<%# Eval("Horasbonific")%>' runat="server" ToolTip="Desconto horas bonificables a cumplir" Height="16" Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Desc" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" ImageUrl="~/Imagenes/help.png" ToolTip='<%# Eval("Descripcion")%>' runat="server" Height="16" Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="Descripcion" />--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="panel-footer">
                                                        <asp:Label Text="" runat="server" ID="lbl_totalHorasFechaSeleccionada" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Resumen de licencias</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <uc1:Licencia runat="server" ID="C_LicAnioAnterior" />
                            </div>
                            <div class="col-md-4">
                                <uc1:Licencia runat="server" ID="C_LicAnioEnCurso" />
                            </div>
                            <div class="col-md-4">
                                <uc1:Licencia runat="server" ID="C_LicEspInvierno" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Resumen de licencias por motivos de salud</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <uc1:Licencia runat="server" ID="C_LicEnfComun" />
                            </div>
                            <div class="col-md-6">
                                <uc1:Licencia runat="server" ID="C_LicEnfFamiliar" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="false">
                                    <div class="panel panel-warning">
                                        <div class="panel-heading" role="tab" id="headingOne">
                                            <h1 class="panel-title">
                                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseAcumulado" aria-expanded="false" aria-controls="collapseAcumulado">Lleva acumulados <span class="caret" />
                                                </a>
                                            </h1>
                                        </div>
                                        <div id="collapseAcumulado" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                            <table class="table-condensed">
                                                <tr>
                                                    <td>Días por enfermedad con goce de fondo estímulo</td>
                                                    <td>
                                                        <asp:Label Text="" ID="lbl_EnfermedadCGFE" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Image ImageUrl="~/Imagenes/help.png" ToolTip="Le corresponden al agente 20 días al año por enfermedad (común o familiar) con goce de fondo estímilo. [Ley 330, art.21,inc C, item 10]" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Días por enfermedad sin goce de fondo estímulo</td>
                                                    <td>
                                                        <asp:Label Text="" ID="lbl_EnfermedadSGFE" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </div>
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
