using Microsoft.AspNetCore.Identity;

namespace ProyectoSGCDAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}

