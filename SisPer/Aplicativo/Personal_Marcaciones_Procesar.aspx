<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Personal_Marcaciones_Procesar.aspx.cs" Inherits="SisPer.Aplicativo.Personal_Marcaciones_Procesar" %>

<%@ Register Src="~/Aplicativo/Menues/MenuPersonalAgente.ascx" TagPrefix="uc1" TagName="MenuPersonalAgente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuPersonalJefe.ascx" TagPrefix="uc1" TagName="MenuPersonalJefe" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuPersonalAgente runat="server" ID="MenuPersonalAgente" />
    <uc1:MenuPersonalJefe runat="server" ID="MenuPersonalJefe" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Procesar Marcaciones
        <asp:Label Text="" ID="lbl_dia" runat="server" /></h2>
    <table>
        <tr>
            <td valign="top">
                <div runat="server" id="div_DiaCorrecto" visible="false">
                    <table style="border-style: solid; border-width: thin; border-color: inherit;">
                        <tr>
                            <td align="center">
                                <h3>Perfecto! ya podés cerrar el día.</h3>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Image ID="Image1" ImageUrl="~/Imagenes/ok.png" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="center">

                                <asp:Button Text="Cerrar día" runat="server" ID="btn_CerrarDia" OnClick="btn_CerrarDia_Click" OnClientClick="javascript:if (!confirm('¿Desea CERRAR esta día? Esta acción agendará automáticamente las tardanzas, prolongaciones de jornada y salidas antes de hora que figuran en la grilla de correctos.')) return false;" /></td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="div_DiaIncorrecto" visible="false">
                    <table>
                        <tr>
                            <td>
                                <h3>Inconsistencias Casa Central:
                                    <asp:Label Text="" ID="lbl_Cant_Incons_CC" runat="server" /></h3>
                                <asp:GridView ID="gv_Inconsistencias_CC" runat="server" ForeColor="#717171"
                                    EmptyDataText="Bravo! no existen inconsistencias."
                                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                    OnPageIndexChanging="gv_Inconsistencias_CC_PageIndexChanging"
                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                        <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                        <asp:BoundField DataField="Observaciones" HeaderText="Tipo inconsistencia" ReadOnly="true" SortExpression="Observaciones" />
                                        <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btn_AnalizarInconsistencia" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                    ToolTip='<%#Eval("Observaciones")%>' ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarInconsistencia_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3>Inconsistencias Interior:
                                    <asp:Label Text="" ID="lbl_Cant_Incons_Int" runat="server" /></h3>
                                <asp:GridView ID="gv_Inconsistencias_Int" runat="server" ForeColor="#717171"
                                    EmptyDataText="Bravo! no existen inconsistencias."
                                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                    OnPageIndexChanging="gv_Inconsistencias_Int_PageIndexChanging"
                                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                    <Columns>
                                        <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                        <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                        <asp:BoundField DataField="Observaciones" HeaderText="Tipo inconsistencia" ReadOnly="true" SortExpression="Observaciones" />
                                        <asp:TemplateField HeaderText="Analizar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btn_AnalizarInconsistencia" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                    ToolTip='<%#Eval("Observaciones")%>' ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarInconsistencia_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <h3>Correctos por cerrar:
                            <asp:Label Text="" ID="lbl_Cant_Corr" runat="server" /></h3>
                            <asp:GridView ID="gv_Correcto" runat="server" ForeColor="#717171"
                                EmptyDataText="Ups! ningun registro correcto."
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                OnPageIndexChanging="gv_Correcto_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Entrada" HeaderText="Entrada" ReadOnly="true" SortExpression="Entrada" />
                                    <asp:BoundField DataField="Salida" HeaderText="Salida" ReadOnly="true" SortExpression="Salida" />

                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ReadOnly="true" SortExpression="Observaciones" />

                                    <asp:BoundField DataField="Tardanza" HeaderText="Tardanza" ReadOnly="true" SortExpression="Tardanza" />
                                    <asp:BoundField DataField="ProlJorn" HeaderText="Prol. Jorn" ReadOnly="true" SortExpression="ProlJorn" />
                                    <asp:BoundField DataField="SalidaAntesHora" HeaderText="Salida Antes" ReadOnly="true" SortExpression="SalidaAntesHora" />

                                    <asp:TemplateField HeaderText="Detalle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_Analizar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Ver detalle" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarInconsistencia_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button Text="Exportar excel" runat="server" ID="btn_exportarExcel" OnClick="btn_exportarExcel_Click" />
                            <asp:Button Text="Cerrar todos" runat="server" ID="btn_cerrar_todos_individuales" OnClick="btn_cerrar_todos_individuales_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Cerrados:
                            <asp:Label Text="" ID="lbl_cerrados" runat="server" /></h3>
                            <asp:GridView ID="gv_Cerrados" runat="server" ForeColor="#717171"
                                EmptyDataText="Ups! ningun agente cerrado en el dia."
                                AutoGenerateColumns="False" GridLines="None" AllowPaging="true"
                                OnPageIndexChanging="gv_Cerrados_PageIndexChanging"
                                CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                <Columns>
                                    <asp:BoundField DataField="Legajo" HeaderText="Legajo" ReadOnly="true" SortExpression="Legajo" />
                                    <asp:BoundField DataField="Agente" HeaderText="Agente" ReadOnly="true" SortExpression="Agente" />
                                    <asp:BoundField DataField="Entrada" HeaderText="Entrada" ReadOnly="true" SortExpression="Entrada" />
                                    <asp:BoundField DataField="Salida" HeaderText="Salida" ReadOnly="true" SortExpression="Salida" />

                                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ReadOnly="true" SortExpression="Observaciones" />

                                    <asp:BoundField DataField="Tardanza" HeaderText="Tardanza" ReadOnly="true" SortExpression="Tardanza" />
                                    <asp:BoundField DataField="ProlJorn" HeaderText="Prol. Jorn" ReadOnly="true" SortExpression="ProlJorn" />
                                    <asp:BoundField DataField="SalidaAntesHora" HeaderText="Salida Antes" ReadOnly="true" SortExpression="SalidaAntesHora" />

                                    <asp:TemplateField HeaderText="Detalle" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_Analizar" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                ToolTip="Ver detalle" ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_AnalizarInconsistencia_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
