using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace SisPer.Aplicativo
{
    public partial class Agente
    {
        /// <summary>
        /// Obtiene los agentes subordinados directos incluidos los jefes de las areas que depeden del mismo.
        /// </summary>
        /// <returns>Lista de agentes.</returns>
        public List<Agente> ObtenerAgentesSubordinadosDirectos()
        {
            Model1Container cxt = new Model1Container();
            //Obtengo la lista de agentes del departamento
            List<Agente> agentesSubordinados = (from agente in this.Area.Agentes
                                                where agente.Id != this.Id && agente.FechaBaja == null
                                                select agente).ToList();
            //Agrego los a la lista los jefes de los departamentos que estan subordinados al mio
            //ya que ellos no pueden ver sus solicitudes
            foreach (Area areaSubordinada in this.Area.Subordinados)
            {
                Agente jefe = areaSubordinada.Agentes.FirstOrDefault(a => a.Jefe && a.FechaBaja == null);
                Agente jefeTemporal = areaSubordinada.Agentes.FirstOrDefault(a => a.JefeTemporal && a.FechaBaja == null);

                if (jefe != null)
                {
                    agentesSubordinados.Add(jefe);
                }

                if (jefeTemporal != null)
                {
                    agentesSubordinados.Add(jefeTemporal);
                }
            }

            return agentesSubordinados;
        }

        /// <summary>
        /// Obtiene los agentes subordinados directos incluidos todos los de las areas que dependen del mismo.
        /// </summary>
        /// <param name="perfilPersonalComoAgente">
        /// Verdadero: Por mas que el agente sea de personal se tomará como agente normal, es decir devolverá unicamente los agentes que dependan unicamente de el.
        /// Falso: devolvera todos los agentes que no esten dados de baja ya que actua como agente de personal.</param>
        /// <returns>Lista de agentes.</returns>
        public List<Agente> ObtenerAgentesSubordinadosCascada(bool perfilPersonalComoAgente = false)
        {

            List<Agente> agentes = new List<Agente>();

            if (
                (perfilPersonalComoAgente || (this.Perfil) == PerfilUsuario.Agente) //Si es de personal pero ingresa como agente (jefe).
                && Area.Nombre != "Sub-Administración" //si es de subadministración se cargan todos los agentes de la administración que no esten dados de baja
                )
            {
                if ((this.Jefe || this.JefeTemporal) && this.Area != null)
                {
                    Area area = this.Area;
                    agentes = ObtenerAgentesAreaCascada(area);
                }
                else
                {
                    agentes.Add(this);
                }
            }
            else
            {
                Model1Container cxt = new Model1Container();
                agentes = cxt.Agentes.Where(a => a.FechaBaja == null).ToList();
            }

            return agentes;
        }

        private List<Agente> ObtenerAgentesAreaCascada(Area area)
        {
            List<Agente> ret = new List<Agente>();

            foreach (Agente item in area.Agentes.Where(a => a.FechaBaja == null))
            {//Cargo los agentes del area
                ret.Add(item);
            }

            if (area.Subordinados.Count != 0)
            {//Por cada area subordinada recorro en busqueda de mas agentes
                foreach (Area item in area.Subordinados)
                {
                    List<Agente> respuesta = ObtenerAgentesAreaCascada(item);

                    foreach (Agente item1 in respuesta)
                    {//Agrego los agents del area que acabo de recorrer
                        ret.Add(item1);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Obtiene el estado actual de agente, 
        /// si existe un estado agendado para el dia de hoy 
        /// Ej: Natalicio, Licencia, etc., Si no posee ningun estado agendado previamente,
        /// toma el estado dependiendo de las salidas.
        /// </summary>
        public string EstadoActual
        {
            get
            {
                string estado = ObtenerEstadoActual(this.Id);
                return estado == null ? "" : estado;
            }
        }


        private string ObtenerEstadoActual(int id)
        {
            string estado = string.Empty; 

            using (var cxt = new Model1Container())
            {
                estado = cxt.Estado_agente(id).First();
            }

            return estado;
        }

        /// <summary>
        /// Obtiene el estado del agente para un día dado
        /// </summary>
        /// <param name="d">Fecha buscada</param>
        /// <param name="SinFinDeSemana">Indica si la funcion muestra el fin de semana como estado o no</param>
        /// <returns></returns>
        public EstadoAgente ObtenerEstadoAgenteParaElDia(DateTime d, bool SinFinDeSemana = false)
        {
            EstadoAgente estado = null;
            var cxt = new Model1Container();

            if (SinFinDeSemana)
            {
                #region Sin fin de semana (se usa para calendarios evitando el first or default cuando tienen fin de semana y licencia anual o algun otro estado)
                estado = cxt.EstadosAgente.FirstOrDefault(ea => ea.Dia.Day == d.Day && ea.Dia.Month == d.Month && ea.Dia.Year == d.Year && ea.AgenteId == Id && ea.TipoEstado.Estado != "Fin de semana");
                if (estado == null)
                {
                    //verifico si no es el natalicio si es asi, LO GUARDO
                    try
                    {
                        if (Legajo_datos_personales.FechaNacimiento == new DateTime(Legajo_datos_personales.FechaNacimiento.Year, d.Month, d.Day))
                        {
                            TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Natalicio");
                            EstadoAgente ea = new EstadoAgente()
                            {
                                AgenteId = Id,
                                AgenteId1 = Id,
                                Dia = d,
                                TipoEstadoAgenteId = tea.Id
                            };

                            cxt.EstadosAgente.AddObject(ea);
                            cxt.SaveChanges();

                            estado = cxt.EstadosAgente.FirstOrDefault(e => e.Id == ea.Id);
                        }
                    }
                    catch { }

                    if (estado == null && cxt.Feriados.FirstOrDefault(f => f.Dia == d) != null)
                    {
                        TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Feriado");
                        EstadoAgente ea = new EstadoAgente()
                        {
                            AgenteId = Id,
                            AgenteId1 = Id,
                            Dia = d,
                            TipoEstadoAgenteId = tea.Id
                        };

                        cxt.EstadosAgente.AddObject(ea);
                        cxt.SaveChanges();

                        estado = cxt.EstadosAgente.FirstOrDefault(e => e.Id == ea.Id);
                    }
                }

                #endregion
            }
            else
            {
                #region Con fin de semana
                estado = cxt.EstadosAgente.FirstOrDefault(ea => ea.Dia.Day == d.Day && ea.Dia.Month == d.Month && ea.Dia.Year == d.Year && ea.AgenteId == Id);
                if (estado == null)
                {
                    //verifico si no es el natalicio si es asi, LO GUARDO
                    try
                    {
                        if (Legajo_datos_personales.FechaNacimiento == new DateTime(Legajo_datos_personales.FechaNacimiento.Year, d.Month, d.Day))
                        {
                            TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Natalicio");
                            EstadoAgente ea = new EstadoAgente()
                            {
                                AgenteId = Id,
                                AgenteId1 = Id,
                                Dia = d,
                                TipoEstadoAgenteId = tea.Id
                            };

                            cxt.EstadosAgente.AddObject(ea);
                            cxt.SaveChanges();

                            estado = cxt.EstadosAgente.FirstOrDefault(e => e.Id == ea.Id);
                        }
                    }
                    catch { }
                    

                    if (estado == null && cxt.Feriados.FirstOrDefault(f => f.Dia == d) != null)
                    {
                        TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Feriado");
                        EstadoAgente ea = new EstadoAgente()
                        {
                            AgenteId = Id,
                            AgenteId1 = Id,
                            Dia = d,
                            TipoEstadoAgenteId = tea.Id
                        };

                        cxt.EstadosAgente.AddObject(ea);
                        cxt.SaveChanges();

                        estado = cxt.EstadosAgente.FirstOrDefault(e => e.Id == ea.Id);
                    }

                    if (estado == null && (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday))
                    {
                        //NO TIENE ESTADO AGENDADO PERO ES FIN DE SEMANA
                        //PRIMERO REVISO QUE NO TENGA MARCACIONES CARGADAS Y SI ES ASI, DEVUELVO EL DIA COMO CON ESTADO FIN DE SEMANA.
                        ResumenDiario rd = cxt.ResumenesDiarios.FirstOrDefault(rrdd => rrdd.Dia == d && rrdd.AgenteId == this.Id);

                        if (rd != null && rd.Marcaciones.Count > 0)
                        {
                            //Si entro por aca es porque tiene marcaciones por lo tanto no tengo que cargar automáticamente un estado.
                        }
                        else
                        {
                            TipoEstadoAgente tea = cxt.TiposEstadoAgente.First(t => t.Estado == "Fin de semana");
                            EstadoAgente ea = new EstadoAgente()
                            {
                                AgenteId = Id,
                                AgenteId1 = Id,
                                Dia = d,
                                TipoEstadoAgenteId = tea.Id
                            };

                            cxt.EstadosAgente.AddObject(ea);
                            cxt.SaveChanges();

                            estado = cxt.EstadosAgente.FirstOrDefault(e => e.Id == ea.Id);
                        }
                    }
                }
                #endregion
            }

            return estado;
        }

        public ResumenDiario ObtenerResumenDiario(DateTime dia)
        {
            Model1Container cxt = new Model1Container();
            return cxt.ResumenesDiarios.FirstOrDefault(rd => rd.AgenteId == this.Id && rd.Dia == dia);
        }

        /// <summary>
        /// Devuelve los valores pasados por parametro.
        /// </summary>
        /// <param name="d">Parametro de entrada: Dia buscado</param>
        /// <param name="LlegoTarde">Salida: Devuelve si llego tarde o no. Para esta función el no tener marca tambien marca como llegada tarde.</param>
        /// <param name="HoraMarcada">Salida: Hora de marcación o "No marco" si no marco la entrada</param>
        /// <param name="MarcoManual">Salida: Indica si la marcación fue manual o no</param>
        public void Tardanza(DateTime d, out bool LlegoTarde, out string HoraMarcada, out bool MarcoManual)
        {
            LlegoTarde = true;
            HoraMarcada = "999:99";
            MarcoManual = false;
            string HoraMasMargenDeCincoMinutos = HorasString.SumarHoras(new string[] { "000:05", this.ObtenerHoraEntradaLaboral(d) });
            DS_Marcaciones ds = ProcesosGlobales.ObtenerMarcaciones(d, this.Legajo.ToString());

            if (ds != null && ds.Marcacion.Rows.Count > 0)
            {
                //obtengo la menor marcacion del día.
                foreach (DS_Marcaciones.MarcacionRow item in ds.Marcacion.Rows)
                {
                    if (HorasString.AMayorQueB(HoraMarcada, item.Hora))
                    {
                        HoraMarcada = item.Hora;
                        MarcoManual = item.MarcaManual;
                    }
                }
            }

            LlegoTarde = HorasString.AMayorQueB(HoraMasMargenDeCincoMinutos, HoraMarcada) ? false : true;

            HoraMarcada = HoraMarcada == "999:99" ? "No marco" : HoraMarcada;
        }

        /// <summary>
        /// Verifica si existe algún  medio franco que modifica la hora de entrada y ajusta la misma
        /// Ej: Causa de fuerza mayor, lluvia extrema.-
        /// </summary>
        /// <returns></returns>
        public string ObtenerHoraEntradaLaboral(DateTime dia)
        {
            string ret = this.HoraEntrada;

            using (var cxt = new Model1Container())
            {
                AsuetoParcial ap = cxt.AsuetosParciales.FirstOrDefault(aapp => aapp.AreaId == this.AreaId && aapp.Dia == dia && aapp.HorarioQueModifica == "Entrada");

                if (ap != null)
                {
                    ret = ap.Hora;
                }
            }

            return ret;
        }

        /// <summary>
        /// Verifica si existe algún  medio franco que modifica la hora de salida y ajusta la misma
        /// Ej:Visita de cristina salida a las 11 hs;
        /// </summary>
        /// <returns></returns>
        public string ObtenerHoraSalidaLaboral(DateTime dia)
        {
            string ret = this.HoraSalida;

            using (var cxt = new Model1Container())
            {
                AsuetoParcial ap = cxt.AsuetosParciales.FirstOrDefault(aapp => aapp.AreaId == this.AreaId && aapp.Dia == dia && aapp.HorarioQueModifica == "Salida");

                if (ap != null)
                {
                    ret = ap.Hora;
                }
            }

            return ret;
        }
    }
}