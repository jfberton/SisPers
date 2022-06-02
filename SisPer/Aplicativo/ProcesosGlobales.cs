using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public static class ProcesosGlobales
    {
        #region Salidas

        private static bool EsDeMañana(string hora)
        {
            bool ret = false;
            ret = !HorasString.RestarHoras("13:00", DateTime.Now.ToString("HH:mm")).Contains("-");
            return ret;
        }

        public static void ProcesarSalidasSinTerminar(List<Salida> salidas = null)
        {
            DateTime dia = DateTime.Today;
            Agente agente = null;
            Agente agendado_por = null;
            TipoMovimientoHora tmh = null;
            string descripcion = string.Empty;

            foreach (Salida salida in salidas)
            {
                using (var procesar_salidas_cxt = new Model1Container())
                {
                    string horas = "00:00";

                    Salida salidaCxt = procesar_salidas_cxt.Salidas.FirstOrDefault(s => s.Id == salida.Id);
                    if (salidaCxt != null)
                    {
                        if ((EsDeMañana(salidaCxt.HoraDesde) && !EsDeMañana(DateTime.Now.ToString("HH:mm"))) || salida.Dia < DateTime.Today)
                        {//la salida fue de mañana y estamos por la tarde o en otro dia
                            salidaCxt.HoraHasta = "13:00";
                            try
                            {
                                procesar_salidas_cxt.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                descripcion = ex.Message;
                            }

                            //cargo los parametros para agendar el movimiento de hora si es salida particular
                            dia = salidaCxt.Dia;
                            agente = salidaCxt.Agente;
                            agendado_por = salidaCxt.Agente;
                            horas = HorasString.RestarHoras(salidaCxt.HoraHasta, salidaCxt.HoraDesde);
                            tmh = procesar_salidas_cxt.TiposMovimientosHora.First(t => t.Tipo == "Salida particular");
                            descripcion = "Finalizado automáticamente";
                        }

                        if (!EsDeMañana(salidaCxt.HoraDesde) && salida.Dia < DateTime.Today)
                        {
                            //la salida fue de tarde y estamos en otro dia
                            salidaCxt.HoraHasta = "20:00";
                            try
                            {
                                procesar_salidas_cxt.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                descripcion = ex.Message;
                            }

                            //cargo los parametros para agendar el movimiento de hora si es salida particular
                            dia = salidaCxt.Dia;
                            agente = salidaCxt.Agente;
                            agendado_por = salidaCxt.Agente;
                            horas = HorasString.RestarHoras(salidaCxt.HoraHasta, salidaCxt.HoraDesde);
                            tmh = procesar_salidas_cxt.TiposMovimientosHora.First(t => t.Tipo == "Salida particular");
                            descripcion = "Finalizado automáticamente";
                        }

                        //si la salida era particular y chequeo las horas (alguno de los parametros que se cargan dentro del using cxt) esta bien cargado por defecto es "00:00" y despues de usarla la vuelvo a poner en default
                        if (salida.Tipo == TipoSalida.Particular && horas != "00:00")
                        {
                            ProcesosGlobales.AgendarMovimientoHoraEnResumenDiario(dia, agente, agendado_por, horas, tmh, descripcion);
                            dia = DateTime.Today;
                            agente = null;
                            agendado_por = null;
                            horas = "00:00";
                            tmh = null;
                            descripcion = string.Empty;
                        }
                    }
                }
            }
        }

        #endregion

        #region Horarios vespertinos

        /// <summary>
        /// Cancela las solicitudes de HV que sean de dia anterior a hoy
        /// </summary>
        public static void CancelarSolicitudesHVPorVencimiento()
        {
            //Model1Container cxt = new Model1Container();

            //var hvs = cxt.HorariosVespertinos.Where(hv => hv.Estado == EstadosHorarioVespertino.Solicitado).ToList();

            //hvs = hvs.Where(hv => (hv.Dia.DayOfWeek == DayOfWeek.Friday && hv.Dia.AddDays(3) < DateTime.Today) || (hv.Dia.AddDays(1) < DateTime.Today)).ToList();

            //foreach (HorarioVespertino item in hvs)
            //{
            //    item.Estado = EstadosHorarioVespertino.CanceladoAutomatico;
            //}

            //cxt.SaveChanges();
        }

        /// <summary>
        /// Graba la una modificación de estado de un horario vespertino
        /// </summary>
        /// <param name="id">Id del horario vespertino</param>
        /// <param name="estadosHorarioVespertino">Estado del horario vespertino</param>
        /// <param name="ag">Agente que realiza la modificación</param>
        public static void ModificarEstadoHV(int id, EstadosHorarioVespertino estadosHorarioVespertino, Agente agente)
        {
            Model1Container cxt = new Model1Container();
            Agente ag = agente;
            HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hvs => hvs.Id == id);
            hv.Estado = estadosHorarioVespertino;
            hv.AgenteId1 = ag.Id;
            cxt.SaveChanges();
        }

        #endregion

        #region Francos compensatorios

        public static void CancelarSolicitudesFrancosPorVencimiento()
        {
            //Model1Container cxt = new Model1Container();
            //bool guardaCambios = false;
            //var francosSolicitados = cxt.Francos.Where(fs => fs.Estado == EstadosFrancos.Solicitado);

            //var fSol = from f in francosSolicitados
            //           select new
            //           {
            //               Id = f.Id,
            //               PrimerDiaFranco = (from dia in f.DiasFranco select dia.Dia).Min()
            //           };

            //foreach (var item in fSol)
            //{
            //    if (item.PrimerDiaFranco < DateTime.Today)
            //    {
            //        Franco f = cxt.Francos.First(fr => fr.Id == item.Id);
            //        f.Estado = EstadosFrancos.CanceladoAutomatico;
            //        guardaCambios = true;
            //    }
            //}

            //if (guardaCambios)
            //{
            //    cxt.SaveChanges();
            //}
        }

        public static void ModificarEstadoFranco(int id, EstadosFrancos estadosFrancos, Agente ag, string observ = null)
        {
            Model1Container cxt = new Model1Container();
            Franco fra = cxt.Francos.FirstOrDefault(fr => fr.Id == id);
            fra.Estado = estadosFrancos;
            MovimientoFranco mf = new MovimientoFranco();
            mf.AgenteId = ag.Id;
            mf.Fecha = DateTime.Today;
            mf.Estado = estadosFrancos;
            mf.Observacion = observ;
            fra.MovimientosFranco.Add(mf);

            if (estadosFrancos == EstadosFrancos.Aprobado)
            {
                foreach (DiaFranco df in fra.DiasFranco)
                {
                    AgendarMovimientoHoraEnResumenDiario(df.Dia, fra.Agente, ag, "007:00", cxt.TiposMovimientosHora.First(t => t.Tipo == "Franco Compensatorio"), string.Empty);
                    EliminarEstadoPorAprobar(fra);
                    AgendarEstadoDiaAgente(ag, fra.Agente, df.Dia, cxt.TiposEstadoAgente.First(tea => tea.Estado == "Franco compensatorio"));
                }
            }

            if (estadosFrancos == EstadosFrancos.Cancelado || estadosFrancos == EstadosFrancos.CanceladoAutomatico)
            {
                EliminarEstadoPorAprobar(fra);
            }

            cxt.SaveChanges();
        }

        /// <summary>
        /// elimina el estado asociado al franco para el dia que fue solicitado.
        /// </summary>
        /// <param name="fra">Franco</param>
        private static void EliminarEstadoPorAprobar(Franco fra)
        {
            Model1Container cxt = new Model1Container();
            DateTime dia = fra.DiasFranco.First().Dia;
            EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(e => e.AgenteId == fra.AgenteId && e.Dia == dia);

            if (ea != null && ea.TipoEstado.Estado == "Franco compensatorio p/aprobar")
            {
                cxt.EstadosAgente.DeleteObject(ea);
            }

            cxt.SaveChanges();
        }

        /// <summary>
        /// Permite eliminar unicamente si el franco fue generado automaticamente desde la funcion de perfil personal 
        /// agregar movimiento dia
        /// </summary>
        public static void EliminarFrancoYMovimientos(Franco fra)
        {
            if (fra != null)
            {
                Model1Container cxt = new Model1Container();
                Franco fraCxt = cxt.Francos.FirstOrDefault(f => f.Id == fra.Id);



                if (fraCxt != null && fraCxt.MovimientosFranco.Count == 1 && fraCxt.Estado == EstadosFrancos.Aprobado)
                {
                    //obtengo el movimiento del franco y el dia franco
                    MovimientoFranco mf = fraCxt.MovimientosFranco.First();
                    DiaFranco df = fraCxt.DiasFranco.First();

                    //Restituir el estado del dia
                    DateTime dia = df.Dia;
                    EstadoAgente ea = cxt.EstadosAgente.FirstOrDefault(e => e.AgenteId == fra.AgenteId && e.Dia == dia);

                    if (ea != null && ea.TipoEstado.Estado == "Franco compensatorio")
                    {
                        cxt.EstadosAgente.DeleteObject(ea);
                    }

                    //antes de eliminar obtengo los datos que necesito para agendar las horas.
                    Agente agente = cxt.Agentes.First(ag => ag.Id == fra.AgenteId);
                    Agente agPers = cxt.Agentes.First(agp => agp.Id == mf.AgenteId);

                    //Elimino el movimiento, el diafranco y el franco
                    cxt.MovimientosFrancos.DeleteObject(mf);
                    cxt.DiasFrancos.DeleteObject(df);
                    cxt.Francos.DeleteObject(fraCxt);

                    cxt.SaveChanges();

                    //sumar las horas restadas por el franco que se eliminó.
                    AgendarMovimientoHoraEnResumenDiario(dia, agente, agPers, "007:00", cxt.TiposMovimientosHora.First(t => t.Tipo == "Ajustar en mas"), "Se devuelven las horas al anular el Franco agendado para la fecha.");

                }
                else
                {
                    throw new Exception("Esta por eliminar un franco que no puede pero aun asi llego hasta aca. Id del franco = " + fra.Id.ToString());
                }
            }
        }

        /// <summary>
        /// Función que permite al agente de perfil personal agendar un franco compensatorio para un agente en una fecha dada
        /// </summary>
        /// <param name="ag">Agente al cual se le agendará el franco</param>
        /// <param name="personal">Agente de perfil Personal</param>
        /// <param name="dia">Día en donde se agendará el franco</param>
        public static void CrearFrancoAprobado(Agente ag, Agente personal, DateTime dia)
        {
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);

            Franco f = new Franco();
            f.AgenteId = ag.Id;
            f.FechaSolicitud = DateTime.Today;

            DiaFranco d = new DiaFranco();
            d.Dia = dia;
            f.DiasFranco.Add(d);

            cxt.Francos.AddObject(f);

            cxt.SaveChanges();

            ModificarEstadoFranco(f.Id, EstadosFrancos.Aprobado, personal, "Agendado a través de funcionalidad agendar movimientos.");

            ResumenDiario rd = agCxt.ResumenesDiarios.FirstOrDefault(rdiario => rdiario.Dia == dia);

            if (rd != null)
            {
                AnalizarResumenDiario(rd);
            }

        }

        #endregion

        #region Agendar movimientos

        private static bool AcumulaInmediatamente(TipoMovimientoHora tmh)
        {
            bool ret = false;

            //ret = (tmh.Id == 4 || tmh.Id == 5); //si son horarios vespertinos o francos compensatorios los acumulo automaticamente, los demas tipos de movimientos los acumulo al momento de cerrar el dia 

            return ret;
        }

        public static bool AgendarMovimientoHoraEnResumenDiario(DateTime d, Agente ag, Agente agendadoPor, string horas, TipoMovimientoHora tipoMovimiento, string descripcion)
        {
            try
            {
                using (var cxt = new Model1Container())
                {
                    Agente agCxt = cxt.Agentes.First(a => a.Id == ag.Id);
                    ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rd1 => rd1.AgenteId == ag.Id && rd1.Dia == d);

                    //si no tiene registro diario agrego uno nuevo
                    if (rd == null)
                    {
                        rd = new ResumenDiario();
                        ///////////////
                        rd.Dia = d;
                        rd.HEntrada = "000:00";
                        rd.HSalida = "000:00";
                        rd.HVEnt = "000:00";
                        rd.HVSal = "000:00";
                        rd.Horas = "000:00";
                        rd.AgenteId = ag.Id;
                        rd.Inconsistente = true;
                        rd.MarcoTardanza = false;
                        rd.MarcoProlongJornada = false;
                        rd.ObservacionInconsistente = "No posee marcaciones.";
                        cxt.ResumenesDiarios.AddObject(rd);

                        cxt.SaveChanges();

                    }

                    //agrego un movimiento a la lista de movimientos de horas diarias 
                    //con el movimiento de la salida particular
                    MovimientoHora mh = new MovimientoHora();
                    mh.TipoMovimientoHoraId = tipoMovimiento.Id;
                    mh.AgenteId = agendadoPor.Id;
                    mh.Descripcion = descripcion;
                    mh.Horas = horas;

                    /*
                    //Si el agente no tiene horario flexible agenda normalmente haciendo las discriminaciones de bonificacion y demas
                    //si el agente tiene horario flexible agenda el moimiento como viene. 
                    //Los totales y ajustes de horas bonificacion, año anterior, año actual se van a calcular al momento del cierre del día.
                    if (!(ag.HorarioFlexible.HasValue && ag.HorarioFlexible == true))
                    {
                        //El movimiento resta horas
                        if (!tipoMovimiento.Suma)
                        {
                            #region el movimiento RESTA las horas pasadas por parámetro

                            //tiene horas año anterior que cubre la el gasto?
                            if (!agCxt.HorasAcumuladasAnioAnterior.Contains("-") && !HorasString.RestarHoras(ag.HorasAcumuladasAnioAnterior, horas).Contains("-"))
                            {
                                //las resto del año anterior
                                mh.DescontoDeAcumuladoAnioAnterior = true;
                                if (AcumulaInmediatamente(tipoMovimiento))
                                { agCxt.HorasAcumuladasAnioAnterior = HorasString.RestarHoras(agCxt.HorasAcumuladasAnioAnterior, horas); }
                            }
                            else
                            {
                                //Si tiene algo del año anterior se hace un movimiento para ese saldo y 
                                //se modifica las horas del mov para restarle las horas restantes
                                //a este año y se actualizan los acumulados en el agente
                                if (!agCxt.HorasAcumuladasAnioAnterior.Contains("-") && agCxt.HorasAcumuladasAnioAnterior != "000:00")
                                {
                                    MovimientoHora mh1 = new MovimientoHora();
                                    mh1.TipoMovimientoHoraId = tipoMovimiento.Id;
                                    mh1.Horas = agCxt.HorasAcumuladasAnioAnterior;
                                    mh1.DescontoDeAcumuladoAnioAnterior = true;
                                    mh1.AgenteId = agendadoPor.Id;
                                    mh1.Descripcion = descripcion;
                                    rd.MovimientosHoras.Add(mh1);

                                    mh.Horas = HorasString.RestarHoras(horas, mh1.Horas);

                                    if (AcumulaInmediatamente(tipoMovimiento))
                                    { agCxt.HorasAcumuladasAnioAnterior = "000:00"; }
                                }

                                //las resto del año actual las horas o el resto de las horas que no cubrio
                                //lo del año anterior.
                                mh.DescontoDeAcumuladoAnioAnterior = false;
                                //Dependiendo de las horas acumuladas del año en curso
                                if (!agCxt.HorasAcumuladasAnioActual.Contains("-"))
                                {//el normal de los casos
                                    if (AcumulaInmediatamente(tipoMovimiento))
                                    { agCxt.HorasAcumuladasAnioActual = HorasString.RestarHoras(agCxt.HorasAcumuladasAnioActual, mh.Horas); }
                                }
                                else
                                { //Posee horas acumuladas pero en contra. Entonces las sumo y las niego :P
                                    if (AcumulaInmediatamente(tipoMovimiento))
                                    { agCxt.HorasAcumuladasAnioActual = "-" + HorasString.SumarHoras(new string[2] { agCxt.HorasAcumuladasAnioActual.Replace("-", ""), mh.Horas }); }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region el movimiento SUMA las horas padas por parámetro

                            //Primero se verifica si no tiene horaspor cubrir por alguna bonificacion

                            BonificacionOtorgada Bo = agCxt.BonificacionesOtorgadas.FirstOrDefault(bo => bo.Mes == d.Month && bo.Anio == d.Year);

                            if (Bo != null && Bo.HorasAdeudadas != "000:00")
                            {
                                #region tiene horas bonificables por cubrir en el mes que esta por agregar el movimiento

                                if (HorasString.RestarHoras(horas, Bo.HorasAdeudadas.Replace("-", "")).Contains("-"))
                                {   //debe mas horas que las que hizo
                                    //marco todo el movimiento como que entro a horas bonificables por cubrir
                                    mh.DescontoDeHorasBonificables = true;
                                    //resto y vuelvo a poner el signo negativo a las horas por devolver
                                    Bo.HorasAdeudadas = "-" + HorasString.RestarHoras(Bo.HorasAdeudadas.Replace("-", ""), horas);
                                }
                                else
                                {
                                    //hizo mas horas que las que tenia que devolver.
                                    //creo un movimiento nuevo con las horas que cubrio
                                    MovimientoHora mh1 = new MovimientoHora();
                                    mh1.Horas = Bo.HorasAdeudadas.Replace("-", "");
                                    mh1.TipoMovimientoHoraId = tipoMovimiento.Id;
                                    mh1.DescontoDeHorasBonificables = true;
                                    mh1.AgenteId = agendadoPor.Id;
                                    mh1.Descripcion = descripcion;
                                    rd.MovimientosHoras.Add(mh1);
                                    //paso el resto de las horas a sumar las horas a favor del agente
                                    mh.Horas = HorasString.RestarHoras(horas, mh1.Horas);
                                    //pongo a cero las horas a cubrir por bonificación
                                    Bo.HorasAdeudadas = "000:00";

                                    //Actualizo las horas acumuladas del agente
                                    #region Verifico horas año anterior (lo mismo que en el caso de que no tuviera horas bonificables por cubrir)

                                    if (agCxt.HorasAcumuladasAnioAnterior.Contains("-") && HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioAnterior, mh.Horas }).Contains("-"))
                                    {
                                        //las resto del año anterior
                                        mh.DescontoDeAcumuladoAnioAnterior = true;
                                        if (AcumulaInmediatamente(tipoMovimiento))
                                        { agCxt.HorasAcumuladasAnioAnterior = HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioAnterior, mh.Horas }); }
                                    }
                                    else
                                    {
                                        //Si tiene algo del año anterior se hace un movimiento para ese saldo y 
                                        //se modifica las horas del mov para restarle las horas restantes
                                        //a este año y se actualizan los acumulados en el agente
                                        if (agCxt.HorasAcumuladasAnioAnterior.Contains("-") && agCxt.HorasAcumuladasAnioAnterior != "-000:00")
                                        {
                                            MovimientoHora mh2 = new MovimientoHora();
                                            mh2.TipoMovimientoHoraId = tipoMovimiento.Id;
                                            mh2.Horas = agCxt.HorasAcumuladasAnioAnterior.Replace("-", "");
                                            mh2.DescontoDeAcumuladoAnioAnterior = true;
                                            mh2.AgenteId = agendadoPor.Id;
                                            mh2.Descripcion = descripcion;
                                            rd.MovimientosHoras.Add(mh2);

                                            mh.Horas = HorasString.RestarHoras(mh.Horas, mh2.Horas);

                                            if (AcumulaInmediatamente(tipoMovimiento))
                                            { agCxt.HorasAcumuladasAnioAnterior = "000:00"; }
                                        }

                                        //las resto del año actual las horas o el resto de las horas que no cubrio
                                        //lo del año anterior.
                                        mh.DescontoDeAcumuladoAnioAnterior = false;
                                        //Dependiendo de las horas acumuladas del año en curso
                                        if (agCxt.HorasAcumuladasAnioActual.Contains("-"))
                                        {//En caso de tener horas negativas, resto las horas que suman menos las que restan :P jejeje
                                            if (AcumulaInmediatamente(tipoMovimiento))
                                            { agCxt.HorasAcumuladasAnioActual = HorasString.RestarHoras(mh.Horas, agCxt.HorasAcumuladasAnioActual.Replace("-", "")); }
                                        }
                                        else
                                        { //el normal de los casos (no tiene horas en contra en el año) sumo normalmente.
                                            if (AcumulaInmediatamente(tipoMovimiento))
                                            { agCxt.HorasAcumuladasAnioActual = HorasString.SumarHoras(new string[2] { agCxt.HorasAcumuladasAnioActual, mh.Horas }); }
                                        }
                                    }

                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                #region No tiene horas por cubrir de bonificacion asi que a verificar si tiene horas por cubrir del año anterior
                                //las horas por cubrir del año anterior superan las que se estan por cargar
                                if (agCxt.HorasAcumuladasAnioAnterior.Contains("-") && HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioAnterior, horas }).Contains("-"))
                                {
                                    //las resto del año anterior
                                    mh.DescontoDeAcumuladoAnioAnterior = true;
                                    if (AcumulaInmediatamente(tipoMovimiento))
                                    { agCxt.HorasAcumuladasAnioAnterior = HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioAnterior, horas }); }
                                }
                                else
                                {
                                    //Si tiene algo del año anterior se hace un movimiento para ese saldo y 
                                    //se modifica las horas del mov para restarle las horas restantes
                                    //a este año y se actualizan los acumulados en el agente
                                    if (agCxt.HorasAcumuladasAnioAnterior.Contains("-") && agCxt.HorasAcumuladasAnioAnterior != "-000:00")
                                    {
                                        MovimientoHora mh1 = new MovimientoHora();
                                        mh1.TipoMovimientoHoraId = tipoMovimiento.Id;
                                        mh1.Horas = agCxt.HorasAcumuladasAnioAnterior.Replace("-", "");
                                        mh1.DescontoDeAcumuladoAnioAnterior = true;
                                        mh1.AgenteId = agendadoPor.Id;
                                        mh1.Descripcion = descripcion;
                                        rd.MovimientosHoras.Add(mh1);

                                        mh.Horas = HorasString.RestarHoras(horas, mh1.Horas);

                                        if (AcumulaInmediatamente(tipoMovimiento))
                                        { agCxt.HorasAcumuladasAnioAnterior = "000:00"; }
                                    }

                                    //las resto del año actual las horas o el resto de las horas que no cubrio
                                    //lo del año anterior.
                                    mh.DescontoDeAcumuladoAnioAnterior = false;
                                    //Dependiendo de las horas acumuladas del año en curso
                                    if (agCxt.HorasAcumuladasAnioActual.Contains("-"))
                                    {//En caso de tener horas negativas, resto las horas que suman menos las que restan :P jejeje
                                        if (AcumulaInmediatamente(tipoMovimiento))
                                        { agCxt.HorasAcumuladasAnioActual = HorasString.RestarHoras(mh.Horas, agCxt.HorasAcumuladasAnioActual.Replace("-", "")); }
                                    }
                                    else
                                    { //el normal de los casos (no tiene horas en contra en el año) sumo normalmente.
                                        if (AcumulaInmediatamente(tipoMovimiento))
                                        { agCxt.HorasAcumuladasAnioActual = HorasString.SumarHoras(new string[2] { agCxt.HorasAcumuladasAnioActual, mh.Horas }); }
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }
                    }
                    */

                    rd.MovimientosHoras.Add(mh);
                    rd.Horas = HorasString.TotalizarMovimientosHorasDiarias(rd.MovimientosHoras);

                    cxt.SaveChanges();
                    return true;
                }
            }
            catch 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Distribuye las horas acumuladas al cierre del día para los agentes con horario flexible
        /// </summary>
        /// <param name="rd">Resumen diario cerrado.</param>
        /// <returns></returns>
        public static bool DistribuirHorasCierreDiaHorarioFlexible(ResumenDiario rd)
        {
            bool ret = true;

            try
            {
                using (var cxt = new Model1Container())
                {
                    ResumenDiario rdCxt = cxt.ResumenesDiarios.First(rrdd => rrdd.Id == rd.Id);

                    rdCxt.AcumuloHorasAnioActual = "00:00";
                    rdCxt.AcumuloHorasAnioAnterior = "00:00";
                    rdCxt.AcumuloHorasBonificacion = "00:00";
                    rdCxt.AcumuloHorasMes = "00:00";

                    HorasMesHorarioFlexible hm = rdCxt.Agente.HorasMesHorarioFlexibles.FirstOrDefault(bo => bo.Mes == rdCxt.Dia.Month && bo.Anio == rdCxt.Dia.Year);
                    if (hm == null)
                    {
                        hm = new HorasMesHorarioFlexible();
                        hm.AgenteId = rdCxt.AgenteId;
                        hm.HorasAcumuladas = "00:00";
                        hm.Anio = rdCxt.Dia.Year;
                        hm.Mes = rdCxt.Dia.Month;
                        cxt.HorasMesHorarioFlexibles.AddObject(hm);
                        cxt.SaveChanges();
                    }

                    string horas = rdCxt.Horas;

                    if (horas.Contains("-"))
                    {
                        //resta

                        List<MovimientoHora> movimientosquedescuentanfuaradelmes = rdCxt.MovimientosHoras.Where(mh =>
                                                                                                    mh.Tipo.Tipo == "Tardanza" ||
                                                                                                    mh.Tipo.Tipo == "Salida Particular" ||
                                                                                                    mh.Tipo.Tipo == "Franco Compensatorio").ToList();

                        foreach (MovimientoHora mh in movimientosquedescuentanfuaradelmes)
                        {
                            string horasMovimiento = mh.Horas;

                            //en realidad se realiza una resta porque las horas del dia son negativas y las hoars del movimiento son positivas (aunque en realidad reste por el tipo de movimiento
                            horas = HorasString.SumarHoras(new string[] { horas, horasMovimiento });

                            #region Controlo si puedo descontar todo o parte de las horas del movimiento con horas a favor del año anterior
                            //tiene horas año anerior que cubre el gasto?
                            if (!rdCxt.Agente.HorasAcumuladasAnioAnterior.Contains("-") && !HorasString.RestarHoras(rdCxt.Agente.HorasAcumuladasAnioAnterior, horasMovimiento).Contains("-"))
                            {
                                //las resto del año anterior
                                rdCxt.Agente.HorasAcumuladasAnioAnterior = HorasString.RestarHoras(rdCxt.Agente.HorasAcumuladasAnioAnterior, horasMovimiento);
                                //las decuento de las horas totales adeudadas del dia y las guardo en el campo AcumuloAnioAnterior
                                rdCxt.AcumuloHorasAnioAnterior = ("-" + horasMovimiento.Replace("-", "")).Replace("-000:00", "00:00").Replace("-00:00", "00:00");
                                horasMovimiento = "00:00";
                            }
                            else
                            {
                                //Si tiene algo del año anterior se hace un movimiento para ese saldo y 
                                //se modifica las horas del mov para restarle las horas restantes
                                //a este año y se actualizan los acumulados en el agente
                                if (!rdCxt.Agente.HorasAcumuladasAnioAnterior.Contains("-") && rdCxt.Agente.HorasAcumuladasAnioAnterior != "000:00")
                                {
                                    //Asigno las horas del año anterior al campo horas año anterior del resumen diario
                                    rdCxt.AcumuloHorasAnioAnterior = ("-" + rdCxt.Agente.HorasAcumuladasAnioAnterior.Replace("-", "")).Replace("-000:00", "00:00").Replace("-00:00", "00:00");
                                    //calculo el nuevo monto de horas a descontar por el movimiento
                                    horasMovimiento = HorasString.RestarHoras(horasMovimiento, rdCxt.Agente.HorasAcumuladasAnioAnterior.Replace("-", ""));
                                    //pongo a cero las horas a favor del anio anterior
                                    rdCxt.Agente.HorasAcumuladasAnioAnterior = "000:00";
                                }
                            }

                            #endregion

                            #region Controlo si puedo descontar todo o parte de las horas del movimiento con horas a favor del año actual

                            //tiene horas año actual que cubre la el gasto?
                            if (!rdCxt.Agente.HorasAcumuladasAnioActual.Contains("-") && !HorasString.RestarHoras(rdCxt.Agente.HorasAcumuladasAnioActual, horasMovimiento).Contains("-"))
                            {
                                //las resto del año actual
                                rdCxt.Agente.HorasAcumuladasAnioActual = HorasString.RestarHoras(rdCxt.Agente.HorasAcumuladasAnioActual, horasMovimiento);
                                //las decuento de las horas totales adeudadas del dia y las guardo en el campo AcumuloAnioActual
                                rdCxt.AcumuloHorasAnioActual = ("-" + horasMovimiento.Replace("-", "")).Replace("-000:00", "00:00").Replace("-00:00", "00:00");
                                horasMovimiento = "00:00";
                            }
                            else
                            {
                                //Si tiene algo del año actual se hace un movimiento para ese saldo y 
                                //se modifica las horas del mov para restarle las horas restantes
                                //a este año y se actualizan los acumulados en el agente
                                if (!rdCxt.Agente.HorasAcumuladasAnioActual.Contains("-") && rdCxt.Agente.HorasAcumuladasAnioActual != "000:00")
                                {
                                    //Asigno las horas del año anterior al campo horas año anterior del resumen diario
                                    rdCxt.AcumuloHorasAnioActual = ("-" + rdCxt.Agente.HorasAcumuladasAnioActual.Replace("-", "")).Replace("-000:00", "00:00").Replace("-00:00", "00:00");
                                    //calculo el nuevo monto de horas a descontar por el movimiento
                                    horasMovimiento = HorasString.RestarHoras(horasMovimiento, rdCxt.Agente.HorasAcumuladasAnioActual.Replace("-", ""));
                                    //pongo a cero las horas a favor del anio anterior
                                    rdCxt.Agente.HorasAcumuladasAnioActual = "000:00";
                                }
                            }

                            #endregion
                        }

                        //acumulo al total mes del dia, las horas correspondientes al total del dia menos las horas pertenecientes a los movimientos que descuentan de otras partes
                        rdCxt.AcumuloHorasMes = (HorasString.SumarHoras(new string[] { rdCxt.AcumuloHorasMes, horas }));

                        //Actualizo las horas acumuladas del mes
                        if (hm != null)
                        {
                            hm.HorasAcumuladas = HorasString.SumarHoras(new string[] { hm.HorasAcumuladas, rdCxt.AcumuloHorasMes });
                        }
                        else
                        {
                            hm = new HorasMesHorarioFlexible();
                            hm.AgenteId = rdCxt.AgenteId;
                            hm.HorasAcumuladas = rdCxt.AcumuloHorasMes;
                            hm.Anio = rdCxt.Dia.Year;
                            hm.Mes = rdCxt.Dia.Month;
                            cxt.HorasMesHorarioFlexibles.AddObject(hm);
                        }
                    }
                    else
                    { //suma
                        //primero obtengo la bonificacion si existe
                        BonificacionOtorgada Bo = rdCxt.Agente.BonificacionesOtorgadas.FirstOrDefault(bo => bo.Mes == rdCxt.Dia.Month && bo.Anio == rdCxt.Dia.Year);

                        if (Bo != null && Bo.HorasAdeudadas != "000:00")
                        {
                            #region tiene horas bonificables por cubrir en el mes que esta cerrando el dia

                            if (HorasString.RestarHoras(horas, Bo.HorasAdeudadas.Replace("-", "")).Contains("-"))
                            {   //debe mas horas que las que hizo
                                //marco todo el saldo diario como que entro a horas bonificables por cubrir
                                rdCxt.AcumuloHorasBonificacion = horas;
                                //resto y vuelvo a poner el signo negativo a las horas por devolver
                                Bo.HorasAdeudadas = "-" + HorasString.RestarHoras(Bo.HorasAdeudadas.Replace("-", ""), horas);
                            }
                            else
                            {
                                //hizo mas horas que las que tenia que devolver de bonificacion.
                                //guardo en acumulado horas bonificacion las horas que cumpli y el resto acumulo en el mes
                                rdCxt.AcumuloHorasBonificacion = Bo.HorasAdeudadas.Replace("-", "");
                                rdCxt.AcumuloHorasMes = HorasString.RestarHoras(horas, rdCxt.AcumuloHorasBonificacion);

                                //pongo a cero las horas a cubrir por bonificación
                                Bo.HorasAdeudadas = "000:00";

                                //Actualizo las hoas acumuladas del mes
                                if (hm != null)
                                {
                                    hm.HorasAcumuladas = HorasString.SumarHoras(new string[] { hm.HorasAcumuladas, rdCxt.AcumuloHorasMes });
                                }
                                else
                                {
                                    hm = new HorasMesHorarioFlexible();
                                    hm.HorasAcumuladas = rdCxt.AcumuloHorasMes;
                                    hm.AgenteId = rdCxt.AgenteId;
                                    hm.Anio = rdCxt.Dia.Year;
                                    hm.Mes = rdCxt.Dia.Month;
                                    cxt.HorasMesHorarioFlexibles.AddObject(hm);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region No tiene horas bonificación por cumplir, va todo al total mensual

                            rdCxt.AcumuloHorasMes = horas;


                            //Actualizo las horas acumuladas del mes
                            if (hm != null)
                            {
                                hm.HorasAcumuladas = HorasString.SumarHoras(new string[] { hm.HorasAcumuladas, rdCxt.AcumuloHorasMes });
                            }
                            else
                            {
                                hm = new HorasMesHorarioFlexible();
                                hm.HorasAcumuladas = rdCxt.AcumuloHorasMes;
                                hm.AgenteId = rdCxt.AgenteId;
                                hm.Anio = rdCxt.Dia.Year;
                                hm.Mes = rdCxt.Dia.Month;
                                cxt.HorasMesHorarioFlexibles.AddObject(hm);
                            }

                            #endregion
                        }
                    }

                    cxt.SaveChanges();
                }
            }
            catch
            {
                ret = false;
            }


            return ret;
        }

        /// <summary>
        /// Agenda un estado al agente en un día
        /// </summary>
        /// <param name="agendadoPor">El agente que esta agendando</param>
        /// <param name="ag">EL agente que va a ser agendado</param>
        /// <param name="d">Fecha en que se guardará el movimiento</param>
        /// <param name="ea">Estado a agendar</param>
        /// <param name="year">En caso de ser licencia deberá indicar el año al que esta haciendo referencia para poder computar corractamente los días usufructuados</param>
        /// <returns></returns>
        public static bool AgendarEstadoDiaAgente(Agente agendadoPor, Agente ag, DateTime d, TipoEstadoAgente ea, int year = 0)
        {
            try
            {

                Model1Container cxt = new Model1Container();

                SolicitudDeEstado solest = (
                                            from se in cxt.SolicitudesDeEstado
                                            where
                                                se.FechaDesde >= d &&
                                                se.FechaHasta <= d &&
                                                //se.TipoEstadoAgenteId == ea.Id &&
                                                se.AgenteId == ag.Id
                                            select
                                                se).FirstOrDefault();
                if (solest != null)
                {
                    solest.Estado = EstadoSolicitudDeEstado.Aprobado;
                }

                if (ea.Estado == "Licencia Anual" || ea.Estado == "Licencia Anual (Saldo)" || ea.Estado == "Licencia Anual (Anticipo)")
                {
                    var tipoLicencia = cxt.TiposDeLicencia.First(tl => tl.Tipo == "Licencia anual");


                    //Si tiene dias usufructuados por licencia (del mismo tipo que quiero cargar) primero las elimino
                    List<DiaUsufructado> dias_usufructuados_por_borrar = cxt.DiasUsufructuados.Where(uu => uu.Dia == d && uu.AgenteId == ag.Id && uu.TipoLicenciaId == tipoLicencia.Id).ToList();

                    foreach (DiaUsufructado du in dias_usufructuados_por_borrar)
                    {
                        cxt.DiasUsufructuados.DeleteObject(du);
                    }

                    //Agrego el nuevo dia usufructuado
                    cxt.DiasUsufructuados.AddObject(new DiaUsufructado
                    {
                        AgenteId = ag.Id,
                        AgentePersonalId = agendadoPor.Id,
                        Anio = year,
                        Dia = d,
                        TipoLicenciaId = tipoLicencia.Id
                    });
                }

                EstadoAgente eag = new EstadoAgente();
                eag.TipoEstado = cxt.TiposEstadoAgente.First(te => te.Id == ea.Id);
                eag.AgendadoPor = cxt.Agentes.First(a => a.Id == agendadoPor.Id);
                eag.Agente = cxt.Agentes.First(a => a.Id == ag.Id);
                eag.Dia = d;

                cxt.EstadosAgente.AddObject(eag);
                cxt.SaveChanges();

                #region Verificar y aprobar solicitud si existe
                //obtengo la solicitud
                SolicitudDeEstado sol_est = cxt.SolicitudesDeEstado.FirstOrDefault(ssee => ssee.AgenteId == ag.Id && ssee.FechaDesde <= d && ssee.FechaHasta >= d);

                //si existe la solicitud recorro los dias que pertenecen a la misma y si todos tienen agendado algun estado la apruebo y listo
                if (sol_est != null)
                {
                    bool aprobar_solicitud = true;

                    for (DateTime i = sol_est.FechaDesde; i <= sol_est.FechaHasta; i = i.AddDays(1))
                    {
                        if (cxt.EstadosAgente.FirstOrDefault(eeaa => eeaa.AgenteId == ag.Id && eeaa.Dia == i) == null)
                        {
                            //si en alguno de los dias que forman parte de la solicitud no tiene estado agendado no debo aprobarla
                            aprobar_solicitud = false;
                            break;
                        }
                    }

                    if (aprobar_solicitud)
                    {
                        sol_est.Estado = EstadoSolicitudDeEstado.Aprobado;
                        cxt.SaveChanges();
           
                    }

                }

                #endregion

                ResumenDiario rd = ag.ResumenesDiarios.FirstOrDefault(rdiario => rdiario.Dia == d);

                if (rd != null)
                {
                    AnalizarResumenDiario(rd);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool EliminarAgendaEstadoDiaAgente(Agente ag, DateTime d, TipoEstadoAgente ea)
        {
            try
            {

                Model1Container cxt = new Model1Container();

                EstadoAgente eag1 = cxt.EstadosAgente.FirstOrDefault(_ea => _ea.Dia == d && _ea.Agente.Id == ag.Id && _ea.TipoEstadoAgenteId == ea.Id);

                if (eag1 != null)
                {
                    cxt.EstadosAgente.DeleteObject(eag1);
                }

                cxt.SaveChanges();


                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Este metodo verifica toma com parámetro primer dia del mes siguiente al que se desea cerrar
        /// cierre y actualia las horas del mes cerrado
        /// Si corresponde actualiza las horas acumuladas del año anterior
        /// y del año actual
        /// Si arranca un año nuevo: Pone las horas del año actual en las horas del año anterior y 
        ///                         las del año actual en cero.
        /// Si pasamos el mes 6: pone en cero las horas del año anterior.
        /// </summary>
        private static void CierreDeMes(DateTime primerDiaMesSiguienteDelQueCierra)
        {
            Model1Container cxt = new Model1Container();
            var variablesglobales = cxt.VariablesGlobales.First();

            if (variablesglobales.UltimaFechaConsulta.Year < primerDiaMesSiguienteDelQueCierra.Year)
            {
                //Arranca año nuevo
                //Paso las horas acumuladas al campo de horas del año anterior en cada uno de los agentes
                var agentes = cxt.Agentes.Where(a => a.FechaBaja == null);
                foreach (Agente item in agentes)
                {
                    if (item.HorasAcumuladasAnioActual != "000:00")
                    {
                        item.HorasAcumuladasAnioAnterior = item.HorasAcumuladasAnioActual;
                        item.HorasAcumuladasAnioActual = "000:00";
                    }
                }

                //Actualizo la fecha de consulta.
                cxt.VariablesGlobales.First().UltimaFechaConsulta = primerDiaMesSiguienteDelQueCierra;
                cxt.SaveChanges();
            }
            else
            {
                if (variablesglobales.UltimaFechaConsulta.Year > primerDiaMesSiguienteDelQueCierra.Year)
                {
                    //error esto nunca puede pasar, si el un caso extremo llego a pasar lo corrijo
                    cxt.VariablesGlobales.First().UltimaFechaConsulta = DateTime.Today;
                    cxt.SaveChanges();
                }
                else
                {
                    //esta en el mismo año
                    if (variablesglobales.UltimaFechaConsulta.Month < primerDiaMesSiguienteDelQueCierra.Month)
                    {
                        //Mes nuevo && variablesglobales.UltimoAnioHorasEliminadas 
                        //Verifico que el mes que paso haya sido el 6, 
                        //si es asi tengo que eliminar las horas acumuladas del año anterior

                        var agentes = cxt.Agentes.Where(a => a.FechaBaja == null);
                        foreach (Agente item in agentes)
                        {
                            if (variablesglobales.UltimaFechaConsulta.Month == 6 &&
                                variablesglobales.UltimoAnioHorasEliminadas != null &&
                                variablesglobales.UltimoAnioHorasEliminadas.Value != DateTime.Today.Year - 1)
                            {
                                //esta en el mismo año el cierre (meses del mismo año) y el ultimo mes cerrado es el 6, al cerrar el mes 7 
                                //deberia eliminar las horas del año anterior si no fueron eliminadas, por eso controlo que en variables globales
                                //ya no este dentro de ultimoañohoraseliminadas el año anterior o sea si estoy en 2014 que no se hayan eliminado todavia las del 2013
                                ///////////////
                                //las horas que perdio puedo obtenerlas en el campo 
                                //horas año anterior del cierre del mes 6
                                HAAnteriorEliminadas horasperdidas = new HAAnteriorEliminadas()
                                {
                                    AgenteId = item.Id,
                                    Anio = DateTime.Today.Year - 1,
                                    Horas = item.HorasAcumuladasAnioAnterior
                                };

                                cxt.HAAnteriorEliminadasSet.AddObject(horasperdidas);
                                item.HorasAcumuladasAnioAnterior = "000:00";
                            }
                        }

                        cxt.VariablesGlobales.First().UltimaFechaConsulta = primerDiaMesSiguienteDelQueCierra;
                        cxt.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Por cada agente elimina las horas acumuladas del año anterior a la fecha en que se procesa
        /// y las resguarda en la tabla HAAEliminadasAnioAnterior
        /// </summary>
        /// <remarks>
        /// Ejemplo: corro este proceso en el año 2014, recorre los agentes que no esten dados de baja y por cada uno
        /// resguardo el campo HorasAcumuladasAñoAnterior en la tabla HAAEliminadasAnioAnterior
        /// y por ultimo modifica el campo UltimoAnioHorasEliminadas de la tabla variables globales para futuras consultas.
        /// </remarks>
        public static void EliminarHorasAñoAnterior()
        {
            using (var cxt = new Model1Container())
            {
                //verifico si ya no se ejecuto la consulta dentro de este año, de ser asi no hago nada.
                if (cxt.VariablesGlobales.First().UltimoAnioHorasEliminadas == null ||
                    cxt.VariablesGlobales.First().UltimoAnioHorasEliminadas.Value != DateTime.Today.Year - 1)
                {
                    var agentes = cxt.Agentes.Where(a => a.FechaBaja == null);
                    foreach (Agente item in agentes)
                    {
                        HAAnteriorEliminadas horasperdidas = new HAAnteriorEliminadas()
                        {
                            AgenteId = item.Id,
                            Anio = DateTime.Today.Year - 1,
                            Horas = item.HorasAcumuladasAnioAnterior
                        };
                        cxt.HAAnteriorEliminadasSet.AddObject(horasperdidas);
                        item.HorasAcumuladasAnioAnterior = "000:00";
                    }
                    //modifico el ultimo año eliminado 
                    cxt.VariablesGlobales.First().UltimoAnioHorasEliminadas = DateTime.Today.Year - 1;
                    cxt.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Verifica si es un mes nuevo y vuelve a agregar bonificaciones a todos los agentes que poseían bonificaciones el mes anterior
        /// </summary>
        public static void RegenerarBonificaciones()
        {
            using (var cxt = new Model1Container())
            {
                int year = cxt.BonificacionesOtorgadas.Max(y => y.Anio);
                int month = cxt.BonificacionesOtorgadas.Where(b => b.Anio == year).Max(m => m.Mes);
                int days = DateTime.DaysInMonth(year, month);

                DateTime ultimoDiaDelMes = new DateTime(year, month, days);

                if (DateTime.Today > ultimoDiaDelMes)
                {
                    //estoy en un mes nuevo del ultimo mes que se generaron las bonificaciones
                    var agentesConBonificaciones = cxt.Agentes.Where(a => a.PoseeBonificacion == true);

                    foreach (var ag in agentesConBonificaciones)
                    {
                        string horas = ag.BonificacionesOtorgadas.Last().HorasOtorgadas;

                        ag.HorasBonificacionACubrir = "-" + horas;
                        ag.BonificacionesOtorgadas.Add(new BonificacionOtorgada() { Anio = DateTime.Today.Year, Mes = DateTime.Today.Month, HorasOtorgadas = horas, HorasAdeudadas = horas });

                    }
                    cxt.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Verifica si se generó notificación de presentación de informes el mes anterior y si no es asi las genera
        /// </summary>
        public static void VerificarYAgregarNotificacionJefe(int idAgente)
        {
            using (var cxt = new Model1Container())
            {
                Notificacion not = cxt.Notificaciones.Where(nn => nn.AgenteId == idAgente).ToList().FirstOrDefault(nn1 => nn1.Descripcion.Replace("Presentar al departamento Personal los informes de horarios vespertinos y salidas diarias del mes de ", "") == DateTime.Today.AddMonths(-1).ToString("MMMM 'de' yyyy"));
                if (not == null)
                {

                    Notificacion_Tipo nt = cxt.Notificacion_Tipos.FirstOrDefault(nntt => nntt.Tipo == "Automática");
                    if (nt == null)
                    {
                        nt = new Notificacion_Tipo() { Tipo = "Automática" };
                        cxt.Notificacion_Tipos.AddObject(nt);
                    }

                    Notificacion_Estado ne = cxt.Notificacion_Estados.FirstOrDefault(nnee => nnee.Estado == "Generada");
                    if (ne == null)
                    {
                        ne = new Notificacion_Estado() { Estado = "Generada" };
                        cxt.Notificacion_Estados.AddObject(ne);
                    }


                    Agente ag = cxt.Agentes.First(a => a.Id == idAgente);
                    Notificacion notificacion = new Notificacion();

                    notificacion.Descripcion = "Presentar al departamento Personal los informes de horarios vespertinos y salidas diarias del mes de " + DateTime.Today.AddMonths(-1).ToString("MMMM 'de' yyyy");
                    notificacion.Destinatario = ag;
                    notificacion.ObservacionPendienteRecibir = string.Empty;

                    notificacion.Tipo = nt;
                    cxt.Notificaciones.AddObject(notificacion);

                    Notificacion_Historial notHist = new Notificacion_Historial()
                    {
                        Agente = ag,
                        Estado = ne,
                        Fecha = DateTime.Now,
                        Notificacion = notificacion
                    };

                    cxt.Notificacion_Historiales.AddObject(notHist);

                    cxt.SaveChanges();
                }
            }
        }

        #endregion

        #region Huellas

        /// <summary>
        /// Devuelve las marcaciones digitales y manuales del legajo solicitado en una fecha dada.
        /// </summary>
        /// <param name="fecha">Fecha buscada</param>
        /// <param name="legajo">Legajo buscado</param>
        /// <returns></returns>
        public static DS_Marcaciones ObtenerMarcaciones(DateTime fecha, string legajo)
        {
            DS_Marcaciones ret = new DS_Marcaciones();
            int pruebaLegajo = 0;
            if (int.TryParse(legajo.ToString(), out pruebaLegajo))
            {
                //obtengo las marcaciones digitales de todo el personal en una fecha dada
                #region Desmarcar
                using (ClockCardEntities cxt = new ClockCardEntities())
                {
                    var marcacionesDigitales = (from h in cxt.FICHADA
                                                where
                                                h.FIC_FECHA == fecha && h.LEG_LEGAJO == pruebaLegajo
                                                select new
                                                {
                                                    Legajo = h.LEG_LEGAJO,
                                                    Fecha = h.FIC_FECHA,
                                                    Hora = h.FIC_HORA
                                                }).ToList();

                    foreach (var item in marcacionesDigitales)
                    {
                        DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                        dr.Legajo = item.Legajo.ToString();
                        dr.Fecha = item.Fecha;
                        dr.Hora = item.Hora;
                        dr.MarcaManual = false;
                        ret.Marcacion.Rows.Add(dr);
                    }
                }
                #endregion

                using (Model1Container cxt = new Model1Container())
                {
                    //Marcaciones .
                    var resumenDiarioSinDigitalNiAnuladas = (from mm in cxt.Marcaciones
                                                             where
                                                             !mm.Anulada &&
                                                             mm.ResumenDiario.Dia == fecha &&
                                                             mm.ResumenDiario.Agente.Legajo == pruebaLegajo
                                                             select new
                                                             {
                                                                 Legajo = mm.ResumenDiario.Agente.Legajo,
                                                                 Fecha = mm.ResumenDiario.Dia,
                                                                 Hora = mm.Hora,
                                                                 Manual = mm.Manual
                                                             }).ToList();

                    foreach (var item in resumenDiarioSinDigitalNiAnuladas)
                    {
                        var result = ret.Marcacion.Select("Legajo = '" + legajo + "' AND Hora = '" + item.Hora + "'");
                        //Evito cargar una hora que ya esté en el resumen
                        if (result.Count() == 0)
                        {
                            DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                            dr.Legajo = item.Legajo.ToString();
                            dr.Fecha = item.Fecha;
                            dr.Hora = item.Hora;
                            dr.MarcaManual = item.Manual;
                            ret.Marcacion.Rows.Add(dr);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Devuelve las marcaciones digitales y manuales del legajo solicitado en una fecha dada. 
        /// Las marcaciones marcadas como provisorias o no definitivas son las que carga el agente o jefe
        /// y estan pendientes de aprobarce en personal
        /// </summary>
        /// <param name="fecha">Fecha buscada</param>
        /// <param name="legajo">Legajo buscado</param>
        /// <returns></returns>
        public static DS_Marcaciones ObtenerMarcacionesConProvisorias(DateTime fecha, string legajo)
        {
            DS_Marcaciones ret = new DS_Marcaciones();
            int pruebaLegajo = 0;
            if (int.TryParse(legajo, out pruebaLegajo))
            {
                //obtengo las marcaciones digitales de todo el personal en una fecha dada
                #region Desmarcar
                using (ClockCardEntities cxt = new ClockCardEntities())
                {
                    var marcacionesDigitales = (from h in cxt.FICHADA
                                                where
                                                h.FIC_FECHA == fecha && h.LEG_LEGAJO == pruebaLegajo
                                                select new
                                                {
                                                    Legajo = h.LEG_LEGAJO,
                                                    Fecha = h.FIC_FECHA,
                                                    Hora = h.FIC_HORA
                                                }).ToList();

                    foreach (var item in marcacionesDigitales)
                    {
                        DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                        dr.Legajo = item.Legajo.ToString();
                        dr.Fecha = item.Fecha;
                        dr.Hora = item.Hora;
                        dr.MarcaManual = false;
                        ret.Marcacion.Rows.Add(dr);
                    }
                }
                #endregion

                using (Model1Container cxt = new Model1Container())
                {
                    //Marcaciones .
                    var resumenDiarioSinDigitalNiAnuladas = (from mm in cxt.Marcaciones
                                                             where
                                                             !mm.Anulada &&
                                                             mm.ResumenDiario.Dia == fecha &&
                                                             mm.ResumenDiario.Agente.Legajo == pruebaLegajo
                                                             select new
                                                             {
                                                                 Legajo = mm.ResumenDiario.Agente.Legajo,
                                                                 Fecha = mm.ResumenDiario.Dia,
                                                                 Hora = mm.Hora,
                                                                 Manual = mm.Manual
                                                             }).ToList();

                    foreach (var item in resumenDiarioSinDigitalNiAnuladas)
                    {
                        var result = ret.Marcacion.Select("Legajo = '" + legajo + "' AND Hora = '" + item.Hora + "'");
                        //Evito cargar una hora que ya esté en el resumen
                        if (result.Count() == 0)
                        {
                            DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                            dr.Legajo = item.Legajo.ToString();
                            dr.Fecha = item.Fecha;
                            dr.Hora = item.Hora;
                            dr.MarcaManual = item.Manual;
                            ret.Marcacion.Rows.Add(dr);
                        }
                    }

                    var entradasSalidasProvisorias = from es in cxt.EntradasSalidas
                                                     where
                                                        es.Agente.Legajo == pruebaLegajo &&
                                                        es.Fecha == fecha &&
                                                        !es.CerradoPersonal
                                                     select new
                                                     {
                                                         Legajo = es.Agente.Legajo,
                                                         Fecha = es.Fecha,
                                                         HoraEntrada = es.Entrada,
                                                         HoraSalida = es.Salida,
                                                         Manual = true,
                                                         Definitivo = false
                                                     };

                    foreach (var item in entradasSalidasProvisorias)
                    {
                        var result = ret.Marcacion.Select("Legajo = '" + legajo + "' AND (Hora = '" + item.HoraEntrada + "' OR Hora = '" + item.HoraSalida + "')");
                        //Evito cargar una hora que ya esté en el resumen
                        if (result.Count() == 0)
                        {
                            DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                            dr.Legajo = item.Legajo.ToString();
                            dr.Fecha = item.Fecha;
                            dr.Hora = item.HoraEntrada;
                            dr.MarcaManual = item.Manual;
                            dr.EsDefinitivo = item.Definitivo;
                            ret.Marcacion.Rows.Add(dr);

                            if (item.HoraSalida != "")
                            {
                                DS_Marcaciones.MarcacionRow dr1 = ret.Marcacion.NewMarcacionRow();
                                dr1.Legajo = item.Legajo.ToString();
                                dr1.Fecha = item.Fecha;
                                dr1.Hora = item.HoraSalida;
                                dr1.MarcaManual = item.Manual;
                                dr1.EsDefinitivo = item.Definitivo;

                                ret.Marcacion.Rows.Add(dr1);
                            }

                        }
                    }


                }
            }

            return QuitarMarcacionesAnuladas(pruebaLegajo, fecha, ret);
        }

        private static DS_Marcaciones QuitarMarcacionesAnuladas(int legajo, DateTime fecha, DS_Marcaciones ret)
        {
            using (var cxt = new Model1Container())
            {
                //independientemente si fue o no anulada, si esta en el resumen diario no debe volver a mostrarse como marcacion individual
                var HorasAnuladasResumenDiario = (from mm in cxt.Marcaciones
                                                  where
                                                  mm.Anulada &&
                                                  mm.ResumenDiario.Dia == fecha &&
                                                  mm.ResumenDiario.Agente.Legajo == legajo
                                                  select new
                                                  {
                                                      Hora = mm.Hora
                                                  }).ToList();

                foreach (var item in HorasAnuladasResumenDiario)
                {
                    DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();

                    foreach (DS_Marcaciones.MarcacionRow file in ret.Marcacion)
                    {
                        if (file.Hora == item.Hora)
                        {
                            dr = file;
                        }
                    }

                    int i = ret.Marcacion.Rows.IndexOf(dr);

                    if (i >= 0)
                    {
                        ret.Marcacion.Rows.RemoveAt(i);
                    }
                }

            }

            return ret;
        }

        /// <summary>
        /// Devuelve las marcaciones digitales y manuales de todo el personal en una fecha dada.
        /// </summary>
        /// <param name="fecha">Fecha buscada</param>
        /// <returns></returns>
        public static DS_Marcaciones ObtenerMarcaciones(DateTime fecha)
        {
            DS_Marcaciones ret = new DS_Marcaciones();
            //obtengo las marcaciones digitales de todo el personal en una fecha dada
            using (ClockCardEntities cxt = new ClockCardEntities())
            {
                var marcacionesDigitales = (from h in cxt.FICHADA
                                            where h.FIC_FECHA == fecha
                                            select new
                                            {
                                                Legajo = h.LEG_LEGAJO,
                                                Fecha = h.FIC_FECHA,
                                                Hora = h.FIC_HORA
                                            }).ToList();

                foreach (var item in marcacionesDigitales)
                {
                    DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                    dr.Legajo = item.Legajo.ToString();
                    dr.Fecha = item.Fecha;
                    dr.Hora = item.Hora;
                    dr.MarcaManual = false;
                    ret.Marcacion.Rows.Add(dr);
                }
            }

            using (Model1Container cxt = new Model1Container())
            {

                //Marcaciones .
                var resumenDiarioSinDigitalNiAnuladas = (from mm in cxt.Marcaciones
                                                         where
                                                         !mm.Anulada &&
                                                         mm.ResumenDiario.Dia == fecha
                                                         select new
                                                         {
                                                             Legajo = mm.ResumenDiario.Agente.Legajo,
                                                             Fecha = mm.ResumenDiario.Dia,
                                                             Hora = mm.Hora,
                                                             Manual = mm.Manual
                                                         }).ToList();

                foreach (var item in resumenDiarioSinDigitalNiAnuladas)
                {
                    var result = ret.Marcacion.Select("Legajo = '" + item.Legajo + "' AND Hora = '" + item.Hora + "'");
                    //Evito cargar una hora que ya esté en el resumen
                    if (result.Count() == 0)
                    {
                        DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                        dr.Legajo = item.Legajo.ToString();
                        dr.Fecha = item.Fecha;
                        dr.Hora = item.Hora;
                        dr.MarcaManual = item.Manual;
                        ret.Marcacion.Rows.Add(dr);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Devuelve las marcaciones digitales y manuales de todo el personal en una fecha dada.
        /// </summary>
        /// <param name="fecha">Fecha buscada</param>
        /// <returns></returns>
        public static DS_Marcaciones ObtenerMarcaciones(DateTime fechaDesde, DateTime fechaHasta)
        {
            DS_Marcaciones ret = new DS_Marcaciones();
            //obtengo las marcaciones digitales de todo el personal en una fecha dada
            using (ClockCardEntities cxt = new ClockCardEntities())
            {
                var marcacionesDigitales = (from h in cxt.FICHADA
                                            where h.FIC_FECHA >= fechaDesde && h.FIC_FECHA <= fechaHasta
                                            select new
                                            {
                                                Legajo = h.LEG_LEGAJO,
                                                Fecha = h.FIC_FECHA,
                                                Hora = h.FIC_HORA
                                            }).ToList();

                foreach (var item in marcacionesDigitales)
                {
                    DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                    dr.Legajo = item.Legajo.ToString();
                    dr.Fecha = item.Fecha;
                    dr.Hora = item.Hora;
                    dr.MarcaManual = false;
                    ret.Marcacion.Rows.Add(dr);
                }
            }

            using (Model1Container cxt = new Model1Container())
            {

                //Marcaciones .
                var resumenDiarioSinDigitalNiAnuladas = (from mm in cxt.Marcaciones
                                                         where
                                                         !mm.Anulada &&
                                                         mm.ResumenDiario.Dia >= fechaDesde && mm.ResumenDiario.Dia <= fechaHasta
                                                         select new
                                                         {
                                                             Legajo = mm.ResumenDiario.Agente.Legajo,
                                                             Fecha = mm.ResumenDiario.Dia,
                                                             Hora = mm.Hora,
                                                             Manual = mm.Manual
                                                         }).ToList();

                foreach (var item in resumenDiarioSinDigitalNiAnuladas)
                {
                    var result = ret.Marcacion.Select("Legajo = '" + item.Legajo + "' AND Hora = '" + item.Hora + "'");
                    //Evito cargar una hora que ya esté en el resumen
                    if (result.Count() == 0)
                    {
                        DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                        dr.Legajo = item.Legajo.ToString();
                        dr.Fecha = item.Fecha;
                        dr.Hora = item.Hora;
                        dr.MarcaManual = item.Manual;
                        ret.Marcacion.Rows.Add(dr);
                    }
                }
            }

            return ret;
        }

        public static DS_Marcaciones ObtenerMarcaciones(DateTime fechaDesde, DateTime fechaHasta, string legajo)
        {
            DS_Marcaciones ret = new DS_Marcaciones();
            int pruebaLegajo = 0;
            if (int.TryParse(legajo.ToString(), out pruebaLegajo))
            {
                //obtengo las marcaciones digitales de todo el personal en una fecha dada
                using (ClockCardEntities cxt = new ClockCardEntities())
                {
                    var marcacionesDigitales = (from h in cxt.FICHADA
                                                where
                                                h.FIC_FECHA >= fechaDesde && h.FIC_FECHA <= fechaHasta && h.LEG_LEGAJO == pruebaLegajo
                                                select new
                                                {
                                                    Legajo = h.LEG_LEGAJO,
                                                    Fecha = h.FIC_FECHA,
                                                    Hora = h.FIC_HORA
                                                }).ToList();

                    foreach (var item in marcacionesDigitales)
                    {
                        DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                        dr.Legajo = item.Legajo.ToString();
                        dr.Fecha = item.Fecha;
                        dr.Hora = item.Hora;
                        dr.MarcaManual = false;
                        ret.Marcacion.Rows.Add(dr);
                    }
                }

                using (Model1Container cxt = new Model1Container())
                {
                    //Marcaciones .
                    var resumenDiarioSinDigitalNiAnuladas = (from mm in cxt.Marcaciones
                                                             where
                                                             !mm.Anulada &&
                                                             mm.ResumenDiario.Dia >= fechaDesde && mm.ResumenDiario.Dia <= fechaHasta &&
                                                             mm.ResumenDiario.Agente.Legajo == pruebaLegajo
                                                             select new
                                                             {
                                                                 Legajo = mm.ResumenDiario.Agente.Legajo,
                                                                 Fecha = mm.ResumenDiario.Dia,
                                                                 Hora = mm.Hora,
                                                                 Manual = mm.Manual
                                                             }).ToList();

                    foreach (var item in resumenDiarioSinDigitalNiAnuladas)
                    {
                        var result = ret.Marcacion.Select("Legajo = '" + legajo + "' AND Hora = '" + item.Hora + "'");
                        //Evito cargar una hora que ya esté en el resumen
                        if (result.Count() == 0)
                        {
                            DS_Marcaciones.MarcacionRow dr = ret.Marcacion.NewMarcacionRow();
                            dr.Legajo = item.Legajo.ToString();
                            dr.Fecha = item.Fecha;
                            dr.Hora = item.Hora;
                            dr.MarcaManual = item.Manual;
                            ret.Marcacion.Rows.Add(dr);
                        }
                    }
                }
            }
            return ret;
        }

        #endregion

        #region Resumenes Diarios

        public static ResumenDiario AnalizarResumenDiario(ResumenDiario rdAg)
        {
            try
            {
                using (Model1Container cxt = new Model1Container())
                {
                    Agente ag = cxt.Agentes.First(a => a.Id == rdAg.AgenteId);
                    ResumenDiario rd = cxt.ResumenesDiarios.First(r => r.Id == rdAg.Id);

                    rd.Inconsistente = false;

                    //si no esta cerrado analizo
                    if (!(rd.Cerrado.HasValue && rd.Cerrado == true))
                    {
                        EstadoAgente ea = rd.Agente.ObtenerEstadoAgenteParaElDia(rd.Dia);
                        rd.AnularMarcacionesNulas();
                        rd.ObservacionInconsistente = string.Empty;

                        #region Analizo marcaciones
                        if (rd.Marcaciones.Count(m => !m.Anulada) % 2 != 0)
                        {
                            //cantidad de marcaciones impares
                            rd.ObservacionInconsistente = "Marcaciones impares";
                            rd.Inconsistente = true;
                        }
                        else
                        {
                            //Cantidad de marcaciones pares hay que 
                            //verificar si estan correctas las horas tomadas
                            if (rd.Marcaciones.Count(m => !m.Anulada) == 0)
                            {//puede no tener marcaciones y entrar aqui de todas maneras.

                                if (ea != null || new DateTime(rd.Dia.Year, rd.Agente.Legajo_datos_personales.FechaNacimiento.Month, rd.Agente.Legajo_datos_personales.FechaNacimiento.Day) == rd.Dia)
                                {
                                    rd.Inconsistente = false;
                                    if (ea != null)
                                    {
                                        rd.ObservacionInconsistente = ea.TipoEstado.Estado;
                                    }
                                    else
                                    {
                                        rd.ObservacionInconsistente = "Natalicio";
                                    }

                                    //rd.Cerrado = true;
                                }
                                else
                                {
                                    rd.Inconsistente = true;
                                    rd.ObservacionInconsistente = "Ausente.";
                                }
                            }
                            else
                            {   //tiene marcaciones y son pares.
                                List<Marcacion> entradaSalida = new List<Marcacion>();
                                Marcacion entradaAsignada = null;
                                Marcacion salidaAsignada = null;

                                if (HorasString.HoraNoNula(rd.HEntrada) && rd.Marcaciones.Where(m => !m.Anulada && m.Hora == rd.HEntrada).Count() > 0)
                                {
                                    entradaAsignada = rd.Marcaciones.First(m => !m.Anulada && m.Hora == rd.HEntrada);
                                    entradaSalida.Add(entradaAsignada);
                                }
                                else
                                {
                                    entradaAsignada = rd.Marcaciones.OrderBy(mm => mm.Hora).ToList()[0];// marcacionesAPartirDeLaHoraDesde[0];
                                    entradaSalida.Add(entradaAsignada);
                                }

                                if (HorasString.HoraNoNula(rd.HSalida) && rd.Marcaciones.Where(m => !m.Anulada && m.Hora == rd.HSalida).Count() > 0)
                                {
                                    salidaAsignada = rd.Marcaciones.First(m => !m.Anulada && m.Hora == rd.HSalida);
                                    entradaSalida.Add(salidaAsignada);
                                }
                                else
                                {
                                    salidaAsignada = rd.Marcaciones.OrderBy(mm => mm.Hora).ToList()[1];//marcacionesAPartirDeLaHoraDesde[1];
                                    entradaSalida.Add(salidaAsignada);
                                }

                                entradaSalida = entradaSalida.OrderBy(es => es.Hora).ToList();
                                //HorasTrabajadas: es la diferencia de horas entre la segunda marcacion y la primera
                                //string horasTrabajadas = HorasString.RestarHoras(entradaSalida[1].Hora, entradaSalida[0].Hora);
                                //int horas = 0;
                                //int minutos = 0;
                                rd.HEntrada = entradaSalida[0].Hora;
                                ////    rd.HSalida = entradaSalida[1].Hora;

                                //if ((int.TryParse(horasTrabajadas.Split(':')[0], out horas) && horas == 0) && (int.TryParse(horasTrabajadas.Split(':')[0], out minutos) && minutos <= 59))
                                //{
                                //    rd.Inconsistente = true;
                                //    rd.ObservacionInconsistente = "Trabajo menos de una hora.";
                                //    rd.HEntrada = entradaSalida[0].Hora;
                                //    rd.HSalida = entradaSalida[1].Hora;
                                //}
                                //else
                                //{
                                //    rd.HEntrada = entradaSalida[0].Hora;
                                //    rd.HSalida = entradaSalida[1].Hora;
                                //    rd.Inconsistente = false;
                                //    rd.ObservacionInconsistente = string.Empty;
                                //}
                            }
                        }
                        #endregion

                        #region Horarios flexibles

                        if (ag.HorarioFlexible.HasValue && ag.HorarioFlexible == true)
                        {
                            if (ea != null)
                            {
                                rd.Inconsistente = false;
                                rd.ObservacionInconsistente = ea.TipoEstado.Estado;
                                rd.Cerrado = true;

                                HorasMesHorarioFlexible hmf = rd.Agente.HorasMesHorarioFlexibles.FirstOrDefault(hhmm => hhmm.Anio == rd.Dia.Year && hhmm.Mes == rd.Dia.Month);
                                //Si no se acumulo el mes y el agente tenia agendado en el dia un movimiento de horas por cumplir antes de que se le agendara la justificacion de ausencia debo quitar el mismo y redistribuir las horas
                                if ((hmf == null || (hmf.AcumuladoEnTotalAnual ?? false) == false))
                                {
                                    MovimientoHora mh = rd.MovimientosHoras.FirstOrDefault(m => m.Descripcion == "" && m.Tipo.Tipo == "Horas por trabajar");
                                    if (mh != null)
                                    {
                                        cxt.MovimientosHoras.DeleteObject(mh);
                                        rd.Horas = HorasString.SumarHoras(new string[] { rd.Horas, "06:30" });
                                        rd.AcumuloHorasMes = null;
                                        rd.AcumuloHorasBonificacion = null;
                                        rd.AcumuloHorasAnioAnterior = null;
                                        rd.AcumuloHorasAnioActual = null;

                                        cxt.SaveChanges();

                                        hmf.HorasAcumuladas = HorasString.SumarHoras(new string[] { hmf.HorasAcumuladas, "06:30" });
                                        Model1Container cxta = new Model1Container();
                                        return cxta.ResumenesDiarios.First(rrdd => rrdd.Id == rd.Id);
                                    }
                                }

                            }
                            else
                            {
                                //si no tiene estado justificando ausencia (feriados, fin de semana, etc) debe cumplir xx:xx cantidad de horas diarias

                                MovimientoHora mh = rd.MovimientosHoras.FirstOrDefault(m => m.Descripcion == "" && m.Tipo.Tipo == "Horas por trabajar");

                                if (mh == null)
                                {
                                    TipoMovimientoHora tmh = cxt.TiposMovimientosHora.FirstOrDefault(tm => tm.Tipo == "Horas por trabajar");
                                    AgendarMovimientoHoraEnResumenDiario(rd.Dia, rd.Agente, rd.Agente, "06:30", tmh, "");
                                }

                                rd.Inconsistente = false;
                                rd.ObservacionInconsistente = "Horario flexible";
                            }
                        }

                        #endregion

                        cxt.SaveChanges();
                    }

                    //si esta cerrado, es flexible y no se acumuló
                    if (rd.Cerrado.HasValue && rd.Cerrado == true &&
                        ag.HorarioFlexible.HasValue && ag.HorarioFlexible == true &&
                        rd.Horas.Replace("-", "").Replace("000", "00") != "00:00" &&
                        rd.AcumuloHorasMes == null &&
                        rd.AcumuloHorasBonificacion == null &&
                        rd.AcumuloHorasAnioAnterior == null &&
                        rd.AcumuloHorasAnioActual == null)
                    {
                        ProcesosGlobales.DistribuirHorasCierreDiaHorarioFlexible(rd);
                        rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Id == rd.Id);
                    }

                    return rd;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool AbrirDiaCerrado(int id_agente, DateTime dia, int id_responsable)
        {
            bool ret = false;

            try
            {
                using (var cxt = new Model1Container())
                {
                    Agente ag = cxt.Agentes.FirstOrDefault(aagg => aagg.Id == id_agente);
                    Agente responsable = cxt.Agentes.First(aagg => aagg.Id == id_responsable);
                    ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.AgenteId == id_agente && rrdd.Dia == dia);
                    //Verifico que exista el resumen diario, que este cerrado y que no sea por franco compensatorio
                    if (rd != null && (rd.Cerrado ?? false) == true && rd.MovimientosHoras.FirstOrDefault(mh => mh.Tipo.Tipo == "Franco Compensatorio") == null)
                    {
                        ret = true;

                        if (ag.HorarioFlexible ?? false == true)
                        {
                            //el agente tiene horario flexible... Pueden pasar dos cosas, el mes esta acumulado o no
                            int anio = dia.Year; int mes = dia.Month;
                            HorasMesHorarioFlexible hmhf = cxt.HorasMesHorarioFlexibles.FirstOrDefault(hh => hh.AgenteId == id_agente && hh.Anio == anio && hh.Mes == mes);

                            if (hmhf != null)
                            {
                                if (hmhf.AcumuladoEnTotalAnual ?? false == true)
                                {
                                    //acumulo el mes en los totales anuales
                                    //PRIMERO: Debo restaurar las horas impactadas
                                    ag.HorasAcumuladasAnioActual = HorasString.RestarHoras(ag.HorasAcumuladasAnioActual, hmhf.HorasAcumuladas);
                                    hmhf.AcumuladoEnTotalAnual = null;
                                }

                                foreach (MovimientoHora mh in rd.MovimientosHoras)
                                {
                                    if (mh.Tipo.Suma)
                                    {
                                        hmhf.HorasAcumuladas = HorasString.RestarHoras(hmhf.HorasAcumuladas, mh.Horas);
                                    }
                                    else
                                    {
                                        hmhf.HorasAcumuladas = HorasString.SumarHoras(new string[] { hmhf.HorasAcumuladas, mh.Horas });
                                    }
                                }
                            }

                            rd.MovimientosHoras.ToList().ForEach(item => cxt.MovimientosHoras.DeleteObject(item));

                            rd.Cerrado = null;
                            rd.Horas = "00:00";
                            rd.AcumuloHorasAnioActual = null;
                            rd.AcumuloHorasAnioAnterior = null;
                            rd.AcumuloHorasBonificacion = null;
                            rd.AcumuloHorasMes = null;


                            AbrioDia abrio = new AbrioDia()
                            {
                                ResumenDiarioId = rd.Id,
                                AgenteId = responsable.Id
                            };

                            cxt.AbrioDias.AddObject(abrio);

                            cxt.SaveChanges();

                        }
                        else
                        {
                            //el agente no tiene horario flexible...

                            //PASOS:Eliminar los movimientos de horas asociados al resumen diario 
                            /////////a medida que voy eliminando voy restituyendo las horas
                            /////////Luego abro el resumen diario y limpio los valores que tenia cargados
                            foreach (MovimientoHora mh in rd.MovimientosHoras)
                            {
                                if (mh.Tipo.Suma)
                                {
                                    ag.HorasAcumuladasAnioActual = HorasString.RestarHoras(ag.HorasAcumuladasAnioActual, mh.Horas);
                                }
                                else
                                {
                                    ag.HorasAcumuladasAnioActual = HorasString.SumarHoras(new string[] { ag.HorasAcumuladasAnioActual, mh.Horas });
                                }
                            }

                            rd.MovimientosHoras.ToList().ForEach(item => cxt.MovimientosHoras.DeleteObject(item));

                            rd.Cerrado = null;
                            rd.Horas = "00:00";

                            cxt.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }


        #endregion

        internal static void RegistrarImpresion(Agente usuarioLogueado, string tipoReporte, DateTime fecha, string nombreMaquina, string localIP)
        {
            using (var cxt = new Model1Container())
            {
                cxt.ImpresionReportes.AddObject(new ImpresionReporte()
                {
                    AgenteId = usuarioLogueado.Id,
                    FechaHora = fecha,
                    NombreMaquina = nombreMaquina,
                    IP = localIP,
                    TipoReporte = tipoReporte
                });

                cxt.SaveChanges();
            }
        }

        internal static void RegistrarMovimientoSobreAgente(Agente agente_Destino, Agente agentePersonal, DateTime fecha, string nombreMaquina, string LocalIP, string tipoMovimiento)
        {
            using (var cxt = new Model1Container())
            {
                cxt.MovimientosSobreAgente.AddObject(new MovimientoSobreAgente()
                {
                    AgenteId = agente_Destino.Id,
                    AgenteId1 = agentePersonal.Id,
                    Fecha = fecha,
                    IP = LocalIP,
                    NombreMaquina = nombreMaquina,
                    TipoMovimiento = tipoMovimiento
                });

                cxt.SaveChanges();
            }
        }

        internal static int ObtenerDiasLicenciaAnualAgente(Agente agente, int year)
        {
            /*
                1	Licencia anual
                2	Licencia especial invierno
                3	Licencia enfermedad común
                4	Licencia enfermedad familiar
             */

            int dias = 0;

            double antiguedad = (new DateTime(year, 1, 1) - agente.Legajo_datos_laborales.FechaIngresoAminPub).TotalDays / 365;
            antiguedad = antiguedad + agente.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes + (agente.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes / 12);

            if (antiguedad <= 5)
            {
                dias = 23;
            }

            if (antiguedad > 5 && antiguedad <= 10)
            {
                dias = 28;
            }

            if (antiguedad > 10 && antiguedad <= 18)
            {
                dias = 42;
            }

            if (antiguedad > 18)
            {
                dias = 49;
            }

            return dias;
        }

        public static string ObtenerEncuadre(SolicitudDeEstado se)
        {
            if (se.TipoEnfermedad != null)
            {
                if (se.TipoEstadoAgente.Estado == "Licencia Anual" || se.TipoEstadoAgente.Estado == "Licencia Anual (Saldo)" || se.TipoEstadoAgente.Estado == "Licencia Anual (Anticipo)")
                {
                    return se.TipoEnfermedad.ToString();
                }
                else
                {
                    return ((TipoMovimientoEnfermedad)se.TipoEnfermedad).ToString();
                }
            }

            return string.Empty;
        }

        internal static int ObtenerDiasUsufructuados(LicenciaAgente lic)
        {
            int dias = 0;

            using (var cxt = new Model1Container())
            {
                string tipo = cxt.TiposDeLicencia.First(t => t.Id == lic.TipoLicenciaId).Tipo;
                var agente = cxt.Agentes.First(a => a.Id == lic.AgenteId);

                //tipo: Tipo de licencia <> Estado dia agente
                switch (tipo)
                {
                    //Licencia anual: cuenta los días de licencia de la tabla intermedia en donde guardo el año para la cual fue cargada (antes unicamente existia la tabla tipos de estado agente) 
                    //en la cual al guardarlo no cargaba el año y no podia hacer el recuento.
                    case "Licencia anual":

                        dias = (from du in agente.DiasUsufructados
                                where du.Anio == lic.Anio
                                select du).Count();
                        dias = (from du in cxt.DiasUsufructuados
                                where du.Anio == lic.Anio && du.AgenteId == lic.AgenteId
                                select du
                            ).Count();

                        dias = dias + lic.DiasUsufructuadosIniciales;
                        break;

                    //Licencia especial de invierno: Aquí recuento los dias agendados con TipoEstadoAgente "Licencia especial invierno"
                    case "Licencia especial invierno":
                        dias = (from ea in agente.EstadosPorDiaAgente
                                where ea.Dia.Year == lic.Anio && ea.TipoEstado.Estado == "Licencia especial invierno"
                                select ea).Count();
                        break;

                    //Licencia especial de invierno: Aquí recuento los dias agendados con TipoEstadoAgente "Enfermedad común"
                    case "Licencia enfermedad común":
                        dias = (from ea in agente.EstadosPorDiaAgente
                                where ea.Dia.Year == lic.Anio && ea.TipoEstado.Estado == "Enfermedad común"
                                select ea).Count();
                        break;

                    //Licencia especial de invierno: Aquí recuento los dias agendados con TipoEstadoAgente "Enfermedad familiar"
                    case "Licencia enfermedad familiar":
                        dias = (from ea in agente.EstadosPorDiaAgente
                                where ea.Dia.Year == lic.Anio && ea.TipoEstado.Estado == "Enfermedad familiar"
                                select ea).Count();
                        break;

                    default:
                        break;
                }

            }

            return dias;
        }
    }
}