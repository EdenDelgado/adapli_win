using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("servicios")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_servicio")]
        public int IdServicio { get; set; }

        [Column("clave")]
        [Required]
        [StringLength(50)]
        public string Clave { get; set; }

        [Column("s_nombre")]
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Column("descripcion")]
        [StringLength(4000)]
        public string Descripcion { get; set; }

        [Column("lineaarticulo")]
        public int? LineaArticulo { get; set; }

        [Column("umedidacompra")]
        public int? UMedidaCompra { get; set; } = 1;

        [Column("umedidaventa")]
        public int? UMedidaVenta { get; set; } = 1;

        [Column("foto")]
        [StringLength(255)]
        public string Foto { get; set; }

        [Column("contenido")]
        public int? Contenido { get; set; } = 1;

        [Column("marca")]
        public int? Marca { get; set; }

        [Column("modelo")]
        public int? Modelo { get; set; } = 1;

        [Column("color")]
        public int? Color { get; set; } = 1;

        [Column("genero")]
        [StringLength(50)]
        public string Genero { get; set; }

        [Column("medida")]
        public int? Medida { get; set; } = 1;

        [Column("inventariable")]
        public bool Inventariable { get; set; } = false;

        [Column("artpad")]
        public int? ArtPad { get; set; } = 0;

        [Column("kit")]
        public bool Kit { get; set; } = false;

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;

        [Column("maximo")]
        public int? Maximo { get; set; } = 0;

        [Column("minimo")]
        public int? Minimo { get; set; } = 0;

        [Column("reorden")]
        public int? Reorden { get; set; } = 0;

        [Column("ClaveProServ")]
        [StringLength(10)]
        public string ClaveProServ { get; set; }

        [Column("fechaInicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fechaFin")]
        public DateTime? FechaFin { get; set; }

        [Column("id_empresa")]
        public int? IdEmpresa { get; set; }

        [Column("preciocompra")]
        public double? PrecioCompra { get; set; }

        [Column("idObjetoImp")]
        public int? IdObjetoImp { get; set; }

        [Column("UserCreator")]
        public int? UserCreator { get; set; }

        [Column("UserLastUpdate")]
        public int? UserLastUpdate { get; set; }

        [Column("DateLastUpdate")]
        public DateTime? DateLastUpdate { get; set; }

        [Column("Comentarios")]
        [StringLength(800)]
        public string Comentarios { get; set; }

        [Column("idUbicacionAlmacen")]
        public int? IdUbicacionAlmacen { get; set; }
    }
}