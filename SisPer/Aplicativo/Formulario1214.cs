using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class Formulario1214
    {
        public bool PuedeEnviar()
        {
            bool ret = true;

            foreach (Agente1214 agente in Nomina)
            {
                if (agente.Estado == EstadoAgente1214.Solicitado)
                {
                    ret = false;
                }
            }

            ret = ret && Estado == Estado1214.Confeccionado;

            return ret;
        }
    }
}