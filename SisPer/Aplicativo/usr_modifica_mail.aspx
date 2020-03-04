<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="usr_modifica_mail.aspx.cs" Inherits="SisPer.Aplicativo.usr_modifica_mail" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Modificar mail</h2>
    &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
    <br />
    <div class=""
    <div class="row">
        <div class="col-md-12">
            <div class="input-group">
                <span class="input-group-addon">Mail actual</span>
                <asp:Label Text="" ID="lbl_mail_actual" runat="server" CssClass="form-control" />
            </div>
        </div>
        <br />
        <br />
        <div class="col-md-12">
            <div class="input-group">
                <span class="input-group-addon">Nuevo correo</span>
                <input type="text" runat="server" id="tb_mail_nuevo" class="form-control" />
                <span class="input-group-btn">
                    <button class="btn btn-primary" type="button" id="btn_enviar_validacion" runat="server" onserverclick="btn_enviar_validacion_ServerClick">Enviar validación!</button>
                </span>
            </div>
        </div>
        <br />
        <asp:RequiredFieldValidator ControlToValidate="tb_mail_nuevo" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar su mail' />"
            ID="rv_mail_vacio" runat="server" ErrorMessage="Debe ingresar su mail">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="rv_mail_correcto" runat="server" ControlToValidate="tb_mail_nuevo"
            Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar un mail válido.' />"
            ErrorMessage="Debe ingresar un mail válido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
        </asp:RegularExpressionValidator>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
