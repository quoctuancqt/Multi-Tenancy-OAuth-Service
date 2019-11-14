using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OAuthService.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseEnvironment(Environment);
                });

        public static string Environment
        {
            get
            {
                var path = $"{AppContext.BaseDirectory}\\Environment.txt";
                if (!File.Exists(path)) return "Development";
                return File.ReadAllText(path);
            }
        }
    }
}
