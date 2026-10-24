using System;

namespace Sisvta.Domain.Entities
{
    public class LineaArticulo
    {
        public int IdLineasArticulos { get; set; } // id_lineasarticulos (Llave primaria)
        public string Nombre { get; set; }          // nombre
        public int GrupoLinea { get; set; }        // grupolinea
        public int Status { get; set; }            // status
        public DateTime DateRecord { get; set; }   // daterecord
        public float Utilidad { get; set; }        // utilidad
        public int PrcComision { get; set; }       // prcComision
        public int IdEmpresa { get; set; }         // id_empresa
    }
}