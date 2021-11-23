<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="MainPandemia.aspx.cs" Inherits="SisPer.Aplicativo.MainPandemia" %>


<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc3" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>
<%@ Register Src="~/Aplicativo/Controles/AdministrarDiaAgente.ascx" TagPrefix="uc1" TagName="AdministrarDiaAgente" %>
<%@ Register Src="~/Aplicativo/Controles/VisualizarDiaAgente.ascx" TagPrefix="uc1" TagName="VisualizarDiaAgente" %>

<script runat="server" type="text/c#">
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc3:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc2:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
        <uc1:DatosAgente ID="DatosAgente1" runat="server" />
        <h1>Atención!</h1>
        <p>Ante la situación de público conocimiento respecto al coronavirus (COVID-19), y de acuerdo con los lineamientos de asistencia, se torna inviable la carga y control de movimientos diarios generados fuera del sistema, por lo que el mismo se verá reducido en sus funcionalidades originales. </p>
    </div>
    <div>
        <asp:Button Text="Continuar ->" runat="server" CssClass="btn btn-primary btn-lg" ID="btn_continuar_main_agente" OnClick="btn_continuar_main_agente_Click" />

        <div class="panel panel-default" runat="server" id="div_huellas" visible="false">
            <div class="panel-heading">
                <h3 class="panel-title">Marcaciones entrada - salida</h3>
            </div>
            <div class="panel-body">
                <table class="table-condensed">
                    <tr>
                        <td>
                            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" OnDayRender="Calendar1_DayRender" BorderColor="#999999" CellPadding="4"
                                OnSelectionChanged="Calendar1_SelectionChanged" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                Height="180px" Width="200px">
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt"></DayHeaderStyle>
                                <NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
                                <OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White"></SelectedDayStyle>
                                <SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
                                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True"></TitleStyle>
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black"></TodayDayStyle>
                                <WeekendDayStyle BackColor="#FFFFCC"></WeekendDayStyle>
                            </asp:Calendar>
                        </td>
                        <td>
                            <asp:GridView ID="gv_Huellas" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" PageSize="20" AllowPaging="true" OnPageIndexChanging="gv_Huellas_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ReadOnly="true" SortExpression="Fecha" />
                                    <asp:BoundField DataField="Hora" HeaderText="Hora" ReadOnly="true" SortExpression="Hora" />
                                    <asp:CheckBoxField DataField="MarcaManual" ItemStyle-HorizontalAlign="Center" HeaderText="Manual" ReadOnly="true" />
                                    <asp:CheckBoxField DataField="EsDefinitivo" ItemStyle-HorizontalAlign="Center" HeaderText="Definitivo" ReadOnly="true" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="div_ES" visible="false">
                    <table class="table-condensed">
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary5" runat="server" DisplayMode="BulletList"
                                    ValidationGroup="MarcacionesES" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h3>Modificar E/S</h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Estas marcaciones serán impactadas una vez que personal cierre el día.</td>
                        </tr>
                        <tr>
                            <td>Entrada</td>
                            <td>
                                <div id="datetimepicker5" class="input-group date">
                                    <input id="h_entrada" runat="server" class="form-control" type="text" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-time"></span>
                                    </span>
                                </div>
                                <asp:CustomValidator ID="cv_puedemodificar" runat="server"
                                    ErrorMessage="No se puede modificar debido a que el jefe ya ha enviado las marcaciones del día a personal" OnServerValidate="cv_puedemodificar_ServerValidate"
                                    Text="&lt;img src='../Imagenes/exclamation.gif' title='No se puede modificar debido a que el jefe ya ha enviado las marcaciones del día a personal' /&gt;"
                                    ValidationGroup="MarcacionesES"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ControlToValidate="h_entrada" Text="<img src='../Imagenes/exclamation.gif' title='El campo es obligatorio' />"
                                    ID="RequiredFieldValidator" runat="server" ValidationGroup="MarcacionesES" ErrorMessage="El campo es obligatorio"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="MarcacionesES"
                                    Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                    ControlToValidate="h_entrada" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                    ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator></td>
                        </tr>
                        <tr>
                            <td>Salida</td>
                            <td>
                                <div id="datetimepicker6" class="input-group date">
                                    <input id="h_salida" runat="server" class="form-control" type="text" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-time"></span>
                                    </span>
                                </div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationGroup="MarcacionesES"
                                    Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                    ControlToValidate="h_salida" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                    ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Button Text="Guardar" class="btn btn-primary" runat="server" ID="btn_GuardarMarcacionES" OnClick="btn_GuardarMarcacionES_Click" />
                </div>
            </div>
        </div>


    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
