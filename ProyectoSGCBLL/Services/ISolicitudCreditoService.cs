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
    }
}
