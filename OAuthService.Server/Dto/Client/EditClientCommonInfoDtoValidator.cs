using FluentValidation;

namespace OAuthService.Server.Dto
{
    public class EditClientCommonInfoDtoValidator : AbstractValidator<EditClientCommonInfoDto>
    {
        public EditClientCommonInfoDtoValidator()
        {
            RuleFor(x => x.ClientName).NotEmpty().NotNull();
            RuleFor(x => x.ClientEmail).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.LogoUri).NotNull().NotEmpty();
            RuleFor(x => x.ShortcutIconUri).NotNull().NotEmpty();
        }
    }
}
