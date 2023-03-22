using HR.DTOs.HR;
using HR.Interface;
using HR.Service;
using HR.Utils.Base;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class ModifyPasswordController : AreaBaseController
    {
        private readonly IEntityService<UserInfo> _UserService;
        public ModifyPasswordController(IEntityService<UserInfo> service)
        {
            _UserService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("~/API/Pwd/Modify")]
        public bool ModifyPassword(IFormCollection form)
        {
            string userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            return (_UserService as UserService).ModifyPassword(userId, form);
        }
    }
}
