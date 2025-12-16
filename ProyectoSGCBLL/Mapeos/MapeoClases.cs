using AutoMapper;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Mapeos
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            CreateMap<SolicitudCredito, SolicitudCreditoDto>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.IdSolicitud))
                .ForMember(dest => dest.Identificacion,
                           opt => opt.MapFrom(src => src.identificacion))
                .ForMember(dest => dest.Comentarios,
                           opt => opt.MapFrom(src => src.comentarios))
                .ForMember(dest => dest.Estado,
                           opt => opt.MapFrom(src => src.Estado));

            CreateMap<SolicitudCreditoDto, SolicitudCredito>()
                .ForMember(dest => dest.IdSolicitud,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.identificacion,
                           opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.comentarios,
                           opt => opt.MapFrom(src => src.Comentarios))
                .ForMember(dest => dest.Estado,
                           opt => opt.MapFrom(src => src.Estado));
        }
    }
}