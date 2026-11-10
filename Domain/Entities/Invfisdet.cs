using System;

namespace Sisvta.Domain.Entities
{
    public class InvFisDet
    {
        public int Id { get; set; }
        public int Articulo { get; set; }
        public double? Cantidad { get; set; }
        public DateTime? DateRecord { get; set; } = DateTime.Now;
        public double? InvFisico { get; set; }
        public double? CantidadReal { get; set; }
    }
}