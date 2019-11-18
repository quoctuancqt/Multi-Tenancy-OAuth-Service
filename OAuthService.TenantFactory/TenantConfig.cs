namespace OAuthService.TenantFactory
{
    public class TenantConfig
    {
        public string Domain { get; set; }

        public string TenantStorage { get; set; }

        public string GetTenantIdBySubDomain(string subDomain)
        {
            return string.Format(Domain, subDomain);
        }
    }
}
