using Microsoft.AspNetCore.Identity;
using ProyectoSGCDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Task ActualizarUsuario(string userId, string email, string? password, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CrearUsuario(string email, string password, string role)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded && !string.IsNullOrEmpty(role))
                await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        public Task EliminarUsuario(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> ObtenerTodosUsuarios()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> ObtenerUsuarioPorId(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
