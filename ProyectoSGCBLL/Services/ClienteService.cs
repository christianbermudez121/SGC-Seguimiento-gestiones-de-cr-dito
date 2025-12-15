using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;

namespace ProyectoSGCBLL.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        public ClienteService(IClienteRepository repo) => _repo = repo;

        public Task<List<Cliente>> ListarAsync(string? q, bool? activos) =>
            _repo.GetAllAsync(q, activos);

        public Task<Cliente?> ObtenerAsync(int id) =>
            _repo.GetByIdAsync(id);

        public async Task<(bool ok, string? error)> CrearAsync(Cliente cliente)
        {
            // Reglas mínimas
            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
                return (false, "La identificación es requerida.");
            if (string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Apellido1))
                return (false, "Nombre y Apellido1 son requeridos.");

            cliente.Identificacion = cliente.Identificacion.Trim();

            if (await _repo.ExistsByIdentificacionAsync(cliente.Identificacion))
                return (false, "Ya existe un cliente con esa identificación.");

            await _repo.AddAsync(cliente);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> ActualizarAsync(Cliente cliente)
        {
            var actual = await _repo.GetByIdAsync(cliente.Id);
            if (actual == null) return (false, "Cliente no encontrado.");

            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
                return (false, "La identificación es requerida.");

            cliente.Identificacion = cliente.Identificacion.Trim();

            if (await _repo.ExistsByIdentificacionAsync(cliente.Identificacion, cliente.Id))
                return (false, "Ya existe otro cliente con esa identificación.");

            // Mapeo controlado
            actual.Identificacion = cliente.Identificacion;
            actual.Nombre = cliente.Nombre?.Trim() ?? "";
            actual.Apellido1 = cliente.Apellido1?.Trim() ?? "";
            actual.Apellido2 = cliente.Apellido2?.Trim();
            actual.Correo = cliente.Correo?.Trim();
            actual.Telefono = cliente.Telefono?.Trim();
            actual.FechaNacimiento = cliente.FechaNacimiento;
            actual.Activo = cliente.Activo;

            await _repo.UpdateAsync(actual);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> EliminarAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return (false, "Cliente no encontrado.");

            await _repo.DeleteAsync(c);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> CambiarEstadoAsync(int id, bool activo)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return (false, "Cliente no encontrado.");

            c.Activo = activo;
            await _repo.UpdateAsync(c);
            return (true, null);
        }
    }
}
