<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Jefe_Ag_EntradaSalida.aspx.cs" Inherits="SisPer.Aplicativo.Jefe_Ag_EntradaSalida" %>

<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>



<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe1" />
    <uc1:MenuJefe runat="server" ID="MenuJefe1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />

    <h2>Agendar horario de ingreso y egreso personal</h2>
    <p />
    <hr />
    <h3>Seleccione area para su control</h3>
    <p />
    <asp:DropDownList runat="server" ID="ddl_areas_a_cargo">
    </asp:DropDownList>
    <asp:Button Text="Ver" runat="server" ID="btn_ver_marcaciones" OnClick="btn_ver_marcaciones_Click" />
    <hr />

    <div class="panel panel-info" runat="server" id="div_edificioCentral">
        <div class="panel-heading">
            <h3 class="panel-title">AVISO</h3>
        </div>
        <div class="panel-body">
            Esta funcionalidad fue diseñada para visualizar areas que tienen permitido el ingreso de marcaciones manuales.-
        </div>
    </div>


    <div runat="server" id="div_FueraDelEdificio">
        <table style="margin: 15px;">
            <tr>
                <td style="vertical-align: top;">
                    <asp:Calendar runat="server" ID="Calendario" BackColor="White" BorderColor="#999999" Font-Names="Verdana"
                        Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" CellPadding="4"
                        DayNameFormat="Shortest" OnSelectionChanged="Calendario_SelectionChanged"
                        OnDayRender="Calendario_DayRender" OnVisibleMonthChanged="Calendario_VisibleMonthChanged">
                        <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <TitleStyle BackColor="#999999" Font-Bold="True" BorderColor="Black" />
                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <WeekendDayStyle BackColor="#FFFFCC" />
                    </asp:Calendar>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btn_Imprimir" Text="Imprimir Planilla Mensual" CausesValidation="false" ToolTip="En la impresión saldrán únicamente las fechas que se encuentren aprobadas por personal." OnClick="btn_Imprimir_Click" />
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td style="vertical-align: top;">
                    <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" OnPageIndexChanging="gridView_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre y apellido" ReadOnly="true" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Entrada" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--guardo el id del agente en un label que lo dejo escondido para despues poder recuperarlo--%>
                                    <asp:Label runat="server" Text='<%#Eval("Id") %>' ID="lbl_IdAgente" Visible="false" />
                                    <asp:TextBox runat="server" ID="h_entrada" Width="50" Text='<%#Eval("Hentrada") %>' Enabled='<%#Eval("Enabled") %>' />
                                    <asp:MaskedEditExtender runat="server" Mask="99:99" MaskType="Time" TargetControlID="h_entrada" ID="h_entrada_MaskedEditExtender"></asp:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="h_entrada" Text="<img src='../Imagenes/exclamation.gif' title='El campo es obligatorio' />"
                                        ID="RequiredFieldValidator" runat="server" ErrorMessage="El campo es obligatorio"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                        ControlToValidate="h_entrada" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                        ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Salida" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="h_salida" Width="50" Text='<%#Eval("Hsalida") %>' Enabled='<%#Eval("Enabled") %>' />
                                    <asp:MaskedEditExtender runat="server" InputDirection="RightToLeft"
                                        Mask="99:99" MaskType="Time" TargetControlID="h_salida" ID="h_salida_MaskedEditExtender"></asp:MaskedEditExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="h_salida" Text="<img src='../Imagenes/exclamation.gif' title='El campo es obligatorio' />"
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                        ControlToValidate="h_salida" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                        ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ImageUrl='<%#Eval("DireccionImagen") %>' ToolTip='<%#Eval("Tooltip") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btn_Guardar" Text="Enviar" OnClick="btn_Guardar_Click" />
                </td>
            </tr>
        </table>
    </div>


    <div class="panel panel-danger" id="div_Atencion" runat="server" visible="false">
        <div class="panel-heading">
            <h3 class="panel-title">ATENCION!</h3>
        </div>
        <div class="panel-body">
            <p>Esta por enviar un resumen diario con agentes que no poseen marcaciones o poseen marcaciones incompletas.</p>
            <asp:GridView ID="gv_Error" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label Text=" - " runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nombre" HeaderText="Agentes con marcaciones incompletas" ReadOnly="true" />
                </Columns>
            </asp:GridView>
            <p>Seguro que decea continuar?</p>
            <asp:Button Text="Continuar y enviar" CssClass="btn btn-primary" runat="server" ID="btn_GuardarDeTodasFormas" OnClick="btn_GuardarDeTodasFormas_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        <asp:Button Text="Cancelar" runat="server" CssClass="btn btn-danger" ID="btn_CancelarGuardado" OnClick="btn_CancelarGuardado_Click" />
        </div>
    </div>

</asp:Content>
