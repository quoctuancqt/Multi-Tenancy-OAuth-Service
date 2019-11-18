using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuthService.Core.Proxy;
using System;

namespace OAuthService.TenantFactory.Exntensions
{
    public static class ProxyExtension
    {
        public static IServiceCollection AddOAuthProxy(this IServiceCollection services,
           IConfiguration configuration)
        {

            services.AddHttpClient<ProxyBase>("ProxyBase", client =>
            client.BaseAddress = new Uri(configuration.GetSection("JWTSettings").GetValue<string>("Issuer")));

            return services;
        }
    }
}
