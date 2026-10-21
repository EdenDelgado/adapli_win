using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.UseCases.DTOs;

namespace Sisvta.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(string id);
        Task<bool> InsertarAsync(Producto producto);
        Task<bool> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(string id);
        Task<IEnumerable<ProductoDto>> BuscarAsync(string filtro);
        Task<IEnumerable<Producto>> ObtenerPorLineaAsync(int linea);
        Task<IEnumerable<Producto>> ObtenerPorLineaYStatusAsync(int? linea, int? status);

        // Cambiamos esta línea en la interfaz:
      
 
        Task<IEnumerable<ProductoDto>> ObtenerFiltradosDtoAsync(string lineaFiltro, int? status, string clavePrefix, string grupoFiltro = null);
        
    }
}