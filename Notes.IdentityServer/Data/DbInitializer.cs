using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Notes.IdentityServer.Data
{
    public class DbInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.GetRequiredService<AuthDbContext>().Database.Migrate();
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var config = new IdentityServerConfiguration(configuration);

            if (!context.Clients.Any())
                foreach (var client in config.Clients)
                    context.Clients.Add(client.ToEntity());

            if (!context.IdentityResources.Any())
                foreach (var resource in config.IdentityResources)
                    context.IdentityResources.Add(resource.ToEntity());

            if (!context.ApiScopes.Any())
                foreach (var resource in config.ApiScopes)
                    context.ApiScopes.Add(resource.ToEntity());

            if (!context.ApiResources.Any())
                foreach (var resource in config.ApiResources)
                    context.ApiResources.Add(resource.ToEntity());

            context.SaveChanges();
        }
    }
}
