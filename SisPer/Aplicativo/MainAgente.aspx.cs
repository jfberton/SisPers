using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class MainAgente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente agSession = Session["UsuarioLogueado"] as Agente;

                if (agSession == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                int id = 0;
                string idUsr = Request.QueryString["Usr"];
                if (idUsr != null && idUsr != string.Empty)
                {
                    id = Convert.ToInt32(idUsr);
                }
                else
                {
                    id = agSession.Id;
                }

                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                Session["Agente"] = ag;
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                if (usuarioLogueado.Perfil == PerfilUsuario.Personal)
                {
                    MenuPersonalAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuPersonalJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }
                else
                {
                    MenuAgente1.Visible = !(usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                    MenuJefe1.Visible = (usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
                }

                DatosAgente1.Agente = ag;

                Calendar1.SelectedDate = DateTime.Today;


                VerificarEstadoAgente();

                CargarEnumeraciones();
                CargarMarcacionesFecha();

                RefrescarModuloSalidas();
                RefrescarModuloHV();
                RefrescarModuloFrancos();
                RefrescarModuloDiasSinCerrar();

                bool mostrarMensaje = Convert.ToBoolean(Session["MostrarMensageBienvenida"]);
                Session["MostrarMensageBienvenida"] = false;
                if (mostrarMensaje)
                {
                    MensageBienvenida.Show();
                }
            }
        }


        private void VerificarEstadoAgente()
        {
            Agente ag = Session["Agente"] as Agente;
            string estado_agente = ag.EstadoActual;
            //ListadoAgentesParaGrilla.ActualizarPropiedad(ag.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Estado, estado_agente);
            if (estado_agente != "Activo")
            {
                if (estado_agente == "Sin datos")
                {
                    lbl_EstadoFuera.Text = "No se encuentran datos de la marcación de entrada";
                }
                else
                {
                    lbl_EstadoFuera.Text = "El agente no se encuentra presente por: " + estado_agente.Replace("_", " ");
                    Salida ultimasalida = ObtenerUltimaSalida();
                    P_SalidasDiarias.Enabled = (ultimasalida != null && ultimasalida.HoraHasta == null);
                }
            }
            else
            {
                lbl_EstadoFuera.Visible = false;
                P_SalidasDiarias.Enabled = true;
            }
        }

        private EstadoAgente ObtenerEstadoAgenteParaElDia(DateTime d)
        {
            Model1Container cxt = new Model1Container();
            return cxt.EstadosAgente.FirstOrDefault(hd => hd.AgenteId == DatosAgente1.Agente.Id && hd.Dia == d);
        }

        private void CargarEnumeraciones()
        {
            //SALIDAS
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(TipoSalida)))
            {
                dictionary.Add(value, Enum.GetName(typeof(TipoSalida), value));
            }

            Ddl_TipoSalida.DataSource = dictionary;
            Ddl_TipoSalida.DataTextField = "Value";
            Ddl_TipoSalida.DataValueField = "Key";
            Ddl_TipoSalida.DataBind();
        }



        #region Salidas

        private void RefrescarModuloSalidas()
        {
            Session["NuevaSalida"] = "No";
            Salida ultimasalida = ObtenerUltimaSalida();
            tb_DestinoSalida.Text = string.Empty;

            //verifico el estado de la ultima salida
            if (ultimasalida != null && ultimasalida.HoraHasta == null)
            {
                //Existe una ultima salida y no esta cerrada aun
                btn_NuevaSalida.Visible = false;
                PanelNuevaSalida.Visible = false;
                PanelTieneSalidaActiva.Visible = true;

                lbl_HoraSalida.Text = ultimasalida.HoraDesde;
                lbl_TipoSalida.Text = ultimasalida.Tipo.ToString();
                lbl_HorasCorridas.Text = HorasString.RestarHoras(DateTime.Now.ToString("HH:mm"), ultimasalida.HoraDesde);
            }
            else
            {
                //No existen salidas en la fecha o la que existe esta cerrada
                btn_NuevaSalida.Visible = true;
                PanelNuevaSalida.Visible = false;
                PanelTieneSalidaActiva.Visible = false;
            }

            CargarGrillaSalidas();
        }

        private Salida ObtenerUltimaSalida()
        {
            Agente ag = Session["Agente"] as Agente;
            DateTime d = DateTime.Today;
            Salida ultimasalida = null;
            Model1Container cxt = new Model1Container();
            var salidas = cxt.Salidas.Where(s => s.HoraHasta == null && s.AgenteId == ag.Id).ToList();
            ProcesosGlobales.ProcesarSalidasSinTerminar(salidas);
            //Obtengo la ultima salida
            if (cxt.Salidas.Where(s => s.AgenteId == ag.Id && s.Dia == d).Count() > 0)
            {
                int id = cxt.Salidas.Where(s => s.AgenteId == ag.Id && s.Dia == d).Max(s1 => s1.Id);
                ultimasalida = cxt.Salidas.First(s => s.Id == id);
            }
            return ultimasalida;
        }

        private bool VerificarSiPuedeMarcarSalida()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Agente ag = Session["Agente"] as Agente;
            bool ret = true;
            ret = ret && (usuarioLogueado.Perfil == PerfilUsuario.Personal || usuarioLogueado.Jefe || usuarioLogueado.JefeTemporal);
            ret = ret && (usuarioLogueado.Perfil == PerfilUsuario.Personal || usuarioLogueado.Id != ag.Id);
            //ret = ret || 

            return ret;
        }

        protected void btn_NuevaSalida_Click(object sender, EventArgs e)
        {

            Salida sal = ObtenerUltimaSalida();

            if (((sal != null && sal.HoraHasta != null) || sal == null))
            {
                PanelNuevaSalida.Visible = true;
                btn_NuevaSalida.Visible = false;
                Session["NuevaSalida"] = "Si";
            }
            else
            {
                RefrescarModuloSalidas();
            }
        }

        protected void btn_ComenzarSalida_Click(object sender, EventArgs e)
        {
            if (Session["NuevaSalida"].ToString() == "Si")
            {
                Page.Validate("Salida");
                if (Page.IsValid)
                {
                    Agente ag = Session["Agente"] as Agente;
                    Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;

                    Model1Container cxt = new Model1Container();

                    Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);

                    Salida sal = new Salida();
                    sal.AgenteId = ag.Id;
                    sal.Dia = DateTime.Today;
                    sal.HoraDesde = DateTime.Now.ToString("HH:mm");
                    sal.Tipo = (TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue);
                    sal.Destino = tb_DestinoSalida.Text;

                    if (VerificarSiPuedeMarcarSalida() || sal.Tipo == TipoSalida.Indisposición)
                    {
                        if (usuarioLogueado.Id != ag.Id || usuarioLogueado.Perfil == PerfilUsuario.Personal)
                        {
                            //Le esta marcando la salida un jefe
                            sal.AgenteId1 = usuarioLogueado.Id;
                        }

                        switch (Ddl_TipoSalida.SelectedItem.Text)
                        {
                            case "Particular":
                                agCxt.Estado = EstadosAgente.Salida_Particular;
                                break;
                            case "Oficial":
                                agCxt.Estado = EstadosAgente.Salida_Oficial;
                                break;
                            case "Indisposición":
                                sal.HoraDesde = "10:00";
                                sal.HoraHasta = "13:00";
                                ProcesosGlobales.AgendarEstadoDiaAgente(Session["UsuarioLogueado"] as Agente, DatosAgente1.Agente, DateTime.Today, cxt.TiposEstadoAgente.First(te => te.Estado == "Indisposición"));

                                break;
                            default:
                                break;
                        }

                        cxt.Salidas.AddObject(sal);
                        cxt.SaveChanges();

                        Session["Agente"] = agCxt;
                    }
                    else
                    {
                        Controles.MessageBox.Show(this, "Solicite la marcación de salida a su jefe, superior o a personal.", Controles.MessageBox.Tipo_MessageBox.Info);
                        RefrescarModuloSalidas();
                    }
                }
            }

            RefrescarModuloSalidas();
            VerificarEstadoAgente();
        }

        protected void btn_TerminarSalida_Click(object sender, EventArgs e)
        {
            Model1Container cxt = new Model1Container();
            Agente ag = Session["Agente"] as Agente;
            Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);
            //obtengo la salida pendiente de finalizar
            Salida sal = cxt.Salidas.First(s => s.AgenteId == ag.Id && s.HoraHasta == null);

            if (sal != null)
            {
                //la finalizo
                sal.HoraHasta = DateTime.Now.ToString("HH:mm");

                if (!HorasString.HoraNoNula(HorasString.RestarHoras(sal.HoraHasta, sal.HoraDesde)))
                {
                    //la resta dio cero, entonces la elimino
                    cxt.Salidas.DeleteObject(sal);
                }
                else
                {
                    //si es personal, agrego el registro a los movimientos diarios
                    if (sal.Tipo == TipoSalida.Particular)
                    {
                        DateTime d = DateTime.Today;
                        ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(d, agCxt, Session["UsuarioLogueado"] as Agente, HorasString.RestarHoras(sal.HoraHasta, sal.HoraDesde), cxt.TiposMovimientosHora.First(t => t.Tipo == "Salida particular"), string.Empty);
                    }
                }

                agCxt.Estado = 0;

                DatosAgente1.Agente = agCxt;

                cxt.SaveChanges();

                Session["Agente"] = agCxt;

                RefrescarModuloSalidas();

                VerificarEstadoAgente();
            }
        }

        protected void btn_CancelarNuevaSalida_Click(object sender, EventArgs e)
        {
            btn_NuevaSalida.Visible = true;
            PanelNuevaSalida.Visible = false;
        }

        protected void gridViewSalidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSalidas.PageIndex = e.NewPageIndex;
            CargarGrillaSalidas();
        }

        private void CargarGrillaSalidas()
        {
            Agente ag = Session["Agente"] as Agente;
            Model1Container cxt = new Model1Container();
            Agente agCtx = cxt.Agentes.First(a => a.Id == ag.Id);
            DateTime d = DateTime.Today;

            var items = (from s in agCtx.Salidas
                         where s.Dia == d && s.HoraHasta != null
                         select new
                         {
                             Tipo = s.Tipo,
                             Desde = s.HoraDesde,
                             Hasta = s.HoraHasta,
                             Destino = s.Destino,
                             Diferencia = HorasString.RestarHoras(s.HoraHasta, s.HoraDesde),
                             MarcoJefe = s.Jefe != null ? s.Jefe.ApellidoYNombre : " - "
                         }).ToList();

            GridViewSalidas.DataSource = items;
            GridViewSalidas.DataBind();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Ddl_TipoSalida.SelectedItem.Text != "Ninguno";
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Model1Container cxt = Session["CXT"] as Model1Container;
            Agente ag = Session["Agente"] as Agente;
            DateTime d = DateTime.Today;

            if (((TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue)) == TipoSalida.Indisposición)
            {
                args.IsValid = ag.Salidas.Where(s => s.Tipo == TipoSalida.Indisposición && s.Dia.Month == d.Month && s.Dia.Year == d.Year).Count() == 0;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void CustomValidator11_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Model1Container cxt = Session["CXT"] as Model1Container;
            Agente ag = Session["Agente"] as Agente;
            DateTime d = DateTime.Today;

            if (((TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue)) == TipoSalida.Oficial || ((TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue)) == TipoSalida.Particular)
            {
                args.IsValid = tb_DestinoSalida.Text.Length > 0;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void CustomValidator10_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Model1Container cxt = Session["CXT"] as Model1Container;
            Agente ag = Session["Agente"] as Agente;
            DateTime d = DateTime.Today;

            var salidas = from s in ag.Salidas
                          where
                            s.Dia.Year == DateTime.Today.Year &&
                            s.Tipo == TipoSalida.Particular
                          select s;

            string horasAcumuladas = "000:00";

            foreach (Salida s in salidas)
            {
                string horasSalida = HorasString.RestarHoras(s.HoraHasta, s.HoraDesde);
                horasAcumuladas = HorasString.SumarHoras(new string[] { horasAcumuladas, horasSalida });
            }

            if (Convert.ToInt32(horasAcumuladas.Split(':')[0]) >= 40)
            {
                if (((TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue)) == TipoSalida.Indisposición || ((TipoSalida)Convert.ToInt32(Ddl_TipoSalida.SelectedValue)) == TipoSalida.Oficial)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }

        #endregion

        #region Horario Vespertino

        private void RefrescarModuloHV()
        {
            Session["NuevoHV"] = "No";
            OcultarPanelHV();
            CargarGrillaHV();
        }

        private void OcultarPanelHV()
        {
            PanelSolicitarHV.Visible = false;

            tb_Dia.Value = string.Empty;
            tb_HoraDesde.Value = string.Empty;
            tb_HoraHasta.Value = string.Empty;
            tb_MotivoHV.Text = string.Empty;

            btn_NuevaSolicitudHV.Visible = true;
        }

        private void CargarGrillaHV()
        {
            //ProcesosGlobales.CancelarSolicitudesHVPorVencimiento();

            Agente ag = Session["Agente"] as Agente;
            Model1Container cxt = new Model1Container();
            var hvs = cxt.HorariosVespertinos.Where(
                                        hv => hv.AgenteId == ag.Id && 
                                        (
                                            (hv.Dia.Month == DateTime.Today.Month && hv.Dia.Year == DateTime.Today.Year) 
                                            || hv.Estado == EstadosHorarioVespertino.Solicitado 
                                            || hv.Estado == EstadosHorarioVespertino.Aprobado)
                                        ).ToList();
            var items = (from hv in hvs
                         select new
                         {
                             Id = hv.Id,
                             Estado = hv.Estado,
                             Dia = hv.Dia,
                             Desde = hv.HoraInicio,
                             Hasta = hv.HoraFin,
                             Motivo = hv.Motivo,
                             Horas = hv.Horas,
                             Jefe = hv.Jefe != null ? hv.Jefe.ApellidoYNombre : hv.Agente.ApellidoYNombre
                         }).OrderByDescending(d => d.Dia).ToList();

            GridViewHV.DataSource = items;
            GridViewHV.DataBind();
        }

        protected void gridViewHV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHV.PageIndex = e.NewPageIndex;
            CargarGrillaHV();
        }

        protected void btn_EliminarSolicitudHV_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Model1Container cxt = new Model1Container();
            HorarioVespertino hv = cxt.HorariosVespertinos.First(hvs => hvs.Id == id);
            if (hv.Estado == EstadosHorarioVespertino.Solicitado)
            {
                cxt.HorariosVespertinos.DeleteObject(hv);
                cxt.SaveChanges();
                RefrescarModuloHV();
            }
            else
            {
                Controles.MessageBox.Show(this, "No se puede eliminar la solicitud ya que fue aprobada por el jefe", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_NuevaSolicitudHV_Click(object sender, EventArgs e)
        {
            Session["NuevoHV"] = "Si";
            PanelSolicitarHV.Visible = true;
            btn_NuevaSolicitudHV.Visible = false;
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            OcultarPanelHV();
        }

        protected void btn_SolicitarHV_Click(object sender, EventArgs e)
        {
            if (Session["NuevoHV"].ToString() == "Si")
            {
                Page.Validate("HV");
                if (Page.IsValid)
                {
                    Model1Container cxt = new Model1Container();
                    Agente ag = Session["Agente"] as Agente;
                    Agente agenteDelContexto = cxt.Agentes.First(a => a.Id == ag.Id);
                    HorarioVespertino hv = new HorarioVespertino();
                    hv.Dia = Convert.ToDateTime(tb_Dia.Value);
                    hv.HoraInicio = tb_HoraDesde.Value;
                    hv.HoraFin = tb_HoraHasta.Value;
                    hv.Estado = EstadosHorarioVespertino.Solicitado;
                    hv.Motivo = tb_MotivoHV.Text;

                    //con estos datos verifico que ya no haya cargado el mismo horario vespertino (hace para atras con el boton del browser)
                    HorarioVespertino hvexiste = agenteDelContexto.HorariosVespertinos.FirstOrDefault(
                                                    hhv => hhv.Dia == hv.Dia &&
                                                    hhv.HoraInicio == hv.HoraInicio &&
                                                    hhv.HoraFin == hv.HoraFin &&
                                                    hhv.Estado == hv.Estado &&
                                                    hhv.Motivo == hv.Motivo
                                                    );
                    if (hvexiste == null)
                    {
                        agenteDelContexto.HorariosVespertinos.Add(hv);
                        cxt.SaveChanges();
                    }
                }
            }
            RefrescarModuloHV();
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_Dia.Value, out d) && d >= DateTime.Today;
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                args.IsValid = !(HorasString.RestarHoras(tb_HoraHasta.Value, tb_HoraDesde.Value).Contains("-"));
            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Convert.ToDateTime(tb_Dia.Value);
            args.IsValid = ObtenerEstadoAgenteParaElDia(d) == null;
        }

        protected void CustomValidator12_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d = Convert.ToDateTime(tb_Dia.Value);
            args.IsValid = !ExisteHorarioVespertinoDia(d);
        }

        private bool ExisteHorarioVespertinoDia(DateTime d)
        {
            Agente ag = Session["Agente"] as Agente;
            bool ret = false;
            using (var cxt = new Model1Container())
            {
                HorarioVespertino hvexiste = cxt.HorariosVespertinos.FirstOrDefault(
                                                    hhv => hhv.Dia == d &&
                                                    hhv.AgenteId == ag.Id
                                                    );
                ret = hvexiste != null;
            }

            return ret;
        }

        protected void btn_administrar_hv_Click(object sender, ImageClickEventArgs e)
        {
            int id_hv = int.Parse(((ImageButton)sender).CommandArgument);

            using (var cxt = new Model1Container())
            {
                HorarioVespertino hv = cxt.HorariosVespertinos.First(x=>x.Id == id_hv);

                if (hv.Estado == EstadosHorarioVespertino.Aprobado)
                {
                    Session["Id"] = ((ImageButton)sender).CommandArgument;

                    Response.Redirect("~/Aplicativo/Personal_TerminarHV.aspx");
                }

            }
            MessageBox.Show(this, "Puede administrar únicamente horarios vespertinos con estado Aprobado.-", MessageBox.Tipo_MessageBox.Danger);
            
        }

        #endregion

        #region Francos

        private void RefrescarModuloFrancos()
        {
            Session["NuevoFC"] = "No";
            CargarGrillaFrancos();
            gv_MovFrancos.DataSource = null;
            gv_MovFrancos.DataBind();
            OcultarPanelFranco();
        }

        protected void btn_NuevaSolicitudFranco_Click(object sender, EventArgs e)
        {
            Session["NuevoFC"] = "Si";
            btn_SolicitarFranco.Visible = false;
            PanelSolicitarFranco.Visible = true;
        }

        protected void btn_EliminarSolicitudFC_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Model1Container cxt = new Model1Container();
            Franco fc = cxt.Francos.First(f => f.Id == id);
            if (fc.Estado == EstadosFrancos.Solicitado)
            {
                TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Franco compensatorio p/aprobar");
                Agente ag = Session["Agente"] as Agente;

                ProcesosGlobales.EliminarAgendaEstadoDiaAgente(ag, fc.DiasFranco.First().Dia, tea);

                while (fc.DiasFranco.Count > 0)
                {
                    cxt.DiasFrancos.DeleteObject(fc.DiasFranco.First());
                }

                while (fc.MovimientosFranco.Count > 0)
                {
                    cxt.MovimientosFrancos.DeleteObject(fc.MovimientosFranco.First());
                }



                cxt.Francos.DeleteObject(fc);

                cxt.SaveChanges();

                RefrescarModuloFrancos();
            }
            else
            {
                Controles.MessageBox.Show(this, "Se pueden eliminar únicamente los registros que tienen estado \"Solicitado\".", Controles.MessageBox.Tipo_MessageBox.Info, "Imposible eliminar");
            }
        }

        protected void btn_EliminarSolicitudArt_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Model1Container cxt = new Model1Container();
            EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(x => x.Id == id);
            Agente ag = Session["Agente"] as Agente;
            if (ea != null && (cxt.Salidas.FirstOrDefault(ss => ss.Tipo == TipoSalida.Particular && ss.Dia == ea.Dia && ss.AgenteId == ea.AgenteId)) == null)
            {
                ProcesosGlobales.EliminarAgendaEstadoDiaAgente(ag, ea.Dia, ea.TipoEstado);
                RefrescarModuloFrancos();
            }
            else
            {
                Controles.MessageBox.Show(this, "No se puede eliminar el Art. el mismo ya fue impactado ante la solicitud y ausencia del agente.", Controles.MessageBox.Tipo_MessageBox.Info, "Imposible eliminar");
            }

        }

        protected void btn_AceptarFranco_Click(object sender, EventArgs e)
        {
            if (Session["NuevoFC"].ToString() == "Si")
            {
                if (ddl_tipo_solicitud_franco.SelectedItem.Text == "Franco compensatorio")
                {
                    Page.Validate("Francos");
                    if (Page.IsValid)
                    {
                        AgendarSolicitudFranco();
                        RefrescarModuloFrancos();
                    }
                }
                else
                {
                    Page.Validate("articulo");
                    if (Page.IsValid)
                    {
                        AgendarSolicitudArticulo();
                        RefrescarModuloFrancos();
                    }
                }
            }
        }

        private void AgendarSolicitudFranco()
        {
            Agente ag = Session["Agente"] as Agente;
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);

            Franco f = new Franco();
            f.AgenteId = ag.Id;
            f.FechaSolicitud = DateTime.Today;

            DiaFranco d = new DiaFranco();
            d.Dia = Convert.ToDateTime(tb_FechaFranco.Value);
            f.DiasFranco.Add(d);

            MovimientoFranco mf = new MovimientoFranco();
            mf.Estado = EstadosFrancos.Solicitado;
            mf.Fecha = DateTime.Today;
            mf.AgenteId = ag.Id;

            var otroFC = from fc in agCxt.Francos
                         where
                         fc.DiasFranco.Where(df => df.Dia == d.Dia).Count() > 0 &&
                         fc.Estado == mf.Estado
                         select fc;

            //si no existe otra solicitud ese día con estado solicitada lo agendo.
            if (otroFC.Count() == 0)
            {
                f.MovimientosFranco.Add(mf);

                cxt.Francos.AddObject(f);

                cxt.SaveChanges();

                TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Franco compensatorio p/aprobar");

                ProcesosGlobales.AgendarEstadoDiaAgente(agCxt, agCxt, Convert.ToDateTime(tb_FechaFranco.Value), tea);
            }
        }

        private void AgendarSolicitudArticulo()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            Agente ag = Session["Agente"] as Agente;

            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);
            Agente usuarioLogueadoCxt = cxt.Agentes.First(a => a.Id == usuarioLogueado.Id);

            DateTime d;
            d = Convert.ToDateTime(tb_FechaFranco.Value);

            TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Razones particulares");

            ProcesosGlobales.AgendarEstadoDiaAgente(usuarioLogueadoCxt, agCxt, Convert.ToDateTime(tb_FechaFranco.Value), tea);
        }

        protected void btn_CancelarFranco_Click(object sender, EventArgs e)
        {
            OcultarPanelFranco();
        }

        private void OcultarPanelFranco()
        {
            PanelSolicitarFranco.Visible = false;
            tb_FechaFranco.Value = string.Empty;

            btn_SolicitarFranco.Visible = true;
        }

        protected void GridViewFrancos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFrancos.PageIndex = e.NewPageIndex;
            CargarGrillaFrancos();
        }

        protected void GridViewArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewArticulos.PageIndex = e.NewPageIndex;
            CargarGrillaFrancos();
        }

        protected void btn_VerMovimientos_Click(object sender, ImageClickEventArgs e)
        {
            LimpiarResaltado();

            GridViewRow row = ((GridViewRow)((Control)sender).Parent.Parent);
            row.Font.Bold = true;
            row.ForeColor = Color.Black;
            int idFranco = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());

            CargarMovimientos(idFranco);
        }

        private void LimpiarResaltado()
        {
            foreach (GridViewRow gvr in GridViewFrancos.Rows)
            {
                gvr.Font.Bold = false;
                gvr.ForeColor = Color.Gray;
            }
        }

        private void CargarMovimientos(int idFranco)
        {
            Model1Container cxt = new Model1Container();
            Franco f = cxt.Francos.FirstOrDefault(fr => fr.Id == idFranco);
            var movimientos = (from m in f.MovimientosFranco
                               select new
                               {
                                   Fecha = m.Fecha,
                                   Agente = m.Agente.ApellidoYNombre,
                                   Estado = m.Estado,
                                   Observacion = m.Observacion
                               }).ToList();

            gv_MovFrancos.DataSource = movimientos;
            gv_MovFrancos.DataBind();

            modalHistMov.Attributes["aria-hidden"] = "false";
            modalHistMov.Attributes["class"] = "modal fade in";
            modalHistMov.Attributes["style"] = "display: block;";
        }

        protected void btn_cerrar_modal_ServerClick(object sender, EventArgs e)
        {
            modalHistMov.Attributes["aria-hidden"] = "true";
            modalHistMov.Attributes["class"] = "modal fade";
            modalHistMov.Attributes["style"] = "display: none;";
        }

        private void CargarGrillaFrancos()
        {
            Agente ag = Session["Agente"] as Agente;
            Model1Container cxt = new Model1Container();
            Agente agenteDelContexto = cxt.Agentes.First(a => a.Id == ag.Id);
            TipoEstadoAgente tea = cxt.TiposEstadoAgente.FirstOrDefault(tt => tt.Estado == "Razones particulares");


            var items = (from fc in agenteDelContexto.Francos
                         where ((fc.FechaSolicitud.Month == DateTime.Today.Month) && (fc.FechaSolicitud.Year == DateTime.Today.Year))
                         || (fc.Estado != EstadosFrancos.Aprobado && fc.Estado != EstadosFrancos.Cancelado && fc.Estado != EstadosFrancos.CanceladoAutomatico)
                         select new
                         {
                             Id = fc.Id,
                             Estado = fc.Estado,
                             Dia = fc.FechaSolicitud,
                             DiaInicial = (from d in fc.DiasFranco select d.Dia).Min(),
                             CantidadDias = fc.DiasFranco.Count,
                             Horas = fc.DiasFranco.Count * 7
                         }).ToList();

            GridViewFrancos.DataSource = items;
            GridViewFrancos.DataBind();

            List<EstadoAgente> estados_articulo_mes = (from ea in cxt.EstadosAgente
                                                       where ea.AgenteId == ag.Id && ea.Dia.Month == DateTime.Today.Month && ea.Dia.Year == DateTime.Today.Year && ea.TipoEstadoAgenteId == tea.Id
                                                       select ea
                                                        ).ToList();

            var items_grilla_articulo = (from ea in estados_articulo_mes
                                         select new
                                         {
                                             Id = ea.Id,
                                             DiaInicial = ea.Dia,
                                             Estado = (cxt.Salidas.FirstOrDefault(ss => ss.Tipo == TipoSalida.Particular && ss.Dia == ea.Dia && ss.AgenteId == ag.Id)) != null ? "Aprobado/Impactado" : "Solicitado",
                                             Horas = "06:30"
                                         }).ToList();

            GridViewArticulos.DataSource = items_grilla_articulo;
            GridViewArticulos.DataBind();
        }

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_FechaFranco.Value, out d) && d >= DateTime.Today;
        }

        protected void CustomValidator6_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Agente ag = Session["Agente"] as Agente;
            Model1Container cxt = new Model1Container();
            Agente agenteDelContexto = cxt.Agentes.First(a => a.Id == ag.Id);

            DateTime diaSolicitado;
            DateTime.TryParse(tb_FechaFranco.Value, out diaSolicitado);

            var francosTomadosPorAprobar = from f in agenteDelContexto.Francos
                                           where
                                                f.Estado != EstadosFrancos.Cancelado &&
                                                f.Estado != EstadosFrancos.CanceladoAutomatico &&
                                                f.Estado != EstadosFrancos.Aprobado &&
                                                f.DiasFranco.First().Dia.Month == diaSolicitado.Month &&
                                                f.DiasFranco.First().Dia.Year == diaSolicitado.Year
                                           select f;

            args.IsValid = francosTomadosPorAprobar.Count() < 2;
        }

        protected void CustomValidator7_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Agente ag = Session["Agente"] as Agente;

            DateTime diaSolicitado;
            DateTime.TryParse(tb_FechaFranco.Value, out diaSolicitado);
            int francos_por_aprobar_O_APROBADOS = 0;
            using (var cxt = new Model1Container())
            {
                int prueba = (from ff in cxt.Francos
                              join df in cxt.DiasFrancos on ff.Id equals df.FrancoId
                              where df.Dia >= DateTime.Today && ff.AgenteId == ag.Id
                              select new { fId = ff.Id, fEstado = ff.Estado }).Count(f => f.fEstado != EstadosFrancos.Cancelado && f.fEstado != EstadosFrancos.CanceladoAutomatico);

                //APROBADOS TAMBIEN TENGO QUE TENER EN CUENTA YA QUE A PARTIR DE ESTA NUEVA IMPLEMENTACION LAS HORAS NO SE APLICAN AUTOMATICAMENTE AL MOMENTO DE LA APROBACION, LO HACEN RECIEN EN EL DIA QUE SE CUMPLIO EL FRANCO
                francos_por_aprobar_O_APROBADOS = prueba;//cxt.Francos.Count(ff => ff.AgenteId == ag.Id && ff.Estado != EstadosFrancos.Cancelado && ff.Estado != EstadosFrancos.CanceladoAutomatico /*&& ff.Estado != EstadosFrancos.Aprobado*/);
            }

            //var francosTomadosPorAprobar = from f in ag.Francos
            //                               where
            //                                    f.Estado != EstadosFrancos.Cancelado &&
            //                                    f.Estado != EstadosFrancos.CanceladoAutomatico &&
            //                                    f.Estado != EstadosFrancos.Aprobado
            //                               select f;

            int horasATomar = 7 * (francos_por_aprobar_O_APROBADOS + 1);

            string horasDisponibles = "00:00";

            if ((ag.HorarioFlexible ?? false) == true)
            {
                HorasMesHorarioFlexible hmhf = ag.HorasMesHorarioFlexibles.FirstOrDefault(hm => hm.Mes == DateTime.Today.Month && hm.Anio == DateTime.Today.Year);
                string horasAcumuladasMes = hmhf != null ? hmhf.HorasAcumuladas : "00:00";
                horasDisponibles = HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioActual, ag.HorasAcumuladasAnioAnterior, horasAcumuladasMes });
            }
            else
            {
                horasDisponibles = HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioActual, ag.HorasAcumuladasAnioAnterior });
            }

            args.IsValid = horasATomar <= Convert.ToInt32(horasDisponibles.Split(':')[0]);
        }

        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            if (DateTime.TryParse(tb_FechaFranco.Value, out d))
            {
                args.IsValid = ObtenerEstadoAgenteParaElDia(d) == null;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void validator_articulo4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = VerificarSiPuedeMarcarSalida();
        }

        #endregion

        #region Huellas

        public List<Notificacion> Notificaciones_por_enviar { get; set; }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
            }
            else
            {
                if (e.Day.Date <= DateTime.Today)
                {
                    using (var cxt = new Model1Container())
                    {
                        Notificaciones_por_enviar = (from nn in cxt.Notificaciones
                                                     where
                                                     nn.AgenteId == DatosAgente1.Agente.Id &&
                                                     nn.HistorialEstadosNotificacion.FirstOrDefault(nh => nh.Estado.Estado == "Enviada") == null
                                                     select nn).ToList();

                        Notificacion notFecha = Notificaciones_por_enviar.FirstOrDefault(nn => nn.HistorialEstadosNotificacion.First(nh => nh.Estado.Estado == "Generada").Fecha.Date == e.Day.Date);

                        if (notFecha != null)
                        {
                            e.Cell.BackColor = Color.OrangeRed;
                            e.Cell.ToolTip = "Debe envio de notificación N° " + notFecha.Id.ToString();
                        }
                        else
                        {
                            DS_Marcaciones ds = ProcesosGlobales.ObtenerMarcaciones(e.Day.Date, DatosAgente1.Agente.Legajo.ToString());
                            if (ds.Marcacion.Rows.Count == 0)
                            {
                                if (DatosAgente1.Agente.ObtenerEstadoAgenteParaElDia(e.Day.Date) == null)
                                {
                                    e.Cell.BackColor = Color.Red;
                                    e.Cell.ToolTip = "Figura ausente";
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            CargarMarcacionesFecha();
        }

        private void CargarMarcacionesFecha()
        {
            using (var cxt = new SisPer.Aplicativo.ClockCardEntities())
            {
                Agente ag = Session["Agente"] as Agente;
                DateTime fechaSeleccionada = Calendar1.SelectedDate;
                DS_Marcaciones marcaciones = ProcesosGlobales.ObtenerMarcacionesConProvisorias(fechaSeleccionada, ag.Legajo.ToString());

                var itemsGrillaMarcaciones = (from m in marcaciones.Marcacion
                                              select new
                                              {
                                                  Legajo = m.Legajo,
                                                  Fecha = m.Fecha,
                                                  Hora = m.Hora,
                                                  MarcaManual = m.MarcaManual,
                                                  EsDefinitivo = m.EsDefinitivo
                                              }).ToList();

                gv_Huellas.DataSource = itemsGrillaMarcaciones.OrderBy(i => i.Hora).ToList();
                gv_Huellas.DataBind();

                EntradaSalida es = ag.EntradasSalidas.FirstOrDefault(eess => eess.Fecha == Calendar1.SelectedDate);

                if (ag.MarcaManual(Calendar1.SelectedDate))
                {
                    div_ES.Visible = true;

                    if (es != null)
                    {
                        if (es.Entrada == "00:00")
                        {
                            btn_registrar_entrada_laboral.Visible = true;
                            lbl_hora_entrada_manual_registrada.Visible = false;
                        }
                        else
                        {
                            btn_registrar_entrada_laboral.Visible = false;
                            lbl_hora_entrada_manual_registrada.Visible = true;

                            lbl_hora_entrada_manual_registrada.Text = es.Entrada;
                        }

                        if (es.Salida == "00:00")
                        {
                            btn_registrar_salida_laboral.Visible = true;
                            lbl_hora_salida_manual_registrada.Visible = false;
                        }
                        else
                        {
                            btn_registrar_salida_laboral.Visible = false;
                            lbl_hora_salida_manual_registrada.Visible = true;

                            lbl_hora_salida_manual_registrada.Text = es.Salida;
                        }
                    }
                    else
                    {
                        btn_registrar_entrada_laboral.Visible = true;
                        lbl_hora_entrada_manual_registrada.Visible = false;

                        btn_registrar_salida_laboral.Visible = true;
                        lbl_hora_salida_manual_registrada.Visible = false;
                    }

                }
                else
                {
                    div_ES.Visible = false;
                }
            }
        }

        protected void gv_Huellas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Huellas.PageIndex = e.NewPageIndex;
            CargarMarcacionesFecha();
        }

        protected void btn_registrar_entrada_laboral_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate == DateTime.Today)
            {
                Model1Container cxt = new Model1Container();
                Agente ag = Session["Agente"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);
                string hora_Entrada = DateTime.Now.ToString("HH:mm");

                ResumenDiario rd = agCxt.ObtenerResumenDiario(Calendar1.SelectedDate);
                if (rd == null)
                {
                    rd = new ResumenDiario()
                    {
                        Dia = Calendar1.SelectedDate,
                        Horas = "00:00",
                        AgenteId = ag.Id,
                        HEntrada = "00:00",
                        HSalida = "00:00",
                        HVEnt = "00:00",
                        HVSal = "00:00",
                        MarcoTardanza = false,
                        MarcoProlongJornada = false,
                        Inconsistente = false,
                        ObservacionInconsistente = "",
                        Cerrado = false,
                        AcumuloHorasBonificacion = "00:00",
                        AcumuloHorasAnioActual = "00:00",
                        AcumuloHorasMes = "00:00",
                        AgenteId1 = null
                    };

                    cxt.ResumenesDiarios.AddObject(rd);
                    cxt.SaveChanges();
                }
                
                rd.HEntrada = hora_Entrada;

                Marcacion marcacion = new Marcacion()
                {
                    Hora = hora_Entrada,
                    Manual = true,
                    Anulada = false,
                    ResumenDiarioId = rd.Id
                };
                
                cxt.Marcaciones.AddObject(marcacion);

                EntradaSalida e_s = agCxt.EntradasSalidas.FirstOrDefault(io => io.Fecha == Calendar1.SelectedDate);

                if (e_s == null)
                {
                    e_s = new EntradaSalida();
                    e_s.Fecha = Calendar1.SelectedDate;
                    e_s.Entrada = "00:00";
                    e_s.Salida = "00:00";
                    cxt.EntradasSalidas.AddObject(e_s);
                }

                e_s.Entrada = hora_Entrada;
                e_s.AgenteId = ag.Id;
                e_s.AgenteId1 = ag.Id;
                e_s.Enviado = true;
                e_s.CerradoPersonal = true;

                cxt.SaveChanges();

                btn_registrar_entrada_laboral.Visible = false;
                lbl_hora_entrada_manual_registrada.Visible = true;
                lbl_hora_entrada_manual_registrada.Text = e_s.Entrada;

                Session["Agente"] = cxt.Agentes.FirstOrDefault(a => a.Id == ag.Id);
            }
            else
            {
                MessageBox.Show(this, "No esta permitido ingresar registros en fechas distintas a la actual.", MessageBox.Tipo_MessageBox.Danger, "No es posible registrar movimiento");
            }
        }

        protected void btn_registrar_salida_laboral_Click(object sender, EventArgs e)
        {
            if (Calendar1.SelectedDate == DateTime.Today)
            {
                Model1Container cxt = new Model1Container();
                Agente ag = Session["Agente"] as Agente;
                Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);

                EntradaSalida e_s = agCxt.EntradasSalidas.FirstOrDefault(io => io.Fecha == Calendar1.SelectedDate);

                if (e_s == null)
                {
                    MessageBox.Show(this, "Debe registrar primero la entrada!", MessageBox.Tipo_MessageBox.Danger);
                }
                else
                {
                    string hora_Entrada = e_s.Entrada;
                    string Hora_Salida = DateTime.Now.ToString("HH:mm");

                    ResumenDiario rd = agCxt.ObtenerResumenDiario(Calendar1.SelectedDate);
                    rd.HSalida = Hora_Salida;

                    Marcacion marcacion = new Marcacion()
                    {
                        Hora = Hora_Salida,
                        Manual = true,
                        Anulada = false,
                        ResumenDiarioId = rd.Id
                    };

                    cxt.Marcaciones.AddObject(marcacion);

                    e_s.Fecha = Calendar1.SelectedDate;
                    e_s.Entrada = hora_Entrada;
                    e_s.Salida = Hora_Salida;
                    e_s.AgenteId = ag.Id;
                    e_s.AgenteId1 = ag.Id;
                    e_s.Enviado = true;
                    e_s.CerradoPersonal = true;

                    cxt.SaveChanges();

                    btn_registrar_salida_laboral.Visible = false;
                    lbl_hora_salida_manual_registrada.Visible = true;
                    lbl_hora_salida_manual_registrada.Text = e_s.Salida;

                    Session["Agente"] = cxt.Agentes.FirstOrDefault(a => a.Id == ag.Id);
                }
            }
            else
            {
                MessageBox.Show(this, "No esta permitido ingresar registros en fechas distintas a la actual.", MessageBox.Tipo_MessageBox.Danger, "No es posible registrar movimiento");
            }
        }

        #endregion

        #region Días sin cerrar

        private void RefrescarModuloDiasSinCerrar()
        {
            Cargar_dias_por_cerrar_agente();
        }

        private void Cargar_dias_por_cerrar_agente()
        {
            DateTime dia_desde = DateTime.Today.AddYears(-1);
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                var dias = (from rrdd in cxt.ResumenesDiarios
                            where
                                rrdd.AgenteId == ag.Id &&
                                rrdd.Dia >= dia_desde &&
                                (rrdd.Cerrado ?? false) == false
                            select new
                            {
                                Id = rrdd.Id,
                                Dia = rrdd.Dia,
                                Motivo = rrdd.ObservacionInconsistente
                            }
                                ).OrderBy(x => x.Dia).ToList();

                gv_dias_sin_cerrar.DataSource = dias;
                gv_dias_sin_cerrar.DataBind();
            }
        }

        protected void btn_ver_dia_sin_cerrar_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            Agente usuario_logueado = Session["UsuarioLogueado"] as Agente;

            if (ag.Id == usuario_logueado.Id)//si los usuarios son iguales no puede cambiar nada ni agregar nada ya que por mas que sea de personal no puede editar sus dias.
            {
                using (var cxt = new Model1Container())
                {
                    int id_rd = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                    ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Id == id_rd);

                    VisualizarDiaAgente.LoadControl("~/Aplicativo/Controles/AdministrarDiaAgente.ascx");

                    VisualizarDiaAgente.AgenteBuscado = ag;
                    VisualizarDiaAgente.DiaBuscado = rd.Dia;
                    VisualizarDiaAgente.ResumenDiarioBuscado = rd;
                    VisualizarDiaAgente.CargarValores();

                    VisualizarDiaAgente.Visible = true;
                }

            }
            else
            {//Si no es el mismo tiene que ser el jefe o alguien de Personal ver si puede editar algo o que quede en manos de personal unicamente
             //por ahora lo dejo en manos de personal asi que cualquiera de los dos caminos muestra lo mismo
                using (var cxt = new Model1Container())
                {
                    int id_rd = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                    ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Id == id_rd);

                    VisualizarDiaAgente.LoadControl("~/Aplicativo/Controles/AdministrarDiaAgente.ascx");

                    VisualizarDiaAgente.AgenteBuscado = ag;
                    VisualizarDiaAgente.DiaBuscado = rd.Dia;
                    VisualizarDiaAgente.ResumenDiarioBuscado = rd;
                    VisualizarDiaAgente.CargarValores();

                    VisualizarDiaAgente.Visible = true;
                }
            }

            string script = "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#verDiaSinCerrar').modal('show')});</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDiaSinCerrar", script, false);
        }

        protected void gv_dias_sin_cerrar_PreRender(object sender, EventArgs e)
        {
            if (gv_dias_sin_cerrar.Rows.Count > 0)
            {
                gv_dias_sin_cerrar.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        #endregion

    }
}