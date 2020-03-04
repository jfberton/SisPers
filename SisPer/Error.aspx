﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Error.aspx.cs" Inherits="SisPer.Error" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-danger">
        <div class="panel-heading">
            <h3>Upps... ocurrió un error!</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <asp:Image ImageUrl="~/Imagenes/Error.jpg" Height="300" runat="server" />
                </div>
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-12">
                            <p>Malas noticias, lamentablemente la acción que intentó realizar generó un error; pero de los errores se aprende. <b>Su error fue guardado para poder analizarlo.</b></p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            Intente
                <asp:LinkButton ID="btn_VolverAEmpezar" Text="Volver al inicio " OnClick="btn_VolverAEmpezar_Click" runat="server" />
                            , si el problema persiste, comuniquese con el área de sistemas.
                        </div>
                    </div>
                   <%-- <div class="row">
                        <div class="col-md-12">
                            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-warning">
                                    <div class="panel-heading" role="tab" id="headingError">
                                        <h1 class="panel-title">
                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseError" aria-expanded="false" aria-controls="collapseError">Detalle del error
                                                </a>
                                        </h1>
                                                <span class="caret" />
                                            <h1 class="panel-title">
                                                &nbsp;</h1>
                                    </div>
                                    <div id="collapseError" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                        <div class="panel-body">
                                           
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Message</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_Message" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Source</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_Source" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Instance</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_Instance" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Data</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_Data" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>URL</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_URL" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Target Site</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_TargetSite" runat="server" />
                                                </div>
                                            </div> 
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Stack Trace</label>
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:Label Text="" ID="lbl_StackTrace" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</span>
</asp:Content>
