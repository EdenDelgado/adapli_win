using System;
using System.Collections.Generic;
using System.Text;

namespace Sisvta.UseCases.DTO

{
    public class ReportePedidoItemDto
    {
        public int Id_Factura { get; set; }
        public DateTime FechaFactura { get; set; }
        public string IdCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public int Detallefatura { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
    }
}
