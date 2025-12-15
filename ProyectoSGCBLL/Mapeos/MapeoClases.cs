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
            CreateMap<SolicitudCredito, SolicitudCreditoDto>();

            CreateMap<SolicitudCreditoDto, SolicitudCredito>();
        }

    }
}
