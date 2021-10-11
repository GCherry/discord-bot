using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutorial.DataAccess.MSSQL.Context;
using Tutorial.DataAccess.MSSQL.Interfaces;
using Tutorial.DataAccess.MSSQL.Repositories;

namespace Tutorial.DataAccess.MSSQL.Extensions
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TutorialDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IServerRepository, ServerRepository>();

            return services;
        }
    }
}
