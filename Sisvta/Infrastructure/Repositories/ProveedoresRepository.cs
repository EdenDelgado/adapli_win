using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
//using Sisvta.Infrastructure.Persistence Data; // Ajusta este namespace según la ubicación de tu DbContext
using Sisvta.Infrastructure.Persistence.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sisvta.Infrastructure.Repositories
{
    public class ProveedoresRepository : IProveedoresRepository
    {
        private readonly ApplicationDbContext _context; // Cambia "ApplicationDbContext" por el nombre de tu DbContext actual

        public ProveedoresRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Proveedores>> ObtenerTodosAsync()
        {
            return await _context.Proveedores.ToListAsync();
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

            _context.Proveedores.Update(proveedor);
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