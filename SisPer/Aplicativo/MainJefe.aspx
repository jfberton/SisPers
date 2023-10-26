<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="MainJefe.aspx.cs" Inherits="SisPer.Aplicativo.MainJefe" %>

<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc3" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc4" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="~/Aplicativo/Controles/ImagenAgente.ascx" TagPrefix="uc1" TagName="ImagenAgente" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc4:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc1:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
    <uc3:DatosAgente ID="DatosAgente1" runat="server" />
    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." OnPreRender="GridView1_PreRender"
                OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="false" GridLines="None" CssClass="compact stripe">
                <Columns>
                    <asp:BoundField DataField="nombre_area" HeaderText="Area" ReadOnly="true" />
                    <asp:BoundField DataField="nivel" HeaderText="Nivel" ReadOnly="true" />
                    <asp:BoundField DataField="legajo" HeaderText="Legajo" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="nombre_agente" HeaderText="Nombre y Apellido" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Bonif." ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" ToolTip="Horas por cumplir de bonificación por mayor dedicación otorgada"
                                Text='<%#Eval("horas_bonificacion") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="estado_agente" HeaderText="Estado" ReadOnly="true" />
                    <asp:BoundField DataField="dias_por_cerrar" HeaderText="Dias sin cerrar" ReadOnly="true" />
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
                                    <button type="button" class="btn btn-default btn-lg" style="width: 100%" runat="server" id="btn_imprimir_carnet" onserverclick="btn_imprimir_carnet_ServerClick" disabled="disabled">Imprimir carnet</button>
                                </div>
                                <div class="col-md-4">
                                    <button type="button" class="btn btn-default btn-lg" data-dismiss="modal" style="width: 100%">
                                        <span class="glyphicon glyphicon-remove"></span>Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Horarios vespertinos pendientes de aprobación<br />
                        <span class="small">Si se solicitó en día donde el agente marca manual, el HV deberá ser cerrado por usted una vez aprobado.</span></h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewHVPendientesAprobar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewHVPendientesAprobar_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                DataFormatString="{0:D}" />
                            <asp:BoundField DataField="Desde" HeaderText="Hora desde" ReadOnly="true" SortExpression="Desde" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hora hasta" ReadOnly="true" SortExpression="Hasta" />
                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="img_Motivo" ImageUrl="../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                            <asp:TemplateField HeaderText="Aprobar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AprobarHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/accept.png" OnClick="btn_AprobarHV_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rechazar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_RechazarHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Rechazar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_RechazarHV_Click"
                                        OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default" runat="server">
                <div class="panel-heading">
                    <h3 class="panel-title">Horarios Vespertinos aprobados por cerrar
                        <br />
                        <span class="small">Aprobados por usted en día donde el agente realizó marcaciones manuales.</span></h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewHVPorCerrar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewHVPorCerrar_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                DataFormatString="{0:D}" />
                            <asp:BoundField DataField="Desde" HeaderText="Hora desde" ReadOnly="true" SortExpression="Desde" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hora hasta" ReadOnly="true" SortExpression="Hasta" />
                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="img_Motivo" ImageUrl="../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                            <asp:TemplateField HeaderText="Terminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_TerminarHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Terminar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_TerminarHV_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Solicitudes del mes enviadas a Personal</h3>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gv_EstadosSolicitados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                        OnPageIndexChanging="gv_EstadosSolicitados_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                            <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Francos compensatorios pendientes de aprobación</h3>
                </div>
                <div class="panel-body">
                    <input type="hidden" id="MotivoRechazo" runat="server" value="" />
                    <asp:GridView ID="GridViewFrancos" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancosPendientesAprobar_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                            <asp:BoundField DataField="Dia" HeaderText="Fecha solicitud" ReadOnly="true" SortExpression="Dia"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="DiaInicial" HeaderText="Dia inicial" ReadOnly="true" SortExpression="DiaInicial"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="CantidadDias" HeaderText="Dias" ReadOnly="true" SortExpression="CantidadDias" />
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                            <asp:TemplateField HeaderText="Aprobar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AprobarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/accept.png" OnClick="btn_AprobarFranco_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rechazar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_RechazarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Rechazar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_RechazarFranco_Click"
                                        OnClientClick="ConfirmaRechazo()" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <%--Solicitudes pendientes de aprobación--%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-7">
                            <h4 class="panel-title">Gestionar medicos pendientes de aprobación</h4>
                        </div>
                        <div class="col-md-4">
                            <ul class="nav navbar-nav navbar-right">
                                <li>
                                    <div class="input-group">
                                        <input type="text" style="padding: 10px;" runat="server" id="tb_LegajoBuscado" class="form-control" placeholder="Legajo">
                                        <span class="input-group-btn">
                                            <button type="button" runat="server" onserverclick="btn_filtrarSolicitudes_Click" class="btn btn-primary">
                                                <span class="glyphicon glyphicon-search"></span>
                                            </button>
                                        </span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gv_MedicosSolicitados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                        OnPageIndexChanging="gv_Estados_medico_Solicitados_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Fechahora" HeaderText="Solicitado el" ReadOnly="true" SortExpression="Fechahora" DataFormatString="{0:g}" NullDisplayText=" - " />
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="DNI" HeaderText="DNI" ReadOnly="true" SortExpression="DNI" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                            <asp:BoundField DataField="Encuadre" HeaderText="Encuadre" ReadOnly="true" SortExpression="Encuadre" NullDisplayText=" - " />
                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Administrar_medico" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ImageUrl="~/Imagenes/user_go.png"
                                        OnClick="btn_Administrar_medico_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <br />

    <div class="modal fade" id="modal_solicitud_medico" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div runat="server" id="Div1" class="modal-content panel-primary">
                <div class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title" id="myModalLabel">
                        <asp:Label Text="" ID="lbl_modal_solicitud_Id" runat="server" Visible="false" />
                        <asp:Label Text="" ID="Label2" runat="server" />

                    </h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <table class="table-condensed">
                                <tr style="width: 100%">
                                    <td>
                                        <strong>Ingrese actuación electrónica</strong>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_pre_act_elect" runat="server" /></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_actuacion_electronica" /></td>
                                                <td>-Ae</td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <table class="table-condensed">
                                <tr style="width: 100%">
                                    <td>
                                        <strong>Ficha médica</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_ficha_medica" runat="server" />
                                    </td>
                                </tr>
                                <tr style="width: 100%">
                                    <td>
                                        <strong>Agente</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_agente" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Domicilio</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_domicilio" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Tipo</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_tipo" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="fila_familiar">
                                    <td>
                                        <strong>Familiar</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_familiar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Encuadre</strong>
                                    </td>
                                    <td>
                                        <asp:Label Text="" ID="lbl_encuadre" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <strong>Fecha desde</strong>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbl_fecha_desde" Enabled="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Button Text="Imprimir" class="btn btn-default" Style="width: 100%" runat="server" ID="btn_imprimir_solicitud_medico" OnClick="btn_imprimir_solicitud_medico_ServerClick" />
                        </div>
                    </div>
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
                    { "orderable": false, "targets": 4 },
                    { "orderable": false, "targets": 5 },
                    { "orderable": false, "targets": 6 },
                    { "orderable": false, "width": "5%", "targets": 7 }
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

        function ConfirmaRechazo() {
            var strInput = prompt("Motivo de rechazo", "");
            if (strInput != null && strInput != "") {
                var tBox = document.getElementById('<%=MotivoRechazo.ClientID%>');
                tBox.value = strInput;
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <script>
        $(document).ready(function () {
            // Enable the button `btn_imprimir_solicitud_medico` only when the field `tb_actuacion_electronica` has text.
            var btnImprimirSolicitudMedico = document.getElementById('<%= btn_imprimir_solicitud_medico.ClientID %>');
            var tbActuacionElectronica = document.getElementById('<%= tb_actuacion_electronica.ClientID %>');
            btnImprimirSolicitudMedico.disabled = tbActuacionElectronica.value === "";

            
            tbActuacionElectronica.addEventListener("input", function () {
                btnImprimirSolicitudMedico.disabled = tbActuacionElectronica.value === "";
            });

            tbFechaHasta.addEventListener("input", function () {
                // Obtenemos la fecha del label
                var fecha_desde = $("#<%=lbl_fecha_desde.ClientID %>");

            // Convertimos la fecha del label a un objeto Date
            var fecha_desde_objeto = moment(fecha_desde.text(), "DD/MM/YYYY").toDate();

        });

            // Función para manejar el cambio en la clase del div
            function handleClassChange(mutationsList, observer) {
                for (let mutation of mutationsList) {
                    if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                        const div = document.getElementById('modal_solicitud_medico');
                        if (div.classList.contains('in')) {
                            // El div se ha mostrado
                            console.log('El div se ha mostrado');
                            // Realiza las acciones que desees

                            var btnImprimirSolicitudMedico = document.getElementById('<%= btn_imprimir_solicitud_medico.ClientID %>');
                            var tbActuacionElectronica = document.getElementById('<%= tb_actuacion_electronica.ClientID %>');

                            tbActuacionElectronica.value = '';
                           
                            btnImprimirSolicitudMedico.disabled = true;

                            tbActuacionElectronica.focus();

                        } else {
                            // El div se ha ocultado, aquí puedes realizar las acciones que desees
                            console.log('El div se ha ocultado');
                            // Limpia los controles o realiza otras acciones
                            // Ejemplo: div.classList.remove('in');
                        }
                    }
                }
            }

            // Observa el div para detectar cambios en su clase
            const targetDiv = document.getElementById('modal_solicitud_medico');
            const observer = new MutationObserver(handleClassChange);
            const config = { attributes: true };
            observer.observe(targetDiv, config);

        });
    </script>
</asp:Content>
