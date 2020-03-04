using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace SisPer.Aplicativo
{
    public partial class Personal_Ag_AgregarHoraDia : System.Web.UI.Page
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
                        Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                        InicializaDatosPagina(ag);
                    }
                    catch {
                        p_datosAgente.Visible = false;
                    }
                }

            }
        }

        private void InicializaDatosPagina(Agente ag)
        {
            DatosAgente1.Agente = ag;
            tb_Fecha.Value = DateTime.Today.ToShortDateString();
            lbl_FechaSeleccionada.Text = DateTime.Today.ToLongDateString();
            CargarMarcacionesFecha();
            CargarGrillaDeHorasFechaSeleccionada();
            LimpiarControles();
            p_datosAgente.Visible = true;
        }

        #region Horas

        protected void SeleccionoOtroItem(object sender, EventArgs e)
        {
            lbl_SumaResta.Text = Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado != null ? Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado.Suma ? "(+)" : "(-)" : "";
        }

        protected void btn_Guardar_Click(object sender, EventArgs e)
        {
            Page.Validate("MovimientoHora");
            if (Page.IsValid)
            {
                DateTime d = new DateTime();
                if (DateTime.TryParse(tb_Fecha.Value, out d))
                {
                    Agente ag = DatosAgente1.Agente;
                    Agente agendador = Session["UsuarioLogueado"] as Agente;
                    string horas = tb_Horas.Text;

                    TipoMovimientoHora tmh = Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado;
                    string descripcion = tb_descripcion.Text;
                    Model1Container cxt = new Model1Container();
                    ResumenDiario rD = cxt.ResumenesDiarios.FirstOrDefault(rd => rd.AgenteId == DatosAgente1.Agente.Id && rd.Dia == d);
                    if ((rD == null) ||
                        (rD != null &&
                            rD.MovimientosHoras.Where(mh =>
                                mh.AgendadoPor.Id == agendador.Id &&
                                mh.Tipo.Id == tmh.Id &&
                                mh.Horas == horas).Count() == 0
                         )
                        )
                    {
                        if (rD != null)
                        {
                            if (rD.MovimientosHoras.Where(mh => mh.Tipo.Id == tmh.Id).Count() > 0 &&
                                (Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado.Tipo == "Tardanza" ||
                                Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado.Tipo == "Prolongación de jornada"))
                            {
                                Controles.MessageBox.Show(this,"Los movimientos de tardanza y prolongación de jornada se pueden agendar uno por día unicamente.", Controles.MessageBox.Tipo_MessageBox.Warning);
                            }
                            else
                            {
                                ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, agendador, horas, tmh, descripcion);
                                //ListadoAgentesParaGrilla.ActualizarPropiedad(ag.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.HoraBonificacion, "");
                            }
                        }
                        else
                        {
                            ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, ag, agendador, horas, tmh, descripcion);
                            //ListadoAgentesParaGrilla.ActualizarPropiedad(ag.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.HoraBonificacion, "");
                        }

                    }
                    else
                    {
                        Controles.MessageBox.Show(this,"EL movimiento que esta intentando agendar ya fue agendado.", Controles.MessageBox.Tipo_MessageBox.Warning);
                    }
                }
            }

            LimpiarControles();
            CargarGrillaDeHorasFechaSeleccionada();
        }

        private void LimpiarControles()
        {
            if (Session["HorasPorAgendar"] != null)
            {
                tb_Horas.Text = Session["HorasPorAgendar"].ToString();
                Session["HorasPorAgendar"] = null;
            }
            else { tb_Horas.Text = ""; }

            if (Session["TipoMovimientoAgendar"] != null)
            {
                Ddl_TipoMovimientoHora1.SeleccionarTipoMovimientoHora = Session["TipoMovimientoAgendar"].ToString();
            }
            else { Ddl_TipoMovimientoHora1.SeleccionarTipoMovimientoHora = "0"; }

            if (Session["Descripcion"] != null)
            {
                tb_descripcion.Text = Session["Descripcion"].ToString();
                Session["Descripcion"] = null;
            }
            else
            { tb_descripcion.Text = ""; }


            if (Session["DiaBuscado"] != null)
            {
                tb_Fecha.Value = Convert.ToDateTime(Session["DiaBuscado"]).ToLongDateString();
                lbl_FechaSeleccionada.Text = Convert.ToDateTime(Session["DiaBuscado"]).ToLongDateString();
                CargarMarcacionesFecha();
                CargarGrillaDeHorasFechaSeleccionada();
                Session["DiaBuscado"] = null;
            }

           
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Personal_Ag_Listado.aspx");
        }

        protected void tb_Fecha_TextChanged(object sender, EventArgs e)
        {
            Page.Validate("Fecha");
            if (Page.IsValid)
            {
                CargarGrillaDeHorasFechaSeleccionada();
                CargarMarcacionesFecha();
                DateTime fechaSeleccionada = new DateTime();
                DateTime.TryParse(tb_Fecha.Value, out fechaSeleccionada);
                lbl_FechaSeleccionada.Text = fechaSeleccionada.ToLongDateString();
                LimpiarControles();
            }
        }

        private void CargarGrillaDeHorasFechaSeleccionada()
        {
            Model1Container cxt = new Model1Container();
            DateTime d = new DateTime();
            DateTime.TryParse(tb_Fecha.Value, out d);
            ResumenDiario horasDia = cxt.ResumenesDiarios.FirstOrDefault(hd => hd.AgenteId == DatosAgente1.Agente.Id && hd.Dia == d);
            if (horasDia != null)
            {
                lbl_totalHorasFechaSeleccionada.Text = "Total acumulado " + horasDia.Horas + " hs.";
                var items = (from mhd in horasDia.MovimientosHoras
                             select new
                             {
                                 Id = mhd.Id,
                                 Movimiento = mhd.Tipo.Tipo,
                                 Operador = mhd.Tipo.Suma ? "(+)" : "(-)",
                                 Horas = mhd.Horas,
                                 Horasanioanterior = mhd.DescontoDeAcumuladoAnioAnterior ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png",
                                 Horasbonific = mhd.DescontoDeHorasBonificables ? "~/Imagenes/accept.png" : "~/Imagenes/invisible.png"
                             }).ToList();
                GridView1.DataSource = items;
                GridView1.DataBind();
            }
            else
            {
                lbl_totalHorasFechaSeleccionada.Text = "";
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            DatosAgente1.Refrescar();

        }

        private void CargarMarcacionesFecha()
        {
            Agente ag = DatosAgente1.Agente;
            DateTime fechaSeleccionada = new DateTime();
            DateTime.TryParse(tb_Fecha.Value, out fechaSeleccionada);
            DS_Marcaciones marcaciones = ProcesosGlobales.ObtenerMarcaciones(fechaSeleccionada, ag.Legajo.ToString());

            gv_Huellas.DataSource = marcaciones.Marcacion;
            gv_Huellas.DataBind();
        }

        protected void gv_Huellas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Huellas.PageIndex = e.NewPageIndex;
            CargarMarcacionesFecha();
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            CargarGrillaDeHorasFechaSeleccionada();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = new DateTime();
            args.IsValid = DateTime.TryParse(tb_Fecha.Value, out d);
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Ddl_TipoMovimientoHora1.TipoMovimientoHoraSeleccionado != null;
        }

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Convert.ToDateTime(tb_Fecha.Value);
            args.IsValid = ObtenerEstadoAgenteParaElDia(d) == null;
        }

        private EstadoAgente ObtenerEstadoAgenteParaElDia(DateTime d)
        {
            Model1Container cxt = new Model1Container();
            return cxt.EstadosAgente.FirstOrDefault(hd => hd.AgenteId == DatosAgente1.Agente.Id && hd.Dia == d);
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Convert.ToDateTime(tb_Fecha.Value);
            Model1Container cxt = new Model1Container();
            args.IsValid = cxt.Feriados.FirstOrDefault(f => f.Dia == d) == null;
        }

        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Convert.ToDateTime(tb_Fecha.Value);
            Model1Container cxt = new Model1Container();
            CierreMensual cm = cxt.CierreMensual.FirstOrDefault(cm1 => cm1.AgenteId == DatosAgente1.Agente.Id && cm1.Anio == d.Year && cm1.Mes == d.Month);
            args.IsValid = cm == null;
        }

        #endregion

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
                        InicializaDatosPagina(ag);
                    }
                    else
                    {
                        Controles.MessageBox.Show(this, "No se encontro agente con el legajo buscado",Controles.MessageBox.Tipo_MessageBox.Warning);
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

        protected void btn_NuevaBusqueda_Click(object sender, EventArgs e)
        {
            btn_buscarAgente.Disabled = false;
            tb_legajoAgente.Disabled = false;
            btn_NuevaBusqueda.Visible = false;
            p_datosAgente.Visible = false;
        }

       
    }
}