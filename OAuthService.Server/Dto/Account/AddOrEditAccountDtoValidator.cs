using FluentValidation;

namespace OAuthService.Server.Dto
{
    public class AddOrEditAccountDtoValidator : AbstractValidator<AddOrEditAccountDto>
    {
        public AddOrEditAccountDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();

            RuleFor(x => x.ClientId).NotEmpty();
        }
    }
}
