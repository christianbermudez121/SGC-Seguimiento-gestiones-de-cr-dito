using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Repositories
{
    public class SolicitudCreditoRepository : ISolicitudCreditoRepository
    {
        private readonly AppDbContext _context;

        public SolicitudCreditoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AgregarSolicitudAsync(SolicitudCredito solicitudcredito)
        {
            if (solicitudcredito == null)
                return false;

            // Buscar y asignar IdCliente por la identificación proporcionada
            if (string.IsNullOrWhiteSpace(solicitudcredito.identificacion))
            {
                // No hay identificación: no se puede asociar cliente -> fallo controlado
                return false;
            }

            var identificacionTrim = solicitudcredito.identificacion.Trim();
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Identificacion == identificacionTrim);

            if (cliente == null)
            {
                // Cliente no existe: crear uno nuevo automáticamente
                cliente = new Cliente
                {
                    Identificacion = identificacionTrim,
                    Nombre = "Cliente", // Nombre por defecto, se puede actualizar después
                    Apellido1 = identificacionTrim, // Usar identificación como apellido temporal
                    Activo = true,
                    FechaRegistro = DateTime.UtcNow
                };

                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync(); // Guardar para obtener el Id
            }

            solicitudcredito.IdCliente = cliente.Id;

            // Establecer fecha si no viene
            if (solicitudcredito.FechaSolicitud == default)
                solicitudcredito.FechaSolicitud = DateTime.UtcNow;

            // Establecer estado por defecto si no viene
            if (string.IsNullOrWhiteSpace(solicitudcredito.Estado))
                solicitudcredito.Estado = "Ingresado";

            await _context.SolicitudesCredito.AddAsync(solicitudcredito);
            var cambios = await _context.SaveChangesAsync();

            // Después de SaveChangesAsync, el IdSolicitud debe estar poblado con el valor generado
            return cambios > 0;
        }

        public async Task<SolicitudCredito> ObtenerPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion)) return null;
            return await _context.SolicitudesCredito.FirstOrDefaultAsync(s => s.identificacion == identificacion);
        }

        public async Task<SolicitudCredito?> ObtenerActivaPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion)) return null;

            return await _context.SolicitudesCredito
                .FirstOrDefaultAsync(s =>
                    s.identificacion == identificacion &&
                    (s.Estado == "Registrado" || s.Estado == "Devolución"));
        }


        public async Task<List<SolicitudCredito>> ObtenerSolicitudesAsync()
        {
            return await _context.SolicitudesCredito.AsNoTracking().ToListAsync();
        }

        public async Task<SolicitudCredito?> ObtenerPorIdAsync(int id)
        {
            return await _context.SolicitudesCredito.FirstOrDefaultAsync(s => s.IdSolicitud == id);
        }

        public async Task<bool> ActualizarSolicitudAsync(SolicitudCredito solicitudcredito)
        {
            var existente = await _context.SolicitudesCredito.FirstOrDefaultAsync(s => s.IdSolicitud == solicitudcredito.IdSolicitud);
            if (existente == null) return false;

            // No actualizar identificación ni IdCliente ya que el cliente ya está asociado
            // existente.identificacion = solicitudcredito.identificacion;
            // existente.IdCliente = solicitudcredito.IdCliente;
            
            existente.MontoSolicitado = solicitudcredito.MontoSolicitado;
            existente.comentarios = solicitudcredito.comentarios;
            existente.Documento = solicitudcredito.Documento;
            existente.Estado = solicitudcredito.Estado;

            _context.SolicitudesCredito.Update(existente);
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0;
        }

        public async Task<bool> EliminarSolicitudAsync(int id)
        {
            var existente = await _context.SolicitudesCredito.FirstOrDefaultAsync(s => s.IdSolicitud == id);
            if (existente == null) return false;

            _context.SolicitudesCredito.Remove(existente);
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0;
        }
    }
}
