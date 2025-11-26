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
        Task CrearUsuario(string email, string password, string role);
        Task ObtenerUsuarioPorId(string userId);
        Task EliminarUsuario(string userId);
        Task ActualizarUsuario(string userId, string email, string? password, string role);
    }
}
