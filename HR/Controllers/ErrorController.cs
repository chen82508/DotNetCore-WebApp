using Microsoft.AspNetCore.Mvc;

namespace HR.Controllers
{
    public class ErrorController : Controller
    {
        [Route("~/Error/500")]
        public IActionResult Error500()
        {
            return View();
        }
    }
}
