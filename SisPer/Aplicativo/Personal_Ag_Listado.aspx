<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Ag_Listado.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Ag_Listado" %>

<%@ Register Src="Controles/Ddl_Areas.ascx" TagName="Ddl_Areas" TagPrefix="uc1" %>

<%@ Register Src="~/Aplicativo/Controles/ImagenAgente.ascx" TagPrefix="uc1" TagName="ImagenAgente" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc4" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="~/Aplicativo/Controles/OtorgarBonificacion.ascx" TagPrefix="uc1" TagName="OtorgarBonificacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc4:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." OnPreRender="GridView1_PreRender"
        OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="false" GridLines="None" CssClass="compact stripe">
        <Columns>
            <asp:BoundField DataField="nombre_area" HeaderText="Area" ReadOnly="true" />
            <asp:BoundField DataField="nivel" HeaderText="Nivel" ReadOnly="true" />
            <asp:BoundField DataField="legajo" HeaderText="Legajo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="nombre_agente" HeaderText="Nombre y Apellido" ReadOnly="true" />
            <asp:TemplateField HeaderText="Admin." ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:ImageButton ID="btn_Administrar" runat="server" CommandArgument='<%#Eval("id_agente")%>'
                        ToolTip='<%#String.Concat("Administrar ", Eval("nombre_agente"))%>' ImageUrl="~/Imagenes/user_go.png"
                        OnClick="btn_Administrar_Click" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>


    <div class="modal fade" id="modal_agente" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div runat="server" id="modal_title" class="modal-content panel-primary">
                <div class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title" id="myModalLabel">
                        <asp:Label Text="" ID="lbl_modal_agente_Id" runat="server" Visible="false" />
                        <asp:Label Text="" ID="lbl_modal_titulo" runat="server" />

                    </h3>
                    <h4>
                        <strong>Estado actual: 
                        </strong>
                        <asp:Label Text="" ID="lbl_estado" runat="server" />
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <table class="table-condensed">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <table class="table-condensed">
                                            <tr style="width: 100%">
                                                <td>
                                                    <strong>Legajo</strong>
                                                </td>
                                                <td>
                                                    <asp:Label Text="" ID="lbl_Legajo" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>DNI</strong>
                                                </td>
                                                <td>
                                                    <asp:Label Text="" ID="lbl_DNI" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Fecha ingreso</strong>
                                                </td>
                                                <td>
                                                    <asp:Label Text="" ID="lbl_FechIngreso" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Fecha nacimiento</strong>
                                                </td>
                                                <td>
                                                    <asp:Label Text="" ID="lbl_FechNac" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>E-Mail
                                                    </strong>
                                                </td>
                                                <td>
                                                    <asp:Label Text="" ID="lbl_Email" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: right">
                                        <uc1:ImagenAgente runat="server" ID="ImagenAgente1" Width="150" Height="150" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_detalle" onserverclick="btn_detalle_ServerClick">Detalle</button>
                        </div>
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_pantalla_agente" onserverclick="btn_pantalla_agente_ServerClick">Pantalla agente</button>
                        </div>
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_pantalla_jefe" onserverclick="btn_pantalla_jefe_ServerClick">Pantalla jefe</button>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_solicitar" onserverclick="btn_solicitar_ServerClick">Solicitar</button>
                        </div>
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_imprimir_carnet" onserverclick="btn_imprimir_carnet_ServerClick">Imprimir carnet</button>
                        </div>
                        <div class="col-md-4">
                            <button type="button" class="btn btn-default btn-lg" style="width: 100%" data-toggle="modal" href="#otorgar_bonificacion">Otorgar bonif.</button>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <button type="button" class="btn btn-warning btn-lg" style="width: 100%" runat="server" id="btn_editar" onserverclick="btn_editar_ServerClick">Editar</button>
                        </div>
                        <div class="col-md-4">
                            <button type="button" class="btn btn-danger btn-lg" style="width: 100%" runat="server" id="btn_eliminar" onserverclick="btn_eliminar_ServerClick" onclick="javascript:if (!confirm('¿Realmente desea eliminar este agente?')) return false;">Eliminar</button>
                        </div>
                        <div class="col-md-4">
                             <button type="button" class="btn btn-default btn-lg" data-dismiss="modal" style="width: 100%" >
                                 <span class="glyphicon glyphicon-remove"></span> Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="otorgar_bonificacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-sm " role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Otorgar Bonificación</h4>
                </div>
                <div class="modal-body">
                    <table class="table-condensed">
                        <tr>
                            <td>Horas por bonificación</td>
                            <td>
                                <asp:TextBox runat="server" ID="tb_horasABonificar" Width="50px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" Text="Cancelar" CssClass="btn btn-default" data-dismiss="modal" runat="server" />
                    <asp:Button Text="Aceptar" ID="btn_AceptarBonificacion" CssClass="btn btn-default" OnClick="btn_AceptarBonificacion_Click" runat="server" />
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="contentScripts">
    <script>

        $(document).ready(function () {
            $('#<%= GridView1.ClientID %>').DataTable({
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
                    { "orderable": false, "targets": 4 }
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
