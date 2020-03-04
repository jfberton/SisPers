<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Cerrar_EntradaSalida.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Cerrar_EntradaSalida" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente1" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />

    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Cerrar agendas de horario de ingreso y egreso personal</h2>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-5">
                    <div class="row">
                        <div class="col-md-11">
                            <div class="form-group">
                                <div id="datetimepicker1" class="input-group date">
                                    <input id="tb_dia" runat="server" class="form-control" type="text" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <asp:CustomValidator ID="cv_VerificarFecha" runat="server" ErrorMessage="La fecha ingresada no es válida"
                                Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es válida' />"
                                OnServerValidate="cv_VerificarFecha_ServerValidate"></asp:CustomValidator>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:Button Text="Buscar" CssClass="btn btn-primary" runat="server" ID="btn_Buscar" OnClick="btn_Buscar_Click" />

                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-8">
                                    <h2 class="panel-title">Envíos del día
                        <asp:Label Text="" ID="lbl_fecha_seleccionada" runat="server" /></h2>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button Text="Nueva busqueda" Visible="false" CssClass="btn btn-primary" runat="server" ID="btn_nuevaBusqueda" OnClick="btn_nuevaBusqueda_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-5">

                                    <asp:GridView ID="gv_presentaciones" runat="server" Width="100%" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gv_presentaciones_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Jefe" HeaderText="Jefe" ReadOnly="true" SortExpression="Jefe" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="true" SortExpression="Area" />
                                            <asp:TemplateField HeaderText="Cerrado" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("PathImagenCerrado")%>' runat="server" Height="16" Width="16" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_Ver" runat="server" OnClick="btn_Ver_Click" CommandArgument='<%#Eval("IdJefe")%>' ImageUrl="~/Imagenes/bullet_go.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="col-md-7">
                                    <asp:Panel runat="server" ID="p_marcaciones">
                                        <h3>
                                            <asp:Label Text="" ID="lbl_legajo" runat="server" />
                                            -
                    <asp:Label Text="" ID="lbl_Jefe" runat="server" /></h3>
                                        <asp:GridView ID="gv_marcaciones" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                            AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gv_marcaciones_PageIndexChanging"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre y apellido" ReadOnly="true" SortExpression="Nombre" />
                                                <asp:BoundField DataField="Hentrada" HeaderText="Entrada" ReadOnly="true" SortExpression="Hentrada" />
                                                <asp:BoundField DataField="HSalida" HeaderText="Salida" ReadOnly="true" SortExpression="HSalida" />
                                            </Columns>
                                        </asp:GridView>
                                        <p>
                                            <asp:Button runat="server" ID="btn_Cerrar" CssClass="btn btn-success btn-lg" Text="Aprobar" OnClick="btn_Cerrar_Click" ToolTip="Al aprobar el dia para estas marcaciones, el jefe no podrá modificarlas." />
                                        </p>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-danger">
                <div class="panel-heading" role="tab" id="titulo_sin_enviar">
                    <h1 class="panel-title">
                        <a data-toggle="collapse" href="#collapse_sin_enviar" aria-expanded="false" aria-controls="collapse_sin_enviar">Áreas sin enviar notificaciones en la fecha seleccionada
                        <span class="badge">
                            <asp:Label Text="" ID="lbl_cantidad_areas_sin_enviar" runat="server" /></span>
                            <span class="caret" />
                        </a>
                    </h1>
                </div>
                <div id="collapse_sin_enviar" class="panel-collapse collapse">
                    <div class="panel-body">
                        <asp:GridView ID="gv_sin_enviar" runat="server" Width="100%" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                            AutoGenerateColumns="False" GridLines="None" AllowPaging="false" OnPageIndexChanging="gv_presentaciones_PageIndexChanging"
                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="true" SortExpression="Area" />
                                <asp:BoundField DataField="Jefe" HeaderText="Jefe" ReadOnly="true" SortExpression="Jefe" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="contentScripts">
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });
    </script>
</asp:Content>
