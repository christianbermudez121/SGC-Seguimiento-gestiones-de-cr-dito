using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Repositories
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllAsync(string? q, bool? activos);
        Task<Cliente?> GetByIdAsync(int id);
        Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null);

        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(Cliente cliente);
    }
}
