<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Formulario1214_Solicitudes.aspx.cs" Inherits="SisPer.Aplicativo.Formulario1214_Solicitudes" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" Visible="false" runat="server">
    <uc1:MenuPersonalJefe runat="server" Visible="false" ID="MenuPersonalJefe" />
    <uc1:MenuPersonalAgente runat="server" Visible="false" ID="MenuPersonalAgente" />
    <uc1:MenuJefe runat="server" Visible="false" ID="MenuJefe" />
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <%--Solicitudes pendientes--%>
    <div class="panel panel-default" runat="server" id="panel_solicitudes">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Solicitudes pendientes</h4>
                </div>
                <div class="col-md-7">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_pendientes.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body"> <%--style="height: 300px; overflow-y: scroll;"--%>
            <asp:GridView ID="gv_pendientes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="f1214_id" HeaderText="F1214 N°" ReadOnly="true" SortExpression="f1214_id" />
                    <asp:BoundField DataField="area" HeaderText="Area" ReadOnly="true" SortExpression="area" />
                    <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" SortExpression="agente" />
                    <asp:BoundField DataField="destino" HeaderText="Destino" ReadOnly="true" SortExpression="destino" />
                    <asp:BoundField DataField="desde" HeaderText="Desde" ReadOnly="true" SortExpression="desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="hasta" DataFormatString="{0:d}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ID="btn_ver_pendiente" runat="server" OnClick="btn_ver_pendiente_Click" CommandArgument='<%#Eval("agente214_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="modal fade" id="modal_ve" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Solicitud de comisión N°
                        <asp:Label Text="" ID="lbl_f1214_id" runat="server" /></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Jefe de comisión:</label>
                                    <asp:Label Text="" ID="lbl_jefe_de_comision" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Solicitud para el agente:</label>
                                    <asp:Label Text="" ID="lbl_agente" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                                <p>
                                    <label>Destino:</label>
                                    <asp:Label Text="" ID="lbl_destino" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-4">
                                <p>
                                    <label>Por:</label>
                                    <asp:Label Text="" ID="lbl_dias" runat="server" />
                                    días.
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <p>
                                    <label>Desde</label>
                                    <asp:Label Text="" ID="lbl_desde" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-6">
                                <p>
                                    <label>Hasta</label>
                                    <asp:Label Text="" ID="lbl_hasta" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Tareas:</label>
                                    <asp:Label Text="" ID="lbl_tareas" runat="server" />
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Aprobar" CssClass="btn btn-success" ID="btn_aprobar" OnClick="btn_aprobar_Click" runat="server" />
                        <asp:Button Text="Rechazar" CssClass="btn btn-danger" ID="btn_rechazar" OnClick="btn_rechazar_Click" runat="server" />

                        <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <%--Solicitudes aprobadas y rechazadas--%>
    <div class="panel panel-default" runat="server" id="panel_solicitudes_aprobadas_rechazadas">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Solicitudes aprobadas - rechazadas - canceladas</h4>
                </div>
                <div class="col-md-7">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_otras_solicitudes.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body" ><%--style="height: 300px; overflow-y: scroll;"--%>
            <asp:GridView ID="gv_otras_solicitudes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="f1214_id" HeaderText="F1214 N°" ReadOnly="true" />
                    <asp:BoundField DataField="estado_1214" HeaderText="Estado 1214" ReadOnly="true" />
                    <asp:BoundField DataField="area" HeaderText="Area" ReadOnly="true" SortExpression="area" />
                    <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" SortExpression="agente" />
                    <asp:BoundField DataField="destino" HeaderText="Destino" ReadOnly="true" SortExpression="destino" />
                    <asp:BoundField DataField="desde" HeaderText="Desde" ReadOnly="true" SortExpression="desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="estado_solicitud" HeaderText="Hasta" ReadOnly="true" SortExpression="hasta" DataFormatString="{0:d}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ID="btn_ver_otros" runat="server" OnClick="btn_ver_otros_Click" CommandArgument='<%#Eval("agente214_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="modal fade" id="ver_otra_solicitud" role="dialog" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Datos Form 3168 y solicitud seleccionada</h4>
                        </div>
                        <div class="modal-body">
                            <div class="panel panel-default" runat="server" id="panel_f1214">
                                <div class="panel-heading">
                                    <h1 class="panel-title">
                                        <asp:Label Text="" ID="lbl_encabezado_f1214" runat="server" /></h1>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <p>
                                                <label>Jefe de comisión:</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_jefe" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <p>
                                                <label>Destino:</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_destino" runat="server" />
                                            </p>
                                        </div>
                                        <div class="col-md-4">
                                            <p>
                                                <label>Por:</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_cantidad_dias" runat="server" />
                                                días.
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <p>
                                                <label>Desde</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_desde" runat="server" />
                                            </p>
                                        </div>
                                        <div class="col-md-6">
                                            <p>
                                                <label>Hasta</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_hasta" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <p>
                                                <label>Tareas:</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_tareas" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default" runat="server" id="panel_solicitud">
                                <div class="panel-heading">
                                    <h1 class="panel-title">
                                        <asp:Label Text="" ID="lbl_encabezado_solicitud" runat="server" />
                                    </h1>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <p>
                                                <label>Solicitud para el agente:</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_agente" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <p>
                                                <label>Aprobado el</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_aprobado_el" runat="server" />
                                            </p>
                                        </div>
                                        <div class="col-md-7">
                                            <p>
                                                <label>por</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_aprobado_por" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <p>
                                                <label>Rechazado el</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_rechazado_el" runat="server" />
                                            </p>
                                        </div>
                                        <div class="col-md-7">
                                            <p>
                                                <label>por</label>
                                                <asp:Label Text="" ID="lbl_otra_solicitud_rechazado_por" runat="server" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <%--Solicitudes de numeros de anticipos --%>
    <div class="panel panel-primary" id="panel_anticipos" runat="server">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Nro de Anticipos pendientes</h4>
                </div>
                <div class="col-md-7">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_anticipos.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body" ><%--style="height: 300px; overflow-y: scroll;"--%>
            <asp:GridView ID="gv_anticipos" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="f1214_id" HeaderText="F1214 N°" ReadOnly="true" SortExpression="f1214_id" />
                    <asp:BoundField DataField="area" HeaderText="Area" ReadOnly="true" SortExpression="area" />
                    <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" SortExpression="agente" />
                    <asp:BoundField DataField="nro_anticipo" HeaderText="Nro Anticipo" ReadOnly="true" SortExpression="nro_anticipo" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ID="btn_ver_otorgar_anticipo" runat="server" OnClick="btn_ver_otorgar_anticipo_Click" CommandArgument='<%#Eval("agente214_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="modal fade" id="modal_anticipo" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Datos de comisión del agente para anticipo</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-4">
                                <p>
                                    <label>Solicitud N°:</label>
                                    <asp:Label Text="" ID="lbl_ant_nro_comision" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-8">
                                <p>
                                    <label>Jefe de comisión:</label>
                                    <asp:Label Text="" ID="lbl_ant_jefe_comision" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Solicitud para el agente:</label>
                                    <asp:Label Text="" ID="lbl_ant_agente" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                                <p>
                                    <label>Destino:</label>
                                    <asp:Label Text="" ID="lbl_ant_destino" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-4">
                                <p>
                                    <label>Por:</label>
                                    <asp:Label Text="" ID="lbl_ant_dias" runat="server" />
                                    días.
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <p>
                                    <label>Desde</label>
                                    <asp:Label Text="" ID="lbl_ant_desde" runat="server" />
                                </p>
                            </div>
                            <div class="col-md-6">
                                <p>
                                    <label>Hasta</label>
                                    <asp:Label Text="" ID="lbl_ant_hasta" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Tareas:</label>
                                    <asp:Label Text="" ID="lbl_ant_tareas" runat="server" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <p>
                                    <label>Nro Anticipo:</label>
                                    <asp:TextBox runat="server" ID="tb_ant_nro_anticipo" CssClass="form-control" />
                                </p>
                            </div>
                            <div class="col-md-8" id="ant_otorgado_por_PanelRowColumn" runat="server">
                                <p>
                                    <label >Última modificación:</label>
                                    <asp:Label Text="" ID="lbl_ant_otorgado_por" runat="server" />
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Aceptar" CssClass="btn btn-success" ID="btn_ant_aceptar" OnClick="btn_ant_aceptar_Click" runat="server" />

                        <button type="button" class="btn btn-default" data-dismiss="modal">Volver</button>
                    </div>
                </div>
            </div>
        </div>

    </div>

     <%--Numeros de anticipos otorgados--%>
    <div class="panel panel-primary" id="panel_anticipos_otorgados" runat="server">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-5">
                    <h4 class="panel-title">Nro de Anticipos otorgados</h4>
                </div>
                <div class="col-md-7">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_anticipos.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body"><%--style="height: 300px; overflow-y: scroll;"--%>
            <asp:GridView ID="gv_anticipos_otorgados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="f1214_id" HeaderText="F1214 N°" ReadOnly="true" SortExpression="f1214_id" />
                    <asp:BoundField DataField="area" HeaderText="Area" ReadOnly="true" SortExpression="area" />
                    <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" SortExpression="agente" />
                    <asp:BoundField DataField="nro_anticipo" HeaderText="Nro Anticipo" ReadOnly="true" SortExpression="nro_anticipo" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ID="btn_ver_otorgar_anticipo" runat="server" OnClick="btn_ver_otorgar_anticipo_Click" CommandArgument='<%#Eval("agente214_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>

        function filtrar(phrase, _id) {
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById(_id);
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }
                table.rows[r].style.display = displayStyle;
            }
        }
    </script>
</asp:Content>
