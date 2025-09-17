using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TP08_Tanel_Dobrovitzky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConfigurarJuego()
        {
            ViewBag.Categorias = BD.ObtenerCategorias();
            ViewBag.Dificultades = BD.ObtenerDificultades();
            return View();
        }

        [HttpPost]
        public IActionResult Comenzar(string username, int dificultad, int categoria)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["Error"] = "Ingresá un nombre de usuario.";
                return RedirectToAction("ConfigurarJuego");
            }
            Juego.CargarPartida(username, dificultad, categoria);
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetInt32("Dificultad", dificultad);
            HttpContext.Session.SetInt32("Categoria", categoria);
            return RedirectToAction("Jugar");
        }

        public IActionResult Jugar()
        {
            var pregunta = Juego.ObtenerProximaPregunta();
            if (pregunta == null)
            {
                return View("Fin");
            }
            ViewBag.Pregunta = pregunta;
            ViewBag.Respuestas = Juego.ObtenerProximasRespuestas(pregunta.IDPregunta);
            return View("Juego");
        }

        [HttpPost]
        public IActionResult VerificarRespuesta(int idRespuesta)
        {
            bool correcta = Juego.VerificarRespuesta(idRespuesta);
            ViewBag.Correcta = correcta;
            return View("Respuesta");
        }
    }
}
