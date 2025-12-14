using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCDAL.Entities
{
    public class SolicitudCredito
    {
        public int IdSolicitud { get; set; }
        public string identificacion { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public decimal MontoSolicitado { get; set; }
        public string comentarios { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Registrado";

        public ICollection<HistorialGestion> HistorialGestiones { get; set; }
            = new List<HistorialGestion>();
    }
}
