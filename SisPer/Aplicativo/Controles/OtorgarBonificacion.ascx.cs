using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo.Controles
{
    public partial class OtorgarBonificacion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                
            }
        }

        public delegate void miDelegado();

        public event miDelegado OtorgoBonificacion;

        public string IdAgente 
        { 
            get { return AgenteId.Value; } 
            set { 
                AgenteId.Value = value.ToString();
                int id = 0;
                using (var cxt = new Model1Container())
                {
                    if (int.TryParse(AgenteId.Value, out id))
                    {
                        Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                        if (ag != null)
                        {
                            Label1.Text = ag.ApellidoYNombre;
                            if (ag.BonificacionesOtorgadas.Count > 0)
                            {
                                string horas = ag.BonificacionesOtorgadas.Last().HorasOtorgadas;
                                tb_horasABonificar.Text = horas;
                            }
                            else
                            {
                                tb_horasABonificar.Text = "005:00";
                            }
                        }
                    }
                }
            } 
        }

        protected void btn_AceptarBonificacion_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(AgenteId.Value);
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == id);

            BonificacionOtorgada bo = agCxt.BonificacionesOtorgadas.FirstOrDefault(b => b.Mes == DateTime.Today.Month);

            if (bo!=null)
            {
                MessageBox.Show(this.Page, "El agente ya posee bonificación otorgada este mes, espere al mes siguiente o consulte con Sistemas.", MessageBox.Tipo_MessageBox.Warning);
            }
            else
            {
                if (Verificar())
                {
                    agCxt.PoseeBonificacion = true;
                    agCxt.HorasBonificacionACubrir = "-" + tb_horasABonificar.Text.Replace("-", "");
                    agCxt.BonificacionesOtorgadas.Add(new BonificacionOtorgada() { Anio = DateTime.Today.Year, Mes = DateTime.Today.Month, HorasOtorgadas= tb_horasABonificar.Text, HorasAdeudadas = tb_horasABonificar.Text });
                    cxt.SaveChanges();
                    OtorgoBonificacion();
                }
            }
        }

        private bool Verificar()
        {
            string[] HoraMinuto = tb_horasABonificar.Text.Split(':');
            int hora, minuto = 0;
            return (Int32.TryParse(HoraMinuto[0], out hora) && hora > 0) || (Int32.TryParse(HoraMinuto[1], out minuto) && minuto > 0);
        }

        
    }
}