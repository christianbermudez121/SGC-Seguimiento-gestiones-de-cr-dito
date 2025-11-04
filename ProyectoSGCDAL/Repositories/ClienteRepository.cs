using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Data;
using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext ctx) : base(ctx) { }

        public Task<Cliente?> GetByIdentificacionAsync(string id)
            => _set.FirstOrDefaultAsync(x => x.Identificacion == id);

        public Task<bool> ExisteIdentificacionAsync(string id, int? excluirId = null)
            => _set.AnyAsync(x => x.Identificacion == id && (!excluirId.HasValue || x.Id != excluirId.Value));
    }
}
