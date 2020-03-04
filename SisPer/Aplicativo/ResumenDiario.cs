using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class ResumenDiario
    {
        public void AnularMarcacionesNulas()
        {
            foreach (Marcacion item in this.Marcaciones)
            {
                int iHora = Convert.ToInt16(item.Hora.Split(':')[0]);
                int iMin = Convert.ToInt16(item.Hora.Split(':')[1]);
                if (iHora + iMin == 0)
                {
                    item.Anulada = true;
                }
            }
        }

        public string HorasConMovimientosSinCerrar()
        {
            string ret = string.Empty;

            ret = Horas;


            foreach (MovimientoHora item in MovimientosHoras)
            {
                if (item.Id == 0)
                {
                    if (item.Tipo.Suma && !ret.Contains("-"))
                    {
                        ret = HorasString.SumarHoras(new string[] { ret, item.Horas });
                    }
                    else
                    {
                        if (item.Tipo.Suma && ret.Contains("-"))
                        {
                            ret = HorasString.RestarHoras(item.Horas, ret.Replace("-", ""));
                        }
                        else
                        {
                            //el movimiento resta pero como el acumulado es negativo lo sumo
                            if (ret.Contains("-"))
                            {
                                ret = "-" + HorasString.SumarHoras(new string[] { ret.Replace("-", ""), item.Horas });
                            }
                            else
                            {
                                ret = HorasString.RestarHoras(ret, item.Horas);
                            }
                        }
                    }
                }
            }

            return ret;
        }

    }
}