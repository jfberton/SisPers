<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatosAgente.ascx.cs"
    Inherits="SisPer.Aplicativo.Controles.DatosAgente" %>
<br />
<div class="panel-group" id="accordion" role="tablist" aria-multiselectable="false">
    <div class="panel panel-info">
        <div class="panel-heading" role="tab" id="headingOne">
            <h1 class="panel-title">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne">Agente
                <asp:Label Text="" ID="lbl_Id" runat="server" Visible="false" /> 
                    <asp:Label Text="" ID="lbl_NombreAgente" runat="server" />
                    <span class="caret" />
                </a>
            </h1>
        </div>
        <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
            <div class="panel-body">

                 <div class="alert alert-warning">
                    <strong>Atención!</strong> Los datos aquí informados están sujetos a modificación por cierre de días pendientes.
                </div>

                <asp:Panel runat="server" ID="p_AgenteComun" Visible="false">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <b>Legajo</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_Legajo" runat="server" />
                            </td>
                            <td>
                                <b>Horas año anterior</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasAnioAnterior" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>E-Mail</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_mail" runat="server" />
                            </td>
                            <td>
                                <b>Horas año actual</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasAnioActual" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Unidad organizativa &nbsp;&nbsp;&nbsp;&nbsp;</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_Area" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <b>Total de horas disponibles</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasTotales" runat="server"
                                    Style="font-weight: 700" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Posee mayor dedicación&nbsp;</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_Bonificacion" runat="server" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <b>Horas de mayor dedicación por cubrir</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_HorasBonificacionACubrir" runat="server" />
                            </td>
                            <td>
                                <b>Tardanzas acumulada del mes</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_Tardanzas" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel runat="server" ID="p_agenteFlexible" Visible="false">
                    <table class="table-condensed">
                        <tr>
                            <td>
                                <b>Legajo</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_legajoFlexible" runat="server" />
                            </td>
                            <td>
                                <b>Horas año anterior</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasAnioAnteriorFlexible" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>E-Mail</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_mailFlexible" runat="server" />
                            </td>
                            <td>
                                <b>Horas año actual</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasAnioActualFlexible" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Unidad organizativa &nbsp;&nbsp;&nbsp;&nbsp;</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_areaFlexible" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <b>Horas acumuladas del mes</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasAcumuladasMes" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Posee mayor dedicación&nbsp;</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lbl_BonificacionFlexible" runat="server" />
                            </td>
                            <td>
                                <b>Total de horas disponibles</b>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label Text="" ID="lbl_HorasTotalesFlexible" runat="server"
                                    Style="font-weight: 700" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Horas de mayor dedicación por cubrir</b>
                            </td>
                            <td>
                                <asp:Label Text="" ID="lblHorasBonificacionACubrirFlexible" runat="server" />
                            </td>
                            <td>
                                <b>Horas por cerrar día</b></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lbl_HorasPorCumplirDiaFlexible" runat="server" /></td>
                        </tr>
                    </table>
                </asp:Panel>

            </div>
        </div>
    </div>
</div>

