using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Entities;

namespace SGC_Seguimiento_gestiones_de_crédito.Controllers
{
    [Authorize(Roles = "Analista,Gestor,Administrador")]
    public class SeguimientoController : Controller
    {
        private readonly ISolicitudCreditoService _solicitudService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeguimientoController(
            ISolicitudCreditoService solicitudService,
            UserManager<ApplicationUser> userManager)
        {
            _solicitudService = solicitudService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _solicitudService.ObtenerSolicitudesCredito();

          
            if (response == null || response.EsError)
            {
                TempData["err"] = response?.Mensaje ?? "No se pudieron cargar las gestiones.";
                return View(Enumerable.Empty<SolicitudCreditoDto>());
            }

            return View(response.Data ?? Enumerable.Empty<SolicitudCreditoDto>());
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEstadoPartial(int id)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");

            var response = await _solicitudService.ObtenerSolicitudPorId(id);

            if (response == null || response.EsError || response.Data == null)
                return BadRequest(response?.Mensaje ?? "No se pudo cargar la gestión (no existe o hubo un error).");

            return PartialView("_ChangeEstadoModal", response.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEstado(int id, string nuevoEstado, string? comentarios)
        {
            if (id <= 0)
                return BadRequest("Id inválido.");

            if (string.IsNullOrWhiteSpace(nuevoEstado))
                return BadRequest("Debe seleccionar un nuevo estado.");

            
            var getResp = await _solicitudService.ObtenerSolicitudPorId(id);
            if (getResp == null || getResp.EsError || getResp.Data == null)
                return BadRequest(getResp?.Mensaje ?? "No se pudo cargar la gestión.");

            var dto = getResp.Data;

           
            var estadoActual = dto.Estado ?? string.Empty;
            var esAdmin = User.IsInRole("Administrador");
            var esAnalista = User.IsInRole("Analista");
            var esGestor = User.IsInRole("Gestor");

            bool permitido = false;

            if (esAdmin)
            {
                permitido = true; 
            }
            else if (esAnalista && (estadoActual == "Ingresado" || estadoActual == "Registrado"))
            {
                permitido = (nuevoEstado == "Enviado aprobación");
            }
            else if (esGestor && estadoActual == "Enviado aprobación")
            {
                permitido = (nuevoEstado == "Aprobado" || nuevoEstado == "Devolución");
            }

            if (!permitido)
                return BadRequest("No tiene permisos para cambiar a ese estado desde el estado actual.");

      
            var usuario = await _userManager.GetUserAsync(User);
            dto.Estado = nuevoEstado;
            dto.Comentarios = comentarios ?? string.Empty;
            dto.UsuarioId = usuario?.Id ?? string.Empty;

            var updResp = await _solicitudService.EditarSolicitud(dto);
            if (updResp == null || updResp.EsError)
                return BadRequest(updResp?.Mensaje ?? "No se pudo actualizar la gestión.");

            return Json(new { success = true });
        }
    }
}