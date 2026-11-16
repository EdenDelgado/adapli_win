using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id_empros")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("telefono1")]
        public string? Telefono1 { get; set; }

        [Column("telefono2")]
        public string? Telefono2 { get; set; }

        [Column("rfc")]
        public string? Rfc { get; set; }

        [Column("domicilio")]
        public string? Domicilio { get; set; }

        [Column("curp")]
        public string? curp { get; set; }

        [Column("status")]
        public int Status { get; set; }

        // Constructor por defecto
        public Cliente()
        {
            Nombre = string.Empty;
        }
    }
}