namespace OAuthService.Server.Domain
{
    public class UserClient
    {
        public UserClient() { }

        public UserClient(string userID, string clientId)
        {
            UserId = userID;
            ClientId = clientId;
        }

        public string Id { get; set; }
        
        public string UserId { get; set; }
        
        public virtual User User { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
