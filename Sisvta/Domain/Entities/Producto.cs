using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Sisvta.Domain.Entities
{
    public class Producto
    {
        public string Claveproserv { get; private set; }
        public string Clave { get; private set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public bool Inventariable { get; private set; }

        public int? Umedida { get; private set; }

        public int? Prodserv { get; private set; }
        //public string Almacen { get; private set; }
        public int? Linea { get; private set; }
       // public int Grupo { get; private set; }
        public int? Status { get; private set; }
        public double? Pcompra { get; private set; }
//public decimal Pventa { get; private set; }

        // 1. Constructor privado requerido por EF Core para la materialización
        private Producto() { }

        // 2. Tu constructor existente para la lógica de negocio


        public Producto(string clave, string nombre, string descripcion, bool inventariable, int? umedida, int? prodserv, string claveproserv, int? linea, int? status, double? pcompra)
        {
            Clave = clave;
            Nombre = nombre;
            Descripcion = descripcion;
            Linea = linea;
            Umedida = umedida;
            Prodserv = prodserv; // <-- Ahora coincide perfectamente con el int? del formulario
            Claveproserv = claveproserv;
            Inventariable = inventariable;
            Status = status;
            Pcompra = pcompra;
        }
    }
}