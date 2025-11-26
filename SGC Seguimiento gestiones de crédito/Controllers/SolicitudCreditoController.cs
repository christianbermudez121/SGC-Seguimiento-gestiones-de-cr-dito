using Microsoft.AspNetCore.Mvc;

namespace SGC_Seguimiento_gestiones_de_crédito.Controllers
{
    public class SolicitudCreditoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
