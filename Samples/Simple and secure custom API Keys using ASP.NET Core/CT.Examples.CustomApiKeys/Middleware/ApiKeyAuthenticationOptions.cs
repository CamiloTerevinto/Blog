using Microsoft.AspNetCore.Authentication;

namespace CT.Examples.CustomApiKeys.Middleware
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string HeaderName = "x-api-key";
        public const string DefaultScheme = "ApiKey";
    }
}