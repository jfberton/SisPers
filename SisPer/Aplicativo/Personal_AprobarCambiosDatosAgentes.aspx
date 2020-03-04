<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_AprobarCambiosDatosAgentes.aspx.cs" Inherits="SisPer.Aplicativo.Personal_AprobarCambiosDatosAgentes" %>


<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc1:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">Cambios solicitados</h1>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title">Datos particulares</h1>
                                </div>
                                <div class=" panel-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Apellido y nombre
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tb_ApyNom" runat="server" CssClass="form-control" Enabled="False" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    CUIT/CUIL
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tb_CUIL" runat="server" CssClass="form-control" Enabled="False" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row" style="visibility:hidden;">
                                                <div class="col-md-3">
                                                    DNI
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tb_DNI" runat="server" CssClass="form-control" Enabled="False" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Fecha de nacimiento
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tb_FechaNacimiento" CssClass="form-control" runat="server" Enabled="False" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    Ficha médica
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox runat="server" ID="tb_FichaMedica" CssClass="form-control" Enabled="False" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row" style="visibility:hidden;">
                                                <div class="col-md-3">
                                                    E-Mail
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="tb_Email" runat="server" CssClass="form-control" Enabled="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title">Foto carnet</h1>
                                </div>
                                <div class=" panel-body">
                                    <div class="thumbnail">
                                        <asp:Image ID="img_cuenta" ImageUrl="" BackColor="GrayText" Width="150" Height="150"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h1 class="panel-title">Datos domicilio</h1>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-1">Dirección</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="tb_direccion" Enabled="False" CssClass="form-control" />
                                </div>
                                <div class="col-md-1">Localidad</div>
                                <div class="col-md-5">
                                    <asp:TextBox runat="server" ID="tb_localidad" Enabled="False" CssClass="form-control" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-1">Aclaraciones</div>
                                <div class="col-md-11">
                                    <asp:TextBox runat="server" ID="tb_aclaracion_domicilio" Enabled="False" CssClass="form-control" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="col-md-12">
                <asp:Button Text="Aceptar cambios" ID="btn_Aceptar" runat="server" CssClass="btn btn-lg btn-success"
                    OnClick="btn_Aceptar_Click" />
                <asp:Button Text="Rechazar cambios" ID="btn_Rechazar" runat="server" CssClass="btn btn-lg btn-danger"
                    OnClick="btn_Rechazar_Click" />
                <asp:Button Text="Volver" ID="btn_Volver" runat="server" CssClass="btn btn-lg btn-default"
                    OnClick="btn_Volver_Click" />
            </div>
        </div>
    </div>


</asp:Content>
