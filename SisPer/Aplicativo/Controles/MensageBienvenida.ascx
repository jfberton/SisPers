<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MensageBienvenida.ascx.cs" Inherits="SisPer.Aplicativo.Controles.MensageBienvenida" %>

<div class="modal fade" tabindex="-1" role="dialog" id="modalBienvenida">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Sistema personal</h4>
            </div>
            <div class="modal-body">
                <h1>Bienvenido! <small>
                    <asp:Label Text="" runat="server" ID="lbl_ApyNom" /></small>

                </h1>
                <div runat="server" id="div_valido_mail" class="alert alert-success">
                    Acabas de validar tu correo:
                    <label>
                        <asp:Label Text="" ID="lbl_correo" runat="server" /></label>
                </div>
                <div runat="server" id="div_correo_pendiente_de_validacion" class="alert alert-warning">
                    Recuerde que debe validar su correo electrónico!, desde: Editar datos -> Cambiar e-mail.
                </div>
                <h3>
                    <asp:Label Text="" ID="lbl_ultimo_acceso" runat="server" />
                </h3>
                <div class="panel-group" id="accordion_novedades" role="tablist" aria-multiselectable="true">
                    <h3>
                        <asp:Label Text="Te recuerdo que:" runat="server" ID="lbl_tiene_algo_por_revisar" /></h3>
                    <div class="panel panel-default" id="panel_mensajes" runat="server">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" data-parent="#accordion_novedades" href="#collapse_mensajes" aria-expanded="false" aria-controls="collapse_mensajes">Tienes
                                    <asp:Label Text="" ID="lbl_mensajes" runat="server" />
                                    mensaje por leer <small>(click para ver detalle)</small>
                                </a>
                            </h4>
                        </div>
                        <div id="collapse_mensajes" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <asp:GridView ID="gv_mensajes" runat="server" EmptyDataText="No existen registros cargados." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="true" />
                                        <asp:BoundField DataField="emisor" HeaderText="De" ReadOnly="true" />
                                        <asp:BoundField DataField="asunto" HeaderText="Asunto" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default" id="panel_solicitudes" runat="server">
                        <div class="panel-heading" role="tab" id="headingTwo">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion_novedades" href="#collapse_solicitudes" aria-expanded="false" aria-controls="collapse_solicitudes">Tienes
                                    <asp:Label Text="" ID="lbl_solicitudes" runat="server" />
                                    solicitudes de presentación de documentación <small>(click para ver detalle)</small>
                                </a>
                            </h4>
                        </div>
                        <div id="collapse_solicitudes" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <asp:GridView ID="gv_solicitudes" runat="server" EmptyDataText="No existen registros cargados." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="Número" ReadOnly="true" SortExpression="Id" />
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="true" SortExpression="Descripcion" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default" id="panel_solicitudes_1214" runat="server">
                        <div class="panel-heading" role="tab" id="headingThree">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion_novedades" href="#collapse_solicitudes_1214" aria-expanded="false" aria-controls="collapse_solicitudes_1214">Tienes
                                    <asp:Label Text="" ID="lbl_solicitudes_1214" runat="server" />
                                    solicitudes de comisión por revisar <small>(click para ver detalle)</small>
                                </a>
                            </h4>
                        </div>
                        <div id="collapse_solicitudes_1214" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                            <div class="panel-body">
                                <asp:GridView ID="gv_pendientes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                    AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="f1214_id" HeaderText="F1214 N°" ReadOnly="true" SortExpression="f1214_id" />
                                        <asp:BoundField DataField="area" HeaderText="Area" ReadOnly="true" SortExpression="area" />
                                        <asp:BoundField DataField="agente" HeaderText="Agente" ReadOnly="true" SortExpression="agente" />
                                        <asp:BoundField DataField="destino" HeaderText="Destino" ReadOnly="true" SortExpression="destino" />
                                        <asp:BoundField DataField="desde" HeaderText="Desde" ReadOnly="true" SortExpression="desde" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="hasta" DataFormatString="{0:d}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
    </div>
</div>
