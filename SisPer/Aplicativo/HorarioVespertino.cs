using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class HorarioVespertino
    {
        public string Horas {
            get {
                return HorasString.RestarHoras(HoraFin, HoraInicio);
            }
        }
    }
}