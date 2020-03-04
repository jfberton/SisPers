using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class Legajo_hijo
    {
        public int Edad
        {
            get {
                return DateTime.Today.AddTicks(-Fecha_de_nacimiento.Ticks).Year - 1;
            }
        }
    }
}