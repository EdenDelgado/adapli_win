using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("inventario_detalles")]
    public class InventarioDetalle
    {
        [Key]
        [Column("id_invdet")]
        public int IdInvDet { get; set; }

        [Column("articulo")]
        public int? Articulo { get; set; }

        [Column("movinventario")]
        public int? MovInventario { get; set; }

        [Column("cantidad")]
        public float? Cantidad { get; set; }

        [Column("preciou")]
        public decimal? PrecioU { get; set; }

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("id_doctoscp_det")]
        public int? IdDoctoScpDet { get; set; }

        [Column("descripcion")]
        [MaxLength(4000)]
        public string Descripcion { get; set; }

        // Constructor vacío requerido por EF Core
        public InventarioDetalle()
        {
            Articulo = 0;
            MovInventario = 0;
            Cantidad = 0;
            PrecioU = 0;
            Status = 1;
            IdDoctoScpDet = 0;
            DateRecord = DateTime.Now;
        }
    }
}