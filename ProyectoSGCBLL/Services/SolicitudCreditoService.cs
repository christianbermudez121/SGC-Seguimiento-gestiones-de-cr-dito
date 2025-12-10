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
            var solicitud = await _solicitudesrepository.ObtenerPorIdentificacionAsync(solicitudCreditoDto.Identificacion);
            var validaciones = await validarAsync(solicitud);
            if (validaciones.EsError)
            {
                return validaciones;
            }

            respuesta.Data = _mapper.Map<SolicitudCreditoDto>(solicitud);
            return respuesta;


        }

        public async Task<CustomResponse<List<SolicitudCreditoDto>>> ObtenerSolicitudesCredito()
        {
            var respuesta = new CustomResponse<List<SolicitudCreditoDto>>();
            var solicitudes = await _solicitudesrepository.ObtenerSolicitudesAsync();
            respuesta.Data = _mapper.Map<List<SolicitudCreditoDto>>(solicitudes);
            return respuesta;
        }

        public async Task<CustomResponse<SolicitudCreditoDto>> ObtenerSolicitudesCreditoPorIdentificacion(string identificacion)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();

            var solicitudes = await _solicitudesrepository.ObtenerPorIdentificacionAsync(identificacion);
            
            respuesta.Data = _mapper.Map<SolicitudCreditoDto>(solicitudes);
            return respuesta;
        }

        private async Task<CustomResponse<SolicitudCreditoDto>> validarAsync(SolicitudCredito solicitud)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();
            if (solicitud == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Solicitud no encontrada";
                return respuesta;
                // puedo agregar N validaciones de negocio
            }
            var existente = await _solicitudesrepository.ObtenerPorIdentificacionAsync(solicitud.identificacion);
            if (existente != null)
            {
                var estado = existente.Estado ?? string.Empty;
                if (string.Equals(estado, "Registrado", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(estado, "Devolución", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(estado, "Devolucion", StringComparison.OrdinalIgnoreCase))
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = $"El usuario con identificación {solicitud.identificacion} ya cuenta con la solicitud de crédito {existente.IdSolicitud}, por favor resolver la gestión antes de ingresar otra nueva";
                    return respuesta;
                }
            }

            return respuesta;
        }
    }
}
