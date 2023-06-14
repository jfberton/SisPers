
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/01/2023 12:43:56
-- Generated from EDMX file: C:\Users\berton\Documents\Desarrollo\ATP\antes del 2020\SisPer\SisPer\Aplicativo\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BackupPersonal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AgenteAbrioDia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AbrioDias] DROP CONSTRAINT [FK_AgenteAbrioDia];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteAgente_legajo_datos_laborales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajos_datos_laborales] DROP CONSTRAINT [FK_AgenteAgente_legajo_datos_laborales];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteAgente_legajo_datos_personales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajos_datos_personales] DROP CONSTRAINT [FK_AgenteAgente_legajo_datos_personales];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteAgente1214]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes1214] DROP CONSTRAINT [FK_AgenteAgente1214];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteAgente12142]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes1214] DROP CONSTRAINT [FK_AgenteAgente12142];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteAgente12143]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes1214] DROP CONSTRAINT [FK_AgenteAgente12143];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteBonificacionOtorgada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BonificacionesOtorgadas] DROP CONSTRAINT [FK_AgenteBonificacionOtorgada];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteCambioClave]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CambiosDeClave] DROP CONSTRAINT [FK_AgenteCambioClave];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteCambioPendiente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CambiosPendientes] DROP CONSTRAINT [FK_AgenteCambioPendiente];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteCierreMensual]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CierreMensual] DROP CONSTRAINT [FK_AgenteCierreMensual];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteCorteLicenciaAnual]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CorteLicencia] DROP CONSTRAINT [FK_AgenteCorteLicenciaAnual];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteCorteLicenciaAnual1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CorteLicencia] DROP CONSTRAINT [FK_AgenteCorteLicenciaAnual1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteDestinatario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Destinatarios] DROP CONSTRAINT [FK_AgenteDestinatario];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteDiaAutorizadoRemoto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiasAutorizadosRemoto] DROP CONSTRAINT [FK_AgenteDiaAutorizadoRemoto];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteDiaUsufructado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiasUsufructuados] DROP CONSTRAINT [FK_AgenteDiaUsufructado];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteDiaUsufructado1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiasUsufructuados] DROP CONSTRAINT [FK_AgenteDiaUsufructado1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteEntradaSalida]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntradasSalidas] DROP CONSTRAINT [FK_AgenteEntradaSalida];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteEntradaSalida1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntradasSalidas] DROP CONSTRAINT [FK_AgenteEntradaSalida1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteEstadoAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EstadosAgente] DROP CONSTRAINT [FK_AgenteEstadoAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteEstadoAgente1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EstadosAgente] DROP CONSTRAINT [FK_AgenteEstadoAgente1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteFormulario1214]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Formularios1214] DROP CONSTRAINT [FK_AgenteFormulario1214];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteFranco]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Francos] DROP CONSTRAINT [FK_AgenteFranco];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteHAAnteriorEliminadas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HAAnteriorEliminadasSet] DROP CONSTRAINT [FK_AgenteHAAnteriorEliminadas];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteHistorialEstadosNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notificacion_Historiales] DROP CONSTRAINT [FK_AgenteHistorialEstadosNotificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteHorarioVespertino]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HorariosVespertinos] DROP CONSTRAINT [FK_AgenteHorarioVespertino];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteHorarioVespertino1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HorariosVespertinos] DROP CONSTRAINT [FK_AgenteHorarioVespertino1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteHorasMesHorarioFlexible]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HorasMesHorarioFlexibles] DROP CONSTRAINT [FK_AgenteHorasMesHorarioFlexible];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteImpresionReporte]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImpresionReportes] DROP CONSTRAINT [FK_AgenteImpresionReporte];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteLegajo_fojas_de_servicio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_fojas_de_servicio] DROP CONSTRAINT [FK_AgenteLegajo_fojas_de_servicio];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteLicenciaAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LicenciasAgentes] DROP CONSTRAINT [FK_AgenteLicenciaAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMarcacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marcaciones] DROP CONSTRAINT [FK_AgenteMarcacion];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMemo_17_DDJJ]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Memo_17_DDJJs] DROP CONSTRAINT [FK_AgenteMemo_17_DDJJ];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMensaje]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Mensajes] DROP CONSTRAINT [FK_AgenteMensaje];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteModificacion_cierre_mes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modificaciones_cierres_meses] DROP CONSTRAINT [FK_AgenteModificacion_cierre_mes];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMovimientoFranco]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosFrancos] DROP CONSTRAINT [FK_AgenteMovimientoFranco];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMovimientoHora]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosHoras] DROP CONSTRAINT [FK_AgenteMovimientoHora];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMovimientoSobreAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosSobreAgente] DROP CONSTRAINT [FK_AgenteMovimientoSobreAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteMovimientoSobreAgente1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosSobreAgente] DROP CONSTRAINT [FK_AgenteMovimientoSobreAgente1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notificaciones] DROP CONSTRAINT [FK_AgenteNotificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteObservacionGuardia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObservacionesGuardia] DROP CONSTRAINT [FK_AgenteObservacionGuardia];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteReasignacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reasignaciones] DROP CONSTRAINT [FK_AgenteReasignacion];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteResumenDiario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ResumenesDiarios] DROP CONSTRAINT [FK_AgenteResumenDiario];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteResumenDiario1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ResumenesDiarios] DROP CONSTRAINT [FK_AgenteResumenDiario1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteSalida]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Salidas] DROP CONSTRAINT [FK_AgenteSalida];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteSalida1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Salidas] DROP CONSTRAINT [FK_AgenteSalida1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteSesion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sesiones] DROP CONSTRAINT [FK_AgenteSesion];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteSolicitudDeEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolicitudesDeEstado] DROP CONSTRAINT [FK_AgenteSolicitudDeEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteSolicitudDeEstado1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolicitudesDeEstado] DROP CONSTRAINT [FK_AgenteSolicitudDeEstado1];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteTurnoIngresoPermitido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TurnosIngresoPermitido] DROP CONSTRAINT [FK_AgenteTurnoIngresoPermitido];
GO
IF OBJECT_ID(N'[dbo].[FK_AgenteValidacion_email]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Validaciones_email] DROP CONSTRAINT [FK_AgenteValidacion_email];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes] DROP CONSTRAINT [FK_AreaAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Areas] DROP CONSTRAINT [FK_AreaArea];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaAsuetoParcial]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AsuetosParciales] DROP CONSTRAINT [FK_AreaAsuetoParcial];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaReasignacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reasignaciones] DROP CONSTRAINT [FK_AreaReasignacion];
GO
IF OBJECT_ID(N'[dbo].[FK_CierreMensualModificacion_cierre_mes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modificaciones_cierres_meses] DROP CONSTRAINT [FK_CierreMensualModificacion_cierre_mes];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectorioArchivo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Archivos] DROP CONSTRAINT [FK_DirectorioArchivo];
GO
IF OBJECT_ID(N'[dbo].[FK_Estrato1214Formulario1214]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Formularios1214] DROP CONSTRAINT [FK_Estrato1214Formulario1214];
GO
IF OBJECT_ID(N'[dbo].[FK_Formulario1214Agente1214]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes1214] DROP CONSTRAINT [FK_Formulario1214Agente1214];
GO
IF OBJECT_ID(N'[dbo].[FK_FrancoDiaFranco]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiasFrancos] DROP CONSTRAINT [FK_FrancoDiaFranco];
GO
IF OBJECT_ID(N'[dbo].[FK_FrancoMovimientoFranco]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosFrancos] DROP CONSTRAINT [FK_FrancoMovimientoFranco];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_datos_personalesLegajo_conyugue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajos_conyuge] DROP CONSTRAINT [FK_Legajo_datos_personalesLegajo_conyugue];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_datos_personalesLegajo_hijo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_hijos] DROP CONSTRAINT [FK_Legajo_datos_personalesLegajo_hijo];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_datos_personalesLegajo_historial_domicilio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_historial_domicilios] DROP CONSTRAINT [FK_Legajo_datos_personalesLegajo_historial_domicilio];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_datos_personalesLegajo_titulo_certificado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_titulos_certificados] DROP CONSTRAINT [FK_Legajo_datos_personalesLegajo_titulo_certificado];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_foja_de_servicioLegajo_afectacion_designacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajos_afectaciones_designaciones] DROP CONSTRAINT [FK_Legajo_foja_de_servicioLegajo_afectacion_designacion];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_foja_de_servicioLegajo_carrera_administrativa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajos_carreras_administrativas] DROP CONSTRAINT [FK_Legajo_foja_de_servicioLegajo_carrera_administrativa];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_fojas_de_servicioLegajo_otro_evento]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_otros_eventos] DROP CONSTRAINT [FK_Legajo_fojas_de_servicioLegajo_otro_evento];
GO
IF OBJECT_ID(N'[dbo].[FK_Legajo_fojas_de_servicioLegajo_pago_subrrogancia_bonificacion_antiguedad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad] DROP CONSTRAINT [FK_Legajo_fojas_de_servicioLegajo_pago_subrrogancia_bonificacion_antiguedad];
GO
IF OBJECT_ID(N'[dbo].[FK_Memo_17_DDJJMemo_17_DDJJ_Hijo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Memo_17_DDJJ_Hijos] DROP CONSTRAINT [FK_Memo_17_DDJJMemo_17_DDJJ_Hijo];
GO
IF OBJECT_ID(N'[dbo].[FK_MensajeDestinatario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Destinatarios] DROP CONSTRAINT [FK_MensajeDestinatario];
GO
IF OBJECT_ID(N'[dbo].[FK_Notificacion_EstadoNotificacion_Historial]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notificacion_Historiales] DROP CONSTRAINT [FK_Notificacion_EstadoNotificacion_Historial];
GO
IF OBJECT_ID(N'[dbo].[FK_NotificacionHistorialEstadosNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notificacion_Historiales] DROP CONSTRAINT [FK_NotificacionHistorialEstadosNotificacion];
GO
IF OBJECT_ID(N'[dbo].[FK_ResumenDiarioAbrioDia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AbrioDias] DROP CONSTRAINT [FK_ResumenDiarioAbrioDia];
GO
IF OBJECT_ID(N'[dbo].[FK_ResumenDiarioMarcacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marcaciones] DROP CONSTRAINT [FK_ResumenDiarioMarcacion];
GO
IF OBJECT_ID(N'[dbo].[FK_ResumenDiarioMovimientoHora]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosHoras] DROP CONSTRAINT [FK_ResumenDiarioMovimientoHora];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoEstadoAgenteEstadoAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EstadosAgente] DROP CONSTRAINT [FK_TipoEstadoAgenteEstadoAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoEstadoAgenteSolicitudDeEstado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolicitudesDeEstado] DROP CONSTRAINT [FK_TipoEstadoAgenteSolicitudDeEstado];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoHorariosFexibleAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Agentes] DROP CONSTRAINT [FK_TipoHorariosFexibleAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoLicenciaDiaUsufructado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DiasUsufructuados] DROP CONSTRAINT [FK_TipoLicenciaDiaUsufructado];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoLicenciaLicenciaAgente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LicenciasAgentes] DROP CONSTRAINT [FK_TipoLicenciaLicenciaAgente];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoMovimientoHoraMovimientoHora]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovimientosHoras] DROP CONSTRAINT [FK_TipoMovimientoHoraMovimientoHora];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoNotificacionNotificacion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notificaciones] DROP CONSTRAINT [FK_TipoNotificacionNotificacion];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AbrioDias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AbrioDias];
GO
IF OBJECT_ID(N'[dbo].[Agentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agentes];
GO
IF OBJECT_ID(N'[dbo].[Agentes1214]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Agentes1214];
GO
IF OBJECT_ID(N'[dbo].[Archivos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Archivos];
GO
IF OBJECT_ID(N'[dbo].[Areas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Areas];
GO
IF OBJECT_ID(N'[dbo].[AsuetosParciales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AsuetosParciales];
GO
IF OBJECT_ID(N'[dbo].[BonificacionesOtorgadas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BonificacionesOtorgadas];
GO
IF OBJECT_ID(N'[dbo].[CambiosDeClave]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CambiosDeClave];
GO
IF OBJECT_ID(N'[dbo].[CambiosPendientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CambiosPendientes];
GO
IF OBJECT_ID(N'[dbo].[CierreMensual]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CierreMensual];
GO
IF OBJECT_ID(N'[dbo].[CorteLicencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CorteLicencia];
GO
IF OBJECT_ID(N'[dbo].[Destinatarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Destinatarios];
GO
IF OBJECT_ID(N'[dbo].[DiasAutorizadosRemoto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiasAutorizadosRemoto];
GO
IF OBJECT_ID(N'[dbo].[DiasFrancos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiasFrancos];
GO
IF OBJECT_ID(N'[dbo].[DiasProcesados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiasProcesados];
GO
IF OBJECT_ID(N'[dbo].[DiasUsufructuados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiasUsufructuados];
GO
IF OBJECT_ID(N'[dbo].[Directorios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Directorios];
GO
IF OBJECT_ID(N'[dbo].[EntradasSalidas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntradasSalidas];
GO
IF OBJECT_ID(N'[dbo].[EstadosAgente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EstadosAgente];
GO
IF OBJECT_ID(N'[dbo].[Estratos1214]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estratos1214];
GO
IF OBJECT_ID(N'[dbo].[Feriados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feriados];
GO
IF OBJECT_ID(N'[dbo].[Formularios1214]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Formularios1214];
GO
IF OBJECT_ID(N'[dbo].[Francos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Francos];
GO
IF OBJECT_ID(N'[dbo].[HAAnteriorEliminadasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HAAnteriorEliminadasSet];
GO
IF OBJECT_ID(N'[dbo].[HorariosVespertinos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HorariosVespertinos];
GO
IF OBJECT_ID(N'[dbo].[HorasMesHorarioFlexibles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HorasMesHorarioFlexibles];
GO
IF OBJECT_ID(N'[dbo].[ImpresionReportes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImpresionReportes];
GO
IF OBJECT_ID(N'[dbo].[Legajo_fojas_de_servicio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_fojas_de_servicio];
GO
IF OBJECT_ID(N'[dbo].[Legajo_hijos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_hijos];
GO
IF OBJECT_ID(N'[dbo].[Legajo_historial_domicilios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_historial_domicilios];
GO
IF OBJECT_ID(N'[dbo].[Legajo_otros_eventos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_otros_eventos];
GO
IF OBJECT_ID(N'[dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad];
GO
IF OBJECT_ID(N'[dbo].[Legajo_titulos_certificados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajo_titulos_certificados];
GO
IF OBJECT_ID(N'[dbo].[Legajos_afectaciones_designaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajos_afectaciones_designaciones];
GO
IF OBJECT_ID(N'[dbo].[Legajos_carreras_administrativas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajos_carreras_administrativas];
GO
IF OBJECT_ID(N'[dbo].[Legajos_conyuge]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajos_conyuge];
GO
IF OBJECT_ID(N'[dbo].[Legajos_datos_laborales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajos_datos_laborales];
GO
IF OBJECT_ID(N'[dbo].[Legajos_datos_personales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legajos_datos_personales];
GO
IF OBJECT_ID(N'[dbo].[LicenciasAgentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LicenciasAgentes];
GO
IF OBJECT_ID(N'[dbo].[Marcaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marcaciones];
GO
IF OBJECT_ID(N'[dbo].[Memo_17_DDJJ_Hijos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Memo_17_DDJJ_Hijos];
GO
IF OBJECT_ID(N'[dbo].[Memo_17_DDJJs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Memo_17_DDJJs];
GO
IF OBJECT_ID(N'[dbo].[Mensajes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mensajes];
GO
IF OBJECT_ID(N'[dbo].[Modificaciones_cierres_meses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modificaciones_cierres_meses];
GO
IF OBJECT_ID(N'[dbo].[MovimientosFrancos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovimientosFrancos];
GO
IF OBJECT_ID(N'[dbo].[MovimientosHoras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovimientosHoras];
GO
IF OBJECT_ID(N'[dbo].[MovimientosSobreAgente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovimientosSobreAgente];
GO
IF OBJECT_ID(N'[dbo].[Notificacion_Estados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notificacion_Estados];
GO
IF OBJECT_ID(N'[dbo].[Notificacion_Historiales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notificacion_Historiales];
GO
IF OBJECT_ID(N'[dbo].[Notificacion_Tipos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notificacion_Tipos];
GO
IF OBJECT_ID(N'[dbo].[Notificaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notificaciones];
GO
IF OBJECT_ID(N'[dbo].[ObservacionesGuardia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObservacionesGuardia];
GO
IF OBJECT_ID(N'[dbo].[Reasignaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reasignaciones];
GO
IF OBJECT_ID(N'[dbo].[ResumenesDiarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResumenesDiarios];
GO
IF OBJECT_ID(N'[dbo].[Salidas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Salidas];
GO
IF OBJECT_ID(N'[dbo].[Sesiones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sesiones];
GO
IF OBJECT_ID(N'[dbo].[SolicitudesDeEstado]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SolicitudesDeEstado];
GO
IF OBJECT_ID(N'[dbo].[TiposDeLicencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposDeLicencia];
GO
IF OBJECT_ID(N'[dbo].[TiposEstadoAgente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposEstadoAgente];
GO
IF OBJECT_ID(N'[dbo].[TiposHorariosFexibles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposHorariosFexibles];
GO
IF OBJECT_ID(N'[dbo].[TiposMovimientosHora]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposMovimientosHora];
GO
IF OBJECT_ID(N'[dbo].[TurnosIngresoPermitido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TurnosIngresoPermitido];
GO
IF OBJECT_ID(N'[dbo].[Validaciones_email]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Validaciones_email];
GO
IF OBJECT_ID(N'[dbo].[VariablesGlobales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VariablesGlobales];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Agentes'
CREATE TABLE [dbo].[Agentes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApellidoYNombre] nvarchar(max)  NOT NULL,
    [Usr] nvarchar(max)  NOT NULL,
    [Pass] nvarchar(max)  NOT NULL,
    [Legajo] int  NOT NULL,
    [Jefe] bit  NOT NULL,
    [JefeTemporal] bit  NOT NULL,
    [Perfil] int  NOT NULL,
    [AreaId] int  NULL,
    [Estado] int  NOT NULL,
    [HorasAcumuladasAnioActual] nvarchar(max)  NOT NULL,
    [HorasAcumuladasAnioAnterior] nvarchar(max)  NOT NULL,
    [PoseeBonificacion] bit  NOT NULL,
    [HorasBonificacionACubrir] nvarchar(max)  NOT NULL,
    [FechaBaja] datetime  NULL,
    [HoraEntrada] nvarchar(max)  NOT NULL,
    [HoraSalida] nvarchar(max)  NOT NULL,
    [HorarioFlexible] bit  NULL,
    [JefeTemporalHasta] datetime  NULL,
    [TipoHorariosFexibleId] int  NULL,
    [TipoHorarioFlexibleDesde] datetime  NULL,
    [TipoHorarioFlexibleHasta] datetime  NULL
);
GO

-- Creating table 'Areas'
CREATE TABLE [dbo].[Areas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [AreaId] int  NULL,
    [Interior] bit  NULL
);
GO

-- Creating table 'ResumenesDiarios'
CREATE TABLE [dbo].[ResumenesDiarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [Horas] nvarchar(max)  NOT NULL,
    [AgenteId] int  NOT NULL,
    [HEntrada] nvarchar(max)  NOT NULL,
    [HSalida] nvarchar(max)  NOT NULL,
    [HVEnt] nvarchar(max)  NOT NULL,
    [HVSal] nvarchar(max)  NOT NULL,
    [MarcoTardanza] bit  NOT NULL,
    [MarcoProlongJornada] bit  NOT NULL,
    [Inconsistente] bit  NOT NULL,
    [ObservacionInconsistente] nvarchar(max)  NOT NULL,
    [Cerrado] bit  NULL,
    [AcumuloHorasBonificacion] nvarchar(max)  NULL,
    [AcumuloHorasAnioActual] nvarchar(max)  NULL,
    [AcumuloHorasAnioAnterior] nvarchar(max)  NULL,
    [AcumuloHorasMes] nvarchar(max)  NULL,
    [AgenteId1] int  NULL
);
GO

-- Creating table 'MovimientosHoras'
CREATE TABLE [dbo].[MovimientosHoras] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ResumenDiarioId] int  NOT NULL,
    [Horas] nvarchar(max)  NOT NULL,
    [TipoMovimientoHoraId] int  NOT NULL,
    [DescontoDeAcumuladoAnioAnterior] bit  NOT NULL,
    [DescontoDeHorasBonificables] bit  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [AgenteId] int  NOT NULL
);
GO

-- Creating table 'HorariosVespertinos'
CREATE TABLE [dbo].[HorariosVespertinos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [HoraInicio] nvarchar(max)  NOT NULL,
    [HoraFin] nvarchar(max)  NOT NULL,
    [Estado] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [Motivo] nvarchar(max)  NOT NULL,
    [AgenteId1] int  NULL
);
GO

-- Creating table 'Salidas'
CREATE TABLE [dbo].[Salidas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [HoraDesde] nvarchar(max)  NOT NULL,
    [HoraHasta] nvarchar(max)  NULL,
    [Tipo] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgenteId1] int  NULL,
    [Destino] nvarchar(max)  NULL
);
GO

-- Creating table 'Francos'
CREATE TABLE [dbo].[Francos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Estado] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [InfPersSaldoAnioAnt] nvarchar(max)  NULL,
    [InfPersSaldoAnioEnCurso] nvarchar(max)  NULL,
    [FechaSolicitud] datetime  NOT NULL
);
GO

-- Creating table 'DiasFrancos'
CREATE TABLE [dbo].[DiasFrancos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FrancoId] int  NOT NULL,
    [Dia] datetime  NOT NULL
);
GO

-- Creating table 'MovimientosFrancos'
CREATE TABLE [dbo].[MovimientosFrancos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FrancoId] int  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Estado] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [Observacion] nvarchar(max)  NULL
);
GO

-- Creating table 'TiposMovimientosHora'
CREATE TABLE [dbo].[TiposMovimientosHora] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL,
    [Suma] bit  NOT NULL,
    [Manual] bit  NOT NULL
);
GO

-- Creating table 'VariablesGlobales'
CREATE TABLE [dbo].[VariablesGlobales] (
    [UltimaFechaConsulta] datetime  NOT NULL,
    [Id] smallint IDENTITY(1,1) NOT NULL,
    [EnMantenimiento] bit  NOT NULL,
    [UltimoAnioHorasEliminadas] int  NULL
);
GO

-- Creating table 'CierreMensual'
CREATE TABLE [dbo].[CierreMensual] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [HorasAnioAnterior] nvarchar(max)  NOT NULL,
    [HorasAnioActual] nvarchar(max)  NOT NULL,
    [HorasMes] nvarchar(max)  NOT NULL,
    [FechaCierre] datetime  NOT NULL,
    [Mes] int  NOT NULL,
    [Anio] int  NOT NULL,
    [Tiene_que_modificar] bit  NOT NULL,
    [Dias_sin_cerrar] int  NULL
);
GO

-- Creating table 'EstadosAgente'
CREATE TABLE [dbo].[EstadosAgente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [TipoEstadoAgenteId] int  NOT NULL,
    [Dia] datetime  NOT NULL,
    [AgenteId1] int  NOT NULL
);
GO

-- Creating table 'TiposEstadoAgente'
CREATE TABLE [dbo].[TiposEstadoAgente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Estado] nvarchar(max)  NOT NULL,
    [Abreviatura] nvarchar(max)  NOT NULL,
    [MarcaJefe] bit  NOT NULL,
    [MarcaPersonal] bit  NOT NULL,
    [Codigo] int  NULL,
    [OrdenPrioridad] int  NULL
);
GO

-- Creating table 'Feriados'
CREATE TABLE [dbo].[Feriados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [Motivo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BonificacionesOtorgadas'
CREATE TABLE [dbo].[BonificacionesOtorgadas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Mes] int  NOT NULL,
    [Anio] int  NOT NULL,
    [HorasOtorgadas] nvarchar(max)  NOT NULL,
    [HorasAdeudadas] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CambiosPendientes'
CREATE TABLE [dbo].[CambiosPendientes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApyNom] nvarchar(max)  NOT NULL,
    [DNI] nvarchar(max)  NOT NULL,
    [FechaNacimiento] datetime  NOT NULL,
    [Mail] nvarchar(max)  NOT NULL,
    [CUIL] nvarchar(max)  NOT NULL,
    [FichaMedica] nvarchar(max)  NOT NULL,
    [Dom_direccion] nvarchar(max)  NOT NULL,
    [Dom_localidad] nvarchar(max)  NOT NULL,
    [Dom_aclaraciones] nvarchar(max)  NOT NULL,
    [Agente_Id] int  NOT NULL
);
GO

-- Creating table 'ObservacionesGuardia'
CREATE TABLE [dbo].[ObservacionesGuardia] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Observacion] nvarchar(max)  NOT NULL,
    [AgenteId] int  NOT NULL
);
GO

-- Creating table 'EntradasSalidas'
CREATE TABLE [dbo].[EntradasSalidas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Entrada] nvarchar(max)  NOT NULL,
    [Salida] nvarchar(max)  NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgenteId1] int  NOT NULL,
    [CerradoPersonal] bit  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Enviado] bit  NULL
);
GO

-- Creating table 'Reasignaciones'
CREATE TABLE [dbo].[Reasignaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [AreaId] int  NOT NULL,
    [Desde] datetime  NOT NULL,
    [Hasta] datetime  NULL
);
GO

-- Creating table 'Marcaciones'
CREATE TABLE [dbo].[Marcaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Hora] nvarchar(max)  NOT NULL,
    [Manual] bit  NOT NULL,
    [ResumenDiarioId] int  NOT NULL,
    [AgenteId] int  NULL,
    [Anulada] bit  NOT NULL
);
GO

-- Creating table 'DiasProcesados'
CREATE TABLE [dbo].[DiasProcesados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [Cerrado] bit  NOT NULL
);
GO

-- Creating table 'SolicitudesDeEstado'
CREATE TABLE [dbo].[SolicitudesDeEstado] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgenteId1] int  NOT NULL,
    [TipoEstadoAgenteId] int  NOT NULL,
    [FechaDesde] datetime  NOT NULL,
    [FechaHasta] datetime  NOT NULL,
    [Estado] int  NOT NULL,
    [FechaHoraSolicitud] datetime  NULL,
    [TipoEnfermedad] int  NULL,
    [Lugar] nvarchar(max)  NULL,
    [Fam_NomyAp] nvarchar(max)  NULL,
    [Fam_Parentesco] nvarchar(max)  NULL
);
GO

-- Creating table 'HAAnteriorEliminadasSet'
CREATE TABLE [dbo].[HAAnteriorEliminadasSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Anio] int  NOT NULL,
    [Horas] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Archivos'
CREATE TABLE [dbo].[Archivos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Path] nvarchar(max)  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [FechaCreacion] datetime  NOT NULL,
    [DirectorioId] int  NOT NULL
);
GO

-- Creating table 'Directorios'
CREATE TABLE [dbo].[Directorios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Padre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ImpresionReportes'
CREATE TABLE [dbo].[ImpresionReportes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [IP] nvarchar(max)  NOT NULL,
    [TipoReporte] nvarchar(max)  NOT NULL,
    [FechaHora] datetime  NOT NULL,
    [NombreMaquina] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HorasMesHorarioFlexibles'
CREATE TABLE [dbo].[HorasMesHorarioFlexibles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Mes] int  NOT NULL,
    [Anio] int  NOT NULL,
    [HorasAcumuladas] nvarchar(max)  NOT NULL,
    [AcumuladoEnTotalAnual] bit  NULL
);
GO

-- Creating table 'MovimientosSobreAgente'
CREATE TABLE [dbo].[MovimientosSobreAgente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TipoMovimiento] nvarchar(max)  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [IP] nvarchar(max)  NOT NULL,
    [NombreMaquina] nvarchar(max)  NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgenteId1] int  NOT NULL
);
GO

-- Creating table 'TiposDeLicencia'
CREATE TABLE [dbo].[TiposDeLicencia] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LicenciasAgentes'
CREATE TABLE [dbo].[LicenciasAgentes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [TipoLicenciaId] int  NOT NULL,
    [Anio] int  NOT NULL,
    [DiasOtorgados] int  NOT NULL,
    [DiasUsufructuadosIniciales] int  NOT NULL
);
GO

-- Creating table 'DiasUsufructuados'
CREATE TABLE [dbo].[DiasUsufructuados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dia] datetime  NOT NULL,
    [Anio] int  NOT NULL,
    [TipoLicenciaId] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgentePersonalId] int  NOT NULL
);
GO

-- Creating table 'Sesiones'
CREATE TABLE [dbo].[Sesiones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaHoraInicio] datetime  NOT NULL,
    [Maquina] nvarchar(max)  NOT NULL,
    [Ip] nvarchar(max)  NOT NULL,
    [FechaHoraFin] datetime  NULL,
    [AgenteId] int  NOT NULL
);
GO

-- Creating table 'Mensajes'
CREATE TABLE [dbo].[Mensajes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [FechaEnvio] datetime  NOT NULL,
    [Asunto] nvarchar(max)  NOT NULL,
    [Cuerpo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Destinatarios'
CREATE TABLE [dbo].[Destinatarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MensajeId] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [FechaLeido] datetime  NULL,
    [MarcoImportante] bit  NULL,
    [Elimino] nvarchar(max)  NULL,
    [ConfirmoEliminacion] nvarchar(max)  NULL
);
GO

-- Creating table 'Notificaciones'
CREATE TABLE [dbo].[Notificaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TipoNotificacionId] int  NOT NULL,
    [AgenteId] int  NOT NULL,
    [Vencimiento] datetime  NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [ObservacionPendienteRecibir] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Notificacion_Tipos'
CREATE TABLE [dbo].[Notificacion_Tipos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Notificacion_Historiales'
CREATE TABLE [dbo].[Notificacion_Historiales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NotificacionId] int  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [AgenteId] int  NOT NULL,
    [Notificacion_EstadoId] int  NOT NULL
);
GO

-- Creating table 'Notificacion_Estados'
CREATE TABLE [dbo].[Notificacion_Estados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Estado] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CorteLicencia'
CREATE TABLE [dbo].[CorteLicencia] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL,
    [Anio] nvarchar(max)  NOT NULL,
    [Desde] datetime  NOT NULL,
    [NumInstrumento] nvarchar(max)  NOT NULL,
    [AgenteId] int  NOT NULL,
    [AgenteId1] int  NOT NULL
);
GO

-- Creating table 'AbrioDias'
CREATE TABLE [dbo].[AbrioDias] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ResumenDiarioId] int  NOT NULL,
    [AgenteId] int  NOT NULL
);
GO

-- Creating table 'AsuetosParciales'
CREATE TABLE [dbo].[AsuetosParciales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AreaId] int  NOT NULL,
    [Dia] datetime  NOT NULL,
    [HorarioQueModifica] nvarchar(max)  NOT NULL,
    [Hora] nvarchar(max)  NOT NULL,
    [Observacion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Formularios1214'
CREATE TABLE [dbo].[Formularios1214] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desde] datetime  NOT NULL,
    [Hasta] datetime  NOT NULL,
    [Destino] nvarchar(max)  NOT NULL,
    [TareasACumplir] nvarchar(max)  NOT NULL,
    [Estado] int  NOT NULL,
    [Movilidad] int  NOT NULL,
    [Anticipo] int  NOT NULL,
    [MontoAnticipo] decimal(9,2)  NOT NULL,
    [AgenteId] int  NOT NULL,
    [Fuera_provincia] bit  NOT NULL,
    [Usa_chofer] bit  NOT NULL,
    [Estrato1214Id] int  NOT NULL,
    [AnticipoViaticos] decimal(9,2)  NOT NULL,
    [AnticipoMovilidad] decimal(9,2)  NOT NULL,
    [FechaAprobacion] datetime  NULL,
    [Vehiculo_dominio] nvarchar(max)  NOT NULL,
    [Vehiculo_particular_titular] nvarchar(max)  NOT NULL,
    [Vehiculo_particular_tipo_combustible] nvarchar(max)  NOT NULL,
    [Vehiculo_particular_poliza_nro] nvarchar(max)  NOT NULL,
    [Vehiculo_particular_poliza_vigencia] nvarchar(max)  NOT NULL,
    [Vehiculo_particular_poliza_cobertura] nvarchar(max)  NOT NULL,
    [Fecha_confeccion] datetime  NULL,
    [PorcentajeLiquidacionViatico] int  NOT NULL,
    [MovilidadAsociadaADispo] int  NULL,
    [NroDispo] int  NULL
);
GO

-- Creating table 'Agentes1214'
CREATE TABLE [dbo].[Agentes1214] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_Agente] int  NOT NULL,
    [Estado] int  NOT NULL,
    [Formulario1214Id] int  NOT NULL,
    [Id_Area] int  NOT NULL,
    [FechaAprobacion] datetime  NULL,
    [FechaRechazo] datetime  NULL,
    [JefeComicion] bit  NOT NULL,
    [Aprobado_por_agente_id] int  NULL,
    [Rechazado_por_agente_id] int  NULL,
    [Chofer] bit  NOT NULL,
    [NroAnticipo] nvarchar(max)  NULL,
    [NroAnticipoCargadoPor] int  NULL
);
GO

-- Creating table 'Legajos_datos_laborales'
CREATE TABLE [dbo].[Legajos_datos_laborales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaIngresoAminPub] datetime  NOT NULL,
    [FechaIngresoATP] datetime  NOT NULL,
    [AniosAntiguedadReconicidosOtrasPartes] smallint  NOT NULL,
    [MesesAntiguedadReconocidosOtrasPartes] smallint  NOT NULL,
    [FichaMedica] nvarchar(max)  NOT NULL,
    [CUIT] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Grupo] nvarchar(max)  NOT NULL,
    [Apartado] nvarchar(max)  NOT NULL,
    [Cargo] nvarchar(max)  NOT NULL,
    [Situacion_de_revista] nvarchar(max)  NOT NULL,
    [Agente_Id] int  NOT NULL
);
GO

-- Creating table 'Legajos_datos_personales'
CREATE TABLE [dbo].[Legajos_datos_personales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DNI] nvarchar(max)  NOT NULL,
    [Sexo] nvarchar(max)  NOT NULL,
    [FechaNacimiento] datetime  NOT NULL,
    [Domicilio] nvarchar(max)  NOT NULL,
    [DomicilioObservaciones] nvarchar(max)  NOT NULL,
    [EstadoCivil] nvarchar(max)  NOT NULL,
    [Domicilio_localidad] nvarchar(max)  NOT NULL,
    [Agente_Id] int  NOT NULL
);
GO

-- Creating table 'Legajo_historial_domicilios'
CREATE TABLE [dbo].[Legajo_historial_domicilios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Legajo_datos_personalesId] int  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Domicilio] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Legajos_conyuge'
CREATE TABLE [dbo].[Legajos_conyuge] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Apellido_y_nombre] nvarchar(max)  NOT NULL,
    [DNI] nvarchar(max)  NOT NULL,
    [Fecha_de_nacimiento] datetime  NOT NULL,
    [Trabaja] bit  NOT NULL,
    [Lugar_de_trabajo] nvarchar(max)  NOT NULL,
    [Dependencia] nvarchar(max)  NOT NULL,
    [Asignacion_familiar] bit  NOT NULL,
    [Profesion] nvarchar(max)  NOT NULL,
    [Legajo_datos_personales_Id] int  NOT NULL
);
GO

-- Creating table 'Legajo_hijos'
CREATE TABLE [dbo].[Legajo_hijos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Apellido_y_nombre] nvarchar(max)  NOT NULL,
    [DNI] nvarchar(max)  NOT NULL,
    [Fecha_de_nacimiento] datetime  NOT NULL,
    [Observaciones] nvarchar(max)  NOT NULL,
    [Legajo_datos_personalesId] int  NOT NULL
);
GO

-- Creating table 'Legajo_fojas_de_servicio'
CREATE TABLE [dbo].[Legajo_fojas_de_servicio] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Agente_Id] int  NOT NULL
);
GO

-- Creating table 'Legajo_titulos_certificados'
CREATE TABLE [dbo].[Legajo_titulos_certificados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Legajo_datos_personalesId] int  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Lugar_insticucion] nvarchar(max)  NOT NULL,
    [Duracion] nvarchar(max)  NOT NULL,
    [Fecha_emision] datetime  NOT NULL,
    [Tipo_certificado] nvarchar(max)  NOT NULL,
    [Path_archivo] nvarchar(max)  NOT NULL,
    [Nivel] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Legajo_pagos_subrrogancia_bonificacion_antiguedad'
CREATE TABLE [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Instrumento_nro] nvarchar(max)  NOT NULL,
    [Instrumento_tipo] nvarchar(max)  NOT NULL,
    [Instrumento_fecha] datetime  NOT NULL,
    [Instrumento_vigencia] nvarchar(max)  NOT NULL,
    [Path_archivo] nvarchar(max)  NOT NULL,
    [Legajo_fojas_de_servicioId] int  NOT NULL
);
GO

-- Creating table 'Legajo_otros_eventos'
CREATE TABLE [dbo].[Legajo_otros_eventos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Lugar] nvarchar(max)  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Legajo_fojas_de_servicioId] int  NOT NULL,
    [Path_archivo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Legajos_afectaciones_designaciones'
CREATE TABLE [dbo].[Legajos_afectaciones_designaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Instrumento_tipo] nvarchar(max)  NOT NULL,
    [Instrumento_numero] nvarchar(max)  NOT NULL,
    [Instrumento_fecha] datetime  NOT NULL,
    [Legajo_foja_de_servicioId] int  NOT NULL,
    [Path_archivo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Modificaciones_cierres_meses'
CREATE TABLE [dbo].[Modificaciones_cierres_meses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fecha] datetime  NOT NULL,
    [AgenteId_modificacion] int  NOT NULL,
    [CierreMensualId] int  NOT NULL,
    [HoraAnioAnterior] nvarchar(max)  NOT NULL,
    [HoraAnioActual] nvarchar(max)  NOT NULL,
    [HorasMes] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Legajos_carreras_administrativas'
CREATE TABLE [dbo].[Legajos_carreras_administrativas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo_novedad] nvarchar(max)  NOT NULL,
    [Nro_instrumento] nvarchar(max)  NOT NULL,
    [Tipo_instrumento] nvarchar(max)  NOT NULL,
    [Fecha_instrumento] datetime  NOT NULL,
    [Path_archivo] nvarchar(max)  NOT NULL,
    [Legajo_foja_de_servicioId] int  NOT NULL,
    [Cargo] nvarchar(max)  NOT NULL,
    [Grupo] nvarchar(max)  NOT NULL,
    [Apartado] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Validaciones_email'
CREATE TABLE [dbo].[Validaciones_email] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Clave] nvarchar(max)  NOT NULL,
    [Mail] nvarchar(max)  NOT NULL,
    [Fecha_envio] datetime  NOT NULL,
    [Fecha_validado] datetime  NULL
);
GO

-- Creating table 'CambiosDeClave'
CREATE TABLE [dbo].[CambiosDeClave] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [FechaSolicitud] datetime  NOT NULL,
    [FechaAceptacion] datetime  NULL,
    [Guid] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Memo_17_DDJJs'
CREATE TABLE [dbo].[Memo_17_DDJJs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [fecha_modificacion] datetime  NOT NULL,
    [dp_apellido_y_nombre] nvarchar(max)  NOT NULL,
    [dp_nacionalidad] nvarchar(max)  NOT NULL,
    [dp_nativo_naturalizado] nvarchar(max)  NOT NULL,
    [dp_nacimiento_lugar] nvarchar(max)  NOT NULL,
    [dp_nacimiento_fecha] datetime  NOT NULL,
    [dp_clase] nvarchar(max)  NOT NULL,
    [dp_dni] nvarchar(max)  NOT NULL,
    [dp_estado_civil] nvarchar(max)  NOT NULL,
    [dp_domicilio_direccion] nvarchar(max)  NOT NULL,
    [dp_domicilio_barrio] nvarchar(max)  NOT NULL,
    [dp_domicilio_localidad] nvarchar(max)  NOT NULL,
    [dp_domicilio_provincia] nvarchar(max)  NOT NULL,
    [dp_domicilio_codpost] nvarchar(max)  NOT NULL,
    [dp_profesion] nvarchar(max)  NOT NULL,
    [dp_instruccion] nvarchar(max)  NOT NULL,
    [dp_conyugue_apellido_y_nombre] nvarchar(max)  NOT NULL,
    [dp_conyugue_nacionalidad] nvarchar(max)  NOT NULL,
    [dp_padre_apellido_y_nombre] nvarchar(max)  NOT NULL,
    [dp_padre_nacionalidad] nvarchar(max)  NOT NULL,
    [dp_padre_vive] bit  NOT NULL,
    [dp_madre_apellido_y_nombre] nvarchar(max)  NOT NULL,
    [dp_madre_nacionalidad] nvarchar(max)  NOT NULL,
    [dp_madre_vive] bit  NOT NULL,
    [aa_cargo] nvarchar(max)  NOT NULL,
    [aa_contrato_obra_ingreso] datetime  NOT NULL,
    [aa_contrato_obra_instrumento] nvarchar(max)  NOT NULL,
    [aa_contrato_serv_ingreso] datetime  NOT NULL,
    [aa_contrato_serv_instrumento] nvarchar(max)  NOT NULL,
    [aa_nombramiento_ingreso] datetime  NOT NULL,
    [aa_nombramiento_instrumento] nvarchar(max)  NOT NULL,
    [ca_en_admin_nacional] nvarchar(max)  NOT NULL,
    [ca_en_admin_provincial] nvarchar(max)  NOT NULL,
    [ca_privados] nvarchar(max)  NOT NULL,
    [ca_otros_antecedentes] nvarchar(max)  NOT NULL,
    [aa_funcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Memo_17_DDJJ_Hijos'
CREATE TABLE [dbo].[Memo_17_DDJJ_Hijos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Memo_17_DDJJ_Id] int  NOT NULL,
    [dp_hijo_apellido_y_nombre] nvarchar(max)  NOT NULL,
    [dp_hijo_fecha_nacimiento] datetime  NOT NULL
);
GO

-- Creating table 'TiposHorariosFexibles'
CREATE TABLE [dbo].[TiposHorariosFexibles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL,
    [Hentrada] nvarchar(5)  NULL,
    [Hsalida] nvarchar(5)  NULL,
    [Hjornada] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Estratos1214'
CREATE TABLE [dbo].[Estratos1214] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Estrato] nvarchar(max)  NOT NULL,
    [ImpDentroProv] decimal(9,2)  NOT NULL,
    [ImpFueraProv] decimal(9,2)  NOT NULL
);
GO

-- Creating table 'DiasAutorizadosRemoto'
CREATE TABLE [dbo].[DiasAutorizadosRemoto] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Dia] datetime  NOT NULL
);
GO

-- Creating table 'TurnosIngresoPermitido'
CREATE TABLE [dbo].[TurnosIngresoPermitido] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AgenteId] int  NOT NULL,
    [Dia] datetime  NOT NULL,
    [Turno] nchar(2)  NOT NULL,
    [Desde] nchar(5)  NOT NULL,
    [Hasta] nchar(5)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Agentes'
ALTER TABLE [dbo].[Agentes]
ADD CONSTRAINT [PK_Agentes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [PK_Areas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResumenesDiarios'
ALTER TABLE [dbo].[ResumenesDiarios]
ADD CONSTRAINT [PK_ResumenesDiarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MovimientosHoras'
ALTER TABLE [dbo].[MovimientosHoras]
ADD CONSTRAINT [PK_MovimientosHoras]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HorariosVespertinos'
ALTER TABLE [dbo].[HorariosVespertinos]
ADD CONSTRAINT [PK_HorariosVespertinos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Salidas'
ALTER TABLE [dbo].[Salidas]
ADD CONSTRAINT [PK_Salidas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Francos'
ALTER TABLE [dbo].[Francos]
ADD CONSTRAINT [PK_Francos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiasFrancos'
ALTER TABLE [dbo].[DiasFrancos]
ADD CONSTRAINT [PK_DiasFrancos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MovimientosFrancos'
ALTER TABLE [dbo].[MovimientosFrancos]
ADD CONSTRAINT [PK_MovimientosFrancos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposMovimientosHora'
ALTER TABLE [dbo].[TiposMovimientosHora]
ADD CONSTRAINT [PK_TiposMovimientosHora]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VariablesGlobales'
ALTER TABLE [dbo].[VariablesGlobales]
ADD CONSTRAINT [PK_VariablesGlobales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CierreMensual'
ALTER TABLE [dbo].[CierreMensual]
ADD CONSTRAINT [PK_CierreMensual]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EstadosAgente'
ALTER TABLE [dbo].[EstadosAgente]
ADD CONSTRAINT [PK_EstadosAgente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposEstadoAgente'
ALTER TABLE [dbo].[TiposEstadoAgente]
ADD CONSTRAINT [PK_TiposEstadoAgente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Feriados'
ALTER TABLE [dbo].[Feriados]
ADD CONSTRAINT [PK_Feriados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BonificacionesOtorgadas'
ALTER TABLE [dbo].[BonificacionesOtorgadas]
ADD CONSTRAINT [PK_BonificacionesOtorgadas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CambiosPendientes'
ALTER TABLE [dbo].[CambiosPendientes]
ADD CONSTRAINT [PK_CambiosPendientes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ObservacionesGuardia'
ALTER TABLE [dbo].[ObservacionesGuardia]
ADD CONSTRAINT [PK_ObservacionesGuardia]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntradasSalidas'
ALTER TABLE [dbo].[EntradasSalidas]
ADD CONSTRAINT [PK_EntradasSalidas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reasignaciones'
ALTER TABLE [dbo].[Reasignaciones]
ADD CONSTRAINT [PK_Reasignaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Marcaciones'
ALTER TABLE [dbo].[Marcaciones]
ADD CONSTRAINT [PK_Marcaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiasProcesados'
ALTER TABLE [dbo].[DiasProcesados]
ADD CONSTRAINT [PK_DiasProcesados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SolicitudesDeEstado'
ALTER TABLE [dbo].[SolicitudesDeEstado]
ADD CONSTRAINT [PK_SolicitudesDeEstado]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HAAnteriorEliminadasSet'
ALTER TABLE [dbo].[HAAnteriorEliminadasSet]
ADD CONSTRAINT [PK_HAAnteriorEliminadasSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Archivos'
ALTER TABLE [dbo].[Archivos]
ADD CONSTRAINT [PK_Archivos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Directorios'
ALTER TABLE [dbo].[Directorios]
ADD CONSTRAINT [PK_Directorios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ImpresionReportes'
ALTER TABLE [dbo].[ImpresionReportes]
ADD CONSTRAINT [PK_ImpresionReportes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HorasMesHorarioFlexibles'
ALTER TABLE [dbo].[HorasMesHorarioFlexibles]
ADD CONSTRAINT [PK_HorasMesHorarioFlexibles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MovimientosSobreAgente'
ALTER TABLE [dbo].[MovimientosSobreAgente]
ADD CONSTRAINT [PK_MovimientosSobreAgente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposDeLicencia'
ALTER TABLE [dbo].[TiposDeLicencia]
ADD CONSTRAINT [PK_TiposDeLicencia]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LicenciasAgentes'
ALTER TABLE [dbo].[LicenciasAgentes]
ADD CONSTRAINT [PK_LicenciasAgentes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiasUsufructuados'
ALTER TABLE [dbo].[DiasUsufructuados]
ADD CONSTRAINT [PK_DiasUsufructuados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sesiones'
ALTER TABLE [dbo].[Sesiones]
ADD CONSTRAINT [PK_Sesiones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Mensajes'
ALTER TABLE [dbo].[Mensajes]
ADD CONSTRAINT [PK_Mensajes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Destinatarios'
ALTER TABLE [dbo].[Destinatarios]
ADD CONSTRAINT [PK_Destinatarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notificaciones'
ALTER TABLE [dbo].[Notificaciones]
ADD CONSTRAINT [PK_Notificaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notificacion_Tipos'
ALTER TABLE [dbo].[Notificacion_Tipos]
ADD CONSTRAINT [PK_Notificacion_Tipos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notificacion_Historiales'
ALTER TABLE [dbo].[Notificacion_Historiales]
ADD CONSTRAINT [PK_Notificacion_Historiales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notificacion_Estados'
ALTER TABLE [dbo].[Notificacion_Estados]
ADD CONSTRAINT [PK_Notificacion_Estados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CorteLicencia'
ALTER TABLE [dbo].[CorteLicencia]
ADD CONSTRAINT [PK_CorteLicencia]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AbrioDias'
ALTER TABLE [dbo].[AbrioDias]
ADD CONSTRAINT [PK_AbrioDias]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AsuetosParciales'
ALTER TABLE [dbo].[AsuetosParciales]
ADD CONSTRAINT [PK_AsuetosParciales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Formularios1214'
ALTER TABLE [dbo].[Formularios1214]
ADD CONSTRAINT [PK_Formularios1214]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Agentes1214'
ALTER TABLE [dbo].[Agentes1214]
ADD CONSTRAINT [PK_Agentes1214]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajos_datos_laborales'
ALTER TABLE [dbo].[Legajos_datos_laborales]
ADD CONSTRAINT [PK_Legajos_datos_laborales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajos_datos_personales'
ALTER TABLE [dbo].[Legajos_datos_personales]
ADD CONSTRAINT [PK_Legajos_datos_personales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_historial_domicilios'
ALTER TABLE [dbo].[Legajo_historial_domicilios]
ADD CONSTRAINT [PK_Legajo_historial_domicilios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajos_conyuge'
ALTER TABLE [dbo].[Legajos_conyuge]
ADD CONSTRAINT [PK_Legajos_conyuge]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_hijos'
ALTER TABLE [dbo].[Legajo_hijos]
ADD CONSTRAINT [PK_Legajo_hijos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_fojas_de_servicio'
ALTER TABLE [dbo].[Legajo_fojas_de_servicio]
ADD CONSTRAINT [PK_Legajo_fojas_de_servicio]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_titulos_certificados'
ALTER TABLE [dbo].[Legajo_titulos_certificados]
ADD CONSTRAINT [PK_Legajo_titulos_certificados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_pagos_subrrogancia_bonificacion_antiguedad'
ALTER TABLE [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad]
ADD CONSTRAINT [PK_Legajo_pagos_subrrogancia_bonificacion_antiguedad]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajo_otros_eventos'
ALTER TABLE [dbo].[Legajo_otros_eventos]
ADD CONSTRAINT [PK_Legajo_otros_eventos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajos_afectaciones_designaciones'
ALTER TABLE [dbo].[Legajos_afectaciones_designaciones]
ADD CONSTRAINT [PK_Legajos_afectaciones_designaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Modificaciones_cierres_meses'
ALTER TABLE [dbo].[Modificaciones_cierres_meses]
ADD CONSTRAINT [PK_Modificaciones_cierres_meses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Legajos_carreras_administrativas'
ALTER TABLE [dbo].[Legajos_carreras_administrativas]
ADD CONSTRAINT [PK_Legajos_carreras_administrativas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Validaciones_email'
ALTER TABLE [dbo].[Validaciones_email]
ADD CONSTRAINT [PK_Validaciones_email]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CambiosDeClave'
ALTER TABLE [dbo].[CambiosDeClave]
ADD CONSTRAINT [PK_CambiosDeClave]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Memo_17_DDJJs'
ALTER TABLE [dbo].[Memo_17_DDJJs]
ADD CONSTRAINT [PK_Memo_17_DDJJs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Memo_17_DDJJ_Hijos'
ALTER TABLE [dbo].[Memo_17_DDJJ_Hijos]
ADD CONSTRAINT [PK_Memo_17_DDJJ_Hijos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposHorariosFexibles'
ALTER TABLE [dbo].[TiposHorariosFexibles]
ADD CONSTRAINT [PK_TiposHorariosFexibles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Estratos1214'
ALTER TABLE [dbo].[Estratos1214]
ADD CONSTRAINT [PK_Estratos1214]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DiasAutorizadosRemoto'
ALTER TABLE [dbo].[DiasAutorizadosRemoto]
ADD CONSTRAINT [PK_DiasAutorizadosRemoto]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TurnosIngresoPermitido'
ALTER TABLE [dbo].[TurnosIngresoPermitido]
ADD CONSTRAINT [PK_TurnosIngresoPermitido]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AgenteId] in table 'ResumenesDiarios'
ALTER TABLE [dbo].[ResumenesDiarios]
ADD CONSTRAINT [FK_AgenteResumenDiario]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteResumenDiario'
CREATE INDEX [IX_FK_AgenteResumenDiario]
ON [dbo].[ResumenesDiarios]
    ([AgenteId]);
GO

-- Creating foreign key on [ResumenDiarioId] in table 'MovimientosHoras'
ALTER TABLE [dbo].[MovimientosHoras]
ADD CONSTRAINT [FK_ResumenDiarioMovimientoHora]
    FOREIGN KEY ([ResumenDiarioId])
    REFERENCES [dbo].[ResumenesDiarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResumenDiarioMovimientoHora'
CREATE INDEX [IX_FK_ResumenDiarioMovimientoHora]
ON [dbo].[MovimientosHoras]
    ([ResumenDiarioId]);
GO

-- Creating foreign key on [AgenteId] in table 'HorariosVespertinos'
ALTER TABLE [dbo].[HorariosVespertinos]
ADD CONSTRAINT [FK_AgenteHorarioVespertino]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteHorarioVespertino'
CREATE INDEX [IX_FK_AgenteHorarioVespertino]
ON [dbo].[HorariosVespertinos]
    ([AgenteId]);
GO

-- Creating foreign key on [FrancoId] in table 'DiasFrancos'
ALTER TABLE [dbo].[DiasFrancos]
ADD CONSTRAINT [FK_FrancoDiaFranco]
    FOREIGN KEY ([FrancoId])
    REFERENCES [dbo].[Francos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrancoDiaFranco'
CREATE INDEX [IX_FK_FrancoDiaFranco]
ON [dbo].[DiasFrancos]
    ([FrancoId]);
GO

-- Creating foreign key on [FrancoId] in table 'MovimientosFrancos'
ALTER TABLE [dbo].[MovimientosFrancos]
ADD CONSTRAINT [FK_FrancoMovimientoFranco]
    FOREIGN KEY ([FrancoId])
    REFERENCES [dbo].[Francos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrancoMovimientoFranco'
CREATE INDEX [IX_FK_FrancoMovimientoFranco]
ON [dbo].[MovimientosFrancos]
    ([FrancoId]);
GO

-- Creating foreign key on [AgenteId] in table 'Francos'
ALTER TABLE [dbo].[Francos]
ADD CONSTRAINT [FK_AgenteFranco]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteFranco'
CREATE INDEX [IX_FK_AgenteFranco]
ON [dbo].[Francos]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'MovimientosFrancos'
ALTER TABLE [dbo].[MovimientosFrancos]
ADD CONSTRAINT [FK_AgenteMovimientoFranco]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMovimientoFranco'
CREATE INDEX [IX_FK_AgenteMovimientoFranco]
ON [dbo].[MovimientosFrancos]
    ([AgenteId]);
GO

-- Creating foreign key on [TipoMovimientoHoraId] in table 'MovimientosHoras'
ALTER TABLE [dbo].[MovimientosHoras]
ADD CONSTRAINT [FK_TipoMovimientoHoraMovimientoHora]
    FOREIGN KEY ([TipoMovimientoHoraId])
    REFERENCES [dbo].[TiposMovimientosHora]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoMovimientoHoraMovimientoHora'
CREATE INDEX [IX_FK_TipoMovimientoHoraMovimientoHora]
ON [dbo].[MovimientosHoras]
    ([TipoMovimientoHoraId]);
GO

-- Creating foreign key on [AreaId] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [FK_AreaArea]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaArea'
CREATE INDEX [IX_FK_AreaArea]
ON [dbo].[Areas]
    ([AreaId]);
GO

-- Creating foreign key on [AreaId] in table 'Agentes'
ALTER TABLE [dbo].[Agentes]
ADD CONSTRAINT [FK_AreaAgente]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaAgente'
CREATE INDEX [IX_FK_AreaAgente]
ON [dbo].[Agentes]
    ([AreaId]);
GO

-- Creating foreign key on [AgenteId] in table 'Salidas'
ALTER TABLE [dbo].[Salidas]
ADD CONSTRAINT [FK_AgenteSalida]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteSalida'
CREATE INDEX [IX_FK_AgenteSalida]
ON [dbo].[Salidas]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'Salidas'
ALTER TABLE [dbo].[Salidas]
ADD CONSTRAINT [FK_AgenteSalida1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteSalida1'
CREATE INDEX [IX_FK_AgenteSalida1]
ON [dbo].[Salidas]
    ([AgenteId1]);
GO

-- Creating foreign key on [AgenteId1] in table 'HorariosVespertinos'
ALTER TABLE [dbo].[HorariosVespertinos]
ADD CONSTRAINT [FK_AgenteHorarioVespertino1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteHorarioVespertino1'
CREATE INDEX [IX_FK_AgenteHorarioVespertino1]
ON [dbo].[HorariosVespertinos]
    ([AgenteId1]);
GO

-- Creating foreign key on [AgenteId] in table 'CierreMensual'
ALTER TABLE [dbo].[CierreMensual]
ADD CONSTRAINT [FK_AgenteCierreMensual]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteCierreMensual'
CREATE INDEX [IX_FK_AgenteCierreMensual]
ON [dbo].[CierreMensual]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'MovimientosHoras'
ALTER TABLE [dbo].[MovimientosHoras]
ADD CONSTRAINT [FK_AgenteMovimientoHora]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMovimientoHora'
CREATE INDEX [IX_FK_AgenteMovimientoHora]
ON [dbo].[MovimientosHoras]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'EstadosAgente'
ALTER TABLE [dbo].[EstadosAgente]
ADD CONSTRAINT [FK_AgenteEstadoAgente]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteEstadoAgente'
CREATE INDEX [IX_FK_AgenteEstadoAgente]
ON [dbo].[EstadosAgente]
    ([AgenteId]);
GO

-- Creating foreign key on [TipoEstadoAgenteId] in table 'EstadosAgente'
ALTER TABLE [dbo].[EstadosAgente]
ADD CONSTRAINT [FK_TipoEstadoAgenteEstadoAgente]
    FOREIGN KEY ([TipoEstadoAgenteId])
    REFERENCES [dbo].[TiposEstadoAgente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoEstadoAgenteEstadoAgente'
CREATE INDEX [IX_FK_TipoEstadoAgenteEstadoAgente]
ON [dbo].[EstadosAgente]
    ([TipoEstadoAgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'EstadosAgente'
ALTER TABLE [dbo].[EstadosAgente]
ADD CONSTRAINT [FK_AgenteEstadoAgente1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteEstadoAgente1'
CREATE INDEX [IX_FK_AgenteEstadoAgente1]
ON [dbo].[EstadosAgente]
    ([AgenteId1]);
GO

-- Creating foreign key on [AgenteId] in table 'BonificacionesOtorgadas'
ALTER TABLE [dbo].[BonificacionesOtorgadas]
ADD CONSTRAINT [FK_AgenteBonificacionOtorgada]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteBonificacionOtorgada'
CREATE INDEX [IX_FK_AgenteBonificacionOtorgada]
ON [dbo].[BonificacionesOtorgadas]
    ([AgenteId]);
GO

-- Creating foreign key on [Agente_Id] in table 'CambiosPendientes'
ALTER TABLE [dbo].[CambiosPendientes]
ADD CONSTRAINT [FK_AgenteCambioPendiente]
    FOREIGN KEY ([Agente_Id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteCambioPendiente'
CREATE INDEX [IX_FK_AgenteCambioPendiente]
ON [dbo].[CambiosPendientes]
    ([Agente_Id]);
GO

-- Creating foreign key on [AgenteId] in table 'ObservacionesGuardia'
ALTER TABLE [dbo].[ObservacionesGuardia]
ADD CONSTRAINT [FK_AgenteObservacionGuardia]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteObservacionGuardia'
CREATE INDEX [IX_FK_AgenteObservacionGuardia]
ON [dbo].[ObservacionesGuardia]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'EntradasSalidas'
ALTER TABLE [dbo].[EntradasSalidas]
ADD CONSTRAINT [FK_AgenteEntradaSalida]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteEntradaSalida'
CREATE INDEX [IX_FK_AgenteEntradaSalida]
ON [dbo].[EntradasSalidas]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'EntradasSalidas'
ALTER TABLE [dbo].[EntradasSalidas]
ADD CONSTRAINT [FK_AgenteEntradaSalida1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteEntradaSalida1'
CREATE INDEX [IX_FK_AgenteEntradaSalida1]
ON [dbo].[EntradasSalidas]
    ([AgenteId1]);
GO

-- Creating foreign key on [AgenteId] in table 'Reasignaciones'
ALTER TABLE [dbo].[Reasignaciones]
ADD CONSTRAINT [FK_AgenteReasignacion]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteReasignacion'
CREATE INDEX [IX_FK_AgenteReasignacion]
ON [dbo].[Reasignaciones]
    ([AgenteId]);
GO

-- Creating foreign key on [AreaId] in table 'Reasignaciones'
ALTER TABLE [dbo].[Reasignaciones]
ADD CONSTRAINT [FK_AreaReasignacion]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaReasignacion'
CREATE INDEX [IX_FK_AreaReasignacion]
ON [dbo].[Reasignaciones]
    ([AreaId]);
GO

-- Creating foreign key on [ResumenDiarioId] in table 'Marcaciones'
ALTER TABLE [dbo].[Marcaciones]
ADD CONSTRAINT [FK_ResumenDiarioMarcacion]
    FOREIGN KEY ([ResumenDiarioId])
    REFERENCES [dbo].[ResumenesDiarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResumenDiarioMarcacion'
CREATE INDEX [IX_FK_ResumenDiarioMarcacion]
ON [dbo].[Marcaciones]
    ([ResumenDiarioId]);
GO

-- Creating foreign key on [AgenteId] in table 'Marcaciones'
ALTER TABLE [dbo].[Marcaciones]
ADD CONSTRAINT [FK_AgenteMarcacion]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMarcacion'
CREATE INDEX [IX_FK_AgenteMarcacion]
ON [dbo].[Marcaciones]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'SolicitudesDeEstado'
ALTER TABLE [dbo].[SolicitudesDeEstado]
ADD CONSTRAINT [FK_AgenteSolicitudDeEstado]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteSolicitudDeEstado'
CREATE INDEX [IX_FK_AgenteSolicitudDeEstado]
ON [dbo].[SolicitudesDeEstado]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'SolicitudesDeEstado'
ALTER TABLE [dbo].[SolicitudesDeEstado]
ADD CONSTRAINT [FK_AgenteSolicitudDeEstado1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteSolicitudDeEstado1'
CREATE INDEX [IX_FK_AgenteSolicitudDeEstado1]
ON [dbo].[SolicitudesDeEstado]
    ([AgenteId1]);
GO

-- Creating foreign key on [TipoEstadoAgenteId] in table 'SolicitudesDeEstado'
ALTER TABLE [dbo].[SolicitudesDeEstado]
ADD CONSTRAINT [FK_TipoEstadoAgenteSolicitudDeEstado]
    FOREIGN KEY ([TipoEstadoAgenteId])
    REFERENCES [dbo].[TiposEstadoAgente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoEstadoAgenteSolicitudDeEstado'
CREATE INDEX [IX_FK_TipoEstadoAgenteSolicitudDeEstado]
ON [dbo].[SolicitudesDeEstado]
    ([TipoEstadoAgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'HAAnteriorEliminadasSet'
ALTER TABLE [dbo].[HAAnteriorEliminadasSet]
ADD CONSTRAINT [FK_AgenteHAAnteriorEliminadas]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteHAAnteriorEliminadas'
CREATE INDEX [IX_FK_AgenteHAAnteriorEliminadas]
ON [dbo].[HAAnteriorEliminadasSet]
    ([AgenteId]);
GO

-- Creating foreign key on [DirectorioId] in table 'Archivos'
ALTER TABLE [dbo].[Archivos]
ADD CONSTRAINT [FK_DirectorioArchivo]
    FOREIGN KEY ([DirectorioId])
    REFERENCES [dbo].[Directorios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectorioArchivo'
CREATE INDEX [IX_FK_DirectorioArchivo]
ON [dbo].[Archivos]
    ([DirectorioId]);
GO

-- Creating foreign key on [AgenteId] in table 'ImpresionReportes'
ALTER TABLE [dbo].[ImpresionReportes]
ADD CONSTRAINT [FK_AgenteImpresionReporte]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteImpresionReporte'
CREATE INDEX [IX_FK_AgenteImpresionReporte]
ON [dbo].[ImpresionReportes]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'HorasMesHorarioFlexibles'
ALTER TABLE [dbo].[HorasMesHorarioFlexibles]
ADD CONSTRAINT [FK_AgenteHorasMesHorarioFlexible]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteHorasMesHorarioFlexible'
CREATE INDEX [IX_FK_AgenteHorasMesHorarioFlexible]
ON [dbo].[HorasMesHorarioFlexibles]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'MovimientosSobreAgente'
ALTER TABLE [dbo].[MovimientosSobreAgente]
ADD CONSTRAINT [FK_AgenteMovimientoSobreAgente]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMovimientoSobreAgente'
CREATE INDEX [IX_FK_AgenteMovimientoSobreAgente]
ON [dbo].[MovimientosSobreAgente]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'MovimientosSobreAgente'
ALTER TABLE [dbo].[MovimientosSobreAgente]
ADD CONSTRAINT [FK_AgenteMovimientoSobreAgente1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMovimientoSobreAgente1'
CREATE INDEX [IX_FK_AgenteMovimientoSobreAgente1]
ON [dbo].[MovimientosSobreAgente]
    ([AgenteId1]);
GO

-- Creating foreign key on [AgenteId] in table 'LicenciasAgentes'
ALTER TABLE [dbo].[LicenciasAgentes]
ADD CONSTRAINT [FK_AgenteLicenciaAgente]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteLicenciaAgente'
CREATE INDEX [IX_FK_AgenteLicenciaAgente]
ON [dbo].[LicenciasAgentes]
    ([AgenteId]);
GO

-- Creating foreign key on [TipoLicenciaId] in table 'LicenciasAgentes'
ALTER TABLE [dbo].[LicenciasAgentes]
ADD CONSTRAINT [FK_TipoLicenciaLicenciaAgente]
    FOREIGN KEY ([TipoLicenciaId])
    REFERENCES [dbo].[TiposDeLicencia]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoLicenciaLicenciaAgente'
CREATE INDEX [IX_FK_TipoLicenciaLicenciaAgente]
ON [dbo].[LicenciasAgentes]
    ([TipoLicenciaId]);
GO

-- Creating foreign key on [TipoLicenciaId] in table 'DiasUsufructuados'
ALTER TABLE [dbo].[DiasUsufructuados]
ADD CONSTRAINT [FK_TipoLicenciaDiaUsufructado]
    FOREIGN KEY ([TipoLicenciaId])
    REFERENCES [dbo].[TiposDeLicencia]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoLicenciaDiaUsufructado'
CREATE INDEX [IX_FK_TipoLicenciaDiaUsufructado]
ON [dbo].[DiasUsufructuados]
    ([TipoLicenciaId]);
GO

-- Creating foreign key on [AgenteId] in table 'DiasUsufructuados'
ALTER TABLE [dbo].[DiasUsufructuados]
ADD CONSTRAINT [FK_AgenteDiaUsufructado]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteDiaUsufructado'
CREATE INDEX [IX_FK_AgenteDiaUsufructado]
ON [dbo].[DiasUsufructuados]
    ([AgenteId]);
GO

-- Creating foreign key on [AgentePersonalId] in table 'DiasUsufructuados'
ALTER TABLE [dbo].[DiasUsufructuados]
ADD CONSTRAINT [FK_AgenteDiaUsufructado1]
    FOREIGN KEY ([AgentePersonalId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteDiaUsufructado1'
CREATE INDEX [IX_FK_AgenteDiaUsufructado1]
ON [dbo].[DiasUsufructuados]
    ([AgentePersonalId]);
GO

-- Creating foreign key on [AgenteId] in table 'Sesiones'
ALTER TABLE [dbo].[Sesiones]
ADD CONSTRAINT [FK_AgenteSesion]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteSesion'
CREATE INDEX [IX_FK_AgenteSesion]
ON [dbo].[Sesiones]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'Mensajes'
ALTER TABLE [dbo].[Mensajes]
ADD CONSTRAINT [FK_AgenteMensaje]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMensaje'
CREATE INDEX [IX_FK_AgenteMensaje]
ON [dbo].[Mensajes]
    ([AgenteId]);
GO

-- Creating foreign key on [MensajeId] in table 'Destinatarios'
ALTER TABLE [dbo].[Destinatarios]
ADD CONSTRAINT [FK_MensajeDestinatario]
    FOREIGN KEY ([MensajeId])
    REFERENCES [dbo].[Mensajes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MensajeDestinatario'
CREATE INDEX [IX_FK_MensajeDestinatario]
ON [dbo].[Destinatarios]
    ([MensajeId]);
GO

-- Creating foreign key on [AgenteId] in table 'Destinatarios'
ALTER TABLE [dbo].[Destinatarios]
ADD CONSTRAINT [FK_AgenteDestinatario]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteDestinatario'
CREATE INDEX [IX_FK_AgenteDestinatario]
ON [dbo].[Destinatarios]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'ResumenesDiarios'
ALTER TABLE [dbo].[ResumenesDiarios]
ADD CONSTRAINT [FK_AgenteResumenDiario1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteResumenDiario1'
CREATE INDEX [IX_FK_AgenteResumenDiario1]
ON [dbo].[ResumenesDiarios]
    ([AgenteId1]);
GO

-- Creating foreign key on [TipoNotificacionId] in table 'Notificaciones'
ALTER TABLE [dbo].[Notificaciones]
ADD CONSTRAINT [FK_TipoNotificacionNotificacion]
    FOREIGN KEY ([TipoNotificacionId])
    REFERENCES [dbo].[Notificacion_Tipos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoNotificacionNotificacion'
CREATE INDEX [IX_FK_TipoNotificacionNotificacion]
ON [dbo].[Notificaciones]
    ([TipoNotificacionId]);
GO

-- Creating foreign key on [AgenteId] in table 'Notificaciones'
ALTER TABLE [dbo].[Notificaciones]
ADD CONSTRAINT [FK_AgenteNotificacion]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteNotificacion'
CREATE INDEX [IX_FK_AgenteNotificacion]
ON [dbo].[Notificaciones]
    ([AgenteId]);
GO

-- Creating foreign key on [NotificacionId] in table 'Notificacion_Historiales'
ALTER TABLE [dbo].[Notificacion_Historiales]
ADD CONSTRAINT [FK_NotificacionHistorialEstadosNotificacion]
    FOREIGN KEY ([NotificacionId])
    REFERENCES [dbo].[Notificaciones]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotificacionHistorialEstadosNotificacion'
CREATE INDEX [IX_FK_NotificacionHistorialEstadosNotificacion]
ON [dbo].[Notificacion_Historiales]
    ([NotificacionId]);
GO

-- Creating foreign key on [AgenteId] in table 'Notificacion_Historiales'
ALTER TABLE [dbo].[Notificacion_Historiales]
ADD CONSTRAINT [FK_AgenteHistorialEstadosNotificacion]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteHistorialEstadosNotificacion'
CREATE INDEX [IX_FK_AgenteHistorialEstadosNotificacion]
ON [dbo].[Notificacion_Historiales]
    ([AgenteId]);
GO

-- Creating foreign key on [Notificacion_EstadoId] in table 'Notificacion_Historiales'
ALTER TABLE [dbo].[Notificacion_Historiales]
ADD CONSTRAINT [FK_Notificacion_EstadoNotificacion_Historial]
    FOREIGN KEY ([Notificacion_EstadoId])
    REFERENCES [dbo].[Notificacion_Estados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Notificacion_EstadoNotificacion_Historial'
CREATE INDEX [IX_FK_Notificacion_EstadoNotificacion_Historial]
ON [dbo].[Notificacion_Historiales]
    ([Notificacion_EstadoId]);
GO

-- Creating foreign key on [AgenteId] in table 'CorteLicencia'
ALTER TABLE [dbo].[CorteLicencia]
ADD CONSTRAINT [FK_AgenteCorteLicenciaAnual]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteCorteLicenciaAnual'
CREATE INDEX [IX_FK_AgenteCorteLicenciaAnual]
ON [dbo].[CorteLicencia]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId1] in table 'CorteLicencia'
ALTER TABLE [dbo].[CorteLicencia]
ADD CONSTRAINT [FK_AgenteCorteLicenciaAnual1]
    FOREIGN KEY ([AgenteId1])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteCorteLicenciaAnual1'
CREATE INDEX [IX_FK_AgenteCorteLicenciaAnual1]
ON [dbo].[CorteLicencia]
    ([AgenteId1]);
GO

-- Creating foreign key on [ResumenDiarioId] in table 'AbrioDias'
ALTER TABLE [dbo].[AbrioDias]
ADD CONSTRAINT [FK_ResumenDiarioAbrioDia]
    FOREIGN KEY ([ResumenDiarioId])
    REFERENCES [dbo].[ResumenesDiarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ResumenDiarioAbrioDia'
CREATE INDEX [IX_FK_ResumenDiarioAbrioDia]
ON [dbo].[AbrioDias]
    ([ResumenDiarioId]);
GO

-- Creating foreign key on [AgenteId] in table 'AbrioDias'
ALTER TABLE [dbo].[AbrioDias]
ADD CONSTRAINT [FK_AgenteAbrioDia]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAbrioDia'
CREATE INDEX [IX_FK_AgenteAbrioDia]
ON [dbo].[AbrioDias]
    ([AgenteId]);
GO

-- Creating foreign key on [AreaId] in table 'AsuetosParciales'
ALTER TABLE [dbo].[AsuetosParciales]
ADD CONSTRAINT [FK_AreaAsuetoParcial]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[Areas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaAsuetoParcial'
CREATE INDEX [IX_FK_AreaAsuetoParcial]
ON [dbo].[AsuetosParciales]
    ([AreaId]);
GO

-- Creating foreign key on [AgenteId] in table 'Formularios1214'
ALTER TABLE [dbo].[Formularios1214]
ADD CONSTRAINT [FK_AgenteFormulario1214]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteFormulario1214'
CREATE INDEX [IX_FK_AgenteFormulario1214]
ON [dbo].[Formularios1214]
    ([AgenteId]);
GO

-- Creating foreign key on [Id_Agente] in table 'Agentes1214'
ALTER TABLE [dbo].[Agentes1214]
ADD CONSTRAINT [FK_AgenteAgente1214]
    FOREIGN KEY ([Id_Agente])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAgente1214'
CREATE INDEX [IX_FK_AgenteAgente1214]
ON [dbo].[Agentes1214]
    ([Id_Agente]);
GO

-- Creating foreign key on [Formulario1214Id] in table 'Agentes1214'
ALTER TABLE [dbo].[Agentes1214]
ADD CONSTRAINT [FK_Formulario1214Agente1214]
    FOREIGN KEY ([Formulario1214Id])
    REFERENCES [dbo].[Formularios1214]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Formulario1214Agente1214'
CREATE INDEX [IX_FK_Formulario1214Agente1214]
ON [dbo].[Agentes1214]
    ([Formulario1214Id]);
GO

-- Creating foreign key on [Agente_Id] in table 'Legajos_datos_laborales'
ALTER TABLE [dbo].[Legajos_datos_laborales]
ADD CONSTRAINT [FK_AgenteAgente_legajo_datos_laborales]
    FOREIGN KEY ([Agente_Id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAgente_legajo_datos_laborales'
CREATE INDEX [IX_FK_AgenteAgente_legajo_datos_laborales]
ON [dbo].[Legajos_datos_laborales]
    ([Agente_Id]);
GO

-- Creating foreign key on [Agente_Id] in table 'Legajos_datos_personales'
ALTER TABLE [dbo].[Legajos_datos_personales]
ADD CONSTRAINT [FK_AgenteAgente_legajo_datos_personales]
    FOREIGN KEY ([Agente_Id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAgente_legajo_datos_personales'
CREATE INDEX [IX_FK_AgenteAgente_legajo_datos_personales]
ON [dbo].[Legajos_datos_personales]
    ([Agente_Id]);
GO

-- Creating foreign key on [Legajo_datos_personalesId] in table 'Legajo_historial_domicilios'
ALTER TABLE [dbo].[Legajo_historial_domicilios]
ADD CONSTRAINT [FK_Legajo_datos_personalesLegajo_historial_domicilio]
    FOREIGN KEY ([Legajo_datos_personalesId])
    REFERENCES [dbo].[Legajos_datos_personales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_datos_personalesLegajo_historial_domicilio'
CREATE INDEX [IX_FK_Legajo_datos_personalesLegajo_historial_domicilio]
ON [dbo].[Legajo_historial_domicilios]
    ([Legajo_datos_personalesId]);
GO

-- Creating foreign key on [Legajo_datos_personales_Id] in table 'Legajos_conyuge'
ALTER TABLE [dbo].[Legajos_conyuge]
ADD CONSTRAINT [FK_Legajo_datos_personalesLegajo_conyugue]
    FOREIGN KEY ([Legajo_datos_personales_Id])
    REFERENCES [dbo].[Legajos_datos_personales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_datos_personalesLegajo_conyugue'
CREATE INDEX [IX_FK_Legajo_datos_personalesLegajo_conyugue]
ON [dbo].[Legajos_conyuge]
    ([Legajo_datos_personales_Id]);
GO

-- Creating foreign key on [Legajo_datos_personalesId] in table 'Legajo_hijos'
ALTER TABLE [dbo].[Legajo_hijos]
ADD CONSTRAINT [FK_Legajo_datos_personalesLegajo_hijo]
    FOREIGN KEY ([Legajo_datos_personalesId])
    REFERENCES [dbo].[Legajos_datos_personales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_datos_personalesLegajo_hijo'
CREATE INDEX [IX_FK_Legajo_datos_personalesLegajo_hijo]
ON [dbo].[Legajo_hijos]
    ([Legajo_datos_personalesId]);
GO

-- Creating foreign key on [Agente_Id] in table 'Legajo_fojas_de_servicio'
ALTER TABLE [dbo].[Legajo_fojas_de_servicio]
ADD CONSTRAINT [FK_AgenteLegajo_fojas_de_servicio]
    FOREIGN KEY ([Agente_Id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteLegajo_fojas_de_servicio'
CREATE INDEX [IX_FK_AgenteLegajo_fojas_de_servicio]
ON [dbo].[Legajo_fojas_de_servicio]
    ([Agente_Id]);
GO

-- Creating foreign key on [Legajo_datos_personalesId] in table 'Legajo_titulos_certificados'
ALTER TABLE [dbo].[Legajo_titulos_certificados]
ADD CONSTRAINT [FK_Legajo_datos_personalesLegajo_titulo_certificado]
    FOREIGN KEY ([Legajo_datos_personalesId])
    REFERENCES [dbo].[Legajos_datos_personales]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_datos_personalesLegajo_titulo_certificado'
CREATE INDEX [IX_FK_Legajo_datos_personalesLegajo_titulo_certificado]
ON [dbo].[Legajo_titulos_certificados]
    ([Legajo_datos_personalesId]);
GO

-- Creating foreign key on [Legajo_fojas_de_servicioId] in table 'Legajo_pagos_subrrogancia_bonificacion_antiguedad'
ALTER TABLE [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad]
ADD CONSTRAINT [FK_Legajo_fojas_de_servicioLegajo_pago_subrrogancia_bonificacion_antiguedad]
    FOREIGN KEY ([Legajo_fojas_de_servicioId])
    REFERENCES [dbo].[Legajo_fojas_de_servicio]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_fojas_de_servicioLegajo_pago_subrrogancia_bonificacion_antiguedad'
CREATE INDEX [IX_FK_Legajo_fojas_de_servicioLegajo_pago_subrrogancia_bonificacion_antiguedad]
ON [dbo].[Legajo_pagos_subrrogancia_bonificacion_antiguedad]
    ([Legajo_fojas_de_servicioId]);
GO

-- Creating foreign key on [Legajo_fojas_de_servicioId] in table 'Legajo_otros_eventos'
ALTER TABLE [dbo].[Legajo_otros_eventos]
ADD CONSTRAINT [FK_Legajo_fojas_de_servicioLegajo_otro_evento]
    FOREIGN KEY ([Legajo_fojas_de_servicioId])
    REFERENCES [dbo].[Legajo_fojas_de_servicio]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_fojas_de_servicioLegajo_otro_evento'
CREATE INDEX [IX_FK_Legajo_fojas_de_servicioLegajo_otro_evento]
ON [dbo].[Legajo_otros_eventos]
    ([Legajo_fojas_de_servicioId]);
GO

-- Creating foreign key on [Legajo_foja_de_servicioId] in table 'Legajos_afectaciones_designaciones'
ALTER TABLE [dbo].[Legajos_afectaciones_designaciones]
ADD CONSTRAINT [FK_Legajo_foja_de_servicioLegajo_afectacion_designacion]
    FOREIGN KEY ([Legajo_foja_de_servicioId])
    REFERENCES [dbo].[Legajo_fojas_de_servicio]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_foja_de_servicioLegajo_afectacion_designacion'
CREATE INDEX [IX_FK_Legajo_foja_de_servicioLegajo_afectacion_designacion]
ON [dbo].[Legajos_afectaciones_designaciones]
    ([Legajo_foja_de_servicioId]);
GO

-- Creating foreign key on [AgenteId_modificacion] in table 'Modificaciones_cierres_meses'
ALTER TABLE [dbo].[Modificaciones_cierres_meses]
ADD CONSTRAINT [FK_AgenteModificacion_cierre_mes]
    FOREIGN KEY ([AgenteId_modificacion])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteModificacion_cierre_mes'
CREATE INDEX [IX_FK_AgenteModificacion_cierre_mes]
ON [dbo].[Modificaciones_cierres_meses]
    ([AgenteId_modificacion]);
GO

-- Creating foreign key on [CierreMensualId] in table 'Modificaciones_cierres_meses'
ALTER TABLE [dbo].[Modificaciones_cierres_meses]
ADD CONSTRAINT [FK_CierreMensualModificacion_cierre_mes]
    FOREIGN KEY ([CierreMensualId])
    REFERENCES [dbo].[CierreMensual]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CierreMensualModificacion_cierre_mes'
CREATE INDEX [IX_FK_CierreMensualModificacion_cierre_mes]
ON [dbo].[Modificaciones_cierres_meses]
    ([CierreMensualId]);
GO

-- Creating foreign key on [Legajo_foja_de_servicioId] in table 'Legajos_carreras_administrativas'
ALTER TABLE [dbo].[Legajos_carreras_administrativas]
ADD CONSTRAINT [FK_Legajo_foja_de_servicioLegajo_carrera_administrativa]
    FOREIGN KEY ([Legajo_foja_de_servicioId])
    REFERENCES [dbo].[Legajo_fojas_de_servicio]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Legajo_foja_de_servicioLegajo_carrera_administrativa'
CREATE INDEX [IX_FK_Legajo_foja_de_servicioLegajo_carrera_administrativa]
ON [dbo].[Legajos_carreras_administrativas]
    ([Legajo_foja_de_servicioId]);
GO

-- Creating foreign key on [Aprobado_por_agente_id] in table 'Agentes1214'
ALTER TABLE [dbo].[Agentes1214]
ADD CONSTRAINT [FK_AgenteAgente12142]
    FOREIGN KEY ([Aprobado_por_agente_id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAgente12142'
CREATE INDEX [IX_FK_AgenteAgente12142]
ON [dbo].[Agentes1214]
    ([Aprobado_por_agente_id]);
GO

-- Creating foreign key on [Rechazado_por_agente_id] in table 'Agentes1214'
ALTER TABLE [dbo].[Agentes1214]
ADD CONSTRAINT [FK_AgenteAgente12143]
    FOREIGN KEY ([Rechazado_por_agente_id])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteAgente12143'
CREATE INDEX [IX_FK_AgenteAgente12143]
ON [dbo].[Agentes1214]
    ([Rechazado_por_agente_id]);
GO

-- Creating foreign key on [AgenteId] in table 'Validaciones_email'
ALTER TABLE [dbo].[Validaciones_email]
ADD CONSTRAINT [FK_AgenteValidacion_email]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteValidacion_email'
CREATE INDEX [IX_FK_AgenteValidacion_email]
ON [dbo].[Validaciones_email]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'CambiosDeClave'
ALTER TABLE [dbo].[CambiosDeClave]
ADD CONSTRAINT [FK_AgenteCambioClave]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteCambioClave'
CREATE INDEX [IX_FK_AgenteCambioClave]
ON [dbo].[CambiosDeClave]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'Memo_17_DDJJs'
ALTER TABLE [dbo].[Memo_17_DDJJs]
ADD CONSTRAINT [FK_AgenteMemo_17_DDJJ]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteMemo_17_DDJJ'
CREATE INDEX [IX_FK_AgenteMemo_17_DDJJ]
ON [dbo].[Memo_17_DDJJs]
    ([AgenteId]);
GO

-- Creating foreign key on [Memo_17_DDJJ_Id] in table 'Memo_17_DDJJ_Hijos'
ALTER TABLE [dbo].[Memo_17_DDJJ_Hijos]
ADD CONSTRAINT [FK_Memo_17_DDJJMemo_17_DDJJ_Hijo]
    FOREIGN KEY ([Memo_17_DDJJ_Id])
    REFERENCES [dbo].[Memo_17_DDJJs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Memo_17_DDJJMemo_17_DDJJ_Hijo'
CREATE INDEX [IX_FK_Memo_17_DDJJMemo_17_DDJJ_Hijo]
ON [dbo].[Memo_17_DDJJ_Hijos]
    ([Memo_17_DDJJ_Id]);
GO

-- Creating foreign key on [TipoHorariosFexibleId] in table 'Agentes'
ALTER TABLE [dbo].[Agentes]
ADD CONSTRAINT [FK_TipoHorariosFexibleAgente]
    FOREIGN KEY ([TipoHorariosFexibleId])
    REFERENCES [dbo].[TiposHorariosFexibles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoHorariosFexibleAgente'
CREATE INDEX [IX_FK_TipoHorariosFexibleAgente]
ON [dbo].[Agentes]
    ([TipoHorariosFexibleId]);
GO

-- Creating foreign key on [Estrato1214Id] in table 'Formularios1214'
ALTER TABLE [dbo].[Formularios1214]
ADD CONSTRAINT [FK_Estrato1214Formulario1214]
    FOREIGN KEY ([Estrato1214Id])
    REFERENCES [dbo].[Estratos1214]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Estrato1214Formulario1214'
CREATE INDEX [IX_FK_Estrato1214Formulario1214]
ON [dbo].[Formularios1214]
    ([Estrato1214Id]);
GO

-- Creating foreign key on [AgenteId] in table 'DiasAutorizadosRemoto'
ALTER TABLE [dbo].[DiasAutorizadosRemoto]
ADD CONSTRAINT [FK_AgenteDiaAutorizadoRemoto]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteDiaAutorizadoRemoto'
CREATE INDEX [IX_FK_AgenteDiaAutorizadoRemoto]
ON [dbo].[DiasAutorizadosRemoto]
    ([AgenteId]);
GO

-- Creating foreign key on [AgenteId] in table 'TurnosIngresoPermitido'
ALTER TABLE [dbo].[TurnosIngresoPermitido]
ADD CONSTRAINT [FK_AgenteTurnoIngresoPermitido]
    FOREIGN KEY ([AgenteId])
    REFERENCES [dbo].[Agentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AgenteTurnoIngresoPermitido'
CREATE INDEX [IX_FK_AgenteTurnoIngresoPermitido]
ON [dbo].[TurnosIngresoPermitido]
    ([AgenteId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------