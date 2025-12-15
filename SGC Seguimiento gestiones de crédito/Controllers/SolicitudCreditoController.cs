using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCBLL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoSGCDAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SGC_Seguimiento_gestiones_de_crédito.Controllers
{
    [Authorize(Roles = "ServicioCliente,Administrador")]
    public class SolicitudCreditoController : Controller
    {
        private readonly ISolicitudCreditoService _solicitudService;
        private readonly ILogger<SolicitudCreditoController> _logger;

        public SolicitudCreditoController(ISolicitudCreditoService solicitudService, ILogger<SolicitudCreditoController> logger)
        {
            _solicitudService = solicitudService;
            _logger = logger;
        }

        // GET: /SolicitudCredito
        public async Task<IActionResult> Index()
        {
            var resp = await _solicitudService.ObtenerSolicitudesCredito();
            if (resp == null || resp.EsError)
            {
                TempData["Error"] = resp?.Mensaje ?? "Error al recuperar solicitudes.";
                return View(new List<SolicitudCreditoDto>());
            }

            return View(resp.Data);
        }

        // Devuelve la vista parcial con el formulario
        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreateSolicitudPartial", new SolicitudCreditoDto());
        }

        // Recibe la petición AJAX y devuelve JSON (no redirige)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([FromForm] SolicitudCreditoDto solicitud)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    _logger.LogWarning("CreateAjax: ModelState inválido: {Errors}", errors);
                    return BadRequest(new { message = "Datos inválidos", errors });
                }

                var resp = await _solicitudService.AgregarSolicitudCredito(solicitud);

                if (resp == null)
                {
                    _logger.LogError("CreateAjax: respuesta nula del servicio al crear solicitud. DTO: {@Solicitud}", solicitud);
                    return StatusCode(500, new { message = "Respuesta nula del servicio." });
                }

                if (resp.EsError)
                {
                    _logger.LogWarning("CreateAjax: servicio devolvió error: {Mensaje}", resp.Mensaje);
                    return BadRequest(new { message = resp.Mensaje });
                }

                _logger.LogInformation("CreateAjax: solicitud creada id={Id}", resp.Data?.Id);
                return Ok(new { message = "Solicitud creada", data = resp.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAjax: excepción al crear solicitud. DTO: {@Solicitud}", solicitud);
#if DEBUG
                // En desarrollo se puede devolver el detalle para facilitar debugging
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
#else
                return StatusCode(500, new { message = "Error interno del servidor" });
#endif
            }
        }

        // Mantener Create tradicional por compatibilidad (opcional)
        // GET: /SolicitudCredito/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST normal (si se usa sin AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudCreditoDto solicitud)
        {
            if (!ModelState.IsValid)
                return View(solicitud);

            var resp = await _solicitudService.AgregarSolicitudCredito(solicitud);
            if (resp == null || resp.EsError)
            {
                ModelState.AddModelError(string.Empty, resp?.Mensaje ?? "Error al crear");
                return View(solicitud);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET partial: /SolicitudCredito/EditPartial/{id}
        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            if (id <= 0)
                return BadRequest();

            var resp = await _solicitudService.ObtenerSolicitudPorId(id);
            if (resp == null || resp.EsError || resp.Data == null)
                return NotFound();

            return PartialView("_EditSolicitudPartial", resp.Data);
        }

        // POST AJAX: /SolicitudCredito/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAjax([FromForm] SolicitudCreditoDto solicitud)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var resp = await _solicitudService.EditarSolicitud(solicitud);
            if (resp == null)
                return BadRequest(new { message = "Respuesta nula del servicio." });

            if (resp.EsError)
                return BadRequest(new { message = resp.Mensaje });

            return Ok(new { message = "Solicitud actualizada", data = resp.Data });
        }

        // GET partial: /SolicitudCredito/DeletePartial/{id}
        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            if (id <= 0)
                return BadRequest();

            var resp = await _solicitudService.ObtenerSolicitudPorId(id);
            if (resp == null || resp.EsError || resp.Data == null)
                return NotFound();

            return PartialView("_DeleteSolicitudPartial", resp.Data);
        }

        // POST AJAX: /SolicitudCredito/DeleteAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var resp = await _solicitudService.EliminarSolicitud(id);
            if (resp == null)
                return BadRequest(new { message = "Respuesta nula del servicio." });

            if (resp.EsError)
                return BadRequest(new { message = resp.Mensaje });

            return Ok(new { message = "Solicitud eliminada", id });

        }
    }
}
