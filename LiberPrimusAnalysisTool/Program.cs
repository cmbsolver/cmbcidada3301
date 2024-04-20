using ImageMagick;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Utility.Character;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace LiberPrimusAnalysisTool
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            if (!System.IO.Directory.Exists("output"))
            {
                System.IO.Directory.CreateDirectory("output");
            }

            //ResourceLimits.LimitMemory(new Percentage(10));
            MagickNET.SetTempDirectory("./output");

            // create hosting object and DI layer
            using IHost host = CreateHostBuilder(args).Build();

            // create a service scope
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                services.GetRequiredService<App>().Run(args);
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            }
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] strings)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((_, services) =>
                {
                    // Mediatr
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPageData).Assembly));

                    // Quick and dirty character
                    services.AddSingleton<ICharacterRepo, CharacterRepo>();

                    // The application singleton
                    services.AddSingleton<App>();
                });
        }
    }
}