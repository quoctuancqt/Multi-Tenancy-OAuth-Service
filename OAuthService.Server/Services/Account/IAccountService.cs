using System.Threading.Tasks;
using OAuthService.JWT.Services;
using OAuthService.Server.Dto;

namespace OAuthService.Server.Services
{
    public interface IAccountService : IAccountManager
    {
        Task<string> CreateAsync(AddOrEditAccountDto dto);

        Task<string> CreateOrEditAsync(AddOrEditAccountDto dto);

        Task ChangeUserNameAsync(string email);

        Task UpdatePasswordAsync(string userName, string password);

        Task InviteUserAsync(InviteUserDto dto);
    }
}
