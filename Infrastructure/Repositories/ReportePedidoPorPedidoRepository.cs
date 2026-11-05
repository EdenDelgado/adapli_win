namespace Sisvta.Infrastructure.Repositories
{
    using Microsoft.Data.SqlClient;
    using Sisvta.UseCases;
    using Sisvta.UseCases.DTO;
   
    using System;
    using System.Collections.Generic;

    public class ReportePedidoPorPedidoRepository : IReportePedidoPorPedidoRepository
    {
        private readonly string _connectionString;

        public ReportePedidoPorPedidoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ReportePedidoItemDto> ObtenerDatosReporte()
        {
            var lista = new List<ReportePedidoItemDto>();
            string query = @"
                SELECT 
                    p.id_factura AS Id_Factura,
                    p.fechafactura AS FechaFactura,
                    c.id_empros AS IdCliente,
                    c.nombre AS NombreCliente,
                    d.id_detallefatura AS DetalleFatura,
                    d.idprodserv AS Clave,
                    ISNULL(prod.descripcion, 'SIN DESCRIPCION') AS Descripcion,
                    d.cantidad AS Cantidad,
                    d.preciou AS Precio,
                    (d.cantidad * d.preciou) AS Importe
                FROM dbo.doctoscc p
                INNER JOIN dbo.clientes c ON p.cliente = c.id_empros
                INNER JOIN dbo.doctoscc_det d ON p.id_factura = d.id_factura
                LEFT JOIN dbo.servicios prod ON d.idprodserv = prod.id_servicio
                WHERE p.id_factura IS NOT NULL AND d.id_detallefatura IS NOT NULL AND d.cantidad > 0
                ORDER BY p.id_factura ASC, d.id_detallefatura ASC;";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ReportePedidoItemDto
                        {
                            Id_Factura = reader["Id_Factura"] != DBNull.Value ? Convert.ToInt32(reader["Id_Factura"]) : 0,
                            FechaFactura = reader["FechaFactura"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFactura"]) : DateTime.Now,
                            IdCliente = reader["IdCliente"] != DBNull.Value ? reader["IdCliente"].ToString() ?? string.Empty : string.Empty,
                            NombreCliente = reader["NombreCliente"] != DBNull.Value ? reader["NombreCliente"].ToString() ?? string.Empty : string.Empty,
                            Detallefatura = reader["Detallefatura"] != DBNull.Value ? Convert.ToInt32(reader["Detallefatura"]) : 0,
                            Clave = reader["Clave"] != DBNull.Value ? reader["Clave"].ToString() ?? string.Empty : string.Empty,
                            Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() ?? string.Empty : string.Empty,
                            Cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToDecimal(reader["Cantidad"]) : 0m,
                            Precio = reader["Precio"] != DBNull.Value ? Convert.ToDecimal(reader["Precio"]) : 0m,
                            Importe = reader["Importe"] != DBNull.Value ? Convert.ToDecimal(reader["Importe"]) : 0m
                        });
                    }
                }
            }

            return lista;
        }
    }
}