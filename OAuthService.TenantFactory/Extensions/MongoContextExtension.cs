using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OAuthService.Core.Exceptions;
using OAuthService.TenantFactory;

namespace Webcast.Core.Exntensions
{
    public static class MongoContextExtension
    {
        public static IServiceCollection AddMongoContext(this IServiceCollection services)
        {
            services.AddScoped((serviceProvider) =>
            {
                object factory = serviceProvider.GetService<IHttpContextAccessor>();

                HttpContext context = ((HttpContextAccessor)factory).HttpContext;

                string webCastSecurity = context.Request.Headers["WebSecurity"].ToString();

                if (string.IsNullOrEmpty(webCastSecurity))
                    throw new ForbiddenException("You can't access this api please provide license about 'Web Cast Security'  before action ");

                var tenantFactory = serviceProvider.GetService<ITenantFactory>();

                string tenantId = tenantFactory.GetTenantId(webCastSecurity);

                return new MongoFactory(tenantFactory, tenantId);
            });

            return services;
        }
    }
}
