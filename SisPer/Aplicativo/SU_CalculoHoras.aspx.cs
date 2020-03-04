using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class SU_CalculoHoras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_sumar_Click(object sender, EventArgs e)
        {
            try
            {
                lb_resutado_suma.Text = HorasString.SumarHoras(new string[] { tb_sum_primer_termino.Text, tb_sum_segundo_termino.Text });
            }
            catch
            {
                lb_resutado_suma.Text = "Error en los datos ingresados";
            }
        }

        protected void btn_restar_Click(object sender, EventArgs e)
        {
            try
            {
                lb_resultado_resta.Text = HorasString.RestarHoras(tb_rest_primer_termino.Text, tb_rest_segundo_termino.Text);
            }
            catch
            {
                lb_resultado_resta.Text = "Error en los datos ingresados";
            }
        }

        protected void btn_recalcular_licencias_Click(object sender, EventArgs e)
        {
            /*
               1	Licencia anual
               2	Licencia especial invierno
               3	Licencia enfermedad común
               4	Licencia enfermedad familiar
            */

            using (var cxt = new Model1Container())
            {
                List<Agente> agentes = cxt.Agentes.Where(a => a.FechaBaja == null).ToList();

                foreach (Agente agente in agentes)
                {
                    int dias = 0;
                    int year = DateTime.Today.Year - 1;
                    LicenciaAgente la = cxt.LicenciasAgentes.FirstOrDefault(ll => ll.AgenteId == agente.Id && ll.Anio == year && ll.TipoLicenciaId == 1);

                    double antiguedad = (new DateTime(year, 12, 1) - agente.Legajo_datos_laborales.FechaIngresoAminPub).TotalDays / 365;
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

                    if (la == null)
                    {
                        la = new LicenciaAgente()
                        {
                            AgenteId = agente.Id,
                            DiasOtorgados = dias,
                            DiasUsufructuadosIniciales = 0,
                            Anio = year,
                            TipoLicenciaId = 1
                        };
                        cxt.LicenciasAgentes.AddObject(la);
                    }
                    else
                    {
                        if (la.DiasOtorgados < dias)
                        {
                            la.DiasOtorgados = dias;
                        }
                    }

                    
                }

                cxt.SaveChanges();
            }


        }
    }
}