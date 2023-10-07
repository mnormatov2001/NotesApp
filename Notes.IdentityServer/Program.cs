using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Notes.IdentityServer.Data;
using Notes.IdentityServer.Models;
using Notes.IdentityServer.Services;
using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DbConnection");
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 5;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
                config.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            var cfg = new IdentityServerConfiguration(builder.Configuration);
            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(cfg.ApiResources)
                .AddInMemoryIdentityResources(cfg.IdentityResources)
                .AddInMemoryApiScopes(cfg.ApiScopes)
                .AddInMemoryClients(cfg.Clients)
                .AddDeveloperSigningCredential();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "notesApp.Identity.cooke";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IEmailSender, MailKitEmailSender>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "wwwroot"))
            });

            app.UseRouting();
            app.UseIdentityServer();
            // app.UseHttpsRedirection();

            app.MapDefaultControllerRoute();

            using (var scope = app.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                try
                {
                    var dbContext = provider.GetRequiredService<AuthDbContext>();
                    DbInitializer.Initialize(dbContext);
                }
                catch (Exception e)
                {
                    var logger = provider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occurred while app initialization");
                }
            }

            app.Run();
        }
    }
}