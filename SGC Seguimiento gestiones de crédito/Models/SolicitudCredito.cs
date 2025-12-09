using System;
using System.ComponentModel.DataAnnotations;

namespace SGC_Seguimiento_gestiones_de_credito.Models
{
    public class SolicitudCredito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [Range(1, 10000000, ErrorMessage = "El monto no puede exceder ₡10,000,000.")]
        public decimal Monto { get; set; }

        public string Estado { get; set; } = "Registrado";

        // Nombre del archivo guardado en wwwroot/Documentos (opcional)
        public string DocumentoNombre { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}