using Microsoft.Extensions.DependencyInjection;
using System;

namespace OAuthService.JWT.Extensions
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddHttpJWT<TClient>(this IServiceCollection services, Uri baseAddress)
          where TClient : class
        {
            services.AddHttpClient<TClient>(typeof(TClient).Name, client => client.BaseAddress = baseAddress);

            return services;
        }
    }
}
