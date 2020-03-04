<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="ag_dias_mes.aspx.cs" Inherits="SisPer.Aplicativo.ag_dias_mes" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>
<%@ Register Src="~/Aplicativo/Controles/DatosAgente.ascx" TagPrefix="uc1" TagName="DatosAgente" %>
<%@ Register Src="~/Aplicativo/Controles/VisualizarDiaAgente.ascx" TagPrefix="uc1" TagName="VisualizarDiaAgente" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Asistencia mensual</h3>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Agente</label>
                        </div>
                        <div class="col-md-9">
                            <asp:DropDownList runat="server" ID="ddl_agente" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-md-2">
                            <label>Mes</label>
                        </div>
                        <div class="col-md-5">
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
                        <div class="col-md-5">
                            <asp:DropDownList runat="server" ID="ddl_Anio" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:Button Text="Buscar" runat="server" ID="btn_buscar" CssClass="btn btn-primary" OnClick="btn_buscar_Click" />
                    <asp:Button Text="Nueva busqueda" ID="btn_NuevaBusqueda" CssClass="btn btn-danger" OnClick="btn_NuevaBusqueda_Click" Visible="false" runat="server" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Panel runat="server" ID="panel_totales_HorarioFlexible" Visible="false">
                        <b>
                            <asp:Label Text="Horas acumuladas del mes (sobre los dias cerrados en proceso automático)" runat="server" />
                            <div class="tooltip"></div>
                        </b>
                        <table>
                            <tr>
                                <td>Horas acumuladas año anterior:</td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horasAcumuladasAAnt" runat="server" /></td>
                                <td>&nbsp;Horas acumuladas año actual:</td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horasAcumuladasAAct" runat="server" /></td>
                                <td>&nbsp;Horas adeudadas bonificación:</td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horasAdeudadasBonificacion" runat="server" /></td>
                                <td>&nbsp;Horas acumuladas mes:</td>
                                <td>
                                    <asp:Label Text="" ID="lbl_horasAcumuladasMes" runat="server" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
            <br />
            <asp:Panel runat="server" ID="panelResultadoBusqueda" Visible="false">
                <div class="row">
                    <div class="col-md-4">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h2 class="panel-title">Días mes</h2>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="gv_movimientosMes" runat="server" ForeColor="#717171"
                                            EmptyDataText="Sin registros"
                                            AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                            OnPageIndexChanging="gv_movimientosMes_PageIndexChanging"
                                            CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            <Columns>
                                                <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                                <asp:TemplateField HeaderText="Incon" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgIncon" ImageUrl='<%# Eval("PathWarning")%>' ToolTip='<%#Eval("ObservacionInconsistencia")%>' runat="server" Height="16"
                                                            Width="16" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cerró" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" ImageUrl='<%# Eval("PathImagenCerrado")%>' runat="server" Height="16" ToolTip='<%# Eval("CerradoPor")%>'
                                                            Width="16" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_AnalizarInconsistencia" runat="server" OnClick="btn_AnalizarInconsistencia_Click" CommandArgument='<%#Eval("Dia")%>'
                                                            ToolTip='<%#Eval("ObservacionInconsistencia")%>' ImageUrl="~/Imagenes/bullet_go.png" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div runat="server" id="div_datos_cierre_mes">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h1 class="panel-title">Datos cierre mes de
                                        <asp:Label Text="" ID="lbl_cierre_mes_cerrado" runat="server" /></h1>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-6">Fecha cierre</div>
                                        <div class="col-md-6">
                                            <asp:Label Text="" ID="lbl_cierre_fecha_cierre" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-6">Horas año anterior</div>
                                        <div class="col-md-6">
                                            <asp:Label Text="" ID="lbl_cierre_horas_anio_anterior" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-6">Horas año actual</div>
                                        <div class="col-md-6">
                                            <asp:Label Text="" ID="lbl_cierre_horas_anio_actual" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row" runat="server" id="div_modificaciones">
                                        <div class="col-md-12">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h1 class="panel-title">Modificaciones</h1>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:GridView ID="gv_modificaciones" runat="server" ForeColor="#717171"
                                                                EmptyDataText="Sin registros" AutoGenerateColumns="False" GridLines="None" AllowPaging="false"
                                                                CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                                <Columns>
                                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:d}" />
                                                                    <asp:BoundField DataField="horas_anio_anterior" HeaderText="Horas año anterior" ReadOnly="true" />
                                                                    <asp:BoundField DataField="horas_anio_actual" HeaderText="Horas año actual" ReadOnly="true" />
                                                                    <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" />
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
                        </div>
                        <uc1:VisualizarDiaAgente runat="server" ID="VisualizarDiaAgente" ReadOnly="true"/>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
