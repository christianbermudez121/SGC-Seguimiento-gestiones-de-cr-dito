using ProyectoSGCDAL.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositorios
{
    public interface ISolicitudCreditoRepositorio
    {
        Task<SolicitudCredito> ObtenerPorIdentificacionAsync(string identificacion);
        Task<bool> AgregarSolicitudAsync(SolicitudCredito solicitudcredito);
    }
}
