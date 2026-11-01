using System.Collections.Generic;
using Sisvta.UseCases.DTO;
using Sisvta.UseCases;

namespace Sisvta.UseCases
{
    public class ReportePedidoPorPedidoUseCase
    {
        private readonly IReportePedidoPorPedidoRepository _repository;

        public ReportePedidoPorPedidoUseCase(IReportePedidoPorPedidoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ReportePedidoItemDto> Ejecutar()
        {
            // Aquí puedes agregar reglas de negocio adicionales si las necesitas antes de entregar al formulario
            return _repository.ObtenerDatosReporte();
        }
    }
}