using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("concepto")]
    public class Concepto
    {
        [Key]
        [Column("id_conceptosprov")]
        public int IdConceptosProv { get; set; }

        [MaxLength(50)]
        [Column("concepto")]
        public string? DescripcionConcepto { get; set; }

        [MaxLength(50)]
        [Column("conceptoabreviado")]
        public string? ConceptoAbreviado { get; set; }

        [Column("naturaleza")]
        public int? Naturaleza { get; set; } = 1; // Entrada = 1, Salida = 2

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("DateRecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        // Constructor vacío
        public Concepto() { }

        // Constructor útil para inicializaciones rápidas
        public Concepto(string? concepto, string? conceptoAbreviado, int naturaleza, int status)
        {
            DescripcionConcepto = concepto;
            ConceptoAbreviado = conceptoAbreviado;
            Naturaleza = naturaleza;
            Status = status;
            DateRecord = DateTime.Now;
        }
    }
}