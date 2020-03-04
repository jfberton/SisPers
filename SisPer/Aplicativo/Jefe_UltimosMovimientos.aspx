<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Jefe_UltimosMovimientos.aspx.cs" Inherits="SisPer.Aplicativo.Jefe_UltimosMovimientos" %>

<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc1" %>

<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc1:MenuJefe ID="MenuJefe1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Últimos movimientos del personal a cargo</h2>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Salidas del día</h3>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="GridViewSalidas" runat="server" EmptyDataText="No existen salidas diarias por mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewSalidas_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="MarcoJefe" HeaderText="Marco el Jefe" ReadOnly="true"
                                        SortExpression="MarcoJefe" />
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button runat="server" ID="btn_ListadoSalidasDiarias" CssClass="btn btn-primary" Text="Imprimir listado"
                                OnClick="btn_ListadoSalidasDiarias_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Movimientos por venir <small>Feriados, francos compensatorios, horarios vespertinos, licencias</small></h3>
                        </div>
                        <div class="panel-body">
                            
                                <asp:GridView ID="GvDetalleMov" runat="server" EmptyDataText="No existen movimientos por mostrar en el día seleccionado." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GVDetalleMov_PageIndexChanging"
                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" SortExpression="Fecha" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                    </Columns>
                                </asp:GridView>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
