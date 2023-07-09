using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;

namespace Notes.Data.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataBase(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddScoped<INotesDbContext>(provider =>
                provider.GetRequiredService<NotesDbContext>());
            return services;
        }
    }
}
