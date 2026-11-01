using Sisvta.Domain.Entities;

namespace Sisvta.UseCases;

public interface IObtenerTotalVentasQuery
{
    IEnumerable<VentaPorClienteFecha> Ejecutar(DateTime desde, DateTime hasta, int? idCliente);
}