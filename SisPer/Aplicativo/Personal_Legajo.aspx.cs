using Microsoft.Reporting.WebForms;
using SisPer.Aplicativo.Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisPer.Aplicativo
{
    public partial class Personal_Legajo : System.Web.UI.Page
    {

        private string pathArchivosDisco = System.Web.HttpRuntime.AppDomainAppPath + "Archivos\\";
        private string pathImagenesDisco = System.Web.HttpRuntime.AppDomainAppPath + "Imagenes\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Agente usuariologueado = Session["UsuarioLogueado"] as Agente;

                if (usuariologueado == null)
                {
                    Response.Redirect("~/Default.aspx?mode=session_end");
                }

                var modoVisualizacion = Session["VerLegajo"] != null;
                if (modoVisualizacion)
                {
                    Session["VerLegajo"] = null;

                    if (usuariologueado.Perfil == PerfilUsuario.Personal)
                    {
                        MenuPersonalAgente.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
                        MenuPersonalJefe.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    }
                    else
                    {
                        MenuAgente.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);
                        MenuJefe.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                    }

                    Session["Agente"] = usuariologueado;
                    OcultarBotonesEditar();
                    CargarDatosAgente();
                }
                else
                {
                    if (usuariologueado.Perfil != PerfilUsuario.Personal)
                    {
                        Response.Redirect("../default.aspx?mode=trucho");
                    }
                    else
                    {
                        MenuPersonalJefe.Visible = (usuariologueado.Jefe || usuariologueado.JefeTemporal);
                        MenuPersonalAgente.Visible = !(usuariologueado.Jefe || usuariologueado.JefeTemporal);

                        Model1Container cxt = new Model1Container();
                        if (Session["IdAg"] != null)
                        {
                            int id = Convert.ToInt32(Session["IdAg"]);
                            Agente ag = cxt.Agentes.FirstOrDefault(a => a.Id == id);
                            Session["IdAg"] = null;
                            Session["Agente"] = ag;
                            CargarDatosAgente();
                            //OcultarBotonesEditar();
                        }
                        else
                        {
                            Session["Agente"] = null;
                            OcultarBotonesEditar();
                            Controles.MessageBox.Show(this, "Error al obtener los datos del agente", Controles.MessageBox.Tipo_MessageBox.Danger, "Atencion", "../Aplicativo/MainPersonal.aspx");
                        }
                    }
                }
            }
        }

        struct ItemListArea
        {
            private int id;
            public int Id
            {
                set { id = value; }
                get { return id; }
            }

            private string valor;
            public string Valor
            {
                set { valor = value; }
                get { return valor; }
            }
            /// <summary>
            /// Instancia un nuevo itemlist
            /// </summary>
            /// <param name="p">Id</param>
            /// <param name="q">Valor</param>
            public ItemListArea(int p, string q)
            {
                id = p;
                valor = q;
            }
        }

        private void CargarDatosAgente()
        {
            Agente agSession = Session["Agente"] as Agente;

            if (agSession != null)
            {
                var cxt = new Model1Container();
                Agente ag = cxt.Agentes.First(aa => aa.Id == agSession.Id);

                #region Cargo los datos del dropdownlist de las areas

                List<ItemListArea> lista = new List<ItemListArea>();
                ItemListArea item = new ItemListArea(0, "Seleccione:");
                lista.Add(item);
                var items = (from pp in cxt.Areas
                             select new { Id = pp.Id, Valor = pp.Nombre }).OrderBy(a => a.Valor);

                foreach (var i in items)
                {
                    lista.Add(new ItemListArea(i.Id, i.Valor));
                }

                ddl_areas.DataValueField = "Id";
                ddl_areas.DataTextField = "Valor";
                ddl_areas.DataSource = lista;
                ddl_areas.DataBind();

                #endregion

                #region Encabezado

                ImagenAgente.Agente = ag;
                lbl_nombre_agente.Text = tb_nombre_agente.Value = ag.ApellidoYNombre;

                lbl_estado_civil.Text = ag.Legajo_datos_personales.EstadoCivil;
                lbl_sexo.Text = ag.Legajo_datos_personales.Sexo;
                lbl_dni.Text = ag.Legajo_datos_personales.DNI; // tb_dni.Value
                lbl_legajo.Text = tb_legajo.Value = ag.Legajo.ToString();
                lbl_cuit.Text = tb_cuit.Value = ag.Legajo_datos_laborales.CUIT;
                lbl_situacion_de_revista.Text = ag.Legajo_datos_laborales.Situacion_de_revista;

                if (ag.Legajo_datos_laborales.Cargo != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Cargo: " + ag.Legajo_datos_laborales.Cargo;
                }
                if (ag.Legajo_datos_laborales.Grupo != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Grupo: " + ag.Legajo_datos_laborales.Grupo;
                }
                if (ag.Legajo_datos_laborales.Apartado != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Apartado: " + ag.Legajo_datos_laborales.Apartado;
                }

                #endregion

                #region Datos laborales
                TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                LicenciaAgente licAgAnioAct = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                LicenciaAgente licAgAnioanterior = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                lbl_ingreso_admin_publica.Text = tb_ingreso_admin_publica.Value = ag.Legajo_datos_laborales.FechaIngresoAminPub.ToShortDateString();
                lbl_ingreso_atp.Text = tb_ingreso_atp.Value = ag.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                lbl_anios_en_otras_partes.Text = ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes.ToString() + " años y " + ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes.ToString() + " meses";
                tb_anios_en_otras_partes.Value = ag.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes.ToString();
                tb_meses_en_otras_partes.Value = ag.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes.ToString();
                lbl_ficha_medica.Text = tb_ficha_medica.Value = ag.Legajo_datos_laborales.FichaMedica;
                lbl_licencia_anio_en_curso.Text = tb_licencia_anio_en_curso.Value = licAgAnioAct != null ? licAgAnioAct.DiasOtorgados.ToString() : " - ";
                lbl_licencia_anio_anterior.Text = tb_licencia_anio_anterior.Value = licAgAnioanterior != null ? licAgAnioanterior.DiasOtorgados.ToString() : " - ";
                lbl_mail.Text = ag.Legajo_datos_laborales.Email;

                lbl_area_agente.Text = ag.Area.Nombre;
                lbl_trabaja_como.Text = (!ag.Jefe && !ag.JefeTemporal) ? "Agente" : (!ag.Jefe) ? "Jefe temporal hasta " + ag.JefeTemporalHasta.Value.ToShortDateString() : "Jefe";
                lbl_hora_desde.Text = tb_horario_laboral_desde.Value = ag.HoraEntrada;
                lbl_hora_hasta.Text = tb_horario_laboral_hasta.Value = ag.HoraSalida;
                lbl_horario_flexible.Text = (ag.HorarioFlexible ?? false) == true ? "SI" : "NO";

                lbl_usuario.Text = tb_usuario.Value = ag.Usr;
                lbl_perfil_agente.Text = ag.Perfil.ToString();

                #endregion

                #region Datos Personales
                lbl_direccion.Text = tb_direccion.Value = ag.Legajo_datos_personales.Domicilio;
                lbl_localidad.Text = tb_localidad.Value = ag.Legajo_datos_personales.Domicilio_localidad;
                img_aclaraciones_domicilio.ToolTip = tb_aclaraciones_direccion.Value = ag.Legajo_datos_personales.DomicilioObservaciones;

                // ---->>> titulos
                var titulos = (from tt in ag.Legajo_datos_personales.Legajo_titulo_certificado
                               where tt.Tipo_certificado == "Titulo"
                               select new
                               {
                                   nivel = tt.Nivel,
                                   titulo = tt.Descripcion,
                                   id = tt.Id
                               }).ToList();
                gv_titulos.DataSource = titulos;
                gv_titulos.DataBind();

                // ---->>> certificados
                var certificados = (from cc in ag.Legajo_datos_personales.Legajo_titulo_certificado
                                    where cc.Tipo_certificado == "Certificado"
                                    select new
                                    {
                                        descripcion = cc.Descripcion,
                                        lugar = cc.Lugar_insticucion,
                                        fecha_emision = cc.Fecha_emision,
                                        duracion = cc.Duracion,
                                        id = cc.Id
                                    }).ToList();
                gv_certificados.DataSource = certificados;
                gv_certificados.DataBind();

                lbl_nombre_conyuge.Text = tb_nombre_conyuge.Value = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.Apellido_y_nombre : "";
                lbl_dni_conyuge.Text = tb_dni_conyuge.Value = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.DNI : "";
                lbl_fecha_nacimiento_conyuge.Text = tb_fecha_nacimiento_conyuge.Value = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.Fecha_de_nacimiento.ToShortDateString() : "";
                lbl_trabaja_conyuge.Text = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.Trabaja ? "SI" : "NO" : "";
                lbl_lugar_de_trabajo_conyuge.Text = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.Lugar_de_trabajo : "";
                if (ag.Legajo_datos_personales.Conyuge != null && ag.Legajo_datos_personales.Conyuge.Dependencia != string.Empty && ag.Legajo_datos_personales.Conyuge.Dependencia.Length > 0)
                {
                    lbl_lugar_de_trabajo_conyuge.Text = lbl_lugar_de_trabajo_conyuge.Text + " - Dependencia: " + ag.Legajo_datos_personales.Conyuge.Dependencia;
                }

                lbl_profesion_conyuge.Text = tb_profesion_conyuge.Value = (ag.Legajo_datos_personales.Conyuge != null) ? ag.Legajo_datos_personales.Conyuge.Profesion : "";

                // ---->>> hijos
                var hijos = (from hh in ag.Legajo_datos_personales.Hijos
                             select new
                             {
                                 nombre = hh.Apellido_y_nombre,
                                 dni = hh.DNI,
                                 nacimiento = hh.Fecha_de_nacimiento,
                                 edad = hh.Edad,
                                 id = hh.Id,
                                 observaciones = hh.Observaciones
                             }).ToList();
                gv_hijos.DataSource = hijos;
                gv_hijos.DataBind();

                #endregion

                #region Fojas de servicio
                // ---->>> cargar contratos de obra
                if (ag.Legajo_fojas_de_servicio != null)
                {
                    var carrera = (from cc in ag.Legajo_fojas_de_servicio.Carrera_administrativa
                                   select new
                                   {
                                       id = cc.Id,
                                       fecha = cc.Fecha_instrumento,
                                       tipo_novedad = cc.Tipo_novedad,
                                       tipo_instrumento = cc.Tipo_instrumento,
                                       nro_instrumento = cc.Nro_instrumento,
                                       cargo = cc.Cargo,
                                       grupo = cc.Grupo,
                                       apartado = cc.Apartado
                                   }).ToList();
                    gv_carrera_administrativa.DataSource = carrera;
                    gv_carrera_administrativa.DataBind();

                    // ---->>> cargar afectacion, designacion, asignacion
                    var afectaciones = (from ff in ag.Legajo_fojas_de_servicio.Afectaciones
                                        select new
                                        {
                                            tipo_instrumento = ff.Instrumento_tipo,
                                            nro_instrumento = ff.Instrumento_numero,
                                            anio_instrumento = ff.Instrumento_fecha.Year,
                                            descripcion = ff.Descripcion,
                                            id = ff.Id
                                        }).ToList();
                    gv_afectacion_designacion_asignacion.DataSource = afectaciones;
                    gv_afectacion_designacion_asignacion.DataBind();

                    // ---->>> cargar subrrogancia, bonificacion, antiguedad
                    var subrrogancias = (from ff in ag.Legajo_fojas_de_servicio.Pagos_Extras
                                         select new
                                         {
                                             tipo_instrumento = ff.Instrumento_tipo,
                                             nro_instrumento = ff.Instrumento_nro,
                                             anio_instrumento = ff.Instrumento_fecha,
                                             vigencia_instrumento = ff.Instrumento_vigencia,
                                             id = ff.Id
                                         }).ToList();

                    gv_subrrogancia_bonificacion_antiguedad.DataSource = subrrogancias;
                    gv_subrrogancia_bonificacion_antiguedad.DataBind();

                    // ---->>> cargar otros eventos
                    var otros_eventos = (from ff in ag.Legajo_fojas_de_servicio.Otros_eventos
                                         select new
                                         {
                                             descripcion = ff.Descripcion,
                                             lugar = ff.Lugar,
                                             fecha = ff.Fecha,
                                             id = ff.Id
                                         }).ToList();
                    gv_otros_eventos.DataSource = otros_eventos;
                    gv_otros_eventos.DataBind();
                }
                #endregion

            }
            else
            {
                OcultarBotonesEditar();
                Controles.MessageBox.Show(this, "Error al obtener los datos del agente", Controles.MessageBox.Tipo_MessageBox.Danger, "Atencion", "../Aplicativo/MainPersonal.aspx");
            }
        }

        private void OcultarBotonesEditar()
        {

            //Encabezado
            btn_editar_nombre_agente.Visible = false;
            btn_editar_estado_civil.Visible = false;
            btn_editar_sexo.Visible = false;
            //btn_editar_dni.Visible = false;
            btn_editar_legajo.Visible = false;
            btn_editar_cuit.Visible = false;
            btn_editar_situacion_de_revista.Visible = false;

            //Datos Laborales
            btn_editar_ingreso_admin_publica.Visible = false;
            btn_editar_ingreso_atp.Visible = false;
            btn_editar_anios_en_otras_partes.Visible = false;
            btn_editar_ficha_medica.Visible = false;
            btn_editar_licencia_anio_en_curso.Visible = false;
            btn_editar_licencia_anio_anterior.Visible = false;
            //btn_editar_mail.Visible = false;

            btn_editar_area.Visible = false;
            btn_editar_trabaja_como.Visible = false;
            btn_editar_hora_desde.Visible = false;
            btn_editar_hora_hasta.Visible = false;
            btn_editar_horario_flexible.Visible = false;

            btn_editar_usuario.Visible = false;
            btn_editar_perfil.Visible = false;
            btn_resetear_contraseña.Visible = false;

            //Datos personales
            btn_editar_direccion.Visible = false;
            btn_editar_localidad.Visible = false;
            btn_editar_aclaraciones_direccion.Visible = false;

            btn_agregar_titulo.Visible = false;
            btn_agregar_certificado.Visible = false;

            btn_editar_nombre_conyuge.Visible = false;
            btn_editar_dni_conyuge.Visible = false;
            btn_editar_fecha_nacimiento_conyuge.Visible = false;
            btn_editar_trabaja_conyuge.Visible = false;
            btn_editar_profesion_conyuge.Visible = false;
            btn_editar_lugar_de_trabajo_conyuge.Visible = false;

            btn_agregar_hijo.Visible = false;

            //fojas de servicio
            btn_agregar_carrera_administrativa.Visible = false;
            btn_agregar_afectacion_designacion.Visible = false;
            btn_agregar_bonificacion_antiguedad.Visible = false;
            btn_agregar_otros_eventos.Visible = false;

        }

        #region Encabezado

        protected void btn_aceptar_imagen_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            HttpPostedFile file = archivo_imagen.PostedFile;

            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentLength > 3145728)
                    {
                        Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                    }
                    else
                    {
                        string path_Destino = pathImagenesDisco + agente_cxt.Legajo.ToString() + "\\Original.jpg";

                        if (File.Exists(path_Destino))
                        {
                            File.Delete(path_Destino);
                        }

                        if (!Directory.Exists(pathImagenesDisco + agente_cxt.Legajo.ToString()))
                        {
                            Directory.CreateDirectory(pathImagenesDisco + agente_cxt.Legajo.ToString());
                        }

                        file.SaveAs(path_Destino);
                    }

                    ImagenAgente.Agente = agente_cxt;

                    Session["Agente"] = agente_cxt;
                }
                else
                {
                    Controles.MessageBox.Show(this, "No hay seleccionada ninguna imagen", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        protected void btn_aceptar_nombre_agente_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.ApellidoYNombre = tb_nombre_agente.Value;
                lbl_nombre_agente.Text = agente_cxt.ApellidoYNombre;
                //ListadoAgentesParaGrilla.ActualizarPropiedad(agente_cxt.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Nombre, agente_cxt.ApellidoYNombre);
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_estado_civil_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_personales.EstadoCivil = ddl_estado_civil.Text;
                lbl_estado_civil.Text = agente_cxt.Legajo_datos_personales.EstadoCivil;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_sexo_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_personales.Sexo = ddl_sexo.Text;
                lbl_sexo.Text = agente_cxt.Legajo_datos_personales.Sexo;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        //protected void btn_aceptar_dni_Click(object sender, EventArgs e)
        //{
        //    Agente ag = Session["Agente"] as Agente;
        //    using (var cxt = new Model1Container())
        //    {
        //        Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

        //        //realizar los cambios sobre el agente
        //        agente_cxt.Legajo_datos_personales.DNI = tb_dni.Value;
        //        lbl_dni.Text = agente_cxt.Legajo_datos_personales.DNI;
        //        ///////////////////////////////////////

        //        cxt.SaveChanges();
        //        Session["Agente"] = agente_cxt;
        //    }
        //}

        protected void btn_aceptar_legajo_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo = Convert.ToInt32(tb_legajo.Value);
                lbl_legajo.Text = agente_cxt.Legajo.ToString();
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        protected void btn_aceptar_cuit_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_laborales.CUIT = tb_cuit.Value;
                lbl_cuit.Text = agente_cxt.Legajo_datos_laborales.CUIT;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_nacimiento_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            if (DateTime.TryParse(tb_nacimiento.Value, out fecha))
            {
                Agente ag = Session["Agente"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente
                    agente_cxt.Legajo_datos_personales.FechaNacimiento = fecha;
                    lbl_nacimiento.Text = agente_cxt.Legajo_datos_personales.FechaNacimiento.ToShortDateString();
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "La fecha ingresada es inválida", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_situacion_de_revista_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_laborales.Situacion_de_revista = ddl_tipo_situacion_de_revista.SelectedItem.Text;
                agente_cxt.Legajo_datos_laborales.Cargo = tb_cargo_situacion_de_revista.Value;
                agente_cxt.Legajo_datos_laborales.Grupo = tb_grupo_situacion_de_revista.Value;
                agente_cxt.Legajo_datos_laborales.Apartado = tb_apartado_situacion_de_revista.Value;

                lbl_situacion_de_revista.Text = agente_cxt.Legajo_datos_laborales.Situacion_de_revista;

                if (agente_cxt.Legajo_datos_laborales.Cargo != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Cargo: " + agente_cxt.Legajo_datos_laborales.Cargo;
                }
                if (agente_cxt.Legajo_datos_laborales.Grupo != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Grupo: " + agente_cxt.Legajo_datos_laborales.Grupo;
                }
                if (agente_cxt.Legajo_datos_laborales.Apartado != "")
                {
                    lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Apartado: " + agente_cxt.Legajo_datos_laborales.Apartado;
                }



                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        #endregion

        #region Datos laborales
        protected void btn_aceptar_admin_publica_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            if (DateTime.TryParse(tb_ingreso_admin_publica.Value, out fecha))
            {
                Agente ag = Session["Agente"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente
                    agente_cxt.Legajo_datos_laborales.FechaIngresoAminPub = fecha;
                    lbl_ingreso_admin_publica.Text = agente_cxt.Legajo_datos_laborales.FechaIngresoAminPub.ToShortDateString();
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "La fecha ingresada es inválida", Controles.MessageBox.Tipo_MessageBox.Warning);
            }

        }

        protected void btn_aceptar_ingreso_atp_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            if (DateTime.TryParse(tb_ingreso_atp.Value, out fecha))
            {
                Agente ag = Session["Agente"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente
                    agente_cxt.Legajo_datos_laborales.FechaIngresoATP = fecha;
                    lbl_ingreso_atp.Text = agente_cxt.Legajo_datos_laborales.FechaIngresoATP.ToShortDateString();
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "La fecha ingresada es inválida", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_anios_en_otras_partes_Click(object sender, EventArgs e)
        {
            Int16 anios;
            Int16 meses;

            if (Int16.TryParse(tb_anios_en_otras_partes.Value, out anios) && Int16.TryParse(tb_meses_en_otras_partes.Value, out meses))
            {
                Agente ag = Session["Agente"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente
                    agente_cxt.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes = anios;
                    agente_cxt.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes = meses;
                    lbl_anios_en_otras_partes.Text = agente_cxt.Legajo_datos_laborales.AniosAntiguedadReconicidosOtrasPartes + " años y " + agente_cxt.Legajo_datos_laborales.MesesAntiguedadReconocidosOtrasPartes + " meses";
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Los números de años y meses son inválidos", Controles.MessageBox.Tipo_MessageBox.Warning);
            }


        }

        protected void btn_aceptar_ficha_medica_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_laborales.FichaMedica = tb_ficha_medica.Value;
                lbl_ficha_medica.Text = agente_cxt.Legajo_datos_laborales.FichaMedica;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        //protected void btn_aceptar_mail_Click(object sender, EventArgs e)
        //{
        //    Agente ag = Session["Agente"] as Agente;
        //    using (var cxt = new Model1Container())
        //    {
        //        Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

        //        //realizar los cambios sobre el agente
        //        agente_cxt.Legajo_datos_laborales.Email = tb_mail.Value;
        //        lbl_mail.Text = agente_cxt.Legajo_datos_laborales.Email;
        //        //ListadoAgentesParaGrilla.ActualizarPropiedad(agente_cxt.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Mail, agente_cxt.Legajo_datos_laborales.Email);
        //        ///////////////////////////////////////

        //        cxt.SaveChanges();
        //        Session["Agente"] = agente_cxt;
        //    }

        //}

        protected void btn_aceptar_licencia_anio_en_curso_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            int dias;
            if (int.TryParse(tb_licencia_anio_en_curso.Value, out dias))
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                    LicenciaAgente licencia = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                    //LicenciaAgente licencia = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                    //realizar los cambios sobre el agente
                    if (licencia != null)
                    {
                        licencia.DiasOtorgados = dias;
                        lbl_licencia_anio_en_curso.Text = licencia.DiasOtorgados.ToString() + " días.";
                    }
                    else
                    {
                        LicenciaAgente la = new LicenciaAgente();
                        la.AgenteId = ag.Id;
                        la.TipoLicenciaId = tli.Id;
                        la.Anio = DateTime.Today.Year;
                        la.DiasOtorgados = dias;
                        la.DiasUsufructuadosIniciales = 0;
                        cxt.LicenciasAgentes.AddObject(la);
                        lbl_licencia_anio_en_curso.Text = la.DiasUsufructuadosIniciales.ToString() + " días.";
                    }
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "El número ingresado es inválido", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_licencia_anio_anterior_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            int dias;
            if (int.TryParse(tb_licencia_anio_anterior.Value, out dias))
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    TipoLicencia tli = cxt.TiposDeLicencia.First(t => t.Tipo == "Licencia anual");
                    //LicenciaAgente licencia = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year && l.Tipo.Id == tli.Id);
                    LicenciaAgente licencia = ag.Licencias.FirstOrDefault(l => l.Anio == DateTime.Today.Year - 1 && l.Tipo.Id == tli.Id);

                    //realizar los cambios sobre el agente
                    if (licencia != null)
                    {
                        licencia.DiasOtorgados = dias;
                        lbl_licencia_anio_anterior.Text = licencia.DiasOtorgados.ToString() + " días.";
                    }
                    else
                    {
                        LicenciaAgente la = new LicenciaAgente();
                        la.AgenteId = ag.Id;
                        la.TipoLicenciaId = tli.Id;
                        la.Anio = DateTime.Today.Year - 1;
                        la.DiasOtorgados = dias;
                        la.DiasUsufructuadosIniciales = 0;
                        cxt.LicenciasAgentes.AddObject(la);
                        lbl_licencia_anio_anterior.Text = la.DiasUsufructuadosIniciales.ToString() + " días.";
                    }
                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "El número ingresado es inválido", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_area_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;

            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                Reasignacion re = agente_cxt.Reasignaciones.FirstOrDefault(r => r.Hasta == null);
                if (agente_cxt.AreaId != null && agente_cxt.AreaId != Convert.ToInt32(ddl_areas.SelectedItem.Value))
                {
                    if (re != null)
                    {
                        re.Hasta = DateTime.Today;
                    }

                    agente_cxt.AreaId = Convert.ToInt32(ddl_areas.SelectedValue);
                    Reasignacion nueva = new Reasignacion();
                    nueva.Desde = DateTime.Today;
                    nueva.AgenteId = agente_cxt.Id;
                    nueva.AreaId = Convert.ToInt32(ddl_areas.SelectedValue);

                    cxt.Reasignaciones.AddObject(nueva);
                    //ListadoAgentesParaGrilla.ActualizarPropiedad(agente_cxt.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Area, ddl_areas.SelectedItem.Text);
                }
                else
                {
                    if (agente_cxt.AreaId == null)
                    {
                        agente_cxt.AreaId = Convert.ToInt32(ddl_areas.SelectedValue);
                        Reasignacion nueva = new Reasignacion();
                        nueva.Desde = DateTime.Today;
                        nueva.AgenteId = agente_cxt.Id;
                        nueva.AreaId = Convert.ToInt32(ddl_areas.SelectedValue);

                        cxt.Reasignaciones.AddObject(nueva);
                        //ListadoAgentesParaGrilla.ActualizarPropiedad(agente_cxt.Id, ListadoAgentesParaGrilla.PropiedadPorActualizar.Area, ddl_areas.SelectedItem.Text);
                    }
                }

                lbl_area_agente.Text = ddl_areas.SelectedItem.Text;

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_trabaja_como_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                switch (ddl_tipo_agente.SelectedItem.Text)
                {
                    case "Agente":
                        agente_cxt.Jefe = false;
                        agente_cxt.JefeTemporal = false;
                        agente_cxt.JefeTemporalHasta = null;
                        break;
                    case "Jefe":
                        agente_cxt.Jefe = true;
                        agente_cxt.JefeTemporal = false;
                        agente_cxt.JefeTemporalHasta = null;
                        break;
                    case "Jefe temporal":
                        DateTime fecha;
                        if (DateTime.TryParse(tb_jefe_temporal_hasta.Value, out fecha))
                        {
                            agente_cxt.Jefe = false;
                            agente_cxt.JefeTemporal = true;
                            agente_cxt.JefeTemporalHasta = fecha;
                        }
                        else
                        {
                            Controles.MessageBox.Show(this, "Debe ingresar fecha hasta para el cargo de jefe temporal.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        break;
                    default:
                        break;
                }

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_horario_laboral_desde_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;

            Regex reg_hora = new Regex("([01]?[0-9]|2[0-3]):[0-5][0-9]");
            Match validacion = reg_hora.Match(tb_horario_laboral_desde.Value);

            if (validacion.Success && validacion.Value == tb_horario_laboral_desde.Value)
            {

                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente

                    agente_cxt.HoraEntrada = tb_horario_laboral_desde.Value;
                    lbl_hora_desde.Text = agente_cxt.HoraEntrada;

                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "La hora ingresada no tiene el formato correcto.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }

        }

        protected void btn_aceptar_horario_laboral_hasta_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;

            Regex reg_hora = new Regex("([01]?[0-9]|2[0-3]):[0-5][0-9]");
            Match validacion = reg_hora.Match(tb_horario_laboral_hasta.Value);

            if (validacion.Success && validacion.Value == tb_horario_laboral_hasta.Value)
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    //realizar los cambios sobre el agente

                    agente_cxt.HoraSalida = tb_horario_laboral_hasta.Value;
                    lbl_hora_hasta.Text = agente_cxt.HoraSalida;

                    ///////////////////////////////////////

                    cxt.SaveChanges();
                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "La hora ingresada no tiene el formato correcto.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_horario_flexible_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                switch (ddl_horario_flexible.SelectedItem.Text)
                {
                    case "Si":
                        agente_cxt.HorarioFlexible = true;
                        break;
                    case "No":
                        agente_cxt.HorarioFlexible = false;
                        break;
                    default:
                        break;
                }

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_usuario_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                string usuario = tb_usuario.Value.ToUpper();
                bool existe = cxt.Agentes.FirstOrDefault(aa => aa.Id != agente_cxt.Id && aa.Usr.ToUpper() == usuario) != null && usuario != "SU";
                if (!existe)
                {
                    agente_cxt.Usr = usuario;
                    lbl_usuario.Text = agente_cxt.Usr;
                }
                else
                {
                    Controles.MessageBox.Show(this, "Ya existe un usuario con ese nombre.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_perfil_usuario_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                switch (ddl_perfil_usuario.SelectedItem.Text)
                {
                    case "Agente":
                        agente_cxt.Perfil = PerfilUsuario.Agente;
                        break;
                    case "Personal":
                        agente_cxt.Perfil = PerfilUsuario.Personal;
                        break;
                    case "Guardia":
                        agente_cxt.Perfil = PerfilUsuario.Guardia;
                        break;
                    default:
                        break;
                }

                lbl_perfil_agente.Text = ddl_perfil_usuario.SelectedItem.Text;

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_resetear_contraseña_Click(object sender, EventArgs e)
        {

            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                string pass = System.Web.Security.Membership.GeneratePassword(8, 0).ToUpper();
                pass = Regex.Replace(pass, @"[^a-zA-Z0-9]", m => "9");
                agente_cxt.Pass = Cripto.Encriptar(pass);

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;

                ///////////////////////////////////////

                //creo el reporte
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Aplicativo/Reportes/OtorgoClave.rdlc");

                OtorgoClave_DS ds = new OtorgoClave_DS();

                Agente personal = Session["UsuarioLogueado"] as Agente;

                OtorgoClave_DS.DatosRow dr = ds.Datos.NewDatosRow();

                dr.AgentePersonal = personal.ApellidoYNombre;
                dr.NombreAgente = agente_cxt.ApellidoYNombre;
                dr.Usuario = agente_cxt.Usr;
                dr.Departamento = agente_cxt.Area.Nombre;
                dr.Clave = pass;
                dr.Fecha = "Resistencia, " + DateTime.Today.ToLongDateString();

                ds.Datos.Rows.Add(dr);

                ReportDataSource maestro = new ReportDataSource("DS", ds.Datos.Rows);

                viewer.LocalReport.DataSources.Add(maestro);

                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = null;
                string encoding = null;
                string extension = null;
                string deviceInfo = null;
                byte[] bytes = null;

                deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>";

                //Render the report
                bytes = viewer.LocalReport.Render("PDF", deviceInfo, out  mimeType, out encoding, out extension, out streamids, out warnings);
                Session["Bytes"] = bytes;

                string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
            }
        }

        #endregion

        #region Datos personales

        protected void btn_aceptar_direccion_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_personales.Domicilio = tb_direccion.Value;

                Legajo_historial_domicilio hd = agente_cxt.Legajo_datos_personales.Historial_domicilios.FirstOrDefault(hhdd => hhdd.Domicilio.Contains(agente_cxt.Legajo_datos_personales.Domicilio));
                if (hd == null)
                {
                    hd = new Legajo_historial_domicilio();
                    agente_cxt.Legajo_datos_personales.Historial_domicilios.Add(hd);
                }

                hd.Domicilio = agente_cxt.Legajo_datos_personales.Domicilio + " - Localidad: " + agente_cxt.Legajo_datos_personales.Domicilio_localidad + " - Observaciones: " + agente_cxt.Legajo_datos_personales.DomicilioObservaciones;
                hd.Fecha = DateTime.Now;

                lbl_direccion.Text = agente_cxt.Legajo_datos_personales.Domicilio;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        protected void btn_aceptar_aclaraciones_direccion_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_personales.DomicilioObservaciones = tb_aclaraciones_direccion.Value;

                Legajo_historial_domicilio hd = agente_cxt.Legajo_datos_personales.Historial_domicilios.FirstOrDefault(hhdd => hhdd.Domicilio.Contains(agente_cxt.Legajo_datos_personales.Domicilio));
                if (hd == null)
                {
                    hd = new Legajo_historial_domicilio();
                    agente_cxt.Legajo_datos_personales.Historial_domicilios.Add(hd);
                }

                hd.Domicilio = agente_cxt.Legajo_datos_personales.Domicilio + " - Localidad: " + agente_cxt.Legajo_datos_personales.Domicilio_localidad + " - Observaciones: " + agente_cxt.Legajo_datos_personales.DomicilioObservaciones;
                hd.Fecha = DateTime.Now;

                img_aclaraciones_domicilio.ToolTip = agente_cxt.Legajo_datos_personales.DomicilioObservaciones;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_localidad_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente
                agente_cxt.Legajo_datos_personales.Domicilio_localidad = tb_localidad.Value;

                Legajo_historial_domicilio hd = agente_cxt.Legajo_datos_personales.Historial_domicilios.FirstOrDefault(hhdd => hhdd.Domicilio.Contains(agente_cxt.Legajo_datos_personales.Domicilio));
                if (hd == null)
                {
                    hd = new Legajo_historial_domicilio();
                    agente_cxt.Legajo_datos_personales.Historial_domicilios.Add(hd);
                }

                hd.Domicilio = agente_cxt.Legajo_datos_personales.Domicilio + " - Localidad: " + agente_cxt.Legajo_datos_personales.Domicilio_localidad + " - Observaciones: " + agente_cxt.Legajo_datos_personales.DomicilioObservaciones;
                hd.Fecha = DateTime.Now;

                lbl_localidad.Text = agente_cxt.Legajo_datos_personales.Domicilio_localidad;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_nombre_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                agente_cxt.Legajo_datos_personales.Conyuge.Apellido_y_nombre = tb_nombre_conyuge.Value;
                lbl_nombre_conyuge.Text = agente_cxt.Legajo_datos_personales.Conyuge.Apellido_y_nombre;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_dni_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                agente_cxt.Legajo_datos_personales.Conyuge.DNI = tb_dni_conyuge.Value;
                lbl_dni_conyuge.Text = agente_cxt.Legajo_datos_personales.Conyuge.DNI;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_fecha_nacimiento_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                agente_cxt.Legajo_datos_personales.Conyuge.Fecha_de_nacimiento = Convert.ToDateTime(tb_fecha_nacimiento_conyuge.Value);
                lbl_fecha_nacimiento_conyuge.Text = agente_cxt.Legajo_datos_personales.Conyuge.Fecha_de_nacimiento.ToShortDateString();
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        protected void btn_aceptar_trabaja_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                switch (ddl_trabaja_conyuge.SelectedItem.Text)
                {
                    case "Si":
                        agente_cxt.Legajo_datos_personales.Conyuge.Trabaja = true;
                        break;
                    case "No":
                        agente_cxt.Legajo_datos_personales.Conyuge.Trabaja = false;
                        break;
                    default:
                        break;
                }

                lbl_trabaja_conyuge.Text = ddl_trabaja_conyuge.Text;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        protected void btn_aceptar_lugar_de_trabajo_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                lbl_lugar_de_trabajo_conyuge.Text = ddl_lugar_de_trabajo_conyuge.Text;

                if (ddl_lugar_de_trabajo_conyuge.Text == "Público")
                {
                    lbl_lugar_de_trabajo_conyuge.Text = lbl_lugar_de_trabajo_conyuge.Text + " - Dependencia: " + tb_nombre_lugar_de_trabajo_conyuge.Value;
                }

                agente_cxt.Legajo_datos_personales.Conyuge.Lugar_de_trabajo = lbl_lugar_de_trabajo_conyuge.Text;
                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }

        }

        protected void btn_aceptar_profesion_conyuge_Click(object sender, EventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                //realizar los cambios sobre el agente

                Legajo_conyuge conyuge;
                if (agente_cxt.Legajo_datos_personales.Conyuge == null)
                {
                    conyuge = new Legajo_conyuge()
                    {
                        Apellido_y_nombre = "",
                        Asignacion_familiar = false,
                        Dependencia = "",
                        DNI = "",
                        Fecha_de_nacimiento = DateTime.Today,
                        Lugar_de_trabajo = "",
                        Profesion = "",
                        Trabaja = false
                    };

                    agente_cxt.Legajo_datos_personales.Conyuge = conyuge;
                }

                agente_cxt.Legajo_datos_personales.Conyuge.Profesion = tb_profesion_conyuge.Value;
                lbl_profesion_conyuge.Text = agente_cxt.Legajo_datos_personales.Conyuge.Profesion;

                ///////////////////////////////////////

                cxt.SaveChanges();
                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_hijo_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            if (DateTime.TryParse(tb_fecha_nacimiento_hijo.Value, out fecha) && tb_nombre_hijo.Value != "" && tb_dni_hijo.Value != "")
            {
                Agente ag = Session["Agente"] as Agente;
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    Legajo_hijo hijo = new Legajo_hijo();
                    hijo.Apellido_y_nombre = tb_nombre_hijo.Value;
                    hijo.DNI = tb_dni_hijo.Value;
                    hijo.Fecha_de_nacimiento = Convert.ToDateTime(tb_fecha_nacimiento_hijo.Value);
                    hijo.Observaciones = tb_observaciones_hijo.Value;

                    agente_cxt.Legajo_datos_personales.Hijos.Add(hijo);

                    cxt.SaveChanges();

                    var hijos = (from hh in agente_cxt.Legajo_datos_personales.Hijos
                                 select new
                                 {
                                     nombre = hh.Apellido_y_nombre,
                                     dni = hh.DNI,
                                     nacimiento = hh.Fecha_de_nacimiento,
                                     edad = hh.Edad,
                                     id = hh.Id,
                                     observaciones = hh.Observaciones
                                 }).ToList();
                    gv_hijos.DataSource = hijos;
                    gv_hijos.DataBind();

                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los datos son obligatorios, excepto las observaciones.", Controles.MessageBox.Tipo_MessageBox.Warning);
            }

        }

        protected void btn_eliminar_hijo_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int idHijo = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                cxt.Legajo_hijos.DeleteObject(cxt.Legajo_hijos.First(hh => hh.Id == idHijo));

                cxt.SaveChanges();

                var hijos = (from hh in agente_cxt.Legajo_datos_personales.Hijos
                             select new
                             {
                                 nombre = hh.Apellido_y_nombre,
                                 dni = hh.DNI,
                                 nacimiento = hh.Fecha_de_nacimiento,
                                 edad = hh.Edad,
                                 id = hh.Id,
                                 observaciones = hh.Observaciones
                             }).ToList();

                gv_hijos.DataSource = hijos;
                gv_hijos.DataBind();

                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_aceptar_agregar_titulo_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_titulo.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_fecha_emision_titulo.Value, out fecha) && tb_descripcion_titlulo.Value != "" && tb_duracion_titulo.Value != "" && tb_institucion_titulo.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    Legajo_titulo_certificado titulo = new Legajo_titulo_certificado();
                    titulo.Descripcion = tb_descripcion_titlulo.Value;
                    titulo.Nivel = "Título " + ddl_estudios.SelectedItem.Text;
                    titulo.Duracion = tb_duracion_titulo.Value + " años";
                    titulo.Lugar_insticucion = tb_institucion_titulo.Value;
                    titulo.Fecha_emision = fecha;
                    titulo.Tipo_certificado = "Titulo";


                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Titulos\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            titulo.Path_archivo = fullpath;
                            agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado.Add(titulo);

                            cxt.SaveChanges();

                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }

                        var titulos = (from tt in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                                       where tt.Tipo_certificado == "Titulo"
                                       select new
                                       {
                                           nivel = tt.Nivel,
                                           titulo = tt.Descripcion,
                                           id = tt.Id
                                       }).ToList();
                        gv_titulos.DataSource = titulos;
                        gv_titulos.DataBind();

                        Session["Agente"] = agente_cxt;
                    }
                    else
                    {
                        titulo.Path_archivo = "";
                        agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado.Add(titulo);
                        cxt.SaveChanges();

                        var titulos = (from tt in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                                       where tt.Tipo_certificado == "Titulo"
                                       select new
                                       {
                                           nivel = tt.Nivel,
                                           titulo = tt.Descripcion,
                                           id = tt.Id
                                       }).ToList();
                        gv_titulos.DataSource = titulos;
                        gv_titulos.DataBind();

                        Session["Agente"] = agente_cxt;
                    }

                }

            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_ver_titulo_Click(object sender, ImageClickEventArgs e)
        {
            int idTitulo = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_titulo_certificado titulo = cxt.Legajo_titulos_certificados.First(tt => tt.Id == idTitulo);

                string path = pathArchivosDisco + titulo.Legajo_datos_personales.Agente.Legajo + "\\Titulos\\" + Path.GetFileName(titulo.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }
            }
        }

        protected void btn_quitar_titulo_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int idTitulo = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_titulo_certificado titulo = cxt.Legajo_titulos_certificados.First(hh => hh.Id == idTitulo);

                string path = pathArchivosDisco + titulo.Legajo_datos_personales.Agente.Legajo + "\\Titulos\\" + Path.GetFileName(titulo.Path_archivo);

                cxt.Legajo_titulos_certificados.DeleteObject(titulo);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var titulos = (from tt in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                               where tt.Tipo_certificado == "Titulo"
                               select new
                               {
                                   nivel = tt.Nivel,
                                   titulo = tt.Descripcion,
                                   id = tt.Id
                               }).ToList();
                gv_titulos.DataSource = titulos;
                gv_titulos.DataBind();

                Session["Agente"] = agente_cxt;

            }
        }

        protected void btn_agregar_certificado_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_certificado.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_fecha_curso.Value, out fecha) && tb_descripcion_curso.Value != "" && tb_duracion_curso.Value != "" && tb_lugar_curso.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    Legajo_titulo_certificado certificado = new Legajo_titulo_certificado();
                    certificado.Descripcion = tb_descripcion_curso.Value;
                    certificado.Nivel = " - ";
                    certificado.Duracion = tb_duracion_curso.Value + " " + ddl_duracion_curso.SelectedItem.Text;
                    certificado.Lugar_insticucion = tb_lugar_curso.Value;
                    certificado.Fecha_emision = fecha;
                    certificado.Tipo_certificado = "Certificado";


                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Certificados curso\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            certificado.Path_archivo = fullpath;
                            agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado.Add(certificado);

                            cxt.SaveChanges();

                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }

                        var certificados = (from cc in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                                            where cc.Tipo_certificado == "Certificado"
                                            select new
                                            {
                                                descripcion = cc.Descripcion,
                                                lugar = cc.Lugar_insticucion,
                                                fecha_emision = cc.Fecha_emision,
                                                duracion = cc.Duracion,
                                                id = cc.Id
                                            }).ToList();
                        gv_certificados.DataSource = certificados;
                        gv_certificados.DataBind();

                        Session["Agente"] = agente_cxt;
                    }
                    else
                    {
                        certificado.Path_archivo = "";
                        agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado.Add(certificado);
                        cxt.SaveChanges();

                        var certificados = (from cc in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                                            where cc.Tipo_certificado == "Certificado"
                                            select new
                                            {
                                                descripcion = cc.Descripcion,
                                                lugar = cc.Lugar_insticucion,
                                                fecha_emision = cc.Fecha_emision,
                                                duracion = cc.Duracion,
                                                id = cc.Id
                                            }).ToList();
                        gv_certificados.DataSource = certificados;
                        gv_certificados.DataBind();

                        Session["Agente"] = agente_cxt;
                    }

                }

            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_ver_certificado_Click(object sender, ImageClickEventArgs e)
        {
            int idCertificado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_titulo_certificado certificado = cxt.Legajo_titulos_certificados.First(tt => tt.Id == idCertificado);

                string path = pathArchivosDisco + certificado.Legajo_datos_personales.Agente.Legajo + "\\Certificados curso\\" + Path.GetFileName(certificado.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

            }
        }

        protected void btn_quitar_certificado_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int idCertificado = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_titulo_certificado certificado = cxt.Legajo_titulos_certificados.First(hh => hh.Id == idCertificado);

                string path = pathArchivosDisco + certificado.Legajo_datos_personales.Agente.Legajo + "\\Certificados curso\\" + Path.GetFileName(certificado.Path_archivo);

                cxt.Legajo_titulos_certificados.DeleteObject(certificado);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var certificados = (from cc in agente_cxt.Legajo_datos_personales.Legajo_titulo_certificado
                                    where cc.Tipo_certificado == "Certificado"
                                    select new
                                    {
                                        descripcion = cc.Descripcion,
                                        lugar = cc.Lugar_insticucion,
                                        fecha_emision = cc.Fecha_emision,
                                        duracion = cc.Duracion,
                                        id = cc.Id
                                    }).ToList();
                gv_certificados.DataSource = certificados;
                gv_certificados.DataBind();

                Session["Agente"] = agente_cxt;

            }
        }

        #endregion

        #region Fojas de servicio
        protected void btn_aceptar_carrera_administrativa_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_carrera_administrativa.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_fecha_instrumento_carrera_administrativa.Value, out fecha) && tb_nro_instrumento_carrera_administrativa.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    if (agente_cxt.Legajo_fojas_de_servicio == null)
                    {
                        agente_cxt.Legajo_fojas_de_servicio = new Legajo_foja_de_servicio();
                        cxt.SaveChanges();
                    }

                    Legajo_carrera_administrativa novedad_carrera = new Legajo_carrera_administrativa()
                    {
                        Apartado = tb_apartado_carrera_administrativa.Value,
                        Cargo = tb_cargo_carrera_administrativa.Value,
                        Fecha_instrumento = fecha,
                        Grupo = tb_grupo_carrera_administrativa.Value,
                        Nro_instrumento = tb_nro_instrumento_carrera_administrativa.Value,
                        Tipo_instrumento = ddl_tipo_instrumento_carrera_administrativa.SelectedItem.Text,
                        Tipo_novedad = ddl_tipo_movimiento_carrera_administrativa.SelectedItem.Text
                    };

                    agente_cxt.Legajo_fojas_de_servicio.Carrera_administrativa.Add(novedad_carrera);

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Carrera administrativa\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            novedad_carrera.Path_archivo = fullpath;



                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }
                    }
                    else
                    {
                        novedad_carrera.Path_archivo = "";
                    }

                    agente_cxt.Legajo_datos_laborales.Situacion_de_revista = ddl_tipo_movimiento_carrera_administrativa.SelectedItem.Text;
                    agente_cxt.Legajo_datos_laborales.Cargo = tb_cargo_carrera_administrativa.Value;
                    agente_cxt.Legajo_datos_laborales.Grupo = tb_grupo_carrera_administrativa.Value;
                    agente_cxt.Legajo_datos_laborales.Apartado = tb_apartado_carrera_administrativa.Value;

                    lbl_situacion_de_revista.Text = agente_cxt.Legajo_datos_laborales.Situacion_de_revista;

                    if (agente_cxt.Legajo_datos_laborales.Cargo != "")
                    {
                        lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Cargo: " + agente_cxt.Legajo_datos_laborales.Cargo;
                    }
                    if (agente_cxt.Legajo_datos_laborales.Grupo != "")
                    {
                        lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Grupo: " + agente_cxt.Legajo_datos_laborales.Grupo;
                    }
                    if (agente_cxt.Legajo_datos_laborales.Apartado != "")
                    {
                        lbl_situacion_de_revista.Text = lbl_situacion_de_revista.Text + " - Apartado: " + agente_cxt.Legajo_datos_laborales.Apartado;
                    }

                    cxt.SaveChanges();

                    var carrera = (from cc in agente_cxt.Legajo_fojas_de_servicio.Carrera_administrativa
                                   select new
                                   {
                                       id = cc.Id,
                                       fecha = cc.Fecha_instrumento,
                                       tipo_novedad = cc.Tipo_novedad,
                                       tipo_instrumento = cc.Tipo_instrumento,
                                       nro_instrumento = cc.Nro_instrumento,
                                       cargo = cc.Cargo,
                                       grupo = cc.Grupo,
                                       apartado = cc.Apartado
                                   }).ToList();
                    gv_carrera_administrativa.DataSource = carrera;
                    gv_carrera_administrativa.DataBind();

                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto cargo, grupo, apartado y el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_situacion_revista_novedad_carrera_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_carrera_administrativa.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_fecha_instrumento_carrera_administrativa.Value, out fecha) && tb_nro_instrumento_carrera_administrativa.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    if (agente_cxt.Legajo_fojas_de_servicio == null)
                    {
                        agente_cxt.Legajo_fojas_de_servicio = new Legajo_foja_de_servicio();
                        cxt.SaveChanges();
                    }

                    Legajo_carrera_administrativa novedad_carrera = new Legajo_carrera_administrativa()
                    {
                        Apartado = tb_apartado_carrera_administrativa.Value,
                        Cargo = tb_cargo_carrera_administrativa.Value,
                        Fecha_instrumento = fecha,
                        Grupo = tb_grupo_carrera_administrativa.Value,
                        Nro_instrumento = tb_nro_instrumento_carrera_administrativa.Value,
                        Tipo_instrumento = ddl_tipo_instrumento_carrera_administrativa.SelectedItem.Text,
                        Tipo_novedad = ddl_tipo_movimiento_carrera_administrativa.SelectedItem.Text
                    };

                    agente_cxt.Legajo_fojas_de_servicio.Carrera_administrativa.Add(novedad_carrera);

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Carrera administrativa\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            novedad_carrera.Path_archivo = fullpath;



                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }
                    }
                    else
                    {
                        novedad_carrera.Path_archivo = "";
                    }

                    cxt.SaveChanges();

                    var carrera = (from cc in agente_cxt.Legajo_fojas_de_servicio.Carrera_administrativa
                                   select new
                                   {
                                       id = cc.Id,
                                       fecha = cc.Fecha_instrumento,
                                       tipo_novedad = cc.Tipo_novedad,
                                       tipo_instrumento = cc.Tipo_instrumento,
                                       nro_instrumento = cc.Nro_instrumento,
                                       cargo = cc.Cargo,
                                       grupo = cc.Grupo,
                                       apartado = cc.Apartado
                                   }).ToList();
                    gv_carrera_administrativa.DataSource = carrera;
                    gv_carrera_administrativa.DataBind();

                    Session["Agente"] = agente_cxt;
                }
            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto cargo, grupo, apartado y el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_afectacion_designacion_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_afectacion_designacion.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_anio_instrumento_afectacion_designacion.Value, out fecha) && tb_nro_instrumento_afectacion_designacion.Value != "" && tb_descripcion_afectacion_designacion.Value != "" && tb_tipo_instrumento_afectacion_designacion.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    if (agente_cxt.Legajo_fojas_de_servicio == null)
                    {
                        agente_cxt.Legajo_fojas_de_servicio = new Legajo_foja_de_servicio();
                        cxt.SaveChanges();
                    }

                    Legajo_afectacion_designacion afectacion = new Legajo_afectacion_designacion();
                    afectacion.Descripcion = tb_descripcion_afectacion_designacion.Value;
                    afectacion.Instrumento_fecha = fecha;
                    afectacion.Instrumento_numero = tb_nro_instrumento_afectacion_designacion.Value;
                    afectacion.Instrumento_tipo = tb_tipo_instrumento_afectacion_designacion.Value;

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Afectaciones - designaciones\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            afectacion.Path_archivo = fullpath;
                            agente_cxt.Legajo_fojas_de_servicio.Afectaciones.Add(afectacion);

                            cxt.SaveChanges();

                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }

                        var afectaciones = (from ff in agente_cxt.Legajo_fojas_de_servicio.Afectaciones
                                            select new
                                            {
                                                tipo_instrumento = ff.Instrumento_tipo,
                                                nro_instrumento = ff.Instrumento_numero,
                                                anio_instrumento = ff.Instrumento_fecha.Year,
                                                descripcion = ff.Descripcion,
                                                id = ff.Id
                                            }).ToList();
                        gv_afectacion_designacion_asignacion.DataSource = afectaciones;
                        gv_afectacion_designacion_asignacion.DataBind();

                        Session["Agente"] = agente_cxt;
                    }
                    else
                    {
                        afectacion.Path_archivo = "";
                        agente_cxt.Legajo_fojas_de_servicio.Afectaciones.Add(afectacion);
                        cxt.SaveChanges();

                        var afectaciones = (from ff in agente_cxt.Legajo_fojas_de_servicio.Afectaciones
                                            select new
                                            {
                                                tipo_instrumento = ff.Instrumento_tipo,
                                                nro_instrumento = ff.Instrumento_numero,
                                                anio_instrumento = ff.Instrumento_fecha.Year,
                                                descripcion = ff.Descripcion,
                                                id = ff.Id
                                            }).ToList();
                        gv_afectacion_designacion_asignacion.DataSource = afectaciones;
                        gv_afectacion_designacion_asignacion.DataBind();

                        Session["Agente"] = agente_cxt;
                    }

                }

            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_otro_evento_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_otro_evento.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_fecha_otro_evento.Value, out fecha) && tb_descripcion_otros_eventos.Value != "" && tb_lugar_otros_eventos.Value != "" && tb_descripcion_otros_eventos.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    if (agente_cxt.Legajo_fojas_de_servicio == null)
                    {
                        agente_cxt.Legajo_fojas_de_servicio = new Legajo_foja_de_servicio();
                        cxt.SaveChanges();
                    }

                    Legajo_otro_evento oevento = new Legajo_otro_evento();
                    oevento.Fecha = fecha;
                    oevento.Lugar = tb_lugar_otros_eventos.Value;
                    oevento.Descripcion = tb_descripcion_otros_eventos.Value;

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Otros eventos\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            oevento.Path_archivo = fullpath;
                            agente_cxt.Legajo_fojas_de_servicio.Otros_eventos.Add(oevento);

                            cxt.SaveChanges();

                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }

                        var otros_eventos = (from ff in agente_cxt.Legajo_fojas_de_servicio.Otros_eventos
                                             select new
                                             {
                                                 descripcion = ff.Descripcion,
                                                 lugar = ff.Lugar,
                                                 fecha = ff.Fecha,
                                                 id = ff.Id
                                             }).ToList();
                        gv_otros_eventos.DataSource = otros_eventos;
                        gv_otros_eventos.DataBind();

                        Session["Agente"] = agente_cxt;
                    }
                    else
                    {
                        oevento.Path_archivo = "";
                        agente_cxt.Legajo_fojas_de_servicio.Otros_eventos.Add(oevento);
                        cxt.SaveChanges();

                        var otros_eventos = (from ff in agente_cxt.Legajo_fojas_de_servicio.Otros_eventos
                                             select new
                                             {
                                                 descripcion = ff.Descripcion,
                                                 lugar = ff.Lugar,
                                                 fecha = ff.Fecha,
                                                 id = ff.Id
                                             }).ToList();
                        gv_otros_eventos.DataSource = otros_eventos;
                        gv_otros_eventos.DataBind();

                        Session["Agente"] = agente_cxt;
                    }

                }

            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_aceptar_agregar_bonificacion_antiguedad_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = archivo_bonificacion_antiguedad.PostedFile;
            DateTime fecha;
            Agente ag = Session["Agente"] as Agente;

            if (DateTime.TryParse(tb_anio_instrumento_bonificacion_antiguedad.Value, out fecha) && tb_nro_instrumento_bonificacion_antiguedad.Value != "" && tb_tipo_instrumento_bonificacion_antiguedad.Value != "" && tb_vigencia_instrumento_bonificacion_antiguedad.Value != "")
            {
                using (var cxt = new Model1Container())
                {
                    Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                    if (agente_cxt.Legajo_fojas_de_servicio == null)
                    {
                        agente_cxt.Legajo_fojas_de_servicio = new Legajo_foja_de_servicio();
                        cxt.SaveChanges();
                    }

                    Legajo_pago_subrrogancia_bonificacion_antiguedad subrrogancia = new Legajo_pago_subrrogancia_bonificacion_antiguedad();
                    subrrogancia.Instrumento_fecha = fecha;
                    subrrogancia.Instrumento_nro = tb_nro_instrumento_bonificacion_antiguedad.Value;
                    subrrogancia.Instrumento_tipo = tb_tipo_instrumento_bonificacion_antiguedad.Value;
                    subrrogancia.Instrumento_vigencia = tb_vigencia_instrumento_bonificacion_antiguedad.Value;

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > 3145728)
                        {
                            Controles.MessageBox.Show(this, "El archivo a subir debe superar los 3mb.", Controles.MessageBox.Tipo_MessageBox.Warning);
                        }
                        else
                        {
                            string path = pathArchivosDisco + ag.Legajo + "\\Bonificación - antiguedad - subrrogancia\\";
                            string fullpath = path + Path.GetFileName(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            subrrogancia.Path_archivo = fullpath;
                            agente_cxt.Legajo_fojas_de_servicio.Pagos_Extras.Add(subrrogancia);

                            cxt.SaveChanges();

                            if (File.Exists(fullpath))
                            {
                                File.Delete(fullpath);
                            }

                            file.SaveAs(fullpath);
                        }

                        var subrrogancias = (from ff in agente_cxt.Legajo_fojas_de_servicio.Pagos_Extras
                                             select new
                                             {
                                                 tipo_instrumento = ff.Instrumento_tipo,
                                                 nro_instrumento = ff.Instrumento_nro,
                                                 anio_instrumento = ff.Instrumento_fecha,
                                                 vigencia_instrumento = ff.Instrumento_vigencia,
                                                 id = ff.Id
                                             }).ToList();

                        gv_subrrogancia_bonificacion_antiguedad.DataSource = subrrogancias;
                        gv_subrrogancia_bonificacion_antiguedad.DataBind();

                        Session["Agente"] = agente_cxt;
                    }
                    else
                    {
                        subrrogancia.Path_archivo = "";
                        agente_cxt.Legajo_fojas_de_servicio.Pagos_Extras.Add(subrrogancia);
                        cxt.SaveChanges();

                        var subrrogancias = (from ff in agente_cxt.Legajo_fojas_de_servicio.Pagos_Extras
                                             select new
                                             {
                                                 tipo_instrumento = ff.Instrumento_tipo,
                                                 nro_instrumento = ff.Instrumento_nro,
                                                 anio_instrumento = ff.Instrumento_fecha,
                                                 vigencia_instrumento = ff.Instrumento_vigencia,
                                                 id = ff.Id
                                             }).ToList();

                        gv_subrrogancia_bonificacion_antiguedad.DataSource = subrrogancias;
                        gv_subrrogancia_bonificacion_antiguedad.DataBind();

                        Session["Agente"] = agente_cxt;
                    }

                }

            }
            else
            {
                Controles.MessageBox.Show(this, "Todos los campos son obligatorios excepto el archivo digitalizado.-", Controles.MessageBox.Tipo_MessageBox.Warning);
            }
        }

        protected void btn_eliminar_novedad_carrera_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_carrera_administrativa novedad = cxt.Legajos_carreras_administrativas.First(hh => hh.Id == id);

                string path = pathArchivosDisco + novedad.Legajo_foja_de_servicio.Agente.Legajo + "\\Carrera administrativa\\" + Path.GetFileName(novedad.Path_archivo);

                cxt.Legajos_carreras_administrativas.DeleteObject(novedad);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var carrera = (from cc in agente_cxt.Legajo_fojas_de_servicio.Carrera_administrativa
                               select new
                               {
                                   id = cc.Id,
                                   fecha = cc.Fecha_instrumento,
                                   tipo_novedad = cc.Tipo_novedad,
                                   tipo_instrumento = cc.Tipo_instrumento,
                                   nro_instrumento = cc.Nro_instrumento,
                                   cargo = cc.Cargo,
                                   grupo = cc.Grupo,
                                   apartado = cc.Apartado
                               }).ToList();
                gv_carrera_administrativa.DataSource = carrera;
                gv_carrera_administrativa.DataBind();

                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_eliminar_afectacion_designacion_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_afectacion_designacion item_a_eliminar = cxt.Legajos_afectaciones_designaciones.First(hh => hh.Id == id);

                string path = pathArchivosDisco + item_a_eliminar.Legajo_foja_de_servicio.Agente.Legajo + "\\Afectaciones - designaciones\\" + Path.GetFileName(item_a_eliminar.Path_archivo);

                cxt.Legajos_afectaciones_designaciones.DeleteObject(item_a_eliminar);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var afectaciones = (from ff in agente_cxt.Legajo_fojas_de_servicio.Afectaciones
                                    select new
                                    {
                                        tipo_instrumento = ff.Instrumento_tipo,
                                        nro_instrumento = ff.Instrumento_numero,
                                        anio_instrumento = ff.Instrumento_fecha.Year,
                                        descripcion = ff.Descripcion,
                                        id = ff.Id
                                    }).ToList();
                gv_afectacion_designacion_asignacion.DataSource = afectaciones;
                gv_afectacion_designacion_asignacion.DataBind();

                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_eliminar_subrogancia_bonificacion_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_pago_subrrogancia_bonificacion_antiguedad item_a_eliminar = cxt.Legajo_pagos_subrrogancia_bonificacion_antiguedad.First(hh => hh.Id == id);

                string path = pathArchivosDisco + item_a_eliminar.Legajo_fojas_de_servicio.Agente.Legajo + "\\Bonificación - antiguedad - subrrogancia\\" + Path.GetFileName(item_a_eliminar.Path_archivo);

                cxt.Legajo_pagos_subrrogancia_bonificacion_antiguedad.DeleteObject(item_a_eliminar);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var subrrogancias = (from ff in agente_cxt.Legajo_fojas_de_servicio.Pagos_Extras
                                     select new
                                     {
                                         tipo_instrumento = ff.Instrumento_tipo,
                                         nro_instrumento = ff.Instrumento_nro,
                                         anio_instrumento = ff.Instrumento_fecha,
                                         vigencia_instrumento = ff.Instrumento_vigencia,
                                         id = ff.Id
                                     }).ToList();

                gv_subrrogancia_bonificacion_antiguedad.DataSource = subrrogancias;
                gv_subrrogancia_bonificacion_antiguedad.DataBind();

                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_eliminar_otros_eventos_Click(object sender, ImageClickEventArgs e)
        {
            Agente ag = Session["Agente"] as Agente;
            using (var cxt = new Model1Container())
            {
                Agente agente_cxt = cxt.Agentes.First(aa => aa.Id == ag.Id);

                int id = Convert.ToInt32(((ImageButton)sender).CommandArgument);

                Legajo_otro_evento item_a_eliminar = cxt.Legajo_otros_eventos.First(hh => hh.Id == id);

                string path = pathArchivosDisco + item_a_eliminar.Legajo_fojas_de_servicio.Agente.Legajo + "\\Otros eventos\\" + Path.GetFileName(item_a_eliminar.Path_archivo);

                cxt.Legajo_otros_eventos.DeleteObject(item_a_eliminar);

                cxt.SaveChanges();

                try
                {
                    File.Delete(path);
                }
                catch { }

                var otros_eventos = (from ff in agente_cxt.Legajo_fojas_de_servicio.Otros_eventos
                                     select new
                                     {
                                         descripcion = ff.Descripcion,
                                         lugar = ff.Lugar,
                                         fecha = ff.Fecha,
                                         id = ff.Id
                                     }).ToList();
                gv_otros_eventos.DataSource = otros_eventos;
                gv_otros_eventos.DataBind();

                Session["Agente"] = agente_cxt;
            }
        }

        protected void btn_ver_novedad_carrera_Click(object sender, ImageClickEventArgs e)
        {
            int idItem_buscado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_carrera_administrativa itemBuscado = cxt.Legajos_carreras_administrativas.First(tt => tt.Id == idItem_buscado);

                string path = pathArchivosDisco + itemBuscado.Legajo_foja_de_servicio.Agente.Legajo + "\\Carrera administrativa\\" + Path.GetFileName(itemBuscado.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

            }
        }

        protected void btn_ver_otros_eventos_Click(object sender, ImageClickEventArgs e)
        {
            int idItem_buscado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_otro_evento itemBuscado = cxt.Legajo_otros_eventos.First(tt => tt.Id == idItem_buscado);

                string path = pathArchivosDisco + itemBuscado.Legajo_fojas_de_servicio.Agente.Legajo + "\\Otros eventos\\" + Path.GetFileName(itemBuscado.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

            }
        }

        protected void btn_ver_subrrogancia_bonificacion_Click(object sender, ImageClickEventArgs e)
        {
            int idItem_buscado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_pago_subrrogancia_bonificacion_antiguedad itemBuscado = cxt.Legajo_pagos_subrrogancia_bonificacion_antiguedad.First(tt => tt.Id == idItem_buscado);

                string path = pathArchivosDisco + itemBuscado.Legajo_fojas_de_servicio.Agente.Legajo + "\\Bonificación - antiguedad - subrrogancia\\" + Path.GetFileName(itemBuscado.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

            }
        }

        protected void btn_ver_afectacion_designacion_Click(object sender, ImageClickEventArgs e)
        {
            int idItem_buscado = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            using (var cxt = new Model1Container())
            {
                Legajo_afectacion_designacion itemBuscado = cxt.Legajos_afectaciones_designaciones.First(tt => tt.Id == idItem_buscado);

                string path = pathArchivosDisco + itemBuscado.Legajo_foja_de_servicio.Agente.Legajo + "\\Afectaciones - designaciones\\" + Path.GetFileName(itemBuscado.Path_archivo);

                try
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    Session["Bytes"] = bytes;

                    string script = "<script type='text/javascript'>window.open('Reportes/ReportePDF.aspx');window.navigate(document.URL);</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "VentanaPadre", script);
                }
                catch (Exception)
                {
                    Controles.MessageBox.Show(this, "No se encuentra cargado el digital.-", Controles.MessageBox.Tipo_MessageBox.Warning);
                }

            }
        }

        #endregion

       
    }
}