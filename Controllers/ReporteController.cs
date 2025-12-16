using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Services;

namespace SGC_Seguimiento_gestiones_de_credito.Controllers
{
    [Authorize(Roles = "Analista,Gestor,Administrador")]
    public class ReporteController : Controller
    {
        private readonly IHistorialGestionService _historialService;

        public ReporteController(IHistorialGestionService historialService)
        {
            _historialService = historialService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var lista = await _historialService.ObtenerTodosAsync();
            return Json(new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPorSolicitud(int solicitudId)
        {
            var lista = await _historialService.ObtenerPorSolicitudAsync(solicitudId);
            return Json(new { data = lista });
        }
    }
}