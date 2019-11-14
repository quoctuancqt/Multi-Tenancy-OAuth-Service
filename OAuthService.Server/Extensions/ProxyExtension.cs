using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using OAuthService.Server.Proxy;

namespace OAuthService.Server.Extensions
{
    public static class ProxyExtension
    {
        public static IServiceCollection AddProxy(this IServiceCollection services,
          IConfiguration configuration)
        {

            services.AddHttpClient<ProxyService>("ProxyServiceUri", client =>
            client.BaseAddress = new Uri(configuration.GetValue<string>("ProxyServiceUri")));

            return services;
        }
    }
}
