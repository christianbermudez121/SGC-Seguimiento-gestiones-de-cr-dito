using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Services;

[Authorize]
public class ReporteController : Controller
{
    private readonly IHistorialGestionService _historialService;


    public ReporteController(IHistorialGestionService historialService)
    {
        _historialService = historialService;
    }


    public async Task<IActionResult> Index(int solicitudId)
    {
        var historial = await _historialService.ObtenerPorSolicitudAsync(solicitudId);
        return View(historial);
    }
}
