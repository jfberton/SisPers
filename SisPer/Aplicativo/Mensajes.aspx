<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Mensajes.aspx.cs" Inherits="SisPer.Aplicativo.Mensajes" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Controles/ItemMensaje.ascx" TagPrefix="uc1" TagName="ItemMensaje" %>
<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h4>Nuevo mensaje</h4>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-2">
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
                        Seleccionar destinatarios
                    </button>
                </div>
                <div class="col-md-10">
                    <div class="well" runat="server" id="div_destinatarios">
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <span class="">Asunto</span>
                </div>
                <div class="col-md-10">
                    <%--<asp:TextBox runat="server" ID="tb_asunto" CssClass="form-control" />--%>
                    <asp:DropDownList runat="server" ID="ddl_asunto" CssClass="form-control">
                        <asp:ListItem Text="Seleccione el motivo del mensaje:" />
                        <asp:ListItem Text="Solicitud de licencias" />
                        <asp:ListItem Text="Solicitud de permisos" />
                        <asp:ListItem Text="Solicitud de actualización de los estados/datos del sistema" />
                        <asp:ListItem Text="Otras solicitudes" />
                        <asp:ListItem Text="Comunicación al Dpto. Personal" />
                        <asp:ListItem Text="Comunicación al jefe inmediato y al Dpto. Personal" />
                        <asp:ListItem Text="Otras comunicaciones" />
                        <asp:ListItem Text="Consulta de horas" />
                        <asp:ListItem Text="Consulta de licencias ordinarias" />
                        <asp:ListItem Text="Consulta de licencias médicas" />
                        <asp:ListItem Text="Consulta sobre asignaciones familiares" />
                        <asp:ListItem Text="Otras consultas" />
                    </asp:DropDownList>

                    <br />
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <CKEditor:CKEditorControl ID="CuerpoMensaje" BasePath="~/ckeditor" runat="server">
                    </CKEditor:CKEditorControl>
                </div>
            </div>

            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <%--<button type="button" classs="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                            <h4 class="modal-title" id="myModalLabel">Seleccione destinatarios</h4>
                            <asp:Button Text="Todos" ToolTip="Envia a todos los agentes" runat="server" ID="btn_Todos" OnClick="btn_Todos_Click" CssClass="btn btn-warning" />
                        </div>
                        <div class="modal-body">
                            <div role="tabpanel">

                                <!-- Nav tabs -->
                                <ul class="nav nav-tabs" role="tablist">
                                    <li role="presentation" class="active"><a href="#agentesTab" aria-controls="Agentes" role="tab" data-toggle="tab">Agentes</a></li>
                                    <li role="presentation"><a href="#areasTab" aria-controls="Areas" role="tab" data-toggle="tab">Areas <small>(envía a todos los agentes del área)</small></a></li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content">
                                    <div role="tabpanel" class="tab-pane active" id="agentesTab">
                                        <br />
                                        <div class="input-group">
                                            <span class="input-group-addon">Buscar</span>
                                            <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_Agentes.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                                        </div>
                                        <div style="height: 400px; overflow-y: scroll;">
                                            <asp:GridView ID="gv_Agentes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkbox" TabIndex='<%#Convert.ToInt32(Eval("IdAgente")) %>' Checked='<%#Convert.ToBoolean(Eval("Seleccionado")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                                    <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                                    <asp:BoundField DataField="Area" HeaderText="Area" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div role="tabpanel" class="tab-pane" id="areasTab">
                                        <br />
                                       <div class="input-group">
                                            <span class="input-group-addon">Buscar</span>
                                            <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_Areas.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                                        </div>
                                        <div style="height: 400px; overflow-y: scroll;">
                                            <asp:GridView ID="gv_Areas" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkbox" TabIndex='<%#Convert.ToInt32(Eval("IdArea")) %>' Checked='<%#Convert.ToBoolean(Eval("Seleccionado")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Nombre" HeaderText="Legajo" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            <button type="button" class="btn btn-primary" runat="server" onserverclick="ControlarChecks">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>

            <br />

            <asp:Button Text="Enviar" CssClass="btn btn-primary" ID="btn_Enviar" OnClientClick="return VerificarAntesDeEnviar();" OnClick="btn_Enviar_Click" runat="server" />

        </div>
    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="contentScripts" ID="scriptss" runat="server">
    <script>

        function VerificarAntesDeEnviar() {
            var todos = $("[id*=IDAgente_todos]");
            if (todos.length == 0) {
                return true;
            }
            else {
                return confirm('Atención!\nEstá por enviar el mensaje a todos los agentes!!!, desea continuar?');
            }
        }

        function filter2(phrase, _id) {
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
