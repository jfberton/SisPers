<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SU_CalculoHoras.aspx.cs" Inherits="SisPer.Aplicativo.SU_CalculoHoras" %>

<%@ Register Src="~/Aplicativo/Menues/MenuSU.ascx" TagPrefix="uc1" TagName="MenuSU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuSU runat="server" ID="MenuSU" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Prueba de cálculo de horas</h2>
    <div class="row">
        <div class="col-md-3">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3>Sumar horas</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_sum_primer_termino" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_sum_segundo_termino" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button Text="Calcular" runat="server" CssClass="btn btn-block btn-default" id="btn_sumar" OnClick="btn_sumar_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="well well-sm">
                                <asp:Label Text="" ID="lb_resutado_suma" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3>Restar horas</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_rest_primer_termino" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox runat="server" CssClass="form-control" ID="tb_rest_segundo_termino" />
                        </div>
                    </div>
                     <div class="row">
                        <div class="col-md-12">
                            <asp:Button Text="Calcular" runat="server" CssClass="btn btn-block btn-default" id="btn_restar" OnClick="btn_restar_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="well well-sm">
                                <asp:Label Text="" ID="lb_resultado_resta" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Button Text="REcalcular dias licencia anual otorgados" runat="server" ID="btn_recalcular_licencias" OnClick="btn_recalcular_licencias_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentScripts" runat="server">
</asp:Content>
