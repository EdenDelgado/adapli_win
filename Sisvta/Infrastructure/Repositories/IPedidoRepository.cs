using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sisvta.UseCases.DTO;

namespace Sisvta.Infrastructure.Repositories;

public interface IPedidoRepository
{
    Task<PedidoDto?> ObtenerCabeceraAsync(int idPedido);
    Task<string> ObtenerNombreClienteAsync(int idCliente);
    Task<IEnumerable<PedidoDetalleDto>> ObtenerDetalleAsync(int idPedido);
    Task<int> ObtenerUltimoIdPedidoAsync();
    Task<bool> InsertarPedidoCabeceraAsync(int idPedido, string cliente, DateTime fecha);
    Task<bool> AgregarDetalleAsync(int idPedido, int item, string codigoBarra, decimal cantidad, decimal precio);
    Task<bool> ActualizarDetalleAsync(int idPedido, int item, string codigoBarra, decimal cantidad, decimal precio);
    Task<bool> EliminarDetalleAsync(int idPedido, int item);
    Task<int> ImportarTicketsPendientesAsync();
    Task<int> ImportarDesdeTxtAsync(string rutaArchivo);
}