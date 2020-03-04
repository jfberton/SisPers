<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Area_Listado.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Area_Listado" %>

<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
    <uc1:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-6">
                    <h3 class="panel-title">Lista de unidades organizativas</h3>
                </div>
                <div class="col-md-1">
                        <asp:Button ID="btn_Agregar" CssClass="btn btn-success" runat="server" Text="Agregar" OnClick="btn_Agregar_Click" />
                </div>
                <div class="col-md-4">
                        <ul class="nav navbar-nav navbar-right">
                            <li>
                                <div class="input-group">
                                    <input type="text" class="form-control" placeholder="Filtro" runat="server" id="tb_Busqueda" />
                                    <span class="input-group-btn">
                                        <button type="button" runat="server" onserverclick="btn_Buscar_Click" class="btn btn-primary">
                                            <span class="glyphicon glyphicon-search"></span>
                                        </button>
                                    </span>
                                </div>
                            </li>
                        </ul>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridView_PageIndexChanging"
                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="true" SortExpression="Nombre" />
                    <asp:TemplateField HeaderText="Detalle" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Detalle" runat="server" CommandArgument='<%#Eval("Id")%>'
                                ToolTip="Detalle" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_Detalle_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Editar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                ToolTip="Editar" ImageUrl="~/Imagenes/pencil.png" OnClick="btn_Editar_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Eliminar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                ImageUrl="~/Imagenes/delete.png" ToolTip="Eliminar" OnClick="btn_Eliminar_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
