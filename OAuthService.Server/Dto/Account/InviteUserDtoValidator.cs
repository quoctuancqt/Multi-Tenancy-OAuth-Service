using FluentValidation;

namespace OAuthService.Server.Dto
{
    public class InviteUserDtoValidator : AbstractValidator<InviteUserDto>
    {
        public InviteUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.RedirectUrl).NotEmpty();
        }
    }
}
