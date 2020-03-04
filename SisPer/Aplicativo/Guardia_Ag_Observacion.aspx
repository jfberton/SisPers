<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Guardia_Ag_Observacion.aspx.cs" Inherits="SisPer.Aplicativo.Guardia_Ag_Observacion" %>

<%@ Register Src="~/Aplicativo/Controles/DatosAgente.ascx" TagPrefix="uc1" TagName="DatosAgente" %>
<%@ Register Src="~/Aplicativo/Controles/Card_Agente.ascx" TagPrefix="uc1" TagName="Card_Agente" %>
<%@ Register Src="~/Aplicativo/Menues/MenuGuardia.ascx" TagPrefix="uc1" TagName="MenuGuardia" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenu" runat="server">
    <uc1:MenuGuardia runat="server" ID="MenuGuardia" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:DatosAgente runat="server" ID="DatosAgente1" />
    <table>
        <tr>
            <td style="vertical-align:top;">
                <h3>Observaciones realizadas</h3>
                <asp:GridView ID="GridView1" runat="server" EmptyDataText="No existen registros para mostrar." ForeColor="#717171"
                    AutoGenerateColumns="False" GridLines="None" AllowPaging="true" OnPageIndexChanging="gridView_PageIndexChanging"
                    CssClass="mGrid table-condensed" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" ReadOnly="true" SortExpression="Fecha" DataFormatString="{0:D}" />
                        <asp:TemplateField HeaderText="Ver" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btn_Ver" runat="server" CommandArgument='<%#Eval("Id")%>'
                                    ImageUrl="~/Imagenes/bullet_go.png" OnClick="btn_Ver_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </td>
           <td style="vertical-align: top;">
               <h3>Observación</h3>
               <table>
                   <tr>
                       <td>
                           <asp:TextBox Height="200" Width="500" runat="server" ID="tb_Observacion" TextMode="MultiLine" /></td>
                   </tr>
                   <tr>
                       <td style="text-align:right">
                           <asp:Button ID="btn_NuevaObs" Text="Nueva observación" runat="server" OnClick="btn_NuevaObs_Click"  />
                           <asp:Button ID="btn_GuardarObs" Text="Guardar" runat="server" OnClick="btn_GuardarObs_Click"  /></td>
                   </tr>
               </table>
           </td>
        </tr>
    </table>
</asp:Content>
