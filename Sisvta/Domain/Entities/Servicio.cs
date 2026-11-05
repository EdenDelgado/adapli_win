using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("servicios")]
    public class Servicio
    {
        [Key]
        [Column("id_servicio")]
        public int IdServicio { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("clave")]
        public string Clave { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("s_nombre")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(4000)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("lineaarticulo")]
        public int? LineaArticulo { get; set; }

        [Column("umedidacompra")]
        public int? UMedidaCompra { get; set; } = 1;

        [Column("umedidaventa")]
        public int? UMedidaVenta { get; set; } = 1;

        [MaxLength(255)]
        [Column("foto")]
        public string? Foto { get; set; }

        [Column("contenido")]
        public int? Contenido { get; set; } = 1;

        [Column("marca")]
        public int? Marca { get; set; }

        [Column("modelo")]
        public int? Modelo { get; set; } = 1;

        [Column("color")]
        public int? Color { get; set; } = 1;

        [MaxLength(50)]
        [Column("genero")]
        public string? Genero { get; set; }

        [Column("medida")]
        public int? Medida { get; set; } = 1;

        [Required]
        [Column("inventariable")]
        public bool Inventariable { get; set; } = false;

        [Column("artpad")]
        public int? ArtPad { get; set; } = 0;

        [Required]
        [Column("kit")]
        public bool Kit { get; set; } = false;

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("daterecord")]
        public DateTime? Daterecord { get; set; }

        [Column("maximo")]
        public int? Maximo { get; set; } = 0;

        [Column("minimo")]
        public int? Minimo { get; set; } = 0;

        [Column("reorden")]
        public int? Reorden { get; set; } = 0;

        [MaxLength(10)]
        [Column("ClaveProServ")]
        public string? ClaveProServ { get; set; }

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

        [MaxLength(800)]
        [Column("Comentarios")]
        public string? Comentarios { get; set; }

        [Column("idUbicacionAlmacen")]
        public int? IdUbicacionAlmacen { get; set; }

        // Constructores opcionales para facilitar la inicialización
        public Servicio() { }

        public Servicio(string clave, string nombre, string descripcion, bool inventariable, int? umedidacompra, int? prodServ, string claveProServ, int linea, int status, double precio)
        {
            Clave = clave;
            Nombre = nombre;
            Descripcion = descripcion;
            Inventariable = inventariable;
            UMedidaCompra = umedidacompra;
            LineaArticulo = linea;
            Status = status;
            ClaveProServ = claveProServ;
            Daterecord = DateTime.Now;
        }
    }
}