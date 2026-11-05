namespace Sisvta.UseCases.DTO; // Ajusta el namespace según tu estructura de carpetas

public class PedidoDetalleDto
{
    public string Id_factura { get; set; } = string.Empty;
    public int Item { get; set; } = 0;
    public string idprodserv { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Importe { get; set; }

    // Propiedad calculada útil para mostrar el total por línea en el DataGridView
    public decimal Subtotal => Cantidad * Precio;
}