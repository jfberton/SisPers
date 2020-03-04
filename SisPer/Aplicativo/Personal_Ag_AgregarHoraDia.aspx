<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Ag_AgregarHoraDia.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Ag_AgregarHoraDia" %>

<%@ Register Src="Controles/Ddl_TipoMovimientoHora.ascx" TagName="Ddl_TipoMovimientoHora"
    TagPrefix="uc2" %>
<%@ Register Src="Controles/Ddl_TipoEstadoAgente.ascx" TagName="Ddl_TipoEstadoAgente"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc5" %>
<%@ Register Src="~/Aplicativo/Controles/DatosAgente.ascx" TagPrefix="uc2" TagName="DatosAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc4:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Agendar horas día</h3>
        </div>

        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="input-group">
                        <input runat="server" type="text" id="tb_legajoAgente" class="form-control" placeholder="Ingrese el legajo buscado" />
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
                    <asp:Panel runat="server" Width="100%" ID="p_datosAgente" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <uc2:DatosAgente runat="server" ID="DatosAgente1" />
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="MovimientoHora" CssClass="validationsummary panel panel-danger" DisplayMode="BulletList" HeaderText="&lt;div class='panel-heading'&gt;&nbsp;Corrija los siguientes errores antes de continuar:&lt;/div&gt;" />
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">Datos del movimiento</h4>
                                            </div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label for="tb_Fecha">Fecha</label>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="La fecha ingresada no es válida" OnServerValidate="CustomValidator1_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es válida' /&gt;" ValidationGroup="MovimientoHora"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="CustomValidator9" runat="server" ErrorMessage="El mes en el que intenta agendar horas ya se encuentra cerrado." OnServerValidate="CustomValidator9_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='El mes en el que intenta agendar horas ya se encuentra cerrado.' /&gt;" ValidationGroup="MovimientoHora"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="En la fecha seleccionada el agente tiene agendado entre los movimientos que no estará presente en el organismo" OnServerValidate="CustomValidator6_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='En la fecha seleccionada el agente tiene agendado entre los movimientos que no estará presente en el organismo' /&gt;" ValidationGroup="MovimientoHora"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ErrorMessage="En la fecha seleccionada existe un feriado" OnServerValidate="CustomValidator7_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='En la fecha seleccionada existe un feriado' /&gt;" ValidationGroup="MovimientoHora"></asp:CustomValidator>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div class="form-group">
                                                            <div id="datetimepicker1" class="input-group date">
                                                                <input id="tb_Fecha" runat="server" class="form-control" type="text" />
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label for="Ddl_TipoMovimientoHora1">Movimiento</label>
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Debe seleccionar un tipo de movimiento" OnServerValidate="CustomValidator2_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='Debe seleccionar un tipo de movimiento' /&gt;" ValidationGroup="MovimientoHora"></asp:CustomValidator>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <uc2:Ddl_TipoMovimientoHora ID="Ddl_TipoMovimientoHora1" runat="server" MuestraManuales="true" OnSeleccionoOtroItem="SeleccionoOtroItem" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label for="tb_Horas">
                                                            Horas
                                                       <asp:Label ID="lbl_SumaResta" runat="server" Text="" /></label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Horas" ErrorMessage="Debe ingresar la cantidad de horas a registrar" Text="&lt;img src='../Imagenes/exclamation.gif' title='Debe ingresar la cantidad de horas a registrar' /&gt;" ValidationGroup="MovimientoHora"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_Horas" ErrorMessage="La hora ingresada no es correcta xxx:xx" Text="&lt;img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' /&gt;" ValidationExpression="[0-9][0-9][0-9]:[0-5][0-9]" ValidationGroup="MovimientoHora"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="tb_Horas" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:MaskedEditExtender ID="tb_Horas_MaskedEditExtender" runat="server" ClearMaskOnLostFocus="false" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="999:99" TargetControlID="tb_Horas">
                                                        </asp:MaskedEditExtender>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label for="tb_descripcion">Descripción</label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="tb_descripcion" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <br />
                                                        <asp:Button ID="btn_Guardar" CssClass="btn btn-success" runat="server" OnClick="btn_Guardar_Click" Text="Agendar" />
                                                        <asp:Button ID="btn_Cancelar" CssClass="btn btn-danger" runat="server" OnClick="btn_NuevaBusqueda_Click" Text="Cancelar" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">Detalle del
                                               <asp:Label ID="lbl_FechaSeleccionada" runat="server"></asp:Label></h4>
                                            </div>
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <h5>Marcaciones</h5>
                                                        <asp:GridView ID="gv_Huellas" runat="server" AllowPaging="true" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False"
                                                            CssClass="mGrid table-condensed" EmptyDataText="No existen registros." ForeColor="#717171" GridLines="None" OnPageIndexChanging="gv_Huellas_PageIndexChanging" PagerStyle-CssClass="pgr" PageSize="20">
                                                            <Columns>
                                                                <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                                                <asp:BoundField DataField="Fecha" DataFormatString="{0:d}" HeaderText="Fecha" ReadOnly="true" SortExpression="Fecha" />
                                                                <asp:BoundField DataField="Hora" HeaderText="Hora" ReadOnly="true" SortExpression="Hora" />
                                                                <asp:CheckBoxField DataField="MarcaManual" HeaderText="Manual" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <h5>Movimientos agendados</h5>
                                                        <asp:GridView ID="GridView1" runat="server" AllowPaging="true" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False"
                                                            CssClass="mGrid table-condensed" EmptyDataText="El agente no tiene movimientos de horas en la fecha seleccionada." ForeColor="#717171" GridLines="None" OnPageIndexChanging="gridView_PageIndexChanging" PagerStyle-CssClass="pgr">
                                                            <Columns>
                                                                <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" ReadOnly="true" SortExpression="Movimiento" />
                                                                <asp:BoundField DataField="Operador" HeaderText="Operador" ReadOnly="true" SortExpression="Operador" />
                                                                <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                                                <asp:TemplateField HeaderText="Desc. Año Anterior" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" runat="server" Height="16" ImageUrl='<%# Eval("Horasanioanterior")%>' Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Desc. Bonific." ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="Image1" runat="server" Height="16" ImageUrl='<%# Eval("Horasbonific")%>' Width="16" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <p>
                                                            <asp:Label ID="lbl_totalHorasFechaSeleccionada" runat="server" Text="" />
                                                        </p>
                                                    </div>
                                                </div>
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
