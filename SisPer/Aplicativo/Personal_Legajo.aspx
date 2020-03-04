<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Legajo.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Legajo" %>

<%@ Register Src="~/Aplicativo/Controles/Ddl_Areas.ascx" TagPrefix="uc1" TagName="Ddl_Areas" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Controles/ImagenAgente.ascx" TagPrefix="uc1" TagName="ImagenAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>


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
    <div class="panel panel-primary">
        <div class="panel-heading">
            <h1 class="panel-title">Legajo</h1>
        </div>
        <!-- encabezado -->
        <div class="panel-body">
            <div class="row">
                <div class="col-md-2">
                    <div class="row">
                        <div class="col-md-12">
                            <a href="#" data-toggle="modal" data-target="#editar_Imagen">
                                <uc1:ImagenAgente runat="server" ID="ImagenAgente" Width="120" Height="120" class="form-control" />
                            </a>
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
                                    <asp:Button Text="Aceptar" ID="btn_aceptar_imagen" OnClick="btn_aceptar_imagen_Click" CssClass="btn btn-success" runat="server" />
                                    <button type="button" class="btn btn-warning" data-dismiss="modal">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-10">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Agente</label>
                                    <asp:Label Text="apellido y nombre del agente" ID="lbl_nombre_agente" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_nombre_agente" runat="server" data-toggle="modal" data-target="#editar_nombre_agente">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_nombre_agente" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar apellido y nombre del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">Agente</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_nombre_agente" class="form-control" placeholder="Ingrese apellido y nombre del agente" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_nombre_agente" OnClick="btn_aceptar_nombre_agente_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-warning" data-dismiss="modal">Cancelar</button>
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
                                    <label>Estado civil</label>
                                    <asp:Label Text="Seleccionar" ID="lbl_estado_civil" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_estado_civil" runat="server" data-toggle="modal" data-target="#editar_estado_civil">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_estado_civil" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar estado civil del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-4">Estado civil</div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList runat="server" ID="ddl_estado_civil" CssClass="form-control" onChange="HabilitarBotonAceptarEstadoCivil()">
                                                                <asp:ListItem Text="Seleccione:" />
                                                                <asp:ListItem Text="Soltero" />
                                                                <asp:ListItem Text="Casado" />
                                                                <asp:ListItem Text="Concubinato" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_estado_civil" OnClick="btn_aceptar_estado_civil_Click" Enabled="false" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                    <label>Sexo</label>
                                    <asp:Label Text="Seleccionar" ID="lbl_sexo" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_sexo" runat="server" data-toggle="modal" data-target="#editar_sexo">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_sexo" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar sexo del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-4">Sexo</div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList runat="server" ID="ddl_sexo" CssClass="form-control" onChange="HabilitarBotonAceptarSexo()">
                                                                <asp:ListItem Text="Seleccione:" />
                                                                <asp:ListItem Text="Masculino" />
                                                                <asp:ListItem Text="Femenino" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_sexo" OnClick="btn_aceptar_sexo_Click" Enabled="false" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>D.N.I.</label>
                                    <asp:Label Text="xx.xxx.xxx" ID="lbl_dni" runat="server" />
                                    <%--<button type="button" class="btn btn-sm btn-warning" id="btn_editar_dni" runat="server" data-toggle="modal" data-target="#editar_dni">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_dni" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar D.N.I. del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">D.N.I.</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_dni" class="form-control" placeholder="Ingrese D.N.I. del agente" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_dni" OnClick="btn_aceptar_dni_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Legajo</label>
                                    <asp:Label Text="xxxx" ID="lbl_legajo" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_legajo" runat="server" data-toggle="modal" data-target="#editar_legajo">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_legajo" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar legajo del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">Legajo</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_legajo" class="form-control" placeholder="Ingrese legajo del agente" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_legajo" OnClick="btn_aceptar_legajo_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                    <label>CUIT/CUIL</label>
                                    <asp:Label Text="xx-xxxxxxxx-x" ID="lbl_cuit" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_cuit" runat="server" data-toggle="modal" data-target="#editar_cuit">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_cuit" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar CUIT/CUIL del agente</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">CUIT/CUIL</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_cuit" class="form-control" placeholder="Ingrese CUIT/CUIL del agente" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_cuit" OnClick="btn_aceptar_cuit_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Fecha nacimiento</label>
                                    <asp:Label Text="xx/xx/xxxx" ID="lbl_nacimiento" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_nacimiento" runat="server" data-toggle="modal" data-target="#editar_nacimiento">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_nacimiento" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar fecha de nacimiento</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-6">Fecha de nacimiento</div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <div id="dtp_nacimiento" class="input-group date">
                                                                    <input id="tb_nacimiento" runat="server" class="form-control" type="text" />
                                                                    <span class="input-group-addon">
                                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_nacimiento" OnClick="btn_aceptar_nacimiento_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                    <label>Situación de revista</label>
                                    <asp:Label Text="sin asignar" ID="lbl_situacion_de_revista" runat="server" />
                                    <button type="button" class="btn btn-sm btn-warning" id="btn_editar_situacion_de_revista" runat="server" data-toggle="modal" data-target="#editar_situacion_de_revista">
                                        <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                    </button>
                                    <div class="modal fade" id="editar_situacion_de_revista" role="dialog" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                    <h4 class="modal-title">Editar situación de revista</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            Tipo de movimiento
                                                        </div>
                                                        <div class="col-md-9">
                                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_tipo_situacion_de_revista" onChange="HabilitarBotonAceptarSituacionDeRevista()">
                                                                <asp:ListItem Text="Seleccione:" />
                                                                <asp:ListItem Text="Contrato de locación de obra" />
                                                                <asp:ListItem Text="Contrato de locación de servicio" />
                                                                <asp:ListItem Text="Planta permanente" />
                                                                <asp:ListItem Text="Adscripto" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3">Cargo</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_cargo_situacion_de_revista" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3">Grupo</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_grupo_situacion_de_revista" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3">Apartado</div>
                                                        <div class="col-md-9">
                                                            <input type="text" runat="server" id="tb_apartado_situacion_de_revista" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_situacion_de_revista" Enabled="false" OnClick="btn_aceptar_situacion_de_revista_Click" CssClass="btn btn-success" runat="server" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
    </div>

    <asp:HiddenField ID="TabName" runat="server" />
    <asp:HiddenField ID="titulo_curso" runat="server" />

    <div role="tabpanel" id="Tabs">

        <ul class="nav nav-pills" id="TabPrincipal" role="tablist">
            <li role="presentation" class="active"><a href="#datos_laborales" role="tab" data-toggle="tab">Datos laborales</a></li>
            <li role="presentation"><a href="#datos_personales" role="tab" data-toggle="tab">Datos personales</a></li>
            <li role="presentation"><a href="#fojas_de_servicio" role="tab" data-toggle="tab">Fojas de servicio</a></li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane fade in active" id="datos_laborales">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Ingreso en admin. pública</label>
                                        <asp:Label Text="xx/xx/xxxx" ID="lbl_ingreso_admin_publica" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_ingreso_admin_publica" runat="server" data-toggle="modal" data-target="#editar_admin_publica">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_admin_publica" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar ingreso en admin. pública</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-6">Ingreso en la admin pública</div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <div id="dtp_ingreso_admin_publica" class="input-group date">
                                                                        <input id="tb_ingreso_admin_publica" runat="server" class="form-control" type="text" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_admin_publica" OnClick="btn_aceptar_admin_publica_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Ingreso en A.T.P.</label>
                                        <asp:Label Text="xx/xx/xxxx" ID="lbl_ingreso_atp" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_ingreso_atp" runat="server" data-toggle="modal" data-target="#editar_ingreso_atp">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_ingreso_atp" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar ingreso en A.T.P.</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-6">Ingreso en en A.T.P.</div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <div id="dtp_ingreso_atp" class="input-group date">
                                                                        <input id="tb_ingreso_atp" runat="server" class="form-control" type="text" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_ingreso_atp" OnClick="btn_aceptar_ingreso_atp_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Antiguedad en otras partes</label>
                                        <asp:Label Text="xx años y xx meses" ID="lbl_anios_en_otras_partes" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_anios_en_otras_partes" runat="server" data-toggle="modal" data-target="#editar_anios_en_otras_partes">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_anios_en_otras_partes" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar años reconocidos en otras partes</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-5">Reconocimiento en otras partes</div>
                                                            <div class="col-md-7">
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        <input type="text" id="tb_anios_en_otras_partes" runat="server" class="form-control" />
                                                                    </div>
                                                                    <div class="col-md-1">años</div>
                                                                    <div class="col-md-4">
                                                                        <input type="text" id="tb_meses_en_otras_partes" runat="server" class="form-control" />
                                                                    </div>
                                                                    <div class="col-md-1">meses</div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_anios_en_otras_partes" OnClick="btn_aceptar_anios_en_otras_partes_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Ficha médica</label>
                                        <asp:Label Text="xxxx" ID="lbl_ficha_medica" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_ficha_medica" runat="server" data-toggle="modal" data-target="#editar_ficha_medica">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_ficha_medica" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar ficha médica</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-3">Ficha médica</div>
                                                            <div class="col-md-9">
                                                                <input type="text" id="tb_ficha_medica" runat="server" placeholder="ingrese número de ficha médica" class="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_ficha_medica" OnClick="btn_aceptar_ficha_medica_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Licencia ordinaria año en curso</label>
                                        <asp:Label Text="28 días" ID="lbl_licencia_anio_en_curso" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_licencia_anio_en_curso" runat="server" data-toggle="modal" data-target="#editar_licencia_anio_en_curso">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_licencia_anio_en_curso" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar licencia año en curso</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-6">Licencia año en curso</div>
                                                            <div class="col-md-6">
                                                                <input type="text" id="tb_licencia_anio_en_curso" runat="server" placeholder="ingrese días" class="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_licencia_anio_en_curso" OnClick="btn_aceptar_licencia_anio_en_curso_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Licencia ordinaria año anterior</label>
                                        <asp:Label Text="28 días" ID="lbl_licencia_anio_anterior" runat="server" />
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_licencia_anio_anterior" runat="server" data-toggle="modal" data-target="#editar_licencia_anio_anterior">
                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                        </button>
                                        <div class="modal fade" id="editar_licencia_anio_anterior" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Editar licencia año anterior</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">
                                                            <div class="col-md-6">Licencia año anterior</div>
                                                            <div class="col-md-6">
                                                                <input type="text" id="tb_licencia_anio_anterior" runat="server" placeholder="ingrese días" class="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_licencia_anio_anterior" OnClick="btn_aceptar_licencia_anio_anterior_Click" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                            <div class="col-md-12">
                                <label>Correo electrónico</label>
                                <asp:Label Text="un.correo@un.dominio.com" ID="lbl_mail" runat="server" />
                                <%--<button type="button" class="btn btn-sm btn-warning" id="btn_editar_mail" runat="server" data-toggle="modal" data-target="#editar_mail">
                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                </button>
                                <div class="modal fade" id="editar_mail" role="dialog" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title">Editar correo electrónico</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-3">Ficha médica</div>
                                                    <div class="col-md-9">
                                                        <input id="tb_mail" runat="server" placeholder="ingrese correo electrónico" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button Text="Aceptar" ID="btn_aceptar_mail" OnClick="btn_aceptar_mail_Click" CssClass="btn btn-success" runat="server" />
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-8">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Puesto de trabajo</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>Área</label>
                                                <asp:Label Text="area donde trabaja" ID="lbl_area_agente" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_area" runat="server" data-toggle="modal" data-target="#editar_area">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_area" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar área donde trabaja el agente</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Área</div>
                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList runat="server" ID="ddl_areas" CssClass="form-control" onChange="HabilitarBotonArea()">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_area" OnClick="btn_aceptar_area_Click" CssClass="btn btn-success" Enabled="false" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Como </label>
                                                <asp:Label Text="Jefe temporal hasta 01/09/2015" ID="lbl_trabaja_como" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_trabaja_como" runat="server" data-toggle="modal" data-target="#editar_trabaja_como">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_trabaja_como" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar trabaja como</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Trabaja como</div>
                                                                    <div class="col-md-4">
                                                                        <asp:DropDownList runat="server" ID="ddl_tipo_agente" CssClass="form-control" onChange="MostrarInputHastaJefeTemporal()">
                                                                            <asp:ListItem Text="Seleccione:" />
                                                                            <asp:ListItem Text="Agente" />
                                                                            <asp:ListItem Text="Jefe" />
                                                                            <asp:ListItem Text="Jefe temporal" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="form-group" id="div_hasta_jefe_temporal" style="visibility: hidden;">
                                                                            <div id="dtp_hasta_jefe_temporal" class="input-group date">
                                                                                <input type="text" id="tb_jefe_temporal_hasta" runat="server" class="form-control" placeholder="Hasta" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_trabaja_como" OnClick="btn_aceptar_trabaja_como_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-3">
                                                <label>Entrada</label>
                                                <asp:Label Text="06:30" ID="lbl_hora_desde" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_hora_desde" runat="server" data-toggle="modal" data-target="#editar_hora_desde">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_hora_desde" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar hora laboral desde</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Hora ingreso</div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" runat="server" class="form-control" id="tb_horario_laboral_desde" placeholder="06:30" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_horario_laboral_desde" OnClick="btn_aceptar_horario_laboral_desde_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Salida</label>
                                                <asp:Label Text="13:00" ID="lbl_hora_hasta" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_hora_hasta" runat="server" data-toggle="modal" data-target="#editar_hora_hasta">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_hora_hasta" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar hora laboral hasta</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Hora egreso</div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" runat="server" class="form-control" id="tb_horario_laboral_hasta" placeholder="13:00" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_horario_laboral_hasta" OnClick="btn_aceptar_horario_laboral_hasta_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Horario flexible</label>
                                                <asp:Label Text="Seleccionar" ID="lbl_horario_flexible" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_horario_flexible" runat="server" data-toggle="modal" data-target="#editar_horario_flexible">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_horario_flexible" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar horario flexible</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-4">Horario flexible</div>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList runat="server" ID="ddl_horario_flexible" CssClass="form-control" onChange="HabilitarBotonHorarioFlexible()">
                                                                            <asp:ListItem Text="Seleccione:" />
                                                                            <asp:ListItem Text="Si" />
                                                                            <asp:ListItem Text="No" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_horario_flexible" OnClick="btn_aceptar_horario_flexible_Click" Enabled="false" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Datos del usuario para el sistema</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Usuario</label>
                                                <asp:Label Text="Usuario" ID="lbl_usuario" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_usuario" runat="server" data-toggle="modal" data-target="#editar_usuario">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_usuario" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar usuario de ingreso al sistema</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Usuario</div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" id="tb_usuario" runat="server" class="form-control" placeholder="Usuario" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_usuario" OnClick="btn_aceptar_usuario_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Perfil</label>
                                                <asp:Label Text="Perfil" ID="lbl_perfil_agente" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_perfil" runat="server" data-toggle="modal" data-target="#editar_perfil">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_perfil" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar perfil de usuario</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">Perfil</div>
                                                                    <div class="col-md-9">
                                                                        <asp:DropDownList runat="server" ID="ddl_perfil_usuario" CssClass="form-control" onChange="MostrarDetallePerfilSeleccionado()">
                                                                            <asp:ListItem Text="Seleccione:" />
                                                                            <asp:ListItem Text="Agente" />
                                                                            <asp:ListItem Text="Personal" />
                                                                            <asp:ListItem Text="Guardia" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row" style="visibility: hidden;" id="div_detalle_perfil">
                                                                    <div class="col-md-12">
                                                                        <div class="alert alert-warning">
                                                                            <p id="texto_detalle"></p>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_perfil_usuario" OnClick="btn_aceptar_perfil_usuario_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button Text="Resetear contraseña" ID="btn_resetear_contraseña" CssClass="btn btn-warning btn-block" OnClick="btn_resetear_contraseña_Click" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="datos_personales">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h1 class="panel-title">Datos domicilio</h1>
                                            </div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Dirección</label>
                                                        <asp:Label Text="un domicilio 11222" ID="lbl_direccion" runat="server" />
                                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_direccion" runat="server" data-toggle="modal" data-target="#editar_direccion">
                                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                        </button>
                                                        <div class="modal fade" id="editar_direccion" role="dialog" aria-hidden="true">
                                                            <div class="modal-dialog">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                        <h4 class="modal-title">Editar dirección</h4>
                                                                    </div>
                                                                    <div class="modal-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">Dirección</div>
                                                                            <div class="col-md-9">
                                                                                <input type="text" id="tb_direccion" runat="server" placeholder="ingrese dirección" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_direccion" OnClick="btn_aceptar_direccion_Click" CssClass="btn btn-success" runat="server" />
                                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Localidad</label>
                                                        <asp:Label Text="LOCALIDAD" ID="lbl_localidad" runat="server" />
                                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_localidad" runat="server" data-toggle="modal" data-target="#editar_localidad">
                                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                        </button>
                                                        <div class="modal fade" id="editar_localidad" role="dialog" aria-hidden="true">
                                                            <div class="modal-dialog">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                        <h4 class="modal-title">Editar localidad</h4>
                                                                    </div>
                                                                    <div class="modal-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">Localidad</div>
                                                                            <div class="col-md-9">
                                                                                <input type="text" id="tb_localidad" runat="server" placeholder="ingrese localidad" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_localidad" OnClick="btn_aceptar_localidad_Click" CssClass="btn btn-success" runat="server" />
                                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <label>Aclaraciones</label>
                                                        <asp:Image ImageUrl="~/Imagenes/help.png" ID="img_aclaraciones_domicilio" ToolTip="Ninguna observación" runat="server" />
                                                        <button type="button" class="btn btn-sm btn-warning" id="btn_editar_aclaraciones_direccion" runat="server" data-toggle="modal" data-target="#editar_aclaraciones_direccion">
                                                            <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                        </button>
                                                        <div class="modal fade" id="editar_aclaraciones_direccion" role="dialog" aria-hidden="true">
                                                            <div class="modal-dialog">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                        <h4 class="modal-title">Editar aclaraciones dirección</h4>
                                                                    </div>
                                                                    <div class="modal-body">
                                                                        <div class="row">
                                                                            <div class="col-md-3">Aclaraciones</div>
                                                                            <div class="col-md-9">
                                                                                <input type="text" id="tb_aclaraciones_direccion" runat="server" placeholder="ingrese aclaraciones de direccion" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_aclaraciones_direccion" OnClick="btn_aceptar_aclaraciones_direccion_Click" CssClass="btn btn-success" runat="server" />
                                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div role="tabpanel" id="tabs_titulo_curso">
                                            <ul class="nav nav-tabs" id="TabTituloCurso">
                                                <li class="active"><a data-toggle="tab" href="#tab_titulos">Títulos</a></li>
                                                <li><a data-toggle="tab" href="#tab_cursos">Cursos/capacitaciones</a></li>
                                            </ul>
                                            <div class="tab-content">
                                                <div id="tab_titulos" class="tab-pane fade in active">
                                                    <div class="panel panel-default">
                                                        <div class="panel-body">
                                                            <div style="height: 120px; overflow-y: scroll">
                                                                <asp:GridView ID="gv_titulos" runat="server" EmptyDataText="No existen títulos cargados." ForeColor="#717171"
                                                                    AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="nivel" HeaderText="Nivel" ReadOnly="true" SortExpression="nivel" />
                                                                        <asp:BoundField DataField="titulo" HeaderText="Titulo" ReadOnly="true" SortExpression="titulo" />
                                                                        <asp:TemplateField HeaderText="Ver">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btn_ver_titulo" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                                    ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_titulo_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Quitar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btn_quitar_titulo" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                                    ImageUrl="~/Imagenes/delete.png" OnClick="btn_quitar_titulo_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_titulo" runat="server" data-toggle="modal" data-target="#agregar_titulo">
                                                                Agregar título
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="modal fade" id="agregar_titulo" role="dialog" aria-hidden="true">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                    <h4 class="modal-title">Agregar título</h4>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">Nivel</div>
                                                                        <div class="col-md-3">
                                                                            <asp:DropDownList runat="server" ID="ddl_estudios" CssClass="form-control">
                                                                                <asp:ListItem Text="Primario" />
                                                                                <asp:ListItem Text="Ciclo básico" />
                                                                                <asp:ListItem Text="Secundario" />
                                                                                <asp:ListItem Text="Terciario" />
                                                                                <asp:ListItem Text="Universitario" />
                                                                                <asp:ListItem Text="Posgrado" />
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            Años duración
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <input type="text" placeholder="años" class="form-control" runat="server" id="tb_duracion_titulo" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            Título
                                                                        </div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_descripcion_titlulo" class="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            Institución
                                                                        </div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" placeolder="institución" class="form-control" runat="server" id="tb_institucion_titulo" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            Fecha emisión
                                                                        </div>
                                                                        <div class="col-md-9">
                                                                            <div id="dtp_fecha_titulo" class="input-group date">
                                                                                <input id="tb_fecha_emision_titulo" runat="server" class="form-control" type="text" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">Archivo</div>
                                                                        <div class="col-md-9">
                                                                            <div class="input-group">
                                                                                <span class="input-group-btn">
                                                                                    <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                    <input type="file" id="archivo_titulo" runat="server" accept=".pdf,.doc,.docx" />
                                                                                    </span>
                                                                                </span>
                                                                                <input type="text" class="form-control" readonly>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_agregar_titulo" OnClick="btn_aceptar_agregar_titulo_Click" CssClass="btn btn-success" runat="server" />
                                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="tab_cursos" class="tab-pane fade">
                                                    <div class="panel panel-default">
                                                        <div class="panel-body">
                                                            <div style="height: 120px; overflow-y: scroll">
                                                                <asp:GridView ID="gv_certificados" runat="server" EmptyDataText="No existen cursos/capacitaciones cargadas." ForeColor="#717171"
                                                                    AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="descripcion" />
                                                                        <asp:BoundField DataField="lugar" HeaderText="Lugar" ReadOnly="true" SortExpression="lugar" />
                                                                        <asp:BoundField DataField="fecha_emision" HeaderText="Emitido el" ReadOnly="true" SortExpression="fecha_emision" />
                                                                        <asp:BoundField DataField="duracion" HeaderText="Duración" ReadOnly="true" SortExpression="duracion" />
                                                                        <asp:TemplateField HeaderText="Ver">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btn_ver_certificado" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                                    ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_certificado_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Quitar">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btn_quitar_certificado" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                                    ImageUrl="~/Imagenes/delete.png" OnClick="btn_quitar_certificado_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_certificado" runat="server" data-toggle="modal" data-target="#agregar_certificado">
                                                                Agregar certificado curso - capacitación - congreso
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="modal fade" id="agregar_certificado" role="dialog" aria-hidden="true">
                                                        <div class="modal-dialog">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                    <h4 class="modal-title">Agregar certificado</h4>
                                                                </div>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-3">Descripción</div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_descripcion_curso" class="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">Duración</div>
                                                                        <div class="col-md-7">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="text" runat="server" id="tb_duracion_curso" class="form-control" /></td>
                                                                                    <td>
                                                                                        <asp:DropDownList runat="server" ID="ddl_duracion_curso" CssClass="form-control">
                                                                                            <asp:ListItem Text="horas" />
                                                                                            <asp:ListItem Text="días" />
                                                                                            <asp:ListItem Text="meses" />
                                                                                            <asp:ListItem Text="años" />
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">Lugar</div>
                                                                        <div class="col-md-9">
                                                                            <input type="text" runat="server" id="tb_lugar_curso" class="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">Fecha emisión</div>
                                                                        <div class="col-md-6">
                                                                            <div id="dtp_fecha_curso" class="input-group date">
                                                                                <input id="tb_fecha_curso" runat="server" class="form-control" type="text" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col-md-3">Archivo</div>
                                                                        <div class="col-md-9">
                                                                            <div class="input-group">
                                                                                <span class="input-group-btn">
                                                                                    <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                     <input type="file" id="archivo_certificado" runat="server" name="archivo_certificado" accept=".pdf,.doc,.docx" />
                                                                                    </span>
                                                                                </span>
                                                                                <input type="text" class="form-control" readonly>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="modal-footer">
                                                                    <asp:Button Text="Aceptar" ID="btn_aceptar_agregar_certificado" OnClick="btn_agregar_certificado_Click" CssClass="btn btn-success" runat="server" />
                                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Conyuge - Pareja</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Nombre y apellido</label>conyuge
                                                <asp:Label Text="apellido y nombre" ID="lbl_nombre_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_nombre_conyuge" runat="server" data-toggle="modal" data-target="#editar_nombre_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_nombre_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar apellido y nombre del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-5">Nombre y apellido</div>
                                                                    <div class="col-md-7">
                                                                        <input type="text" runat="server" id="tb_nombre_conyuge" class="form-control" placeholder="Ingrese apellido y nombre" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_nombre_conyuge" OnClick="btn_aceptar_nombre_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-5">
                                                <label>D.N.I.</label>
                                                <asp:Label Text="xx.xxx.xxx" ID="lbl_dni_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_dni_conyuge" runat="server" data-toggle="modal" data-target="#editar_dni_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_dni_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar D.N.I. del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-5">D.N.I.</div>
                                                                    <div class="col-md-7">
                                                                        <input type="text" runat="server" id="tb_dni_conyuge" class="form-control" placeholder="Ingrese D.N.I." />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_dni_conyuge" OnClick="btn_aceptar_dni_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-7">
                                                <label>Fecha de nacimiento</label>
                                                <asp:Label Text="xx/xx/xxxx" ID="lbl_fecha_nacimiento_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_fecha_nacimiento_conyuge" runat="server" data-toggle="modal" data-target="#editar_fecha_nacimiento_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_fecha_nacimiento_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar fecha de nacimiento del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-5">Fecha de nacimiento</div>
                                                                    <div class="col-md-7">
                                                                        <div class="form-group">
                                                                            <div id="dtp_fecha_nacimiento_conyuge" class="input-group date">
                                                                                <input id="tb_fecha_nacimiento_conyuge" runat="server" class="form-control" type="text" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_fecha_nacimiento_conyuge" OnClick="btn_aceptar_fecha_nacimiento_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label>Trabaja</label>
                                                <asp:Label Text="Si/No" ID="lbl_trabaja_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_trabaja_conyuge" runat="server" data-toggle="modal" data-target="#editar_trabaja_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_trabaja_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar trabaja del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-4">Trabaja</div>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList runat="server" ID="ddl_trabaja_conyuge" CssClass="form-control">
                                                                            <asp:ListItem Text="Seleccione:" />
                                                                            <asp:ListItem Text="Si" />
                                                                            <asp:ListItem Text="No" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_trabaja_conyuge" OnClick="btn_aceptar_trabaja_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <label>Profesión</label>
                                                <asp:Label Text="Profesión" ID="lbl_profesion_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_profesion_conyuge" runat="server" data-toggle="modal" data-target="#editar_profesion_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_profesion_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar profesión del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-4">Profesión</div>
                                                                    <div class="col-md-8">
                                                                        <input type="text" runat="server" id="tb_profesion_conyuge" class="form-control" placeholder="Ingrese profesión" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_profesion_conyuge" OnClick="btn_aceptar_profesion_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Lugar</label>
                                                <asp:Label Text="Seleccione" ID="lbl_lugar_de_trabajo_conyuge" runat="server" />
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_editar_lugar_de_trabajo_conyuge" runat="server" data-toggle="modal" data-target="#editar_lugar_de_trabajo_conyuge">
                                                    <asp:Image ImageUrl="~/Imagenes/pencil.png" runat="server" />
                                                </button>
                                                <div class="modal fade" id="editar_lugar_de_trabajo_conyuge" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Editar lugar de trabajo del conyuge o pareja</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-4">Lugar</div>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList runat="server" ID="ddl_lugar_de_trabajo_conyuge" CssClass="form-control" onchange="CambioSeleccion(this);">
                                                                            <asp:ListItem Text="Seleccione:" />
                                                                            <asp:ListItem Text="Público" />
                                                                            <asp:ListItem Text="Privado" />
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-4" id="label_tipo_lugar_de_trabajo" style="visibility: hidden;">Nombre</div>
                                                                    <div class="col-md-8">
                                                                        <input type="text" style="visibility: hidden;" class="form-control" runat="server" placeholder="Ingrese dependencia del lugar de trabajo" id="tb_nombre_lugar_de_trabajo_conyuge" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_lugar_de_trabajo_conyuge" OnClick="btn_aceptar_lugar_de_trabajo_conyuge_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Hijos</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="height: 140px; overflow-y: scroll;">
                                                    <asp:GridView ID="gv_hijos" runat="server" EmptyDataText="No existen hijos cargados." ForeColor="#717171"
                                                        AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                        <Columns>
                                                            <asp:BoundField DataField="nombre" HeaderText="Apellido y nombre" ReadOnly="true" SortExpression="nombre" />
                                                            <asp:BoundField DataField="dni" HeaderText="D.N.I." ReadOnly="true" SortExpression="dni" />
                                                            <asp:BoundField DataField="nacimiento" HeaderText="Natalicio" ReadOnly="true" SortExpression="nacimiento" DataFormatString="{0:d}" />
                                                            <asp:BoundField DataField="edad" HeaderText="Edad" ReadOnly="true" SortExpression="edad" />
                                                            <asp:TemplateField HeaderText="Ver">
                                                                <ItemTemplate>
                                                                    <button
                                                                        type="button"
                                                                        data-toggle="modal"
                                                                        data-target="#ver_hijo"
                                                                        data-nombre='<%#Eval("nombre")%>'
                                                                        data-dni='<%#Eval("dni")%>'
                                                                        data-nacimiento='<%#Eval("nacimiento")%>'
                                                                        data-observaciones='<%#Eval("observaciones")%>'>
                                                                        Ver
                                                                    </button>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quitar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btn_eliminar_hijo" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                        ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_hijo_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <div class="modal fade" id="ver_hijo" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Datos hijo</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        Nombre y apellido
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" id="lbl_nombre_hijo" readonly="readonly" class="form-control" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-1">
                                                                        D.N.I. 
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <input type="text" id="lbl_dni_hijo" readonly="readonly" class="form-control" />
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        Fecha de nacimiento
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <input type="text" id="lbl_fecha_nacimiento_hijo" readonly="readonly" class="form-control" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        Observaciones
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <textarea id="lbl_observaciones_hijo" readonly="readonly" class="form-control" rows="5" placeholder="Ingrese observaciones del hijo">
                                                                        </textarea>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <button type="button" class="btn btn-warning btn-sm" id="btn_agregar_hijo" runat="server" data-toggle="modal" data-target="#agregar_hijo">
                                                    Agregar hijo
                                                </button>

                                                <div class="modal fade" id="agregar_hijo" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Agregar hijo</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        Nombre y apellido
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" id="tb_nombre_hijo" class="form-control" runat="server" placeholder="Ingrese nombre y apellido del hijo" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-1">
                                                                        D.N.I. 
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <input type="text" id="tb_dni_hijo" class="form-control" runat="server" placeholder="D.N.I." />
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        Fecha de nacimiento
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <div id="dtp_fecha_nacimiento_hijo" class="input-group date">
                                                                                <input id="tb_fecha_nacimiento_hijo" runat="server" class="form-control" type="text" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        Observaciones
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <textarea runat="server" id="tb_observaciones_hijo" class="form-control" rows="5" placeholder="Ingrese observaciones del hijo" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_hijo" OnClick="btn_aceptar_hijo_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                </div>
            </div>
            <div role="tabpanel" class="tab-pane fade" id="fojas_de_servicio">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Carrera administrativa</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="height: 120px; overflow-y: scroll">
                                                    <asp:GridView ID="gv_carrera_administrativa" runat="server" EmptyDataText="No existen movimientos cargados." ForeColor="#717171"
                                                        AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                        <Columns>
                                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" />
                                                            <asp:BoundField DataField="tipo_novedad" HeaderText="Tipo movimiento" ReadOnly="true" />
                                                            <asp:BoundField DataField="tipo_instrumento" HeaderText="Instrumento" ReadOnly="true" />
                                                            <asp:BoundField DataField="nro_instrumento" HeaderText="Número" ReadOnly="true" />
                                                            <asp:BoundField DataField="cargo" HeaderText="Cargo" ReadOnly="true" />
                                                            <asp:BoundField DataField="grupo" HeaderText="Grupo" ReadOnly="true" />
                                                            <asp:BoundField DataField="apartado" HeaderText="Apartado" ReadOnly="true" />
                                                            <asp:TemplateField HeaderText="Ver">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btn_ver_novedad_carrera" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                        ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_novedad_carrera_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quitar">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btn_eliminar_novedad_carrera" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                        ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_novedad_carrera_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_carrera_administrativa" runat="server" data-toggle="modal" data-target="#agregar_novedad_carrera">
                                                    Agregar
                                                </button>
                                            </div>
                                        </div>
                                        <div class="modal fade" id="agregar_novedad_carrera" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                        <h4 class="modal-title">Agregar novedad carrera administrativa</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <div class="row">

                                                            <div class="col-md-3">
                                                                Tipo de movimiento
                                                            </div>
                                                            <div class="col-md-9">
                                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_tipo_movimiento_carrera_administrativa" onChange="HabilitarBotonAceptarCarrera()">
                                                                    <asp:ListItem Text="Seleccione:" />
                                                                    <asp:ListItem Text="Contrato de locación de obra" />
                                                                    <asp:ListItem Text="Contrato de locación de servicio" />
                                                                    <asp:ListItem Text="Planta permanente" />
                                                                    <asp:ListItem Text="Adscripto" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                Tipo instrumento
                                                            </div>
                                                            <div class="col-md-9">
                                                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_tipo_instrumento_carrera_administrativa" onChange="HabilitarBotonAceptarCarrera()">
                                                                    <asp:ListItem Text="Seleccione:" />
                                                                    <asp:ListItem Text="Ley" />
                                                                    <asp:ListItem Text="Decreto" />
                                                                    <asp:ListItem Text="Resolución ministerial" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">Nro instrumento</div>
                                                            <div class="col-md-9">
                                                                <input type="text" runat="server" id="tb_nro_instrumento_carrera_administrativa" class="form-control" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">Cargo</div>
                                                            <div class="col-md-9">
                                                                <input type="text" runat="server" id="tb_cargo_carrera_administrativa" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">Grupo</div>
                                                            <div class="col-md-9">
                                                                <input type="text" runat="server" id="tb_grupo_carrera_administrativa" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">Apartado</div>
                                                            <div class="col-md-9">
                                                                <input type="text" runat="server" id="tb_apartado_carrera_administrativa" class="form-control" onkeyup="VerificarCargoGrupoApartado()" />
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                Fecha
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="form-group">
                                                                    <div id="dtp_fecha_instrumento_carrera_administrativa" class="input-group date">
                                                                        <input id="tb_fecha_instrumento_carrera_administrativa" runat="server" class="form-control" type="text" />
                                                                        <span class="input-group-addon">
                                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-md-3">Archivo</div>
                                                            <div class="col-md-9">
                                                                <div class="input-group">
                                                                    <span class="input-group-btn">
                                                                        <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                    <input type="file" id="archivo_carrera_administrativa" runat="server" accept=".pdf,.doc,.docx" />
                                                                        </span>
                                                                    </span>
                                                                    <input type="text" class="form-control" readonly>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <button runat="server" onserverclick="btn_aceptar_situacion_revista_novedad_carrera_Click" id="btn_aceptar_situacion_revista_novedad_carrera" style="visibility: hidden;" class="btn btn-success">Aceptar y copiar a situación de revista actual</button>
                                                        <asp:Button Text="Aceptar" ID="btn_aceptar_carrera_administrativa" OnClick="btn_aceptar_carrera_administrativa_Click" Enabled="false" CssClass="btn btn-success" runat="server" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Afectación, designación, asignación</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div style="height: 120px; overflow-y: scroll">
                                            <asp:GridView ID="gv_afectacion_designacion_asignacion" runat="server" EmptyDataText="No existen registros cargados." ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:BoundField DataField="tipo_instrumento" HeaderText="Instrumento" ReadOnly="true" />
                                                    <asp:BoundField DataField="nro_instrumento" HeaderText="Nro" ReadOnly="true" />
                                                    <asp:BoundField DataField="anio_instrumento" HeaderText="Fecha" ReadOnly="true" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Ver">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_ver_afectacion_designacion" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_afectacion_designacion_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quitar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_eliminar_afectacion_designacion" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_afectacion_designacion_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_afectacion_designacion" runat="server" data-toggle="modal" data-target="#agregar_afectacion_designacion">
                                            Agregar
                                        </button>
                                    </div>
                                </div>
                                <div class="modal fade" id="agregar_afectacion_designacion" role="dialog" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title">Agregar afectación designación asignación</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-3">Nro instrumento</div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_nro_instrumento_afectacion_designacion" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Tipo instrumento
                                                    </div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_tipo_instrumento_afectacion_designacion" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Fecha
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div class="form-group">
                                                            <div id="dtp_instrumento_afectacion_designacion" class="input-group date">
                                                                <input id="tb_anio_instrumento_afectacion_designacion" runat="server" class="form-control" type="text" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Descripcion
                                                    </div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_descripcion_afectacion_designacion" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">Archivo</div>
                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-btn">
                                                                <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                    <input type="file" id="archivo_afectacion_designacion" runat="server" accept=".pdf,.doc,.docx" />
                                                                </span>
                                                            </span>
                                                            <input type="text" class="form-control" readonly>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button Text="Aceptar" ID="btn_aceptar_afectacion_designacion" OnClick="btn_aceptar_afectacion_designacion_Click" CssClass="btn btn-success" runat="server" />
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Subrogancia, bonificación, antigüedad</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div style="height: 120px; overflow-y: scroll">
                                            <asp:GridView ID="gv_subrrogancia_bonificacion_antiguedad" runat="server" EmptyDataText="No existen registros cargados." ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:BoundField DataField="tipo_instrumento" HeaderText="Instrumento" ReadOnly="true" />
                                                    <asp:BoundField DataField="nro_instrumento" HeaderText="Nro" ReadOnly="true" />
                                                    <asp:BoundField DataField="anio_instrumento" HeaderText="Fecha" ReadOnly="true" />
                                                    <asp:BoundField DataField="vigencia_instrumento" HeaderText="Vigencia" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Ver">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_ver_subrrogancia_bonificacion" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_subrrogancia_bonificacion_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quitar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_eliminar_subrogancia_bonificacion" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_subrogancia_bonificacion_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_bonificacion_antiguedad" runat="server" data-toggle="modal" data-target="#agregar_bonificacion_antiguedad">
                                            Agregar
                                        </button>
                                    </div>
                                </div>
                                <div class="modal fade" id="agregar_bonificacion_antiguedad" role="dialog" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title">Agregar subrogancia, bonificación, antigüedad</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-3">Nro instrumento</div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_nro_instrumento_bonificacion_antiguedad" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Tipo instrumento
                                                    </div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_tipo_instrumento_bonificacion_antiguedad" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Fecha
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div class="form-group">
                                                            <div id="dtp_instrumento_bonificacion_antiguedad" class="input-group date">
                                                                <input id="tb_anio_instrumento_bonificacion_antiguedad" runat="server" class="form-control" type="text" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Vigencia
                                                    </div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_vigencia_instrumento_bonificacion_antiguedad" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">Archivo</div>
                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-btn">
                                                                <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                    <input type="file" id="archivo_bonificacion_antiguedad" runat="server" accept=".pdf,.doc,.docx" />
                                                                </span>
                                                            </span>
                                                            <input type="text" class="form-control" readonly>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button Text="Aceptar" ID="btn_aceptar_agregar_bonificacion_antiguedad" OnClick="btn_aceptar_agregar_bonificacion_antiguedad_Click" CssClass="btn btn-success" runat="server" />
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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
                                        <h1 class="panel-title">Otros eventos</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div style="height: 120px; overflow-y: scroll">
                                            <asp:GridView ID="gv_otros_eventos" runat="server" EmptyDataText="No existen registros cargados." ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="true" />
                                                    <asp:BoundField DataField="lugar" HeaderText="Lugar" ReadOnly="true" />
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Ver">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_ver_otros_eventos" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_otros_eventos_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quitar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_eliminar_otros_eventos" runat="server" CommandArgument='<%#Eval("id")%>'
                                                                ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_otros_eventos_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <button type="button" class="btn btn-sm btn-warning" id="btn_agregar_otros_eventos" runat="server" data-toggle="modal" data-target="#agregar_otros_eventos">
                                            Agregar
                                        </button>
                                    </div>
                                </div>
                                <div class="modal fade" id="agregar_otros_eventos" role="dialog" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title">Agregar otros eventos</h4>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-3">Descripción</div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_descripcion_otros_eventos" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Lugar
                                                    </div>
                                                    <div class="col-md-9">
                                                        <input type="text" runat="server" id="tb_lugar_otros_eventos" class="form-control" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        Fecha
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div class="form-group">
                                                            <div id="dtp_fecha_otro_evento" class="input-group date">
                                                                <input id="tb_fecha_otro_evento" runat="server" class="form-control" type="text" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-3">Archivo</div>
                                                    <div class="col-md-9">
                                                        <div class="input-group">
                                                            <span class="input-group-btn">
                                                                <span class="btn btn-primary btn-file">Seleccionar&hellip;
                                                                                    <input type="file" id="archivo_otro_evento" runat="server" accept=".pdf,.doc,.docx" />
                                                                </span>
                                                            </span>
                                                            <input type="text" class="form-control" readonly>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button Text="Aceptar" ID="btn_aceptar_otro_evento" OnClick="btn_aceptar_otro_evento_Click" CssClass="btn btn-success" runat="server" />
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
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

        $('#ver_hijo').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var nombre = button.data('nombre')
            var dni = button.data('dni')
            var nacimiento = button.data('nacimiento')
            var observaciones = button.data('observaciones')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #lbl_nombre_hijo').val(nombre)
            modal.find('.modal-body #lbl_dni_hijo').val(dni)
            modal.find('.modal-body #lbl_fecha_nacimiento_hijo').val(nacimiento.substring(0, 11))
            modal.find('.modal-body #lbl_observaciones_hijo').val(observaciones)
        })

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
            $('#dtp_ingreso_admin_publica').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_ingreso_atp').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_nacimiento_conyuge').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_nacimiento_hijo').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_hasta_jefe_temporal').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_curso').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_otro_evento').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_titulo').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_instrumento_carrera_administrativa').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_instrumento_afectacion_designacion').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_instrumento_bonificacion_antiguedad').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_nacimiento').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });

        //desde cambio seleccion de lugar de trabajo conyuge
        function CambioSeleccion(ddl) {
            //alert(ddl.value);
            var label = document.getElementById("label_tipo_lugar_de_trabajo");
            var input = document.getElementById("<%= tb_nombre_lugar_de_trabajo_conyuge.ClientID %>");
            if (ddl.value == "Público") {
                label.style.visibility = "visible";
                input.style.visibility = "visible";
                label.innerText = "Dependencia";

            }
            else {
                label.style.visibility = "hidden";
                input.style.visibility = "hidden";
            }

        }

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

    <script type="text/javascript">

        $('ul#TabPrincipal a').on('shown.bs.tab', function (e) {
            //e.target // newly activated tab
            //e.relatedTarget // previous active tab
            var tab = document.getElementById("<%= TabName.ClientID %>");
            tab.value = e.target.hash.replace('#', '');
        })

        $('ul#TabTituloCurso a').on('shown.bs.tab', function (e) {
            //e.target // newly activated tab
            //e.relatedTarget // previous active tab
            var tab = document.getElementById("<%= titulo_curso.ClientID %>");
            tab.value = e.target.hash.replace('#', '');
        })

        $(function () {
            var tabName = document.getElementById("<%= TabName.ClientID %>").value != "" ? document.getElementById("<%= TabName.ClientID %>").value : "datos_laborales";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');

            var tabName_titulo_curso = document.getElementById("<%= titulo_curso.ClientID %>").value != "" ? document.getElementById("<%= titulo_curso.ClientID %>").value : "tab_titulos";
            $('#Tabs a[href="#' + tabName_titulo_curso + '"]').tab('show');
        });
    </script>

    <script>
        function MostrarInputHastaJefeTemporal() {
            var ddl = document.getElementById("<%= ddl_tipo_agente.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_trabaja_como.ClientID %>");
            if (ddl.value == "Jefe temporal") {
                div_hasta_jefe_temporal.style.visibility = "visible";
            }
            else {
                div_hasta_jefe_temporal.style.visibility = "hidden";
            }
            btn.disabled = ddl.value == "Seleccione:";

        }

        function HabilitarBotonAceptarSexo() {
            var ddl = document.getElementById("<%= ddl_sexo.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_sexo.ClientID %>");
            btn.disabled = ddl.value == "Seleccione:";
        }

        function HabilitarBotonArea() {
            var ddl = document.getElementById("<%= ddl_areas.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_area.ClientID %>");
            btn.disabled = ddl.value == "0";
        }

        function HabilitarBotonHorarioFlexible() {
            var ddl = document.getElementById("<%= ddl_horario_flexible.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_horario_flexible.ClientID %>");
            btn.disabled = ddl.value == "Seleccione:";
        }

        function HabilitarBotonAceptarEstadoCivil() {
            var ddl = document.getElementById("<%= ddl_estado_civil.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_estado_civil.ClientID %>");
            btn.disabled = ddl.value == "Seleccione:";
        }

        function HabilitarBotonAceptarCarrera() {
            var ddl = document.getElementById("<%= ddl_tipo_movimiento_carrera_administrativa.ClientID %>");
            var ddl1 = document.getElementById("<%= ddl_tipo_instrumento_carrera_administrativa.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_carrera_administrativa.ClientID %>");
            btn.disabled = ddl.value == "Seleccione:" || ddl1.value == "Seleccione:";
        }

        function HabilitarBotonAceptarSituacionDeRevista() {
            var ddl = document.getElementById("<%= ddl_tipo_situacion_de_revista.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_situacion_de_revista.ClientID %>");
            btn.disabled = ddl.value == "Seleccione:";
        }

        function VerificarCargoGrupoApartado() {
            var cargo = document.getElementById("<%= tb_cargo_carrera_administrativa.ClientID %>");
            var grupo = document.getElementById("<%= tb_grupo_carrera_administrativa.ClientID %>");
            var apartado = document.getElementById("<%= tb_apartado_carrera_administrativa.ClientID %>");
            var btn_copiar_situacion_de_revista = document.getElementById("<%= btn_aceptar_situacion_revista_novedad_carrera.ClientID %>");

            if (cargo.value != "" && grupo.value != "" && apartado.value != "") {
                btn_copiar_situacion_de_revista.style.visibility = "visible";
            }
            else {
                btn_copiar_situacion_de_revista.style.visibility = "hidden";
            }
        }

        function MostrarDetallePerfilSeleccionado() {
            var ddl = document.getElementById("<%= ddl_perfil_usuario.ClientID %>");
            var btn = document.getElementById("<%= btn_aceptar_perfil_usuario.ClientID %>");

            switch (ddl.value) {
                case "Agente":
                    texto_detalle.innerText = "Perfil Agente: perfil general utilizado por agentes que acceden al sistema ya sean jefes o no.";
                    div_detalle_perfil.style.visibility = "visible";
                    break;
                case "Personal":
                    texto_detalle.innerText = "Perfil Personal: estos usuarios pueden acceder a cierres de días, modificación de datos, agregar marcaciones, etc.";
                    div_detalle_perfil.style.visibility = "visible";
                    break;
                case "Guardia":
                    texto_detalle.innerText = "Perfil Guardia: estos usuarios pueden verificar/registrar el ingreso de agentes de ATP fuera del horario laboral.";
                    div_detalle_perfil.style.visibility = "visible";
                    break;
                default:
                    div_detalle_perfil.style.visibility = "hidden"
                    btn.disabled = ddl.value == "Seleccione:";
            }
        }
    </script>
</asp:Content>
