<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_Ag_Nuevo.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Ag_Nuevo" %>

<%@ Register Src="Controles/Ddl_Areas.ascx" TagName="Ddl_Areas" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc3:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" />
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3>Detalle agente</h3>
        </div>
        <div class="panel-body">
            &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
                CssClass="validationsummary panel panel-danger " HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
            <p />

            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>Datos personales</h4>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_NomYApAgente">Apellido y Nombre</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_NomYApAgente" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el nombre del agente' />"
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe ingresar el nombre del agente"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_NomYApAgente" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_CUIT">CUIT/CUIL</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_CUIT" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la CUIT/CUIL del agente' />"
                                        ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe ingresar la CUIT/CUIL del agente"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_CUIT" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_DNI">DNI</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_DNI" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el DNI del agente' />"
                                        ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe ingresar el DNI del agente"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_DNI" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_Nacimiento">Fecha de nacimiento</label><asp:RequiredFieldValidator ControlToValidate="tb_Nacimiento" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha de nacimiento del agente' />"
                                        ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe ingresar la fecha de nacimiento del agente"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                        ErrorMessage="La fecha ingresada no es correcta"
                                        OnServerValidate="CustomValidator3_ServerValidate"></asp:CustomValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_Nacimiento" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="tb_Nacimiento_CalendarExtender" runat="server" CssClass="alert alert-info"
                                        Enabled="True" TargetControlID="tb_Nacimiento" Format="d">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_Sexo">Sexo</label>
                                </div>
                                <div class="col-md-8">
                                    <div class="col-md-6">
                                        <asp:RadioButton ID="rb_Masculino" Text=" Masculino" Checked="true" CssClass="form-control" runat="server" GroupName="SexoAgente" AutoPostBack="true" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RadioButton ID="rb_Femenino" Text=" Femenino" Checked="true" CssClass="form-control" runat="server" GroupName="SexoAgente" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_Domicilio">Domicilio</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_Domicilio" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el domicilio del agente' />"
                                        ID="rfv_domicilio" runat="server" ErrorMessage="Debe ingresar el domicilio del agente"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_Domicilio" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>Datos de usuario</h4>

                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Usuario</label>
                                    <asp:RequiredFieldValidator ID="Validator_Usr" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar nombre de usuario para la cuenta' />"
                                        ErrorMessage="Debe ingresar nombre de usuario para la cuenta" ForeColor="Red"
                                        ControlToValidate="tb_Login" />
                                    <asp:CustomValidator ID="Validator_Usr_YaExiste" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Ese nombre de usuario ya fue tomado' />"
                                        ErrorMessage="Ese nombre de usuario ya fue tomado" ForeColor="Red" OnServerValidate="Validator_Usr_YaExiste_ServerValidate"
                                        ControlToValidate="tb_Login" />
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="tb_Login" CssClass="form-control" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Perfil</label>
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe seleccionar un perfil' />"
                                        ErrorMessage="Debe seleccionar un perfil" ForeColor="Red" OnServerValidate="ValidarPerfil_ServerValidate"
                                        ControlToValidate="ddl_Perfiles" />
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" Text="<img src='../Imagenes/exclamation.gif' title='Debe estar a cargo del departamento de Personal para realizar este cambio de Perfil' />"
                                        ErrorMessage="Debe estar a cargo del departamento de Personal para realizar este cambio de Perfil" ForeColor="Red" OnServerValidate="ValidarPerfil2_ServerValidate"
                                        ControlToValidate="ddl_Perfiles" />
                                </div>
                                <div class="col-md-8">
                                    <asp:DropDownList runat="server" ID="ddl_Perfiles" CssClass="form-control">
                                        <asp:ListItem Text="Ninguno" />
                                        <asp:ListItem Text="Agente" />
                                        <asp:ListItem Text="Personal" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                </div>
                                <div class="col-md-8">
                                    <asp:Panel runat="server" ID="MostrarHabilitarClavePanel" Visible="false">
                                        <asp:CheckBox runat="server" ID="chk_HabilitarClave" Text="Modificar clave" CssClass="form-control" Checked="false"
                                            OnCheckedChanged="chk_HabilitarClave_CheckedChanged" AutoPostBack="True" />
                                    </asp:Panel>

                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Panel runat="server" ID="MostrarCamposClave" Visible="true">
                                        <div class="alert alert-warning">La clave se generará al aceptar los cambios.</div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4>Datos laborales</h4>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_legajo">Legajo</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_legajo" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el legajo del agente' />"
                                        ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe ingresar el legajo del agente"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ControlToValidate="tb_legajo" Text="<img src='../Imagenes/exclamation.gif' title='El legajo debe ser un numero entero sin puntos ni comas.' />"
                                        ID="cv_Legajo" runat="server" ErrorMessage="El legajo debe ser un numero entero sin puntos ni comas." OnServerValidate="cv_Legajo_ServerValidate"></asp:CustomValidator>
                                    <asp:CustomValidator ControlToValidate="tb_legajo" Text="<img src='../Imagenes/exclamation.gif' title='No se puede dar de alta un agente con legajo existente consulte con sistemas.' />"
                                        ID="cv_legajo1" runat="server" ErrorMessage="No se puede dar de alta un agente con legajo existente consulte con sistemas." OnServerValidate="cv_legajo1_ServerValidate"></asp:CustomValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_legajo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_Mail">E-mail</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_Mail" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el mail oficial de comunicacion con el agente' />"
                                        ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe ingresar el mail oficial de comunicacion con el agente"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_Mail"
                                        Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar un mail válido.' />"
                                        ErrorMessage="Debe ingresar un mail válido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_Mail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="tb_IngresoAdmPub">Fecha de ingreso Adm. Púb.</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_IngresoAdmPub" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha de ingreso a la administración pública' />"
                                        ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe ingresar la fecha de ingreso a la administración pública"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                        ErrorMessage="La fecha ingresada no es correcta"
                                        OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_IngresoAdmPub" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" Format="d" runat="server" CssClass="alert alert-info"
                                        Enabled="True" TargetControlID="tb_IngresoAdmPub">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Fecha de ingreso A.T.P.</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_IngresoAPlanta" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha de ingreso a planta permanente' />"
                                        ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe ingresar la fecha de ingreso a planta permanente"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator4" runat="server"
                                        Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                        ErrorMessage="La fecha ingresada no es correcta"
                                        OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="tb_IngresoAPlanta" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="tb_IngresoAPlanta_CalendarExtender" Format="d" runat="server" CssClass="alert alert-info"
                                        Enabled="True" TargetControlID="tb_IngresoAPlanta">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Antiguedad reconocida en otras partes</label>
                                </div>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label for="">Años</label>
                                            <asp:RequiredFieldValidator ControlToValidate="tb_AntiguedadAnios" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar los años de antiguedad' />"
                                                ID="RequiredFieldValidator12" runat="server" ErrorMessage="Debe ingresar los años de antiguedad"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="tb_AntiguedadAnios_RegularExpressionValidator" OnServerValidate="tb_AntiguedadAnios_RegularExpressionValidator_ServerValidate" runat="server"
                                                Text="<img src='../Imagenes/exclamation.gif' title='Ingrese unicamente numeros enteros' />"
                                                ControlToValidate="tb_AntiguedadAnios"
                                                ErrorMessage="Ingrese unicamente numeros enteros"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_AntiguedadAnios" />

                                        </div>
                                        <div class="col-md-2">
                                            <label for="">Meses</label>
                                            <asp:RequiredFieldValidator ControlToValidate="tb_AntiguedadMeses" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar los meses de antiguedad' />"
                                                ID="RequiredFieldValidator13" runat="server" ErrorMessage="Debe ingresar los meses de antiguedad"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="tb_AntiguedadMesesRegularExpressionValidator" OnServerValidate="tb_AntiguedadMesesRegularExpressionValidator_ServerValidate" runat="server"
                                                Text="<img src='../Imagenes/exclamation.gif' title='Ingrese unicamente numeros enteros' />"
                                                ControlToValidate="tb_AntiguedadMeses"
                                                ErrorMessage="Ingrese unicamente numeros enteros"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_AntiguedadMeses" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Ficha médica</label>
                                    <asp:RequiredFieldValidator ControlToValidate="tb_FichaMedica" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la ficha medica' />"
                                        ID="RequiredFieldValidator11" runat="server" ErrorMessage="Debe ingresar la ficha medica"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="tb_FichaMedica" CssClass="form-control" />
                                </div>
                            </div>
                            <br />

                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Horario laboral</label>
                                </div>
                                <div class="col-md-3">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="tb_HoraDesde">Desde</label>
                                            <asp:RequiredFieldValidator ControlToValidate="tb_HoraDesde" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el horario de entrada del agente' />"
                                                ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe ingresar el horario de entrada del agente"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="MovimientoHora"
                                                Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                ControlToValidate="tb_HoraDesde" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                                ErrorMessage="La hora ingresada no es correcta xxx:xx"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tb_HoraDesde" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label for="">Hasta</label>
                                            <asp:RequiredFieldValidator ControlToValidate="tb_HoraHasta" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar el horario de salida del agente' />"
                                                ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe ingresar el horario de salida del agente"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="MovimientoHora"
                                                Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                                                ControlToValidate="tb_HoraDesde" ValidationExpression="(([0-9]|2[0-3]):[0-5][0-9])|([0-1][0-9]|2[0-3]):[0-5][0-9]"
                                                ErrorMessage="La hora ingresada no es correcta xxx:xx"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="tb_HoraHasta" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Horario flexible</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:CheckBox ID="chk_HorarioFlexible" CssClass="form-control" runat="server" />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-4">
                                    <label for="">Area donde se encuentra</label>
                                    <asp:CustomValidator ID="cv_area" OnServerValidate="cv_area_ServerValidate" runat="server"
                                                Text="<img src='../Imagenes/exclamation.gif' title='Seleccione el area donde prestará servicios el agente' />"
                                                ErrorMessage="Seleccione el area donde prestará servicios el agente"></asp:CustomValidator>
                                </div>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <uc1:Ddl_Areas ID="Ddl_Areas1" runat="server" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <asp:RadioButton ID="chk_Agente" Text="Agente" Checked="true" CssClass="form-control" runat="server" GroupName="TipoAgente" AutoPostBack="true" OnCheckedChanged="chk_JefeTemporal_CheckedChanged" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RadioButton ID="chk_Jefe" runat="server" Text="Jefe" CssClass="form-control" GroupName="TipoAgente" AutoPostBack="true" OnCheckedChanged="chk_JefeTemporal_CheckedChanged" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RadioButton ID="chk_JefeTemporal" runat="server" CssClass="form-control" GroupName="TipoAgente" Text="Jefe temporal" AutoPostBack="true" OnCheckedChanged="chk_JefeTemporal_CheckedChanged" />
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label for="">
                                                <asp:Label Text="Jefe temporal hasta" runat="server" ID="lbl_JefeHasta" Visible="false" />
                                            </label>
                                            <asp:CustomValidator Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la fecha hasta la cual el agente se desempeña como jefe temporal' />"
                                                ID="CV_JefeTemporalFecha" runat="server" OnServerValidate="CV_JefeTemporalFecha_ServerValidate" ErrorMessage="Debe ingresar la fecha hasta la cual el agente se desempeña como jefe temporal"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CV_JefeTemporal" runat="server"
                                                Text="<img src='../Imagenes/exclamation.gif' title='La fecha ingresada no es correcta' />"
                                                ErrorMessage="La fecha ingresada no es correcta"
                                                OnServerValidate="CV_JefeTemporal_ServerValidate"></asp:CustomValidator>
                                        </div>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="tb_JefetemporalHasta" Visible="false" />
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="alert alert-info"
                                                Enabled="True" TargetControlID="tb_JefetemporalHasta" Format="d">
                                            </asp:CalendarExtender>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btn_Aceptar" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btn_Aceptar_Click" />
                    <asp:Button ID="btn_Cancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" CausesValidation="false" OnClick="btn_Cancelar_Click" />
                </div>
            </div>
        </div>
    </div>

    <br />

</asp:Content>
