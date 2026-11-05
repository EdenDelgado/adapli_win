namespace Sisvta.UseCases;

using System.Collections.Generic;
using Sisvta.UseCases.DTO;
using Sisvta.Infrastructure.Repositories;   

public class GenerarReporteClientesUseCase
{
    private readonly IReportePedidoRepository _repo;

    public GenerarReporteClientesUseCase(IReportePedidoRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<ReportePedidoItemDto> Ejecutar()
    {
        return _repo.ObtenerDatosReporteClientes();
    }
}