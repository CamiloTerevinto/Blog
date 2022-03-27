using Duende.IdentityServer.Models;

namespace CT.Examples.OAuthAuthorizationCodeFlow.IdServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "api-swagger",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:44301/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins = { "https://localhost:44301" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api" },
                },
                new Client
                {
                    ClientId = "python-nb",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:8888" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api" },
                }
            };
    }
}