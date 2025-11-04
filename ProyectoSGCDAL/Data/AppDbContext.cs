using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("Clientes");
                e.HasKey(x => x.Id);
                e.Property(x => x.Identificacion).HasMaxLength(30).IsRequired();
                e.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
                e.Property(x => x.Apellido1).HasMaxLength(80).IsRequired();
                e.Property(x => x.Apellido2).HasMaxLength(80);
                e.Property(x => x.Correo).HasMaxLength(120);
                e.Property(x => x.Telefono).HasMaxLength(40);
                e.HasIndex(x => x.Identificacion).IsUnique();
                
            });
        }
    }
}
