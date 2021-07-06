<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Formulario1214_Generados.aspx.cs" Inherits="SisPer.Aplicativo.Formulario1214_Generados" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


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
    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Formularios 3168 generados</h2>
        </div>
        <div class="panel-body">
            <div class="input-group">
                <span class="input-group-addon" id="basic-addon1">Buscar</span>
                <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_form1214.ClientID %>')" placeholder="ingrese texto buscado" type="text">
            </div>
            <asp:GridView ID="gv_form1214" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Numero" ReadOnly="true" SortExpression="Numero" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                    <asp:BoundField DataField="Generadopor" HeaderText="Confeccionó" ReadOnly="true" SortExpression="Generadopor" />
                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Jefe" HeaderText="Jefe de comisión" ReadOnly="true" SortExpression="Jefe" />
                    <asp:TemplateField HeaderText="Tareas">
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Imagenes/report_user.png" ToolTip='<%#Eval("Tareas") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reimprimir" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_reimprimir" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Reimprimir" ImageUrl="~/Imagenes/page_white_add.png" OnClick="btn_reimprimir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_ver" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Ver" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />
    
    <div class="panel panel-default" runat="server" id="panel_enviados">
        <div class="panel-heading">
            <h2 class="panel-title">Formularios 3168 Enviados por aprobar</h2>
        </div>
        <div class="panel-body">
            <div class="input-group">
                <span class="input-group-addon" id="basic-addon2">Buscar</span>
                <input name="txtTerm" class="form-control" onkeyup="filtrar(this, '<%=gv_form1214.ClientID %>')" placeholder="ingrese texto buscado" type="text">
            </div>
            <asp:GridView ID="gv_Form1214_enviados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Numero" HeaderText="Numero" ReadOnly="true" SortExpression="Numero" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                    <asp:BoundField DataField="Generadopor" HeaderText="Confeccionó" ReadOnly="true" SortExpression="Generadopor" />
                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Jefe" HeaderText="Jefe de comisión" ReadOnly="true" SortExpression="Jefe" />
                    <asp:TemplateField HeaderText="Tareas">
                        <ItemTemplate>
                            <asp:Image ImageUrl="~/Imagenes/report_user.png" ToolTip='<%#Eval("Tareas") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reimprimir" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_reimprimir" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Reimprimir" ImageUrl="~/Imagenes/page_white_add.png" OnClick="btn_reimprimir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_ver" runat="server" CommandArgument='<%#Eval("IdF1214")%>'
                                ToolTip="Ver" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>
        function filtrar(phrase, _id) {
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
