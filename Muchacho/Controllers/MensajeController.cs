using Microsoft.AspNetCore.Mvc;

namespace Muchacho.Controllers
{ 

    public class MensajeController : Controller
    {
        public IActionResult Mensaje()
        {
            return View();
        }

        public IActionResult Enviar(string Nombre, string Correo, string Mensaje)
        {


            ViewBag.Mensaje = "Formulario enviado correctamente";
            return View("Mensaje");
        }
    }
}
