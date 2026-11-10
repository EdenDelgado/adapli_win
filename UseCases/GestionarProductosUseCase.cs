using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.UseCases.DTOs;

namespace Sisvta.UseCases
{
    public class GestionarProductosUseCase
    {
        private readonly IProductoRepository _repo;

        public GestionarProductosUseCase(IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync() => await _repo.ObtenerTodosAsync();

        public async Task<Producto?> ObtenerPorIdAsync(string id) => await _repo.ObtenerPorIdAsync(id);

        public async Task<bool> RegistrarAsync(Producto producto) => await _repo.InsertarAsync(producto);

        public async Task<bool> InsertarAsync(Producto producto) => await _repo.InsertarAsync(producto);

        public async Task<bool> ActualizarAsync(Producto producto) => await _repo.ActualizarAsync(producto);

        public async Task<bool> EliminarAsync(string id) => await _repo.EliminarAsync(id);

        public async Task<IEnumerable<ProductoDto>> BuscarAsync(string filtro) => await _repo.BuscarAsync(filtro);

        public async Task<IEnumerable<Producto>> ObtenerPorLineaAsync(int linea) => await _repo.ObtenerPorLineaAsync(linea);

        // Actualizado para recibir el grupoFiltro como string y reenviarlo al repositorio
        public async Task<IEnumerable<ProductoDto>> ObtenerFiltradosDtoAsync(string lineaFiltro, int? status, string clavePrefix, string grupoFiltro = null) =>
            await _repo.ObtenerFiltradosDtoAsync(lineaFiltro, status, clavePrefix, grupoFiltro);
    }
}