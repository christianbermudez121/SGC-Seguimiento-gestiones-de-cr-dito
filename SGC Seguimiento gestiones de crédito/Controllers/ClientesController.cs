using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Data;
using ProyectoSGCDAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SGC_Seguimiento_gestiones_de_crédito.Controllers
{
    
    public class ClientesController : Controller
    {
        private readonly AppDbContext _db;

        public ClientesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Clientes
        public async Task<IActionResult> Index(string q, bool? activos)
        {
            var query = _db.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.Identificacion.Contains(q) ||
                    c.Nombre.Contains(q) ||
                    (c.Correo ?? string.Empty).Contains(q));
            }

            if (activos.HasValue)
                query = query.Where(c => c.Activo == activos.Value);

            var lista = await query.OrderByDescending(c => c.FechaRegistro).ToListAsync();
            ViewBag.Filtro = q;
            ViewBag.Activos = activos;
            return View(lista);
        }

        // GET partial: /Clientes/CreateModal
        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("Create", new Cliente());
        }

        // POST AJAX: /Clientes/CreateAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromForm] Cliente model)
        {
            if (!ModelState.IsValid)
                return PartialView("Create", model);

            // validar existencia por identificación
            var exists = await _db.Clientes.AnyAsync(c => c.Identificacion == model.Identificacion);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Identificacion), "Ya existe un cliente con esa identificación.");
                return PartialView("Create", model);
            }

            model.FechaRegistro = System.DateTime.UtcNow;
            _db.Clientes.Add(model);
            await _db.SaveChangesAsync();

            return Ok(new { ok = true, msg = "Cliente creado", data = model });
        }

        // GET partial: /Clientes/DetailsModal/5
        [HttpGet]
        public async Task<IActionResult> DetailsModal(int id)
        {
            var cliente = await _db.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return PartialView("_Details", cliente);
        }

        // GET partial: /Clientes/EditModal/5
        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var cliente = await _db.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return PartialView("Edit", cliente);
        }

        // POST AJAX: /Clientes/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax([FromForm] Cliente model)
        {
            if (!ModelState.IsValid)
                return PartialView("Edit", model);

            var existente = await _db.Clientes.FindAsync(model.Id);
            if (existente == null) return NotFound(new { message = "Cliente no encontrado" });

            existente.Nombre = model.Nombre;
            existente.Apellido1 = model.Apellido1;
            existente.Apellido2 = model.Apellido2;
            existente.Correo = model.Correo;
            existente.Telefono = model.Telefono;
            existente.FechaNacimiento = model.FechaNacimiento;
            existente.Activo = model.Activo;

            _db.Clientes.Update(existente);
            await _db.SaveChangesAsync();

            return Ok(new { ok = true, msg = "Cliente actualizado", data = existente });
        }

        // GET partial: /Clientes/DeleteModal/5
        [HttpGet]
        public async Task<IActionResult> DeleteModal(int id)
        {
            var cliente = await _db.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return PartialView("_Delete", cliente);
        }

        // POST AJAX: /Clientes/DeleteAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var cliente = await _db.Clientes.FindAsync(id);
            if (cliente == null) return NotFound(new { message = "Cliente no encontrado" });

            _db.Clientes.Remove(cliente);
            await _db.SaveChangesAsync();
            return Ok(new { ok = true, msg = "Cliente eliminado" });
        }
    }
}