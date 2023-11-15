using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Informe_ajustes_manuales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Agente ag = Session["UsuarioLogueado"] as Agente;

                if (ag == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                if (ag.Perfil != PerfilUsuario.Personal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente.Visible = !(ag.Jefe || ag.JefeTemporal);

                    CargarValores();
                }
            }
        }

        private void CargarValores()
        {
            ddl_Mes.SelectedIndex = DateTime.Today.Month - 1;
            CargarDDLAnio();
        }

        private void CargarDDLAnio()
        {
            for (int i = 2022; i <= DateTime.Now.Year; i++)
            {
                ddl_Anio.Items.Add(i.ToString());
            }

            ddl_Anio.SelectedIndex = ddl_Anio.Items.Count - 1;
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarResultadoBusqueda();
        }

        private Reportes.Ajustes_manuales_horas_DS Buscar()
        {
            using (var cxt = new Model1Container())
            {
                Reportes.Ajustes_manuales_horas_DS ds = new Reportes.Ajustes_manuales_horas_DS();

                int mes = ddl_Mes.SelectedIndex + 1;
                int año = Convert.ToInt32(ddl_Anio.Text);

                var datos = cxt.MovimientosHoras.Where(mmhh =>
                                                            (mmhh.TipoMovimientoHoraId == 6 || mmhh.TipoMovimientoHoraId == 7)
                                                            && mmhh.ResumenDiario.Dia.Month == mes
                                                            && mmhh.ResumenDiario.Dia.Year == año
                                                            && mmhh.ResumenDiario.Cerrado == true
                                                          ).Select(mh => new {
                                                                                AgentePersonal = mh.AgendadoPor.ApellidoYNombre
                                                                                , Legajo_agente_personal = mh.AgendadoPor.Legajo
                                                                                , Agente = mh.ResumenDiario.Agente.ApellidoYNombre
                                                                                , Legajo_agente = mh.ResumenDiario.Agente.Legajo
                                                                                , Tipo = mh.Tipo.Tipo
                                                                                , Fecha = mh.ResumenDiario.Dia
                                                                                , Horas = mh.Horas
                                                                                , Motivo = mh.Descripcion

                                                          }).ToList();

                var agentesPersonal = datos.Select(d => new { Legajo = d.Legajo_agente_personal, ApellidoyNombre = d.AgentePersonal }).Distinct().ToList();

                foreach(var agente_personal in agentesPersonal)
                {
                    Reportes.Ajustes_manuales_horas_DS.GeneralRow gr = ds.General.NewGeneralRow();
                    gr.AgentePersonal = agente_personal.ApellidoyNombre;
                    gr.Legajo = agente_personal.Legajo.ToString();
                    gr.Mes = mes.ToString();
                    gr.Año = año.ToString();

                    switch (mes)
                    {
                        case 1:
                            gr.Mes = "Enero";
                            break;
                        case 2:
                            gr.Mes = "Febrero";
                            break;
                        case 3:
                            gr.Mes = "Marzo";
                            break;
                        case 4:
                            gr.Mes = "Abril";
                            break;
                        case 5:
                            gr.Mes = "Mayo";
                            break;
                        case 6:
                            gr.Mes = "Junio";
                            break;
                        case 7:
                            gr.Mes = "Julio";
                            break;
                        case 8:
                            gr.Mes = "Agosto";
                            break;
                        case 9:
                            gr.Mes = "Septiembre";
                            break;
                        case 10:
                            gr.Mes = "Octubre";
                            break;
                        case 11:
                            gr.Mes = "Noviembre";
                            break;
                        case 12:
                            gr.Mes = "Diciembre";
                            break;
                    }

                    ds.General.Rows.Add(gr);
                    
                    var ajustes_agente = datos.Where(d => d.AgentePersonal == agente_personal.ApellidoyNombre).ToList();
                    foreach (var ajuste_agente in ajustes_agente)
                    { 
                        Reportes.Ajustes_manuales_horas_DS.DetalleRow dr = ds.Detalle.NewDetalleRow();

                        dr.AgentePersonal = ajuste_agente.AgentePersonal;
                        dr.Agente = ajuste_agente.Agente;
                        dr.Legajo = ajuste_agente.Legajo_agente.ToString();
                        dr.Tipo = ajuste_agente.Tipo;
                        dr.Fecha = ajuste_agente.Fecha.ToShortDateString();
                        dr.Horas = ajuste_agente.Horas;
                        dr.Motivo = ajuste_agente.Motivo;
                        ds.Detalle.Rows.Add(dr);
                    }
                }

                return ds;
            }
        }

        private void CargarResultadoBusqueda()
        {
            Reportes.Ajustes_manuales_horas_DS ds = Buscar();

            ///Configuro el reporte
            byte[] bytes = null;

            if (ds.General.Count > 0)
            {
                Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
                Datos_informe_desde_hasta<Ajustes_manuales_horas_DS> data = new Datos_informe_desde_hasta<Ajustes_manuales_horas_DS>();
                data.datos = ds;
                data.desde = DateTime.Now;
                data.hasta = DateTime.Now;

                Informe_ajustas_horas_manuales reporte = new Informe_ajustas_horas_manuales(data, usuarioLogueado);
                bytes = reporte.Generar_informe();
                RegistrarImpresionReporte();
            }

            if (bytes != null)
            {
                Session["Bytes"] = bytes;

                string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
            }
            else
            {
                Controles.MessageBox.Show(this, "La búsqueda realizada no arrojó resultados", Controles.MessageBox.Tipo_MessageBox.Info);
            }

        }

        private void RegistrarImpresionReporte()
        {
            Agente usuarioLogueado = Session["UsuarioLogueado"] as Agente;
            string localIP = Request.UserHostAddress;
            string nombreMaquina = Request.UserHostName;

            ProcesosGlobales.RegistrarImpresion(usuarioLogueado, "INFORME BONIFICACIONES", DateTime.Now, nombreMaquina, localIP);
        }

       
       
    }
}