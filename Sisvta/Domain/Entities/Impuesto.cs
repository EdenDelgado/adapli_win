using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("impuesto")]
    public class Impuesto
    {
        [Key]
        [Column("id_impuesto")]
        public int IdImpuesto { get; set; }

        [Column("impuesto")]
        [StringLength(50)]
        public string NombreImpuesto { get; set; }

        [Column("tipo")]
        public int? Tipo { get; set; } = 0;

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        [Column("predeterminado")]
        public bool Predeterminado { get; set; } = false;

        [Column("porcentaje")]
        public double? Porcentaje { get; set; } = 0;

        [Column("ImpuestoClave")]
        [StringLength(5)]
        public string ImpuestoClave { get; set; }

        [Column("TipoFactor")]
        [StringLength(10)]
        public string TipoFactor { get; set; }
    }
}