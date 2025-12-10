using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSGCBLL.Dtos;
using ProyectoSGCBLL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoSGCDAL.Data;
using Microsoft.AspNetCore.Identity;

namespace SGC_Seguimiento_gestiones_de_crédito.Controllers
{
    [Authorize(Roles = "ServicioCliente")]
    public class SolicitudCreditoController : Controller
    {
        private readonly ISolicitudCreditoService _solicitudService;

        public SolicitudCreditoController(ISolicitudCreditoService solicitudService)
        {
            _solicitudService = solicitudService;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });
            }

            var resp = await _solicitudService.AgregarSolicitudCredito(solicitud);
            if (resp == null)
                return BadRequest(new { message = "Respuesta nula del servicio." });

            if (resp.EsError)
                return BadRequest(new { message = resp.Mensaje });

            // Devuelve el DTO creado para actualizar la UI en cliente
            return Ok(new { message = "Solicitud creada", data = resp.Data });
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
    }
}
