<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Usr_CambiarClave.aspx.cs" Inherits="SisPer.Aplicativo.Usr_CambiarClave" %>

<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc3" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc3:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc2:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cambiar clave</h2>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />



    <div style="width: 350px;">
        <form class="form-signin">
            <input type="password" id="tb_Clave" placeholder="Ingrese nueva clave" runat="server" class="form-control" required autofocus><asp:CustomValidator ID="cv_ClavesCorrectas" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='La clave debe tener como mínimo 6 caracteres' />" ErrorMessage="La clave debe tener como mínimo 6 caracteres"
                OnServerValidate="cv_ClavesCorrectas_ServerValidate"></asp:CustomValidator>

            <input type="password" id="tb_ConfirmaClave" placeholder="Confirmar repitiendo la nueva clave" runat="server" class="form-control" required>
            <asp:CustomValidator ID="cv_ClavesIguales" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Las claves no coinciden' />" ErrorMessage="Las claves no coinciden"
                OnServerValidate="cv_ClavesIguales_ServerValidate"></asp:CustomValidator>

            <asp:Button Text="Cambiar" ID="btn_Cambiar" class="btn btn-lg btn-primary btn-block" OnClick="btn_Cambiar_Click" runat="server" />

            <img src='../Imagenes/exclamation.gif' title='Las claves no coinciden' />

            <div runat="server" id="mensaje" class="alert alert-success" role="alert">La clave fue modificada exitosamente.</div>
        </form>
    </div>
</asp:Content>
