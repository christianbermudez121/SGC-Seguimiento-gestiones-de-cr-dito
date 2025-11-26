using AutoMapper;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public class SolicitudCreditoService : ISolicitudCreditoService
    {
        private readonly ISolicitudCreditoRepository _solicitudesrepository;
        private readonly IMapper _mapper;
        public SolicitudCreditoService(ISolicitudCreditoRepository solicitudesrepository, IMapper mapper)
        {
            _solicitudesrepository = solicitudesrepository;
            _mapper = mapper;
        }
        public async Task<CustomResponse<SolicitudCreditoDto>> AgregarSolicitudCredito(SolicitudCreditoDto solicitudCreditoDto)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();

            if(!await _solicitudesrepository.AgregarSolicitudAsync(_mapper.Map<SolicitudCredito>(solicitudCreditoDto)))
                {
                respuesta.EsError = true;
                respuesta.Mensaje = "Error al agregar la solicitud de crédito.";
                return respuesta;
            }
            return respuesta;
        }
    }
}
