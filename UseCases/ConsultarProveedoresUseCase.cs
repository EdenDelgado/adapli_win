using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sisvta
{
    public interface IConsultarProveedoresUseCase
    {
        Task<IEnumerable<Proveedores>> ObtenerTodosAsync();
        Task<Proveedores?> ObtenerPorIdAsync(int id);
    }

    public class ConsultarProveedoresUseCase : IConsultarProveedoresUseCase
    {
        private readonly IProveedoresRepository _proveedoresRepository;

        public ConsultarProveedoresUseCase(IProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository ?? throw new ArgumentNullException(nameof(proveedoresRepository));
        }

        public async Task<IEnumerable<Proveedores>> ObtenerTodosAsync()
        {
            return await _proveedoresRepository.ObtenerTodosAsync();
        }

        public async Task<Proveedores?> ObtenerPorIdAsync(int id)
        {
            return await _proveedoresRepository.ObtenerPorIdAsync(id);
        }
    }
}