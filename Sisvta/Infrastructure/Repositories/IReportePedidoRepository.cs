using System.Collections.Generic; // <-- ¡Esta línea es la que falta!
using Sisvta.UseCases.DTO;

namespace Sisvta.UseCases
{
    public interface IReportePedidoRepository
    {
        IEnumerable<ReportePedidoItemDto> ObtenerDatosReporte();
        IEnumerable<ReportePedidoItemDto> ObtenerDatosReporteClientes();
    }
}