using System;

namespace ProyectoSGCDAL.Entities
{
    public class HistorialGestion
    {
        public int Id { get; set; }

        // FK a SolicitudCredito.IdSolicitud
        public int IdSolicitud { get; set; }

        // Fecha UTC de la acción
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Identity user id que realizó la acción
        public string UsuarioId { get; set; } = string.Empty;

        // Acciones: "Ingresado", "Enviado aprobación", "Aprobado", "Devolución", etc.
        public string Accion { get; set; } = string.Empty;

        // Comentario opcional del usuario
        public string? Comentarios { get; set; }

        // Navegación opcional
        public SolicitudCredito? SolicitudCredito { get; set; }
    }
}