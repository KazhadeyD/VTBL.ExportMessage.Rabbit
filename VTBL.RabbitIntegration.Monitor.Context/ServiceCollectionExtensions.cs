using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VTBL.RabbitIntegration.Monitor.Context
{
    /// <summary>
    /// Расширения DI-контейнера для регистрации инфраструктуры контекста <c>MSCRM_EXT</c>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует <see cref="MscrmExtDbContext"/> и настраивает SQL Server из connection string <c>MSCRM_EXT</c>.
        /// </summary>
        /// <param name="services">Коллекция сервисов DI.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Та же коллекция сервисов для цепочки вызовов.</returns>
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
