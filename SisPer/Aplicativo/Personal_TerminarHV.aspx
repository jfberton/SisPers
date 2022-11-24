<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_TerminarHV.aspx.cs" Inherits="SisPer.Aplicativo.Personal_TerminarHV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagPrefix="uc3" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc1:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc3:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Administrar horario vespertino aprobado</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Datos solicitud</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-3"><b>Agente</b></div>
                                        <div class="col-md-9">
                                            <asp:Label Text="" ID="lbl_Agente" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-3"><b>Día</b></div>
                                        <div class="col-md-9">
                                            <asp:Label Text="" ID="lbl_Dia" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-3"><b>Hora desde</b></div>
                                        <div class="col-md-3">
                                            <asp:Label Text="" ID="lbl_HoraDesde" runat="server" />
                                        </div>

                                        <div class="col-md-3"><b>Hora hasta</b></div>
                                        <div class="col-md-3">
                                            <asp:Label Text="" ID="lbl_HoraHasta" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-3"><b>Motivo</b></div>
                                        <div class="col-md-9">
                                            <asp:Label Text="" ID="lbl_Descripcion" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-7">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Administrar Entrada - Salida </h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    Entrada
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:DropDownList runat="server" ID="ddl_tipo_marcacion_desde" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddl_tipo_marcacion_desde_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-5">
                                                                    <asp:DropDownList runat="server" ID="ddl_marcaciones_desde" CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                    <asp:Button Text="Registrar entrada" runat="server" CssClass="btn btn-small btn-default" ID="btn_registrar_entrada_hv" OnClick="btn_registrar_entrada_hv_Click" />
                                                                    <asp:TextBox runat="server" ID="tb_entrada_manual" CssClass="form-control" />

                                                                    <asp:CustomValidator
                                                                        ID="CustomFieldValidator3" runat="server" ValidationGroup="HVdesde"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora inicial' />"
                                                                        ErrorMessage="Debe ingresar la hora inicial" OnServerValidate="CustomFieldValidator3_ServerValidate" />
                                                                    <asp:CustomValidator
                                                                        ID="CustomValidator6" runat="server" ValidationGroup="HVdesde"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                                        ErrorMessage="La hora ingresada no es correcta xx:xx" OnServerValidate="CustomValidator6_ServerValidate" />
                                                                </div>
                                                                <div class="col-md-7">
                                                                    <asp:Button Text="Usar marcación" CssClass="btn btn-default" ID="btn_usar_marcacion_entrada" runat="server" OnClick="btn_usar_marcacion_entrada_Click" />
                                                                    <asp:Button Text="Registrar marcación" CssClass="btn btn-default" ID="btn_registrar_marcacion_entrada" runat="server" OnClick="btn_registrar_marcacion_entrada_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <p><asp:Label Text="" ID="leyenda_marcaciones_faciales_entrada" runat="server" /></p>
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
                                                    Salida
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:DropDownList runat="server" ID="ddl_tipo_marcacion_hasta" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddl_tipo_marcacion_hasta_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-5">
                                                                    <asp:DropDownList runat="server" ID="ddl_marcaciones_hasta" CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                    <asp:Button Text="Registrar salida" runat="server" CssClass="btn btn-small btn-default" ID="btn_registrar_salida_hv" OnClick="btn_registrar_salida_hv_Click" />
                                                                    <asp:TextBox runat="server" ID="tb_salida_manual" CssClass="form-control" />


                                                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ValidationGroup="HVhasta"
                                                                        ControlToValidate="tb_salida_manual" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora final' />"
                                                                        ErrorMessage="Debe ingresar la hora final" OnServerValidate="CustomValidator5_ServerValidate">
                                                                    </asp:CustomValidator>
                                                                    <asp:CustomValidator
                                                                        ID="CustomValidator1" runat="server" ValidationGroup="HVhasta"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                                        ErrorMessage="La hora ingresada no es correcta xx:xx" OnServerValidate="CustomValidator7_ServerValidate" />

                                                                </div>
                                                                <div class="col-md-7">
                                                                    <asp:Button Text="Usar marcación" CssClass="btn btn-default" ID="btn_usar_marcacion_salida" runat="server" OnClick="btn_usar_marcacion_salida_Click" />
                                                                    <asp:Button Text="Registrar marcación" CssClass="btn btn-default" ID="btn_registrar_marcacion_salida" runat="server" OnClick="btn_registrar_marcacion_salida_Click" />

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <p><asp:Label Text="" ID="leyenda_marcaciones_faciales_salida" runat="server" /></p>
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
                                                    Resumen
                                                    <asp:Label Text="" ID="lbl_resumen_horas_a_aplicar" runat="server" />
                                                    <asp:CustomValidator ID="CustomValidator4" ValidationGroup="HVHastaGeneral" Text="<img src='../Imagenes/exclamation.gif' title='La hora final debe ser mayor o igual a la hora inicial' />"
                                                        runat="server" ErrorMessage="La hora final debe ser mayor o igual a la hora inicial"
                                                        OnServerValidate="CustomValidator4_ServerValidate">
                                                    </asp:CustomValidator>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <blockquote class="blockquote">
                                                                <p class="mb-0">Ingreso: <asp:Label Text="Sin registrar" runat="server" ID="lbl_ingreso_registrado" /></p>
                                                                <p class="mb-0"><small><asp:Label Text="" ID="lbl_impacto_hv_entrada" runat="server" /></small></p>
                                                            </blockquote>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <blockquote class="blockquote">
                                                                <p class="mb-0">Egreso: <asp:Label Text="Sin registrar" runat="server" ID="lbl_egreso_registrado" /></p>
                                                                <p class="mb-0"><small><asp:Label Text="" ID="lbl_impacto_hv_salida" runat="server" /></small></p>
                                                            </blockquote>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12 text-right">
                                                            <asp:Button ID="btn_Terminar" runat="server" Text="Terminar" OnClick="btn_Terminar_Click" CssClass="btn btn-success" />
                                                            <asp:Button ID="btn_Rechazar" runat="server" Text="Rechazar" OnClick="btn_Rechazar_Click" CssClass="btn btn-danger" />
                                                            <asp:Button ID="btn_Volver" runat="server" Text="Volver" OnClick="btn_Volver_Click" CssClass="btn btn-default" />
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
    </div>
</asp:Content>
