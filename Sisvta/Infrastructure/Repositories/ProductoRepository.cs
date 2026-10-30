using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.UseCases.DTOs;

namespace Sisvta.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context) => _context = context;

        public async Task<bool> ActualizarAsync(Producto producto)
        {
            // 1. Verificamos si EF Core ya tiene una instancia con la misma clave en memoria (localmente)
            var trackedEntity = _context.Set<Producto>().Local.FirstOrDefault(p => p.Clave == producto.Clave);

            if (trackedEntity != null)
            {
                // Si ya está rastreada, copiamos los nuevos valores a la entidad existente en memoria
                _context.Entry(trackedEntity).CurrentValues.SetValues(producto);
            }
            else
            {
                // Si no está rastreada, la actualizamos de forma normal
                _context.Set<Producto>().Update(producto);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ProductoDto>> BuscarAsync(string filtro)
        {
            var sql = @"SELECT claveproserv, clave, id_servicio as Prodserv, s_nombre as nombre, s.descripcion as Descripcion,
                        inventariable, lineaArticulo as Linea, l.nombre as nomlinea, l.grupolinea as grupo, g.nombre as nomgrupo, umedidacompra as umedida,
                        
                        status,ss.status as Nomstatus, ISNULL(preciocompra, 0.00) as Pcompra, 
                        ISNULL(p.precioventa, 0.00) as Pventa, /* p.moneda as idmoneda, m.moneda as Moneda,*/
                        maximo as Maximo, minimo as Minimo, reorden as Reorden,u.nombre as Nommedida
                        FROM dbo.servicios 
                        LEFT JOIN lineaarticulo as l ON servicios.lineaarticulo = l.id_lineasarticulos 
                        LEFT JOIN grupolinea as g ON l.grupolinea = g.id_gruposlineas
                        LEFT JOIN preciosarticulo as p ON servicios.id_servicio = p.articulo
                        /*LEFT JOIN moneda as m ON p.moneda = m.id_moneda*/
                        left join status_servicios as ss on s.status=ss.id_status_servicios
                        left join unidadmedida as u on s.umedidacompra=u.id_um
                        WHERE @p0 = '' OR clave LIKE '%' + @p0 + '%' OR s_nombre LIKE '%' + @p0 + '%' OR CONVERT(varchar, g.id_gruposlineas) LIKE @p0 + '%'";

            return await _context.Database
                .SqlQueryRaw<ProductoDto>(sql, filtro ?? string.Empty)
                .ToListAsync();
        }

        public async Task<bool> EliminarAsync(string id)
        {
            var producto = await ObtenerPorIdAsync(id);
            if (producto == null) return false;

            _context.Set<Producto>().Remove(producto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertarAsync(Producto producto)
        {
            // 1. Verificamos si EF Core ya tiene una instancia con la misma clave en memoria local
            var trackedEntity = _context.Set<Producto>().Local.FirstOrDefault(p => p.Clave == producto.Clave);

            if (trackedEntity != null)
            {
                // Si ya está rastreada, simplemente actualizamos sus valores en lugar de hacer un Add duplicado
                _context.Entry(trackedEntity).CurrentValues.SetValues(producto);
            }
            else
            {
                // Si no está rastreada en memoria, verificamos si ya existe en la base de datos
                var existingInDb = await _context.Set<Producto>().FindAsync(producto.Clave);

                if (existingInDb != null)
                {
                    // Si ya existe en la BD pero no estaba en el rastreador local, actualizamos sus valores
                    _context.Entry(existingInDb).CurrentValues.SetValues(producto);
                }
                else
                {
                    // Si es completamente nuevo, lo agregamos normalmente
                    await _context.Set<Producto>().AddAsync(producto);
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Producto?> ObtenerPorIdAsync(string id)
        {
            return await _context.Set<Producto>().FindAsync(id);
        }

        // Método para consultar productos filtrados únicamente por línea (como entero)
        public async Task<IEnumerable<Producto>> ObtenerPorLineaAsync(int linea)
        {
            return await _context.Set<Producto>()
                .Where(p => p.Linea == linea)
                .ToListAsync();
        }

        // Método para filtrar por línea y status simultáneamente
        public async Task<IEnumerable<Producto>> ObtenerPorLineaYStatusAsync(int? linea, int? status)
        {
            var query = _context.Set<Producto>().AsQueryable();

            if (linea.HasValue)
            {
                query = query.Where(p => p.Linea == linea.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        // Método robusto para filtrar por texto de línea, status, prefijo de clave y nombre de grupo opcional
        public async Task<IEnumerable<ProductoDto>> ObtenerFiltradosDtoAsync(string lineaFiltro, int? status, string clavePrefix, string grupoFiltro = null)
        {
            var sql = @"SELECT claveproserv, clave, id_servicio as Prodserv, s_nombre as nombre, s.descripcion as Descripcion,
                        umedidacompra as umedida,
                        inventariable, s.lineaArticulo as Linea, l.nombre as nomlinea, l.grupolinea as grupo, g.nombre as nomgrupo, s.status as status,
                       ss.status as Nomstatus,
                        ISNULL(preciocompra, 0.00) as Pcompra, 
                        ISNULL(p.precioventa, 0.00) as Pventa, /* p.moneda as idmoneda, m.moneda as Moneda,*/
                        s.maximo as Maximo, s.minimo as Minimo, s.reorden as Reorden,u.nombre as Nommedida
                        FROM dbo.servicios as s
                        LEFT JOIN lineaarticulo as l ON s.lineaArticulo = l.id_lineasarticulos 
                        LEFT JOIN grupolinea as g ON l.grupolinea = g.id_gruposlineas
                        LEFT JOIN preciosarticulo as p ON s.id_servicio = p.articulo AND p.listaprecio = 5
                        left join status_servicios as ss on s.status=ss.id_status_servicios
                       /* LEFT JOIN moneda as m ON p.moneda = m.id_moneda*/
                        left join unidadmedida as u on s.umedidacompra=u.id_um
                        WHERE (@p0 = '' OR CONVERT(varchar, s.lineaArticulo) = @p0 OR l.nombre LIKE '%' + @p0 + '%') 
                          AND (@p1 IS NULL OR s.status = @p1) 
                          AND (@p2 = '' OR s.clave LIKE @p2 + '%') 
                          AND (@p3 = '' OR g.nombre LIKE '%' + @p3 + '%')";

            string lineaParam = string.IsNullOrWhiteSpace(lineaFiltro) ? string.Empty : lineaFiltro.Trim();
            string prefixParam = string.IsNullOrWhiteSpace(clavePrefix) ? string.Empty : clavePrefix.Trim();
            string grupoParam = string.IsNullOrWhiteSpace(grupoFiltro) ? string.Empty : grupoFiltro.Trim();

            return await _context.Database
                .SqlQueryRaw<ProductoDto>(sql,
                    lineaParam,
                    status.HasValue ? (object)status.Value : DBNull.Value,
                    prefixParam,
                    grupoParam)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Set<Producto>().ToListAsync();
        }
    }
}