using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OAuthService.Core.ModelBinders
{
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        private readonly ClientCultureInfo _clientCultureInfo;

        public DateTimeBinderProvider(ClientCultureInfo clientCultureInfo)
        {
            _clientCultureInfo = clientCultureInfo;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime))
            {
                return new DateTimeBinder(_clientCultureInfo);
            }

            return null;
        }
    }
}