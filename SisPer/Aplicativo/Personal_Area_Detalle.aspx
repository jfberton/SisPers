<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Area_Detalle.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Area_Detalle" %>

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
            <h3 class="panel-title">Detalle de la unidad organizativa "<asp:Label ID="lbl_NombreUnidadOrg" runat="server"></asp:Label>"</h3>
        </div>
        <div class="panel-body">
            <div class="alert alert-info">
                <h4 class="panel-title">Depende de 
                    <asp:Label ID="lbl_DependeDe" runat="server"></asp:Label></h4>
            </div>
            <div class="alert alert-warning" runat="server" id="alertInterior">
                <h4><asp:Label ID="lbl_InteriorExterior" runat="server"/></h4>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <h4>Agentes
                        <asp:Label ID="lbl_CantAgentes" CssClass="badge" runat="server"></asp:Label></h4>
                    <asp:GridView ID="GridViewAgentes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewAgente_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Jefe" HeaderText="Jefe" ReadOnly="true" SortExpression="Jefe" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre Agente" ReadOnly="true" SortExpression="Nombre" />
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-6">
                    <h4>Unidades organizativas subordinadas
                        <asp:Label ID="lbl_CantidadSubordinadas" CssClass="badge" runat="server"></asp:Label></h4>
                    <asp:GridView ID="GridViewAreas" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewArea_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre Unidad Organizativa" ReadOnly="true"
                                SortExpression="Nombre" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
