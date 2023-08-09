<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Jefe_Ag_SolicitarEstado.aspx.cs" Inherits="SisPer.Aplicativo.Jefe_Ag_SolicitarEstado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<%@ Register Src="~/Aplicativo/Controles/DatosAgente.ascx" TagPrefix="uc1" TagName="DatosAgente" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" Visible="false" ID="MenuAgente1" />
    <uc1:MenuPersonalAgente runat="server" Visible="false" ID="MenuPersonalAgente1" />

    <uc1:MenuPersonalJefe runat="server" Visible="false" ID="MenuPersonalJefe1" />
    <uc1:MenuJefe runat="server" Visible="false" ID="MenuJefe1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <uc1:DatosAgente runat="server" ID="DatosAgente" />
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            <h2 class="panel-title">Nueva solicitud</h2>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
                        CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Detalle de la solicitud</h3>
                        </div>

                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table-condensed">
                                        <tr>
                                            <td colspan="2">
                                                <label for="ddl_TipoMovimiento" style="font-weight: 700">Tipo</label>
                                                <asp:CustomValidator ID="cv_tipoMovimiento" runat="server" ErrorMessage="Este movimiento puede ser informado únicamente hasta las 08:00hs de día a solicitar."
                                                    Text="<img src='../Imagenes/exclamation.gif' title='Este movimiento puede ser informado únicamente hasta las 08:00hs de día a solicitar.' />"
                                                    OnServerValidate="cv_tipoMovimiento_ServerValidate"></asp:CustomValidator><asp:DropDownList ID="ddl_TipoMovimiento" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_TipoMovimiento_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                               
                                                <asp:Label ID="lbl_domicilio" Text="Domicilio" runat="server" Style="font-weight: 700" />
                                                 <asp:CustomValidator ID="cv_domicilio" runat="server" ErrorMessage="Debe consignar el de domicilio del agente para poder continuar."
                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe consignar el de domicilio del agente para poder continuar.' />"
                                                    OnServerValidate="cv_domicilio_ServerValidate"></asp:CustomValidator><asp:TextBox ID="tb_domicilio" CssClass="form-control" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                
                                                <asp:Label ID="lbl_localidad" Text="Localidad" runat="server" Style="font-weight: 700" />
                                                <asp:CustomValidator ID="cv_localidad" runat="server" ErrorMessage="Debe consignar la localidad del agente para poder continuar."
                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe consignar la localidad del agente para poder continuar.' />"
                                                    OnServerValidate="cv_localidad_ServerValidate"></asp:CustomValidator><asp:TextBox ID="tb_localidad" CssClass="form-control" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel runat="server" ID="p_DatosExtra" Visible="false">
                                                    <asp:Label ID="lbl_DatosExtra" Text="Datos extra" runat="server" Style="font-weight: 700" />
                                                    <br />
                                                    <table>
                                                        <tr>
                                                            <tb colspan="2">
                                                                <asp:Label ID="lbl_encuadre" Text="Encuadre" runat="server" Style="text-decoration: underline" />
                                                            </tb>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddl_Encuadre" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_Encuadre_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lbl_familiar" Text="Datos familiar" runat="server" Style="text-decoration: underline" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <%--familiar--%>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_fam_nomyap" runat="server" ErrorMessage="Debe ingresar el nombre y apellido del familiar."
                                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el nombre y apellido del familiar.' />"
                                                                    OnServerValidate="cv_fam_nomyap_ServerValidate"></asp:CustomValidator>
                                                                <asp:Label ID="lbl_fam_nomyAp" Text="Nombre y apellido" runat="server" />
                                                                <asp:TextBox runat="server" ID="tb_fam_nomyap" CssClass="form-control" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <%--Parentesco--%>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:CustomValidator ID="cv_fam_parentesco" runat="server" ErrorMessage="Debe ingresar el parentesco del familiar."
                                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el parentesco del familiar.' />"
                                                                    OnServerValidate="cv_fam_parentesco_ServerValidate"></asp:CustomValidator>
                                                                <asp:Label ID="lbl_fam_parentesco" Text="Parentesco" runat="server" />
                                                                <asp:TextBox runat="server" ID="tb_fam_parentesco" CssClass="form-control" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <tb>
                                                                <asp:CustomValidator ID="cv_sanatorio" runat="server" ErrorMessage="Debe ingresar el sanatorio donde se encuentra internado."
                                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el sanatorio donde se encuentra internado.' />"
                                                                    OnServerValidate="cv_sanatorio_ServerValidate">
                                                                </asp:CustomValidator>
                                                                <asp:Label Text="Sanatorio" ID="lbl_Sanatorio" runat="server" />
                                                                <asp:TextBox runat="server" ID="tb_sanatorio" CssClass="form-control" />
                                                                <asp:DropDownList runat="server" ID="ddl_provincias" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_provincias_SelectedIndexChanged" Visible="false">
                                                                </asp:DropDownList>
                                                            </tb>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <tb>
                                                                <asp:CustomValidator ID="cv_habitacion" runat="server" ErrorMessage="Debe ingresar la habitación donde se encuentra internado."
                                                                    Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el sanatorio donde se encuentra internado.' />"
                                                                    OnServerValidate="cv_habitacion_ServerValidate">
                                                                </asp:CustomValidator>
                                                                <asp:Label ID="lbl_habitacion" Text="Habitación" runat="server" />
                                                                <asp:TextBox runat="server" ID="tb_habitacion" CssClass="form-control" />
                                                            </tb>
                                                        </tr>

                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <p>
                                                    <asp:Label Text="" ID="lbl_Mensaje" runat="server" />
                                                </p>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Período solicitud</h3>
                        </div>
                        <div class="panel-body">
                            <table class="table-condensed">
                                <tr>
                                    <td>
                                        <label for="tb_desde">Desde</label><asp:CustomValidator ID="VerificarDesde" runat="server" ErrorMessage="La fecha ingresada no es válida"
                                            Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es válida' />"
                                            OnServerValidate="VerificarDesde_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="DesdeHoyEnAdelante" runat="server" ErrorMessage="La fecha ingresada debe ser posterior o igual a la fecha actual."
                                            Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada debe ser posterior o igual a la fecha actual.' />"
                                            OnServerValidate="DesdeHoyEnAdelante_ServerValidate"></asp:CustomValidator>

                                        <div class="form-group">
                                            <div id="datetimepicker1" class="input-group date">
                                                <input id="tb_desde" runat="server" class="form-control" type="text" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>


                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="tb_hasta">Hasta</label><asp:CustomValidator ID="VerificarHasta" runat="server" ErrorMessage="La fecha ingresada no es válida"
                                            Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es válida' />"
                                            OnServerValidate="VerificarHasta_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="VerificarHastaMayorIgualQueDesde" runat="server" ErrorMessage="La fecha hasta debe ser mayor o igual a la fecha desde."
                                            Text="<img src='../Imagenes/exclamation.gif' title='La fecha hasta debe ser mayor o igual a la fecha desde.' />"
                                            OnServerValidate="VerificarHastaMayorIgualQueDesde_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="PoseeSolicitudesEnElRango" runat="server" ErrorMessage="El agente posee solicitudes en el rango informado."
                                            Text="<img src='../Imagenes/exclamation.gif' title='El agente posee solicitudes en el rango informado.' />"
                                            OnServerValidate="PoseeSolicitudesEnElRango_ServerValidate"></asp:CustomValidator>

                                        <div class="form-group">
                                            <div id="datetimepicker2" class="input-group date">
                                                <input id="tb_hasta" runat="server" class="form-control" type="text" />
                                                <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button Text="Solicitar" runat="server" CssClass="btn btn-primary" ID="btn_Solicitar" OnClick="btn_Solicitar_Click" />
                                        <asp:CustomValidator ID="cv_VerificarDiasDisponiblesLicencia" runat="server" ErrorMessage="El agente no puede tomarse mas dias de licencia en el año informado."
                                            Text="<img src='../Imagenes/exclamation.gif' title='El agente no puede tomarse mas dias de licencia en el año informado.' />"
                                            OnServerValidate="cv_VerificarDiasDisponiblesLicencia_ServerValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cv_VerificarSiTieneDiasLicenciaAnteriorAnticipo" runat="server" ErrorMessage="No puede solicitar licencia (anticipo) teniendo saldo licencia año anterior."
                                            Text="<img src='../Imagenes/exclamation.gif' title='No puede solicitar licencia (anticipo) teniendo saldo licencia año anterior.' />"
                                            OnServerValidate="cv_VerificarSiTieneDiasLicenciaAnteriorAnticipo_ServerValidate"></asp:CustomValidator>

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Solicitudes del mes</h3>
                        </div>

                        <div class="panel-body">
                            <asp:GridView ID="gv_EstadosSolicitados" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                OnPageIndexChanging="gv_EstadosSolicitados_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <%-- <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />--%>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" ReadOnly="true" SortExpression="Tipo" />
                                    <asp:BoundField DataField="Encuadre" NullDisplayText=" - " HeaderText="Encuadre" ReadOnly="true" SortExpression="Encuadre" />
                                    <asp:BoundField DataField="Desde" HeaderText="Desde" ReadOnly="true" SortExpression="Desde" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Hasta" HeaderText="Hasta" ReadOnly="true" SortExpression="Hasta" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" ReadOnly="true" SortExpression="Estado" />
                                    <asp:TemplateField HeaderText="Cancelar solicitud" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_Cancelar" runat="server" CommandArgument='<%#Eval("Id")%>' CausesValidation="false"
                                                ToolTip="Cancelar" ImageUrl="~/Imagenes/cancel.png" OnClick="btn_Cancelar_Click"
                                                OnClientClick="javascript:if (!confirm('¿Desea CANCELAR esta solicitud?')) return false;" />
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
</asp:Content>
