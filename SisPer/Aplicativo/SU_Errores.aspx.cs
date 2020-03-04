using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class SU_Errores : System.Web.UI.Page
    {
        private struct MiError
        {
            public DateTime nDATE { get; set; }
            public string nAGENTE { get; set; }
            public string nMESSAGE { get; set; }
            public string nSOURCE { get; set; }
            public string nINSTANCE { get; set; }
            public string nDATA { get; set; }
            public string nURL { get; set; }
            public string nTARGETSITE { get; set; }
            public string nSTACKTRACE { get; set; }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ObtenerLosErrores();
            }
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime d = e.Day.Date;
            List<MiError> errores = Session["ErroresLog"] as List<MiError>;

            var erroresDelDia = (from err in errores
                                 where err.nDATE.Date == d.Date
                                 select err).ToList();

            if (erroresDelDia.Count > 0)
            {
                e.Cell.BackColor = Color.DarkGreen;
                e.Cell.ForeColor = Color.Azure;
                e.Cell.Font.Bold = true;
                e.Cell.ToolTip = "Cantidad errores registrados " + erroresDelDia.Count;
            }
        }

        private void ObtenerLosErrores()
        {
            string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
            StreamReader sr = new StreamReader(directorioRaiz + "Errores_Log.txt");

            List<string> listaErrores = new List<string>();
            List<MiError> listaMisErrores = new List<MiError>();
            string error = string.Empty;
            string linea = string.Empty;

            do
            {
                do
                {
                    linea = sr.ReadLine();
                    if (linea != null)
                    {
                        if (linea != "******************************************************************" && linea.TrimEnd().TrimStart().Length > 0)
                        {
                            error = error + linea + "|";
                        }
                    }
                } while (linea != "******************************************************************" && linea != null && !sr.EndOfStream);
                //llego al final del documento o a la linea de asteriscos

                if (error != "")
                {
                    listaErrores.Add(error);
                    error = string.Empty;
                }

            } while (!sr.EndOfStream);

            foreach (string err in listaErrores)
            {
                listaMisErrores.Add(new MiError()
                {
                    nDATE = Convert.ToDateTime(err.Split('|')[0].Replace("DATE:", "")),
                    nAGENTE = err.Split('|')[1].Replace("AGENTE:", ""),
                    nMESSAGE = err.Split('|')[2].Replace("MESSAGE:", ""),
                    nSOURCE = err.Split('|')[3].Replace("SOURCE:", ""),
                    nINSTANCE = err.Split('|')[4].Replace("INSTANCE:", ""),
                    nDATA = err.Split('|')[5].Replace("DATA:", ""),
                    nURL = err.Split('|')[6].Replace("URL:", ""),
                    nTARGETSITE = err.Split('|')[7].Replace("TARGETSITE:", ""),
                    nSTACKTRACE = err.Split('|')[8].Replace("STACKTRACE:", ""),
                });
            }

            Session["ErroresLog"] = listaMisErrores;

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            CargarErroresFecha();
        }

        private void CargarErroresFecha()
        {
            DateTime d = Calendar1.SelectedDate;
            List<MiError> errores = Session["ErroresLog"] as List<MiError>;

            var erroresDelDia = (from err in errores
                                 where err.nDATE.Date == d.Date
                                 select err).ToList();

            lv_Errores.DataSource = erroresDelDia;
            lv_Errores.DataBind();
        }

        protected void btn_VaciarLog_Click(object sender, EventArgs e)
        {
            string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
            string path = directorioRaiz + "Errores_Log.txt";
            using (var stream = new FileStream(path, FileMode.Truncate))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("");
                }
            }

            ObtenerLosErrores();
        }

        protected void btn_eliminarRDDuplicados_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                var resumenesDiariosDuplicados = (from rd in cxt.ResumenesDiarios
                                                  group rd by rd.AgenteId into rrdd
                                                  select new
                                                  {
                                                      AgenteId = rrdd.Key,
                                                      ResumenesDia = (from resumen in rrdd
                                                                      group resumen by resumen.Dia into resumenDia
                                                                      select new
                                                                      {
                                                                          Dia = resumenDia.Key,
                                                                          ResumenesDiaAgente = resumenDia
                                                                      }).Where(rda => rda.ResumenesDiaAgente.Count() > 1)
                                                  }).Where(rrddAg => rrddAg.ResumenesDia.Count() > 0);

                foreach (var rrddAgente in resumenesDiariosDuplicados)
                {
                    foreach (var dia in rrddAgente.ResumenesDia)
                    {
                        int i = 0;
                        while (i < dia.ResumenesDiaAgente.Count() - 1)
                        {
                            ResumenDiario rd = null;

                            int rrddCerradosdia = dia.ResumenesDiaAgente.Count(rrddaa => rrddaa.Cerrado.HasValue && rrddaa.Cerrado == true);

                            while (rrddCerradosdia > 1)
                            {
                                rd = dia.ResumenesDiaAgente.LastOrDefault(rda => rda.Cerrado.HasValue && rda.Cerrado == true);
                                
                                if (rd != null)
                                {
                                    while (rd.Marcaciones.Count > 0)
                                    {
                                        cxt.Marcaciones.DeleteObject(rd.Marcaciones.First());
                                    }

                                    while (rd.MovimientosHoras.Count > 0)
                                    {
                                        cxt.MovimientosHoras.DeleteObject(rd.MovimientosHoras.First());
                                    }

                                    cxt.ResumenesDiarios.DeleteObject(rd);
                                    i++;
                                    rrddCerradosdia--;
                                }
                            }

                            rd = dia.ResumenesDiaAgente.FirstOrDefault(rda => !rda.Cerrado.HasValue || rda.Cerrado == false);

                            if (rd != null)
                            {
                                while (rd.Marcaciones.Count > 0)
                                {
                                    cxt.Marcaciones.DeleteObject(rd.Marcaciones.First());
                                }

                                while (rd.MovimientosHoras.Count > 0)
                                {
                                    cxt.MovimientosHoras.DeleteObject(rd.MovimientosHoras.First());
                                }

                                cxt.ResumenesDiarios.DeleteObject(rd);
                            }
                            i++;
                        }

                    }
                }

                cxt.SaveChanges();

            }
           
        }

        protected void btn_UnificarLegajo_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                foreach (Agente item in cxt.Agentes.Where(a=>a.FechaBaja.HasValue))
                {
                    //recorro los agentes que fueron dados de baja.-
                    Agente agenteSinDarDeBaja = cxt.Agentes.FirstOrDefault(aa=>aa.Legajo == item.Legajo && !aa.FechaBaja.HasValue);
                    //si existe con el mismo legajo pero esta dado de alta, migro los movimientos del dado de baja al dado de alta
                    if (agenteSinDarDeBaja != null)
                    { 
                        //Estados
                        foreach (EstadoAgente estadoItem in item.EstadosPorDiaAgente)
                        {
                            EstadoAgente ea = agenteSinDarDeBaja.ObtenerEstadoAgenteParaElDia(estadoItem.Dia);
                            if (ea == null || (ea != null && ea.TipoEstado != estadoItem.TipoEstado))
                            {
                                ea = new EstadoAgente();
                                ea.Agente = agenteSinDarDeBaja;
                                ea.Dia = estadoItem.Dia;
                                ea.TipoEstado = estadoItem.TipoEstado;
                                ea.AgendadoPor = estadoItem.AgendadoPor;
                            }
                        }

                        //Francos Compensatorios
                        foreach (Franco francoItem in item.Francos)
                        {
                            Franco fc = agenteSinDarDeBaja.Francos.FirstOrDefault(f => f.DiasFranco.First().Dia == francoItem.DiasFranco.First().Dia);
                            if (fc == null)
                            {
                                fc = new Franco();
                                fc.Agente = agenteSinDarDeBaja;
                                fc.DiasFranco.Add(new DiaFranco()
                                {
                                    Dia = francoItem.DiasFranco.First().Dia
                                });
                                fc.Estado = francoItem.Estado;
                                fc.FechaSolicitud = francoItem.FechaSolicitud;
                                fc.InfPersSaldoAnioAnt = francoItem.InfPersSaldoAnioAnt;
                                fc.InfPersSaldoAnioEnCurso = francoItem.InfPersSaldoAnioEnCurso;
                                foreach (MovimientoFranco mfItem in francoItem.MovimientosFranco)
                                {
                                    fc.MovimientosFranco.Add(new MovimientoFranco()
                                    {
                                        Agente = mfItem.Agente,
                                        Estado = mfItem.Estado,
                                        Fecha = mfItem.Fecha,
                                        Observacion = mfItem.Observacion
                                    });
                                }
                            }
                        }

                        //salidas
                        foreach (Salida salidaItem in item.Salidas)
                        {
                            Salida sal = agenteSinDarDeBaja.Salidas.FirstOrDefault(s => s.Dia == salidaItem.Dia && s.HoraDesde == salidaItem.HoraDesde);
                            if (sal == null)
                            {
                                sal = new Salida();
                                sal.Agente = agenteSinDarDeBaja;
                                sal.Destino = salidaItem.Destino;
                                sal.Dia = salidaItem.Dia;
                                sal.HoraDesde = salidaItem.HoraDesde;
                                sal.HoraHasta = salidaItem.HoraHasta;
                                sal.Jefe = salidaItem.Jefe;
                                sal.Tipo = salidaItem.Tipo;
                            }
                        }

                        //horarios vespertinos
                        foreach (HorarioVespertino hvItem in item.HorariosVespertinos)
                        {
                            HorarioVespertino hv = agenteSinDarDeBaja.HorariosVespertinos.FirstOrDefault(hhvv => hhvv.Dia == hvItem.Dia && hhvv.Motivo == hvItem.Motivo);
                            if (hv == null)
                            {
                                hv = new HorarioVespertino();
                                hv.Agente = agenteSinDarDeBaja;
                                hv.Dia = hvItem.Dia;
                                hv.Estado = hvItem.Estado;
                                hv.HoraFin = hvItem.HoraFin;
                                hv.HoraInicio = hvItem.HoraInicio;
                                hv.Jefe = hvItem.Jefe;
                                hv.Motivo = hvItem.Motivo;
                            }
                        }

                        //resumenesDiarios
                        foreach (ResumenDiario rdItem in item.ResumenesDiarios)
                        {
                            ResumenDiario rd = agenteSinDarDeBaja.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Dia == rdItem.Dia);

                            if (rd == null)
                            {
                                //paso todo el resumen diario
                                rd = new ResumenDiario();
                                rd.Agente = agenteSinDarDeBaja;
                                rd.AcumuloHorasAnioActual = rdItem.AcumuloHorasAnioActual;
                                rd.AcumuloHorasAnioAnterior = rdItem.AcumuloHorasAnioAnterior;
                                rd.AcumuloHorasBonificacion = rdItem.AcumuloHorasBonificacion;
                                rd.AcumuloHorasMes = rdItem.AcumuloHorasMes;
                                rd.Cerrado = rdItem.Cerrado;
                                rd.Dia = rdItem.Dia;
                                rd.HEntrada = rdItem.HEntrada;
                                rd.Horas = rdItem.Horas;
                                rd.HSalida = rdItem.HSalida;
                                rd.HVEnt = rdItem.HVEnt;
                                rd.HVSal = rdItem.HVSal;
                                rd.Inconsistente = rdItem.Inconsistente;
                                foreach (Marcacion marcacionItem in rdItem.Marcaciones)
                                {
                                    rd.Marcaciones.Add(new Marcacion()
                                    {
                                        AgenteAnulo = marcacionItem.AgenteAnulo,
                                        Anulada = marcacionItem.Anulada,
                                        Hora = marcacionItem.Hora,
                                        Manual = marcacionItem.Manual
                                    });
                                }
                                rd.MarcoProlongJornada = rdItem.MarcoProlongJornada;
                                rd.MarcoTardanza = rdItem.MarcoTardanza;
                                foreach (MovimientoHora mhItem in rdItem.MovimientosHoras)
                                {
                                    rd.MovimientosHoras.Add(new MovimientoHora()
                                    {
                                        AgendadoPor = mhItem.AgendadoPor,
                                        DescontoDeAcumuladoAnioAnterior = mhItem.DescontoDeAcumuladoAnioAnterior,
                                        DescontoDeHorasBonificables = mhItem.DescontoDeHorasBonificables,
                                        Descripcion = mhItem.Descripcion,
                                        Horas = mhItem.Horas,
                                        Tipo = mhItem.Tipo
                                    });
                                }
                                rd.ObservacionInconsistente = rdItem.ObservacionInconsistente;
                            }
                            else
                            {
                                //reviso los movimientos y las marcaciones
                                //movimientos horas

                                rd.Agente = agenteSinDarDeBaja;
                                rd.AcumuloHorasAnioActual = rdItem.AcumuloHorasAnioActual;
                                rd.AcumuloHorasAnioAnterior = rdItem.AcumuloHorasAnioAnterior;
                                rd.AcumuloHorasBonificacion = rdItem.AcumuloHorasBonificacion;
                                rd.AcumuloHorasMes = rdItem.AcumuloHorasMes;
                                rd.Cerrado = rdItem.Cerrado;
                                rd.Dia = rdItem.Dia;
                                rd.HEntrada = rdItem.HEntrada;
                                rd.Horas = rdItem.Horas;
                                rd.HSalida = rdItem.HSalida;
                                rd.HVEnt = rdItem.HVEnt;
                                rd.HVSal = rdItem.HVSal;
                                rd.Inconsistente = rdItem.Inconsistente;

                                foreach (MovimientoHora mhItem in rdItem.MovimientosHoras)
                                {
                                    MovimientoHora mh = rd.MovimientosHoras.FirstOrDefault(mmhh => mmhh.Tipo == mhItem.Tipo && mhItem.Horas == mmhh.Horas);
                                    if (mh == null)
                                    {
                                        rd.MovimientosHoras.Add(new MovimientoHora()
                                        {
                                            AgendadoPor = mhItem.AgendadoPor,
                                            DescontoDeAcumuladoAnioAnterior = mhItem.DescontoDeAcumuladoAnioAnterior,
                                            DescontoDeHorasBonificables = mhItem.DescontoDeHorasBonificables,
                                            Descripcion = mhItem.Descripcion,
                                            Horas = mhItem.Horas,
                                            Tipo = mhItem.Tipo
                                        });
                                    }
                                }

                                foreach (Marcacion marcacionItem in rdItem.Marcaciones)
                                {
                                    Marcacion marcacion = rd.Marcaciones.FirstOrDefault(rrdd => rrdd.Hora == marcacionItem.Hora && rrdd.Manual == marcacionItem.Manual && rrdd.Anulada == marcacionItem.Anulada);
                                    if (marcacion == null)
                                    {
                                        rd.Marcaciones.Add(new Marcacion()
                                        {
                                            AgenteAnulo = marcacionItem.AgenteAnulo,
                                            Anulada = marcacionItem.Anulada,
                                            Hora = marcacionItem.Hora,
                                            Manual = marcacionItem.Manual
                                        });
                                    }
                                }
                            }
                            
                        }
                       
                    }

                }

                cxt.SaveChanges();
            }
        }
    }
}