using Sisvta.Domain.Entities;

namespace Sisvta.Infrastructure.Repositories
{
    public interface IVentaRepository
    {
        IEnumerable<VentaPorClienteFecha> ObtenerVentasPorFechaYCliente(DateTime? fechaDesde, DateTime? fechaHasta, int? idCliente);
    }
}