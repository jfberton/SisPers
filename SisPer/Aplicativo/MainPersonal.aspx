<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="MainPersonal.aspx.cs" Inherits="SisPer.Aplicativo.MainPersonal" %>

<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc2" %>
<%@ Register Src="~/Aplicativo/Controles/MensageBienvenida.ascx" TagPrefix="uc1" TagName="MensageBienvenida" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">

    <uc2:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc1:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <uc1:MensageBienvenida runat="server" ID="MensageBienvenida" />
    <br />
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Francos compensatorios</h4>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-6">
                            <h4><small>Francos por aprobar</small></h4>
                            <asp:GridView ID="GridViewFrancosPorAprobar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancosPorAprobar_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Dia" HeaderText="Solicitud" ReadOnly="true" SortExpression="Dia"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DiaInicial" HeaderText="Dia" ReadOnly="true" SortExpression="DiaInicial"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="CantidadDias" HeaderText="Dias" ReadOnly="true" SortExpression="CantidadDias" />
                                    <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                                    <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_AprobarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AprobarFrancoPorAprobar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-6">
                            <h4><small>Francos para la firma, enviados a SubAdministración</small></h4>
                            <asp:GridView ID="GridViewFrancosAprobados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewFrancosAprobados_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Dia" HeaderText="Fecha solicitud" ReadOnly="true" SortExpression="Dia"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="DiaInicial" HeaderText="Dia" ReadOnly="true" SortExpression="DiaInicial"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Aprobar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_AprobarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Aprobar" ImageUrl="~/Imagenes/accept.png" OnClick="btn_AprobarFranco_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rechazar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_RechazarFranco" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Rechazar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_RechazarFranco_Click"
                                                OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;" />
                                        </ItemTemplate>
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
                    <h4 class="panel-title">Horarios vespertinos por terminar</h4>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewHVPendientesAprobar" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewHVPendientesAprobar_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Dia" HeaderText="Día" ReadOnly="true" SortExpression="Dia"
                                DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Desde" HeaderText="Hora desde" ReadOnly="true" SortExpression="Desde" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hora hasta" ReadOnly="true" SortExpression="Hasta" />
                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ID="img_Motivo" ImageUrl="../Imagenes/help.png" ToolTip='<%#Eval("Motivo")%>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Horas" HeaderText="Horas" ReadOnly="true" SortExpression="Horas" />
                            <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AnalizarHV" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarHV_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Cambios pendientes de confirmación</h4>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="GridViewCambiosPendientes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridViewCambiosPendientes_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_AnalizarCambios" runat="server" CommandArgument='<%#Eval("Usr")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarCambios_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-md-7">
                            <h4 class="panel-title">Solicitudes pendientes de aprobación</h4>
                        </div>
                        <div class="col-md-4">
                            <ul class="nav navbar-nav navbar-right">
                                <li>
                                    <div class="input-group">
                                        <input type="text" style="padding: 10px;" runat="server" id="tb_LegajoBuscado" class="form-control" placeholder="Legajo">
                                        <span class="input-group-btn">
                                            <button type="button" runat="server" onserverclick="btn_filtrarSolicitudes_Click" class="btn btn-primary">
                                                <span class="glyphicon glyphicon-search"></span>
                                            </button>
                                        </span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gv_EstadosSolicitados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                        AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                        OnPageIndexChanging="gv_EstadosSolicitados_PageIndexChanging"
                        CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                        <Columns>
                            <asp:BoundField DataField="Fechahora" HeaderText="Solicitado el" ReadOnly="true" SortExpression="Fechahora" DataFormatString="{0:g}" NullDisplayText=" - " />
                            <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                            <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                            <asp:BoundField DataField="Encuadre" HeaderText="Encuadre" ReadOnly="true" SortExpression="Encuadre" NullDisplayText=" - " />
                            <asp:BoundField DataField="Familiar" HeaderText="Familiar" ReadOnly="true" SortExpression="Familiar" NullDisplayText=" - " />
                            <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" ReadOnly="true" SortExpression="Parentesco" NullDisplayText=" - " />
                            <asp:BoundField DataField="Lugar" HeaderText="Lugar" ReadOnly="true" SortExpression="Lugar" NullDisplayText=" - " />
                            <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />

                            <asp:TemplateField HeaderText="Aprobar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Aprobar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Aprobar" ImageUrl="~/Imagenes/accept.png" OnClick="btn_Aprobar_Click"
                                        OnClientClick="javascript:if (!confirm('¿Desea APROBAR esta solicitud?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rechazar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Rechazar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                        ToolTip="Rechazar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_Rechazar_Click"
                                        OnClientClick="javascript:if (!confirm('¿Desea RECHAZAR esta solicitud?')) return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <br />

   
</asp:Content>
