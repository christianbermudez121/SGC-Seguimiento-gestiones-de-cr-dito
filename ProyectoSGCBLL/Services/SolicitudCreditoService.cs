using AutoMapper;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public class SolicitudCreditoService : ISolicitudCreditoService
    {
        private readonly ISolicitudCreditoRepository _solicitudesrepository;
        private readonly IMapper _mapper;
        private readonly IHistorialGestionService _historialService;

        public SolicitudCreditoService(
            ISolicitudCreditoRepository solicitudesrepository,
            IMapper mapper,
            IHistorialGestionService historialService)
        {
            _solicitudesrepository = solicitudesrepository;
            _mapper = mapper;
            _historialService = historialService;
        }

        public async Task<CustomResponse<SolicitudCreditoDto>> AgregarSolicitudCredito(SolicitudCreditoDto dto)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(dto.Identificacion))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La identificación es requerida.";
                return respuesta;
            }

            if (dto.MontoSolicitado <= 0)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El monto solicitado debe ser mayor a 0.";
                return respuesta;
            }

            if (dto.MontoSolicitado > 10000000)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se puede ingresar una solicitud por un monto mayor a 10.000.000 colones.";
                return respuesta;
            }

            var existente = await _solicitudesrepository.ObtenerActivaPorIdentificacionAsync(dto.Identificacion);

            if (existente != null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje =
                    $"El usuario con identificación {dto.Identificacion} ya cuenta con la solicitud {existente.IdSolicitud}";
                return respuesta;
            }

            var entidad = _mapper.Map<SolicitudCredito>(dto);
            
            // Establecer estado inicial si no viene especificado
            if (string.IsNullOrWhiteSpace(entidad.Estado))
            {
                entidad.Estado = "Ingresado";
            }

            var creado = await _solicitudesrepository.AgregarSolicitudAsync(entidad);
            if (!creado)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo crear la solicitud. Por favor, intente nuevamente.";
                return respuesta;
            }

            await _historialService.AgregarAsync(new HistorialGestion
            {
                IdSolicitud = entidad.IdSolicitud,
                EstadoAnterior = string.Empty,
                EstadoNuevo = "Ingresado",
                Comentarios = "Solicitud creada",
                UsuarioId = dto.UsuarioId ?? string.Empty,
                Fecha = DateTime.UtcNow
            });

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

        public async Task<CustomResponse<SolicitudCreditoDto>> ObtenerSolicitudesCreditoPorIdentificacion(string identificacion)
        {
            var respuesta = new CustomResponse<SolicitudCreditoDto>();
            var solicitud = await _solicitudesrepository.ObtenerPorIdentificacionAsync(identificacion);

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

            var estadoAnterior = existente.Estado;

            var entidad = _mapper.Map<SolicitudCredito>(solicitud);
            entidad.IdSolicitud = solicitud.Id;
            // Preservar el IdCliente existente si no viene en el DTO
            if (entidad.IdCliente == 0)
            {
                entidad.IdCliente = existente.IdCliente;
            }

            var ok = await _solicitudesrepository.ActualizarSolicitudAsync(entidad);
            if (!ok)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar la solicitud";
                return respuesta;
            }

            await _historialService.AgregarAsync(new HistorialGestion
            {
                IdSolicitud = entidad.IdSolicitud,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = entidad.Estado,
                Comentarios = solicitud.Comentarios ?? string.Empty,
                UsuarioId = solicitud.UsuarioId ?? string.Empty,
                Fecha = DateTime.UtcNow
            });

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

            // Agregar historial ANTES de eliminar la solicitud
            await _historialService.AgregarAsync(new HistorialGestion
            {
                IdSolicitud = id,
                EstadoAnterior = existente.Estado,
                EstadoNuevo = "Eliminado",
                Comentarios = "Solicitud eliminada",
                UsuarioId = string.Empty,
                Fecha = DateTime.UtcNow
            });

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


