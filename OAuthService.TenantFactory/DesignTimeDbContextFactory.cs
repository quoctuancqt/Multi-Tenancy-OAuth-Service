using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OAuthService.Core.Helpers;
using System.IO;

namespace OAuthService.TenantFactory
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContextDefault>
    {
        public DbContextDefault CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{EnvironmentHelper.Environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DbContextDefault>();

            var connectionString = configuration.GetConnectionString("DesignTimeDbConnectionString");

            builder.UseSqlServer(connectionString);

            return new DbContextDefault(builder.Options);
        }
    }
}
