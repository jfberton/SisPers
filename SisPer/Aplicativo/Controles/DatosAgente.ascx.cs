using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class DatosAgente : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        Agente agente = null;

        public Agente Agente
        {
            get
            {
                Model1Container cxt = new Model1Container();
                int id = lbl_Id.Text != null ? Convert.ToInt32(lbl_Id.Text) : 0;
                agente = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                return agente;
            }
            set
            {
                Model1Container cxt = new Model1Container();
                agente = cxt.Agentes.FirstOrDefault(a => a.Id == value.Id);

                #region Cargar datos del agente

                var datos_agente = cxt.sp_obtener_datos_agente(agente.Id).First();

                lbl_Id.Text = datos_agente.Id.ToString();
                lbl_NombreAgente.Text = datos_agente.ApellidoYNombre;

                if ((datos_agente.HorarioFlexible ?? false) == true)
                {
                    lbl_areaFlexible.Text = datos_agente.Area;
                    lbl_legajoFlexible.Text = datos_agente.Legajo.ToString();
                    lbl_mailFlexible.Text = datos_agente.Email;

                    lbl_HorasAnioActualFlexible.Text = datos_agente.HorasAcumuladasAnioActual;
                    lbl_HorasAnioAnteriorFlexible.Text = datos_agente.HorasAcumuladasAnioAnterior;
                    lbl_HorasAcumuladasMes.Text = datos_agente.horas_hf_acumuladas;

                    lbl_HorasTotalesFlexible.Text = HorasString.SumarHoras(new string[] { lbl_HorasAnioActualFlexible.Text, lbl_HorasAnioAnteriorFlexible.Text, lbl_HorasAcumuladasMes.Text });

                    lbl_BonificacionFlexible.Text = datos_agente.PoseeBonificacion ? "Si" : "No";
                    lblHorasBonificacionACubrirFlexible.Text = datos_agente.Horas_bonificacion_resta_cumplir;

                    //tiene que hacer seis horas y media, de las cuales les resto las horas hechas hasta el momento
                    string horasQueRestanPorCumplir = datos_agente.Horas_HF_por_cumplir;
                    //si no hizo mas horas de las seis y media que tenia que hacer muestro cero unicamente.
                    if (!horasQueRestanPorCumplir.Contains("-"))
                    {
                        lbl_HorasPorCumplirDiaFlexible.Text = horasQueRestanPorCumplir;
                        lbl_HorasPorCumplirDiaFlexible.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        lbl_HorasPorCumplirDiaFlexible.Text = "00:00";
                        lbl_HorasPorCumplirDiaFlexible.ForeColor = Color.Black;
                    }

                    p_AgenteComun.Visible = false;
                    p_agenteFlexible.Visible = true;
                }
                else
                {
                    lbl_Area.Text = datos_agente.Area;
                    lbl_Legajo.Text = datos_agente.Legajo.ToString();
                    lbl_mail.Text = datos_agente.Email;

                    lbl_HorasAnioActual.Text = datos_agente.HorasAcumuladasAnioActual;
                    lbl_HorasAnioAnterior.Text = datos_agente.HorasAcumuladasAnioAnterior;
                    lbl_HorasTotales.Text = datos_agente.Total_horas_disponible;

                    lbl_Tardanzas.Text = datos_agente.Horas_tardanzas;

                    lbl_Bonificacion.Text = agente.PoseeBonificacion ? "Si" : "No";
                    lbl_HorasBonificacionACubrir.Text = datos_agente.Horas_bonificacion_resta_cumplir;

                    p_AgenteComun.Visible = true;
                    p_agenteFlexible.Visible = false;
                }

                #endregion

            }
        }


        public void Refrescar()
        {
            this.Agente = agente;
        }
    }
}