namespace OAuthService.Server.Dto
{
    public class ClientDto
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public string SubDomain { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public bool Enabled { get; set; }
        public string Secret { get; set; }
        public bool IsSystem { get; set; }
    }
}
