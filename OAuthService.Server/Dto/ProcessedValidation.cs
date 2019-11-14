using FluentValidation;
using System;

namespace OAuthService.Server.Dto
{
    public static class ProcessedValidation
    {
        public static ValidationDto CheckValidation<TDto>(TDto model)
        {
            if (model == null)
            {
                return new ValidationDto(false);

            }

            Type type = Type.GetType($"{typeof(TDto)}Validator");

            IValidator validator = (IValidator)Activator.CreateInstance(type);

            return new ValidationDto(validator.Validate(model));
        }

        public static ValidationDto Validate<TDto>(this TDto model)
        {
            return CheckValidation(model);
        }
    }
}
