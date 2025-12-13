using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Data;
using ProyectoSGCDAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositories
{
    public class HistorialGestionRepository : IHistorialGestionRepository
    {
        private readonly AppDbContext _context;

        public HistorialGestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AgregarAsync(HistorialGestion entidad)
        {
            if (entidad == null) return false;
            if (entidad.Fecha == default) entidad.Fecha = System.DateTime.UtcNow;

            await _context.HistorialGestiones.AddAsync(entidad);
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0;
        }

        public async Task<List<HistorialGestion>> ObtenerPorSolicitudAsync(int idSolicitud)
        {
            return await _context.HistorialGestiones
                                 .AsNoTracking()
                                 .Where(h => h.IdSolicitud == idSolicitud)
                                 .OrderByDescending(h => h.Fecha)
                                 .ToListAsync();
        }

        public async Task<HistorialGestion?> ObtenerPorIdAsync(int id)
        {
            return await _context.HistorialGestiones.FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}