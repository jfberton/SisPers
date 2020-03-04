using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public partial class Area
    {
        /// <summary>
        /// Devuelve los agentes que estuvieron en el area en la fecha buscada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>null si no existen datos en la fecha solicitada</returns>
        public List<Agente> ObtenerAgentesAl(DateTime fecha)
        {
            List<Agente> ret = new List<Agente>();

            ret = (from r in this.ReasignacionesDeAgentes
                   where r.Desde <= fecha && (r.Hasta >= fecha || r.Hasta == null)
                   select r.Agente).ToList();

            return ret;
        }

        /// <summary>
        /// Detecta si en algun momento de la cadena de control se produce algun ciclo
        /// </summary>
        /// <returns></returns>
        public bool Recursivo(Area area = null, int idOriginal=0)
        {
            bool ret =false;
            area = area == null ? this : area;
            idOriginal = idOriginal == 0 ? this.Id : idOriginal;

            if (area.DependeDe == null || area.DependeDe.Id == idOriginal)
            {
                //si dependeDe != null y entro aca entonces los id son iguales y hay recursividad.
                ret = area.DependeDe != null;
            }
            else
            {
                ret = Recursivo(area.DependeDe, idOriginal);
            }

            return ret;
        }
    }
}