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

        public async Task<bool> AgregarAsync(HistorialGestion entidad)
        {
            return await _repo.AgregarAsync(entidad);
        }

        public async Task<List<HistorialGestion>> ObtenerPorSolicitudAsync(int idSolicitud)
        {
            return await _repo.ObtenerPorSolicitudAsync(idSolicitud);
        }
    }
}