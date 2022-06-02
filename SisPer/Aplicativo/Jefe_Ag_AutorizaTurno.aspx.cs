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
    public partial class Jefe_Ag_AutorizaTurno : System.Web.UI.Page
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
                agente = cxt.Agentes.Include("TipoHorariosFexible").FirstOrDefault(aa => aa.Id == id_agente);
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
                    List<GvItem> gvItems = cxt.TurnosIngresoPermitido.Where(dd => dd.AgenteId == agente.Id && dd.Dia >= primerDia && dd.Dia <= ultimoDia).ToList().Select(dd => new GvItem (dd.Id, dd.Agente.ApellidoYNombre, dd.Dia, dd.Turno, dd.Desde, dd.Hasta)).ToList();
                        
                    ret = gvItems.Select(dd => dd.Fecha).ToList();

                    Session["gv_items"] = gvItems;
                    gv_autorizaciones.DataSource = gvItems;
                    gv_autorizaciones.DataBind();
                }

                string horario = String.Empty;
                if (agente.HorarioFlexible == true && agente.TipoHorariosFexible == null)
                {
                    MessageBox.Show(this, "El agente " + agente.ApellidoYNombre + " tiene horario flexible libre, no hace falta otorgar permiso", MessageBox.Tipo_MessageBox.Warning);
                    horario = "El agente tiene horario flexible libre, no hace falta otorgar permiso";
                    Calendar1.Visible = false;
                    gv_autorizaciones.Visible = false;
                    lbl_autorizar.Visible = false;
                    ddl_turno_seleccionado.Visible = false; 
                }
                else
                {
                    if (agente.HorarioFlexible == true)
                    {
                        horario = string.Format("Tipo HF: {3} - H. Entrada: {0} - H. Salida: {1} - H. Jornada: {2} ", agente.TipoHorariosFexible.Hentrada, agente.TipoHorariosFexible.Hsalida, agente.TipoHorariosFexible.Hjornada,agente.TipoHorariosFexible.Tipo);
                    }
                    else
                    {
                        horario = string.Format("Datos: H. Entrada: {0} - H. Salida: {1} - {2}", agente.HoraEntrada, agente.HoraSalida, "No es Flexible");
                    }
                    
                    Calendar1.Visible = true;
                    gv_autorizaciones.Visible = true;
                    lbl_autorizar.Visible = true;
                    ddl_turno_seleccionado.Visible = true;
                }
                p_horario.InnerText = horario;

                int horaentrada = int.Parse(agente.HoraEntrada.Substring(0, 2));
                ddl_turno_seleccionado.SelectedValue = horaentrada < 13 ? "TT" : "TM";
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
                    e.Cell.BackColor = Color.Gold;
                    e.Cell.ToolTip = "Autorizado otro turno";
                }
            }
        }

        private struct GvItem
        {
            public int Id { get; set; }
            public string Agente { get; set; }
            public DateTime Fecha { get; set; }
            public string Turno { get; set; }
            public string Desde { get; set; }
            public string Hasta { get; set; }

            public GvItem(int _Id, string _Agente, DateTime _Fecha, string _turno, string _desde, string _hasta) {
                Id = _Id;   
                Agente = _Agente;   
                Fecha = _Fecha;
                Turno = _turno;
                Desde = _desde; 
                Hasta = _hasta;
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
                    string turno = ddl_turno_seleccionado.SelectedValue;
                    string desde = "";
                    string hasta = "";
                    if (turno == "TM")
                    {
                        desde = "06:30";
                        hasta = "13:00";
                    }
                    else
                    {
                        desde = "13:00";
                        hasta = "19:30";
                    }

                    bool resumenDiarioCerrado = cxt.ResumenesDiarios.FirstOrDefault(rd => rd.Dia == selectedDate && rd.AgenteId == ag.Id) == null ? false : cxt.ResumenesDiarios.FirstOrDefault(rd => rd.Dia == selectedDate && rd.AgenteId == ag.Id).Cerrado == true ? true : false;

                    if (!resumenDiarioCerrado)
                    {
                        TurnoIngresoPermitido dia = new TurnoIngresoPermitido() { AgenteId = ag.Id, Dia = selectedDate, Turno = turno, Desde = desde, Hasta = hasta };
                        cxt.TurnosIngresoPermitido.AddObject(dia);
                        cxt.SaveChanges();

                        list.Add(selectedDate);

                        List<GvItem> gvItems = Session["gv_items"] as List<GvItem>;
                        gvItems.Add(new GvItem(dia.Id, dia.Agente.ApellidoYNombre, dia.Dia, dia.Turno, dia.Desde, dia.Hasta));
                        Session["gv_items"] = gvItems;

                        gv_autorizaciones.DataSource = gvItems;
                        gv_autorizaciones.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "El día ya fue cerrado por el sistema, no puede modificar este turno.", Controles.MessageBox.Tipo_MessageBox.Danger, "No se puede eliminar");
                    }
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
                    string turno = ddl_turno_seleccionado.SelectedValue;
                    string desde = "";
                    string hasta = "";
                    if (turno == "TM")
                    {
                        desde = "06:30";
                        hasta = "13:00";
                    }
                    else
                    {
                        desde = "13:00";
                        hasta = "19:30";
                    }

                    TurnoIngresoPermitido dia = cxt.TurnosIngresoPermitido.First(aa => aa.Dia == selectedDate && aa.AgenteId == ag.Id);

                    bool resumenDiarioCerrado = cxt.ResumenesDiarios.FirstOrDefault(rd => rd.Dia == selectedDate && rd.AgenteId == ag.Id) == null ? false : cxt.ResumenesDiarios.FirstOrDefault(rd => rd.Dia == selectedDate && rd.AgenteId == ag.Id).Cerrado == true ? true : false;


                    if (!resumenDiarioCerrado)
                    {

                        cxt.TurnosIngresoPermitido.DeleteObject(dia);
                        cxt.SaveChanges();

                        List<GvItem> gvItems = Session["gv_items"] as List<GvItem>;

                        gvItems.Remove(gvItems.First(gvi=>gvi.Fecha == selectedDate));

                        Session["gv_items"] = gvItems;

                        list.Remove(selectedDate);

                        gv_autorizaciones.DataSource = gvItems;
                        gv_autorizaciones.DataBind();
                    }
                    else {
                        MessageBox.Show(this.Page, "El día ya fue cerrado por el sistema, no puede modificar este turno.", Controles.MessageBox.Tipo_MessageBox.Danger, "No se puede eliminar");
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