<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_SalidasVarias.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_SalidasVarias" %>

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
            <h2 class="panel-title">Informes varios</h2>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Paso 1 - Seleccione el tipo de informe</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:DropDownList runat="server" ID="ddl_TiposDeInforme" CssClass="form-control">
                                        <asp:ListItem Text="Salidas diarias" />
                                        <asp:ListItem Text="Horarios vespertinos" />
                                    </asp:DropDownList>
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
                            <h3 class="panel-title">Paso 2 - Seleccione rango de fechas</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div runat="server" id="Panel_dia">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" GroupName="FechaGroup" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_CheckedFechaChanged" ID="rb_Dia" Text="Obtener para del día: " />
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='El campo día es obligatorio' />"
                                                        ErrorMessage="El campo día es obligatorio" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator></td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_dia" CssClass="form-control" Width="100" />
                                                    <asp:CalendarExtender ID="tb_Nacimiento_CalendarExtender" runat="server" CssClass="alert alert-info"
                                                        Enabled="True" TargetControlID="tb_dia" Format="d"></asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div runat="server" id="Panel_Mes">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" GroupName="FechaGroup" Checked="false" ID="rb_Mes" AutoPostBack="true" OnCheckedChanged="rb_CheckedFechaChanged" Text="Obtener para del mes: " /></td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddl_Mes" CssClass="form-control">
                                                        <asp:ListItem Text="Enero" Value="01" />
                                                        <asp:ListItem Text="Febrero" Value="02" />
                                                        <asp:ListItem Text="Marzo" Value="03" />
                                                        <asp:ListItem Text="Abril" Value="04" />
                                                        <asp:ListItem Text="Mayo" Value="05" />
                                                        <asp:ListItem Text="Junio" Value="06" />
                                                        <asp:ListItem Text="Julio" Value="07" />
                                                        <asp:ListItem Text="Agosto" Value="08" />
                                                        <asp:ListItem Text="Septiembre" Value="09" />
                                                        <asp:ListItem Text="Octubre" Value="10" />
                                                        <asp:ListItem Text="Noviembre" Value="11" />
                                                        <asp:ListItem Text="Diciembre" Value="12" />
                                                    </asp:DropDownList></td>
                                                <td>Año</td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_Anio">
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div runat="server" id="Panel_DesdeHasta">
                                        <table class="table-condensed">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton runat="server" GroupName="FechaGroup" Checked="false" ID="rb_DesdeHasta" AutoPostBack="true" OnCheckedChanged="rb_CheckedFechaChanged" Text="Obtener entre las fechas: " /></td>
                                                <td>Desde
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_Desde" CssClass="form-control" Width="100" />
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="alert alert-info"
                                                        Enabled="True" TargetControlID="tb_Desde" Format="d"></asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Hasta</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tb_Hasta" CssClass="form-control" Width="100" />
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="alert alert-info"
                                                        Enabled="True" TargetControlID="tb_Hasta" Format="d"></asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
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


    <br />
    <br />
</asp:Content>
