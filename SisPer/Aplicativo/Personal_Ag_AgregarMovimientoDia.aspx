<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Ag_AgregarMovimientoDia.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Ag_AgregarMovimientoDia" %>

<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc1" %>
<%@ Register Src="Controles/Ddl_TipoMovimientoHora.ascx" TagName="Ddl_TipoMovimientoHora"
    TagPrefix="uc2" %>
<%@ Register Src="Controles/Ddl_TipoEstadoAgente.ascx" TagName="Ddl_TipoEstadoAgente"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Agendar estados día</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="input-group">
                        <input type="text" runat="server" id="tb_legajoAgente" class="form-control" placeholder="Legajo" />
                        <span class="input-group-btn">
                            <button type="button" runat="server" id="btn_buscarAgente" onserverclick="btn_buscarAgente_Click" class="btn btn-primary">
                                <span class="glyphicon glyphicon-search" />
                            </button>
                            <button type="button" runat="server" id="btn_NuevaBusqueda" data-toggle="tooltip" data-placement="left" title="Nueva búsqueda" onserverclick="btn_NuevaBusqueda_Click" class="btn btn-danger" visible="false">
                                <span class="glyphicon glyphicon-remove-circle" />
                            </button>
                        </span>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Panel runat="server" ID="p_datosAgente" Width="100%" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <uc1:DatosAgente ID="DatosAgente1" runat="server" />
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Estados" CssClass="validationsummary panel panel-danger" DisplayMode="BulletList" HeaderText="&lt;div class='panel-heading'&gt;&nbsp;Corrija los siguientes errores antes de continuar:&lt;/div&gt;" />
                                <br>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">Datos estado</h4>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label for="Ddl_TipoEstadoAgente1">Estado</label>
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Debe seleccionar un tipo de estado para agregar"
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar un tipo de estado para agregar' />"
                                                    OnServerValidate="CustomValidator3_ServerValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cv_VerificarestadosRango" runat="server" ErrorMessage="El agente posee estados agendados en el rango que intenta agendar"
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='El agente posee estados agendados en el rango que intenta agendar' />"
                                                    OnServerValidate="cv_VerificarestadosRango_ServerValidate"></asp:CustomValidator>
                                            </div>
                                            <div class="col-md-8">
                                                <uc3:Ddl_TipoEstadoAgente ID="Ddl_TipoEstadoAgente1" runat="server" OnSelectedItemChanged="Ddl_TipoEstadoAgente1_SelectedItemChanged" />

                                                <label for="ddl_Anio" runat="server" id="lbl_anio" visible="false">Año</label>
                                                <asp:DropDownList runat="server" ID="ddl_Anio" CssClass="form-control" Visible="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label for="tb_desde">Desde</label>
                                                <asp:CustomValidator ID="cv_VerificarDesde" runat="server" ErrorMessage="Debe ingresar una fecha desde válida"
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar una fecha desde válida' />"
                                                    OnServerValidate="cv_VerificarDesde_ServerValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="El mes al cual quiere agendar el movimiento se encuentra cerrado."
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='El mes al cual quiere agendar el movimiento se encuentra cerrado.' />"
                                                    OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
                                            </div>
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <div id="datetimepicker1" class="input-group date">
                                                        <input id="tb_desde" runat="server" class="form-control" type="text" />
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-4">
                                                <label for="tb_hasta">Hasta</label>
                                                <asp:CustomValidator ID="cv_VerificarHasta" runat="server" ErrorMessage="Debe ingresar una hasta desde válida"
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar una fecha hasta válida' />"
                                                    OnServerValidate="cv_VerificarHasta_ServerValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cv_fechaHasta" runat="server" ErrorMessage="La hasta debe ser igual o posterior a la fecha desde."
                                                    ValidationGroup="Estados" Text="<img src='../Imagenes/exclamation.gif' title='La hasta debe ser igual o posterior a la fecha desde.' />"
                                                    OnServerValidate="cv_fechaHasta_ServerValidate"></asp:CustomValidator>
                                            </div>
                                            <div class="col-md-8">

                                                <div class="form-group">
                                                    <div id="datetimepicker2" class="input-group date">
                                                        <input id="tb_hasta" runat="server" class="form-control" type="text" />
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                        </span>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <asp:Button Text="Agendar" runat="server" ID="btn_Agendar" CssClass="btn btn-success" OnClick="btn_Agendar_Click" />
                                                <asp:Button Text="Cancelar" runat="server" ID="btn_Cancelar" CssClass="btn btn-danger" OnClick="btn_NuevaBusqueda_Click" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">Listado mensual de estados cargados al agente</h4>
                                    </div>
                                    <div class="panel-body">

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="input-group">
                                                            <div id="datetimepicker_month" class="input-group date">
                                                                <asp:TextBox runat="server" ID="tb_mes" CssClass="form-control" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                            <span class="input-group-btn">
                                                                <button class="btn" type="button" runat="server" id="btn_cargar_calenadrio" onserverclick="btn_cargar_calenadrio_ServerClick">
                                                                    Ir!
                                                                </button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999"
                                                            CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                                                            OnVisibleMonthChanged="Calendar1_VisibleMonthChanged"
                                                            ForeColor="Black" Height="180px" Width="200px" OnDayRender="Calendar1_DayRender"
                                                            OnPreRender="Calendar1_PreRender">
                                                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                            <NextPrevStyle VerticalAlign="Bottom" />
                                                            <OtherMonthDayStyle ForeColor="#808080" />
                                                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                            <SelectorStyle BackColor="#CCCCCC" />
                                                            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                                                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                            <WeekendDayStyle BackColor="#FFFFCC" />
                                                        </asp:Calendar>
                                                        <br />
                                                        <b>
                                                            <asp:Label ID="Label1" Text="Referencias" runat="server" /></b><br />
                                                        <asp:Label ID="Label4" Text="Contiene licencias: " runat="server" />
                                                        <asp:Label ID="Label5" Text="XX" runat="server" BackColor="DarkGoldenrod" Font-Bold="True"
                                                            ForeColor="#333333" /><br />
                                                        <asp:Label ID="Label6" Text="Feriados: " runat="server" />
                                                        <asp:Label ID="Label7" Text="XX" runat="server" BackColor="DarkRed" Font-Bold="True"
                                                            ForeColor="Azure" /><br />
                                                        <asp:Label ID="Label2" Text="Adeuda documentación: " runat="server" />
                                                        <asp:Label Text="XX" runat="server" BackColor="OrangeRed" /><br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:GridView ID="GridViewEstados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewEstados_PageIndexChanging"
                                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:BoundField DataField="AgendadoPor" HeaderText="Agendado por" ReadOnly="true"
                                                            SortExpression="AgendadoPor" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                                        <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" DataFormatString="{0:d}"
                                                            SortExpression="Dia" />
                                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_Eliminar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                                    ImageUrl="~/Imagenes/delete.png" OnClick="btn_EliminarEstado_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="GridViewTotalesPorEstado" runat="server" AutoGenerateColumns="False" ForeColor="#717171"
                                                    GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewEstados_PageIndexChanging"
                                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                    <Columns>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                                        <asp:BoundField DataField="Dias" HeaderText="Dias" ReadOnly="true" SortExpression="Dias" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <br />
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
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker_month').datetimepicker({
                locale: 'es',
                format: 'MMMM YYYY'
            });
        });
    </script>

</asp:Content>
