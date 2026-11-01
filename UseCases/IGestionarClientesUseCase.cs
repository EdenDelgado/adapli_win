using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;

namespace Sisvta.UseCases; // Ajusta el namespace según la estructura de tu proyecto

public interface IGestionarClientesUseCase
{
    Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> RegistrarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}