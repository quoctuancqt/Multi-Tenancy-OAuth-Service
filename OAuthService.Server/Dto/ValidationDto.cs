using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace OAuthService.Server.Dto
{
    public class ValidationDto
    {
        public bool IsValid { get; private set; }

        public object Errors { get; private set; }

        public ValidationDto() : this(true)
        {
        }

        public ValidationDto(bool isValid)
        {
            IsValid = isValid;
        }

        public ValidationDto(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                Errors = GetErrors(validationResult.Errors);
            }
        }

        private object GetErrors(IList<ValidationFailure> Errors)
        {
            // This code was ported from:
            // https://dejanstojanovic.net/aspnet/2016/december/dictionary-to-object-in-c/

            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var error in Errors)
            {
                expandoObjCollection.Add(new KeyValuePair<string, object>(FirstCharToLower(error.PropertyName), error.ErrorMessage));
            }

            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }

        private static string FirstCharToLower(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToLower() + input.Substring(1);
            }
        }
    }
}
