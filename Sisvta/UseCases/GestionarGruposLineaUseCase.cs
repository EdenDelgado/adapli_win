using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.Domain.Interfaces;
using Sisvta.UseCases.DTO;

namespace Sisvta.UseCases
{
    public class GestionarGruposLineasUseCase
    {
        private readonly IGrupoLineaRepository _repo;

        public GestionarGruposLineasUseCase(IGrupoLineaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GrupoLineaDto>> ObtenerTodosAsync() =>
            await _repo.ObtenerTodosAsync();

        // Método que faltaba en el Caso de Uso
        public async Task GuardarAsync(GrupoLineaDto grupoDto)
        {
            await _repo.GuardarAsync(grupoDto);
        }
    }
}