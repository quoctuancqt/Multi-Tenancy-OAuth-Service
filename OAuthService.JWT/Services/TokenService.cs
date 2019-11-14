using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using OAuthService.JWT.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace OAuthService.JWT.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();

        private readonly IDistributedCache _cache;

        public TokenService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IDictionary<string, object> GenerateToken(TokenRequest dto)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dto.SecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();

            foreach (var claim in dto.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value.ToString()));
            }

            string refreshToken = Guid.NewGuid().ToString("n");

            _refreshTokens.Add(new RefreshToken(refreshToken, dto));

            var token = new JwtSecurityToken(
                issuer: dto.Issuer,
                audience: dto.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(dto.Expiration),
                signingCredentials: creds);

            if (dto.Responses.ContainsKey("access_token")) dto.Responses.Remove("access_token");

            if (dto.Responses.ContainsKey("expires")) dto.Responses.Remove("expires");

            if (dto.Responses.ContainsKey("refresh_token")) dto.Responses.Remove("refresh_token");

            dto.Responses.Add("access_token", new JwtSecurityTokenHandler().WriteToken(token));

            dto.Responses.Add("expires", dto.Expiration.TotalSeconds);

            dto.Responses.Add("refresh_token", refreshToken);

            var keys = dto.Responses.Keys.OrderBy(x => x);

            var dic = new Dictionary<string, object>();

            foreach (string key1 in keys)
            {
                dic.Add(key1, dto.Responses[key1]);
            }

            dic.Add("tokenType", "Bearer");

            return dic;

        }

        public IDictionary<string, object> RefreshAccessToken(string token)
        {
            var refreshToken = GetRefreshToken(token);

            if (refreshToken == null)
            {
                throw new Exception("invalid refresh_token");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was revoked");
            }

            var tokenRequest = refreshToken.TokenRequest;

            _refreshTokens.Remove(refreshToken);

            tokenRequest.Expiration = TimeSpan.FromSeconds(+tokenRequest.TotalSeconds);

            return GenerateToken(refreshToken.TokenRequest);
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = GetRefreshToken(token);

            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was already revoked.");
            }

            refreshToken.Revoked = true;
        }

        private RefreshToken GetRefreshToken(string token)
        {
            var refreshToken = _refreshTokens.SingleOrDefault(x => x.Token == token);

            return refreshToken;
        }

    }
}
