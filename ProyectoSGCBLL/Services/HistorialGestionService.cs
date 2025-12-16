using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public class HistorialGestionService : IHistorialGestionService
    {
        private readonly IHistorialGestionRepository _repo;

        public HistorialGestionService(IHistorialGestionRepository repo)
        {
            _repo = repo;
        }

        public async Task AgregarAsync(HistorialGestion entidad)
        {
            await _repo.AgregarAsync(entidad);
        }

        public async Task<List<HistorialGestion>> ObtenerPorSolicitudAsync(int idSolicitud)
        {
            return await _repo.ObtenerPorSolicitudAsync(idSolicitud);
        }

        public async Task<List<HistorialGestion>> ObtenerTodosAsync()
        {
            return await _repo.ObtenerTodosAsync();
        }
    }
}
