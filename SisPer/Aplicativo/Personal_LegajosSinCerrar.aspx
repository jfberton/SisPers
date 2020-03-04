<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_LegajosSinCerrar.aspx.cs" Inherits="SisPer.Aplicativo.Personal_LegajosSinCerrar" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Legajos sin cerrar</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <label>Mes</label></td>
                            <td>
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
                                </asp:DropDownList></td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_Anio" CssClass="form-control">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <label>Legajo desde</label></td>
                            <td>
                                <asp:TextBox ID="tb_legajoDesde" CssClass="form-control" runat="server" /></td>
                            <td>
                                <label>Legajo hasta</label></td>
                            <td>
                                <asp:TextBox ID="tb_legajoHasta" CssClass="form-control" runat="server" /></td>
                            <td>
                                <asp:Button Text="Buscar" runat="server" ID="btn_buscar" OnClick="btn_buscar_Click" CssClass="btn btn-primary" />
                                <asp:Button Text="Nueva busqueda" ID="btn_NuevaBusqueda" OnClick="btn_NuevaBusqueda_Click" Visible="false" runat="server" class="btn btn-danger" /></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Panel runat="server" ID="p_Grilla" Visible="false">
                        <table class="table-condensed">
                            <tr>
                                <td valign="top">
                                    <asp:GridView ID="gv_legajossincerrar" runat="server" ForeColor="#717171"
                                        EmptyDataText="Sin registros"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                        OnPageIndexChanging="gv_legajossincerrar_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                            <asp:BoundField DataField="Nombre" HeaderText="Agente" ReadOnly="true" SortExpression="Nombre" />
                                            <asp:BoundField DataField="DiasPorCerrar" HeaderText="Días por cerrar" ReadOnly="true" SortExpression="DiasPorCerrar" />
                                            <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_verDiasPorCerrar" runat="server" OnClick="btn_verDiasPorCerrar_Click" CommandArgument='<%#Eval("Legajo")%>' ImageUrl="~/Imagenes/bullet_go.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button Text="Exportar a excel" ID="btn_exportar" CssClass="btn btn-success" OnClick="btn_exportar_Click" runat="server" />
                                </td>
                                <td valign="top">
                                    <asp:Panel ID="p_AgenteSeleccionado" Visible="false" runat="server">
                                        <h3>Agente:
                                        <asp:Label Text="" ID="lbl_agente" runat="server" /></h3>
                                        <asp:GridView ID="gv_diasAgente" runat="server" ForeColor="#717171"
                                            EmptyDataText="Sin registros"
                                            AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="NumeroDia" HeaderText="Número" ReadOnly="true" SortExpression="NumeroDia" DataFormatString="{0:D}" />
                                                <asp:BoundField DataField="DiasPorCerrar" HeaderText="Días por cerrar" ReadOnly="true" SortExpression="DiasPorCerrar" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button Text="Volver" ID="btn_volver" OnClick="btn_volver_Click" CssClass="btn btn-default" runat="server" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
