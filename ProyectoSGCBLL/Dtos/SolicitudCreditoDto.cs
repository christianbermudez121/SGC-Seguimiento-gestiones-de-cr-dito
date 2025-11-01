using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Dtos
{
    public class SolicitudCreditoDto
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public decimal MontoSolicitado { get; set; }
        public string Comentarios { get; set; } = string.Empty;
        public string Documento { get; set; }
    }
}
