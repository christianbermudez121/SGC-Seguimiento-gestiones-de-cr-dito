using ProyectoSGCBLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public interface ISolicitudCreditoService
    {
        Task<CustomResponse<SolicitudCreditoDto>> AgregarSolicitudCredito(SolicitudCreditoDto solicitudCreditoDto);

        Task<CustomResponse<SolicitudCreditoDto>> ObtenerSolicitudesCreditoPorIdentificacion(string identificacion);

        Task<CustomResponse<List<SolicitudCreditoDto>>> ObtenerSolicitudesCredito();

        Task<CustomResponse<SolicitudCreditoDto>> ObtenerSolicitudPorId(int id);

        Task<CustomResponse<SolicitudCreditoDto>> EditarSolicitud(SolicitudCreditoDto solicitud);

        Task<CustomResponse<bool>> EliminarSolicitud(int id);
    }
}
