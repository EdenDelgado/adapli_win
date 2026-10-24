using System;

namespace Sisvta.Domain.Entities
{
    public class GrupoLinea
    {
        public int Id { get; set; }           // id_gruposlineas (Llave primaria)
        public string Nombre { get; set; }      // nombre
        public int Status { get; set; }         // status
       // public DateTime DateRecord { get; set; }// daterecord
       // public int IdEmpresa { get; set; }      // id_empresa
    }
}