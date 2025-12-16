using ProyectoSGCDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositories
{
    public interface IHistorialGestionRepository
    {
        Task<bool> AgregarAsync(HistorialGestion entidad);
        Task<List<HistorialGestion>> ObtenerPorSolicitudAsync(int idSolicitud);
        Task<HistorialGestion?> ObtenerPorIdAsync(int id);
        Task<List<HistorialGestion>> ObtenerTodosAsync();
    }
}