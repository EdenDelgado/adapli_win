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
        }
    }
}