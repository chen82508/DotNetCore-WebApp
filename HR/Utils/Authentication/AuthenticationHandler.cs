using HR.Utils.Authentication.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HR.Utils.Authentication
{
    public class AuthenticationHandler : IAuthenticationSignInHandler
    {
        public const string LOGIN_SCHEME = "LoginAuth";

        public LoginAuthOptions AuthOptions { get; private set; }

        public AuthenticationHandler(IOptions<LoginAuthOptions> options)
        {
            AuthOptions = options.Value;
        }

        public HttpContext HttpContext { get; private set; }
        public AuthenticationScheme Scheme { get; private set; }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            if (Scheme.Name != LOGIN_SCHEME)
            {
                return Task.FromResult(AuthenticateResult.Fail("無法對應驗證方案"));
            }

            if (!HttpContext.Session.Keys.Contains(AuthOptions.SessionKeyName))
            {
                return Task.FromResult(AuthenticateResult.Fail("Session已過期"));
            }

            string un = HttpContext.Session.GetString(AuthOptions.SessionKeyName) ?? string.Empty;
            ClaimsIdentity id = new(LOGIN_SCHEME);
            id.AddClaim(new(ClaimTypes.Name, un));
            ClaimsPrincipal prcp = new(id);
            AuthenticationTicket ticket = new(prcp, LOGIN_SCHEME);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            HttpContext.Response.Redirect($"{AuthOptions.LoginRoute}?{AuthOptions.ReturnUrlKey}={HttpContext.Request.Path}");
            return Task.CompletedTask;
        }

        public async Task ForbidAsync(AuthenticationProperties? properties)
        {
            await HttpContext.ForbidAsync(Scheme.Name);
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            HttpContext = context;
            Scheme = scheme;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            // 獲取使用者名稱
            string username = user.Identity?.Name ?? string.Empty;
            if (!string.IsNullOrEmpty(username))
            {
                HttpContext.Session.SetString(AuthOptions.SessionKeyName, username);
            }
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            if (HttpContext.Session.Keys.Contains(AuthOptions.SessionKeyName))
            {
                HttpContext.Session.Remove(AuthOptions.SessionKeyName);
            }
            return Task.CompletedTask;
        }
    }
}
