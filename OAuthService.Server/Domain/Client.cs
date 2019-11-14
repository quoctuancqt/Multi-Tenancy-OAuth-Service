using OAuthService.JWT.Domain;
using OAuthService.Server.Enums;

namespace OAuthService.Server.Domain
{
    public class Client : ClientBase, IClient
    {
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public bool IsVerifyUser { get; set; }
        public ClientStatus ClientStatus { get; set; }
        public bool IsSystem { get; set; }
    }
}
