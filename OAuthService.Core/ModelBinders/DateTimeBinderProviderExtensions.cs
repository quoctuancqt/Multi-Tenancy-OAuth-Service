using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace OAuthService.Core.ModelBinders
{
    public static class DateTimeBinderProviderExtensions
    {
        public static void RegisterDateTimeProvider(this MvcOptions option, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var clientCultureInfo = serviceProvider.GetService<ClientCultureInfo>();

            option.ModelBinderProviders.Insert(0, new DateTimeBinderProvider(clientCultureInfo));
        }
    }
}