namespace OAuthService.JWT.Domain
{
    public interface IClient
    {
        string Id { get; set; }
        string ClientId { get; set; }
        string ClientName { get; set; }
        string ClientUri { get; set; }
        string LogoUri { get; set; }
        bool Enabled { get; set; }
        string Secret { get; set; }
    }
}
