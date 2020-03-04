<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Notificaciones.aspx.cs" Inherits="SisPer.Aplicativo.Notificaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            <h3>Solicitud de documentación</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-2">
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
                        Seleccionar destinatarios
                    </button>
                </div>
                <div class="col-md-10">
                    <div runat="server" id="div_destinatarios">
                        <asp:TextBox runat="server" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2 well-sm">
                    Vencimiento
                </div>
                <div class="col-md-10 well-sm">
                    <div id="datetimepicker1" class="input-group date">
                        <input id="tb_vto" runat="server" class="form-control" type="text" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    Descripción de notificación requerida
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 well-sm">
                    <textarea runat="server" id="tb_cuerpo" class="form-control"></textarea>
                </div>
            </div>

            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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

            <asp:Button Text="Enviar" CssClass="btn btn-primary" ID="btn_Enviar" OnClick="btn_Enviar_Click" runat="server" />

        </div>
    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="contentScripts" ID="scriptss" runat="server">
    <script>
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
    
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY'
            });
        });
    </script>
</asp:Content>
