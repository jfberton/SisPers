<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_ReasignarAgentes.aspx.cs" Inherits="SisPer.Aplicativo.Personal_ReasignarAgentes" %>

<%@ Register Src="Controles/Ddl_Areas.ascx" TagName="Ddl_Areas" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc3:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Reasignación de agentes a unidades organizativas</h3>
        </div>
        <div class="panel-body">
            &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                CssClass="validationsummary panel panel-danger" ValidationGroup="OrigenDestino" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
            <p />

            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Origen</h4>
                        </div>
                        <div class="panel-body">
                            <div class="row panel">
                                <div class="col-md-2">
                                    <label>Área</label>
                                </div>
                                <div class="col-md-10">
                                    <uc1:Ddl_Areas ID="Ddl_AreasOrigen" runat="server" SetearTextoItemNullo="Agentes sin asignar"
                                        OnSeleccionoOtroItem="OtroItemOrigen" />
                                </div>
                            </div>
                            <div class="row panel">
                                <div class="col-md-2">
                                    <label>Filtro</label>
                                </div>
                                <div class="col-md-10">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="tb_Busqueda" CssClass="form-control" />
                                        <span class="input-group-btn">
                                            <button type="button" runat="server" onserverclick="btn_Buscar_Click" class="btn btn-primary">
                                                <span class="glyphicon glyphicon-search"></span>
                                            </button>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row panel">
                                <div class="col-md-2"></div>
                                <div class="col-md-10">
                                    <asp:GridView ID="GridViewOrigen" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewOrigen_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre y Apellido" ReadOnly="true"
                                                SortExpression="Nombre" />
                                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                            <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="true" SortExpression="Area" />
                                            <asp:TemplateField HeaderText="Reasignar" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_Reasignar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                        ToolTip="Reasignar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_Reasignar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">Destino</h4>
                        </div>
                        <div class="panel-body">
                            <div class="row panel">
                                <div class="col-md-2">
                                    <label>Área</label>
                                </div>
                                <div class="col-md-10">
                                    <uc1:Ddl_Areas ID="Ddl_AreasDestino" runat="server" SetearTextoItemNullo="Ninguna unidad organizativa"
                                        OnSeleccionoOtroItem="OtroItemDestino" />
                                    <asp:CustomValidator ID="AreasIguales" runat="server" ValidationGroup="OrigenDestino"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La unidad organizativa destino debe ser distinta a la unidad organizativa origen' />"
                                        ErrorMessage="La unidad organizativa destino debe ser distinta a la unidad organizativa origen"
                                        ForeColor="Red" OnServerValidate="AreasOrigenYDestinoIguales_ServerValidate" />
                                </div>
                            </div>

                            <div class="row panel">
                                <div class="col-md-2"></div>
                                <div class="col-md-10">
                                    <asp:GridView ID="GridViewDestino" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewDestino_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre y Apellido" ReadOnly="true"
                                                SortExpression="Nombre" />
                                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
