using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCBLL.Services;
using System.Threading.Tasks;

namespace SGC_Seguimiento_gestiones_de_credito.Controllers
{
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

        // Lista principal (DataTable)
        public async Task<IActionResult> Index()
        {
            var resp = await _solicitudService.ObtenerSolicitudesCredito();
            var model = resp.Data ?? new System.Collections.Generic.List<SolicitudCreditoDto>();
            return View(model);
        }

        // Partial para modal de cambio de estado
        [HttpGet]
        public async Task<IActionResult> ChangeEstadoPartial(int id)
        {
            if (id <= 0) return BadRequest();

            var resp = await _solicitudService.ObtenerSolicitudPorId(id);
            if (resp == null || resp.EsError || resp.Data == null) return NotFound();

            return PartialView("_ChangeEstadoPartial", resp.Data);
        }

        // AJAX: cambiar estado (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEstado(int id, string nuevoEstado, string comentarios)
        {
            if (id <= 0) return BadRequest(new { ok = false, msg = "Id inválido" });

            var obtener = await _solicitudService.ObtenerSolicitudPorId(id);
            if (obtener == null || obtener.EsError || obtener.Data == null)
                return BadRequest(new { ok = false, msg = "Solicitud no encontrada" });

            var dto = obtener.Data;
            // Validar transiciones simples 
            var actual = dto.Estado ?? "";
            if (User.IsInRole("Analista"))
            {
                if (!(actual == "Ingresado" && nuevoEstado == "Enviado aprobación"))
                    return BadRequest(new { ok = false, msg = "Transición no permitida para Analista" });
            }
            if (User.IsInRole("Gestor"))
            {
                if (!(actual == "Enviado aprobación" && (nuevoEstado == "Aprobado" || nuevoEstado == "Devolución")))
                    return BadRequest(new { ok = false, msg = "Transición no permitida para Gestor" });
            }

            dto.Estado = nuevoEstado;
            dto.Comentarios = comentarios ?? string.Empty;
            dto.UsuarioId = _userManager.GetUserId(User) ?? string.Empty;

            var editar = await _solicitudService.EditarSolicitud(dto);
            if (editar == null || editar.EsError)
                return BadRequest(new { ok = false, msg = editar?.Mensaje ?? "Error al actualizar estado" });

            // EditarSolicitud ya registra historial; si necesita otro formato, agregar aquí.
            return Ok(new { ok = true, msg = "Estado actualizado", data = editar.Data });
        }
    }
}