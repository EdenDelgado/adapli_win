using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;

namespace Sisvta.UseCases
{
    public class ConsultarClientesUseCase : IConsultarClientesUseCase
    {
        private readonly IClienteRepository _repository;

        public ConsultarClientesUseCase(IClienteRepository repository)
        {
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.ObtenerTodosAsync(cancellationToken);
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Propagamos correctamente el token de cancelación
            return await _repository.ObtenerPorIdAsync(id, cancellationToken);
        }
    }
}