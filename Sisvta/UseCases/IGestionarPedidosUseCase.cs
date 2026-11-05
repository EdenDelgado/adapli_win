using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;

namespace Sisvta.UseCases; // O el namespace correspondiente a tu capa de Application

public interface IGestionarPedidosUseCase
{
    Task<IEnumerable<Pedido>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<Pedido?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> RegistrarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}