using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Entities;

namespace SGC_Seguimiento_gestiones_de_credito.Controllers
{
    [Authorize] // todos los roles autenticados pueden
    public class ClientesController : Controller
    {
        private readonly IClienteService _svc;
        public ClientesController(IClienteService svc) => _svc = svc;

        public async Task<IActionResult> Index(string? q, bool? activos)
        {
            var lista = await _svc.ListarAsync(q, activos);
            ViewBag.Q = q;
            ViewBag.Activos = activos;
            return View(lista);
        }

        public async Task<IActionResult> Details(int id)
        {
            var c = await _svc.ObtenerAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        public IActionResult Create() => View(new Cliente { Activo = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente model)
        {
            if (!ModelState.IsValid) return View(model);

            var r = await _svc.CrearAsync(model);
            if (!r.ok)
            {
                ModelState.AddModelError("", r.error!);
                return View(model);
            }

            TempData["ok"] = "Cliente creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _svc.ObtenerAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var r = await _svc.ActualizarAsync(model);
            if (!r.ok)
            {
                ModelState.AddModelError("", r.error!);
                return View(model);
            }

            TempData["ok"] = "Cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _svc.ObtenerAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var r = await _svc.EliminarAsync(id);
            if (!r.ok)
            {
                TempData["err"] = r.error;
                return RedirectToAction(nameof(Index));
            }

            TempData["ok"] = "Cliente eliminado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id, bool activo)
        {
            var r = await _svc.CambiarEstadoAsync(id, activo);
            if (!r.ok) TempData["err"] = r.error;
            else TempData["ok"] = "Estado actualizado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
