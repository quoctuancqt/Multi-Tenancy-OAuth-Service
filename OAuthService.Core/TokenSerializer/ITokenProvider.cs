namespace OAuthService.Core.TokenSerializer
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITokenProvider
    {
        string GenerateToken<T>(string reason, T model)
            where T : class, ITokenSecurity;

        TokenValidation ValidateToken<T>(string reason, T user, string token)
            where T : class, ITokenSecurity;
    }
}
