using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("pais")]
    public class Pais
    {
        [Key]
        [Column("id_pais")]
        public int IdPais { get; set; }

        [Column("pais")]
        [StringLength(50)]
        public string NombrePais { get; set; }

        [Column("ClavePais")]
        [StringLength(3)]
        public string ClavePais { get; set; }

        [Column("FormatoDeCodigoPostal")]
        [StringLength(50)]
        public string FormatoDeCodigoPostal { get; set; }

        [Column("FormatoDeRegistroDeIdentidadTributaria")]
        [StringLength(200)]
        public string FormatoDeRegistroDeIdentidadTributaria { get; set; }

        [Column("ValidacionDelRegistroDeIdentidadTributaria")]
        [StringLength(50)]
        public string ValidacionDelRegistroDeIdentidadTributaria { get; set; }

        [Column("Agrupaciones")]
        [StringLength(20)]
        public string Agrupaciones { get; set; }
    }
}