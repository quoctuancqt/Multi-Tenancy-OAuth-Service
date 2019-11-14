using OAuthService.JWT.Models;
using System.Collections.Generic;

namespace OAuthService.JWT.Services
{
    public interface ITokenService
    {
        IDictionary<string, object> GenerateToken(TokenRequest dto);

        IDictionary<string, object> RefreshAccessToken(string token);

        void RevokeRefreshToken(string token);

    }
}
