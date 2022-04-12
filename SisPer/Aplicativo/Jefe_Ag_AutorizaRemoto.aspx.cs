using Microsoft.Reporting.WebForms;
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
    public partial class Jefe_Ag_AutorizaRemoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }


                if (
                   ag.Perfil != PerfilUsuario.Personal &&
                   !ag.Jefe && !ag.JefeTemporal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {

                    MenuPersonalJefe1.Visible = (ag.Perfil == PerfilUsuario.Personal);
                    MenuJefe1.Visible = !(ag.Perfil == PerfilUsuario.Personal);
                }

                CargarDDLAgentes();
            }
        }


        private void CargarDDLAgentes()
        {
            Agente ag = Session["UsuarioLogueado"] as Agente;

            ddl_agente.Items.Clear();

            //ddl_agente.Items.Add(new ListItem { Text = ag.ApellidoYNombre, Value = ag.Legajo.ToString(), Selected = true });

            using (var cxt = new Model1Container())
            {
                var items = cxt.sp_obtener_agentes_cascada(ag.Id, false).OrderBy(x => x.nivel_para_ordenar).OrderBy(x => x.nombre_agente).ToList();

                foreach (var item in items)
                {
                    ddl_agente.Items.Add(new ListItem { Text = item.nombre_agente, Value = item.id_agente.ToString() });
                }
            }

            ObtenerDiasAutorizadoAgente();
        }



        private void ObtenerDiasAutorizadoAgente()
        {
            Agente agente = null;

            using (var cxt = new Model1Container())
            {
                int id_agente = int.Parse(ddl_agente.SelectedItem.Value);
                agente = cxt.Agentes.FirstOrDefault(aa => aa.Id == id_agente);
            }

            if (agente != null)
            {
                Session["agenteSeleccionado"] = agente;
            }

            var ret = new List<DateTime>();

            if (agente != null)
            {
                DateTime primerDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                if (Calendar1.VisibleDate != DateTime.MinValue)
                {
                    primerDia = Calendar1.VisibleDate;
                }

                DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);
                using (var cxt = new Model1Container())
                {
                    List<GvItem> gvItems = cxt.DiasAutorizadosRemoto.Where(dd => dd.AgenteId == agente.Id && dd.Dia >= primerDia && dd.Dia <= ultimoDia).ToList().Select(dd => new GvItem (dd.Id, dd.Agente.ApellidoYNombre, dd.Dia)).ToList();
                        
                    ret = gvItems.Select(dd => dd.Fecha).ToList();

                    Session["gv_items"] = gvItems;
                    gv_autorizaciones.DataSource = gvItems;
                    gv_autorizaciones.DataBind();
                }
            }

            Session["DiasAutorizado"] = ret;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            List<DateTime> list = (List<DateTime>)Session["DiasAutorizado"];
            if (list != null)
            {
                if (list.Contains(e.Day.Date))
                {
                    e.Cell.BackColor = Color.Green;
                    e.Cell.ToolTip = "Autorizado remoto";
                }
            }
        }

        private struct GvItem
        {
            public int Id { get; set; }
            public string Agente { get; set; }
            public DateTime Fecha { get; set; }

            public GvItem(int _Id, string _Agente, DateTime _Fecha) {
                Id = _Id;   
                Agente = _Agente;   
                Fecha = _Fecha;
            }
        }


        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            List<DateTime> list = (List<DateTime>)Session["DiasAutorizado"];

            if (list.Contains(Calendar1.SelectedDate))
            {
                QuitarDia(Calendar1.SelectedDate);
            }
            else
            {
                AgregarDia(Calendar1.SelectedDate);
            }
        }

        private void AgregarDia(DateTime selectedDate)
        {
            List<DateTime> list = (List<DateTime>)Session["DiasAutorizado"];

            Agente ag = Session["agenteSeleccionado"] as Agente;

            try
            {
                using (var cxt = new Model1Container())
                {
                    DiaAutorizadoRemoto dia = new DiaAutorizadoRemoto() { AgenteId = ag.Id, Dia = selectedDate };
                    cxt.DiasAutorizadosRemoto.AddObject(dia);
                    cxt.SaveChanges();

                    list.Add(selectedDate);

                    List<GvItem> gvItems= Session["gv_items"] as List<GvItem>;
                    gvItems.Add(new GvItem(dia.Id, dia.Agente.ApellidoYNombre, dia.Dia));
                    Session["gv_items"] = gvItems;

                    gv_autorizaciones.DataSource = gvItems;
                    gv_autorizaciones.DataBind();

                }

            }
            catch (Exception ex)
            {

            }

            Session["DiasAutorizado"] = list;
        }

        private void QuitarDia(DateTime selectedDate)
        {
            List<DateTime> list = (List<DateTime>)Session["DiasAutorizado"];

            Agente ag = Session["agenteSeleccionado"] as Agente;

            try
            {
                using (var cxt = new Model1Container())
                {
                    DiaAutorizadoRemoto dia = cxt.DiasAutorizadosRemoto.First(aa => aa.Dia == selectedDate && aa.AgenteId == ag.Id);
                    GvItem item = new GvItem(dia.Id, dia.Agente.ApellidoYNombre, dia.Dia);

                    EntradaSalida es = cxt.EntradasSalidas.FirstOrDefault(eess => eess.AgenteId == dia.AgenteId && eess.Fecha == dia.Dia);

                    if (es == null)
                    {

                        cxt.DiasAutorizadosRemoto.DeleteObject(dia);
                        cxt.SaveChanges();

                        List<GvItem> gvItems = Session["gv_items"] as List<GvItem>;

                        gvItems.Remove(item);

                        Session["gv_items"] = gvItems;

                        list.Remove(selectedDate);

                        gv_autorizaciones.DataSource = gvItems;
                        gv_autorizaciones.DataBind();
                    }
                    else {
                        MessageBox.Show(this.Page, "El movimiento que esta intentando eliminar ya posee marcaciones enviadas.", Controles.MessageBox.Tipo_MessageBox.Danger, "No se puede eliminar");
                    }
                }

            }
            catch (Exception ex)
            {

            }

            Session["DiasAutorizado"] = list;
        }

        protected void ddl_agente_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calendar1.VisibleDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ObtenerDiasAutorizadoAgente();
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            ObtenerDiasAutorizadoAgente();
        }

        protected void gv_autorizaciones_PreRender(object sender, EventArgs e)
        {
            if (gv_autorizaciones.Rows.Count > 0)
            {
                gv_autorizaciones.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btn_eliminar_autorizacion_dia_Click(object sender, ImageClickEventArgs e)
        {
            DateTime dia = Convert.ToDateTime(((ImageButton)sender).CommandArgument);

            QuitarDia(dia);
        }
    }
}