namespace Sisvta.UseCases.DTO;

public record PedidoDto(int Cliente, DateTime FechaFactura, string Ndet, string Status, string Moneda, string Almacen);
