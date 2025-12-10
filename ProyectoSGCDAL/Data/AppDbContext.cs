using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Entities;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<SolicitudCredito> SolicitudesCredito => Set<SolicitudCredito>();

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

        // OPCIONAL: Si quieres configurar la tabla de SolicitudCredito
        modelBuilder.Entity<SolicitudCredito>(e =>
        {
            e.ToTable("SolicitudesCredito");
            e.HasKey(x => x.IdSolicitud);

            e.Property(x => x.identificacion)
                .HasMaxLength(30)
                .IsRequired();

            e.Property(x => x.comentarios)
                .HasMaxLength(300);

            e.Property(x => x.Estado)
                .HasMaxLength(20);
        });
    }
}


