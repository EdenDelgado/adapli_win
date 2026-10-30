using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.Domain.Interfaces;
using Sisvta.UseCases.DTOs;

namespace Sisvta.UseCases
{
    public class GestionarLineasArticulosUseCase
    {
        private readonly ILineaArticuloRepository _repo;

        public GestionarLineasArticulosUseCase(ILineaArticuloRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<LineaArticuloDto>> ObtenerTodosAsync() =>
            await _repo.ObtenerTodosAsync();

        // Actualizado para aceptar los 4 argumentos que envía Flin
        public async Task CrearAsync(string nombre, string grupoLinea, decimal comision, string idempresa)
        {
            await _repo.CrearAsync(nombre, grupoLinea, comision, idempresa);
        }

        // Actualizado para aceptar los 5 argumentos (Id + 4 datos)
        public async Task ActualizarAsync(int id, string nombre, string grupoLinea, decimal comision, string idempresa)
        {
            await _repo.ActualizarAsync(id, nombre, grupoLinea, comision, idempresa);
        }
    }
}