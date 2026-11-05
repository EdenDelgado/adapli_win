using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("unidadmedida")]
    public class UnidadMedida
    {
        [Key]
        [Column("id_um")]
        public int IdUm { get; set; }

        [Column("nomabrev")]
        [StringLength(50)]
        public string NomAbrev { get; set; }

        [Column("nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        [Column("principal")]
        public bool? Principal { get; set; }

        [Column("ClaveUnidad")]
        [StringLength(5)]
        public string ClaveUnidad { get; set; }

        [Column("c_ClaveUnidad")]
        [StringLength(10)]
        public string C_ClaveUnidad { get; set; }
    }
}