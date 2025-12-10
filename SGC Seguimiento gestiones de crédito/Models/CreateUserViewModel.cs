using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGC_Seguimiento_gestiones_de_crédito.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? SelectedRole { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
