using System;

namespace ProyectoSGCDAL.Entities
{
    public class HistorialGestion
    {
        public int Id { get; set; }

        public int IdSolicitud { get; set; }
        public SolicitudCredito SolicitudCredito { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;

        public string? Comentarios { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
