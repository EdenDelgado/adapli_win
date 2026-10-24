using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.UseCases.DTOs;

namespace Sisvta.Infrastructure.Repositories
{
    public class LineaArticuloRepository : ILineaArticuloRepository
    {
        private readonly ApplicationDbContext _context;

        public LineaArticuloRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<LineaArticuloDto>> ObtenerTodosAsync()
        {
            var sql = @"SELECT id_lineasarticulos as IdLineasArticulos, 
                               l.nombre as Nombre, 
                               l.grupolinea as GrupoLinea, 
                               l.status as Status, 
                               l.daterecord as DateRecord, 
                               l.utilidad as Utilidad, 
                               l.prcComision as PrcComision, 
                               l.id_empresa as IdEmpresa, g.nombre as NombreGrupo 
                        FROM dbo.lineaarticulo as l left join dbo.grupolinea as g on l.grupolinea = g.id_gruposlineas";

            return await _context.Database
                .SqlQueryRaw<LineaArticuloDto>(sql)
                .ToListAsync();
        }

        public async Task CrearAsync(string nombre, string grupoLinea, decimal comision, string idempresa)
        {
            var sql = @"INSERT INTO dbo.lineaarticulo (nombre, grupolinea, prcComision, daterecord, id_empresa) 
                        VALUES (@nombre, @grupoLinea, @comision, GETDATE(), @idempresa)";

            var pNombre = new SqlParameter("@nombre", nombre);
            var pGrupo = new SqlParameter("@grupoLinea", string.IsNullOrEmpty(grupoLinea) ? (object)DBNull.Value : grupoLinea);
            var pComision = new SqlParameter("@comision", comision);
            var pIdEmpresa = new SqlParameter("@idempresa", string.IsNullOrEmpty(idempresa) ? (object)DBNull.Value : idempresa);

            // ¡Añadido pIdEmpresa a la ejecución de SQL!
            await _context.Database.ExecuteSqlRawAsync(sql, pNombre, pGrupo, pComision, pIdEmpresa);
        }

        public async Task ActualizarAsync(int id, string nombre, string grupoLinea, decimal comision, string idempresa)
        {
            var sql = @"UPDATE dbo.lineaarticulo 
                        SET nombre = @nombre, grupolinea = @grupoLinea, prcComision = @comision, id_empresa = @idempresa 
                        WHERE id_lineasarticulos = @id";

            var pId = new SqlParameter("@id", id);
            var pNombre = new SqlParameter("@nombre", nombre);
            var pGrupo = new SqlParameter("@grupoLinea", string.IsNullOrEmpty(grupoLinea) ? (object)DBNull.Value : grupoLinea);
            var pComision = new SqlParameter("@comision", comision);
            var pIdEmpresa = new SqlParameter("@idempresa", string.IsNullOrEmpty(idempresa) ? (object)DBNull.Value : idempresa);

            // Actualizado para recibir y enviar los 5 parámetros correspondientes
            await _context.Database.ExecuteSqlRawAsync(sql, pId, pNombre, pGrupo, pComision, pIdEmpresa);
        }
    }
}