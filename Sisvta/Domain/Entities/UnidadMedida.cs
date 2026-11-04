using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("unidadmedida")]
    public class UnidadMedida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_um")]
        public int IdUm { get; set; }

        [Column("clave")]
        [StringLength(10)]
        public string Clave { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}