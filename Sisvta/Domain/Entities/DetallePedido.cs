using System;
using System.Collections.Generic;
using System.Text;

namespace Sisvta.Domain.Entities;

public class DetallePedido
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public string IdServicio { get; private set; } = string.Empty;
    public decimal Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }

    // Constructor vacío requerido por EF Core
    protected DetallePedido() { }

    public DetallePedido(int pedidoId, string idServicio, decimal cantidad, decimal precioUnitario)
    {
        PedidoId = pedidoId;
        IdServicio = idServicio;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }
}
