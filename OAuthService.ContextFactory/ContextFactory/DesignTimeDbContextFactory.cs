using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OAuthService.Core.Helpers;
using System.IO;

namespace OAuthService.ContextFactory.ContextFactory
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public DbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{EnvironmentHelper.Environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DbContext>();

            var connectionString = configuration.GetConnectionString("DesignTimeDbConnectionString");

            builder.UseSqlServer(connectionString);

            return new DbContext(builder.Options);
        }
    }
}
