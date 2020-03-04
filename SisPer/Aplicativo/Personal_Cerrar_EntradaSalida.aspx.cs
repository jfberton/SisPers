using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Cerrar_EntradaSalida : System.Web.UI.Page
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
                    MenuPersonalJefe1.Visible = (ag.Jefe || ag.JefeTemporal);
                    MenuPersonalAgente1.Visible = !(ag.Jefe || ag.JefeTemporal);
                    tb_dia.Value = DateTime.Today.ToShortDateString();
                    CargarJefesQueMarcaronEntradasSalidasDeLaJornadaLaboralDeSuGenteEnLaFechaSeleccionada();
                }
            }
        }

        private struct ItemGrillaPresentacion
        {
            public int IdJefe { get; set; }
            public string Jefe { get; set; }
            public string Area { get; set; }
            public string PathImagenCerrado { get; set; }
        }

        /// <summary>
        /// No hace falta agregar mas.
        /// </summary>
        private void CargarJefesQueMarcaronEntradasSalidasDeLaJornadaLaboralDeSuGenteEnLaFechaSeleccionada()
        {
            Model1Container cxt = new Model1Container();
            DateTime fecha = Convert.ToDateTime(tb_dia.Value);
            gv_presentaciones.DataSource = null;
            gv_presentaciones.DataBind();
            gv_marcaciones.DataSource = null;
            gv_marcaciones.DataBind();
            p_marcaciones.Visible = false;
            btn_Buscar.Visible = false;
            btn_nuevaBusqueda.Visible = true;
            tb_dia.Disabled = true;
            lbl_fecha_seleccionada.Text = Convert.ToDateTime(tb_dia.Value).ToLongDateString();

            var jefes = (from item in cxt.EntradasSalidas
                         where item.Fecha == fecha && item.Enviado == true
                         select new { IdJefe = item.AgenteId1, Jefe = item.MarcoComoJefe.ApellidoYNombre, Area = item.MarcoComoJefe.Area.Nombre }).Distinct();
            
            if (jefes.Count() > 0)
            {
                List<ItemGrillaPresentacion> itemsGrilla = new List<ItemGrillaPresentacion>();
                
                foreach (var item in jefes)
                {
                    ItemGrillaPresentacion itemGrilla = new ItemGrillaPresentacion();
                    itemGrilla.Area = item.Area;
                    itemGrilla.Jefe = item.Jefe;
                    itemGrilla.IdJefe = item.IdJefe;
                    itemGrilla.PathImagenCerrado = "~/Imagenes/invisible.png";

                    var marcaciones = from mar in cxt.EntradasSalidas
                                      where mar.Fecha == fecha && mar.MarcoComoJefe.Id == item.IdJefe
                                      select mar;
                    
                    foreach (EntradaSalida es in marcaciones)
                    {
                        if (es.CerradoPersonal)
                        {
                            itemGrilla.PathImagenCerrado = "~/Imagenes/accept.png";
                            break;
                        }
                    }

                    itemsGrilla.Add(itemGrilla);
                }

                gv_presentaciones.DataSource = itemsGrilla;
                gv_presentaciones.DataBind();
            }

            var areasPorEnviar = cxt.Areas.Where(aa => aa.Interior == true).ToList();
            List<Area> areasSinEnviar = new List<Area>();
            foreach (Area area in areasPorEnviar)
            {
                if (jefes.FirstOrDefault(jj => jj.Area == area.Nombre) == null)
                {
                    areasSinEnviar.Add(area);
                }
            }

            lbl_cantidad_areas_sin_enviar.Text = areasSinEnviar.Count.ToString();

            if (areasSinEnviar.Count > 0)
            {
                gv_sin_enviar.DataSource = (from aa in areasSinEnviar
                                             select new
                                             {
                                                 Area = aa.Nombre,
                                                 Jefe = aa.Agentes.FirstOrDefault(ag => ag.Jefe == true) != null ? aa.Agentes.FirstOrDefault(ag => ag.Jefe == true).ApellidoYNombre : "SIN JEFE!"
                                             }).ToList();
                gv_sin_enviar.DataBind();
            }
            else
            {
                gv_sin_enviar.DataSource = null;
                gv_sin_enviar.DataBind();
            }
           
        }

        protected void gv_marcaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_marcaciones.PageIndex = e.NewPageIndex;
            using (var cxt = new Model1Container())
            {
                int legajo = Convert.ToInt32(lbl_legajo.Text);
                int idjefe = cxt.Agentes.First(a => a.Legajo == legajo).Id;
                CargarMarcacionesDelJefeSeleccionadoEnElDiaSeleccionado(idjefe);
            }
        }

        protected void gv_presentaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_presentaciones.PageIndex = e.NewPageIndex;
            CargarJefesQueMarcaronEntradasSalidasDeLaJornadaLaboralDeSuGenteEnLaFechaSeleccionada();
        }

        private void CargarMarcacionesDelJefeSeleccionadoEnElDiaSeleccionado(int idjefe)
        {
            Model1Container cxt = new Model1Container();
            DateTime fecha = Convert.ToDateTime(tb_dia.Value);

            Agente jefe = cxt.Agentes.First(a=>a.Id == idjefe);

            lbl_legajo.Text = jefe.Legajo.ToString();
            lbl_Jefe.Text = jefe.ApellidoYNombre;

            var marcaciones = from item in cxt.EntradasSalidas
                              where item.Fecha == fecha && item.MarcoComoJefe.Id == idjefe
                              select item;

            var marcacionesGrilla = (from item in marcaciones
                               where item.Fecha == fecha && item.MarcoComoJefe.Id == idjefe
                               select new
                               {
                                   Id = item.Agente.Id,
                                   Legajo = item.Agente.Legajo,
                                   Nombre = item.Agente.ApellidoYNombre,
                                   Hentrada = item.Entrada,
                                   HSalida = item.Salida
                               }).ToList();

            gv_marcaciones.DataSource = marcacionesGrilla.OrderBy(s => s.Nombre).ToList();
            gv_marcaciones.DataBind();

            btn_Cerrar.Enabled = true;

            //si esta cerrado el dia inhabilito el boton.
            foreach (EntradaSalida es in marcaciones)
            {
                if (es.CerradoPersonal)
                {
                    btn_Cerrar.Enabled = false;
                    break;
                }
            }

            p_marcaciones.Visible = true;
        }

        protected void btn_Cerrar_Click(object sender, EventArgs e)
        {
            Model1Container cxt = new Model1Container();
            DateTime fecha = Convert.ToDateTime(tb_dia.Value);
            int legajo = Convert.ToInt32(lbl_legajo.Text);
            int idjefe = cxt.Agentes.First(a => a.Legajo == legajo && a.FechaBaja == null).Id;

            var marcaciones = from item in cxt.EntradasSalidas
                              where item.Fecha == fecha && item.MarcoComoJefe.Id == idjefe
                              select item;

            foreach (EntradaSalida es in marcaciones)
            {
                if (es.CerradoPersonal != true)
                {
                    es.CerradoPersonal = true;

                    ResumenDiario rd = es.Agente.ObtenerResumenDiario(es.Fecha);
                    if (rd != null)
                    {
                        Model1Container cxt1 = new Model1Container();
                        ResumenDiario rdCxt = cxt1.ResumenesDiarios.First(r => r.Id == rd.Id);
                    
                        rdCxt.HEntrada = es.Entrada;
                        rdCxt.HSalida = es.Salida;
                        rdCxt.Marcaciones.Add(new Marcacion() { Manual = true, Hora = es.Entrada, Anulada = false });
                        rdCxt.Marcaciones.Add(new Marcacion() { Manual = true, Hora = es.Salida, Anulada = false });

                        cxt1.SaveChanges();
                    }
                    else
                    {
                        rd = new ResumenDiario();

                        rd.AgenteId = es.AgenteId;
                        rd.Dia = es.Fecha;
                        rd.HEntrada = es.Entrada;
                        rd.HSalida = es.Salida;
                        rd.HVEnt = "000:00";
                        rd.HVSal = "000:00";
                        rd.Horas = "000:00";
                        rd.Inconsistente = false;
                        rd.MarcoTardanza = false;
                        rd.MarcoProlongJornada = false;
                        rd.ObservacionInconsistente = "";

                        rd.Marcaciones.Add(new Marcacion() { Manual = true, Hora = es.Entrada, Anulada = false });
                        rd.Marcaciones.Add(new Marcacion() { Manual = true, Hora = es.Salida, Anulada = false });

                        Model1Container cxt1 = new Model1Container();

                        cxt1.ResumenesDiarios.AddObject(rd);
                        cxt1.SaveChanges();
                    }
                }
            }

            cxt.SaveChanges();

            CargarJefesQueMarcaronEntradasSalidasDeLaJornadaLaboralDeSuGenteEnLaFechaSeleccionada();

            Controles.MessageBox.Show(this, "Las entradas salidas de " + cxt.Agentes.FirstOrDefault(ag => ag.Id == idjefe).Area.Nombre + " fueron cerradas correctamente.", Controles.MessageBox.Tipo_MessageBox.Success);

            p_marcaciones.Visible = false;
        }

        protected void btn_Ver_Click(object sender, ImageClickEventArgs e)
        {
            int idJefe = Convert.ToInt32(((ImageButton)sender).CommandArgument);

            CargarMarcacionesDelJefeSeleccionadoEnElDiaSeleccionado(idJefe);
        }

        protected void cv_VerificarFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime d;
            args.IsValid = DateTime.TryParse(tb_dia.Value, out d);
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            Validate();

            if (Page.IsValid)
            {
                CargarJefesQueMarcaronEntradasSalidasDeLaJornadaLaboralDeSuGenteEnLaFechaSeleccionada();
                
            }
        }

        protected void btn_nuevaBusqueda_Click(object sender, EventArgs e)
        {
            btn_Buscar.Visible = true;
            btn_nuevaBusqueda.Visible = false;
            tb_dia.Disabled = false;
            gv_presentaciones.DataSource = null;
            gv_presentaciones.DataBind();
            p_marcaciones.Visible = false;
        }

      

    }
}