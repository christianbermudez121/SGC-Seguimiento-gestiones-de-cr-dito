using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Entities;

namespace SGC_Seguimiento_gestiones_de_credito.Controllers
{
    [Authorize(Roles = "Analista,Gestor")]
    public class SeguimientoController : Controller
    {
        private readonly ISolicitudCreditoService _solicitudService;
        private readonly IHistorialGestionService _historialService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeguimientoController(ISolicitudCreditoService solicitudService,
                                     IHistorialGestionService historialService,
                                     UserManager<ApplicationUser> userManager)
        {
            _solicitudService = solicitudService;
            _historialService = historialService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var resp = await _solicitudService.ObtenerSolicitudesCredito();
            if (resp == null || resp.EsError) return View(new List<SolicitudCreditoDto>());
            return View(resp.Data);
        }

        [HttpGet]
        public async Task<IActionResult> HistorialPartial(int solicitudId)
        {
            var lista = await _historialService.ObtenerPorSolicitudAsync(solicitudId);
            return PartialView("_HistorialPartial", lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoAjax(int solicitudId, string nuevoEstado, string comentario)
        {
            var sResp = await _solicitudService.ObtenerSolicitudPorId(solicitudId);
            if (sResp == null || sResp.EsError || sResp.Data == null)
                return BadRequest(new { message = "Solicitud no encontrada" });

            var dto = sResp.Data;

            if (User.IsInRole("Analista"))
            {
                if (nuevoEstado != "Enviado aprobación") return Forbid();
            }
            else if (User.IsInRole("Gestor"))
            {
                if (nuevoEstado != "Aprobado" && nuevoEstado != "Devolución") return Forbid();
            }
            else
            {
                return Forbid();
            }

            dto.Estado = nuevoEstado;
            dto.UsuarioId = _userManager.GetUserId(User);

            var editResp = await _solicitudService.EditarSolicitud(dto);
            if (editResp == null || editResp.EsError) return BadRequest(new { message = editResp?.Mensaje ?? "No se pudo actualizar" });

            var h = new HistorialGestion
            {
                IdSolicitud = solicitudId,
                Accion = nuevoEstado,
                Comentarios = comentario ?? string.Empty,
                UsuarioId = dto.UsuarioId ?? string.Empty,
                Fecha = DateTime.UtcNow
            };
            await _historialService.AgregarAsync(h);

            return Ok(new { message = "Estado actualizado" });
        }
    }
}