<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_InformeMovimientosAnuales.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_InformeMovimientosAnuales" %>

<%@ Register Src="~/Aplicativo/Controles/Ddl_Areas.ascx" TagPrefix="uc1" TagName="Ddl_Areas" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Informe de movimientos anuales</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-6">
                            <label>Año</label>
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList runat="server" ID="ddl_anio" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div runat="server" id="Panel_Legajo">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:RadioButton runat="server" GroupName="AgenteGroup" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_CheckedAgenteChanged" ID="rb_Legajo" Text="Obtener para del legajo: " />
                                <asp:CustomValidator ControlToValidate="tb_legajo" Text="<img src='../Imagenes/exclamation.gif' title='El legajo debe ser un numero entero sin puntos ni comas.' />"
                                    ID="cv_Legajo" runat="server" ErrorMessage="El legajo debe ser un numero entero sin puntos ni comas." OnServerValidate="cv_Legajo_ServerValidate"></asp:CustomValidator>
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox runat="server" ID="tb_Legajo" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div runat="server" id="Panel_Area">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:RadioButton runat="server" GroupName="AgenteGroup" Checked="false" ID="rb_Area" AutoPostBack="true" OnCheckedChanged="rb_CheckedAgenteChanged" Text="Obtener para del área: " /></div>
                            <div class="col-md-4">
                                <uc1:Ddl_Areas runat="server" ID="Ddl_Areas" />
                            </div>
                            <div class="col-md-4">
                                <asp:CheckBox ID="chk_Dependencias" CssClass="form-control" Text="&nbsp; Incluye dependencias" TextAlign="Right" ToolTip="Incluye las areas o departamentos subordinados" runat="server" /></div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Button Text="Generar informe movimientos anuales" CssClass="btn btn-primary" ID="btn_movimientosAnuales" runat="server" OnClick="btn_movimientosAnuales_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
