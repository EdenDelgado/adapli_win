using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;

namespace Sisvta.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
        Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> InsertarAsync(Cliente cliente);
        Task<bool> ActualizarAsync(Cliente cliente);
        Task<bool> EliminarAsync(int id);
    }
}