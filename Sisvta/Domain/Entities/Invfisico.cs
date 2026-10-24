using System;

namespace Sisvta.Domain.Entities
{
    public class InvFisico
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Usuario { get; set; } = 0;
        public DateTime? DateRecord { get; set; } = DateTime.Now;
    }
}