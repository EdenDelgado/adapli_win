using Microsoft.Data.SqlClient;
using Sisvta.Domain.Interfaces;
using Sisvta.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sisvta.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PedidoDto?> ObtenerCabeceraAsync(int idPedido)
        {
            string query = @"
                SELECT p.cliente as cliente, p.fechafactura as fechafactura, p.status as status,p.moneda as moneda, p.almacen as almacen,
                       (SELECT COUNT(*) FROM dbo.doctoscc_det d WHERE d.id_factura = p.id_factura) AS Ndet
                FROM dbo.doctoscc p 
                WHERE p.id_factura = @id_factura";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id_factura", System.Data.SqlDbType.Int).Value = idPedido;

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new PedidoDto(
                
                    Cliente: reader["cliente"] != DBNull.Value ? Convert.ToInt32(reader["cliente"]) : 0,
                    // CORREGIDO: Apuntaba a "fecha" pero en el SELECT dice "fechafactura"
                    FechaFactura: reader["fechafactura"] != DBNull.Value ? Convert.ToDateTime(reader["fechafactura"]) : DateTime.Now,
                    Ndet: reader["Ndet"]?.ToString() ?? "0",
                    Status: reader["status"]?.ToString() ?? string.Empty,
                    Moneda: reader["moneda"]?.ToString() ?? string.Empty,
                    Almacen: reader["almacen"]?.ToString() ?? string.Empty
                );
            }
            return null;
        }

        public async Task<string> ObtenerNombreClienteAsync(int idCliente)
        {
            string query = "SELECT nombre FROM dbo.clientes WHERE id_empros = @idCliente";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@idCliente", System.Data.SqlDbType.Int).Value = idCliente;

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString() ?? string.Empty;
        }

        public async Task<IEnumerable<PedidoDetalleDto>> ObtenerDetalleAsync(int idPedido)
        {
            var lista = new List<PedidoDetalleDto>();
            // CORREGIDO: Se cambia dbo.doctoscc por la tabla de detalle dbo.doctoscc_det
            string query = @"
                SELECT id_factura,id_detallefatura as item, idprodserv, cantidad, preciou as precio, (cantidad * preciou) AS importe 
                FROM dbo.doctoscc_det 
                WHERE id_factura = @idPedido";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@idPedido", System.Data.SqlDbType.Int).Value = idPedido;

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new PedidoDetalleDto
                {
                    Id_factura = reader["id_factura"]?.ToString() ?? string.Empty,
                    Item = reader["item"] != DBNull.Value ? Convert.ToInt32(reader["item"]) : 0,
                    idprodserv = reader["idprodserv"]?.ToString() ?? string.Empty,
                    Cantidad = reader["cantidad"] != DBNull.Value ? Convert.ToDecimal(reader["cantidad"]) : 0m,
                    Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0m,
                    Importe = reader["importe"] != DBNull.Value ? Convert.ToDecimal(reader["importe"]) : 0m
                });
            }

            return lista;
        }

        public async Task<bool> InsertarPedidoCabeceraAsync(int idPedido, string cliente, DateTime fecha)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM dbo.doctoscc WHERE id_factura = @id_factura)
                    UPDATE dbo.doctoscc SET cliente = @cliente, fechafactura = @fecha WHERE id_factura = @id_factura;
                ELSE
                    INSERT INTO dbo.doctoscc (id_factura, cliente, fechafactura) VALUES (@id_factura, @cliente, @fecha);";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id_factura", System.Data.SqlDbType.Int).Value = idPedido;
            cmd.Parameters.Add("@cliente", System.Data.SqlDbType.VarChar, 10).Value = cliente;
            cmd.Parameters.Add("@fecha", System.Data.SqlDbType.DateTime).Value = fecha;

            await con.OpenAsync();
            int filas = await cmd.ExecuteNonQueryAsync();
            return filas > 0;
        }

        public async Task<bool> AgregarDetalleAsync(int id_factura, int id_detallefactura, string idprodserv, decimal cantidad, decimal precio)
        {
            string query = @"
                INSERT INTO dbo.doctoscc_det (id_factura, id_detallefactura, idprodserv, cantidad, precio)
                VALUES (@id_factura, @id_detallefactura, @idprodserv, @cantidad, @precio);";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id_factura", System.Data.SqlDbType.Int).Value = id_factura;
            cmd.Parameters.Add("@id_detallefactura", System.Data.SqlDbType.Int).Value = id_detallefactura;
            cmd.Parameters.Add("@idprodserv", System.Data.SqlDbType.VarChar, 50).Value = idprodserv;
            cmd.Parameters.Add("@cantidad", System.Data.SqlDbType.Decimal).Value = cantidad;
            cmd.Parameters.Add("@precio", System.Data.SqlDbType.Decimal).Value = precio;

            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<int> ObtenerUltimoIdPedidoAsync()
        {
            // CORREGIDO: Apuntando a id_factura en lugar de id
            string query = "SELECT ISNULL(MAX(id_factura), 0) FROM dbo.doctoscc";
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);
            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public Task<bool> ActualizarDetalleAsync(int idPedido, int item, string codigoBarra, decimal cantidad, decimal precio) => throw new NotImplementedException();
        public Task<bool> EliminarDetalleAsync(int idPedido, int item) => throw new NotImplementedException();
        //public Task<int> ImportarDesdeTxtAsync(string rutaArchivo) => throw new NotImplementedException();
        public async Task<int> ImportarDesdeTxtAsync(string ruta)
        {
            if (!File.Exists(ruta)) return 0;

            var lineas = await File.ReadAllLinesAsync(ruta);
            int importados = 0;

            using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            // Omitimos la primera línea si es la cabecera del archivo (ej. Ticket|Fecha|...)
            foreach (var linea in lineas.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var partes = linea.Split('|');
                if (partes.Length < 7) continue;

                // Mapeo y limpieza de los campos del archivo
                if (!int.TryParse(partes[0].Trim(), out int ticket)) continue;
                if (!DateTime.TryParse(partes[1].Trim(), out DateTime fecha)) fecha = DateTime.Now;
                string cliente = partes[2].Trim();
                if (!int.TryParse(partes[3].Trim(), out int item)) item = 0;

                if (!decimal.TryParse(partes[4].Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal cantidad)) cantidad = 0;

                string producto = partes[5].Trim();

                if (!decimal.TryParse(partes[6].Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal precio)) precio = 0;

                using var transaction = con.BeginTransaction();
                try
                {
                    // 1. Insertar la cabecera en doctoscc si no existe para este folio
                    string insertCabecera = @"
                IF NOT EXISTS (SELECT 1 FROM dbo.doctoscc WHERE folio = @folio)
                BEGIN
                    INSERT INTO dbo.doctoscc (folio, cliente, fechafactura, tipo, almacen, id_usuario, vendedor, condicionpago)
                    VALUES (@folio, @cliente, @fecha, 0, 11, 116, 116, 6);
                END";

                    using (var cmdCab = new SqlCommand(insertCabecera, con, transaction))
                    {
                        cmdCab.Parameters.AddWithValue("@folio", ticket);
                        cmdCab.Parameters.AddWithValue("@cliente", cliente);
                        cmdCab.Parameters.AddWithValue("@fecha", fecha);
                        await cmdCab.ExecuteNonQueryAsync();
                    }

                    // Obtener el ID interno (id_factura) generado o existente en doctoscc
                    int idFactura = 0;
                    string getMasterId = "SELECT id_factura FROM dbo.doctoscc WHERE folio = @folio";
                    using (var cmdId = new SqlCommand(getMasterId, con, transaction))
                    {
                        cmdId.Parameters.AddWithValue("@folio", ticket);
                        var res = await cmdId.ExecuteScalarAsync();
                        if (res != null) idFactura = Convert.ToInt32(res);
                    }

                    // 2. Insertar el detalle en doctoscc_det si no existe ese item para esta factura
                    if (idFactura > 0)
                    {
                        string insertDetalle = @"
                    IF NOT EXISTS (SELECT 1 FROM dbo.doctoscc_det WHERE id_factura = @id_factura AND id_detallefatura = @item)
                    BEGIN
                        INSERT INTO dbo.doctoscc_det (id_factura, idprodserv, cantidad, preciou)
                        VALUES (@id_factura, @producto, @cantidad, @precio);
                    END";

                        using (var cmdDet = new SqlCommand(insertDetalle, con, transaction))
                        {
                            cmdDet.Parameters.AddWithValue("@id_factura", idFactura);
                            cmdDet.Parameters.AddWithValue("@item", item);
                            cmdDet.Parameters.AddWithValue("@producto", producto);
                            cmdDet.Parameters.AddWithValue("@cantidad", cantidad);
                            cmdDet.Parameters.AddWithValue("@precio", precio);
                            await cmdDet.ExecuteNonQueryAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    importados++;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return importados;
        }
        //public Task<int> ImportarTicketsPendientesAsync() => throw new NotImplementedException();
        public async Task<int> ImportarTicketsPendientesAsync()
        {
            string query = @"
                BEGIN TRANSACTION;
                
                BEGIN TRY
                    -- 1. Insertar las cabeceras que no existan en doctoscc
                    -- (Ajusta los nombres de columnas de imptickets si difieren, ej: id_factura o id)
                    INSERT INTO dbo.doctoscc (folio, cliente, fechafactura,tipo,almacen,id_usuario,vendedor,condicionpago)
                    SELECT DISTINCT i.idticket, i.nrocli, i.fecha, 0 as tipo, 11 as almacen, 
                    116 as id_usuario, 116 as vendedor, 6 as condicionpago
                    FROM dbo.imptickets i
                    WHERE i.status = 0
                      AND NOT EXISTS (SELECT 1 FROM dbo.doctoscc d WHERE d.folio = i.idticket);

                    -- 2. Insertar los detalles en doctoscc_det
                    INSERT INTO dbo.doctoscc_det (id_factura, idprodserv, cantidad, preciou)
                    SELECT d.id_factura,  i.codigobarra, i.cantidad, i.precio
                    FROM dbo.imptickets i left join dbo.doctoscc d on i.idticket = d.folio
                    WHERE i.status = 0
                      AND NOT EXISTS (
                          SELECT 1 FROM dbo.doctoscc_det det 
                          WHERE det.id_factura = d.id_factura
                      );

                    -- 3. Marcar los registros como procesados en imptickets
                    UPDATE dbo.imptickets 
                    SET status = 1 
                    WHERE status = 0;

                    COMMIT TRANSACTION;
                    SELECT 1; -- Éxito
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                    THROW;
                END CATCH";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, con);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? 1 : 0;
        }
    }
}