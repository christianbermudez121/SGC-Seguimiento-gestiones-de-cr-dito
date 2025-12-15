using ProyectoSGCDAL.Entities;

namespace ProyectoSGCBLL.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> ListarAsync(string? q, bool? activos);
        Task<Cliente?> ObtenerAsync(int id);
        Task<(bool ok, string? error)> CrearAsync(Cliente cliente);
        Task<(bool ok, string? error)> ActualizarAsync(Cliente cliente);
        Task<(bool ok, string? error)> EliminarAsync(int id);
        Task<(bool ok, string? error)> CambiarEstadoAsync(int id, bool activo);
    }
}
