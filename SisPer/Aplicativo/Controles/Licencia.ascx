<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Licencia.ascx.cs" Inherits="SisPer.Aplicativo.Controles.Licencia" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h1 class="panel-title">
            <asp:Label Text="" ID="lbl_Tipo" runat="server" /></h1>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-6">
                Días otorgados
            </div>
            <div class="col-md-6">
                <div class="row">
                    <div class="col-md-10">
                        <div class="input-group">
                            <asp:Label ID="lbl_DiasOtorgados" CssClass="form-control" Text="0" runat="server" />
                            <asp:TextBox runat="server" ID="tb_DiasOtorgados" CssClass="form-control" Text="0" Visible="false" />
                            <span class="input-group-btn">
                                <button type="button" runat="server" id="btn_Edit_DiasOtorgados" visible="false" onserverclick="btn_Edit_DiasOtorgados_Click" class="btn btn-warning">
                                    <span class="glyphicon glyphicon-pencil" />
                                </button>
                                <button type="button" runat="server" id="btn_Accept_DiasOtorgados" onserverclick="btn_Accept_DiasOtorgados_Click" class="btn btn-success" visible="false">
                                    <span class="glyphicon glyphicon-ok" />
                                </button>
                                <button type="button" runat="server" id="btn_Cancel_DiasOtorgados" data-toggle="tooltip" data-placement="left" title="Editar" onserverclick="btn_Cancel_DiasOtorgados_Click" class="btn btn-danger" visible="false">
                                    <span class="glyphicon glyphicon-remove" />
                                </button>
                            </span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:CustomValidator ID="cv_DiasOtorgados" Width="30" runat="server" ErrorMessage="El valor ingresado debe ser numérico"
                            ValidationGroup="DiasOtorgados" Text="<img src='../Imagenes/exclamation.gif' title='El valor ingresado debe ser numérico' />"
                            OnServerValidate="cv_DiasOtorgados_ServerValidate"></asp:CustomValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">Días usufructuados</div>
            <div class="col-md-6">
                <div class="row">
                    <div class="col-md-10">
                        <div class="input-group">
                            <asp:Label ID="lbl_DiasUsufructuados" CssClass="form-control" Text="0" runat="server"/>
                            <asp:TextBox runat="server" ID="tb_DiasUsufructuados" CssClass="form-control" Text="0" Visible="false" />
                            <span class="input-group-btn">
                                <button type="button" runat="server" id="btn_Edit_DiasUsufructuados" visible="false" onserverclick="btn_Edit_DiasUsufructuados_Click" class="btn btn-warning">
                                    <span class="glyphicon glyphicon-pencil" />
                                </button>
                                <button type="button" runat="server" id="btn_Accept_DiasUsufructuados" onserverclick="btn_Accept_DiasUsufructuados_Click" class="btn btn-success" visible="false">
                                    <span class="glyphicon glyphicon-ok" />
                                </button>
                                <button type="button" runat="server" id="btn_Cancel_DiasUsufructuados" data-toggle="tooltip" data-placement="left" title="Editar" onserverclick="btn_Cancel_DiasUsufructuados_Click" class="btn btn-danger" visible="false">
                                    <span class="glyphicon glyphicon-remove" />
                                </button>
                            </span>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:CustomValidator ID="cv_DiasUsufructuados" Width="30" runat="server" ErrorMessage="El valor ingresado debe ser numérico"
                            ValidationGroup="DiasUsufructuados" Text="<img src='../Imagenes/exclamation.gif' title='El valor ingresado debe ser numérico' />"
                            OnServerValidate="cv_DiasUsufructuados_ServerValidate"></asp:CustomValidator>
                    </div>
                </div>
            </div>
        </div>


    </div>
    <div class="panel-footer">
        <div class="row">
            <div class="col-md-6">
                <label>Saldo</label>
            </div>
            <div class="col-md-6">
                <label>
                    <asp:Label Text="0" ID="lbl_Saldo" runat="server" />
                    días</label>
            </div>
        </div>
    </div>
</div>

