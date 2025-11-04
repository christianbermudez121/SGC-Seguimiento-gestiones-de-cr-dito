namespace ProyectoSGCDAL.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = string.Empty; // cédula
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string? Apellido2 { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
       }
}
