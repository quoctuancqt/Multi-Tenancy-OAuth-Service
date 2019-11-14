using OAuthService.JWT.Domain;
using System.Threading.Tasks;

namespace OAuthService.JWT.Services
{
    public interface IJWTClientService
    {
        Task<ClientBase> FindByClientIdAsync(string clientId);

        Task<string> VerifyClientByClientIdAsync(string secretKey);

        Task<string> GetSecretKeyByClientId(string clientId);
    }
}
