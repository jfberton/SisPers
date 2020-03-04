using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace SisPer.Aplicativo
{
    public partial class Personal_Ag_AgregarMovimientoDia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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

                    Model1Container cxt = new Model1Container();
                    Session["CXT"] = cxt;
                    try
                    {
                        int id = Convert.ToInt32(Session["IdAg"]);
                        Session["IdAg"] = null;
                        Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                        InicializarPantalla(ag);
                    }
                    catch
                    {
                        p_datosAgente.Visible = false;
                    }
                }
            }
        }

        private void InicializarPantalla(Agente ag)
        {
            Session["MesEnDondeEstaAgendando"] = DateTime.Today;
            DatosAgente1.Agente = ag;
            Calendar1.SelectedDate = DateTime.Today;
            CargarGrillaEstados();
            p_datosAgente.Visible = true;
        }

        #region Estados

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void AgregarEstadoDia(DateTime dia)
        {
            Model1Container cxt = new Model1Container();
            int idAgenteLogueado = (Session["UsuarioLogueado"] as Agente).Id;
            Agente agendadoPor = cxt.Agentes.First(a => a.Id == idAgenteLogueado);
            Agente agentecxt = cxt.Agentes.First(a1 => a1.Id == DatosAgente1.Agente.Id);
            TipoEstadoAgente te = cxt.TiposEstadoAgente.First(tea => tea.Id == Ddl_TipoEstadoAgente1.EstadoSeleccionado.Id);
            if (te.Estado == "Franco compensatorio")
            {
                ProcesosGlobales.CrearFrancoAprobado(agentecxt, agendadoPor, dia);
            }
            else
            {
                int year = 0;
                int.TryParse(ddl_Anio.Text, out year);
                ProcesosGlobales.AgendarEstadoDiaAgente(agendadoPor, agentecxt, dia, te, year);
            }
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (DatosAgente1.Agente != null)
            {
                if (e.Day.IsOtherMonth)
                {
                    e.Cell.Text = "";
                }
                else
                {
                    Model1Container cxt = new Model1Container();

                    List<Notificacion> notSinEnviar = (from nn in cxt.Notificaciones
                                                       where
                                                       nn.AgenteId == DatosAgente1.Agente.Id &&
                                                       nn.HistorialEstadosNotificacion.FirstOrDefault(nh => nh.Estado.Estado == "Enviada") == null
                                                       select nn).ToList();

                    Notificacion notFecha = notSinEnviar.FirstOrDefault(nn => nn.HistorialEstadosNotificacion.First(nh => nh.Estado.Estado == "Generada").Fecha.Date == e.Day.Date);

                    if (notFecha != null)
                    {
                        e.Cell.BackColor = Color.OrangeRed;
                        e.Cell.ToolTip = "Debe envio de notificación N° " + notFecha.Id.ToString();
                    }
                    else
                    {

                        DateTime d = e.Day.Date;

                        EstadoAgente ea = DatosAgente1.Agente.ObtenerEstadoAgenteParaElDia(d, true);

                        Feriado feriado = cxt.Feriados.FirstOrDefault(f => f.Dia == d);


                        e.Cell.Enabled = false;
                        if (ea != null)
                        {
                            if (ea.TipoEstado.Estado == "Fin de semana")
                            {
                            }
                            else
                            {
                                if (ea.TipoEstado.Estado == "Feriado")
                                {
                                    e.Cell.BackColor = Color.DarkRed;
                                    e.Cell.ForeColor = Color.Azure;
                                    e.Cell.Font.Bold = true;
                                    e.Cell.ToolTip = "Feriado: " + (feriado !=null? feriado.Motivo:"Ningún feriado coincide con este día");
                                }
                                else
                                {
                                    e.Cell.BackColor = Color.DarkGoldenrod;
                                    e.Cell.ForeColor = ColorTranslator.FromHtml("#333333");
                                    e.Cell.Font.Bold = true;
                                    e.Cell.ToolTip = ea.TipoEstado.Estado;
                                }
                            }
                        }
                        else
                        {
                            if (feriado != null)
                            {
                                e.Cell.BackColor = Color.DarkRed;
                                e.Cell.ForeColor = Color.Azure;
                                e.Cell.Font.Bold = true;
                                e.Cell.ToolTip = "Feriado: " + feriado.Motivo;
                            }
                            else
                            {
                                e.Cell.ToolTip = "";
                            }
                        }
                    }

                    
                }
            }
        }

        private void CargarGrillaEstados()
        {
            Model1Container cxt = new Model1Container();

            DateTime d = Convert.ToDateTime(Session["MesEnDondeEstaAgendando"]);
            var estados = (from ea in cxt.EstadosAgente
                           where ea.AgenteId == DatosAgente1.Agente.Id &&
                           ea.Dia.Month == d.Month &&
                           ea.Dia.Year == d.Year &&
                           ea.TipoEstado.Estado != "Fin de semana"
                           select new
                           {
                               Id = ea.Id,
                               AgendadoPor = ea.AgendadoPor.ApellidoYNombre,
                               Estado = ea.TipoEstado.Estado,
                               Dia = ea.Dia
                           }).OrderBy(a => a.Dia).ToList();

            var estados_con_descripcion_año = (from estado in estados
                                               select new
                                               {
                                                   Id = estado.Id,
                                                   AgendadoPor = estado.AgendadoPor,
                                                   Dia = estado.Dia,
                                                   Estado = AgregarAñoSiEsLicencia(estado.Estado, estado.Dia)
                                               }).OrderBy(a => a.Dia).ToList();

            GridViewEstados.DataSource = estados_con_descripcion_año;
            GridViewEstados.DataBind();

            var datos = (from e in estados
                         group e by e.Estado into ee
                         select new { Estado = ee.Key, Dias = ee.Count() }).ToList();

            GridViewTotalesPorEstado.DataSource = datos;
            GridViewTotalesPorEstado.DataBind();

            DatosAgente1.Refrescar();
        }

        private string AgregarAñoSiEsLicencia(string estado, DateTime dia)
        {
            string ret = estado;

            if (estado.Contains("Licencia") && !estado.Contains("Invierno"))
            {
                using (var cxt = new Model1Container())
                {
                    DiaUsufructado du = cxt.DiasUsufructuados.FirstOrDefault(dduu => dduu.AgenteId == DatosAgente1.Agente.Id && dduu.Dia == dia);
                    if (du != null)
                    {
                        ret = estado + " - " + du.Anio;
                    }
                }
            }

            return ret;   
        }

        protected void gridViewEstados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewEstados.PageIndex = e.NewPageIndex;
            CargarGrillaEstados();
        }

        protected void btn_EliminarEstado_Click(object sender, ImageClickEventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idEstado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            EstadoAgente ea = cxt.EstadosAgente.First(est => est.Id == idEstado);
            Agente ag = ea.Agente;
            if (ea.TipoEstado.Estado != "Franco compensatorio p/aprobar")
            {
                if (ea.TipoEstado.Estado == "Franco compensatorio")
                {
                    Franco fra = ag.Francos.FirstOrDefault(f => f.DiasFranco.Where(d => d.Dia == ea.Dia).Count() > 0 && f.Estado == EstadosFrancos.Aprobado);
                    if (fra != null && fra.MovimientosFranco.Count > 1)
                    {
                        Controles.MessageBox.Show(this, "No se puede eliminar un franco compensatorio que no haya sido agendado desde esta funcionalidad (El franco que quiere eliminar fue solicitado por el agente y aprobado por todos sus superiores). Si cree que debe eliminarce comuníquese con Sistemas.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        ProcesosGlobales.EliminarFrancoYMovimientos(fra);
                    }
                }
                else
                {
                    if (ea.TipoEstado.Estado == "Licencia Anual")
                    {
                        DiaUsufructado du = cxt.DiasUsufructuados.FirstOrDefault(d => d.AgenteId == ag.Id && d.Dia == ea.Dia);
                        if (du != null)
                        {
                            cxt.DiasUsufructuados.DeleteObject(du);
                        }
                    }

                    Session["MesEnDondeEstaAgendando"] = ea.Dia;
                    cxt.EstadosAgente.DeleteObject(ea);
                    cxt.SaveChanges();
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Desde aqui no se puede eliminar un Franco compensatorio pendiente de aprobación.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }

            CargarGrillaEstados();

        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Ddl_TipoEstadoAgente1.EstadoSeleccionado != null;
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool ret = true;
            try
            {
                using (var cxt = new Model1Container())
                {
                    DateTime desde = Convert.ToDateTime(tb_desde.Value);
                    DateTime hasta = Convert.ToDateTime(tb_hasta.Value);
                    DateTime mes_desde = new DateTime(desde.Year, desde.Month, 1);
                    DateTime mes_hasta = new DateTime(hasta.Year, hasta.Month, 1);

                    while (mes_desde <= mes_hasta)
                    {
                        ret = ret
                            && (
                                cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == mes_desde.Year && cm.Mes == mes_desde.Month) == null ||
                                cxt.CierreMensual.FirstOrDefault(cm => cm.Anio == mes_desde.Year && cm.Mes == mes_desde.Month).Tiene_que_modificar == false);

                        mes_desde = mes_desde.AddMonths(1);
                    }
                }
            }
            catch
            {
                ret = false;
            }

            args.IsValid = ret;
        }

        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Calendar1.SelectedDate;
            Model1Container cxt = new Model1Container();
            args.IsValid = cxt.Feriados.FirstOrDefault(f => f.Dia == d) == null;
        }

        #endregion

        protected void Calendar1_PreRender(object sender, EventArgs e)
        {
            Calendar1.SelectedDates.Clear();
        }

        protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Calendar1.SelectedDate = e.NewDate;
            Session["MesEnDondeEstaAgendando"] = Calendar1.SelectedDate;
            CargarGrillaEstados();
        }

        protected void btn_buscarAgente_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                int leg = 0;
                Agente ag;

                if (int.TryParse(tb_legajoAgente.Value, out leg))
                {
                    ag = cxt.Agentes.FirstOrDefault(a => a.Legajo == leg && !a.FechaBaja.HasValue);
                    if (ag != null)
                    {
                        tb_legajoAgente.Disabled = true;
                        btn_buscarAgente.Disabled = true;
                        btn_NuevaBusqueda.Visible = true;
                        InicializarPantalla(ag);
                    }
                    else
                    {
                        Controles.MessageBox.Show(this, "No se encontro agente con el legajo buscado", Controles.MessageBox.Tipo_MessageBox.Warning);
                        p_datosAgente.Visible = false;
                    }
                }
                else
                {
                    Controles.MessageBox.Show(this, "El legajo ingresado es inválido", Controles.MessageBox.Tipo_MessageBox.Warning);
                    p_datosAgente.Visible = false;
                }
            }
        }

        protected void btn_Agendar_Click(object sender, EventArgs e)
        {
            Page.Validate("Estados");
            if (IsValid)
            {
                if (tb_hasta.Value == string.Empty)
                {
                    tb_hasta.Value = tb_desde.Value;
                }

                DateTime desde;
                DateTime hasta;
                DateTime.TryParse(tb_desde.Value, out desde);
                DateTime.TryParse(tb_hasta.Value, out hasta);

                for (DateTime i = desde; i <= hasta; i = i.AddDays(1))
                {
                    AgregarEstadoDia(i);
                }

                CargarGrillaEstados();

                //if (DateTime.Today >= desde && DateTime.Today <= hasta)
                //{
                //    ListadoAgentesParaGrilla.ActualizarPropiedad(DatosAgente1.Agente.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Estado, DatosAgente1.Agente.EstadoActual);
                //}
            }
            
        }



        protected void cv_VerificarestadosRango_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            DateTime hasta;
            if (DateTime.TryParse(tb_desde.Value, out desde) && DateTime.TryParse(tb_hasta.Value, out hasta))
            {
                bool noPoseeMovimientooSolicitud = true;
                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    EstadoAgente ea = DatosAgente1.Agente.ObtenerEstadoAgenteParaElDia(d);
                    noPoseeMovimientooSolicitud = noPoseeMovimientooSolicitud && (ea == null || (
                                                                                                    ea != null && (
                                                                                                    ea.TipoEstado.Estado == "Fin de semana" ||
                                                                                                    ea.TipoEstado.Estado == "Feriado" ||
                                                                                                    ea.TipoEstado.Estado == "Natalicio"
                                                                                                                )
                                                                                                    )
                                                                                                );
                }

                args.IsValid = noPoseeMovimientooSolicitud;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void cv_fechaHasta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime desde;
            DateTime hasta;
            args.IsValid = (DateTime.TryParse(tb_desde.Value, out desde) && DateTime.TryParse(tb_hasta.Value, out hasta) && desde <= hasta);
        }

        protected void cv_VerificarDesde_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_desde.Value, out d);
        }

        protected void cv_VerificarHasta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_hasta.Value, out d);
        }

        protected void btn_NuevaBusqueda_Click(object sender, EventArgs e)
        {
            btn_buscarAgente.Disabled = false;
            tb_legajoAgente.Disabled = false;
            btn_NuevaBusqueda.Visible = false;
            p_datosAgente.Visible = false;
        }

        protected void Ddl_TipoEstadoAgente1_SelectedItemChanged(object sender, EventArgs e)
        {
            var tea = Ddl_TipoEstadoAgente1.EstadoSeleccionado;
            if (tea.Estado == "Licencia Anual" || tea.Estado == "Licencia Anual (Saldo)" || tea.Estado == "Licencia Anual (Anticipo)")
            {
                lbl_anio.Visible = true;
                ddl_Anio.Items.Clear();
                ddl_Anio.Items.Add(new ListItem { Text = (DateTime.Today.Year - 2).ToString() });
                ddl_Anio.Items.Add(new ListItem { Text = (DateTime.Today.Year - 1).ToString() });
                ddl_Anio.Items.Add(new ListItem { Text = DateTime.Today.Year.ToString() });
                ddl_Anio.Visible = true;
            }
            else
            {
                lbl_anio.Visible = false;
                ddl_Anio.Visible = false;
            }
        }

        protected void btn_cargar_calenadrio_ServerClick(object sender, EventArgs e)
        {
            string mes_str = tb_mes.Text.Split()[0];
            string anio_str = tb_mes.Text.Split()[1];
            int mes = 0;
            int anio = 0;

            switch (mes_str)
            {
                case "enero":
                    mes = 1;
                    break;
                case "febrero":
                    mes = 2;
                    break;
                case "marzo":
                    mes = 3;
                    break;
                case "abril":
                    mes = 4;
                    break;
                case "mayo":
                    mes = 5;
                    break;
                case "junio":
                    mes = 6;
                    break;
                case "julio":
                    mes = 7;
                    break;
                case "agosto":
                    mes = 8;
                    break;
                case "septiembre":
                    mes = 9;
                    break;
                case "octubre":
                    mes = 10;
                    break;
                case "noviembre":
                    mes = 11;
                    break;
                case "diciembre":
                    mes = 12;
                    break;
                default:
                    break;
            }

            if (int.TryParse(anio_str, out anio) && mes != 0)
            {//Correcto
                DateTime primer_dia_mes_seleccionado = new DateTime(anio, mes, 1);
                Session["MesEnDondeEstaAgendando"] = primer_dia_mes_seleccionado;
                Calendar1.SelectedDates.Clear();
                Calendar1.SelectedDate = primer_dia_mes_seleccionado;
                Calendar1.VisibleDate = primer_dia_mes_seleccionado;
                CargarGrillaEstados();
                p_datosAgente.Visible = true;
            }
            else
            {
                Controles.MessageBox.Show(this, "El período buscado es incorrecto", Controles.MessageBox.Tipo_MessageBox.Danger);
            }
        }
    }
}