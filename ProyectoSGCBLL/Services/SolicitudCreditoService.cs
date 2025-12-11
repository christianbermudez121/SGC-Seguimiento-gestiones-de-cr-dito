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
        public async Task<CustomResponse<SolicitudCreditoDto>> AgregarSolicitudCredito(SolicitudCreditoDto dto)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();

         
            var existente = await _solicitudesrepository.ObtenerActivaPorIdentificacionAsync(dto.Identificacion);

            //Validacion de monto maximo
            if (dto.MontoSolicitado > 10000000)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se puede ingresar una solicitud por un monto mayor a 10.000.000 colones.";
                return respuesta;
            }

            //Validacion de solicitud ya existente para la misma identificacion
            if (existente != null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje =
                    $"El usuario con identificación {dto.Identificacion} ya cuenta con la solicitud de crédito {existente.IdSolicitud}, por favor resolver la gestión antes de ingresar otra nueva";

                return respuesta; // Detener proceso
            }

            // SI NO EXISTE, crear
            var entidad = _mapper.Map<SolicitudCredito>(dto);

            var creado = await _solicitudesrepository.AgregarSolicitudAsync(entidad);
            if (!creado)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo crear la solicitud.";
                return respuesta;
            }

            // Retornar DTO creado
            dto.Id = entidad.IdSolicitud;
            respuesta.Data = dto;
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


        // nuevos métodos
        public async Task<CustomResponse<SolicitudCreditoDto>> ObtenerSolicitudPorId(int id)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();
            var solicitud = await _solicitudesrepository.ObtenerPorIdAsync(id);
            if (solicitud == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Solicitud no encontrada";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<SolicitudCreditoDto>(solicitud);
            return respuesta;
        }

        public async Task<CustomResponse<SolicitudCreditoDto>> EditarSolicitud(SolicitudCreditoDto solicitud)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();
            var existente = await _solicitudesrepository.ObtenerPorIdAsync(solicitud.Id);
            if (existente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Solicitud no encontrada";
                return respuesta;
            }

            // mapear dto a entidad
            var entidad = _mapper.Map<SolicitudCredito>(solicitud);
            entidad.IdSolicitud = solicitud.Id;

            var ok = await _solicitudesrepository.ActualizarSolicitudAsync(entidad);
            if (!ok)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar la solicitud";
                return respuesta;
            }

            respuesta.Data = _mapper.Map<SolicitudCreditoDto>(entidad);
            return respuesta;
        }

        public async Task<CustomResponse<bool>> EliminarSolicitud(int id)
        {
            var respuesta = new CustomResponse<bool>();
            var existente = await _solicitudesrepository.ObtenerPorIdAsync(id);
            if (existente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Solicitud no encontrada";
                return respuesta;
            }

            var ok = await _solicitudesrepository.EliminarSolicitudAsync(id);
            if (!ok)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar la solicitud";
                return respuesta;
            }

            respuesta.Data = true;
            return respuesta;
        }
    }
}
