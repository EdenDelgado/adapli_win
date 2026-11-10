using System;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;

namespace Sisvta.UseCases
{
    public class GestionarProveedoresUseCase
    {
        private readonly IProveedoresRepository _proveedoresRepository;

        // Inyectamos el repositorio en lugar del DbContext
        public GestionarProveedoresUseCase(IProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository ?? throw new ArgumentNullException(nameof(proveedoresRepository));
        }

        public async Task GuardarAsync(Proveedores proveedor)
        {
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