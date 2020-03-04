using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_ReasignarAgentes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                MenuPersonalJefe1.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                MenuPersonalAgente1.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                Ddl_AreasOrigen.AreaSeleccionado = null;
                Ddl_AreasDestino.AreaSeleccionado = null;
                Session["Filtro"] = string.Empty;
                CargarGrillaOrigen();
                CargarGrillaDestino();
            }
        }

        private void CargarGrillaDestino()
        {
            Model1Container cxt = new Model1Container();
            int areaDestino = Ddl_AreasDestino.AreaSeleccionado != null ? Ddl_AreasDestino.AreaSeleccionado.Id : 0;
            var items = from a in cxt.Agentes
                        where (a.AreaId == areaDestino || (a.AreaId == null && areaDestino == 0)) && a.FechaBaja==null
                        select new
                        {
                            Id = a.Id,
                            Nombre = a.ApellidoYNombre,
                            Legajo = a.Legajo
                        };
            GridViewDestino.DataSource = items;
            GridViewDestino.DataBind();
        }

        protected void OtroItemOrigen(object sender, EventArgs e)
        {
            CargarGrillaOrigen();
        }

        protected void OtroItemDestino(object sender, EventArgs e)
        {
            CargarGrillaDestino();
        }

        protected void gridViewOrigen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOrigen.PageIndex = e.NewPageIndex;
            CargarGrillaOrigen();
        }

        protected void gridViewDestino_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDestino.PageIndex = e.NewPageIndex;
            CargarGrillaDestino();
        }

        private void CargarGrillaOrigen()
        {
            string filtro = Session["Filtro"] != null ? Session["Filtro"].ToString() : string.Empty;
            Model1Container cxt = new Model1Container();

            if (filtro == string.Empty)
            {
                int areaOrigen = Ddl_AreasOrigen.AreaSeleccionado != null ? Ddl_AreasOrigen.AreaSeleccionado.Id : 0;
                var items = (from a in cxt.Agentes
                             where (a.AreaId == areaOrigen || (a.AreaId == null && areaOrigen == 0)) && a.FechaBaja == null
                             select new
                             {
                                 Id = a.Id,
                                 Nombre = a.ApellidoYNombre,
                                 Legajo = a.Legajo,
                                 Area = a.Area.Nombre
                             }).ToList();

                GridViewOrigen.DataSource = items;
                GridViewOrigen.DataBind();

                GridViewOrigen.Columns[ObtenerColumna("Area")].Visible = false;

            }
            else
            {
                var items = (from a in cxt.Agentes
                             where a.FechaBaja == null
                             select new
                             {
                                 Id = a.Id,
                                 Nombre = a.ApellidoYNombre,
                                 Legajo = a.Legajo,
                                 Area = a.Area != null ? a.Area.Nombre : "Sin asignar"
                             }).ToList();

                var itemsFiltrados = (from i in items
                                      where
                                        Cadena.Normalizar(i.Nombre.ToUpper()).Contains(filtro) ||
                                        Cadena.Normalizar(i.Legajo.ToString().ToUpper()).Contains(filtro)
                                      select i).ToList();

                GridViewOrigen.DataSource = itemsFiltrados;
                GridViewOrigen.DataBind();

                GridViewOrigen.Columns[ObtenerColumna("Area")].Visible = true;
            }
        }

        /// <summary>
        /// Header
        /// </summary>
        /// <param name="p">Header</param>
        /// <returns></returns>
        private int ObtenerColumna(string p)
        {
            int id = 0;
            foreach (DataControlField item in GridViewOrigen.Columns)
            {
                if (item.HeaderText == p)
                {
                    id = GridViewOrigen.Columns.IndexOf(item);
                    break;
                }
            }

            return id;
        }

        protected void btn_Reasignar_Click(object sender, ImageClickEventArgs e)
        {
            Page.Validate("OrigenDestino");

            if (Page.IsValid)
            {
                Model1Container cxt = new Model1Container();
                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                Agente ag = cxt.Agentes.First(a => a.Id == id);

                //Si tenia una reasignacion anterior, la termino.
                Reasignacion re = ag.Reasignaciones.FirstOrDefault(r => r.Hasta == null);
                if (ag.AreaId != null && ag.AreaId != Ddl_AreasDestino.AreaSeleccionado.Id)
                {
                    if (re != null)
                    {
                        re.Hasta = DateTime.Today;
                    }

                    if (Ddl_AreasDestino.AreaSeleccionado == null)
                    {
                        ag.AreaId = null;
                    }
                    else
                    {
                        ag.AreaId = Ddl_AreasDestino.AreaSeleccionado.Id;
                        //Si el destino es distinto de null, agrego una reasignación nueva 
                        Reasignacion nueva = new Reasignacion();
                        nueva.Desde = DateTime.Today;
                        nueva.AgenteId = ag.Id;
                        nueva.AreaId = Ddl_AreasDestino.AreaSeleccionado.Id;

                        cxt.Reasignaciones.AddObject(nueva);
                        //ListadoAgentesParaGrilla.ActualizarPropiedad(ag.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Area, Ddl_AreasDestino.AreaSeleccionado.Nombre);
                    }

                    cxt.SaveChanges();
                }
                else
                {
                    if (ag.AreaId == null)
                    {
                        if (Ddl_AreasDestino.AreaSeleccionado == null)
                        {
                            ag.AreaId = null;
                        }
                        else
                        {
                            ag.AreaId = Ddl_AreasDestino.AreaSeleccionado.Id;
                            //Si el destino es distinto de null, agrego una reasignación nueva 
                            Reasignacion nueva = new Reasignacion();
                            nueva.Desde = DateTime.Today;
                            nueva.AgenteId = ag.Id;
                            nueva.AreaId = Ddl_AreasDestino.AreaSeleccionado.Id;

                            cxt.Reasignaciones.AddObject(nueva);
                            //ListadoAgentesParaGrilla.ActualizarPropiedad(ag.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Area, Ddl_AreasDestino.AreaSeleccionado.Nombre);
                        }

                        cxt.SaveChanges();
                    }
                }

                

                CargarGrillaOrigen();
                CargarGrillaDestino();
            }
        }

        protected void AreasOrigenYDestinoIguales_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Ddl_AreasOrigen.AreaSeleccionado != Ddl_AreasDestino.AreaSeleccionado;
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            Session["Filtro"] = Cadena.Normalizar(tb_Busqueda.Text.ToUpper());
            CargarGrillaOrigen();
        }
    }
}