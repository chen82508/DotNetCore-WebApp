using Microsoft.AspNetCore.Mvc;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class ProbationController : Controller
    {
        public IActionResult CreateForm()
        {
            return View();
        }

        [HttpPost]
        [Route("~/API/Probation/Submit")]
        public JsonResult SubmitFormInfo(IFormCollection form)
        {
            return new JsonResult(new
            {
                success = true,
                message = "",
            });
        }
    }
}
