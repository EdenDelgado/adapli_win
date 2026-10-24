using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities // O tu namespace de modelos
{
    [Table("almacen")]
    public class Almacen
    {
        [Key]
        [Column("id_almacen")]
        public int IdAlmacen { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("calle")]
        public string? Calle { get; set; }

        [Column("ciudad")]
        public string? Ciudad { get; set; }

        [Column("estado")]
        public short? Estado { get; set; }

        [Column("pais")]
        public int Pais { get; set; }

        [Column("cp")]
        public string? cp { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("leyendatickt")]
        public string? Leyendatickt { get; set; }

        [Column("default")]
        public bool Default { get; set; }
    }
}