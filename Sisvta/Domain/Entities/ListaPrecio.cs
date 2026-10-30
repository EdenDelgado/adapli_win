using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("listaprecio")]
    public class ListaPrecio
    {
        [Key]
        [Column("id_listaprecio")]
        public int IdListaPrecio { get; set; }

        [Column("listaprecio")]
        [StringLength(50)]
        public string NombreListaPrecio { get; set; }

        [Column("staus")]
        public int? Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        [Column("condicion")]
        [StringLength(50)]
        public string Condicion { get; set; }

        // Como 'default' es una palabra reservada en C#, la mapeamos directamente a la columna SQL con corchetes
        [Column("default")]
        public bool EsDefault { get; set; } = false;

        [Column("porcentaje")]
        public double? Porcentaje { get; set; }

        [Column("pdv")]
        public bool? Pdv { get; set; }

        [Column("id_empresa")]
        public int? IdEmpresa { get; set; }
    }
}