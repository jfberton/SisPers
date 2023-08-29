<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_Domicilios_Agente.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_Domicilios_Agente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Aplicativo/Controles/Ddl_Areas.ascx" TagPrefix="uc1" TagName="Ddl_Areas" %>

<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc6:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc2:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc1:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Informes cierres mensuales</h2>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Paso 3 - Seleccione el/los agentes</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div runat="server" id="Panel_Legajo">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" GroupName="AgenteGroup" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_CheckedAgenteChanged" ID="rb_Legajo" Text="Obtener para del legajo: " />
                                                    <asp:CustomValidator Text="<img src='../Imagenes/exclamation.gif' title='El legajo debe ser un numero entero sin puntos ni comas.' />"
                                                        ID="cv_Legajo" runat="server" ErrorMessage="El legajo debe ser un numero entero sin puntos ni comas." OnServerValidate="cv_Legajo_ServerValidate"></asp:CustomValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_Legajo" Width="100" CssClass="form-control" />

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div runat="server" id="Panel_Area">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" GroupName="AgenteGroup" Checked="false" ID="rb_Area" AutoPostBack="true" OnCheckedChanged="rb_CheckedAgenteChanged" Text="Obtener para del área: " /></td>
                                                <td>
                                                    <uc1:Ddl_Areas runat="server" ID="Ddl_Areas" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chk_Dependencias" Text="Incluye dependencias" TextAlign="Right" ToolTip="Incluye las areas o departamentos subordinados" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-mg-4">
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button Text="Buscar" ID="btn_Buscar" CssClass="btn btn-primary" OnClick="btn_Buscar_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
