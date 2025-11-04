using ProyectoSGCDAL.Entities;

namespace ProyectoSGCBLL.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> ListarAsync(string? filtro = null, bool? soloActivos = null);
        Task<Cliente?> ObtenerAsync(int id);
        Task CrearAsync(Cliente c);
        Task ActualizarAsync(Cliente c);
        Task EliminarAsync(int id);   // físico
        Task DesactivarAsync(int id); // lógico
        Task<bool> IdentificacionDisponibleAsync(string identificacion, int? excluirId = null);
    }
}
