using Sisvta.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sisvta.Domain.Interfaces
{
    public interface IProveedoresRepository
    {
        Task<IEnumerable<Proveedores>> ObtenerTodosAsync();
        Task<Proveedores?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Proveedores proveedor);
        Task ActualizarAsync(Proveedores proveedor);
        Task EliminarAsync(int id);
    }
}