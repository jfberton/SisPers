using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Marcaciones_Procesar : System.Web.UI.Page
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

                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente.Visible = !(ag.Jefe || ag.JefeTemporal);

                    if (Request.QueryString["check"] == null)
                    {
                        int dia = Convert.ToInt32(Request.QueryString["d"]);
                        int mes = Convert.ToInt32(Request.QueryString["m"]);
                        int anio = Convert.ToInt32(Request.QueryString["a"]);

                        DateTime diaBuscado = new DateTime(anio, mes, dia);

                        lbl_dia.Text = diaBuscado.ToLongDateString();

                        Session["DiaBuscado"] = diaBuscado;
                        Session["Inconsistencias"] = new List<Agente>();
                        Session["Correctos"] = new List<Agente>();

                        AnalizarySeparar(diaBuscado);
                    }
                    else
                    {//vuelvo de reparar un agente
                        Model1Container cxt = new Model1Container();
                        //variables por parametro en la URL
                        int dia = Convert.ToInt32(Request.QueryString["d"]);
                        int mes = Convert.ToInt32(Request.QueryString["m"]);
                        int anio = Convert.ToInt32(Request.QueryString["a"]);

                        DateTime diaBuscado = new DateTime(anio, mes, dia);

                        //Variable de session en que estuvo trabajando el agente Personal
                        //modificando los valores del resumen.
                        ResumenDiario rd = Session["RD"] as ResumenDiario;

                        //obtengo el agente a traves del resumen diario
                        Agente agente = cxt.Agentes.FirstOrDefault(a => a.Id == rd.AgenteId);

                        //Variables de session en las cuales tenia cargado los items correctos e incorrectos antes de dejar esta pagina
                        //es decir la primera ves que se llamo a la misma.
                        List<itemsGrillaInconsistente> itemsInc = Session["ListadoDeItemsInconsistentes"] as List<itemsGrillaInconsistente>;
                        List<itemsGrillaCorrecto> itemsCo = Session["ListadoDeItemsCorrectos"] as List<itemsGrillaCorrecto>;

                        if (rd.Inconsistente)
                        {
                            //verifico que no este entre los correctos, si es asi lo quito y lo agrego a los incorrectos
                            //sino no hace falta hacer nada.
                            if (itemsCo.Count(ic => ic.Id == rd.AgenteId) > 0)
                            {
                                itemsGrillaCorrecto igc = itemsCo.First(ic => ic.Id == rd.AgenteId);
                                int indiceaBorrar = itemsCo.IndexOf(igc);
                                itemsCo.RemoveAt(indiceaBorrar);

                                Area area = agente.AreaId.HasValue ? (cxt.Areas.First(aa => aa.Id == agente.AreaId)) : null;
                                itemsInc.Add(new itemsGrillaInconsistente() { 
                                    Id = agente.Id, 
                                    Agente = agente.ApellidoYNombre, 
                                    Interior = area != null ? (area.Interior.HasValue ? area.Interior.Value : false) : false,
                                    Legajo = agente.Legajo, 
                                    Observaciones = rd.ObservacionInconsistente });
                            }
                        }
                        else
                        { //resumen correcto

                            //verifico que no este entre los incorrectos, si es asi lo quito y lo agrego a los correctos
                            //SINO VERIFICO SI ESTA CERRADO O SI NECESITA MARCARSE COMO CERRADO.
                            if (itemsInc.Count(ic => ic.Id == rd.AgenteId) > 0)
                            {
                                itemsGrillaInconsistente igi = itemsInc.First(ic => ic.Id == rd.AgenteId);
                                int indiceaBorrar = itemsInc.IndexOf(igi);
                                itemsInc.RemoveAt(indiceaBorrar);
                                itemsGrillaCorrecto igc = new itemsGrillaCorrecto()
                                                                {
                                                                    Id = agente.Id,
                                                                    Agente = agente.ApellidoYNombre,
                                                                    Legajo = agente.Legajo,
                                                                    Entrada = rd.HEntrada,
                                                                    Salida = rd.HSalida,
                                                                    Observaciones = rd.ObservacionInconsistente,
                                                                    HorasTrabajadas = agente.ObtenerHoraEntradaLaboral(rd.Dia) == "06:30" ?
                                                                                                HorasString.RestarHoras(rd.HSalida, agente.ObtenerHoraEntradaLaboral(rd.Dia)) :
                                                                                                HorasString.RestarHoras(rd.HSalida, rd.HEntrada),
                                                                    Cerrado = rd.Cerrado.HasValue && rd.Cerrado == true
                                                                };

                                igc = AnalizarResumenDiarioCorrecto(igc);

                                itemsCo.Add(igc);
                            }
                            else
                            {
                                // VERIFICO SI ESTA CERRADO O SI NECESITA MARCARSE COMO CERRADO.
                                itemsGrillaCorrecto igc = itemsCo.First(ic => ic.Id == rd.AgenteId);
                                int i = itemsCo.IndexOf(igc);
                                itemsCo.RemoveAt(i);

                                igc.Cerrado = rd.Cerrado.HasValue && rd.Cerrado == true;
                                igc = AnalizarResumenDiarioCorrecto(igc);
                                itemsCo.Add(igc);
                            }
                        }

                        Session["ListadoDeItemsInconsistentes"] = itemsInc;
                        Session["ListadoDeItemsCorrectos"] = itemsCo;

                        RefrescarGrillaConsistentes();
                        RefrescarGrillaInconsistentes_CC();
                        RefrescarGrillaInconsistentes_Interior();

                       if (itemsInc.Count == 0)
                        {
                            if (!cxt.DiasProcesados.First(d => d.Dia == diaBuscado).Cerrado)
                            {
                                div_DiaCorrecto.Visible = true;
                                div_DiaIncorrecto.Visible = false;
                            }
                        }
                        else
                        {
                            div_DiaCorrecto.Visible = false;
                            div_DiaIncorrecto.Visible = true;
                        }


                    }
                }
            }
        }

        private void AnalizarySeparar(DateTime diaBuscado)
        {
            List<Agente> inconsistentes = Session["Inconsistencias"] as List<Agente>;
            List<Agente> correctos = Session["Correctos"] as List<Agente>;

            using (Model1Container cxt = new Model1Container())
            {
                try
                {
                    List<Agente> agentes = cxt.Agentes.Where(a => a.FechaBaja == null).ToList();
                    foreach (Agente ag in agentes)
                    {
                        ResumenDiario agRd = cxt.ResumenesDiarios.FirstOrDefault(resdia => resdia.AgenteId == ag.Id && resdia.Dia == diaBuscado);
                        if (agRd == null)
                        {
                            ResumenDiario rd = new ResumenDiario();
                            ///////////////
                            rd.Dia = diaBuscado;
                            rd.HEntrada = "000:00";
                            rd.HSalida = "000:00";
                            rd.HVEnt = "000:00";
                            rd.HVSal = "000:00";
                            rd.Horas = "000:00";
                            rd.AgenteId = ag.Id;
                            rd.Inconsistente = true;
                            rd.MarcoTardanza = false;
                            rd.MarcoProlongJornada = false;
                            rd.ObservacionInconsistente = "Ausente.";

                            EstadoAgente ea = ag.ObtenerEstadoAgenteParaElDia(diaBuscado);

                            if (ea != null || (ag.HorarioFlexible.HasValue && ag.HorarioFlexible==true))
                            {
                                if (ea != null)
                                {
                                    rd.Inconsistente = false;
                                    rd.ObservacionInconsistente = ea.TipoEstado.Estado;
                                    correctos.Add(ag);
                                }
                                else
                                {
                                    rd.Inconsistente = false;
                                    rd.ObservacionInconsistente = "Horario flexible";
                                    correctos.Add(ag);
                                }
                            }
                            else
                            {
                                inconsistentes.Add(ag);
                            }

                            cxt.ResumenesDiarios.AddObject(rd);
                            cxt.SaveChanges();
                        }
                        else
                        {
                            //Si no esta cerrado lo proceso nuevamente por si hubo cambios.
                            if (!(agRd.Cerrado.HasValue && agRd.Cerrado == true))
                            {
                                agRd = ProcesosGlobales.AnalizarResumenDiario(agRd);
                            }

                            if (!agRd.Inconsistente)
                            {
                                correctos.Add(ag);
                            }
                            else
                            {
                                inconsistentes.Add(ag);
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                Session["Inconsistencias"] = inconsistentes;
                Session["Correctos"] = correctos;
            }

            CargarCorrectos();
            CargarInconsistentes();
        }

        private struct itemsGrillaInconsistente
        {
            public int Id { get; set; }
            public string Agente { get; set; }
            public int Legajo { get; set; }
            public bool Interior { get; set; }
            public string Observaciones { get; set; }
        }

        private struct itemsGrillaCorrecto
        {
            public int Id { get; set; }
            public string Agente { get; set; }
            public int Legajo { get; set; }
            public string Entrada { get; set; }
            public string Salida { get; set; }
            public string HorasTrabajadas { get; set; }

            public string Observaciones { get; set; }
            
            public string Tardanza { get; set; }
            public string ProlJorn { get; set; }
            public string SalidaAntesHora { get; set; }

            public bool Cerrado { get; set; }
        }

        private void CargarInconsistentes()
        {
            List<Agente> inconsistentes = Session["Inconsistencias"] as List<Agente>;
            List<itemsGrillaInconsistente> items = new List<itemsGrillaInconsistente>();
            DateTime diaBuscado = Convert.ToDateTime(Session["DiaBuscado"]);
            Model1Container cxt = new Model1Container();

            foreach (Agente item in inconsistentes)
            {
                ResumenDiario rd = item.ObtenerResumenDiario(diaBuscado);
                Area area = item.AreaId.HasValue ? (cxt.Areas.First(aa => aa.Id == item.AreaId.Value)) : null;
                items.Add(new itemsGrillaInconsistente() { Id = item.Id, Agente = item.ApellidoYNombre, Interior = area != null ? (area.Interior.HasValue ? area.Interior.Value : false) : false, Legajo = item.Legajo, Observaciones = rd.ObservacionInconsistente });
            }

            Session["ListadoDeItemsInconsistentes"] = items;

            gv_Inconsistencias_CC.DataSource = items.Where(ii => !ii.Interior).OrderBy(i => i.Legajo).ToList();
            gv_Inconsistencias_CC.DataBind();
            lbl_Cant_Incons_CC.Text = items.Where(ii => !ii.Interior).Count().ToString();

            gv_Inconsistencias_Int.DataSource = items.Where(ii => ii.Interior).OrderBy(i => i.Legajo).ToList();
            gv_Inconsistencias_Int.DataBind();
            lbl_Cant_Incons_Int.Text = items.Where(ii => ii.Interior).Count().ToString();

            if (items.Count == 0)
            {
                if (!cxt.DiasProcesados.First(d => d.Dia == diaBuscado).Cerrado)
                {
                    div_DiaCorrecto.Visible = true;
                    div_DiaIncorrecto.Visible = false;
                }
            }
            else
            {
                div_DiaCorrecto.Visible = false;
                div_DiaIncorrecto.Visible = true;
            }
        }

        private void CargarCorrectos()
        {
            List<Agente> correctos = Session["Correctos"] as List<Agente>;
            List<itemsGrillaCorrecto> items = new List<itemsGrillaCorrecto>();
            DateTime diaBuscado = Convert.ToDateTime(Session["DiaBuscado"]);

            foreach (Agente item in correctos)
            {
                ResumenDiario rd = item.ObtenerResumenDiario(diaBuscado);
                if (rd != null)
                {
                    itemsGrillaCorrecto igc = new itemsGrillaCorrecto()
                    {
                        Id = item.Id,
                        Agente = item.ApellidoYNombre,
                        Legajo = item.Legajo,
                        Entrada = rd.HEntrada == "000:00" ? "-" : rd.HEntrada,
                        Salida = rd.HSalida == "000:00" ? "-" : rd.HSalida,
                        Observaciones = rd.ObservacionInconsistente,
                        HorasTrabajadas = item.HoraEntrada == "06:30" ?
                                                    HorasString.RestarHoras(rd.HSalida, item.HoraEntrada) :
                                                    HorasString.RestarHoras(rd.HSalida, rd.HEntrada),
                        Cerrado = rd.Cerrado.HasValue && rd.Cerrado == true
                    };

                    igc = AnalizarResumenDiarioCorrecto(igc);

                    items.Add(igc);
                }
                else
                {
                    throw new Exception("El agente " + item.ApellidoYNombre + "esta marcado como correcto el dia " + diaBuscado.ToString("dd//MM//yyyy"));
                }
            }

            Session["ListadoDeItemsCorrectos"] = items;

            gv_Correcto.DataSource = items.Where(i => i.Cerrado == false).OrderBy(i => i.Legajo).ToList();
            gv_Correcto.DataBind();

            gv_Cerrados.DataSource = items.Where(i => i.Cerrado == true).OrderBy(i => i.Legajo).ToList();
            gv_Cerrados.DataBind();

            lbl_Cant_Corr.Text = items.Count(i => i.Cerrado == false).ToString();
            lbl_cerrados.Text = items.Count(i => i.Cerrado == true).ToString();
        }

        private itemsGrillaCorrecto AnalizarResumenDiarioCorrecto(itemsGrillaCorrecto igc)
        {
            if (!igc.Cerrado)
            {
                DateTime diaBuscado = Convert.ToDateTime(Session["DiaBuscado"]);
                Model1Container cxt = new Model1Container();
                Agente ag = cxt.Agentes.First(a => a.Id == igc.Id);
                //analizo unicamente si en el dia buscado el agente no tiene ningun estado (licencias y demas) ni tiene horario flexible
                if (ag.ObtenerEstadoAgenteParaElDia(diaBuscado) == null && !(ag.HorarioFlexible.HasValue && ag.HorarioFlexible == true))
                {
                    try
                    {
                        if (igc.HorasTrabajadas == "-006:30" || igc.HorasTrabajadas == "-000:00")
                        {
                            igc.HorasTrabajadas = " - ";
                        }

                        igc.HorasTrabajadas = Recortar000(igc.HorasTrabajadas);


                        if (igc.HorasTrabajadas != " - " && (ag.HorarioFlexible.HasValue || ag.HorarioFlexible == false))
                        {//buscar tardanzas, prolongacion de jornada, salida antes de hora.

                            //Tardanzas
                            if (HorasString.AMayorQueB(igc.Entrada, HorasString.SumarHoras(new string[] { ag.ObtenerHoraEntradaLaboral(diaBuscado), "00:05" })))
                            {
                                igc.Tardanza = HorasString.RestarHoras(igc.Entrada, HorasString.SumarHoras(new string[] { ag.ObtenerHoraEntradaLaboral(diaBuscado), "000:05" }));
                                igc.Tardanza = Recortar000(igc.Tardanza);
                            }

                            //Salidas despues de hora Prolongacion de jornada
                            if (HorasString.AMayorQueB(igc.Salida, ag.ObtenerHoraSalidaLaboral(diaBuscado)))
                            {
                                string horasDeMas = HorasString.RestarHoras(igc.Salida, ag.ObtenerHoraSalidaLaboral(diaBuscado));
                                //Permito que se agende hasta una hora como prolongacion de jornada, desde ahi en mas debe hacerce
                                //un horario vespertino.
                                if (HorasString.AMayorQueB(horasDeMas, "000:30"))
                                {
                                    igc.ProlJorn = "00:30";
                                }
                                else
                                {
                                    igc.ProlJorn = horasDeMas;
                                    igc.ProlJorn = Recortar000(igc.ProlJorn);
                                }
                            }

                            //Salidas antes de hora 
                            //Controlo que la marcacion de salida es antes de la hora en la que debe salir y tambien
                            //que no tenga ninguna salida que termino a las 13
                            if (HorasString.AMayorQueB(ag.ObtenerHoraSalidaLaboral(diaBuscado), igc.Salida))
                            {
                                if (ag.Salidas.Where(s => s.Dia == diaBuscado && s.HoraHasta == "13:00").Count() == 0)
                                {
                                    igc.SalidaAntesHora = HorasString.RestarHoras(ag.ObtenerHoraSalidaLaboral(diaBuscado), igc.Salida);
                                    igc.SalidaAntesHora = Recortar000(igc.SalidaAntesHora);
                                }
                            }
                        }
                        return igc;
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message + " - en el proceso del agente: " + igc.Agente);
                    }
                }
            }

            return igc;
        }

        private string Recortar000(string p)
        {
            if (p.Split(':')[0].Substring(0, 1) == "0" && p.Length > 5)
            {
                p = p.Substring(1, 5);
            }

            return p;
        }

        /// <summary>
        /// Actualiza la grilla con las variables de session previamente cargadas 
        /// </summary>
        private void RefrescarGrillaInconsistentes_Interior()
        {
            List<itemsGrillaInconsistente> itemsInc = Session["ListadoDeItemsInconsistentes"] as List<itemsGrillaInconsistente>;
            
            gv_Inconsistencias_Int.DataSource = itemsInc.Where(ii => ii.Interior).OrderBy(i => i.Legajo).ToList();
            gv_Inconsistencias_Int.DataBind();
            lbl_Cant_Incons_Int.Text = itemsInc.Where(ii => ii.Interior).Count().ToString();
        }

        /// <summary>
        /// Actualiza la grilla con las variables de session previamente cargadas 
        /// </summary>
        private void RefrescarGrillaInconsistentes_CC()
        {
            List<itemsGrillaInconsistente> itemsInc = Session["ListadoDeItemsInconsistentes"] as List<itemsGrillaInconsistente>;

            gv_Inconsistencias_CC.DataSource = itemsInc.Where(ii => !ii.Interior).OrderBy(i => i.Legajo).ToList();
            gv_Inconsistencias_CC.DataBind();
            lbl_Cant_Incons_CC.Text = itemsInc.Where(ii => !ii.Interior).Count().ToString();
        }

        /// <summary>
        /// Actualiza la grilla con las variables de session previamente cargadas 
        /// </summary>
        private void RefrescarGrillaConsistentes()
        {
            List<itemsGrillaCorrecto> itemsCo = Session["ListadoDeItemsCorrectos"] as List<itemsGrillaCorrecto>;

            gv_Correcto.DataSource = itemsCo.Where(i => i.Cerrado == false).OrderBy(i => i.Legajo).ToList();
            gv_Correcto.DataBind();

            gv_Cerrados.DataSource = itemsCo.Where(i => i.Cerrado == true).OrderBy(i => i.Legajo).ToList();
            gv_Cerrados.DataBind();

            lbl_Cant_Corr.Text = itemsCo.Count(i => i.Cerrado == false).ToString();
            lbl_cerrados.Text = itemsCo.Count(i => i.Cerrado == true).ToString();
        }

        protected void gv_Inconsistencias_CC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Inconsistencias_CC.PageIndex = e.NewPageIndex;
            RefrescarGrillaInconsistentes_CC();
        }

        protected void gv_Inconsistencias_Int_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Inconsistencias_Int.PageIndex = e.NewPageIndex;
            RefrescarGrillaInconsistentes_Interior();
        }

        protected void gv_Correcto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Correcto.PageIndex = e.NewPageIndex;
            RefrescarGrillaConsistentes();
        }

         protected void gv_Cerrados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Cerrados.PageIndex = e.NewPageIndex;
            RefrescarGrillaConsistentes();
        }

        protected void btn_AnalizarInconsistencia_Click(object sender, ImageClickEventArgs e)
        {
            string id = ((ImageButton)sender).CommandArgument;
            DateTime dia = Convert.ToDateTime(Session["DiaBuscado"]);
            Model1Container cxt = new Model1Container();

            Session["Id"] = id;
            Session["d"] = dia;
            if (!cxt.DiasProcesados.First(d => d.Dia == dia).Cerrado)
            {
                Response.Redirect("~/Aplicativo/Personal_Marcaciones_Procesar_Dia.aspx");
            }
            else
            {
                Controles.MessageBox.Show(this, "No se pueden editar los movimientos del dia cerrado.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_CerrarDia_Click(object sender, EventArgs e)
        {
            if (Session["Bandera"] == null)
            {
                btn_CerrarDia.Enabled = false;
                if (AgendarTardanzasProlongacionesSalidasAntesDeHora())
                {
                    DateTime diabuscado = Convert.ToDateTime(Session["DiaBuscado"]);
                    Model1Container cxt = new Model1Container();
                    DiaProcesado dp = cxt.DiasProcesados.First(d => d.Dia == diabuscado);
                    dp.Cerrado = true;
                    cxt.SaveChanges();
                    Session["Bandera"] = null;
                    div_DiaIncorrecto.Visible = false;
                    Controles.MessageBox.Show(this, "El día se cerró correctamente.", Controles.MessageBox.Tipo_MessageBox.Success);
                }
            }
        }

        /// <summary>
        /// Agenda todo eso de todos los agentes en la fecha a cerrar.
        /// </summary>
        /// <returns></returns>
        private bool AgendarTardanzasProlongacionesSalidasAntesDeHora()
        {
            Session["Bandera"] = true;

            List<itemsGrillaCorrecto> itemsCo = Session["ListadoDeItemsCorrectos"] as List<itemsGrillaCorrecto>;
            DateTime diabuscado = Convert.ToDateTime(Session["DiaBuscado"]);

             var agentesParaAgendarMovimientosHoras = from i in itemsCo
                                                      where i.Tardanza!=null || i.ProlJorn!= null || i.SalidaAntesHora != null
                                                      select new
                                                      {
                                                          Id = i.Id,
                                                          Tardanza = i.Tardanza,
                                                          ProlJor = i.ProlJorn,
                                                          SalidaAntesDeHora = i.SalidaAntesHora
                                                      };

             Model1Container cxt = new Model1Container();

             foreach (var item in agentesParaAgendarMovimientosHoras)
             {
                 Agente ag = cxt.Agentes.First(a => a.Id == item.Id);

                 if (item.ProlJor != null)
                 {
                     TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Prolongación de jornada");
                     ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.ProlJor, tmh, "agendado automáticamente");
                 }

                 if (item.Tardanza != null)
                 {
                     TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Tardanza");
                     ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.Tardanza, tmh, "agendado automáticamente");
                 }

                 if (item.SalidaAntesDeHora != null)
                 {
                     TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Salida antes de hora");
                     ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.SalidaAntesDeHora, tmh, "agendado automáticamente");
                 }
             }

            return true;
        }

        protected void btn_exportarExcel_Click(object sender, EventArgs e)
        {
            DateTime diabuscado = Convert.ToDateTime(Session["DiaBuscado"]);

            gv_Correcto.AllowPaging = false;
            RefrescarGrillaConsistentes();

            if (gv_Correcto.Rows.Count > 0 && gv_Correcto.Visible == true)
            {
                GridViewExportUtil.Export("Lista correctos " + diabuscado.ToString("dd-MM-yyyy") + ".xls", gv_Correcto);
            }

            gv_Correcto.AllowPaging = true;
            RefrescarGrillaConsistentes();

        }

        protected void btn_cerrar_todos_individuales_Click(object sender, EventArgs e)
        {
            List<itemsGrillaCorrecto> itemsCo = Session["ListadoDeItemsCorrectos"] as List<itemsGrillaCorrecto>;
            DateTime diabuscado = Convert.ToDateTime(Session["DiaBuscado"]);

            var agentesParaAgendarMovimientosHoras = from i in itemsCo
                                                     where !i.Cerrado
                                                     select new
                                                     {
                                                         Id = i.Id,
                                                         Tardanza = i.Tardanza,
                                                         ProlJor = i.ProlJorn,
                                                         SalidaAntesDeHora = i.SalidaAntesHora
                                                     };

            Model1Container cxt = new Model1Container();

            foreach (var item in agentesParaAgendarMovimientosHoras)
            {
                Agente ag = cxt.Agentes.First(a => a.Id == item.Id);

                if (!(ag.HorarioFlexible.HasValue && ag.HorarioFlexible==true) || ag.ObtenerEstadoAgenteParaElDia(diabuscado)!= null)
                {

                    ResumenDiario rd = ag.ResumenesDiarios.First(r => r.Dia == diabuscado);

                    if (item.ProlJor != null)
                    {
                        TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Prolongación de jornada");
                        ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.ProlJor, tmh, "agendado automáticamente");
                    }

                    if (item.Tardanza != null)
                    {
                        TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Tardanza");
                        ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.Tardanza, tmh, "agendado automáticamente");
                    }

                    if (item.SalidaAntesDeHora != null)
                    {
                        TipoMovimientoHora tmh = cxt.TiposMovimientosHora.First(mh => mh.Tipo == "Salida antes de hora");
                        ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(diabuscado, ag, ag, item.SalidaAntesDeHora, tmh, "agendado automáticamente");
                    }

                    rd.Cerrado = true;
                }
            }

            cxt.SaveChanges();

            CargarCorrectos();

        }
     
    }
}