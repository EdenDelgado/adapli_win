using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;

namespace Sisvta.UseCases;

public interface IConsultarClientesUseCase
{
    /// <summary>
    /// Obtiene la lista completa de clientes registrados.
    /// </summary>
    Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca y retorna un cliente específico por su identificador único.
    /// </summary>
    Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registra un nuevo cliente en el sistema.
    /// </summary>
   // Task<bool> RegistrarAsync(Cliente cliente);

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    Task<bool> ActualizarAsync(Cliente cliente);

    /// <summary>
    /// Elimina o da de baja a un cliente por su identificador.
    /// </summary>
    Task<bool> EliminarAsync(int id);
}