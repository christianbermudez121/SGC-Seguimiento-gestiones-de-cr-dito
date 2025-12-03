using Microsoft.AspNetCore.Identity;
using ProyectoSGCDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSGCBLL.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> ObtenerTodosUsuarios();
        Task<IdentityResult> CrearUsuario(string email, string password, string role);
        Task<ApplicationUser?> ObtenerUsuarioPorId(string userId);
        Task EliminarUsuario(string userId);
        Task ActualizarUsuario(string userId, string email, string? password, string role);
    }
}
