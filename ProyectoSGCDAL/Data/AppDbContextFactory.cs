using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProyectoSGCDAL.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Usa SQLite porque es tu base
            optionsBuilder.UseSqlite("Data Source=sgc.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

