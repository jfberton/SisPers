using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Ag_Tardanzas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (usuariologueado.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    Calendar1.SelectedDate = DateTime.Today;
                    lbl_FechaSeleccionada.Text = "Detalle de tardanzas del día " + Calendar1.SelectedDate.ToLongDateString();
                    Session["DiaBuscado"] = DateTime.MinValue;
                    rb_CasaCentral.Checked = true;
                    rb_Marco.Checked = true;
                    CargarGrilla();
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            lbl_FechaSeleccionada.Text = "Detalle de tardanzas del día " + Calendar1.SelectedDate.ToLongDateString();
            CargarGrilla();
        }

        protected struct ItemGrilla
        {
            public string Legajo { get; set; }
            public bool Interior { get; set; }
            public string Agente { get; set; }
            public string HoraEntrada { get; set; }
            public bool MarcaManual { get; set; }
            public string TodosLosCampos { get; set; }
        }

        private void CargarGrilla()
        {
            using (Model1Container cxt = new Model1Container())
            {
                DateTime diaGuardadoEnSession = Convert.ToDateTime(Session["DiaBuscado"]);
                var agentes = cxt.Agentes.Where(a=>a.FechaBaja==null);
                DateTime d = Calendar1.SelectedDate;
                List<ItemGrilla> itemsGrilla = new List<ItemGrilla>();

                if (diaGuardadoEnSession != d)
                {
                    gv_Huellas.PageIndex = 0;
                    foreach (Agente ag in agentes)
                    {
                        bool llegoTarde = false;
                        bool marcoManual = false;
                        string horaMarcada = "";
                        ag.Tardanza(d, out llegoTarde, out horaMarcada, out marcoManual);

                        if (llegoTarde)
                        {
                            string enviar = ag.Id.ToString();
                            enviar = enviar + "]";
                            string hora = horaMarcada != "No marco" ? HorasString.RestarHoras(horaMarcada, ag.ObtenerHoraEntradaLaboral(d)) : "000:00";
                            enviar = enviar + hora;
                            itemsGrilla.Add(new ItemGrilla()
                            {
                                Legajo = ag.Legajo.ToString(),
                                Interior = ag.Area != null ? (ag.Area.Interior.HasValue ? ag.Area.Interior.Value : false) : false,
                                Agente = ag.ApellidoYNombre,
                                HoraEntrada = horaMarcada,
                                MarcaManual = marcoManual,
                                TodosLosCampos = enviar
                            });
                        }
                    }

                    Session["DiaBuscado"] = d;
                    Session["Resultados"] = itemsGrilla;
                }
                else
                {
                    itemsGrilla = Session["Resultados"] as List<ItemGrilla>;
                }

                bool interior = rb_Interior.Checked;
                bool sinMarcacion = rb_NoMarco.Checked;

                var itemsfiltrados = from item in itemsGrilla
                                     where item.Interior == interior &&
                                            ((sinMarcacion && item.HoraEntrada =="No marco") || (!sinMarcacion && item.HoraEntrada !="No marco"))
                                     select item;

                gv_Huellas.DataSource = itemsfiltrados.OrderByDescending(ig => ig.HoraEntrada).ToList();
                gv_Huellas.DataBind();
            }
        }

        protected void gv_Huellas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Huellas.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void btn_Ir_Click(object sender, ImageClickEventArgs e)
        {
            //recorda que le pasas de esta manera los datos
            //TodosLosCampos = ag.Id + "]" + horaMarcada != "No marco" ? HorasString.RestarHoras(horaMarcada, ag.HoraEntrada) : "000:00"
            string[] datos = ((ImageButton)sender).CommandArgument.Split(']');
            Session["IdAg"] = datos[0];
            if (datos[1] != "000:00")
            {
                Session["HorasPorAgendar"] = datos[1];
            }
            else
            {
                Session["HorasPorAgendar"] = datos[1];
                Session["Descripcion"] = "No marco entrada";
            }

            Model1Container cxt = new Model1Container();
            TipoMovimientoHora mh = cxt.TiposMovimientosHora.FirstOrDefault(tmh=>tmh.Tipo =="Tardanza");
            Session["TipoMovimientoAgendar"] = mh != null ? mh.Id.ToString() : "0";
            Response.Redirect("~/Aplicativo/Personal_Ag_AgregarHoraDia.aspx");
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}