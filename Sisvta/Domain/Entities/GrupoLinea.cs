using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("gruposlineas")]
    public class GrupoLinea
    {
        [Key]
        [Column("id_gruposlineas")]
        public int Id { get; set; }

        [Column("nombre")]
        [StringLength(100)] // Ajusta la longitud según tu esquema si es necesario
        public string Nombre { get; set; }

        [Column("status")]
        public int Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        [Column("id_empresa")]
        public int IdEmpresa { get; set; }
    }
}