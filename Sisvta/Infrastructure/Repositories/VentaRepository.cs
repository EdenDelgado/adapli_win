namespace Sisvta.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Sisvta.Domain.Entities;

public class VentaRepository : IVentaRepository
{
    private readonly string _connectionString;

    public VentaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<VentaPorClienteFecha> ObtenerVentasPorFechaYCliente(DateTime? fechaDesde, DateTime? fechaHasta, string? idCliente)
    {
        var resultado = new List<VentaPorClienteFecha>();

        string query = @"
            SELECT 
                CAST(p.fechafactura AS DATE) AS Fecha,
                p.cliente AS IdCliente,
                ISNULL(c.nombre, 'Sin Nombre') AS Nombre,
                COUNT(DISTINCT p.id_factura) AS CantidadPedidos,
                SUM(d.cantidad * d.preciou) AS TotalVendido
            FROM dbo.doctoscc p
            INNER JOIN dbo.doctoscc_det d ON p.id_factura = d.id_factura
            LEFT JOIN dbo.clientes c ON p.cliente = c.id_empros
            WHERE (@FechaDesde IS NULL OR CAST(p.fechafactura AS DATE) >= @FechaDesde)
              AND (@FechaHasta IS NULL OR CAST(p.fechafactura AS DATE) <= @FechaHasta)
              AND (@IdCliente IS NULL OR p.cliente = @IdCliente)
            GROUP BY p.fechafactura, p.cliente, c.nombre
            ORDER BY Fechafactura DESC, TotalVendido DESC;";

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@FechaDesde", (object?)fechaDesde?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaHasta", (object?)fechaHasta?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCliente", string.IsNullOrWhiteSpace(idCliente) ? DBNull.Value : idCliente);

            con.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    resultado.Add(new VentaPorClienteFecha
                    {
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        IdCliente = reader["IdCliente"].ToString() ?? string.Empty,
                        Nombre = reader["Nombre"].ToString() ?? string.Empty,
                        CantidadPedidos = Convert.ToInt32(reader["CantidadPedidos"]),
                        TotalVendido = Convert.ToDecimal(reader["TotalVendido"])
                    });
                }
            }
        }

        return resultado;
    }

    // Sobrecarga para int? que reutiliza la lógica principal convirtiéndolo a string
    public IEnumerable<VentaPorClienteFecha> ObtenerVentasPorFechaYCliente(DateTime? fechaDesde, DateTime? fechaHasta, int? idCliente)
    {
        string? idClienteStr = idCliente?.ToString();
        return ObtenerVentasPorFechaYCliente(fechaDesde, fechaHasta, idClienteStr);
    }
}