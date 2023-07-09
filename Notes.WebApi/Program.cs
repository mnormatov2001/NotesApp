using Notes.Application.Common.Mappings;
using Notes.Application.Extensions;
using Notes.Application.Interfaces;
using Notes.Data;
using Notes.Data.Extensions;
using System.Reflection;
using Notes.WebApi.Middleware;

namespace Notes.WebApi
{
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

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) 
                app.UseDeveloperExceptionPage();

            app.UseCustomExceptionHandler();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            
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
                    throw;
                }
            }

            app.Run();
        }
    }
}