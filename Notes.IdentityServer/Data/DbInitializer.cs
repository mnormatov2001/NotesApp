using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Notes.IdentityServer.Data;

public class DbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly AuthDbContext _authDbContext;
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ConfigurationDbContext _configurationDbContext;

    public DbInitializer(
        IConfiguration configuration,
        AuthDbContext authDbContext,
        PersistedGrantDbContext persistedGrantDbContext,
        ConfigurationDbContext configurationDbContext)
    {
            _configuration = configuration;
            _authDbContext = authDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _configurationDbContext = configurationDbContext;
        }

    public void Initialize()
    {
            _authDbContext.Database.Migrate();
            _persistedGrantDbContext.Database.Migrate();
            _configurationDbContext.Database.Migrate();

            var config = new IdentityServerConfiguration(_configuration);

            if (!_configurationDbContext.Clients.Any())
                foreach (var client in config.Clients)
                    _configurationDbContext.Clients.Add(client.ToEntity());

            if (!_configurationDbContext.IdentityResources.Any())
                foreach (var resource in config.IdentityResources)
                    _configurationDbContext.IdentityResources.Add(resource.ToEntity());

            if (!_configurationDbContext.ApiScopes.Any())
                foreach (var resource in config.ApiScopes)
                    _configurationDbContext.ApiScopes.Add(resource.ToEntity());

            if (!_configurationDbContext.ApiResources.Any())
                foreach (var resource in config.ApiResources)
                    _configurationDbContext.ApiResources.Add(resource.ToEntity());

            _configurationDbContext.SaveChanges();
        }
}