using System.Collections.Generic;

using System.Threading.Tasks;
using Sisvta.Domain.Entities;

namespace Sisvta.UseCases;

public interface IConsultarProductosUseCase
{
    /// <summary>
    /// Obtiene la lista completa de productos disponibles.
    /// </summary>
    Task<IEnumerable<Producto>> ObtenerTodosAsync();

    /// <summary>
    /// Busca y retorna un producto específico por su código o ID.
    /// </summary>
    Task<Producto?> ObtenerPorIdAsync(string id);
}