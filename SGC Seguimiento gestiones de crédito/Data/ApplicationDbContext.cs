using Microsoft.EntityFrameworkCore;
using SGC_Seguimiento_gestiones_de_credito.Models;

namespace SGC_Seguimiento_gestiones_de_credito.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SolicitudCredito> Solicitudes { get; set; }
        public DbSet<HistorialSolicitud> Historial { get; set; }
    }
}
