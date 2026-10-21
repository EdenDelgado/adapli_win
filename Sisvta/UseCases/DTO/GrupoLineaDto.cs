using System;

namespace Sisvta.UseCases.DTO
{
    public class GrupoLineaDto
    {
        public int? Grupo { get; set; }       // Mapeado desde id_gruposlineas
        public string? Nombre { get; set; }     // nombre
       // public int? Status { get; set; }        // status
       // public DateTime? DateRecord { get; set; }// daterecord
       // public int? IdEmpresa { get; set; }     // id_empresa
    }
}