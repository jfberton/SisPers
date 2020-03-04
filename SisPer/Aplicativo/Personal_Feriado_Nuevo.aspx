<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Feriado_Nuevo.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Feriado_Nuevo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
    <uc1:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h4 class="panel-title">Feriado</h4>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ValidationGroup="feriado"
                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />

                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Fecha</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_Fecha" ValidationGroup="feriado"
                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha del feriado' />"
                        ErrorMessage="Debe ingresar la fecha del feriado"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar una fecha válida' />"
                        ErrorMessage="Debe seleccionar una fecha válida" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="feriado"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='En la fecha seleccionada ya existe un feriado agendado' />"
                        ErrorMessage="En la fecha seleccionada ya existe un feriado agendado" OnServerValidate="CustomValidator2_ServerValidate" ValidationGroup="feriado"></asp:CustomValidator>
                </div>
                <div class="col-md-8">
                    <div id="datetimepicker2" class="input-group date">
                        <input id="tb_Fecha" runat="server" class="form-control" type="text" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Descripcion / Motivo</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Motivo"
                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el motivo del feriado' />" ValidationGroup="feriado"
                        ErrorMessage="Debe ingresar el motivo del feriado"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-8">
                    <asp:TextBox ID="tb_Motivo" runat="server" CssClass="form-control" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Button Text="Guardar" ID="btn_Guardar" runat="server" CssClass="btn btn-primary" ValidationGroup="feriado"
                        OnClick="btn_Guardar_Click" />
                    <asp:Button Text="Cancelar" ID="btn_cancelar" CausesValidation="false" runat="server" CssClass="btn btn-danger"
                        OnClick="btn_cancelar_Click" />
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h4 class="panel-title">Asueto parcial</h4>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList" ValidationGroup="asueto"
                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <label>Fecha</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_fecha_asueto_parcial" ValidationGroup="asueto"
                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha del asueto' />"
                        ErrorMessage="Debe ingresar la fecha del asueto"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cv_fecha_asueto_valida" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar una fecha válida' />"
                        ErrorMessage="Debe seleccionar una fecha válida" OnServerValidate="cv_fecha_asueto_valida_ServerValidate" ValidationGroup="asueto"></asp:CustomValidator>
                    <asp:CustomValidator ID="cv_dia_libre_asueto_parcial" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='En la fecha seleccionada ya existe un feriado agendado' />"
                        ErrorMessage="En la fecha seleccionada ya existe un feriado agendado" OnServerValidate="cv_dia_libre_asueto_parcial_ServerValidate" ValidationGroup="asueto"></asp:CustomValidator>
                </div>
                <div class="col-md-4">
                    <div id="datetimepicker1" class="input-group date">
                        <input id="tb_fecha_asueto_parcial" runat="server" class="form-control" type="text" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Area/s</label>
                    <asp:CustomValidator ID="cv_al_menos_un_area" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar al menos un área' />"
                        ErrorMessage="Debe seleccionar al menos un área" OnServerValidate="cv_al_menos_un_area_ServerValidate" ValidationGroup="asueto"></asp:CustomValidator>
                </div>
                <div class="col-md-4">
                    <div class="input-group navbar-btn">
                        <div runat="server" id="div_destinatarios">
                            <input type="text" runat="server" class="form-control" disabled="true" placeholder="Agregar areas">
                        </div>
                        <span class="input-group-btn">
                            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#area_modal">
                                <span class="glyphicon glyphicon-search"></span>
                            </button>
                        </span>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <label>E/S</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList runat="server" ID="ddl_entrada_salida" CssClass="form-control">
                        <asp:ListItem Text="Entrada" />
                        <asp:ListItem Text="Salida" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label>Hora</label>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="asueto"
                        Text="<img src='../../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                        ControlToValidate="tb_hora_entrada_salida" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                        ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-4">
                    <div id="datetimepicker_hora" class="input-group date">
                        <input id="tb_hora_entrada_salida" runat="server" class="form-control" type="text" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-time"></span>
                        </span>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <label>Observaciones</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_observaciones" ValidationGroup="asueto"
                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar observación del asueto' />"
                        ErrorMessage="Debe ingresar la fecha del feriado"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-10">
                    <asp:TextBox runat="server" CssClass="form-control" ID="tb_observaciones" />

                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Button Text="Guardar" runat="server" ID="btn_guardar_entrada_salida" ValidationGroup="asueto" OnClick="btn_guardar_entrada_salida_Click" CssClass="btn btn-primary" />
                    <asp:Button Text="Cancelar" runat="server" CssClass="btn btn-danger" ID="btn_cancelar_asueto" OnClick="btn_cancelar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="area_modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Seleccione destinatarios</h4>
                    <asp:Button Text="Todos" ToolTip="Envia a todos los agentes" runat="server" ID="btn_Todos" OnClick="btn_Todos_Click" CssClass="btn btn-warning" />
                </div>
                <div class="modal-body">
                    Filtro 
                    <input name="txtTerm" onkeyup="filter2(this, '<%=gv_Areas.ClientID %>')" type="text">
                    <p></p>
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
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                    <button type="button" class="btn btn-primary" runat="server" onserverclick="ControlarChecks">Aceptar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="contentScripts">
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

        $(function () {
            $('#datetimepicker1').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY'
            });
        });

        $(function () {
            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY'
            });
        });

        $(function () {
            $('#datetimepicker_hora').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
    </script>
</asp:Content>
