using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace OAuthService.Core.ModelBinders
{
    public class DateTimeBinder : IModelBinder
    {
        private readonly ClientCultureInfo _clientCultureInfo;

        public DateTimeBinder(ClientCultureInfo clientCultureInfo)
        {
            _clientCultureInfo = clientCultureInfo;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
            {
                bindingContext.Result =
                    ModelBindingResult.Success(null);

                return Task.CompletedTask;
            }

            if (DateTime.TryParse(valueProviderResult.FirstValue, null, DateTimeStyles.AdjustToUniversal, out var datetime))
            {
                bindingContext.Result =
                    ModelBindingResult.Success(_clientCultureInfo.GetUtcTime(datetime));
            }

            return Task.CompletedTask;
        }
    }
}