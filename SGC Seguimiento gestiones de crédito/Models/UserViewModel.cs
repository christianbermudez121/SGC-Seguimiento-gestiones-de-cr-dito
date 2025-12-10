using System.Collections.Generic;

namespace SGC_Seguimiento_gestiones_de_crédito.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
