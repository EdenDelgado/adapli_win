using System;
using System.Collections.Generic;
using Sisvta.Infrastructure.Repositories;
using Sisvta.Domain.Entities;

namespace Sisvta.UseCases
{
    public class ObtenerTotalVentasQuery : IObtenerTotalVentasQuery
    {
        private readonly IVentaRepository _ventaRepository;

        public ObtenerTotalVentasQuery(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        // Método principal (con tipos nulables)
        public IEnumerable<VentaPorClienteFecha> Ejecutar(DateTime? fechaDesde, DateTime? fechaHasta, int? idCliente)
        {
            return _ventaRepository.ObtenerVentasPorFechaYCliente(fechaDesde, fechaHasta, idCliente);
        }

        // Sobrecarga con DateTime normal -> Usamos un cast explícito (DateTime?) para invocar al método principal correcto
        public IEnumerable<VentaPorClienteFecha> Ejecutar(DateTime desde, DateTime hasta, int? idCliente)
        {
            return Ejecutar((DateTime?)desde, (DateTime?)hasta, idCliente);
        }

        // Sobrecarga con ID en string -> Convierte a int? y hace el cast correcto
        public IEnumerable<VentaPorClienteFecha> Ejecutar(DateTime desde, DateTime hasta, string? idCliente)
        {
            int? idInt = int.TryParse(idCliente, out var parsed) ? parsed : null;
            return Ejecutar((DateTime?)desde, (DateTime?)hasta, idInt);
        }
    }
}