using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("moneda")]
    public class Moneda
    {
        [Key]
        [Column("Id_moneda")]
        public int IdMoneda { get; set; }

        [Column("moneda")]
        [StringLength(50)]
        public string SimboloMoneda { get; set; }

        [Column("nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Column("status")]
        public bool Status { get; set; } = true;

        [Column("DateRecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;
    }
}