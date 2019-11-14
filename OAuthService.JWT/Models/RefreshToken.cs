﻿namespace OAuthService.JWT.Models
{
    public class RefreshToken
    {
        public RefreshToken() { }
        public RefreshToken(string token,
            TokenRequest tokenRequest,
            bool revoked = false)
        {
            TokenRequest = tokenRequest;
            Token = token;
            Revoked = Revoked;
        }

        public TokenRequest TokenRequest { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
    }
}
