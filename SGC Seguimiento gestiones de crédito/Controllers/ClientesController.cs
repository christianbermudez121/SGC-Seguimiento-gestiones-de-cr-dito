using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Entities;

namespace SGC.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _svc;

        public ClientesController(IClienteService svc)
        {
            _svc = svc;
        }

        // Página principal con filtros y primer render del grid
        public async Task<IActionResult> Index(string? q, bool? activos)
        {
            ViewBag.Filtro = q;
            ViewBag.Activos = activos;
            var lista = await _svc.ListarAsync(q, activos);
            return View(lista);
        }

        // Partial para refrescar el grid (llamado por AJAX desde Index)
        public async Task<PartialViewResult> ListPartial(string? q, bool? activos)
        {
            var lista = await _svc.ListarAsync(q, activos);
            return PartialView("_Grid", lista);
        }

        // ==================== MODALES (GET) ====================

        // Modal: Crear
        [HttpGet]
        public IActionResult CreateModal()
        {
            return PartialView("_Form", new Cliente());
        }

        // Modal: Editar
        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var cli = await _svc.ObtenerAsync(id);
            if (cli == null) return NotFound();
            return PartialView("_Form", cli);
        }

        // Modal: Detalles
        [HttpGet]
        public async Task<IActionResult> DetailsModal(int id)
        {
            var cli = await _svc.ObtenerAsync(id);
            if (cli == null) return NotFound();
            return PartialView("_Details", cli);
        }

        // Modal: Eliminar (confirmación)
        [HttpGet]
        public async Task<IActionResult> DeleteModal(int id)
        {
            var cli = await _svc.ObtenerAsync(id);
            if (cli == null) return NotFound();
            return PartialView("_Delete", cli);
        }


        // Crear (AJAX): devuelve JSON cuando OK, o el formulario con validaciones cuando hay error
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax(Cliente model)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", model);

            try
            {
                await _svc.CrearAsync(model);
                return Json(new { ok = true, msg = "Cliente creado correctamente." });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return PartialView("_Form", model);
            }
        }

        // Editar 
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax(Cliente model)
        {
            if (!ModelState.IsValid)
                return PartialView("_Form", model);

            try
            {
                await _svc.ActualizarAsync(model);
                return Json(new { ok = true, msg = "Cliente actualizado correctamente." });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return PartialView("_Form", model);
            }
        }

     
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                await _svc.EliminarAsync(id);
                return Json(new { ok = true, msg = "Cliente eliminado." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarAjax(int id)
        {
            try
            {
                await _svc.DesactivarAsync(id);
                return Json(new { ok = true, msg = "Cliente desactivado." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

      
        [HttpPost]
        public async Task<IActionResult> ValidarIdentificacion(string identificacion, int? id)
        {
            var disponible = await _svc.IdentificacionDisponibleAsync(identificacion, id);
            return Json(disponible); // true = válido; false = muestra mensaje de error remoto
        }
    }
}
