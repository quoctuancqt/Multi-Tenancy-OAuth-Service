namespace OAuthService.JWT.Domain
{
    public abstract class ClientBase : IClient
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public string ShortcutIconUri { get; set; }
        public string SubDomain { get; set; }
        public bool Enabled { get; set; }
        public string Secret { get; set; } = "8peqmuvCtTQ5KyST7RDjGKtbozGlHLjq";
    }
}
