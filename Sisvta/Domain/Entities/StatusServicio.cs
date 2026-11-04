using System;

namespace Sisvta.Domain.Entities
{
    public class StatusServicio
    {
        public int IdStatusServicios { get; set; }
        public string Status { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Daterecord { get; set; }

        // Constructor vacío requerido por EF Core
        public StatusServicio() { }

        // Constructor opcional para inicializar
        public StatusServicio(string status, string descripcion)
        {
            Status = status;
            Descripcion = descripcion;
            Daterecord = DateTime.Now;
        }
    }
}