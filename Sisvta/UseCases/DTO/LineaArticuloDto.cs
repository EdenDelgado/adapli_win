using System;

namespace Sisvta.UseCases.DTOs
{
    public class LineaArticuloDto
    {
        public int IdLineasArticulos { get; set; } // id_lineasarticulos (Llave primaria)
        public string Nombre { get; set; }          // nombre
        public string NombreGrupo { get; set; }    // nombre del grupo de línea

        //public int Status { get; set; }            // status
        //public DateTime DateRecord { get; set; }   // daterecord

        // Cambiamos float por double aquí:
        //public double Utilidad { get; set; }       // utilidad

        public int PrcComision { get; set; }       // prcComision

        public int GrupoLinea { get; set; }        // grupolinea
        //public int IdEmpresa { get; set; }         // id_empresa
    }
}