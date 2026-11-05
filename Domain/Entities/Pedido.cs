using System;
using System.Collections.Generic;
using System.Text;

namespace Sisvta.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public string ClaveCliente { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }
    public decimal Total { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string Moneda { get; private set; } = string.Empty;
    public string Almacen { get; private set; } = string.Empty;

    // Propiedad de navegación para EF Core (relación con los detalles)
    private readonly List<DetallePedido> _detalles = new();
    public IReadOnlyCollection<DetallePedido> Detalles => _detalles.AsReadOnly();

    // Constructor vacío requerido por EF Core
    protected Pedido() { }

    public Pedido(string claveCliente, DateTime fecha, decimal total)
    {
        ClaveCliente = claveCliente;
        Fecha = fecha;
        Total = total;
    }
}
