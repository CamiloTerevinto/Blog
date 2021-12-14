using CT.Examples.CustomApiKeys.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CT.Examples.CustomApiKeys.Middleware
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly ICacheService _cacheService;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, ICacheService cacheService) : base(options, logger, encoder, clock)
        {
            _cacheService = cacheService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderName, out var apiKey) || apiKey.Count != 1)
            {
                Logger.LogWarning("An API request was received without the x-api-key header");
                return AuthenticateResult.Fail("Invalid parameters");
            }

            var clientId = await _cacheService.GetClientIdFromApiKey(apiKey);

            if (clientId == null)
            {
                Logger.LogWarning("An API request was received with an invalid API key: {ApiKey}", apiKey);
                return AuthenticateResult.Fail("Invalid parameters");
            }

            Logger.BeginScope("{ClientId}", clientId);
            Logger.LogInformation("Client authenticated");

            var claims = new[] { new Claim(ClaimTypes.Name, clientId.Value.ToString()) };
            var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}
