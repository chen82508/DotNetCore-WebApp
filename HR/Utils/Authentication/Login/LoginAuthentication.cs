using HR.Database;
using HR.Interface;
using HR.Models.Entity;
using HR.Utils.Crypto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HR.Utils.Authentication.Login
{
    public class LoginAuthentication : IAuthenticate<IFormCollection>
    {
        private readonly IConfiguration _Configuration;
        private readonly ICrypto _Crypto;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly HRSysContext _HRContext;

        public LoginAuthentication(IConfiguration configuration, ICrypto crypto, IHttpContextAccessor httpContextAccessor, HRSysContext context)
        {
            _Configuration = configuration;
            _Crypto = crypto;
            _HttpContextAccessor = httpContextAccessor;
            _HRContext = context;
        }

        public bool FormDataAuthenticate(IFormCollection form)
        {
            string UserId = form["user-id"];
            string Password = form["password"];

            bool PassAuth = false;

            SY_USER? user = _HRContext.Set<SY_USER>().Where(usr => usr.SY_USR_ID == UserId).FirstOrDefault();
            if (user != null)
            {
                return Password == _Crypto.Decrypt(user.SY_USR_PWD);
            }

            return PassAuth;
        }

        /// <summary>
        /// 註冊登入資訊的session
        /// </summary>
        /// <param name="CNameValue">CName的值</param>
        /// <returns></returns>
        public async Task RegisterLoginSession(string CNameValue)
        {
            SY_USER user = _HRContext.Set<SY_USER>().Where(usr => usr.SY_USR_ID == CNameValue).First();

            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, CNameValue),
                new(ClaimTypes.GivenName, user.SY_USR_NAM),
                new(ClaimTypes.Role, "Admin"),
            };

            int Expire = _Configuration.GetValue<int>("CookieExpire");
            AuthenticationProperties properties = new()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(Expire),
                IsPersistent = false,
            };

            ClaimsIdentity id = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _HttpContextAccessor.HttpContext?.SignInAsync(new ClaimsPrincipal(id), properties);
        }

        /// <summary>
        /// 註銷登入資訊，以登出系統
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await _HttpContextAccessor.HttpContext?.SignOutAsync();
        }
    }
}
