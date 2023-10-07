using IdentityServer4.Models;

namespace Notes.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public IdentityServerConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("IdentityServerConfiguration");
            ApiScopes = section.GetSection("ApiScopes").Get<ApiScope[]>();
            ApiResources = section.GetSection("ApiResources").Get<ApiResource[]>();
            Clients = section.GetSection("Clients").Get<Client[]>();
        }
        public IEnumerable<ApiScope> ApiScopes { get; }
        public IEnumerable<ApiResource> ApiResources { get; }
        public IEnumerable<IdentityResource> IdentityResources { get; }
            = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public IEnumerable<Client> Clients { get; }
    }
}
