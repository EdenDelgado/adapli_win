using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("proveedores", Schema = "dbo")]
    public class Proveedores
    {
        [Key]
        [Column("Id_proveedor")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("id_estado")]
        public short? IdEstado { get; set; } = 0;

        [StringLength(15)]
        [Column("rfc")]
        public string? Rfc { get; set; }

        [StringLength(30)]
        [Column("curp")]
        public string? Curp { get; set; }

        [StringLength(100)]
        [Column("telefono")]
        public string? Telefono { get; set; }

        [StringLength(50)]
        [Column("mail")]
        public string? Mail { get; set; }

        [StringLength(5)]
        [Column("cp")]
        public string? Cp { get; set; }

        [StringLength(255)]
        [Column("observaciones")]
        public string? Observaciones { get; set; }

        [Column("id_condicionpago")]
        public int? IdCondicionPago { get; set; } = 0;

        [Column("id_tipoproveedor")]
        public int? IdTipoProveedor { get; set; } = 0;

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("DateRecord", TypeName = "datetime")]
        public DateTime? DateRecord { get; set; }

        [Column("moneda")]
        public int? Moneda { get; set; } = 0;

        [Column("pais")]
        public int? Pais { get; set; } = 0;

        [Column("status_proveedor")]
        public int? StatusProveedor { get; set; }

        [Column("ciudad")]
        public int? Ciudad { get; set; }

        [StringLength(20)]
        [Column("clave")]
        public string? Clave { get; set; }

        [Column("limite_credito", TypeName = "money")]
        public decimal? LimiteCredito { get; set; }

        [Column("principal")]
        public bool? Principal { get; set; }
    }
}