using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VTBL.RabbitIntegration.Monitor.Context;
using VTBL.RabbitIntegration.Monitor.UI.Services;

namespace VTBL.RabbitIntegration.Monitor.UI
{
    /// <summary>
    /// Конфигурация DI и HTTP-конвейера Razor Pages приложения.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Инициализирует конфигурацию приложения.
        /// </summary>
        /// <param name="configuration">Конфигурация окружения.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Регистрирует сервисы приложения в DI-контейнере.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddMscrmExtContext(Configuration);
            services.AddScoped<IExportMessageRabbitStatusNameService, ExportMessageRabbitStatusNameService>();
            services.AddScoped<IKkaMessageService, KkaMessageService>();
            services.AddScoped<INovaMessageService, NovaMessageService>();
            services.AddScoped<IRemarketingMessageService, RemarketingMessageService>();
        }

        /// <summary>
        /// Настраивает HTTP-конвейер обработки запросов.
        /// </summary>
        /// <param name="app">Построитель конвейера.</param>
        /// <param name="env">Окружение хостинга.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
