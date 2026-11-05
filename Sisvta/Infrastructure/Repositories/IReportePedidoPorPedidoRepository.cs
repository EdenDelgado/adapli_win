using System.Collections.Generic;
using Sisvta.UseCases.DTO;

namespace Sisvta.UseCases
{
    public interface IReportePedidoPorPedidoRepository
    {
        IEnumerable<ReportePedidoItemDto> ObtenerDatosReporte();
    }
}