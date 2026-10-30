using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("estados")]
    public class Estado
    {
        [Key]
        [Column("id_estado")]
        public int IdEstado { get; set; }

        [Column("nombre")]
        [StringLength(20)]
        public string Nombre { get; set; }

        [Column("id_zona")]
        public double IdZona { get; set; } = 0;

        [Column("status")]
        public int Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? Daterecord { get; set; } = DateTime.Now;

        [Column("pais")]
        public int? Pais { get; set; }

        [Timestamp]
        [Column("upsize_ts")]
        public byte[] UpsizeTs { get; set; }

        [Column("Clave")]
        [StringLength(50)]
        public string Clave { get; set; }
    }
}