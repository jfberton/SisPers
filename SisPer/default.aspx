<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SisPer._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/customBoostrap.css" rel="stylesheet" />
    <link href="css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="css/signincss.css" rel="stylesheet" />
    <link href="css/sticky-footer.css" rel="stylesheet" />


    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="assets/js/ie-emulation-modes-warning.js"></script>
    <script src="js/jquery-1.11.2.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="assets/js/docs.min.js"></script>
    <script src="js/bootstrap-datetimepicker.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="assets/js/ie10-viewport-bug-workaround.js"></script>

    <title>Sistema Personal</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <nav class="navbar navbar-inverse navbar-fixed-top">
                    <div class="container">
                        <div class="navbar-header">
                            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                                <span class="sr-only">Toggle navigation</span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                            </button>
                            <a class="navbar-brand" href="default.aspx">Sistema Personal <span class="glyphicon glyphicon-home small" data-toggle="tooltip" data-placement="left" title="Pantalla principal"></span></a>
                        </div>
                        <div id="navbar" class="collapse navbar-collapse">
                            <ul class="nav navbar-nav">
                                <li><a href="Legislacion.aspx">Legislación</a></li>
                                <li><a href="#">Acerca de</a></li>
                            </ul>
                        </div>
                        <!--/.nav-collapse -->
                    </div>
                </nav>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="container">
                    <asp:Panel runat="server" ID="p_EnMantenimiento" Visible="false" Width="100%" BackColor="White">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Image ImageUrl="~/Imagenes/Mantenimiento.png" runat="server" /></td>
                            </tr>
                        </table>

                    </asp:Panel>

                    <asp:Panel runat="server" ID="p_Normal">
                        <div class="jumbotron">
                            <h1>Bienvenido al sistema de personal
                <p class="lead">
                    El sistema tiene como propósito el servir como medio de integración y orientación al agente de esta administración permitiendo realizar solicitudes y consultas de estado sobre horas acumuladas, licencias, francos, horarios vespertinos.
También consultar legislación vigente: Leyes, Decretos, Circulares, Formularios
                </p>
                                <h1></h1>
                            </h1>
                        </div>


                        <asp:HiddenField ID="hidTAB" runat="server" Value="home" />

                        <div>
                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs hidden" role="tablist" id="myTabs">
                                <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab"></a></li>
                                <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab"></a></li>
                            </ul>

                            <!-- Tab panes -->
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane fade in active" id="home">
                                    <div style="width: 350px; margin: 0 auto;">
                                        <form class="form-signin">
                                            <h2 class="form-signin-heading">Ingrese sus credenciales</h2>
                                            <input type="text" id="tb_Usuario" placeholder="Usuario" runat="server" class="form-control" autofocus>
                                            <div class="checkbox"></div>
                                            <input type="password" id="tb_Contraseña" placeholder="Contraseña" runat="server" class="form-control">
                                            <div class="checkbox"></div>
                                            <asp:Button Text="Ingresar" class="btn btn-lg btn-primary btn-block" OnClick="IngresarButtton_Click" runat="server" />
                                            <asp:Button Text="Cancelar" class="btn btn-lg btn-danger btn-block" OnClick="CancelarButtton_Click" ID="btn_cancelar_dev" Visible="false" runat="server" />
                                        </form>

                                        <a href="#" id="olvidemiclave">olvide mi clave</a>
                                    </div>
                                </div>
                                
                                <div role="tabpanel" class="tab-pane fade" id="profile">
                                    <div style="width: 350px; margin: 0 auto;">

                                        <h2 class="form-signin-heading">Recuperar clave</h2>
                                        <p class="alert alert-danger" runat="server" id="lbl_noexistecorreo" visible="false">
                                            El correo especificado no se encuentra entre la lista de correos válidos! Diríjase a Personal para solicitar una nueva clave.
                                        </p>
                                        <p class="alert alert-success" runat="server" id="lbl_se_envio_mail" visible="false">
                                            Se envió un correo electrónico para reestablecer la contraseña.
                                        </p>
                                        <input type="text" id="tb_mail" placeholder="correo electrónico" runat="server" class="form-control" autofocus>
                                        <div class="checkbox"></div>
                                        <button class="btn btn-lg btn-primary btn-block" type="button" id="btn_enviar_solicitud" runat="server" onserverclick="btn_enviar_solicitud_ServerClick">Recuperar clave!</button>


                                        <a href="#" id="recordemiclave">Volver</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="p_Ambiente_prueba">
                        <div class="jumbotron">
                            <h1>Ambiente de pruebas
                <p class="lead">
                    El ambiente de pruebas se uliliza <b><u>únicamente a los fines de probar y verificar funcionalidades</u></b>, antes de que se implementen en el sistema real.
                    <b><u>La información que aquí se muestra carece de validez oficial más que para el fin establecido.</u></b>
                </p>
                            </h1>
                        </div>

                        <div class="input-group">
                            <span class="input-group-addon">Buscar</span>
                            <input name="txtTerm" class="form-control" onkeyup="filter2(this, '<%=gv_Agentes.ClientID %>')" placeholder="ingrese texto buscado" type="text">
                        </div>
                        <div style="height: 400px; overflow-y: scroll; margin: 0 auto;">
                            <asp:GridView ID="gv_Agentes" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                                AutoGenerateColumns="False" GridLines="None" CssClass="mGrid table-condensed" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" />
                                    <asp:BoundField DataField="Area" HeaderText="Area" />
                                    <asp:TemplateField HeaderText="Ingresar" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/Imagenes/bullet_go.png" ID="btn_ingresar" runat="server" CommandArgument='<%#Convert.ToInt32(Eval("IdAgente")) %>' OnClick="btn_ingresar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="row">
            <footer class="footer">
                <table class="footerMaster">
                    <tr>
                        <td><small>Existen
                                <asp:Label Text="" ID="lbl_usuarios_logueados" runat="server" />
                            usuarios conectados</small></td>
                        <td>&copy; DIRECCIÓN DE INFORMÁTICA - Departamento de desarrollo y administración de datos - 2013 -
                        <asp:Label Text="" ID="lbl_AñoActual" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton Text="Admin" CssClass="small" runat="server" ID="btn_modo_dev" OnClick="btn_modo_dev_Click" />
                        </td>
                    </tr>
                </table>
            </footer>
        </div>
    </form>



    <script>
        function filter2(phrase, _id) {
            var words = phrase.value.toLowerCase().split(" ");
            var table = document.getElementById(_id);
            var ele;
            for (var r = 1; r < table.rows.length; r++) {
                ele = table.rows[r].innerHTML.replace(/<[^>]+>/g, "");
                var displayStyle = 'none';
                for (var i = 0; i < words.length; i++) {
                    if (ele.toLowerCase().indexOf(words[i]) >= 0)
                        displayStyle = '';
                    else {
                        displayStyle = 'none';
                        break;
                    }
                }
                table.rows[r].style.display = displayStyle;
            }
        }

        $(document).ready(function () {
            // for bootstrap 3 use 'shown.bs.tab', for bootstrap 2 use 'shown' in the next line
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                // save the latest tab; use cookies if you like 'em better:
                localStorage.setItem('lastTab', $(this).attr('href'));
            });

            // go to the latest tab, if it exists:
            var lastTab = localStorage.getItem('lastTab');
            if (lastTab) {
                $('[href="' + lastTab + '"]').tab('show');
            }
        });

        $('#olvidemiclave').click(function () {
            $('#myTabs a[href="#profile"]').tab('show');
        });

        $('#recordemiclave').click(function () {
            $('#myTabs a[href="#home"]').tab('show');
        });
    </script>

</body>
</html>
