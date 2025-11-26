using ProyectoSGCDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositories
{
    public class SolicitudCreditoRepository : ISolicitudCreditoRepository
    {
        private List<SolicitudCredito> solicitudes = new List<SolicitudCredito>()
            {
            new SolicitudCredito
            {
                IdSolicitud = 1,
                identificacion = "1234567890",
                IdCliente = 1,
                MontoSolicitado = 5000,
                comentarios = "Necesito el crédito para comprar un vehículo.",
                FechaSolicitud = DateTime.Now.AddDays(-10),
                Estado = "Aprobado"
            },
            new SolicitudCredito
            {
                IdSolicitud = 2,
                identificacion = "0987654321",
                IdCliente = 2,
                MontoSolicitado = 15000,
                comentarios = "Crédito para remodelar mi casa.",
                FechaSolicitud = DateTime.Now.AddDays(-5),
                Estado = "Pendiente"
            }
        };




    public async Task<bool> AgregarSolicitudAsync(SolicitudCredito solicitudcredito)
        {
            solicitudcredito.IdSolicitud = solicitudes.Any() ? solicitudes.Max(s => s.IdSolicitud) + 1 : 1;
            return true;
        }

        public async Task<SolicitudCredito> ObtenerPorIdentificacionAsync(string identificacion)
        {
            var solicitud = solicitudes.FirstOrDefault(s => s.identificacion == identificacion);
            return solicitud;
        }
    }
}
