<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Usr_PantallaPropia.aspx.cs" Inherits="SisPer.Aplicativo.Usr_PantallaPropia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc1:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Solicitud de modificación de datos</h1>
        </div>
        <div class="panel-body">
            &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
            <div class="row">
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title">Datos personales
                                    </h1>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Apellido y nombre
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <asp:TextBox ID="tb_ApyNom" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:RequiredFieldValidator ControlToValidate="tb_ApyNom"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su apellido y nombre' />"
                                                                ID="RequiredFieldValidator1" runat="server"
                                                                ErrorMessage="Debe ingresar su apellido y nombre">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    CUIT/CUIL
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <asp:TextBox ID="tb_CUIL" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:RequiredFieldValidator ControlToValidate="tb_CUIL"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su CUIT/CUIL' />"
                                                                ID="RequiredFieldValidator8" runat="server"
                                                                ErrorMessage="'Debe ingresar su CUIT/CUIL">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    DNI
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <asp:Label ID="tb_DNI" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <%--<div class="col-md-1">
                                                            <asp:RequiredFieldValidator ControlToValidate="tb_DNI"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su DNI' />"
                                                                ID="RequiredFieldValidator3" runat="server"
                                                                ErrorMessage="Debe ingresar su DNI">
                                                            </asp:RequiredFieldValidator>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Fecha de nacimiento
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <div id="dtp_fecha_nacimiento" class="input-group date">
                                                                <input id="tb_FechaNacimiento" runat="server" class="form-control" type="text" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">

                                                            <asp:RequiredFieldValidator ControlToValidate="tb_FechaNacimiento"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su fecha de nacimiento' />"
                                                                ID="RequiredFieldValidator5" runat="server"
                                                                ErrorMessage="Debe ingresar su fecha de nacimiento">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                                                ErrorMessage="La fecha ingresada no es correcta"
                                                                OnServerValidate="CustomValidator2_ServerValidate">
                                                            </asp:CustomValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Ficha médica
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <asp:TextBox ID="tb_ficha_medica" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:RequiredFieldValidator ControlToValidate="tb_ficha_medica"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su ficha médica' />"
                                                                ID="RequiredFieldValidator2" runat="server"
                                                                ErrorMessage="Debe ingresar su ficha médica">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row" style="visibility:hidden;">
                                                <div class="col-md-3">
                                                    E-Mail
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="row">
                                                        <div class="col-md-11">
                                                            <asp:TextBox ID="tb_Email" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-md-1">
                                                            <%--<asp:RequiredFieldValidator ControlToValidate="tb_Email" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su mail' />"
                                                                ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe ingresar su mail">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_Email"
                                                                Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar un mail válido.' />"
                                                                ErrorMessage="Debe ingresar un mail válido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                            </asp:RegularExpressionValidator>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title">Foto - carnet</h1>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="thumbnail">
                                                <a href="#" data-toggle="modal" data-target="#editar_Imagen">
                                                    <asp:Image ID="img_cuenta" Width="150" Height="150" runat="server" />
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <small>CLICK SOBRE LA IMAGEN PARA CAMBIAR</small>
                                        </div>
                                    </div>
                                    <div class="modal fade" id="editar_Imagen" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar foto carnet</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="thumbnail">
                                                                <img src="..." alt="..." id="imagen_agente">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="input-group">
                                                                <span class="input-group-btn">
                                                                    <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                          <input type="file" id="archivo_imagen" runat="server" accept=".jpg,.png,.gif" onchange="Previsualizar();" />
                                                                    </span>
                                                                </span>
                                                                <input type="text" class="form-control" readonly>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btnUpload" OnClick="btnUpload_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-warning" data-dismiss="modal">Cancelar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row text-center">
                        <button runat="server" id="btn_cambiar_clave" causesvalidation="false" class="btn btn-default" onserverclick="btn_cambiar_clave_ServerClick"> 
                            <span class="glyphicon glyphicon-lock"></span> Cambiar clave
                        </button>
                        <button runat="server" id="btn_cambiar_email" causesvalidation="false" class="btn btn-default" onserverclick="btn_cambiar_email_ServerClick">
                            <span class="glyphicon glyphicon-envelope"></span> Cambiar e-mail
                        </button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h1 class="panel-title">Datos domicilio</h1>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-1">Dirección</div>
                                <div class="col-md-5">
                                    <div class="col-md-11">
                                        <asp:TextBox runat="server" ID="tb_direccion" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ControlToValidate="tb_direccion"
                                            Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su dirección' />"
                                            ID="RequiredFieldValidator4" runat="server"
                                            ErrorMessage="Debe ingresar su dirección">
                                        </asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-md-1">Localidad</div>
                                <div class="col-md-5">
                                    <div class="col-md-11">
                                        <asp:TextBox runat="server" ID="tb_localidad" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ControlToValidate="tb_localidad"
                                            Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su localidad' />"
                                            ID="RequiredFieldValidator7" runat="server"
                                            ErrorMessage="Debe ingresar su localidad">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">Aclaraciones</div>
                                <div class="col-md-11">
                                    <asp:TextBox runat="server" ID="tb_aclaracion_domicilio" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <asp:Button Text="Solicitar cambio" CssClass="btn btn-lg btn-success" ToolTip="Los cambios quedarán pendientes de su aprobación en Personal"
                runat="server" ID="btn_SolicitarCambios" OnClick="btn_SolicitarCambios_Click" />
            <asp:Button Text="Cancelar" CssClass="btn btn-lg btn-default"
                runat="server" ID="btn_cancelar" CausesValidation="false" OnClick="btn_cancelar_Click" />
        </div>
    </div>

    <script type="text/javascript">
        $(document).on('change', '.btn-file :file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

            var div = input.parent(0).parent(0).parent(0).parent(0);
            if (div.children().length == 2) {
                div.children()[1].remove();
            }

            if (input.get(0).files[0].size > 3145728) {
                var alerta = document.createElement("div");
                alerta.nodeName = "alerta";
                alerta.className = "label label-danger";
                alerta.innerHTML = "El archivo es demasiado grande (supera los 3mb), no será procesado!";
                div.append(alerta);
            }
            else {
                var correcto = document.createElement("div");
                correcto.nodeName = "alerta";
                correcto.className = "label label-success";
                correcto.innerHTML = "Tamaño de archivo válido.";
                div.append(correcto);
            }

            input.trigger('fileselect', [numFiles, label]);
        });

        $(document).ready(function () {
            $('.btn-file :file').on('fileselect', function (event, numFiles, label) {

                var input = $(this).parents('.input-group').find(':text'),
                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                if (input.length) {
                    input.val(log);
                } else {
                    if (log) alert(log);
                }

            });
        });

        $(function () {
            $('#dtp_fecha_nacimiento').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });

        function Previsualizar() {
            var preview = document.getElementById("imagen_agente");
            var file = document.getElementById("<%= archivo_imagen.ClientID %>").files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }
    </script>
</asp:Content>

