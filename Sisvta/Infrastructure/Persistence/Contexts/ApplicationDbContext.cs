using System;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;

namespace Sisvta.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();
        public DbSet<GrupoLinea> GruposLineas => Set<GrupoLinea>();
        public DbSet<LineaArticulo> LineasArticulos => Set<LineaArticulo>();
        public DbSet<Almacen> Almacenes => Set<Almacen>();
        public DbSet<Proveedores> Proveedores => Set<Proveedores>(); // <--- Añadido aquí
        public DbSet<InvFisDet> InvFisDets => Set<InvFisDet>();
        // Añade el DbSet junto a los demás conjuntos:
        public DbSet<InvFisico> InvFisicos => Set<InvFisico>();
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<Impuesto> Impuesto { get; set; }
        public DbSet<ListaPrecio> ListaPrecio { get; set; }
        public DbSet<Moneda> Moneda { get; set; }
        public DbSet<StatusServicio> StatusServicio { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Concepto> Conceptos { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Pais> Pais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_empros");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Domicilio).HasColumnName("domicilio").HasMaxLength(200);
                entity.Property(e => e.Rfc).HasColumnName("rfc").HasMaxLength(50);
            });

            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.ToTable("Proveedores", "dbo");
                entity.HasKey(e => e.Id); // <--- Llave primaria requerida
                entity.Property(e => e.Id).HasColumnName("Id_proveedor");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Direccion).HasColumnName("direccion").HasMaxLength(255);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("servicios", "dbo");
                entity.HasKey(e => e.Clave);
                entity.Property(e => e.Claveproserv).HasColumnName("claveproserv");
                entity.Property(e => e.Clave).HasColumnName("clave");
                entity.Property(e => e.Nombre).HasColumnName("s_nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Prodserv)
                                        .HasColumnName("id_servicio")
                                        .ValueGeneratedOnAdd();

                entity.Property(e => e.Linea).HasColumnName("lineaArticulo");
                entity.Property(e => e.Inventariable).HasColumnName("inventariable");
                entity.Property(e => e.Umedida).HasColumnName("umedidacompra");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Pcompra).HasColumnName("preciocompra");
            });

            modelBuilder.Entity<GrupoLinea>(entity =>
            {
                entity.ToTable("grupolinea", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_gruposlineas");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<LineaArticulo>(entity =>
            {
                entity.ToTable("lineaarticulo", "dbo");
                entity.HasKey(e => e.IdLineasArticulos);
                entity.Property(e => e.IdLineasArticulos).HasColumnName("id_lineasarticulos");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.GrupoLinea).HasColumnName("grupolinea");
                entity.Property(e => e.PrcComision).HasColumnName("prcComision");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("doctoscc", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_pedido");
                entity.Property(e => e.ClaveCliente).HasColumnName("id_cliente");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Total).HasColumnName("total");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Moneda).HasColumnName("moneda");
                entity.Property(e => e.Almacen).HasColumnName("almacen");
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.ToTable("doctoscc_det");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_detalle");
                entity.Property(e => e.PedidoId).HasColumnName("id_pedido");
                entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.PrecioUnitario).HasColumnName("precio");
            });

            // --- Configuración mapeada para la entidad Almacen ---
            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.ToTable("almacen", "dbo");
                entity.HasKey(e => e.IdAlmacen);
                entity.Property(e => e.IdAlmacen).HasColumnName("id_almacen");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Calle).HasColumnName("calle");
                entity.Property(e => e.Ciudad).HasColumnName("ciudad");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Pais).HasColumnName("pais");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.Leyendatickt).HasColumnName("leyendatickt");
                entity.Property(e => e.Default).HasColumnName("default");
            });
            modelBuilder.Entity<InvFisico>(entity =>
            {
                entity.ToTable("invfisico", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_invfisico").ValueGeneratedOnAdd();
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Usuario).HasColumnName("usuario").HasDefaultValue(0);
                entity.Property(e => e.DateRecord).HasColumnName("daterecord").HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<InvFisDet>(entity =>
            {
                entity.ToTable("invfisdet", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_invfisdet").ValueGeneratedOnAdd();
                entity.Property(e => e.Articulo).HasColumnName("articulo").IsRequired();
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.DateRecord).HasColumnName("daterecord").HasDefaultValueSql("getdate()");
                entity.Property(e => e.InvFisico).HasColumnName("invfisico");
                entity.Property(e => e.CantidadReal).HasColumnName("cantidad_real");

                // Opcional: Configurar el índice único compuesto si lo requiere EF Core para restricciones lógicas
                entity.HasIndex(e => new { e.Articulo, e.InvFisico }).IsUnique();
            });

            modelBuilder.Entity<Sisvta.Domain.Entities.StatusServicio>(entity =>
            {
                entity.ToTable("status_servicios");
                entity.HasKey(e => e.IdStatusServicios);

                entity.Property(e => e.IdStatusServicios)
                      .HasColumnName("id_status_servicios");

                entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .HasMaxLength(15);
                //.HasCollation("Modern_Spanish_CI_AS");

                entity.Property(e => e.Descripcion)
                      .HasColumnName("descripcion")
                      .HasMaxLength(50);
                      //.HasCollation("Modern_Spanish_CI_AS");

                entity.Property(e => e.Daterecord)
                      .HasColumnName("daterecord")
                      .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("servicios");
                entity.HasKey(e => e.IdServicio);

                entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
                entity.Property(e => e.Clave).HasColumnName("clave").IsRequired();
                entity.Property(e => e.Nombre).HasColumnName("s_nombre").IsRequired();
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.LineaArticulo).HasColumnName("lineaarticulo");
                entity.Property(e => e.UMedidaCompra).HasColumnName("umedidacompra");
                entity.Property(e => e.UMedidaVenta).HasColumnName("umedidaventa");
                entity.Property(e => e.Inventariable).HasColumnName("inventariable");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.ClaveProServ).HasColumnName("ClaveProServ");
                // Agrega el resto de propiedades si prefieres mapearlas explícitamente aquí 
                // en lugar de usar los atributos [Column] en la entidad.
            });

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.ToTable("concepto");
                entity.HasKey(e => e.IdConceptosProv);
                entity.Property(e => e.IdConceptosProv).HasColumnName("id_conceptosprov");
                entity.Property(e => e.DescripcionConcepto).HasColumnName("concepto");
                entity.Property(e => e.ConceptoAbreviado).HasColumnName("conceptoabreviado");
                entity.Property(e => e.Naturaleza).HasColumnName("naturaleza");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.DateRecord).HasColumnName("DateRecord");
            });
        }
    }
}