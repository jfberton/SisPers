<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Informe_Bonificaciones.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Informe_Bonificaciones" %>

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
            <h3 class="panel-title">Informe de estado de bonificaciones mensuales</h3>
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
            <div class="row">
                <div class="col-md-7">
                    <h4>
                        <asp:Label Text="Listado de agentes con bonificación" Visible="false" ID="lbl_titulo_grilla" runat="server" /></h4>
                    <asp:GridView ID="gv_bonificaciones" runat="server" ForeColor="#717171"
                        EmptyDataText="Sin registros"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" PageSize="20"
                        OnPageIndexChanging="gv_bonificaciones_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="ApellidoyNombre" HeaderText="Apellido y Nombre" ReadOnly="true" SortExpression="ApellidoyNombre" />
                            <asp:BoundField DataField="HorasOtorgadas" HeaderText="Horas otorgadas" ReadOnly="true" SortExpression="HorasOtorgadas" />
                            <asp:BoundField DataField="HorasPorCumplir" HeaderText="Horas adeudadas" ReadOnly="true" SortExpression="HorasPorCumplir" />
                            <asp:TemplateField HeaderText="Detalle" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_VerDetalle" runat="server" OnClick="btn_VerDetalle_Click" CommandArgument='<%#Eval("Legajo")%>'
                                        ImageUrl="~/Imagenes/bullet_go.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-5">
                    <h4>
                        <asp:Label Text="" ID="lbl_AgenteSeleccionado" runat="server" /></h4>
                    <asp:GridView ID="gv_Detalle" runat="server" ForeColor="#717171"
                        EmptyDataText="Sin registros" Visible="false"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                        </Columns>
                    </asp:GridView>
                    <button type="button" runat="server" id="btn_cerrarDetalle" class="btn btn-danger" visible="false" onserverclick="btn_cerrarDetalle_Click">
                        Ocultar detalle
                        <span class="glyphicon glyphicon-remove-circle" />
                    </button>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button Text="Imprimir reporte general" ID="btn_Imprimir" Visible="false" OnClick="btn_Imprimir_Click" runat="server" CssClass="btn btn-primary" />
                    <asp:Button Text="Imprimir reporte agendes deudores" ID="btn_ImprimirAgentesDeudores" Visible="false" OnClick="btn_ImprimirAgentesDeudores_Click" CssClass="btn btn-warning" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
