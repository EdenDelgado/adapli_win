using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.UseCases.DTO; // Asegúrate de importar el namespace de los DTOs si es necesario

namespace Sisvta.Domain.Interfaces
{
    public interface IGrupoLineaRepository
    {
        Task<IEnumerable<GrupoLineaDto>> ObtenerTodosAsync();

        // Agregamos esta línea para declarar el método de guardado en la interfaz
        Task GuardarAsync(GrupoLineaDto grupoDto);
    }
}