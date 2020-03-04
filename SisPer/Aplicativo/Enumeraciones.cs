using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPer.Aplicativo
{
    public static class Enumeraciones
    {
        //----------------------OJO----------------------------
        //NOTARAN QUE ESTAN VALORIZADOS TODOS LOS ESTADOS
        //AL AGREGAR O MODIFICAR RESPETAR LA NUMERACION!!!
        //DE MANERA QUE NO INFLUYA EN LO QUE YA ESTA CODIFICADO.
        //----------------------SALUDOS!------------------------

        //public enum EstadosHorarioVespertino
        //{
        //    Solicitado = 0,
        //    Aprobado = 1,
        //    Terminado = 2,
        //    Cancelado = 3,
        //    CanceladoAutomatico = 4
        //}

        //public enum EstadosFrancos
        //{
        //    Solicitado = 0,
        //    AprobadoJefe = 1,
        //    AprobadoPersonal = 2,
        //    Aprobado = 3,
        //    Cancelado = 4,
        //    CanceladoAutomatico = 5
        //}

        //public enum TipoSalida
        //{
        //    Ninguno = 0,
        //    Particular = 1,
        //    Oficial = 2,
        //    Indisposición = 3
        //}

        //public enum PerfilUsuario
        //{
        //    Ninguno = 0,
        //    Agente = 1,
        //    Personal = 2,
        //    Administrador = 3,
        //    Desarrollador = 4,
        //    Guardia = 5
        //}

        //public enum EstadosAgente
        //{
        //    Activo = 0,
        //    Salida_Particular = 1,
        //    Salida_Oficial = 2,
        //    Indisposición = 3,
        //    Franco_Compensatorio = 4
        //}

        public enum Meses
        { 
            Enero = 1,
            Febrero = 2, 
            Marzo= 3, 
            Abril= 4, 
            Mayo= 5, 
            Junio= 6,
            Julio = 7, 
            Agosto= 8, 
            Septiembre= 9, 
            Octubre= 10, 
            Noviembre= 11, 
            Diciembre= 12
        }

    }
}