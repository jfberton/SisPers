<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Area_Nuevo.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Area_Nuevo" %>

<%@ Register Src="Controles/Ddl_Areas.ascx" TagName="Ddl_Areas" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc3:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
    <uc1:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Datos de unidad organizativa</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Depende de</label>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar el area de la que dependa' />"
                        ErrorMessage="Debe seleccionar el area de la que dependa" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                </div>
                <div class="col-md-8">
                    <uc2:Ddl_Areas ID="Ddl_Areas1" runat="server" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label>Nombre</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Nombre"
                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el nombre del area' />"
                        ErrorMessage="Debe ingresar el nombre del area"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-8">
                    <asp:TextBox ID="tb_Nombre" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:CheckBox ID="chk_Interior" Text="&nbsp; Se encuentra fuera del edificio central" runat="server" CssClass="form-control"></asp:CheckBox>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btn_Guardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="btn_Guardar_Click" />
                    <asp:Button ID="btn_Cancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btn_Cancelar_Click" CausesValidation="false" />
                </div>
            </div>
            <br />
        </div>
    </div>

</asp:Content>
