using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGC_Seguimiento_gestiones_de_credito.Models;
using SGC_Seguimiento_gestiones_de_credito.Data;
using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SGC_Seguimiento_gestiones_de_credito.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Solicitudes
        public IActionResult Index()
        {
            var lista = _context.Solicitudes.ToList();
            return View(lista);
        }

        // GET: /Solicitudes/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Solicitudes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(SolicitudCredito solicitud, IFormFile archivo)
        {
            // Validaciones
            if (solicitud.Monto > 10000000)
                ModelState.AddModelError("Monto", "El monto no puede exceder ₡10,000,000.");

            bool existeActiva = _context.Solicitudes.Any(s =>
                s.ClienteId == solicitud.ClienteId &&
                (s.Estado == "Registrado" || s.Estado == "Devolución"));

            if (existeActiva)
                ModelState.AddModelError("", "El cliente ya tiene una solicitud activa.");

            if (!ModelState.IsValid)
                return View(solicitud);

            // Guardar archivo opcional
            if (archivo != null && archivo.Length > 0)
            {
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documentos");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string ruta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                archivo.CopyTo(stream);

                solicitud.DocumentoNombre = nombreArchivo;
            }

            // Datos iniciales
            solicitud.Estado = "Registrado";
            solicitud.FechaRegistro = DateTime.Now;

            // Guardar en la BD
            _context.Solicitudes.Add(solicitud);
            _context.SaveChanges();

            // Guardar historial
            _context.Historial.Add(new HistorialSolicitud
            {
                SolicitudCreditoId = solicitud.Id,
                Accion = "Creación de solicitud",
                Usuario = "UsuarioActual",
                Fecha = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Mensaje"] = "Solicitud registrada correctamente.";
            return RedirectToAction("Index");
        }

        // GET: /Solicitudes/Editar/{id}
        public IActionResult Editar(int id)
        {
            var solicitud = _context.Solicitudes.FirstOrDefault(x => x.Id == id);
            if (solicitud == null) return NotFound();

            return View(solicitud);
        }

        // POST: /Solicitudes/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(SolicitudCredito solicitud, IFormFile archivo)
        {
            var s = _context.Solicitudes.FirstOrDefault(x => x.Id == solicitud.Id);
            if (s == null) return NotFound();

            // Validaciones
            if (solicitud.Monto > 10000000)
                ModelState.AddModelError("Monto", "El monto no puede exceder ₡10,000,000.");

            bool existeActiva = _context.Solicitudes.Any(x =>
                x.ClienteId == solicitud.ClienteId &&
                x.Id != solicitud.Id &&
                (x.Estado == "Registrado" || x.Estado == "Devolución"));

            if (existeActiva)
                ModelState.AddModelError("", "El cliente ya tiene una solicitud activa.");

            if (!ModelState.IsValid)
                return View(solicitud);

            // Guardar nuevo archivo
            if (archivo != null && archivo.Length > 0)
            {
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documentos");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string ruta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                archivo.CopyTo(stream);

                s.DocumentoNombre = nombreArchivo;
            }

            // Registrar cambio de estado
            if (s.Estado != solicitud.Estado)
            {
                _context.Historial.Add(new HistorialSolicitud
                {
                    SolicitudCreditoId = s.Id,
                    Accion = $"Cambio de estado: {s.Estado} → {solicitud.Estado}",
                    Usuario = "UsuarioActual",
                    Fecha = DateTime.Now
                });
            }

            // Actualizar valores
            s.Monto = solicitud.Monto;
            s.Estado = solicitud.Estado;

            // Registrar edición
            _context.Historial.Add(new HistorialSolicitud
            {
                SolicitudCreditoId = s.Id,
                Accion = "Edición de solicitud",
                Usuario = "UsuarioActual",
                Fecha = DateTime.Now
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Solicitudes/Eliminar/{id}
        public IActionResult Eliminar(int id)
        {
            var solicitud = _context.Solicitudes.FirstOrDefault(x => x.Id == id);

            if (solicitud != null)
            {
                _context.Solicitudes.Remove(solicitud);

                _context.Historial.Add(new HistorialSolicitud
                {
                    SolicitudCreditoId = id,
                    Accion = "Eliminación de solicitud",
                    Usuario = "UsuarioActual",
                    Fecha = DateTime.Now
                });

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: /Solicitudes/Historial/{id}
        public IActionResult Historial(int id)
        {
            var h = _context.Historial
                .Where(x => x.SolicitudCreditoId == id)
                .ToList();

            return View(h);
        }
    }
}
