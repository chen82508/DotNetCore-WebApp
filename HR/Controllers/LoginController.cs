using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Authentication.Login;
using HR.Utils.Mail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace HR.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly IAuthenticate<IFormCollection> _Authenticate;
        private readonly CookieAuthenticationOptions _Options;
        private readonly IEntityService<UserInfo> _UsrService;

        public LoginController(
            IConfiguration configuration,
            IAuthenticate<IFormCollection> authenticate,
            IOptions<CookieAuthenticationOptions> options,
            IEntityService<UserInfo> service)
        {
            _Configuration = configuration;
            _Authenticate = authenticate;
            _Options = options.Value;
            _UsrService = service;
        }

        [Route("~/")]
        [Route("~/Login")]
        [Route("~/Login/Index")]
        public IActionResult Index()
        {
            if (!HttpContext.Request.Query.TryGetValue(_Options.ReturnUrlParameter, out StringValues url))
            {
                url = string.Empty;
            }

            return View((object)url.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection form)
        {
            bool PassAuth = _Authenticate.FormDataAuthenticate(form);
            string ReturnUrl =$"{TempData["_returnUrl"]}";

            if (PassAuth)
            {
                await (_Authenticate as LoginAuthentication).RegisterLoginSession(form["user-id"]);

                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    ReturnUrl = _Configuration.GetValue<string>("DefaultPageRoute");
                }
                return Redirect(ReturnUrl);
            }
            else
            {
                return View("Index", ReturnUrl);
            }
        }

        [HttpGet]
        [Route("~/Login/Logout")]
        public async Task<IActionResult> Logout()
        {
            await (_Authenticate as LoginAuthentication).Logout();

            return Redirect("~/");
        }

        [HttpPost]
        [Route("~/User/Create")]
        public UserInfo CreateUser(UserInfo form)
        {
            return _UsrService.AddData(form);
        }
    }
}
