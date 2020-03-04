<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Licencias.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Licencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc4" %>
<%@ Register Src="~/Aplicativo/Controles/Licencia.ascx" TagPrefix="uc2" TagName="Licencia" %>
<%@ Register Src="~/Aplicativo/Controles/DatosAgente.ascx" TagPrefix="uc2" TagName="DatosAgente" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc4:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">Administrar licencias anuales</h3>
        </div>
        <div class="panel-body">

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">Filtro por legajos</h4>
                </div>
                <div class="panel-body">
                    <div class="row panel">
                        <div class="col-md-1">
                            <label>Desde</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="tb_desde" CssClass="form-control" />
                        </div>
                        <div class="col-md-1">
                            <label>Hasta</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="tb_hasta" CssClass="form-control" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button Text="Buscar" ID="btn_buscar" runat="server" CssClass="btn btn-default" OnClick="btn_buscar_Click" />
                        </div>
                    </div>

                    <div class="row panel">
                        <div class="col-md-3">
                            <asp:GridView ID="GridViewAgentes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="GridViewAgentes_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" />
                                    <asp:BoundField DataField="DiasAnioAnterior" HeaderText="año anterior" ReadOnly="true" />
                                    <asp:BoundField DataField="DiasAnioActual" HeaderText="año actual" ReadOnly="true" />

                                    <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_AdministrarLicencias" runat="server" CommandArgument='<%#Eval("AgenteId")%>'
                                                ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AdministrarLicencias_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-9">
                            <asp:Panel runat="server" ID="p_datosLicencia" Visible="false">

                                <%--Datos agente--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <uc2:DatosAgente runat="server" ID="DatosAgente" />
                                    </div>
                                </div>
                                <%--Datos calculo de antiguedad--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="panel panel-warning">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <asp:LinkButton ID="lnk_mostrar_datos_calculo_antiguedad" OnClick="lnk_mostrar_datos_calculo_antiguedad_Click" runat="server">
                                                        Años de antigüedad totales <span class="badge">
                                                            <asp:Label Text="" ID="lbl_AñosTotalesAntiguedad" runat="server" /></span>
                                                    </asp:LinkButton><span class="caret"></span>
                                                </h4>
                                            </div>
                                            <div class="collapse" id="collapseAntiguedadLicencia" runat="server">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <label>Fecha de ingreso Adm. Púb.</label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                                    <div class="input-group">
                                                                        <asp:Label Text="" CssClass="form-control" ID="lbl_IngresoAdmPub" runat="server" />
                                                                        <asp:TextBox type="text" runat="server" ID="tb_IngresoAdmPub" class="form-control" placeholder="Legajo" />
                                                                        <asp:CalendarExtender ID="CalendarExtender1" Format="d" runat="server" CssClass="alert alert-info"
                                                                            Enabled="True" TargetControlID="tb_IngresoAdmPub"></asp:CalendarExtender>
                                                                        <span class="input-group-btn">
                                                                            <button type="button" runat="server" id="btn_Edit_IngresoAdmPub" visible="false" onserverclick="btn_Edit_IngresoAdmPub_Click" class="btn btn-warning">
                                                                                <span class="glyphicon glyphicon-pencil" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Accept_IngresoAdmPub" onserverclick="btn_Accept_IngresoAdmPub_Click" class="btn btn-success" visible="false">
                                                                                <span class="glyphicon glyphicon-ok" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Cancel_IngresoAdmPub" data-toggle="tooltip" data-placement="left" title="Editar" onserverclick="btn_Cancel_IngresoAdmPub_Click" class="btn btn-danger" visible="false">
                                                                                <span class="glyphicon glyphicon-remove" />
                                                                            </button>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ValidationGroup="IngresoAdmPub" ControlToValidate="tb_IngresoAdmPub" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha de ingreso a la administración pública' />"
                                                                        runat="server" ErrorMessage="Debe ingresar la fecha de ingreso a la administración pública"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ID="cv_IngresoAdmPub" runat="server" ValidationGroup="IngresoAdmPub"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                                                        ErrorMessage="La fecha ingresada no es correcta"
                                                                        OnServerValidate="cv_IngresoAdmPub_ServerValidate"></asp:CustomValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <label>Fecha de ingreso A.T.P.</label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                                    <div class="input-group">
                                                                        <asp:Label Text="" ID="lbl_IngresoAPlanta" CssClass="form-control" runat="server" />
                                                                        <asp:TextBox ID="tb_IngresoAPlanta" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="tb_IngresoAPlanta_CalendarExtender" Format="d" runat="server" CssClass="alert alert-info"
                                                                            Enabled="True" TargetControlID="tb_IngresoAPlanta"></asp:CalendarExtender>
                                                                        <span class="input-group-btn">
                                                                            <button type="button" runat="server" id="btn_Edit_IngresoAPlanta" visible="false" onserverclick="btn_Edit_IngresoAPlanta_Click" class="btn btn-warning">
                                                                                <span class="glyphicon glyphicon-pencil" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Accept_IngresoAPlanta" onserverclick="btn_Accept_IngresoAPlanta_Click" class="btn btn-success" visible="false">
                                                                                <span class="glyphicon glyphicon-ok" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Cancel_IngresoAPlanta" data-toggle="tooltip" data-placement="left" title="Editar" onserverclick="btn_Cancel_IngresoAPlanta_Click" class="btn btn-danger" visible="false">
                                                                                <span class="glyphicon glyphicon-remove" />
                                                                            </button>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ValidationGroup="IngresoAPlanta" ControlToValidate="tb_IngresoAPlanta" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha de ingreso a planta permanente' />"
                                                                        runat="server" ErrorMessage="Debe ingresar la fecha de ingreso a planta permanente"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ID="cv_IngresoAPlanta" runat="server" ValidationGroup="IngresoAPlanta"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                                                        ErrorMessage="La fecha ingresada no es correcta"
                                                                        OnServerValidate="cv_IngresoAPlanta_ServerValidate"></asp:CustomValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <label>Antiguedad reconocida en otras partes</label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                                    <div class="input-group">
                                                                        <asp:Label Text="" ID="lbl_AniosEnOtrasPartes" CssClass="form-control" runat="server" />
                                                                        <asp:Label Text="Años" ID="lbl_labelAños" CssClass=" form-control" runat="server" />
                                                                        <asp:TextBox runat="server" ID="tb_AntiguedadAnios" CssClass="form-control" />
                                                                        <asp:Label Text="Meses" ID="lbl_labelMeses" runat="server" CssClass="form-control" />
                                                                        <asp:TextBox runat="server" ID="tb_AntiguedadMeses" CssClass="form-control" />
                                                                        <span class="input-group-btn">
                                                                            <button type="button" runat="server" id="btn_Edit_AniosEnOtrasPartes" visible="false" onserverclick="btn_Edit_AniosEnOtrasPartes_Click" class="btn btn-warning">
                                                                                <span class="glyphicon glyphicon-pencil" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Accept_AniosEnOtrasPartes" onserverclick="btn_Accept_AniosEnOtrasPartes_Click" class="btn btn-success" visible="false">
                                                                                <span class="glyphicon glyphicon-ok" />
                                                                            </button>
                                                                            <button type="button" runat="server" id="btn_Cancel_AniosEnOtrasPartes" data-toggle="tooltip" data-placement="left" title="Editar" onserverclick="btn_Cancel_AniosEnOtrasPartes_Click" class="btn btn-danger" visible="false">
                                                                                <span class="glyphicon glyphicon-remove" />
                                                                            </button>
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_AntiguedadAnios" ValidationGroup="AniosEnOtrasPartes" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar los años de antiguedad' />"
                                                                        runat="server" ErrorMessage="Debe ingresar los años de antiguedad"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ID="cv_AntiguedadAnios" OnServerValidate="cv_AntiguedadAnios_ServerValidate" runat="server"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='Ingrese unicamente numeros enteros' />" ValidationGroup="AniosEnOtrasPartes"
                                                                        ErrorMessage="Ingrese unicamente numeros enteros"></asp:CustomValidator>
                                                                    <asp:RequiredFieldValidator ControlToValidate="tb_AntiguedadMeses" ValidationGroup="AniosEnOtrasPartes" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar los meses de antiguedad' />"
                                                                        ID="RequiredFieldValidator13" runat="server" ErrorMessage="Debe ingresar los meses de antiguedad"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ID="cv_AntiguedadMeses" ValidationGroup="AniosEnOtrasPartes" OnServerValidate="cv_AntiguedadMeses_ServerValidate" runat="server"
                                                                        Text="<img src='../Imagenes/exclamation.gif' title='Ingrese unicamente numeros enteros' />"
                                                                        ErrorMessage="Ingrese unicamente numeros enteros"></asp:CustomValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Agregar corte licencia--%>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <asp:LinkButton ID="lnk_corte_licencia" OnClick="lnk_corte_licencia_Click" runat="server">
                                                        Cargar corte licencia
                                                    </asp:LinkButton><span class="caret"></span>
                                                </h4>
                                            </div>
                                            <div class="collapse" id="collapseCorteLicencia" runat="server">
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-md-7">
                                                            <table class="table-condensed">
                                                                <tr>
                                                                    <td><b>N° Instr.</b></td>
                                                                    <td>
                                                                        <asp:TextBox CssClass="form-control" runat="server" ID="tb_corte_Instrumento" /></td>
                                                                    <td><b>Año</b></td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_corte_anio">
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><b>Tipo</b></td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_corteTipoLicencia">
                                                                            <asp:ListItem Text="Anual" />
                                                                            <asp:ListItem Text="Invierno" />
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><b>Desde</b></td>
                                                                    <td>
                                                                        <asp:TextBox CssClass="form-control" runat="server" ID="tb_corte_desde" />
                                                                        <asp:CalendarExtender runat="server" Enabled="True" CssClass="alert alert-info" TargetControlID="tb_corte_desde" ID="tb_desde_CalendarExtender"></asp:CalendarExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <h3>
                                                                            <asp:Label Text="" Visible="false" ID="lbl_Visualizarcorte" CssClass="label label-info" runat="server" /></h3>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button Text="Visualizar" ID="btn_Visualizar_Corte" OnClick="btn_Visualizar_Corte_Click" CssClass="btn btn-info" runat="server" /></td>
                                                                    <td>
                                                                        <asp:Button Text="Cargar corte" Visible="false" ID="btn_Cargar_Corte" OnClick="btn_Cargar_Corte_Click" CssClass="btn btn-success" runat="server" />
                                                                        <asp:Button Text="Cancelar" Visible="false" ID="btn_cancelar_corte" OnClick="btn_cancelar_corte_Click" CssClass="btn btn-danger" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div class="col-md-5">
                                                            <asp:Label Text="Instrumentos cargados al agente" runat="server" />
                                                            <asp:GridView ID="gv_cortes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Instrumento" HeaderText="N° Instrumento" />
                                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                                    <asp:BoundField DataField="Anio" HeaderText="Año" />
                                                                    <asp:BoundField DataField="Desde" HeaderText="Desde" DataFormatString="{0:d}" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Licencias anuales--%>
                                <div class="row panel">
                                    <div class="col-md-6">
                                        <uc2:Licencia runat="server" ID="LicenciaAnterior" HabilitarEdicion="true" OnModificoValor="LicenciaAnterior_ModificoValor" />
                                    </div>
                                    <div class="col-md-6">
                                        <uc2:Licencia runat="server" ID="LicenciaActual" HabilitarEdicion="true" OnModificoValor="LicenciaActual_ModificoValor" />
                                    </div>
                                </div>

                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </div>

</asp:Content>
