using Microsoft.EntityFrameworkCore;
using System;

namespace OAuthService.ContextFactory.ContextFactory
{
    public class ContextFactoryBase
    {
        public static TContext GetDbContext<TContext>(string connectionString)
          where TContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TContext>();

            options.UseSqlServer(connectionString);

            return (TContext)Activator.CreateInstance(typeof(TContext), options.Options);
        }
    }
}
