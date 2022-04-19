<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Jefe_Ag_AutorizaRemoto.aspx.cs" Inherits="SisPer.Aplicativo.Jefe_Ag_AutorizaRemoto" %>

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

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Autorizar dias remoto agente</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <label>Agente</label>
                </div>
                <div class="col-md-7">
                    <asp:DropDownList runat="server" ID="ddl_agente" AutoPostBack="true"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddl_agente_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-4">
                        <asp:Calendar runat="server" ID="Calendar1" OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged"
                            BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana"
                            Font-Size="9pt" ForeColor="Black" Height="307px" NextPrevFormat="FullMonth" Width="382px">
                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#999999" />
                            <SelectedDayStyle BackColor="#cdcfd1" ForeColor="White" />
                            <TitleStyle BackColor="White" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                            <TodayDayStyle BackColor="#CCCCCC" />
                        </asp:Calendar>
                    </div>
                    <div class="col-md-7">
                        <asp:GridView ID="gv_autorizaciones" runat="server" ForeColor="#717171"
                            AutoGenerateColumns="False" OnPreRender="gv_autorizaciones_PreRender" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                                <asp:TemplateField HeaderText="Quitar día">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Imagenes/cancel.png" ID="btn_eliminar_autorizacion_dia" runat="server" OnClick="btn_eliminar_autorizacion_dia_Click" CommandArgument='<%#Eval("fecha") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer"></div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gv_autorizaciones.ClientID %>').DataTable({
                "paging": true,
                "language": {
                    "search": "Buscar:",
                    "lengthMenu": "Mostrar _MENU_ agentes por página",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)",
                    "paginate": {
                        "first": "Primero",
                        "last": "Ultimo",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                },
                "order": [[0, 'asc']]
            });
        });
    </script>
</asp:Content >
