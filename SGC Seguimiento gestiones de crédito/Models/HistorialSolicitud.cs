using System;

namespace SGC_Seguimiento_gestiones_de_credito.Models
{
    public class HistorialSolicitud
    {
        public int Id { get; set; }

        // FK hacia SolicitudCredito
        public int SolicitudCreditoId { get; set; }

        // Descripción de la acción: "Creación", "Edición", "Cambio de estado ...", "Eliminación"
        public string Accion { get; set; }

        public string Usuario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}