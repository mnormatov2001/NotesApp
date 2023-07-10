using IdentityModel;
using IdentityServer4.Models;

namespace Notes.IdentityServer
{
    public class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes { get; } =
            new List<ApiScope>
            {
                new ApiScope("notes.app.webApi", "NotesApp-WebAPI")
            };

        public static IEnumerable<ApiResource> ApiResources { get; } =
            new List<ApiResource>
            {
                new ApiResource("notes.app.webApi", "NotesApp-WebAPI",
                    new[] { JwtClaimTypes.Name, JwtClaimTypes.Email })
                {
                    Scopes = new[] { "notes.app.webApi" }
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources { get; }
            = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<Client> Clients { get; } =
            new List<Client>
            {
                new Client
                {
                    ClientId = "notes.app.client",
                    ClientName = "NotesApp-Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "http://.../signin-oidc"
                    },
                    AllowedCorsOrigins =
                    {
                        "http//..."
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://.../singout-oidc"
                    },
                    AllowedScopes =
                    {
                        "notes.app.webApi",
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
