using Sisvta.Domain.Entities;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Repositories;
using Sisvta.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sisvta.UseCases;

public class GestionarPedidosUseCase : IGestionarPedidosUseCase
{
    private readonly IPedidoRepository _repo;
    public GestionarPedidosUseCase(IPedidoRepository repo) => _repo = repo;

    public async Task<(PedidoDto? Cabecera, string NomCliente, IEnumerable<PedidoDetalleDto> Detalle)> CargarCompletoAsync(int idPedido)
    {
        var cabecera = await _repo.ObtenerCabeceraAsync(idPedido);
        string nomCliente = string.Empty;
        IEnumerable<PedidoDetalleDto> detalle = Array.Empty<PedidoDetalleDto>();

        if (cabecera != null)
        {
            // Como cabecera.Cliente ya es int, lo usamos directo sin el out
            nomCliente = await _repo.ObtenerNombreClienteAsync(cabecera.Cliente);
            detalle = await _repo.ObtenerDetalleAsync(idPedido);
        }

        return (cabecera, nomCliente, detalle);
    }

    public Task<int> SiguienteIdPedidoAsync() => _repo.ObtenerUltimoIdPedidoAsync();
    public Task<bool> GuardarCabeceraAsync(int id, string cli, DateTime fec) => _repo.InsertarPedidoCabeceraAsync(id, cli, fec);
    public Task<bool> AgregarItemAsync(int idPed, int item, string cod, decimal cant, decimal pre) => _repo.AgregarDetalleAsync(idPed, item, cod, cant, pre);
    public Task<bool> ActualizarItemAsync(int idPed, int item, string cod, decimal cant, decimal pre) => _repo.ActualizarDetalleAsync(idPed, item, cod, cant, pre);
    public Task<bool> EliminarItemAsync(int idPed, int item) => _repo.EliminarDetalleAsync(idPed, item);
    public Task<int> ImportarTodosAsync() => _repo.ImportarTicketsPendientesAsync();
    public Task<int> ImportarTxtAsync(string ruta) => _repo.ImportarDesdeTxtAsync(ruta);

    public Task<IEnumerable<Pedido>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Pedido?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegistrarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ActualizarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}