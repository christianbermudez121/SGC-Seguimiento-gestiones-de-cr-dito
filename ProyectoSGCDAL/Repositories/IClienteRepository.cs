using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente?> GetByIdentificacionAsync(string identificacion);
        Task<bool> ExisteIdentificacionAsync(string identificacion, int? excluirId = null);
    }
}
