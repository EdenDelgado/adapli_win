using Microsoft.EntityFrameworkCore;
using Sisvta.Domain.Interfaces;
using Sisvta.Infrastructure.Persistence.Contexts;
using Sisvta.UseCases.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sisvta.Infrastructure.Repositories
{
    public class GrupoLineaRepository : IGrupoLineaRepository
    {
        private readonly ApplicationDbContext _context;

        public GrupoLineaRepository(ApplicationDbContext context) => _context = context;



        public async Task<IEnumerable<GrupoLineaDto>> ObtenerTodosAsync()
        {
            var sql = @"SELECT 
                    id_gruposlineas as Grupo, 
                    ISNULL(nombre, '') as Nombre, 
                    ISNULL(status, 0) as Status, 
                    ISNULL(daterecord, GETDATE()) as DateRecord, 
                    ISNULL(id_empresa, 0) as IdEmpresa 
                FROM dbo.grupolinea";

            return await _context.Database
                .SqlQueryRaw<GrupoLineaDto>(sql)
                .ToListAsync();
        }

        // Agregamos la implementación requerida por la interfaz para guardar
        public async Task GuardarAsync(GrupoLineaDto grupoDto)
        {
            // Ajusta los nombres de las columnas (nombre, status, id_empresa, etc.) 
            // según la estructura real de tu tabla dbo.grupolinea en la base de datos.
            var sql = @"INSERT INTO dbo.grupolinea (nombre) VALUES ({0})";

            await _context.Database.ExecuteSqlRawAsync(sql, grupoDto.Nombre);
        }
    }
}