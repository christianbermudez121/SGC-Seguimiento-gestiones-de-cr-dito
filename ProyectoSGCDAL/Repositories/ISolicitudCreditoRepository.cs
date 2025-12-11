using ProyectoSGCDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositories
{
    public interface ISolicitudCreditoRepository
    {
        Task<SolicitudCredito> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> AgregarSolicitudAsync(SolicitudCredito solicitudcredito);
        Task<List<SolicitudCredito>> ObtenerSolicitudesAsync();
        Task<SolicitudCredito?> ObtenerPorIdAsync(int id);
        Task<bool> ActualizarSolicitudAsync(SolicitudCredito solicitudcredito);
        Task<bool> EliminarSolicitudAsync(int id);
        Task<SolicitudCredito?> ObtenerActivaPorIdentificacionAsync(string identificacion);
    }
}
