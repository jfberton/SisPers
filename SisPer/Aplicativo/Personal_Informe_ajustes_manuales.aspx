<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_ajustes_manuales.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_ajustes_manuales" %>

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
            <h3 class="panel-title">Informe de ajustes de horas manuales</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-1">
                            <label>Mes</label></div>
                        <div class="col-md-2">
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
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <label>Año</label></div>
                        <div class="col-md-2">
                            <asp:DropDownList runat="server" ID="ddl_Anio" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:Button Text="Buscar" runat="server" ID="btn_buscar" CssClass="btn btn-primary" OnClick="btn_buscar_Click" /></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
