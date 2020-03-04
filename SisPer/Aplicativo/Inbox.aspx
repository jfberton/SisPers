<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Inbox.aspx.cs" Inherits="SisPer.Aplicativo.Inbox" ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-8">
                            <h4>Mensajeria</h4>
                        </div>
                        <div class="col-md-4">
                            <asp:Button Text="Nuevo mensaje" ID="btn_NuevoMensaje" CssClass="btn btn-primary" OnClick="btn_NuevoMensaje_Click" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div role="tabpanel" id="tabPanel">
                                <!-- Nav tabs -->
                                <ul class="nav nav-pills" role="tablist">
                                    <li role="presentation" runat="server" class="active" id="tabBandejaEntrada"><a runat="server" id="Recibidos" onserverclick="Recibidos_ServerClick">RECIBIDOS</a></li>
                                    <li role="presentation" runat="server" id="tabBandejaEnviados"><a id="Enviados" runat="server" onserverclick="Enviados_ServerClick">ENVIADOS</a></li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content">
                                    <div role="tabpanel" class="tab-pane active" runat="server" id="bandejaEntradaTab">
                                        <div class="panel panel-primary">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="input-group navbar-btn">
                                                            <input type="text" runat="server" id="tb_Buscar_Recibidos" onkeypress="BuscarRecibidos(event)" class="form-control" placeholder="De:">
                                                            <span class="input-group-btn">
                                                                <button type="button" runat="server" id="btn_Buscar_Recibidos" onserverclick="btn_Buscar_Recibidos_ServerClick" class="btn btn-primary">
                                                                    <span class="glyphicon glyphicon-search"></span>
                                                                </button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div runat="server" id="AccordionRecibidos" style="height: 600px; overflow-y: scroll;">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div role="tabpanel" class="tab-pane" runat="server" id="bandejaSalidaTab">
                                        <div class="panel panel-primary">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div role="tabpanel" class="tab-pane active" runat="server" id="Div1">
                                                            <div runat="server" id="accordionEnviados" style="height: 600px; overflow-y: scroll;">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8">

                            <h5>Mensaje</h5>

                            <CKEditor:CKEditorControl ID="CuerpoMensaje" runat="server" BasePath="~/ckeditor" ToolbarCanCollapse="true" ToolbarStartupExpanded="false" ReadOnly="true" Height="500">
                    
                            </CKEditor:CKEditorControl>
                            <button runat="server" onserverclick="btn_responder_Click" id="btn_responder" class="btn btn-success" visible="false">
                                <span class="glyphicon glyphicon-share-alt"></span>
                                Responder
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script type="text/javascript">

        function BuscarRecibidos(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13) {
                var btn = document.getElementById('<%=btn_Buscar_Recibidos.ClientID%>');
                btn.click();
                evt.preventDefault();
            }
        }

        //script que es disparado por el label del mensaje (control de usuario) lo coloqué aca porque si lo coloco en el control se va a repetir varias veces
        function click(mensaje, destino) {
            var div = document.getElementById().value;
            var contenidoCuerpo = document.getElementById().value;
            document.getElementById(destino).innerHTML = mensaje;
        }
    </script>
</asp:Content>
