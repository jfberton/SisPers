using SisPer.Aplicativo.Controles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Formulario1214_Generados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                #region Menues

                if (usuarioLogueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }


                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                #endregion

                CargarF1214();

                if (usuarioLogueado.Area.Nombre == "Sub-Administración")
                {
                    CargarF1214Enviados();
                    CargarF1214Aprobados();
                }
                else
                {
                    panel_enviados.Attributes["Style"] = "display:none";
                    panel_aprobados.Attributes["Style"] = "display:none";
                }
            }
        }

        private void CargarF1214Aprobados()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Model1Container cxt = new Model1Container();

            var items = (from ff in cxt.Formularios1214
                         where
                         ff.Estado == Estado1214.Aprobada
                         select new
                         {
                             Form = ff.Id,
                             Disp = ff.NroDispo/10000,
                             Generadopor = ff.GeneradoPor.ApellidoYNombre,
                             Destino = ff.Destino,
                             Desde = ff.Desde,
                             Hasta = ff.Hasta,
                             Jefe = ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado) != null ? ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado).Agente.ApellidoYNombre : string.Empty,
                             Tareas = ff.TareasACumplir,
                             IdF1214 = ff.Id
                         }).ToList();

            gv_aprobados.DataSource = items;
            gv_aprobados.DataBind();
        }

        private void CargarF1214Enviados()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Model1Container cxt = new Model1Container();



            var items = (from ff in cxt.Formularios1214
                         where
                         ff.Estado == Estado1214.Enviado
                         select new
                         {
                             Numero = ff.Id,
                             Estado = ff.Estado,
                             Generadopor = ff.GeneradoPor.ApellidoYNombre,
                             Destino = ff.Destino,
                             Desde = ff.Desde,
                             Hasta = ff.Hasta,
                             Jefe = ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado) != null ? ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado).Agente.ApellidoYNombre : string.Empty,
                             Tareas = ff.TareasACumplir,
                             IdF1214 = ff.Id
                         }).ToList();

            gv_Form1214_enviados.DataSource = items;
            gv_Form1214_enviados.DataBind();
        }

        private void CargarF1214()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Model1Container cxt = new Model1Container();



            var items = (from ff in cxt.Formularios1214
                         where
                         ff.GeneradoPor.Id == usuarioLogueado.Id 
                         select new
                         {
                             Numero = ff.Id,
                             Estado = ff.Estado,
                             Generadopor = ff.GeneradoPor.ApellidoYNombre,
                             Destino = ff.Destino,
                             Desde = ff.Desde,
                             Hasta = ff.Hasta,
                             Jefe = ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado) != null ? ff.Nomina.FirstOrDefault(aa => aa.JefeComicion && aa.Estado != EstadoAgente1214.Cancelado).Agente.ApellidoYNombre : string.Empty,
                             Tareas = ff.TareasACumplir,
                             IdF1214 = ff.Id
                         }).ToList();

            gv_form1214.DataSource = items;
            gv_form1214.DataBind();

            if (usuarioLogueado.Perfil != PerfilUsuario.Personal)
            {
                gv_form1214.Columns[ObtenerColumna("Confeccionó")].Visible = false;
            }


        }

        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in gv_form1214.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = gv_form1214.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        protected void gv_1214_generados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_form1214.PageIndex = e.NewPageIndex;
            CargarF1214();
        }
       
        protected void btn_ver_Click(object sender, ImageClickEventArgs e)
        {
            int idf1214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Session["id214"] = idf1214;
            Response.Redirect("~/Aplicativo/Form1214_Nuevo.aspx");
        }

        protected void btn_reimprimir_Click(object sender, ImageClickEventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int idf1214 = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                Formulario1214 formulario = cxt.Formularios1214.FirstOrDefault(f => f.Id == idf1214);
                if (formulario != null && formulario.Estado == Estado1214.Enviado)
                {
                    #region imprimir

                    Session["Bytes"] = formulario.GenerarPDFSolicitud();
                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);

                    #endregion
                }
                else
                {
                    Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                    if (usuarioLogueado.Area.Nombre == "Sub-Administración")
                    {
                        Session["Bytes"] = formulario.GenerarPDFSolicitud();
                        string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                    }
                    else
                    {
                        MessageBox.Show(this, "Solo se pueden reimprimir solicitudes en estado Enviada!", MessageBox.Tipo_MessageBox.Info);
                    }
                }
            }
        }

        protected void gv_Form1214_enviados_PreRender(object sender, EventArgs e)
        {
            if (gv_Form1214_enviados.Rows.Count > 0)
            {
                gv_Form1214_enviados.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gv_form1214_PreRender(object sender, EventArgs e)
        {
            if (gv_form1214.Rows.Count > 0)
            {
                gv_form1214.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gv_form1214_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.Cells[1].Text == "Anulado")
                {
                    e.Row.ForeColor = Color.Red;
                    e.Row.Font.Italic = true;
                }

                if (e.Row.Cells[1].Text == "Aprobada")
                {
                    e.Row.ForeColor = Color.Green;
                }
            }
        }

        protected void gv_aprobados_PreRender(object sender, EventArgs e)
        {
            if (gv_aprobados.Rows.Count > 0)
            {
                gv_aprobados.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}