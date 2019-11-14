namespace OAuthService.JWT.Models
{
    using System;

    public class TokenProviderOptions
    {
        public TokenProviderOptions() { }

        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public bool VerifyClient { get; set; } = false;

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(+30);

        public string SecurityKey { get; set; } = "OAuthServices";
    }
}
