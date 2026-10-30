using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("clientes")] // Asegúrate de que coincida con el nombre exacto de la tabla en tu base de datos (ej. "clientes" o "Cliente")
    public class Cliente
    {
        [Key]
        [Column("id_empros")] // Mapeado al nombre de la columna que utilizas en tus consultas SQL (ej. left join clientes as cl on s.cliente = cl.id_empros)
        public int Id { get; set; }

        [Column("nombre")]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Column("domicilio")]
        [MaxLength(250)]
        public string Domicilio { get; set; }

        [Column("rfc")]
        [MaxLength(20)]
        public string Rfc { get; set; }

        // Constructor vacío requerido por Entity Framework y otras librerías
        public Cliente()
        {
            Id = 0;
            Nombre = string.Empty;
            Domicilio = string.Empty;
            Rfc = string.Empty;
        }

        // Constructor con parámetros
        public Cliente(int id, string nombre, string domicilio, string rfc)
        {
            Id = id;
            Nombre = nombre;
            Domicilio = domicilio;
            Rfc = rfc;
        }
    }
}