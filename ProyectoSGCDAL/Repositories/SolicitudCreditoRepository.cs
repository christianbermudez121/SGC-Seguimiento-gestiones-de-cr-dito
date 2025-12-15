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

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Identificacion == solicitudcredito.identificacion.Trim());

            if (cliente == null)
            {
                // Cliente no encontrado: opción por defecto es fallar y permitir que la capa superior decida.
                // Alternativa: crear automáticamente el cliente aquí si el flujo lo requiere.
                return false;
            }

            solicitudcredito.IdCliente = cliente.Id;

            // Establecer fecha si no viene
            if (solicitudcredito.FechaSolicitud == default)
                solicitudcredito.FechaSolicitud = DateTime.UtcNow;

            await _context.SolicitudesCredito.AddAsync(solicitudcredito);
            var cambios = await _context.SaveChangesAsync();

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

            existente.identificacion = solicitudcredito.identificacion;
            existente.IdCliente = solicitudcredito.IdCliente;
            existente.MontoSolicitado = solicitudcredito.MontoSolicitado;
            existente.comentarios = solicitudcredito.comentarios;
            existente.Documento = solicitudcredito.Documento;
            existente.FechaSolicitud = solicitudcredito.FechaSolicitud;
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
