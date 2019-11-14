namespace OAuthService.Server.Domain
{
    public class ClientConfiguration
    {
        public ClientConfiguration() { }

        public ClientConfiguration(string clientId,
            string key,
            string value)
        {
            ClientId = clientId;
            Key = key;
            Value = value;
        }

        public string Id { get; set; }
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
