using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OAuthService.Core.Helpers;
using System.IO;

namespace OAuthService.Server
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<OAuthContext>
    {
        public OAuthContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{EnvironmentHelper.Environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<OAuthContext>();

            var connectionString = configuration.GetConnectionString("DesignTimeDbConnectionString");

            builder.UseSqlServer(connectionString);

            return new OAuthContext(builder.Options);
        }
    }
}
