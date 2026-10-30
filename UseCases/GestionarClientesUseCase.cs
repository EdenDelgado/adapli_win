using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;

namespace Sisvta.UseCases
{
    public class GestionarClientesUseCase : IGestionarClientesUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public GestionarClientesUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            return await _clienteRepository.ObtenerTodosAsync(cancellationToken);
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _clienteRepository.ObtenerPorIdAsync(id, cancellationToken);
        }

        public async Task<bool> RegistrarAsync(Cliente cliente, CancellationToken cancellationToken = default)
        {
            // Cambia 'InsertarAsync' por el nombre real de tu método en el repositorio si es distinto (ej. AgregarAsync)
            return await _clienteRepository.InsertarAsync(cliente);
        }

        public async Task<bool> ActualizarAsync(Cliente cliente, CancellationToken cancellationToken = default)
        {
            return await _clienteRepository.ActualizarAsync(cliente);
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _clienteRepository.EliminarAsync(id);
        }
    }
}