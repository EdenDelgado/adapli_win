namespace Sisvta.UseCases;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;

public class ConsultarClientesUseCase : IConsultarClientesUseCase
{
    private readonly IClienteRepository _repository;

    public ConsultarClientesUseCase(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ObtenerTodosAsync(cancellationToken);
    }

    public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }

    public async Task<bool> ActualizarAsync(Cliente cliente)
    {
        return await _repository.ActualizarAsync(cliente);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        // Si tu repositorio espera un string para eliminar, usa id.ToString() o cámbialo según corresponda
        return await _repository.EliminarAsync(id);
    }
}