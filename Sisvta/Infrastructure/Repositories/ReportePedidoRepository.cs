using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sisvta.UseCases;
using Sisvta.UseCases.DTO;
using System;
using System.Collections.Generic;

namespace Sisvta.Infrastructure.Repositories
{
    public class ReportePedidoRepository : IReportePedidoRepository
    {
        private readonly string _connectionString;



        public ReportePedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? "Server=localhost\\sqlexpress; Database=adaplirenelomeli; User Id=SA; Password=alexis19; TrustServerCertificate=True;";
        }

        // Método que ya tenías para el reporte detallado por pedido
        public IEnumerable<ReportePedidoItemDto> ObtenerDatosReporte()
        {
            var lista = new List<ReportePedidoItemDto>();
            string query = @"
                SELECT 
                    p.id_factura AS IdPedido,
                    p.fechafactura AS FechaPedido,
                    c.id_empros AS IdCliente,
                    c.nombre AS NombreCliente,
                    d.id_detallefatura AS Item,
                    d.idprodserv AS CodigoBarra,
                    ISNULL(prod.descripcion, 'SIN DESCRIPCION') AS Descripcion,
                    d.cantidad AS Cantidad,
                    d.preciou AS Precio,
                    (d.cantidad * d.preciou) AS ImporteItem
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
                            Id_Factura = reader["Id_Factura"] != DBNull.Value ? Convert.ToInt32(reader["IdPedido"]) : 0,
                            FechaFactura = reader["FechaFactura"] != DBNull.Value ? Convert.ToDateTime(reader["FechaPedido"]) : DateTime.Now,
                            IdCliente = reader["IdCliente"] != DBNull.Value ? reader["IdCliente"].ToString() ?? string.Empty : string.Empty,
                            NombreCliente = reader["NombreCliente"] != DBNull.Value ? reader["NombreCliente"].ToString() ?? string.Empty : string.Empty,
                            Detallefatura = reader["Detallefatura"] != DBNull.Value ? Convert.ToInt32(reader["Item"]) : 0,
                            Clave = reader["Clave"] != DBNull.Value ? reader["CodigoBarra"].ToString() ?? string.Empty : string.Empty,
                            Descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() ?? string.Empty : string.Empty,
                            Cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToDecimal(reader["Cantidad"]) : 0m,
                            Precio = reader["Precio"] != DBNull.Value ? Convert.ToDecimal(reader["Precio"]) : 0m,
                            Importe = reader["Importe"] != DBNull.Value ? Convert.ToDecimal(reader["ImporteItem"]) : 0m
                        });
                    }
                }
            }

            return lista;
        }

        // 👈 NUEVO MÉTODO: Específico para agrupar y alimentar el reporte por cliente
        public IEnumerable<ReportePedidoItemDto> ObtenerDatosReporteClientes()
        {
            var lista = new List<ReportePedidoItemDto>();
            string query = @"
                SELECT 
                    c.id_empros AS IdCliente,
                    c.nombre AS NombreCliente,
                    p.id_factura AS Id_Factura,
                    p.fechafactura AS FechaFactura,
                    SUM(d.cantidad * d.preciou) AS Importe
                FROM dbo.clientes c
                INNER JOIN dbo.doctoscc p ON c.id_empros = p.cliente
                INNER JOIN dbo.doctoscc_det d ON p.id_factura = d.id_factura
                GROUP BY c.id_empros, c.nombre, p.id_factura, p.fechafactura
                ORDER BY c.id_empros ASC, p.id_factura ASC;";

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
                            IdCliente = reader["IdCliente"] != DBNull.Value ? reader["IdCliente"].ToString() ?? string.Empty : string.Empty,
                            NombreCliente = reader["NombreCliente"] != DBNull.Value ? reader["NombreCliente"].ToString() ?? string.Empty : string.Empty,
                            Id_Factura = reader["Id_Factura"] != DBNull.Value ? Convert.ToInt32(reader["Id_Factura"]) : 0,
                            FechaFactura = reader["FechaFactura"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFactura"]) : DateTime.Now,
                            Importe = reader["Importe"] != DBNull.Value ? Convert.ToDecimal(reader["Importe"]) : 0m
                        });
                    }
                }
            }

            return lista;
        }
    }
}