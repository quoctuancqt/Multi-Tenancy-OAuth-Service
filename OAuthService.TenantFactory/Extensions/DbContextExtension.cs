using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OAuthService.Core.Exceptions;

namespace OAuthService.TenantFactory.Exntensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddSingleton<ITenantFactory, TenantFactory>();

            services.AddScoped<DbContextDefault>((serviceProvider) =>
            {
                object factory = serviceProvider.GetService<IHttpContextAccessor>();

                HttpContext context = ((HttpContextAccessor)factory).HttpContext;

                string webSecurity = context.Request.Headers["WebSecurity"].ToString();

                if (string.IsNullOrEmpty(webSecurity))
                    throw new ForbiddenException("You can't access this api please provide license about 'Web Cast Security'  before action ");

                var tenantFactory = serviceProvider.GetService<ITenantFactory>();

                string tenantId = tenantFactory.GetTenantId(webSecurity);

                return tenantFactory.GetTenantContext<DbContextDefault>(tenantId);
            });

            return services;
        }
    }
}
