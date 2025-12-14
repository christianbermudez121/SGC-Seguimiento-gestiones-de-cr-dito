using ProyectoSGCDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public interface IHistorialGestionService
    {
        Task AgregarAsync(HistorialGestion entidad);
        Task<List<HistorialGestion>> ObtenerPorSolicitudAsync(int idSolicitud);
    }
}