using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Data;
using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _db;
        public ClienteRepository(AppDbContext db) => _db = db;

        public async Task<List<Cliente>> GetAllAsync(string? q, bool? activos)
        {
            var query = _db.Clientes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(c =>
                    c.Identificacion.Contains(q) ||
                    c.Nombre.Contains(q) ||
                    c.Apellido1.Contains(q) ||
                    (c.Apellido2 != null && c.Apellido2.Contains(q)) ||
                    (c.Correo != null && c.Correo.Contains(q))
                );
            }

            if (activos.HasValue)
                query = query.Where(c => c.Activo == activos.Value);

            return await query
                .OrderBy(c => c.Identificacion)
                .ToListAsync();
        }

        public Task<Cliente?> GetByIdAsync(int id) =>
            _db.Clientes.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null)
        {
            var q = _db.Clientes.AsQueryable().Where(x => x.Identificacion == identificacion);
            if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);
            return q.AnyAsync();
        }

        public async Task AddAsync(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _db.Clientes.Update(cliente);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cliente cliente)
        {
            _db.Clientes.Remove(cliente);
            await _db.SaveChangesAsync();
        }
    }
}
