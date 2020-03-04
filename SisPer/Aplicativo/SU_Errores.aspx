<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SU_Errores.aspx.cs" Inherits="SisPer.Aplicativo.SU_Errores" %>

<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuSU runat="server" id="MenuSU" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Errores log</h2>
    <table>
        <tr>
            <td>
                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC" OnDayRender="Calendar1_DayRender" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px" OnSelectionChanged="Calendar1_SelectionChanged">
                    <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                    <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                    <WeekendDayStyle BackColor="#CCCCFF" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Button Text="Eliminar contenido" runat="server" id="btn_VaciarLog" OnClick="btn_VaciarLog_Click"/>
                <asp:Button Text="Eliminar resumenes diarios duplicados" runat="server" id="btn_eliminarRDDuplicados" OnClick="btn_eliminarRDDuplicados_Click"/>
                <asp:Button Text="Unificar legajos" ID="btn_UnificarLegajo" OnClick="btn_UnificarLegajo_Click" runat="server" />
            </td>
        </tr>
    </table>

    <asp:ListView runat="server" ID="lv_Errores">
        <ItemTemplate>
            <table style="border: thin groove #C0C0C0">
                <tr>
                    <td><b>Fecha</b></td>
                    <td>
                        <asp:Label ID="lbl_Fecha" runat="server" Text='<%#Eval("nDATE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Agente</b></td>
                    <td>
                        <asp:Label ID="lbl_Agente" runat="server" Text='<%#Eval("nAGENTE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Mensaje</b></td>
                    <td>
                        <asp:Label ID="lbl_Mensaje" runat="server" Text='<%#Eval("nMESSAGE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Origen</b></td>
                    <td>
                        <asp:Label ID="lbl_Origen" runat="server" Text='<%#Eval("nSOURCE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Instancia</b></td>
                    <td>
                        <asp:Label ID="lbl_Instancia" runat="server" Text='<%#Eval("nINSTANCE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Data</b></td>
                    <td>
                        <asp:Label ID="lbl_Data" runat="server" Text='<%#Eval("nDATA") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>URL</b></td>
                    <td>
                        <asp:Label ID="lbl_URL" runat="server" Text='<%#Eval("nURL") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>TARGETSITE</b></td>
                    <td>
                        <asp:Label ID="lbl_TARGETSITE" runat="server" Text='<%#Eval("nTARGETSITE") %>'></asp:Label></td>
                </tr>
                <tr>
                    <td><b>STACKTRACE</b></td>
                    <td>
                        <asp:Label ID="lbl_STACKTRACE" runat="server" Text='<%#Eval("nSTACKTRACE") %>'></asp:Label></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
