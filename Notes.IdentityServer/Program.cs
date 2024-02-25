using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Notes.IdentityServer.Data;
using Notes.IdentityServer.Models;
using Notes.IdentityServer.Services;
using Notes.IdentityServer.Services.Interfaces;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString,
        sql => sql.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddScoped<DbInitializer>();

builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 5;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireDigit = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireLowercase = false;
        config.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = dbBuilder =>
            dbBuilder.UseNpgsql(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = dbBuilder =>
            dbBuilder.UseNpgsql(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));

        options.EnableTokenCleanup = true;
    })
    .AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "notesApp.Identity.cooke";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
    config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    config.Cookie.HttpOnly = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IEmailSender, MailKitEmailSender>();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "notes.app-IdentityServer");
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "wwwroot"))
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

app.UseRouting();
app.UseIdentityServer();

if (app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.MapDefaultControllerRoute();

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    try
    {
        provider.GetRequiredService<DbInitializer>()
            .Initialize();
    }
    catch (Exception e)
    {
        var logger = provider.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while app initialization");
    }
}

app.Run();