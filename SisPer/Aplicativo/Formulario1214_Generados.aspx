<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Formulario1214_Generados.aspx.cs" Inherits="SisPer.Aplicativo.Formulario1214_Generados" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default" runat="server" id="panel_enviados">
        <div class="panel-heading">
            <h2 class="panel-title">Formularios 3168 Enviados a Subadministración por aprobar</h2>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_Form1214_enviados" runat="server" EmptyDataText="No existen registros para mostrar."
                OnPreRender="gv_Form1214_enviados_PreRender" AutoGenerateColumns="false" GridLines="None" CssClass="compact stripe">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Numero" ReadOnly="true" SortExpression="Numero" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                    <asp:BoundField DataField="Generadopor" HeaderText="Confeccionó" ReadOnly="true" SortExpression="Generadopor" />
                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Jefe" HeaderText="Jefe de comisión" ReadOnly="true" SortExpression="Jefe" />
                    <asp:TemplateField HeaderText="Tareas">
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Imagenes/report_user.png" ToolTip='<%#Eval("Tareas") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reimprimir" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_reimprimir" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Reimprimir" ImageUrl="~/Imagenes/page_white_add.png" OnClick="btn_reimprimir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_ver" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Ver" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <br />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Formularios 3168 generados por usted</h2>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_form1214" runat="server" EmptyDataText="No existen registros para mostrar." AutoGenerateColumns="false"
                GridLines="None" CssClass="compact stripe" OnPreRender="gv_form1214_PreRender" OnRowDataBound="gv_form1214_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Numero" ReadOnly="true" SortExpression="Numero" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                    <asp:BoundField DataField="Generadopor" HeaderText="Confeccionó" ReadOnly="true" SortExpression="Generadopor" />
                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Jefe" HeaderText="Jefe de comisión" ReadOnly="true" SortExpression="Jefe" />
                    <asp:TemplateField HeaderText="Tareas">
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Imagenes/report_user.png" ToolTip='<%#Eval("Tareas") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reimprimir" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_reimprimir" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Reimprimir" ImageUrl="~/Imagenes/page_white_add.png" OnClick="btn_reimprimir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_ver" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Ver" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />

    

    <div class="panel panel-default" runat="server" id="panel_aprobados">
        <div class="panel-heading">
            <h2 class="panel-title">Formularios 3168 Aprobados en SubAdministración</h2>
        </div>
        <div class="panel-body">
            <asp:GridView ID="gv_aprobados" runat="server" EmptyDataText="No existen registros para mostrar."
                OnPreRender="gv_aprobados_PreRender" AutoGenerateColumns="false" GridLines="None" CssClass="compact stripe">
                <Columns>
                    <asp:BoundField DataField="Form" HeaderText="Nro Form" ReadOnly="true" SortExpression="Form" />
                    <asp:BoundField DataField="Disp" HeaderText="Nro Disp" ReadOnly="true" SortExpression="Disp" />
                    <asp:BoundField DataField="Generadopor" HeaderText="Confeccionó" ReadOnly="true" SortExpression="Generadopor" />
                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Jefe" HeaderText="Jefe de comisión" ReadOnly="true" SortExpression="Jefe" />
                    <asp:TemplateField HeaderText="Tareas">
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Imagenes/report_user.png" ToolTip='<%#Eval("Tareas") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reimprimir" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_reimprimir" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Reimprimir" ImageUrl="~/Imagenes/page_white_add.png" OnClick="btn_reimprimir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_ver" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Ver" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gv_form1214.ClientID %>').DataTable({
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
                    { "orderable": false, "targets": 7 },
                    { "orderable": false, "targets": 8 },
                    { "orderable": false, "targets": 9 }
                ],
                "order": [[0, 'asc']]
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gv_aprobados.ClientID %>').DataTable({
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
                    { "orderable": false, "targets": 7 },
                    { "orderable": false, "targets": 8 },
                    { "orderable": false, "targets": 9 }
                ],
                "order": [[0, 'asc']]
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gv_Form1214_enviados.ClientID %>').DataTable({
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
                    { "orderable": false, "targets": 7 },
                    { "orderable": false, "targets": 8 },
                    { "orderable": false, "targets": 9 }
                ],
                "order": [[0, 'asc']]
            });
        });
    </script>

    


</asp:Content>
