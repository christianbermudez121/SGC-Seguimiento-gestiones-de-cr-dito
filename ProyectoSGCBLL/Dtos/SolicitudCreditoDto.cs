using System;

namespace ProyectoSGCBLL.Dtos
{
    public class SolicitudCreditoDto
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public decimal MontoSolicitado { get; set; }
        public string Comentarios { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        // opcional: id del usuario que realiza la acción (para tracking)
        public string? UsuarioId { get; set; }
    }
}
