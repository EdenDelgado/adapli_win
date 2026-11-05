using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;

namespace Sisvta.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> InsertarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ActualizarAsync(Cliente cliente)
    {
        // Validación de rastreo para evitar el error de entidades duplicadas en memoria
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