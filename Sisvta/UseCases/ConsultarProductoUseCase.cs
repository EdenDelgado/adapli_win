using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;

namespace Sisvta.UseCases;

public class ConsultarProductosUseCase : IConsultarProductosUseCase
{
    private readonly IProductoRepository _repository;

    public ConsultarProductosUseCase(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(string id)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }
}