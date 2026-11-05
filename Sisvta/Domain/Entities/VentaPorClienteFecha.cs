namespace Sisvta.Domain.Entities
{
    public class VentaPorClienteFecha
    {
        public DateTime Fecha { get; set; }
        public String IdCliente { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int CantidadPedidos { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
