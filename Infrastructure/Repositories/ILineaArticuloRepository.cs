using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.UseCases.DTOs;

namespace Sisvta.Domain.Interfaces
{
 
    public interface ILineaArticuloRepository
    {
        Task<IEnumerable<LineaArticuloDto>> ObtenerTodosAsync();
        Task CrearAsync(string nombre, string grupoLinea, decimal comision,string idempresa);
        Task ActualizarAsync(int id, string nombre, string grupoLinea, decimal comision,string idempresa);
    }
}