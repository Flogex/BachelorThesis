using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flogex.Thesis.NeverNote.Shared.Authentication
{
    // Source: https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly BasicAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasicAuthenticationHandler(
            BasicAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            this.Response.Headers["WWW-Authenticate"] = $"Basic charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var cancellation = _httpContextAccessor.HttpContext.RequestAborted;

            if (!this.Request.Headers.TryGetValue("Authorization", out var headerValues))
                return AuthenticateResult.NoResult();

            var header = headerValues.First();

            if (!header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var authValue = header.Substring("Basic ".Length);
            var headerValueBytes = Convert.FromBase64String(authValue);
            var userAndPassword = Encoding.UTF8.GetString(headerValueBytes);
            var separatorIndex = userAndPassword.IndexOf(':');

            if (separatorIndex <= 0)
                return AuthenticateResult.Fail("Invalid Basic authentication header");

            var userName = userAndPassword.Substring(0, separatorIndex);

            var isValid = await _authenticationService.IsValidUserAsync(userName, cancellation);

            if (!isValid)
                return AuthenticateResult.Fail("Invalid username");

            var claims = new[] { new Claim(ClaimTypes.Name, userName) };
            var identity = new ClaimsIdentity(claims, this.Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
