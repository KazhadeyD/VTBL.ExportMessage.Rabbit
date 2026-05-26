using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VTBL.ExportMessage.Rabbit.Context
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMscrmExtContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MSCRM_EXT")
                ?? throw new InvalidOperationException("Connection string 'MSCRM_EXT' is not configured.");

            services.AddDbContext<MscrmExtDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
