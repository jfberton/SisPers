<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Ag_Memo17.aspx.cs" Inherits="SisPer.Aplicativo.Ag_Memo17" %>

<%@ Register Src="~/Aplicativo/Menues/MenuAgente.ascx" TagPrefix="uc1" TagName="MenuAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuJefe.ascx" TagPrefix="uc1" TagName="MenuJefe" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuAgente runat="server" ID="MenuAgente" Visible="false" />
    <uc1:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" Visible="false" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <br />
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title">
                        <a role="button" data-toggle="collapse" href="#memo" aria-expanded="true" aria-controls="collapseOne">MEMORANDUM N° 17
                        </a>
                    </h4>
                </div>
                <div id="memo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        DEL:ADMINISTRADOR GENERAL<br />
                        A: TODO EL PERSONAL DEL ORGANISMO<br />
                        <hr />
                        &nbsp;&nbsp;&nbsp;&nbsp;Visto la prioximidad del nuevo ejercicio 2017 y a fin de actualizar la base de datos en el Sistema de Gestión Personal y Sistema PON, se solicita a todo el personal de esta Organismo completar los datos requeridos a través del FORMULARIO N° 1384 -adjunto al presente- en carácter de Declaración Jurada, para presentarlo antes del 19/12/16 en el Departamento Personal.<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;El mismo será agregado al legajo individual de cada agente tomándose dicha información como actualizada y veraz para todo tipo de trámite o pedido de informe.<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;Atentamente.<br />
                        <br />
                        ADMINISTRACIÓN TRIBUTARIA PROVINCIAL<br />
                        DEPARTAMENTO PERSONAL
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingTwo">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" href="#datos_personales" aria-expanded="true" aria-controls="collapseTwo">DECLARACIÓN JURADA
                        </a>
                    </h4>
                </div>
                <div id="datos_personales" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                    <div class="panel-body">
                        <h3 class="text-center"><u>Datos Personales</u></h3>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Apellido y nombre
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="tb_dp_apellido_y_nombre" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Nacionalidad
                            </div>
                            <div class="col-md-5">
                                <asp:TextBox runat="server" ID="tb_dp_nacionalidad" CssClass="form-control" />
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddl_dp_nativo_naturalizado" CssClass="form-control">
                                    <asp:ListItem Text="nativo" Value="nativo" />
                                    <asp:ListItem Text="naturalizado" Value="naturalizado" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Nacido en 
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_nacimiento_lugar" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                fecha
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div id="dtp_fecha_nacimiento" class="input-group date">
                                        <input type="text" runat="server" id="tb_dp_nacimiento_fecha" class="form-control" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                Clase
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox runat="server" ID="tb_dp_clase" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                D.N.I.
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="tb_dp_dni" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                Estado civil
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_dp_estado_civil" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Domicilio
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_domicilio_direccion" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Barrio
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_domicilio_barrio" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Localidad
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_dp_domicilio_localidad" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Provincia
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="tb_dp_domicilio_provincia" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                C.P.
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="tb_dp_domicilio_codpost" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Profesión
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_profesion" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Instrucción
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_instruccion" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Nombre del cónyugue
                            </div>
                            <div class="col-md-5">
                                <asp:TextBox runat="server" ID="tb_dp_conyugue_apellido_y_nombre" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Nacionalidad
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_dp_conyugue_nacionalidad" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Nombre y Ap. Padre
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_padre_apellido_y_nombre" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Nacionalidad
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="tb_dp_padre_nacionalidad" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <div class="row">
                                    <div class="col-md-3">
                                        Vive
                                    </div>
                                    <div class="col-md-9">
                                        <asp:DropDownList runat="server" ID="ddl_dp_padre_vive" CssClass="form-control">
                                            <asp:ListItem Text="Si" Value="Si" />
                                            <asp:ListItem Text="No" Value="No" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Nombre y Ap. Madre
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="tb_dp_madre_apellido_y_nombre" CssClass="form-control" />
                            </div>
                            <div class="col-md-1">
                                Nacionalidad
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox runat="server" ID="tb_dp_madre_nacionalidad" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <div class="row">
                                    <div class="col-md-3">
                                        Vive
                                    </div>
                                    <div class="col-md-9">
                                        <asp:DropDownList runat="server" ID="ddl_dp_madre_vive" CssClass="form-control">
                                            <asp:ListItem Text="Si" Value="Si" />
                                            <asp:ListItem Text="No" Value="No" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-11">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h1 class="panel-title">Hijos</h1>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class="col-md-11">
                                                        <asp:GridView ID="gv_hijos" runat="server" EmptyDataText="No existen hijos cargados." ForeColor="#717171"
                                                            AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                                            <Columns>
                                                                <asp:BoundField DataField="dp_hijo_apellido_y_nombre" HeaderText="Apellido y nombre" ReadOnly="true" SortExpression="nombre" />
                                                                <asp:BoundField DataField="dp_hijo_fecha_nacimiento" HeaderText="Natalicio" ReadOnly="true" SortExpression="nacimiento" DataFormatString="{0:d}" />
                                                                <asp:TemplateField HeaderText="Ver">
                                                                    <ItemTemplate>
                                                                        <button
                                                                            type="button"
                                                                            data-toggle="modal"
                                                                            data-target="#ver_hijo"
                                                                            data-nombre='<%#Eval("dp_hijo_apellido_y_nombre")%>'
                                                                            data-nacimiento='<%#Eval("dp_hijo_fecha_nacimiento")%>'>
                                                                            Ver
                                                                        </button>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quitar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btn_eliminar_hijo" runat="server" CommandArgument='<%#Eval("dp_hijo_apellido_y_nombre")%>'
                                                                            ImageUrl="~/Imagenes/delete.png" OnClick="btn_eliminar_hijo_Click" OnClientClick="javascript:if (!confirm('¿Desea eliminar este registro?')) return false;" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                <div class="modal fade" id="ver_hijo" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Datos hijo</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        Nombre y apellido
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <input type="text" id="lbl_nombre_hijo" readonly="readonly" class="form-control" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        Fecha de nacimiento
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <input type="text" id="lbl_fecha_nacimiento_hijo" readonly="readonly" class="form-control" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <button type="button" class="btn btn-warning btn-sm" id="btn_agregar_hijo" runat="server" onserverclick="btn_agregar_hijo_ServerClick">
                                                    Agregar hijo
                                                </button>

                                                <div class="modal fade" id="agregar_hijo" role="dialog" aria-hidden="true">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                                                <h4 class="modal-title">Agregar hijo</h4>
                                                            </div>
                                                            <div class="modal-body">
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        Nombre y apellido
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <input type="text" id="tb_nombre_hijo" class="form-control" runat="server" placeholder="Ingrese nombre y apellido del hijo" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row">
                                                                    <div class="col-md-4">
                                                                        Fecha de nacimiento
                                                                    </div>
                                                                    <div class="col-md-8">
                                                                        <div class="form-group">
                                                                            <div id="dtp_fecha_nacimiento_hijo" class="input-group date">
                                                                                <input id="tb_fecha_nacimiento_hijo" runat="server" class="form-control" type="text" />
                                                                                <span class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <asp:Button Text="Aceptar" ID="btn_aceptar_hijo" OnClick="btn_aceptar_hijo_Click" CssClass="btn btn-success" runat="server" />
                                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <h3 class="text-center"><u>Antecedentes administrativos</u></h3>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Cargo
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="tb_aa_cargo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-2">
                                Función que desempeña
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="tb_aa_funcion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3">
                                Fecha de ingreso (Contrato de obra)
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div id="dtp_aa_contrato_obra_ingreso" class="input-group date">
                                        <input id="tb_aa_contrato_obra_ingreso" runat="server" class="form-control" type="text" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                Inst. Legal y N°
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_aa_contrato_obra_instrumento" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                Fecha de ingreso (Contrato de servicio)
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div id="dtp_aa_contrato_serv_ingreso" class="input-group date">
                                        <input id="tb_aa_contrato_serv_ingreso" runat="server" class="form-control" type="text" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                Inst. Legal y N°
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_aa_contrato_serv_instrumento" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                Fecha de nombramiento
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div id="dtp_aa_nombramiento_ingreso" class="input-group date">
                                        <input id="tb_aa_nombramiento_ingreso" runat="server" class="form-control" type="text" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                Inst. Legal y N°
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="tb_aa_nombramiento_instrumento" CssClass="form-control" />
                            </div>
                        </div>

                        <h3 class="text-center"><u>Cargos desempeñados anteriormente</u></h3>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                En la Administración Nacional
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="tb_ca_en_admin_nacional" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                En la Administración Provincial
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="tb_ca_en_admin_provincial" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                Privados
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="tb_ca_privados" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                            </div>
                        </div>

                        <h3 class="text-center"><u>Otros antecedentes</u></h3>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ID="tb_ca_otros_antecedentes" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingFive">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" href="#otros_antecedentes" aria-expanded="true" aria-controls="collapseThree">Observaciones
                        </a>
                    </h4>
                </div>
                <div id="otros_antecedentes" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingThree">
                    <div class="panel-body">
                        <p>
                            EN CASO DE ALTAS BAJAS O MODIFICACION DE DATOS, EL AGENTE DEBERÁ COMUNICAR LA NOVEDAD POR ESCRITO A TRAVÉZ DE UNA NUEVA DECLARACIÓN JURADA DE DATOS PERSONALES, ENTE ESTE DEPARTAMENTO PERSONAL.-
                        </p>
                        <br />
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <button class="btn btn-primary btn-lg" id="btn_guardar_e_imprimir" onclick="javascript:if (!confirm('Al guardar se reemplazarán los datos anteriormente guardados\rDesea continuar?')) return false;" runat="server" onserverclick="btn_guardar_e_imprimir_ServerClick">
                                    <span class="glyphicon glyphicon-floppy-disk"></span>
                                    <span class="glyphicon glyphicon-print"></span>
                                    &nbsp;Guardar e imprimir
                                </button>

                                <button class="btn btn-primary btn-lg" id="btn_imprimir_solamente" title="Imprime la ultima DDJJ guardada" runat="server" onserverclick="btn_imprimir_solamente_ServerClick">
                                    <span class="glyphicon glyphicon-print"></span>
                                    &nbsp;Imprimir solamente
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
    <script>
        $(function () {
            $('#dtp_fecha_nacimiento').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_fecha_nacimiento_hijo').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_aa_contrato_obra_ingreso').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_aa_contrato_serv_ingreso').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
            $('#dtp_aa_nombramiento_ingreso').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });

        $('#ver_hijo').on('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = $(event.relatedTarget)

            // Extract info from data-* attributes
            var nombre = button.data('nombre')
            var dni = button.data('dni')
            var nacimiento = button.data('nacimiento')
            var observaciones = button.data('observaciones')

            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)
            modal.find('.modal-body #lbl_nombre_hijo').val(nombre)
            modal.find('.modal-body #lbl_fecha_nacimiento_hijo').val(nacimiento.substring(0, 11))
        })
    </script>
</asp:Content>
