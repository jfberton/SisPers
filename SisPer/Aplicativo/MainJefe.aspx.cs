using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Controles;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class MainJefe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente agSession = Session["UsuarioLogueado"] as Agente;

                if (agSession == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                int id = 0;
                string idUsr = Request.QueryString["Usr"];
                if (idUsr != null && idUsr != string.Empty)
                {
                    id = Convert.ToInt32(idUsr);
                }
                else
                {
                    id = (Session["UsuarioLogueado"] as Agente).Id;
                }


                Model1Container cxt = new Model1Container();
                Session["CXT"] = cxt;
                Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);

                Session["Agente"] = ag;


                if (
                    (Session["UsuarioLogueado"] as Agente).Perfil != PerfilUsuario.Personal &&
                    !(Session["UsuarioLogueado"] as Agente).Jefe && !(Session["UsuarioLogueado"] as Agente).JefeTemporal)
                {
                    Response.Redirect("../default.aspx?mode=trucho");
                }
                else
                {
                    MenuPersonalJefe1.Visible = ((Session["UsuarioLogueado"] as Agente).Perfil == PerfilUsuario.Personal);
                    MenuJefe1.Visible = !((Session["UsuarioLogueado"] as Agente).Perfil == PerfilUsuario.Personal);
                    DatosAgente1.Agente = ag;
                    Cargar_Grilla();
                    CargarHVS();
                    CargarFrancos();
                    CargarSolicitudes();
                    CargarHVPorCerrar();
                }

                bool mostrarMensaje = Convert.ToBoolean(Session["MostrarMensageBienvenida"]);
                Session["MostrarMensageBienvenida"] = false;
                if (mostrarMensaje)
                {
                    MensageBienvenida.Show();
                }
            }
        }

        #region grilla agentes

        private void Cargar_Grilla()
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                var items = cxt.sp_obtener_agentes_cascada(ag.Id, false).OrderBy(x => x.nivel_para_ordenar).OrderBy(x => x.nombre_agente).ToList();

                GridView1.DataSource = items;
                GridView1.DataBind();
            }
        }

        protected void btn_Administrar_Click(object sender, ImageClickEventArgs e)
        {
            int id_agente = 0;
            if (int.TryParse(((ImageButton)sender).CommandArgument, out id_agente))
            {
                administrar_datos_agente(id_agente);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (e.Row.Cells[1].Text == "Jefe")
                {
                    e.Row.ForeColor = Color.Black;
                    e.Row.Font.Bold = true;
                }

                if (e.Row.Cells[1].Text == "Jefe temporal")
                {
                    e.Row.ForeColor = Color.Blue;
                }

                if (e.Row.Cells[5].Text == "Sin datos")
                {
                    e.Row.ForeColor = Color.DarkRed;
                    e.Row.BackColor = Color.MistyRose;
                    e.Row.Cells[5].ToolTip = "Posiblemente ausente";
                }
            }
        }

        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        private string ObtenerDireccionImagenAgente(Agente u)
        {
            string pathImagen = string.Empty;

            if (Directory.Exists(pathImagenesDisco + u.Legajo))
            {
                if (File.Exists(pathImagenesDisco + u.Legajo + "\\Original.jpg"))
                {//Si no existe la imagen temporal pero si la original, la cargo
                    pathImagen = pathImagenesDisco + u.Legajo + "\\Original.jpg";
                }
                else
                {//Si no existe la imagen temporal y tampoco la original, cargo la default
                    pathImagen = pathImagenesDisco + "UsrDefault.jpg";
                }
            }
            else
            {//Si no existe la carpeta del usuario directamente cargo la imagen de default
                pathImagen = pathImagenesDisco + "UsrDefault.jpg";
            }

            return pathImagen;
        }

        private byte[] ImageToByte(string pathImage)
        {
            try
            {
                System.Drawing.Image imageIn = System.Drawing.Image.FromFile(pathImage);
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            catch
            {
                throw new Exception("CHANNNNNN");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        #endregion

        #region admin agente

        private void administrar_datos_agente(int id_agente)
        {
            using (var cxt = new Model1Container())
            {
                Agente agente = cxt.Agentes.FirstOrDefault(aa => aa.Id == id_agente);

                if (agente != null)
                {
                    lbl_modal_agente_Id.Text = agente.Id.ToString();
                    lbl_modal_titulo.Text = agente.ApellidoYNombre;
                    lbl_Legajo.Text = agente.Legajo.ToString();
                    lbl_Email.Text = agente.Legajo_datos_laborales.Email;
                    lbl_DNI.Text = agente.Legajo_datos_personales.DNI;
                    lbl_FechIngreso.Text = agente.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                    lbl_FechNac.Text = agente.Legajo_datos_personales.FechaNacimiento.ToShortDateString();
                    lbl_estado.Text = cxt.Estado_agente(agente.Id).First();
                    ImagenAgente1.Agente = agente;

                    if (!agente.Jefe && !agente.JefeTemporal)
                    {
                        btn_pantalla_jefe.Disabled = true;
                    }
                    else
                    {
                        btn_pantalla_jefe.Disabled = false;
                    }

                    switch (lbl_estado.Text)
                    {
                        case "Sin datos":
                            modal_title.Attributes["class"] = "modal-content panel-danger";
                            break;
                        case "NATALICIO":
                            modal_title.Attributes["class"] = "modal-content panel-success";
                            break;
                        case "Activo":
                            modal_title.Attributes["class"] = "modal-content panel-success";
                            break;
                        case "Salida Particular":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Salida Oficial":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Indisposición":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        case "Franco compensatorio":
                            modal_title.Attributes["class"] = "modal-content panel-primary";
                            break;
                        case "Franco compensatorio p/aprobar":
                            modal_title.Attributes["class"] = "modal-content panel-warning";
                            break;
                        default:
                            modal_title.Attributes["class"] = "modal-content panel-default";
                            break;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "admin_agente", "<script language=\"javascript\"  type=\"text/javascript\">$(document).ready(function() { $('#modal_agente').modal('show')});</script>", false);
                }
            }

        }

        protected void btn_detalle_ServerClick(object sender, EventArgs e)
        {
            Session["IdAg"] = lbl_modal_agente_Id.Text;
            Response.Redirect("~/Aplicativo/Ag_Detalle.aspx");
        }

        protected void btn_pantalla_agente_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/MainAgente.aspx?Usr=" + lbl_modal_agente_Id.Text);
        }

        protected void btn_pantalla_jefe_ServerClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Model1Container cxt = new Model1Container();
            Agente agCxt = cxt.Agentes.First(a => a.Id == id);

            if (agCxt.Jefe || agCxt.JefeTemporal)
            {
                Response.Redirect("~/Aplicativo/MainJefe.aspx?Usr=" + lbl_modal_agente_Id.Text);
            }
            else
            {
                MessageBox.Show(this.Page, "El agente no tiene perfil de jefe!", MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_solicitar_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Aplicativo/Jefe_Ag_SolicitarEstado.aspx?Usr=" + lbl_modal_agente_Id.Text);
        }

        protected void btn_imprimir_carnet_ServerClick(object sender, EventArgs e)
        {
            Model1Container cxt = new Model1Container();
            int idAg = Convert.ToInt32(lbl_modal_agente_Id.Text);
            Agente u = cxt.Agentes.FirstOrDefault(a => a.Id == idAg);

            ///Obtengo los datos
            Carnet_AgenteDS ds = new Carnet_AgenteDS();
            Carnet_AgenteDS.AgenteRow dr = ds.Agente.NewAgenteRow();
            dr.DNI = u.Legajo_datos_personales.DNI;
            dr.Nombre = u.ApellidoYNombre.Split(',')[1].Trim();
            dr.Apellido = u.ApellidoYNombre.Split(',')[0].Trim().ToUpper();
            dr.Foto = ImageToByte(ObtenerDireccionImagenAgente(u));
            ds.Agente.Rows.Add(dr);


            ///Configuro el reporte
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/Carnet_Agente.rdlc");

            ReportDataSource general = new ReportDataSource("Agente", ds.Agente.Rows);

            viewer.LocalReport.DataSources.Add(general);

            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;
            string deviceInfo = null;
            byte[] bytes = null;

            deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

            //Render the report
            bytes = viewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Session["Bytes"] = bytes;

            string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);

        }

        #endregion


        private void CargarSolicitudes()
        {
            Model1Container cxt = new Model1Container();
            Agente ag = Session["Agente"] as Agente;
            DateTime primerDiaDelMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var solicitudes = (from se in cxt.SolicitudesDeEstado
                               where
                                    se.FechaDesde >= primerDiaDelMes &&
                                    //se.AgenteId1 == SolicitadoPor
                                    se.AgenteId1 == ag.Id
                               select new
                               {
                                   Id = se.Id,
                                   Agente = se.Agente.ApellidoYNombre,
                                   Tipo = se.TipoEstadoAgente.Estado,
                                   Desde = se.FechaDesde,
                                   Hasta = se.FechaHasta,
                                   Estado = se.Estado
                               }).ToList();

            gv_EstadosSolicitados.DataSource = solicitudes;
            gv_EstadosSolicitados.DataBind();
        }

        private void CargarHVS()
        {
            //Obtengo los agentes subordinados
            //List<Agente> agentesSubordinados = ObtenerAgentesSubordinados();

            Agente ag = Session["Agente"] as Agente;

            Model1Container cxt = new Model1Container();

            List<Agente> agentes = ag.ObtenerAgentesSubordinadosCascada(true);

            if (agentes.Count() > 0)
            {
                //cancelo los horarios vespertinos solicitados del departamento del jefe que estan vencidos
                ProcesosGlobales.CancelarSolicitudesHVPorVencimiento();

                //Creo un lista de horarios vespertinos por aprobar
                List<HorarioVespertino> horariosVespertinosSolicitados = new List<HorarioVespertino>();
                //Creo una lista de horarios aprobados por finalizar
                List<HorarioVespertino> horariosVespertinosAprobados = new List<HorarioVespertino>();

                //recorro los agentes del listado obtenido anteriormente y cargo por cada agente los horarios vespertinos pendientes de aprobación y los aprobados 
                //en la lista correspondiente.
                foreach (Agente agente in agentes)
                {
                    Agente agActualizado = cxt.Agentes.First(a => a.Id == agente.Id);

                    foreach (var hv in agActualizado.HorariosVespertinos)
                    {
                        if (hv.Estado == EstadosHorarioVespertino.Solicitado)
                        {
                            horariosVespertinosSolicitados.Add(hv);
                        }
                        if (hv.Estado == EstadosHorarioVespertino.Aprobado)
                        {
                            horariosVespertinosAprobados.Add(hv);
                        }
                    }
                }

                var hvsSolicitados = (from hv in horariosVespertinosSolicitados
                                      select new
                                      {
                                          Id = hv.Id,
                                          Agente = hv.Agente.ApellidoYNombre,
                                          Dia = hv.Dia,
                                          Desde = hv.HoraInicio,
                                          Hasta = hv.HoraFin,
                                          Motivo = hv.Motivo,
                                          Horas = HorasString.RestarHoras(hv.HoraFin, hv.HoraInicio)
                                      }).ToList();

                GridViewHVPendientesAprobar.DataSource = null;
                GridViewHVPendientesAprobar.DataBind();
                GridViewHVPendientesAprobar.DataSource = hvsSolicitados;
                GridViewHVPendientesAprobar.DataBind();
            }
            else
            {
                GridViewHVPendientesAprobar.DataSource = null;
                GridViewHVPendientesAprobar.DataBind();
            }


        }

        protected void btn_AprobarHV_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            Agente ag = Session["UsuarioLogueado"] as Agente;
            var cxt = new Model1Container();
            HorarioVespertino hv = cxt.HorariosVespertinos.First(hvesp => hvesp.Id == id);

            if (ag.Id != hv.AgenteId)
            {
                ModificarEstadoHV(id, EstadosHorarioVespertino.Aprobado);
                CargarHVS();

                if (ag.Area.Interior == true)
                {
                    CargarHVPorCerrar();
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Usted no se puede aprobar los horarios vespertinos generados a si mismo.", Controles.MessageBox.Tipo_MessageBox.Info);
            }
        }

        protected void btn_RechazarHV_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            ModificarEstadoHV(id, EstadosHorarioVespertino.Cancelado);
            CargarHVS();
        }

        private void ModificarEstadoHV(int id, EstadosHorarioVespertino estadosHorarioVespertino)
        {
            Model1Container cxt = new Model1Container();
            Agente ag = Session["UsuarioLogueado"] as Agente;
            HorarioVespertino hv = cxt.HorariosVespertinos.FirstOrDefault(hvs => hvs.Id == id);
            hv.Estado = estadosHorarioVespertino;
            hv.AgenteId1 = ag.Id;
            cxt.SaveChanges();
        }

        protected void gridViewHVPendientesAprobar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHVPendientesAprobar.PageIndex = e.NewPageIndex;
            CargarHVS();
        }

        protected void GridViewFrancosPendientesAprobar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFrancos.PageIndex = e.NewPageIndex;
            CargarFrancos();
        }

        protected void gv_EstadosSolicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_EstadosSolicitados.PageIndex = e.NewPageIndex;
            CargarSolicitudes();
        }

        private void CargarFrancos()
        {
            //Obtengo los agentes subordinados
            //List<Agente> agentesSubordinados = ObtenerAgentesSubordinados();

            Agente ag = Session["Agente"] as Agente;

            Model1Container cxt = new Model1Container();

            List<Agente> agentes = ag.ObtenerAgentesSubordinadosCascada(true);

            if (agentes.Count() > 0)
            {
                //cancelo los Francos que ve
                ProcesosGlobales.CancelarSolicitudesFrancosPorVencimiento();

                //Creo un lista de horarios vespertinos por aprobar
                List<Franco> listaFrancosSolicitados = new List<Franco>();

                //recorro los agentes del listado obtenido anteriormente y cargo por cada agente 
                //los francos pendientes de aprobación
                foreach (Agente agente in agentes)
                {
                    Agente agActualizado = cxt.Agentes.First(a => a.Id == agente.Id);

                    var francosSolicitados = from fr in agActualizado.Francos
                                             where fr.Estado == 0
                                             select fr;

                    foreach (Franco f in francosSolicitados)
                    {
                        listaFrancosSolicitados.Add(f);
                    }
                }

                var frsSolicitados = (from fc in listaFrancosSolicitados
                                      select new
                                      {
                                          Id = fc.Id,
                                          Agente = fc.Agente.ApellidoYNombre,
                                          Estado = fc.Estado,
                                          Dia = fc.FechaSolicitud,
                                          DiaInicial = (from d in fc.DiasFranco select d.Dia).Min(),
                                          CantidadDias = fc.DiasFranco.Count,
                                          Horas = fc.DiasFranco.Count * 7
                                      }).ToList();

                GridViewFrancos.DataSource = frsSolicitados;
                GridViewFrancos.DataBind();
            }
            else
            {
                GridViewFrancos.DataSource = null;
                GridViewFrancos.DataBind();
            }

        }

        protected void btn_RechazarFranco_Click(object sender, ImageClickEventArgs e)
        {
            if (MotivoRechazo.Value != "")
            {
                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                ProcesosGlobales.ModificarEstadoFranco(id, EstadosFrancos.Cancelado, Session["Agente"] as Agente, MotivoRechazo.Value);
                CargarFrancos();
            }
        }

        protected void btn_AprobarFranco_Click(object sender, ImageClickEventArgs e)
        {
            int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            ProcesosGlobales.ModificarEstadoFranco(id, EstadosFrancos.AprobadoJefe, Session["Agente"] as Agente);
            CargarFrancos();

        }

        protected void GridViewHVPorCerrar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewHVPorCerrar.PageIndex = e.NewPageIndex;
            CargarHVPorCerrar();
        }

        protected void btn_TerminarHV_Click(object sender, ImageClickEventArgs e)
        {
            Session["Id"] = ((ImageButton)sender).CommandArgument;

            Response.Redirect("~/Aplicativo/Personal_TerminarHV.aspx");
        }

        private void CargarHVPorCerrar()
        {
            Agente ag = Session["Agente"] as Agente;

            var ids = (from a in ag.ObtenerAgentesSubordinadosCascada(true)
                       select new { a.Id });

            List<int> listIds = new List<int>();

            foreach (var id in ids)
            {
                int identificador = 0;
                if (int.TryParse(id.Id.ToString(), out identificador))
                {
                    listIds.Add(Convert.ToInt32(identificador));
                }
            }

            using (var cxt = new Model1Container())
            {
                List<HorarioVespertino> horariosVespertinosAprobados = new List<HorarioVespertino>();
                
                //TODO: verificar como hacer esta consulta asi traigo unicamente los HV pendientes de cerrar para los jefes de los agentes que tenian autorizado remoto
                //BUG: aca tira error!
                foreach (var hv in cxt.HorariosVespertinos.Include("Agente").Where(hvs => hvs.Estado == EstadosHorarioVespertino.Aprobado))
                {
                    if (hv.Agente.DiasAutorizadoRemoto.FirstOrDefault(dar => dar.Dia == hv.Dia) != null)
                    {
                        horariosVespertinosAprobados.Add(hv);
                    }
                }

                var hvsPorCerrar = (from hv in horariosVespertinosAprobados
                                    where listIds.Contains(hv.Agente.Id)
                                    select new
                                    {
                                        Id = hv.Id,
                                        Agente = hv.Agente.ApellidoYNombre,
                                        Dia = hv.Dia,
                                        Desde = hv.HoraInicio,
                                        Hasta = hv.HoraFin,
                                        Motivo = hv.Motivo,
                                        Horas = HorasString.RestarHoras(hv.HoraFin, hv.HoraInicio)
                                    }).ToList();

                GridViewHVPorCerrar.DataSource = hvsPorCerrar;
                GridViewHVPorCerrar.DataBind();
            }
        }

    }
}