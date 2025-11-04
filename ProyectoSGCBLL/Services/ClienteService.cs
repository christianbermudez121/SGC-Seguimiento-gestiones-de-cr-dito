using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;

namespace ProyectoSGCBLL.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        public ClienteService(IClienteRepository repo) { _repo = repo; }

        public async Task<List<Cliente>> ListarAsync(string? filtro = null, bool? soloActivos = null)
        {
            var lista = await _repo.GetAllAsync();
            var q = lista.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLower();
                q = q.Where(c => c.Identificacion.ToLower().Contains(f)
                              || c.Nombre.ToLower().Contains(f)
                              || c.Apellido1.ToLower().Contains(f)
                              || (c.Apellido2 ?? "").ToLower().Contains(f)
                              || (c.Correo ?? "").ToLower().Contains(f));
            }
            if (soloActivos.HasValue) q = q.Where(c => c.Activo == soloActivos.Value);
            return q.OrderBy(c => c.Apellido1).ThenBy(c => c.Nombre).ToList();
        }

        public Task<Cliente?> ObtenerAsync(int id) => _repo.GetByIdAsync(id);

        public async Task CrearAsync(Cliente c)
        {
            if (await _repo.ExisteIdentificacionAsync(c.Identificacion))
                throw new InvalidOperationException("La identificación ya existe.");
            await _repo.AddAsync(c);
            await _repo.SaveAsync();
        }

        public async Task ActualizarAsync(Cliente c)
        {
            if (await _repo.ExisteIdentificacionAsync(c.Identificacion, c.Id))
                throw new InvalidOperationException("La identificación ya existe.");
            _repo.Update(c);
            await _repo.SaveAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var cli = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Cliente no encontrado.");
            _repo.Remove(cli);
            await _repo.SaveAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var cli = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Cliente no encontrado.");
            cli.Activo = false; _repo.Update(cli);
            await _repo.SaveAsync();
        }

        public async Task<bool> IdentificacionDisponibleAsync(string identificacion, int? excluirId = null)
            => !(await _repo.ExisteIdentificacionAsync(identificacion, excluirId));
    }
}
