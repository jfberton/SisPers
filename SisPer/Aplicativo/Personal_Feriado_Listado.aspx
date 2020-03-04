<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Feriado_Listado.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Feriado_Listado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                <div class="col-md-10">
                    <h3 class="panel-title">Lista de Feriados y asuetos</h3>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btn_Agregar" CssClass="btn btn-success" runat="server" Text="Agregar" OnClick="btn_Agregar_Click" />
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h1 class="panel-title">Feriados</h1>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridView_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" SortExpression="Fecha" />
                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" ReadOnly="true" SortExpression="Motivo" />
                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_Editar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Editar Becado" ImageUrl="~/Imagenes/pencil.png" OnClick="btn_Editar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_Eliminar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ImageUrl="~/Imagenes/delete.png" OnClick="btn_Eliminar_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h1 class="panel-title">Asuetos</h1>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="GridView2" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridView_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" SortExpression="Fecha" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MasDatos")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" ReadOnly="true" SortExpression="Motivo" />
                                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_editar_asueto" runat="server" CommandArgument='<%#Eval("Fecha") + "_" + Eval("MasDatos") + "_" + Eval("Motivo") %>'
                                                ToolTip="Editar Becado" ImageUrl="~/Imagenes/pencil.png" OnClick="btn_editar_asueto_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_eliminar_asueto" runat="server" CommandArgument='<%#Eval("Fecha") + "_" + Eval("MasDatos") + "_" + Eval("Motivo") %>'
                                                ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_asueto_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
