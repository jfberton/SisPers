<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Ag_Tardanzas.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Ag_Tardanzas" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc2" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente1" />
    <uc2:MenuPersonalJefe runat="server" ID="MenuPersonalJefe1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>[EN PRINCIPIO ESTA PAGINA NO SE USA MAS SE USA DESDE PROCESAR DIA EN HUELLAS.]<br />Listado de tardanzas</h2>
    <table>
        <tr>
            <td valign="top">
                <br />
                <asp:Calendar runat="server" BackColor="White" BorderColor="#999999" ID="Calendar1"
                    CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar1_SelectionChanged">
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
            </td>
            <td>
                <p />
            </td>
            <td>
                <asp:Label Text="" ID="lbl_FechaSeleccionada" runat="server" />
                <br />
                <table>
                    <tr>
                        <td>
                            <fieldset>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rb_CasaCentral" Text="Casa central" GroupName="locacion" runat="server" AutoPostBack="True" OnCheckedChanged="rb_CheckedChanged" /></td>
                                        <td>
                                            <asp:RadioButton ID="rb_Interior" Text="Interior" GroupName="locacion" runat="server" AutoPostBack="True" OnCheckedChanged="rb_CheckedChanged" /></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rb_Marco" Text="Marco" GroupName="huella" runat="server" AutoPostBack="True" OnCheckedChanged="rb_CheckedChanged" /></td>
                                        <td>
                                            <asp:RadioButton ID="rb_NoMarco" Text="No marco" GroupName="huella" runat="server" AutoPostBack="True" OnCheckedChanged="rb_CheckedChanged" /></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gv_Huellas" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                    AutoGenerateColumns="False" GridLines="None" PageSize="20" AllowPaging="true" OnPageIndexChanging="gv_Huellas_PageIndexChanging"
                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt">
                    <Columns>
                        <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                        <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                        <asp:BoundField DataField="HoraEntrada" HeaderText="Hora Entrada" ReadOnly="true" SortExpression="HoraEntrada" />
                        <asp:CheckBoxField DataField="MarcaManual" ItemStyle-HorizontalAlign="Center" HeaderText="Manual" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Ir">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ToolTip="Agendar tardanza" ID="btn_Ir" OnClick="btn_Ir_Click" runat="server" CommandArgument='<%#Eval("TodosLosCampos")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
