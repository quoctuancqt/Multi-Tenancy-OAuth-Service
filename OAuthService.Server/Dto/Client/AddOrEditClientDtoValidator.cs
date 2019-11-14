using FluentValidation;

namespace OAuthService.Server.Dto
{
    public class AddOrEditClientDtoValidator: AbstractValidator<AddOrEditClientDto>
    {
        public AddOrEditClientDtoValidator()
        {
            RuleFor(x => x.SubDomain).NotEmpty();
            RuleFor(x => x.ClientName).NotEmpty();
        }
    }
}
