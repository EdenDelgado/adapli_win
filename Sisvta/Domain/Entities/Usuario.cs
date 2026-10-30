using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("idAlmacen")]
        public int? IdAlmacen { get; set; }

        [Column("idCaja")]
        public int? IdCaja { get; set; }

        [Column("id_distribuidor")]
        public int? IdDistribuidor { get; set; }

        [Column("nombre")]
        [Required]
        [MaxLength(40)]
        public string Nombre { get; set; }

        [Column("password")]
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column("nick")]
        [MaxLength(30)]
        public string Nick { get; set; }

        [Column("id_perfil")]
        public int? IdPerfil { get; set; }

        [Column("id_statususer")]
        public int IdStatusUser { get; set; }

        [Column("fecha_status")]
        public DateTime? FechaStatus { get; set; }

        [Column("mail")]
        [Required]
        [MaxLength(100)]
        public string Mail { get; set; }

        [Column("id_empresa")]
        public int IdEmpresa { get; set; }

        [Column("vigencia")]
        public DateTime? Vigencia { get; set; }

        [Column("personal")]
        public int? Personal { get; set; }

        [Column("id_depto")]
        public int? IdDepto { get; set; }

        [Column("orden")]
        public int? Orden { get; set; }

        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        [Column("id_sucursal")]
        public int? IdSucursal { get; set; }

        [Column("UserCreator")]
        public int? UserCreator { get; set; }

        [Column("DateLastUpdate")]
        public DateTime? DateLastUpdate { get; set; }

        [Column("UserLastUpdater")]
        public int? UserLastUpdater { get; set; }

        [Column("Daterecord")]
        public DateTime? DateRecord { get; set; }

        // Constructor vacío requerido por EF Core
        public Usuario()
        {
            IdUsuario = 0;
            Nombre = string.Empty;
            Password = string.Empty;
            Mail = string.Empty;
            IdStatusUser = 1;
            IdEmpresa = 0;
        }

        // Constructor con parámetros principales
        public Usuario(int idUsuario, string nombre, string password, string mail)
        {
            IdUsuario = idUsuario;
            Nombre = nombre ?? string.Empty;
            Password = password ?? string.Empty;
            Mail = mail ?? string.Empty;
            IdStatusUser = 1;
            IdEmpresa = 0;
        }
    }
}