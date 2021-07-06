<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Form1214_Nuevo.aspx.cs" Inherits="SisPer.Aplicativo.Form1214_Nuevo" %>

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
    <div class="panel panel-default" id="main_panel" runat="server">
        <div class="panel-heading">
            <h4 class="panel-title">
                <asp:Label Text="" ID="lbl_encabezado1214" runat="server" /></h4>
        </div>
        <div class="panel-body">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList" ValidationGroup="general_214"
                CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
            <p />

            <asp:HiddenField runat="server" ID="id_formulario" />

            <%--Días, destino y tareas--%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Días, destino y tareas</h4>
                </div>
                <div class="panel-body">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" ValidationGroup="fecha"
                        CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-heading'>&nbsp;Corrija las fechas antes de continuar:</div>" />
                    <p />
                    <div class="row" runat="server" id="div_selecciona_fechas" visible="true">
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-11">
                                    <div class="form-group">
                                        <div class="input-group date" id="datetimepicker1">
                                            <span class="input-group-addon">Desde</span>
                                            <input type="text" class="form-control" id="tb_desde" runat="server" />
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ControlToValidate="tb_desde" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar fecha desde' />"
                                        ID="rv_desde" ValidationGroup="fecha" runat="server" ErrorMessage="Debe ingresar fecha desde"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_desde" runat="server" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='La fecha desde, debe ser igual o mayor a la fecha actual' />"
                                        ErrorMessage="La fecha desde, debe ser igual o mayor a la fecha actual" ForeColor="Red" ValidationGroup="fecha" OnServerValidate="cv_desde_ServerValidate" />
                                    <asp:RequiredFieldValidator ControlToValidate="tb_desde" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar fecha desde' />"
                                        ID="rv_general_desde" ValidationGroup="general_214" runat="server" ErrorMessage="Debe ingresar fecha desde"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_general_desde" runat="server" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='La fecha desde, debe ser igual o mayor a la fecha actual' />"
                                        ErrorMessage="La fecha desde, debe ser igual o mayor a la fecha actual" ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_desde_ServerValidate" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-11">
                                    <div class="input-group date" id="datetimepicker2">
                                         <span class="input-group-addon">Hasta</span>
                                        <input type="text" class="form-control" id="tb_hasta" runat="server" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ControlToValidate="tb_desde" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar fecha hasta' />"
                                        ID="rv_hasta" runat="server" ValidationGroup="fecha" ErrorMessage="Debe ingresar fecha hasta"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_fechas" runat="server" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='La fecha desde, debe ser igual o mayor a la fecha hasta' />"
                                        ErrorMessage="La fecha desde, debe ser igual o mayor a la fecha hasta" ForeColor="Red" ValidationGroup="fecha" OnServerValidate="cv_fechas_ServerValidate" />
                                    <asp:RequiredFieldValidator ControlToValidate="tb_hasta" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar fecha hasta' />"
                                        ID="rv_general_hasta" runat="server" ValidationGroup="general_214" ErrorMessage="Debe ingresar fecha hasta"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cv_general_hasta" runat="server" Style="margin: auto; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='La fecha desde, debe ser igual o mayor a la fecha hasta' />"
                                        ErrorMessage="La fecha desde, debe ser igual o mayor a la fecha hasta" ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_fechas_ServerValidate" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <asp:Button Text="Confirmar fechas" ID="btn_confirmar_fechas" runat="server" ValidationGroup="fecha" CssClass="btn btn-default" OnClick="btn_confirmar_fechas_Click" />

                        </div>
                    </div>
                    <div class="row" runat="server" id="div_muestra_fechas" visible="false">
                        <div class="col-md-10">
                            <p />
                            <asp:Label Text="" ID="lbl_diascorridos" CssClass="alert alert-success" runat="server" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button Text="Cambiar fechas" ID="btn_cambiar_fechas" CausesValidation="false" runat="server" CssClass="btn btn-danger" Visible="false" OnClick="btn_cambiar_fechas_Click" />
                            <p />
                        </div>
                    </div>
                    <p />
                    <div class="row">
                        <div class="col-md-11">
                            <asp:HiddenField ID="dentro_fuera" runat="server" />

                            <div class="input-group">
                                <span class="input-group-addon">Destino</span>
                                <asp:TextBox runat="server" ID="tb_destino" CssClass="form-control" />
                                <div class="input-group-btn">
                                    <button type="button" class="btn btn-default dropdown-toggle" id="btn_action" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Seleccionar <span class="caret"></span></button>
                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li><a href="#" onclick="SeleccionDentroFuera(0);return false;">Seleccionar</a></li>
                                        <li><a href="#" onclick="SeleccionDentroFuera(1);return false;">Dentro de la provincia</a></li>
                                        <li><a href="#" onclick="SeleccionDentroFuera(2);return false;">Fuera de la provincia</a></li>
                                    </ul>
                                </div>
                                <!-- /btn-group -->
                            </div>
                            <!-- /input-group -->
                        </div>
                        <!-- /.col-lg-6 -->


                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ControlToValidate="tb_destino" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar destino' />"
                                ID="rv_destino" runat="server" ValidationGroup="general_214" ErrorMessage="Debe ingresar destino"></asp:RequiredFieldValidator>

                            <asp:CustomValidator ID="cv_dentro_fuera" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar si el destino es dentro o fuera de la provincia.' />"
                                ErrorMessage="Debe seleccionar si el destino es dentro o fuera de la provincia." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_dentro_fuera_ServerValidate" />
                        </div>
                    </div>
                    <p />
                    <div class="row">
                        <div class="col-md-11">
                             <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">Tareas</span>
                                            <asp:TextBox runat="server" TextMode="MultiLine" Rows="3" ID="tb_tareas" CssClass="form-control" />
                                        </div>
                                    </div>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ControlToValidate="tb_tareas" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar las tareas a realizar' />"
                                ID="rv_tareas" runat="server" ValidationGroup="general_214" ErrorMessage="Debe ingresar las tareas a realizar"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>


            <%--Nómina de agentes--%>
            <div class="panel panel-default" id="panel_nomina" runat="server">
                <div class="panel-heading">
                    <h4 class="panel-title">Nómina de agentes</h4>
                    <asp:CustomValidator ID="cv_agentes_rechazados" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe eliminar los agentes rechazados de la nómina.' />"
                        ErrorMessage="Debe eliminar los agentes rechazados de la nómina." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_agentes_rechazados_ServerValidate" />
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-1">1)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Jefe de comisión:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" style="margin-left: 0;" id="btn_agente_1" data-toggle="modal" runat="server" data-target="#modal1">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar jefe de comisión
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_1">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_1" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_1" OnClientClick="RechazarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                            <asp:CustomValidator ID="cv_jefe" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe asignar un jefe de comisión.' />"
                                                ErrorMessage="Debe asignar un jefe de comisión." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_jefe_ServerValidate" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <p />
                            <div class="row">
                                <div class="col-md-1">2)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_2" runat="server" data-toggle="modal" data-target="#modal2">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_2">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_2" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_2" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">3)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_3" runat="server" data-toggle="modal" data-target="#modal3">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_3">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_3" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_3" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">4)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_4" runat="server" data-toggle="modal" data-target="#modal4">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_4">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_4" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_4" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">5)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_5" runat="server" data-toggle="modal" data-target="#modal5">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_5">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_5" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_5" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-11">
                                    <div class="input-group">
                                        <span class="input-group-addon">Estrato del jefe de comisión:</span>
                                        <asp:DropDownList runat="server" ID="ddl_estrato" CssClass="form-control" OnSelectedIndexChanged="ddl_estrato_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:CustomValidator ID="cv_estrato" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar el estrato del jefe de comisión.' />"
                                        ErrorMessage="Debe seleccionar el estrato del jefe de comisión." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_estrato_ServerValidate" />
                                </div>
                            </div>

                            <p />
                            <div class="row">
                                <div class="col-md-1">6)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_6" runat="server" data-toggle="modal" data-target="#modal6">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_6">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_6" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_6" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">7)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_7" runat="server" data-toggle="modal" data-target="#modal7">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_7">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_7" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_7" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">8)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_8" runat="server" data-toggle="modal" data-target="#modal8">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_8">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_8" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_8" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-1">9)</div>
                                <div class="col-md-11">
                                    <div class="row">
                                        <div class="col-md-4"></div>
                                        <div class="col-md-8">
                                            <button type="button" class="btn btn-default" id="btn_agente_9" runat="server" data-toggle="modal" data-target="#modal9">
                                                <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                Agregar agente
                                            </button>
                                            <div class="input-group" runat="server" visible="false" id="group_agente_9">
                                                <asp:TextBox runat="server" ReadOnly="true" ID="txt_agente_9" />
                                                <span class="input-group-btn">
                                                    <asp:Button Text="X" ID="btn_del_agente_9" OnClientClick="ConfirmarEliminacion()" CssClass="btn btn-danger" OnClick="btn_del_agente_x_ServerClick" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <%--Movilidad y anticipos--%>
            <div class="panel panel-default" id="panel_movilidad" runat="server">
                <div class="panel-heading">
                    <h4 class="panel-title">Movilidad y anticipos</h4>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5">
                             <asp:HiddenField ID="hf_movilidad" runat="server" />

                            <div class="input-group">
                                <span class="input-group-addon">Movilidad</span>
                                <div class="input-group-btn">
                                    <button type="button" class="btn btn-default dropdown-toggle" id="btn_seleccion" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Seleccionar <span class="caret"></span></button>
                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li><a href="#" onclick="SeleccionMovilidad(0);return false;">Seleccionar</a></li>
                                        <li><a href="#" onclick="SeleccionMovilidad(1);return false;">Vehículo oficial</a></li>
                                        <li><a href="#" onclick="SeleccionMovilidad(2);return false;">Vehículo particular</a></li>
                                        <li><a href="#" onclick="SeleccionMovilidad(3);return false;">Transporte público</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <asp:CustomValidator ID="cv_anticipo" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar un tipo de movilidad.' />"
                            ErrorMessage="Debe seleccionar un tipo de movilidad." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_anticipo_ServerValidate" />
                        <div class="col-md-4">
                            Anticipo para:
                            <label id="lbl_monto_anticipo" class="badge">Debe seleccionar el tipo de mobilidad.</label>
                        </div>
                        <div class="col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">$</span>
                                <%--<input type="text" runat="server" id="tb_monto_anticipo" class="form-control" placeholder="Monto anticipo.">--%>
                                <asp:TextBox runat="server" CssClass="form-control" ID="tb_monto_anticipo" placeholder="Monto anticipo." />
                                <asp:CustomValidator ID="cv_monto_anticipo" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='El monto del anticipo debe ser numérico.' />"
                                    ErrorMessage="El monto del anticipo debe ser numérico." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_monto_anticipo_ServerValidate" />
                            </div>
                        </div>
                    </div>
                    <br />
                    <%--VEHICULO OFICIAL--%>
                    <div class="row" id="fila_datos_vehiculo_oficial" style="display: none">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    Datos vehiculo oficial
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">Dominio</span>
                                                <asp:TextBox runat="server" ID="txt_dominio_vehiculo_oficial" class="form-control" placeholder="Dominio" />

                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">Agrega chofer?</span>
                                                <asp:DropDownList runat="server" ID="ddl_con_chofer" CssClass="form-control" onchange="ActualizarDestinoAnticipo()">
                                                    <asp:ListItem Text="Seleccionar" Value="0" />
                                                    <asp:ListItem Text="Si" Value="1" />
                                                    <asp:ListItem Text="No" Value="2" />
                                                </asp:DropDownList>
                                                <asp:CustomValidator ID="cv_chofer" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar si necesita o no chofer para la comisión.' />"
                                                    ErrorMessage="Debe seleccionar si necesita o no chofer para la comisión." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_chofer_ServerValidate" />
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="selecciona_chofer">
                                            <div class="row">
                                                <div class="col-md-4"></div>
                                                <div class="col-md-8">
                                                    <button type="button" class="btn btn-default" id="btn_chofer" runat="server" data-toggle="modal" data-target="#modal11">
                                                        <asp:Image ImageUrl="~/Imagenes/user_add.png" runat="server" />
                                                        Agregar chofer
                                                    </button>
                                                    <div class="input-group" runat="server" visible="false" id="group_chofer">
                                                        <asp:TextBox runat="server" ReadOnly="true" ID="txt_chofer" />
                                                        <span class="input-group-btn">
                                                            <asp:Button Text="X" ID="btn_del_agente_chofer" OnClientClick="ConfirmarEliminacionChofer()" CssClass="btn btn-danger" OnClick="btn_del_agente_chofer_ServerClick" runat="server" />
                                                        </span>
                                                    </div>
                                                    <asp:CustomValidator ID="cv_chofer_requerido" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Necesita seleccionar el chofer.' />"
                                                        ErrorMessage="Necesita seleccionar el chofer." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_chofer_requerido_ServerValidate" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--VEHICULO PARTICULAR--%>
                    <div class="row" id="fila_datos_vehiculo_particular" style="display: none">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    Datos vehiculo particular
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">Dominio</span>
                                                        <asp:TextBox runat="server" ID="tb_dominio_particular" class="form-control" placeholder="Dominio" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_dominio_particular" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el dominio del vehículo particular.' />"
                                                        ErrorMessage="Debe ingresar el dominio del vehículo particular." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_dominio_particular_ServerValidate" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">Combustible</span>
                                                        <asp:TextBox runat="server" ID="tb_tipo_combustible_particular" class="form-control" placeholder="Nafta/Diesel" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_tipo_combustible_vehiculo_particular" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el tipo de combustible del vehículo particular.' />"
                                                        ErrorMessage="Debe ingresar el tipo de combustible del vehículo particular." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_tipo_combustible_vehiculo_particular_ServerValidate" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">Titular</span>
                                                        <asp:TextBox runat="server" ID="tb_titular" CssClass="form-control" placeholder="Titular" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_titular_vehiculo_particular" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el titular del vehículo particular.' />"
                                                        ErrorMessage="Debe ingresar el titular del vehículo particular." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_titular_vehiculo_particular_ServerValidate" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">Póliza Nro:</span>
                                                        <asp:TextBox runat="server" ID="tb_poliza_nro" CssClass="form-control" placeholder="Nro Póliza" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_poliza_nro" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el número de seguro del vehículo.' />"
                                                        ErrorMessage="Debe ingresar el número de seguro del vehículo." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_poliza_nro_ServerValidate" />
                                                </div>
                                            </div>

                                        </div>

                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">Cobertura:</span>
                                                        <asp:TextBox runat="server" ID="tb_poliza_cobertura" CssClass="form-control" placeholder="Cobertura" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_poliza_cobertura" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el tipo de cobertura de la póliza.' />"
                                                        ErrorMessage="Debe ingresar el tipo de cobertura de la póliza." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_poliza_cobertura_ServerValidate" />
                                                </div>
                                            </div>

                                        </div>

                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-11">
                                                    <asp:HiddenField runat="server" ID="hf_vigencia" />
                                                    <div class="input-group date" id="datetimepicker3">
                                                        <span class="input-group-addon">Vigencia</span>
                                                        <asp:TextBox runat="server" ID="tb_poliza_vigencia" CssClass="form-control" placeholder="Vigencia" />
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:CustomValidator ID="cv_poliza_vigencia" runat="server" Style="margin-left: 5px; position: absolute;" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la vigencia del seguro del vehículo.' />"
                                                        ErrorMessage="Debe ingresar la vigencia del seguro del vehículo." ForeColor="Red" ValidationGroup="general_214" OnServerValidate="cv_poliza_vigencia_ServerValidate" />
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


            <%--botones confirmar eliminar aprobar, etc--%>
            <div class="row text-right">
                <div class="col-md-12">
                    <asp:Button Text="Confeccionar" CssClass="btn btn-lg btn-primary" OnClick="btn_Confeccionar_Click" runat="server" ID="btn_Confeccionar" />
                    <asp:Button Text="Aprobar" CssClass="btn btn-lg btn-success" OnClick="btn_aprobar_Click" runat="server" ID="btn_aprobar" />
                    <asp:Button Text="Enviar e imprimir" CssClass="btn btn-lg btn-success" OnClick="btn_Enviar_e_imprimir_Click" runat="server" ID="btn_Enviar" />
                    <%--<asp:Button Text="Volver" CssClass="btn btn-lg btn-primary" OnClick="btn_Volver_Click" runat="server" ID="btn_Imprimir" />--%>
                    <asp:Button Text="Anular" CssClass=" btn btn-lg btn-danger" OnClick="btn_Anular_Click" runat="server" ID="btn_Anular" />
                    <asp:Button Text="Cancelar" CssClass=" btn btn-lg btn-default" OnClick="btn_Cancelar_Click" runat="server" ID="btn_Cancelar" />
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente Jefe de comisión--%>
    <div class="modal fade" id="modal1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente Jefe de comisión</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_1.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_1" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 2--%>
    <div class="modal fade" id="modal2" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 2</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_2.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_2" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_2" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 3--%>
    <div class="modal fade" id="modal3" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 3</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_3.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_3" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_3" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 4--%>
    <div class="modal fade" id="modal4" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 4</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_4.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_4" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_4" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 5--%>
    <div class="modal fade" id="modal5" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 5</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_5.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_5" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_5" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 6--%>
    <div class="modal fade" id="modal6" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 6</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_6.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_6" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_6" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 7--%>
    <div class="modal fade" id="modal7" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 7</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_7.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_7" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_7" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 8--%>
    <div class="modal fade" id="modal8" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 8</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_8.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_8" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_8" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 9--%>
    <div class="modal fade" id="modal9" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 9</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_9.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_9" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_9" OnClick="btn_agente_x_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <%--Seleccione agente puesto 10--%>
    <%--<div class="modal fade" id="modal10" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione agente puesto 10</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_10.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_10" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" ID="btn_agente_chofer" OnClick="btn_agente_chofer_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>--%>

    <%--Seleccione agente chofer--%>
    <div class="modal fade" id="modal11" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Seleccione Chofer</h4>
                </div>
                <div class="modal-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_agentes_para_chofer.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_agentes_para_chofer" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                <asp:BoundField DataField="Area" HeaderText="Area" />
                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button Text="Seleccionar" runat="server" CommandArgument='<%# Eval("IdAgente") %>' ID="btn_agente_chofer" OnClick="btn_agente_chofer_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">

    <script type="text/javascript">

        var d = new Date;

        $(function () {

            var d = new Date();
            var e = new Date(d.getFullYear(), d.getMonth(), d.getDate());
            var hidden_dtp = document.getElementById('<%=hf_vigencia.ClientID %>');


            $('#datetimepicker1').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY',
                minDate: e

            });

            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY',
                useCurrent: false //Important! See issue #1075
            });
            $("#datetimepicker1").on("dp.change", function (e) {
                $('#datetimepicker2').data("DateTimePicker").minDate(e.date);
            });
            $("#datetimepicker2").on("dp.change", function (e) {
                $('#datetimepicker1').data("DateTimePicker").maxDate(e.date);
            });

            $('#datetimepicker3').datetimepicker({
                locale: 'es',
                format: 'DD[/]MM[/]YYYY',
                minDate: e

            });

            if (hidden_dtp.value == "deshabilitar") {
                $('#datetimepicker3').data("DateTimePicker").disable();
            }

            let hidden_seleccion = document.getElementById('<%=dentro_fuera.ClientID %>');
            let hidden_seleccion_movilidad = document.getElementById('<%=hf_movilidad.ClientID %>');

            SeleccionDentroFuera(hidden_seleccion.value);
            SeleccionMovilidad(hidden_seleccion_movilidad.value);
        });
    </script>
    

    <script>

        //Confirma la eliminación de un agente de nomina del 214 cuando el mismo ya fue confeccionado y el  agente fue aprobado
        //location.reload(true);
        function ConfirmarEliminacion() {
            var encabezado = document.getElementById('<%=lbl_encabezado1214.ClientID%>');
            if (encabezado.textContent != "Nuevo formulario 3168") {
                if (!confirm("Esta a punto de eliminar un agente de la nomina ya confeccionada, esta seguro de continuar?"))
                    event.preventDefault();
            }

        }

        function ConfirmarEliminacionChofer() {
            var encabezado = document.getElementById('<%=lbl_encabezado1214.ClientID%>');
            if (encabezado.textContent != "Nuevo formulario 3168") {
                if (!confirm("Esta a punto de eliminar al chofer de la nomina ya confeccionada, esta seguro de continuar?"))
                    event.preventDefault();
            }

        }

        function RechazarEliminacion() {
            var encabezado = document.getElementById('<%=lbl_encabezado1214.ClientID%>');
            if (encabezado.textContent != "Nuevo formulario 3168") {
                alert("El jefe de comisión designado no puede eliminarce. Deberá anular el formulario y crear uno nuevo.-")
                event.preventDefault();
            }


        }

        function SeleccionDentroFuera(seleccion) {

            let valorSeleccionado = document.getElementById('<%=dentro_fuera.ClientID %>');
            let btn = document.getElementById('btn_action');
            let id_form = document.getElementById('<%=id_formulario.ClientID %>');

            if (seleccion == "0") {
                btn.innerText = 'Seleccionar '
            }

            if (seleccion == "1") {
                btn.innerText = 'Dentro de la provincia '
            }

            if (seleccion == "2") {
                btn.innerText = 'Fuera de la provincia '
            }

            let span = document.createElement('span')
            span.className = 'caret';

            btn.append(span);

            if (id_form.value != "0") {
                btn.setAttribute('disabled', true);
            }

            valorSeleccionado.value = seleccion;

        }

        function SeleccionMovilidad(seleccion) {
            let valorSeleccionado = document.getElementById('<%=hf_movilidad.ClientID %>');

            valorSeleccionado.value = seleccion;

            ActualizarDestinoAnticipo();
        }

        function ActualizarDestinoAnticipo() {

            var id_form = document.getElementById('<%=id_formulario.ClientID %>');

            var btn = document.getElementById('btn_seleccion');

            btn.innerText = 'Seleccionar ';

            var movil = document.getElementById('<%=hf_movilidad.ClientID %>');
            var anticipo = document.getElementById('lbl_monto_anticipo');
            var tb_monto = document.getElementById('<%=tb_monto_anticipo.ClientID%>');

            var fila_datos_vehiculo_oficial = document.getElementById('fila_datos_vehiculo_oficial');
            var fila_datos_vehiculo_particular = document.getElementById('fila_datos_vehiculo_particular');
            var chofer = document.getElementById('<%=ddl_con_chofer.ClientID%>');
            var col_chofer = document.getElementById('selecciona_chofer');

            if (movil.value == "0") {
                anticipo.textContent = "Debe seleccionar el tipo de mobilidad.";
               
                tb_monto.style = 'display: none';
                fila_datos_vehiculo_oficial.style = 'display: none';
                fila_datos_vehiculo_particular.style = 'display: none';
            }

            if (movil.value == "1" || movil.value == "2") {
                anticipo.textContent = "Gastos vehículo: nafta, otros.";
                tb_monto.style = 'display: normal';

                fila_datos_vehiculo_oficial.style = 'display: none';
                fila_datos_vehiculo_particular.style = 'display: none';

                if (movil.value == "1") {
                    btn.innerText = 'Vehículo oficial '
                    fila_datos_vehiculo_oficial.style = 'display:normal';
                }
                else {
                    btn.innerText = 'Vehículo particular '
                    fila_datos_vehiculo_particular.style = 'display:normal';
                }

                if (chofer.value == "1") {
                    col_chofer.style = 'display: normal';
                }
                else {
                    col_chofer.style = 'display: none';
                }
            }

            if (movil.value == "3") {
                btn.innerText = 'Transporte público '
                anticipo.textContent = "Pasajes";
                tb_monto.disabled = false;
                fila_datos_vehiculo_oficial.style = 'display: none';
                fila_datos_vehiculo_particular.style = 'display: none';
            }

            var span = document.createElement('span')
            span.className = 'caret';

            btn.append(span);

            if (id_form.value != "0") {
                btn.setAttribute('disabled', true);
                tb_monto_anticipo.setAttribute('disabled', true);
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
