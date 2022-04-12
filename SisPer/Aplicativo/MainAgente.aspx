<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="MainAgente.aspx.cs" Inherits="SisPer.Aplicativo.MainAgente" %>

<%@ Register Src="Controles/DatosAgente.ascx" TagName="DatosAgente" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuAgente.ascx" TagName="MenuAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagName="MenuJefe" TagPrefix="uc3" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc4" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc5" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>
<%@ Register Src="~/Aplicativo/Controles/AdministrarDiaAgente.ascx" TagPrefix="uc1" TagName="AdministrarDiaAgente" %>
<%@ Register Src="~/Aplicativo/Controles/VisualizarDiaAgente.ascx" TagPrefix="uc1" TagName="VisualizarDiaAgente" %>




<script runat="server" type="text/c#">
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        lbl_HorasCorridas.Text = SisPer.Aplicativo.HorasString.RestarHoras(DateTime.Now.ToString("HH:mm"), lbl_HoraSalida.Text);
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc5:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc4:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc3:MenuJefe ID="MenuJefe1" runat="server" Visible="false" />
    <uc2:MenuAgente ID="MenuAgente1" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
        <uc1:DatosAgente ID="DatosAgente1" runat="server" />

        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lbl_EstadoFuera" runat="server" CssClass="alert alert-danger" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Salidas diarias</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Panel runat="server" ID="P_SalidasDiarias">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                                ValidationGroup="Salida" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                            <asp:Button ID="btn_NuevaSalida" runat="server" class="btn btn-default btn-primary" OnClick="btn_NuevaSalida_Click" Text="Nueva" />
                            <asp:Panel ID="PanelNuevaSalida" runat="server">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h5>Detalle de salida</h5>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5>Tipo</h5>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="Ddl_TipoSalida" runat="server" Width="200" CssClass="form-control" />
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Debe seleccionar el tipo de salida" OnServerValidate="CustomValidator1_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='Debe seleccionar el tipo de salida' /&gt;" ValidationGroup="Salida"></asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <h5>Destino</h5>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="tb_DestinoSalida" CssClass="form-control" runat="server" />
                                                <asp:CustomValidator ID="CustomValidator11" runat="server" ErrorMessage="Si la salida es oficial debe especificar destino." OnServerValidate="CustomValidator11_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='Si la salida es oficial debe especificar destino.' /&gt;" ValidationGroup="Salida"></asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <br />
                                                <asp:Button ID="btn_ComenzarSalida" runat="server" CausesValidation="true" class="btn btn-success" OnClick="btn_ComenzarSalida_Click" OnClientClick="javascript:if (!confirm('¿Desea comenzar la salida?')) return false;" Text="Comenzar!" ValidationGroup="Salida" />
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="Ya se ha tomado su indispocición mensual" OnServerValidate="CustomValidator3_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='Ya se ha tomado su indispocición mensual' /&gt;" ValidationGroup="Salida"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator10" runat="server" ErrorMessage="El agente ya se tomo las 40 horas disponibles de salida particular." OnServerValidate="CustomValidator10_ServerValidate" Text="&lt;img src='../Imagenes/exclamation.gif' title='El agente ya se tomo las 40 horas disponibles de salida particular.' /&gt;" ValidationGroup="Salida"></asp:CustomValidator>
                                                &nbsp;
                                                <asp:Button ID="btn_CancelarNuevaSalida" runat="server" CausesValidation="false" class="btn btn-danger" OnClick="btn_CancelarNuevaSalida_Click" Text="Cancelar" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PanelTieneSalidaActiva" runat="server">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h5>Datos salida actual</h5>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5>Tipo</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>
                                                    <label>
                                                        <asp:Label ID="lbl_TipoSalida" runat="server" Text="" /></label></h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>Hora salida</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>
                                                    <label>
                                                        <asp:Label ID="lbl_HoraSalida" runat="server" Text="" /></label></h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <h5>Llevan</h5>
                                            </div>
                                            <div class="col-md-3">
                                                <h5>
                                                    <label>
                                                        <asp:Label ID="lbl_HorasCorridas" runat="server" Text="" /></label></h5>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Button ID="btn_TerminarSalida" runat="server" CssClass="btn btn-primary" OnClick="btn_TerminarSalida_Click" Text="Terminar!" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:GridView ID="GridViewSalidas" runat="server" AllowPaging="true" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid table-condensed" EmptyDataText="No existen salidas diarias por mostrar." ForeColor="#717171" GridLines="None" OnPageIndexChanging="gridViewSalidas_PageIndexChanging" PagerStyle-CssClass="pgr">
                                <Columns>
                                    <asp:BoundField DataField="MarcoJefe" HeaderText="Iniciada por" ReadOnly="true" SortExpression="MarcoJefe" />
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                                    <asp:BoundField DataField="Destino" HeaderText="Destino" ReadOnly="true" SortExpression="Destino" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" />
                                    <asp:BoundField DataField="Diferencia" HeaderText="Diferencia" ReadOnly="true" SortExpression="Diferencia" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Horarios vespertinos del mes</h3>
                    </div>
                    <div class="panel-body">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                            ValidationGroup="HV" CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-headig'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                        <asp:Button ID="btn_NuevaSolicitudHV" class="btn btn-default btn-primary" CausesValidation="false" Text="Nueva solicitud"
                            runat="server" OnClick="btn_NuevaSolicitudHV_Click" />
                        <asp:Panel runat="server" ID="PanelSolicitarHV" Visible="false">

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h5>Detalle de la solicitud</h5>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-1">
                                            <h5>Día</h5>
                                        </div>
                                        <div class="col-md-11">
                                            <div class="form-group">
                                                <div id="datetimepicker1" class="input-group date">
                                                    <input id="tb_Dia" runat="server" class="form-control" type="text" />
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="HV"
                                                ControlToValidate="tb_Dia" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el día en que va a realizar el horario vespertino' />"
                                                ErrorMessage="Debe ingresar el día en que va a realizar el horario vespertino"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator2" ValidationGroup="HV" Text="<img src='../Imagenes/exclamation.gif' title='El día seleccionado debe ser igual o mayor al día actual' />"
                                                runat="server" ErrorMessage="El día seleccionado debe ser igual o mayor al día actual"
                                                OnServerValidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator8" ValidationGroup="HV" Text="<img src='../Imagenes/exclamation.gif' title='Imposible solicitar horario vespertino, tiene agendado franco o licencia esa fecha' />"
                                                runat="server" ErrorMessage="Imposible solicitar horario vespertino, tiene agendado franco o licencia esa fecha"
                                                OnServerValidate="CustomValidator8_ServerValidate"></asp:CustomValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <h5>Hora inicial</h5>
                                                </div>
                                                <div class="col-md-6">

                                                    <div class="form-group">
                                                        <div id="datetimepicker2" class="input-group date">
                                                            <input id="tb_HoraDesde" runat="server" class="form-control" type="text" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-time"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="HV"
                                                        ControlToValidate="tb_HoraDesde" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora inicial' />"
                                                        ErrorMessage="Debe ingresar la hora inicial"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="HV"
                                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                        ControlToValidate="tb_HoraDesde" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                                        ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <h5>Hora final</h5>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <div id="datetimepicker3" class="input-group date">
                                                            <input id="tb_HoraHasta" runat="server" class="form-control" type="text" />
                                                            <span class="input-group-addon">
                                                                <span class="glyphicon glyphicon-time"></span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="HV"
                                                        ControlToValidate="tb_HoraHasta" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora final' />"
                                                        ErrorMessage="Debe ingresar la hora final"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="HV"
                                                        Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                        ControlToValidate="tb_HoraHasta" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                                        ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                                    <asp:CustomValidator ID="CustomValidator4" ValidationGroup="HV" Text="<img src='../Imagenes/exclamation.gif' title='La hora final debe ser mayor o igual a la hora inicial' />"
                                                        runat="server" ErrorMessage="La hora final debe ser mayor o igual a la hora inicial"
                                                        OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lbl_DiasHV" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            Motivo
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_MotivoHV"
                                                    ValidationGroup="HV" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar los motivos de la solicitud' />"
                                                    runat="server" ErrorMessage="Debe ingresar los motivos de la solicitud"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:TextBox ID="tb_MotivoHV" CssClass="form-control" runat="server" Width="100%" Height="50" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <asp:Button Text="Solicitar!" ValidationGroup="HV" class="btn btn-success" runat="server" ID="btn_SolicitarHV"
                                                OnClick="btn_SolicitarHV_Click" />&nbsp;
                                                <asp:Button Text="Cancelar" CausesValidation="false" class="btn btn-danger" runat="server" ID="btn_Cancelar"
                                                    OnClick="btn_Cancelar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="GridViewHV" runat="server" EmptyDataText="No existen horarios vespertinos por mostrar en el mes." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewHV_PageIndexChanging"
                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                        <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                            DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Desde" HeaderText="Hora Desde" ReadOnly="true" SortExpression="Desde" />
                                        <asp:BoundField DataField="Hasta" HeaderText="Hora Hasta" ReadOnly="true" SortExpression="Hasta" />
                                        <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Image ID="img_Motivo" ImageUrl="../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btn_EliminarSolicitudHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                    ToolTip="Eliminar solicitud" ImageUrl="~/Imagenes/delete.png" OnClick="btn_EliminarSolicitudHV_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ult. Mod." ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Image ID="UsuarioUltimaModificacion" ImageUrl="~/Imagenes/user_gray.png" ToolTip='<%#Eval("Jefe")%>' runat="server" />
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

        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Francos compensatorios/Art. 51 del mes</h3>
                    </div>
                    <div class="panel-body">
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="BulletList"
                            ValidationGroup="Francos" CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                         <asp:ValidationSummary ID="ValidationSummary6" runat="server" DisplayMode="BulletList"
                            ValidationGroup="articulo" CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                        <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="BulletList"
                            ValidationGroup="AprobarFrancos" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                        <asp:Button ID="btn_SolicitarFranco" class="btn btn-primary" CausesValidation="false" Text="Nueva solicitud"
                            runat="server" OnClick="btn_NuevaSolicitudFranco_Click" />

                        <asp:Panel runat="server" ID="PanelSolicitarFranco" Visible="false">

                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h5>Solicitud de franco compensatorio / Art. 51</h5>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-md-2">Tipo de solicitud </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_tipo_solicitud_franco">
                                                <asp:ListItem Text="Franco compensatorio" />
                                                <asp:ListItem Text="Art. 51" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <h5>Para el </h5>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <div id="datetimepicker4" class="input-group date">
                                                    <input id="tb_FechaFranco" runat="server" class="form-control" type="text" />
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Francos"
                                                ControlToValidate="tb_FechaFranco" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el día en que va a tomarse el franco' />"
                                                ErrorMessage="Debe ingresar el día en que va a tomarse el franco"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator5" ValidationGroup="Francos" Text="<img src='../Imagenes/exclamation.gif' title='El día seleccionado debe ser igual o mayor al día actual' />"
                                                runat="server" ErrorMessage="El día seleccionado debe ser igual o mayor al día actual"
                                                OnServerValidate="CustomValidator5_ServerValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator9" ValidationGroup="Francos" Text="<img src='../Imagenes/exclamation.gif' title='Imposible solicitar franco compensatorio esa fecha, ya tiene agendado franco o licencia.' />"
                                                runat="server" ErrorMessage="Imposible solicitar franco compensatorio esa fecha, ya tiene agendado franco o licencia."
                                                OnServerValidate="CustomValidator9_ServerValidate"></asp:CustomValidator>


                                            <asp:RequiredFieldValidator ID="validator_articulo1" runat="server" ValidationGroup="articulo"
                                                ControlToValidate="tb_FechaFranco" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el día en que va a tomarse el artículo' />"
                                                ErrorMessage="Debe ingresar el día en que va a tomarse el artículo"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="validator_articulo2" ValidationGroup="articulo" Text="<img src='../Imagenes/exclamation.gif' title='El día seleccionado debe ser igual o mayor al día actual' />"
                                                runat="server" ErrorMessage="El día seleccionado debe ser igual o mayor al día actual"
                                                OnServerValidate="CustomValidator5_ServerValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="validator_articulo4" ValidationGroup="articulo" Text="<img src='../Imagenes/exclamation.gif' title='Solicite la marcación del artículo a su jefe, superior o a personal.' />"
                                                runat="server" ErrorMessage="Solicite la marcación del artículo a su jefe, superior o a personal."
                                                OnServerValidate="validator_articulo4_ServerValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="validator_articulo5" ValidationGroup="articulo" Text="<img src='../Imagenes/exclamation.gif' title='No puede realizar la solicitud, el agente supero las 40 horas disponibles.' />"
                                                runat="server" ErrorMessage="No puede realizar la solicitud, el agente supero las 40 horas disponibles."
                                                OnServerValidate="CustomValidator10_ServerValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="validator_articulo3" ValidationGroup="articulo" Text="<img src='../Imagenes/exclamation.gif' title='Imposible solicitar artículo esa fecha, ya tiene agendado franco o licencia.' />"
                                                runat="server" ErrorMessage="Imposible solicitar artículo esa fecha, ya tiene agendado franco o licencia."
                                                OnServerValidate="CustomValidator9_ServerValidate"></asp:CustomValidator>



                                            <asp:CustomValidator ID="CustomValidator6" ValidationGroup="Francos" Text="<img src='../Imagenes/exclamation.gif' title='Imposible solicitar franco compensatorio esa fecha, ya tiene agendado franco o licencia.' />"
                                                runat="server" ErrorMessage="Imposible solicitar franco compensatorio esa fecha, ya tiene 2 solicitudes realizadas."
                                                OnServerValidate="CustomValidator6_ServerValidate"></asp:CustomValidator>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <asp:Button ID="btn_AceptarFranco" class="btn btn-success" runat="server" Text="Solicitar!" OnClick="btn_AceptarFranco_Click"
                                                ValidationGroup="AprobarFrancos" />
                                            <asp:CustomValidator ID="CustomValidator7" ValidationGroup="Francos" Text="<img src='../Imagenes/exclamation.gif' title='Usted no tiene horas disponibles para solicitar el franco.' />"
                                                runat="server" ErrorMessage="Usted no tiene horas disponibles para solicitar el franco." OnServerValidate="CustomValidator7_ServerValidate"></asp:CustomValidator>&nbsp;
                                            <asp:Button ID="btn_CancelarFranco" class="btn btn-danger" runat="server" Text="Cancelar" OnClick="btn_CancelarFranco_Click" CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <table class="table-condensed">
                            <tr>
                                <td valign="top">
                                    <h4>Listado de francos</h4>
                                    <asp:GridView ID="GridViewFrancos" runat="server" EmptyDataText="No existen francos por mostrar en el mes." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancos_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                            <asp:BoundField DataField="Dia" HeaderText="Fecha solicitud" ReadOnly="true" SortExpression="Dia"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="DiaInicial" HeaderText="Fecha solicitada" ReadOnly="true" SortExpression="DiaInicial"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_EliminarSolicitudFC" runat="server" CommandArgument='<%#Eval("Id")%>' OnClientClick="javascript:if (!confirm('¿Desea ELIMINAR esta solicitud?')) return false;"
                                                        ToolTip="Eliminar solicitud" ImageUrl="~/Imagenes/delete.png" OnClick="btn_EliminarSolicitudFC_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Movimientos" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_VerMovimientos" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                        ToolTip="Ver historial de movimientos" ImageUrl="~/Imagenes/clock_go.png" OnClick="btn_VerMovimientos_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <h4>Listado de Art 51</h4>
                                    <asp:GridView ID="GridViewArticulos" runat="server" EmptyDataText="No existen francos por mostrar en el mes." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewArticulos_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                            <asp:BoundField DataField="DiaInicial" HeaderText="Fecha solicitada" ReadOnly="true" SortExpression="DiaInicial"
                                                DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_EliminarSolicitudArt" runat="server" CommandArgument='<%#Eval("Id")%>' OnClientClick="javascript:if (!confirm('¿Desea ELIMINAR esta solicitud?')) return false;"
                                                        ToolTip="Eliminar solicitud" ImageUrl="~/Imagenes/delete.png" OnClick="btn_EliminarSolicitudArt_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                        <div class="modal fade" runat="server" id="modalHistMov" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Historia de movimientos</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div style="height: 400px; overflow-y: scroll;">
                                            <asp:GridView ID="gv_MovFrancos" runat="server" ForeColor="#717171"
                                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                <Columns>
                                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" SortExpression="Fecha"
                                                        DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                                    <asp:BoundField DataField="Observacion" HeaderText="Observacion" ReadOnly="true" SortExpression="Observacion" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" id="btn_cerrar_modal" class="btn btn-default" data-dismiss="modal" runat="server" onserverclick="btn_cerrar_modal_ServerClick">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Marcaciones entrada - salida</h3>
                    </div>
                    <div class="panel-body">
                        <table class="table-condensed">
                            <tr>
                                <td>
                                    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" OnDayRender="Calendar1_DayRender" BorderColor="#999999" CellPadding="4"
                                        OnSelectionChanged="Calendar1_SelectionChanged" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                        Height="180px" Width="200px">
                                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt"></DayHeaderStyle>
                                        <NextPrevStyle VerticalAlign="Bottom"></NextPrevStyle>
                                        <OtherMonthDayStyle ForeColor="#808080"></OtherMonthDayStyle>
                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White"></SelectedDayStyle>
                                        <SelectorStyle BackColor="#CCCCCC"></SelectorStyle>
                                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True"></TitleStyle>
                                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black"></TodayDayStyle>
                                        <WeekendDayStyle BackColor="#FFFFCC"></WeekendDayStyle>
                                    </asp:Calendar>
                                </td>
                                <td>
                                    <asp:GridView ID="gv_Huellas" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                        AutoGenerateColumns="False" GridLines="None" PageSize="20" AllowPaging="true" OnPageIndexChanging="gv_Huellas_PageIndexChanging"
                                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr"
                                        AlternatingRowStyle-CssClass="alt">
                                        <Columns>
                                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" ReadOnly="true" SortExpression="Fecha" />
                                            <asp:BoundField DataField="Hora" HeaderText="Hora" ReadOnly="true" SortExpression="Hora" />
                                            <asp:CheckBoxField DataField="MarcaManual" ItemStyle-HorizontalAlign="Center" HeaderText="Manual" ReadOnly="true" />
                                            <asp:CheckBoxField DataField="EsDefinitivo" ItemStyle-HorizontalAlign="Center" HeaderText="Definitivo" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div runat="server" id="div_ES" visible="false">
                            <table class="table-condensed">
                                <tr>
                                    <td colspan="2">
                                        <asp:ValidationSummary ID="ValidationSummary5" runat="server" DisplayMode="BulletList"
                                            ValidationGroup="MarcacionesES" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <h3>Modificar E/S</h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Estas marcaciones serán impactadas una vez que personal cierre el día.</td>
                                </tr>
                                <tr>
                                    <td>Entrada</td>
                                    <td>
                                        <div id="datetimepicker5" class="input-group date">
                                            <input id="h_entrada" runat="server" class="form-control" type="text" />
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-time"></span>
                                            </span>
                                        </div>
                                        <asp:CustomValidator ID="cv_puedemodificar" runat="server"
                                            ErrorMessage="No se puede modificar debido a que el jefe ya ha enviado las marcaciones del día a personal" OnServerValidate="cv_puedemodificar_ServerValidate"
                                            Text="&lt;img src='../Imagenes/exclamation.gif' title='No se puede modificar debido a que el jefe ya ha enviado las marcaciones del día a personal' /&gt;"
                                            ValidationGroup="MarcacionesES"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ControlToValidate="h_entrada" Text="<img src='../Imagenes/exclamation.gif' title='El campo es obligatorio' />"
                                            ID="RequiredFieldValidator" runat="server" ValidationGroup="MarcacionesES" ErrorMessage="El campo es obligatorio"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="MarcacionesES"
                                            Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                            ControlToValidate="h_entrada" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                            ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator></td>
                                </tr>
                                <tr>
                                    <td>Salida</td>
                                    <td>
                                        <div id="datetimepicker6" class="input-group date">
                                            <input id="h_salida" runat="server" class="form-control" type="text" />
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-time"></span>
                                            </span>
                                        </div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationGroup="MarcacionesES"
                                            Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                            ControlToValidate="h_salida" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                            ErrorMessage="La hora ingresada no es correcta xx:xx"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                            <asp:Button Text="Guardar" class="btn btn-primary" runat="server" ID="btn_GuardarMarcacionES" OnClick="btn_GuardarMarcacionES_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Días por cerrar</h3>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gv_dias_sin_cerrar" runat="server" 
                            EmptyDataText="Excelente!!! - No existen días por cerrar." 
                            OnPreRender="gv_dias_sin_cerrar_PreRender"
                            AutoGenerateColumns="false" GridLines="None" CssClass="compact stripe">
                            <Columns>
                                <asp:BoundField DataField="Id" />
                                <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia" DataFormatString="{0:D}" />
                                <asp:BoundField DataField="Motivo" HeaderText="Motivo" ReadOnly="true" SortExpression="Motivo" />
                                <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_ver_dia_sin_cerrar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                            ToolTip="Ver detalle día" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_ver_dia_sin_cerrar_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                        <div class="modal fade" id="verDiaSinCerrar" tabindex="-1" role="dialog" aria-labelledby="verDiaSinCerrarLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="verDiaSinCerrarLabel">Detalle del dia sin cerrar</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <uc1:VisualizarDiaAgente runat="server" ID="VisualizarDiaAgente" ReadOnly="true" Visible="false" />
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="contentScripts">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gv_dias_sin_cerrar.ClientID %>').DataTable({
                "paging": true,
                "language": {
                    "search": "Buscar:",
                    "lengthMenu": "Mostrar _MENU_ agentes por página",
                    "zeroRecords": "No se encontraron registros",
                    "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de _MAX_ registros totales)",
                    "paginate": {
                        "first": "Primero",
                        "last": "Ultimo",
                        "next": "Siguiente",
                        "previous": "Anterior"
                    }
                },
                "columnDefs": [
                    { "visible": false, "targets": 0 },
                    { "orderable": false, "targets": 1 },
                    { "orderable": false, "targets": 2 },
                    { "orderable": false, "targets": 3 }
                ],
                "order": [[0, 'asc']],
            });
        });

        $(document).ready(
            function actualizarTiempoSalida() {

                var myStyle = document.getElementById('<%= lbl_HoraSalida.ClientID %>');
                //si el label horas salidas esta visible procedo a actualizar
                if (myStyle != null) {
                    //setear valores en milisegundos.
                    var msecPerMinute = 1000 * 60;
                    var msecPerHour = msecPerMinute * 60;
                    var msecPerDay = msecPerHour * 24;

                    //Obtener fecha y hora actual en milisegundos
                    var date = new Date();
                    var dateNowMsec = date.getTime();

                    // cambiar la fecha a la fecha y hora de salida
                    // (tener en cuenta que las salidas se terminan en el día, por lo tanto dejo la fecha y cambio la hora nomas)
                    var hora = document.getElementById('<%= lbl_HoraSalida.ClientID %>').innerHTML.split(':')[0];
                    var minuto = document.getElementById('<%= lbl_HoraSalida.ClientID %>').innerHTML.split(':')[1];

                    date.setHours(hora, minuto)

                    // Get the difference in milliseconds.
                    var interval = dateNowMsec - date.getTime();

                    // Calculate how many days the interval contains. Subtract that
                    // many days from the interval to determine the remainder.
                    var days = Math.floor(interval / msecPerDay);
                    interval = interval - (days * msecPerDay);

                    // Calculate the hours, minutes, and seconds.
                    var hours = Math.floor(interval / msecPerHour);
                    interval = interval - (hours * msecPerHour);

                    var minutes = Math.floor(interval / msecPerMinute);
                    interval = interval - (minutes * msecPerMinute);

                    var seconds = Math.floor(interval / 1000);

                    // Display the result.
                    document.getElementById('<%= lbl_HorasCorridas.ClientID %>').innerHTML = checkTimeSalida(hours) + ":" + checkTimeSalida(minutes) + "hs.";
                    var t = setTimeout(function () { actualizarTiempoSalida() }, 500);
                }

            })

        function checkTimeSalida(i) {
            if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
            return i;
        }
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY',
                daysOfWeekDisabled: [0, 6]
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker3').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker4').datetimepicker({
                locale: 'es',
                format: 'dddd[,] DD [de] MMMM [de] YYYY',
                daysOfWeekDisabled: [0, 6]
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker5').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#datetimepicker6').datetimepicker({
                locale: 'es',
                format: 'LT'
            });
        });
    </script>
</asp:Content>
