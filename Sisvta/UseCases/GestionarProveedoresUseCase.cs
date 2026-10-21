using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sisvta.UseCases
{
    public class GestionarProveedoresUseCase
    {
        private readonly IProveedoresRepository _proveedoresRepository;

        public GestionarProveedoresUseCase(IProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository ?? throw new ArgumentNullException(nameof(proveedoresRepository));
        }

        public async Task<Proveedores?> ObtenerPorIdAsync(int id)
        {
            return await _proveedoresRepository.ObtenerPorIdAsync(id);
        }

        public async Task GuardarAsync(Proveedores proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            if (proveedor.Id == 0)
            {
                await _proveedoresRepository.AgregarAsync(proveedor);
            }
            else
            {
                await _proveedoresRepository.ActualizarAsync(proveedor);
            }
        }

        public async Task EliminarAsync(int id)
        {
            await _proveedoresRepository.EliminarAsync(id);
        }
    }
}