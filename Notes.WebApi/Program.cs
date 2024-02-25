using Notes.Application.Common.Mappings;
using Notes.Application.Extensions;
using Notes.Application.Interfaces;
using Notes.Data;
using Notes.Data.Extensions;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Notes.WebApi.Middleware;

namespace Notes.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
            });

        builder.Services.AddApplication();
        builder.Services.AddDataBase(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddCors(options => 
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader(); 
                policy.AllowAnyMethod(); 
                policy.AllowAnyOrigin();
            }));

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = builder.Configuration["Authentication:Authority"];
                options.Audience = builder.Configuration["Authentication:Audience"];
                options.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
                options.RequireHttpsMetadata = builder.Configuration.GetSection("Authentication")
                    .GetValue<bool>("RequireHttpsMetadata");
                options.TokenValidationParameters.ValidIssuers = builder.Configuration
                    .GetSection("Authentication:TokenValidationParameters:ValidIssuers")
                    .Get<string[]>();
            });

        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.AddSecurityDefinition("AuthToken",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Description = "Authorization token"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "AuthToken"
                        }
                    },
                    new string[] {}
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) 
            app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint("swagger/v1/swagger.json", "notes.app-WebAPI");
        });

        app.UseCustomExceptionHandler();
            
        if (app.Environment.IsProduction()) 
            app.UseHttpsRedirection();
            
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            try
            {
                var dbContext = provider.GetRequiredService<NotesDbContext>();
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
