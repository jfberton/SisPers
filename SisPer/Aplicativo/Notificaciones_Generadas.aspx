<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Notificaciones_Generadas.aspx.cs" Inherits="SisPer.Aplicativo.Notificaciones_Generadas" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <div class="row">
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3>Notificaciones generadas <span class="small">pendientes de responder</span></h3>
                </div>
                <div class="panel-body">
                    <div class="input-group">
                        <span class="input-group-addon">Buscar</span>
                        <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_NotificacionesEnviadas.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                    </div>
                    <div style="height: 400px; overflow-y: scroll;">
                        <asp:GridView ID="gv_NotificacionesEnviadas" runat="server" EmptyDataText="No existen notificaciones para mostrar."
                            AutoGenerateColumns="False" GridLines="None"
                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Número" ReadOnly="true" SortExpression="Id" />
                                <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="Descripcion" />
                                <asp:BoundField DataField="Generada" HeaderText="Generada" ReadOnly="true" SortExpression="Generada" />
                                <asp:BoundField DataField="Notificada" HeaderText="Notificada" ReadOnly="true" SortExpression="Notificada" />
                                <asp:TemplateField HeaderText="Ver">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_VerNotificacionEnviada" runat="server" CommandArgument='<%#Eval("Id")%>' ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_VerNotificacionEnviada_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Button Text="Refrescar" ID="btn_refrescar" CssClass="btn btn-success" OnClick="btn_refrescar_Click" runat="server" />
                    <asp:Button Text="Imprimir listado" ID="btn_Imprimir_Listado" CssClass="btn btn-default" OnClick="btn_Imprimir_Listado_Click" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="panel panel-default" id="p_DescripcionNotifEnviada" runat="server" visible="false">
                <div class="panel-heading">
                    <h3>Descripción de la notificación</h3>
                </div>
                <div class="panel-body">
                    <div runat="server" class="row " id="div_vtoEnviado">
                        <div class="col-md-12">
                            <b>Vence el</b>
                            <asp:Label Text="" ID="lbl_fechaVtoEnviado" runat="server" />
                            <span class="small">
                                <asp:Label Text="" ID="lbl_diasAVencerEnviado" runat="server" /></span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <b>Número</b>
                        </div>
                        <div class="col-me-3">
                            <asp:Label Text="" ID="lbl_NumNotifEnviada" runat="server" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <b>Tipo</b>
                        </div>
                        <div class="col-me-9">
                            <asp:Label Text="" ID="lbl_TipoNotifEnviada" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <b>De</b>
                        </div>
                        <div class="col-me-9">
                            <asp:Label Text="" ID="lbl_DeNotifEnviada" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <b>Para</b>
                        </div>
                        <div class="col-me-9">
                            <asp:Label Text="" ID="lbl_ParaNotifEnviada" runat="server" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <b>Descripción</b>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <textarea class="well-sm" runat="server" style="width: 100%;" rows="3" id="tb_descripcionEnviada" disabled="disabled"></textarea>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button Text="Volver" ID="btn_volver" OnClick="btn_volver_Click" CssClass="btn btn-default" runat="server" />
                            <asp:Button Text="Imprimir" ID="btn_Imprimir_Enviado" OnClick="btn_Imprimir_Enviado_Click" CssClass="btn btn-default" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>
        function filter2(phrase, _id) {
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById(_id);
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }
                table.rows[r].style.display = displayStyle;
            }
        }
    </script>
</asp:Content>
