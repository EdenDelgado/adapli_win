using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Blindaje de seguridad en memoria por si alguna propiedad de texto quedó como null
            foreach (var c in clientes)
            {
                c.Nombre = c.Nombre ?? string.Empty;
                c.Domicilio = c.Domicilio ?? string.Empty;
                c.Rfc = c.Rfc ?? string.Empty;
                c.Telefono1 = c.Telefono1 ?? string.Empty;
                c.curp = c.curp ?? string.Empty;
            }

            return clientes;
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (cliente != null)
            {
                cliente.Nombre = cliente.Nombre ?? string.Empty;
                cliente.Domicilio = cliente.Domicilio ?? string.Empty;
                cliente.Rfc = cliente.Rfc ?? string.Empty;
                cliente.Telefono1 = cliente.Telefono1 ?? string.Empty;
                cliente.curp = cliente.curp ?? string.Empty;
            }

            return cliente;
        }

        public async Task<bool> InsertarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            var trackedEntity = _context.Clientes.Local.FirstOrDefault(c => c.Id == cliente.Id);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(cliente);
            }
            else
            {
                _context.Clientes.Update(cliente);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}