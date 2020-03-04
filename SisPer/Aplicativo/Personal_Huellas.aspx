<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Huellas.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Huellas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc1:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-12">
                    <h3 class="panel-title">Marcaciones de huellas descargadas</h3>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Seleccione la fecha buscada</h3>
                                </div>
                                <div class="panel-body">

                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:RadioButton Text="Diario" ID="rb_Diadio" runat="server" OnCheckedChanged="rb_Diadio_CheckedChanged" AutoPostBack="true" GroupName="Huellas" Checked="true" /></td>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RadioButton Text="Mensual" ID="rb_Mensual" runat="server" OnCheckedChanged="rb_Diadio_CheckedChanged" AutoPostBack="true" GroupName="Huellas" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel runat="server" ID="PanelDiario" Visible="true">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <br />
                                                        <asp:TextBox ID="Calendar1" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Calendar1_CalendarExtender" CssClass="alert alert-info" runat="server" Enabled="True" Format="D" TargetControlID="Calendar1">
                                                        </asp:CalendarExtender>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel runat="server" ID="PanelMensual" Visible="false">
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td><label>Mes</label></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddl_Mes" CssClass="form-control">
                                                                <asp:ListItem Value="1" Text="Enero" />
                                                                <asp:ListItem Value="2" Text="Febrero" />
                                                                <asp:ListItem Value="3" Text="Marzo" />
                                                                <asp:ListItem Value="4" Text="Abril" />
                                                                <asp:ListItem Value="5" Text="Mayo" />
                                                                <asp:ListItem Value="6" Text="Junio" />
                                                                <asp:ListItem Value="7" Text="Julio" />
                                                                <asp:ListItem Value="8" Text="Agosto" />
                                                                <asp:ListItem Value="9" Text="Septiembre" />
                                                                <asp:ListItem Value="10" Text="Octubre" />
                                                                <asp:ListItem Value="11" Text="Noviembre" />
                                                                <asp:ListItem Value="12" Text="Diciembre" />
                                                            </asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddl_Anio" CssClass="form-control">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <div class="input-group">
                                                <input type="text" runat="server" id="tb_Legajo" class="form-control" placeholder="ingrese legajo a buscar o vacío para obtener todos" />
                                                <span class="input-group-btn">
                                                    <button type="button" runat="server" id="btn_buscarAgente" onserverclick="Buscar_Click" class="btn btn-primary">
                                                        <span class="glyphicon glyphicon-search" />
                                                    </button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <table>
                        <tr>
                            <td>
                                <asp:Label Text="" ID="lbl_tituloGrilla" runat="server" />
                                <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" PageSize="20" AllowPaging="true" OnPageIndexChanging="gridView_PageIndexChanging"
                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr table-condensed"
                                    AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ReadOnly="true" SortExpression="Fecha" />
                                        <asp:BoundField DataField="Hora" HeaderText="Hora" ReadOnly="true" SortExpression="Hora" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <asp:Button Text="Generar txt" CssClass="btn btn-success" runat="server" ID="btn_GenTXT"
                                    OnClick="btn_GenTXT_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
