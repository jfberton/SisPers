<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="MainPersonal.aspx.cs" Inherits="SisPer.Aplicativo.MainPersonal" %>

<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">

    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc1:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
    <br />
    <%--Francos compensatorios--%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Francos compensatorios</h4>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-6">
                            <h4><small>Francos por aprobar</small></h4>
                            <asp:GridView ID="GridViewFrancosPorAprobar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancosPorAprobar_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Dia" HeaderText="Solicitud" ReadOnly="true" SortExpression="Dia"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DiaInicial" HeaderText="Dia" ReadOnly="true" SortExpression="DiaInicial"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="CantidadDias" HeaderText="Dias" ReadOnly="true" SortExpression="CantidadDias" />
                                    <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                    <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_AprobarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AprobarFrancoPorAprobar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-6">
                            <h4><small>Francos para la firma, enviados a SubAdministración</small></h4>
                            <asp:GridView ID="GridViewFrancosAprobados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancosAprobados_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Dia" HeaderText="Fecha solicitud" ReadOnly="true" SortExpression="Dia"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DiaInicial" HeaderText="Dia" ReadOnly="true" SortExpression="DiaInicial"
                                        DataFormatString="{0:dd/MM/yyyy}" />
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
                                                OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;" />
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

    <%--Horarios vespertinos y cambios pendientes--%>
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Horarios vespertinos por terminar</h4>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewHVPendientesAprobar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewHVPendientesAprobar_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Desde" HeaderText="Hora desde" ReadOnly="true" SortExpression="Desde" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hora hasta" ReadOnly="true" SortExpression="Hasta" />
                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="img_Motivo" ImageUrl="../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                            <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AnalizarHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarHV_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Cambios pendientes de confirmación</h4>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewCambiosPendientes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewCambiosPendientes_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AnalizarCambios" runat="server" CommandArgument='<%#Eval("Usr")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarCambios_Click" />
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
                            <h4 class="panel-title">Médicos pendientes de aprobación</h4>
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
                        OnPageIndexChanging="gv_EstadosSolicitados_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Fechahora" HeaderText="Solicitado el" ReadOnly="true" SortExpression="Fechahora" DataFormatString="{0:g}" NullDisplayText=" - " />
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                            <asp:BoundField DataField="Encuadre" HeaderText="Encuadre" ReadOnly="true" SortExpression="Encuadre" NullDisplayText=" - " />
                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Administrar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ImageUrl="~/Imagenes/user_go.png"
                                        OnClick="btn_Administrar_Click" />
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
            <div runat="server" id="modal_title" class="modal-content panel-primary">
                <div class="modal-header panel-heading">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title" id="myModalLabel">
                        <asp:Label Text="" ID="lbl_modal_solicitud_Id" runat="server" Visible="false" />
                        <asp:Label Text="" ID="lbl_modal_titulo" runat="server" />

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
                                                <td>
                                                    <strong>Fecha hasta</strong>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_fecha_hasta" />
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
                        <div class="col-md-4">
                            <asp:Button Text="Aprobar" class="btn btn-default" Style="width: 100%" runat="server"
                                ID="btn_aprobar_solicitud_medico" OnClientClick="javascript:if (!confirm('¿Desea APROBAR esta solicitud?')) return false;"
                                OnClick="btn_aprobar_solicitud_medico_ServerClick" />
                        </div>
                        <div class="col-md-4">
                            <asp:Button Text="Rechazar" class="btn btn-default" Style="width: 100%" runat="server" ID="btn_rechazar_solicitud_medico"
                                OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;"
                                OnClick="btn_rechazar_solicitud_medico_ServerClick" />
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
                    <div class="row">
                        <div class="col-md-7">
                            <h4 class="panel-title">Otras solicitudes pendientes de aprobación</h4>
                        </div>
                        <div class="col-md-4">
                            <ul class="nav navbar-nav navbar-right">
                                <li>
                                    <div class="input-group">
                                        <input type="text" style="padding: 10px;" runat="server" id="tb_legajo_otras_solicitudes" class="form-control" placeholder="Legajo">
                                        <span class="input-group-btn">
                                            <button type="button" runat="server" onserverclick="btn_filtrarOtrasSolicitudes_Click" class="btn btn-primary">
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
                    <asp:GridView ID="gv_otras_solicitudes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                        OnPageIndexChanging="gv_otras_solicitudes_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Fechahora" HeaderText="Solicitado el" ReadOnly="true" SortExpression="Fechahora" DataFormatString="{0:g}" NullDisplayText=" - " />
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                            <asp:BoundField DataField="Encuadre" HeaderText="Encuadre" ReadOnly="true" SortExpression="Encuadre" NullDisplayText=" - " />
                            <asp:BoundField DataField="Lugar" HeaderText="Lugar" ReadOnly="true" SortExpression="Lugar" NullDisplayText=" - " />
                            <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />

                            <asp:TemplateField HeaderText="Aprobar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Aprobar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/accept.png" OnClick="btn_Aprobar_Click"
                                        OnClientClick="javascript:if (!confirm('¿Desea APROBAR esta solicitud?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rechazar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Rechazar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Rechazar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_Rechazar_Click"
                                        OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="contentScripts">
    <script>
        $(document).ready(function () {
            // Enable the button `btn_imprimir_solicitud_medico` only when the field `tb_actuacion_electronica` has text.
            var btnImprimirSolicitudMedico = document.getElementById('<%= btn_imprimir_solicitud_medico.ClientID %>');
            var tbActuacionElectronica = document.getElementById('<%= tb_actuacion_electronica.ClientID %>');
            btnImprimirSolicitudMedico.disabled = tbActuacionElectronica.value === "";

            // Enable the button `btn_aprobar_solicitud_medico` only when the field `tb_fecha_hasta` has text associated.
            var btnAprobarSolicitudMedico = document.getElementById('<%= btn_aprobar_solicitud_medico.ClientID %>');
            var tbFechaHasta = document.getElementById('<%= tb_fecha_hasta.ClientID %>');
            btnAprobarSolicitudMedico.disabled = tbFechaHasta.value === "";


            tbActuacionElectronica.addEventListener("input", function () {
                btnImprimirSolicitudMedico.disabled = tbActuacionElectronica.value === "";
            });

            tbFechaHasta.addEventListener("input", function () {
                // Obtenemos la fecha del label
                var fecha_desde = $("#<%=lbl_fecha_desde.ClientID %>");

                // Convertimos la fecha del label a un objeto Date
                var fecha_desde_objeto = new Date(fecha_desde.text());

                // Obtenemos la fecha del textbox
                var fecha_hasta = $("#<%= tb_fecha_hasta.ClientID %>");

                // Convertimos la fecha del textbox a un objeto Date
                var fecha_hasta_objeto = new Date(fecha_hasta.val());

                // Validamos que la fecha del textbox sea del tipo fecha
                btnAprobarSolicitudMedico.disabled = !(fecha_hasta_objeto instanceof Date && fecha_hasta_objeto >= fecha_desde_objeto);
            });

        });
    </script>

</asp:Content>
