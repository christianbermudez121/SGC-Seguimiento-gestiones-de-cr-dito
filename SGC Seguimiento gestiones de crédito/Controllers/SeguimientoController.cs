using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Entities;

[Authorize(Roles = "Analista,Gestor,Administrador")]
public class SeguimientoController : Controller
{
    private readonly ISolicitudCreditoService _solicitudService;
    private readonly IHistorialGestionService _historialService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SeguimientoController(
        ISolicitudCreditoService solicitudService,
        IHistorialGestionService historialService,
        UserManager<ApplicationUser> userManager)
    {
        _solicitudService = solicitudService;
        _historialService = historialService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _solicitudService.ObtenerSolicitudesCredito();
        return View(response.Data);
    }

    public async Task<IActionResult> ChangeEstadoPartial(int id)
    {
        var response = await _solicitudService.ObtenerSolicitudPorId(id);
        return PartialView("_ChangeEstadoModal", response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEstado(int id, string nuevoEstado, string comentarios)
    {
        var response = await _solicitudService.ObtenerSolicitudPorId(id);
        var solicitud = response.Data;

        var estadoAnterior = solicitud.Estado;

        solicitud.Estado = nuevoEstado;
        await _solicitudService.EditarSolicitud(solicitud);

        var usuario = await _userManager.GetUserAsync(User);

        await _historialService.AgregarAsync(new HistorialGestion
        {
            IdSolicitud = id,
            UsuarioId = usuario.Id,
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = nuevoEstado,
            Comentarios = comentarios
        });

        return Json(new { success = true });
    }
}


