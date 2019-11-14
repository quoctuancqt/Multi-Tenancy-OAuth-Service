using OAuthService.JWT.Models;
using System.Threading.Tasks;

namespace OAuthService.JWT.Services
{
    public interface IAccountManager
    {
        Task<AccountResult> VerifyAccountAsync(string username,
            string password,
            TokenRequest tokenRequest);

        Task<AccountResult> VerifyAccountAsync(string clientId,
            string username,
            string password,
            TokenRequest tokenRequest);
    }
}
