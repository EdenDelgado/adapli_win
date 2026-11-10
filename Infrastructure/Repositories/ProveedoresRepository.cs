using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sisvta.Infrastructure.Repositories
{
    public class ProveedoresRepository : IProveedoresRepository
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Proveedores>> ObtenerTodosAsync()
        {
            return await _context.Proveedores.AsNoTracking().ToListAsync();
        }

        public async Task<Proveedores?> ObtenerPorIdAsync(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        public async Task AgregarAsync(Proveedores proveedor)
        {
            if (proveedor == null) throw new ArgumentNullException(nameof(proveedor));

            await _context.Proveedores.AddAsync(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Proveedores proveedor)
        {
            if (proveedor == null) throw new ArgumentNullException(nameof(proveedor));

            // Verificamos si el contexto ya tiene una instancia rastreada con este mismo ID en memoria local
            var trackedEntity = _context.Set<Proveedores>()
                .Local
                .FirstOrDefault(p => p.Id == proveedor.Id);

            if (trackedEntity != null)
            {
                // Si ya existe en memoria, copiamos los nuevos valores sobre la instancia rastreada
                _context.Entry(trackedEntity).CurrentValues.SetValues(proveedor);
            }
            else
            {
                // Si no está en memoria, usamos Update de forma segura
                _context.Proveedores.Update(proveedor);
            }

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
    }
}