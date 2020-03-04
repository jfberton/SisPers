<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgentesPorAreaUC.ascx.cs" Inherits="SisPer.Aplicativo.Reportes.AgentesPorAreaUC" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<br />
<input type="hidden" id="IdUO" runat="server" value="0" />
<div class="panel panel-default">
    <div class="panel-heading">
        <h3 class="panel-title">Listado de agentes por area</h3>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-4"><asp:DropDownList runat="server" ID="ddl_Areas" CssClass="form-control"></asp:DropDownList></div>
            <div class="col-md-4"><asp:CheckBox ID="chk_IncluyeDependencias" CssClass="form-control" Text="&nbsp; Incluir dependencias" Checked="false" runat="server" /></div>
            <div class="col-md-4"><asp:Button Text="Obtener listado agentes" ID="btn_Obtener" CssClass="btn btn-primary" OnClick="btn_Obtener_Click" runat="server" /></div>
        </div>
    </div>
</div>
