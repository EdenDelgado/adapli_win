using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sisvta.Domain.Entities
{
    [Table("usuarios")] // Ajusta el nombre de la tabla si en tu base de datos es singular ('usuario')
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Column("login")]
        [Required]
        [StringLength(50)]
        public string Login { get; set; }

        [Column("password")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Column("status")]
        public int? Status { get; set; } = 1;

        [Column("id_empresa")]
        public int? IdEmpresa { get; set; }

        [Column("daterecord")]
        public DateTime? DateRecord { get; set; } = DateTime.Now;
    }
}