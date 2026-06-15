using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace VTBL.ExportMessage.Rabbit.UI
{
    /// <summary>
    /// Точка входа веб-приложения.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Запускает веб-приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Создаёт и настраивает generic host приложения.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <returns>Построитель хоста.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
