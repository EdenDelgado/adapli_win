using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("salidadetinv")]
    public class SalidaDetInv
    {
        [Key]
        [Column("idsalidadet")]
        public int IdSalidaDet { get; set; }

        [Column("producto")]
        public int? Producto { get; set; }

        [Column("qty")]
        public float? Qty { get; set; }

        [Column("salida")]
        public int? Salida { get; set; }

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; }

        [Column("precio")]
        public decimal? Precio { get; set; }

        [Column("descripcion")]
        [MaxLength(4000)]
        public string Descripcion { get; set; }

        // Opcional: Propiedad de navegación si tienes la entidad SalidaInv creada
        // [ForeignKey("Salida")]
        // public virtual SalidaInv SalidaInv { get; set; }

        // Constructor vacío requerido por EF Core
        public SalidaDetInv()
        {
            Producto = 0;
            Qty = 0;
            Salida = 0;
            Precio = 0;
            DateRecord = DateTime.Now;
        }
    }
}