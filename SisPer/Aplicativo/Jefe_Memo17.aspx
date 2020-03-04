<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Jefe_Memo17.aspx.cs" Inherits="SisPer.Aplicativo.Jefe_Memo17" %>

<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc1" %>
<%@ Register Src="~/Aplicativo/Controles/Card_Agente.ascx" TagName="Card_Agente" TagPrefix="uc2" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="panel-title">Listado de agentes Memo 17</h3>
                </div>
            </div>
        </div>

        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gv_memo" Width="100%" runat="server" EmptyDataText="No existen registros para mostrar."
                        AutoGenerateColumns="False" GridLines="None" OnPreRender="gv_memo_PreRender"
                        CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt" OnRowDataBound="GridView1_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="nombre_area" HeaderText="Area" ReadOnly="true" SortExpression="Area" />
                            <asp:BoundField DataField="nivel" HeaderText="Nivel" ReadOnly="true" SortExpression="Nivel" />
                            <asp:BoundField DataField="legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="nombre_agente" HeaderText="Agente" ReadOnly="true" />
                            <asp:BoundField DataField="ultima_modificacion" HeaderText="Generado el" ReadOnly="true" SortExpression="ultima_modificacion" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button Text="Ver" ID="btn_ver" CommandArgument='<%#Eval("Id") %>' OnClick="btn_ver_Click" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>

        $(document).ready(function () {
            $('#<%= gv_memo.ClientID %>').DataTable({
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
                "columnDefs": [
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": 1 },
                    { "orderable": false, "targets": 2 },
                    { "orderable": false, "targets": 3 },
                    { "orderable": false, "targets": 4 },
                    { "orderable": false, "targets": 5 }
                ],
                "order": [[0, 'asc']],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr><td colspan="10" style="background-color:LightGray">Área: ' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            });
        });

    </script>
</asp:Content>
