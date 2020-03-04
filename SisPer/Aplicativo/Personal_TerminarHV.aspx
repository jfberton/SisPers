<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Personal_TerminarHV.aspx.cs" Inherits="SisPer.Aplicativo.Personal_TerminarHV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Menues/MenuPersonalJefe.ascx" TagName="MenuPersonalJefe" TagPrefix="uc1" %>
<%@ Register Src="Menues/MenuPersonalAgente.ascx" TagName="MenuPersonalAgente" TagPrefix="uc2" %>
<%@ Register Src="Menues/MenuJefe.ascx" TagPrefix="uc3" TagName="MenuJefe" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc2:MenuPersonalAgente ID="MenuPersonalAgente1" runat="server" Visible="false" />
    <uc1:MenuPersonalJefe ID="MenuPersonalJefe1" runat="server" Visible="false" />
    <uc3:MenuJefe runat="server" ID="MenuJefe" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.combobox.js" type="text/javascript"></script>

    <h2>Terminar horario vespertino</h2>
    <h3>Datos horario vespertino</h3>

    <table>
        <tr>
            <td>Agente
            </td>
            <td>
                <asp:Label Text="" ID="lbl_Agente" runat="server" />
            </td>
            <td>Día
            </td>
            <td>
                <asp:Label Text="" ID="lbl_Dia" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Hora desde
            </td>
            <td>
                <asp:Label Text="" ID="lbl_HoraDesde" runat="server" />
            </td>
            <td>Hora hasta
            </td>
            <td>
                <asp:Label Text="" ID="lbl_HoraHasta" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Motivo
            </td>
            <td colspan="3">
                <asp:Label Text="" ID="lbl_Descripcion" runat="server" />
            </td>
        </tr>
    </table>
    <h3>Datos reales horario vespertino</h3>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        ValidationGroup="HV" CssClass="validationsummary panel panel-danger" HeaderText="<div class='panel-heading'>&nbsp;Corrija los siguientes errores antes de continuar:</div>" />
    <table>
        <tr>
            <td>Hora marcada desde
            </td>
            <td>
                <editable:EditableDropDownList ID="tb_HoraDesde" CssClass="editDDL" runat="server" 
                    Sorted="true" AutoselectFirstItem="true" Font-Size="30px">
                </editable:EditableDropDownList>
                <asp:CustomValidator 
                    ID="CustomFieldValidator3" runat="server" ValidationGroup="HV"
                    Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora inicial' />"
                    ErrorMessage="Debe ingresar la hora inicial" OnServerValidate="CustomFieldValidator3_ServerValidate"/>
               <asp:CustomValidator 
                    ID="CustomValidator6" runat="server" ValidationGroup="HV"
                    Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                    ErrorMessage="La hora ingresada no es correcta xx:xx" OnServerValidate="CustomValidator6_ServerValidate"/>
               
            </td>
            <td>Hora marcada hasta
            </td>
            <td>
                <editable:EditableDropDownList ID="tb_HoraHasta" Class="editableDDL" runat="server" 
                    Sorted="true" AutoselectFirstItem="true">
                </editable:EditableDropDownList>

                <asp:CustomValidator ID="CustomValidator5" runat="server" ValidationGroup="HV"
                    ControlToValidate="tb_HoraHasta" Text="<img src='../Imagenes/exclamation.gif' title='Debe ingresar la hora final' />"
                    ErrorMessage="Debe ingresar la hora final" OnServerValidate="CustomValidator5_ServerValidate">
                </asp:CustomValidator>
                <asp:CustomValidator 
                    ID="CustomValidator1" runat="server" ValidationGroup="HV"
                    Text="<img src='../Imagenes/exclamation.gif' title='La hora ingresada no es correcta xx:xx' />"
                    ErrorMessage="La hora ingresada no es correcta xx:xx" OnServerValidate="CustomValidator7_ServerValidate"/>
                <asp:CustomValidator ID="CustomValidator4" ValidationGroup="HV" Text="<img src='../Imagenes/exclamation.gif' title='La hora final debe ser mayor o igual a la hora inicial' />"
                    runat="server" ErrorMessage="La hora final debe ser mayor o igual a la hora inicial"
                    OnServerValidate="CustomValidator4_ServerValidate">
                </asp:CustomValidator>
            </td>
        </tr>
    </table>
    <p />
    <h2>
        <asp:Label Text="" ID="lblInterior" runat="server" BorderWidth="1" BorderColor="Black" BackColor="#CC9900" /></h2>
    <asp:Button ID="btn_Terminar" runat="server" Text="Terminar" OnClick="btn_Terminar_Click" />
    <asp:Button ID="btn_Rechazar" runat="server" Text="Rechazar"
        OnClick="btn_Rechazar_Click" />
    <asp:Button ID="btn_Volver" runat="server" Text="Volver" OnClick="btn_Volver_Click" />
</asp:Content>
