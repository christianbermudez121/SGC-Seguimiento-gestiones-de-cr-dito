using Microsoft.AspNetCore.Identity;
using ProyectoSGCDAL.Entities;

namespace ProyectoSGCDAL.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            // ROLES DEL SISTEMA
            string[] roles = { "Administrador", "ServicioCliente", "Analista", "Gestor" };

            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                {
                    var result = await roleMgr.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                        throw new Exception($"Error creando rol {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // USUARIO ADMINISTRADOR
            var adminEmail = "admin@sgc.com";

            var admin = await userMgr.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,  // ← IMPORTANTE
                    NombreCompleto = "Admin del Sistema"
                };

                var result = await userMgr.CreateAsync(newAdmin, "Admin123!");

                if (!result.Succeeded)
                    throw new Exception($"Error creando admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                await userMgr.AddToRoleAsync(newAdmin, "Administrador");
            }
        }
    }
}
