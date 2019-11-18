using MongoDB.Driver;
using OAuthService.Core.Helpers;
using OAuthService.TenantFactory;

namespace OAuthService.TenantFactory
{
    public class MongoFactory
    {
        private readonly ITenantFactory _tenantFactory;

        public IMongoDatabase Database { get; private set; }

        public MongoFactory(ITenantFactory tenantFactory, string clientId)
        {
            _tenantFactory = tenantFactory;

            var tenantProfile = AsyncHelper.RunSync(() => _tenantFactory.GetTenantByTenantIdAsync(clientId));

            var mongoClient = new MongoContext(tenantProfile.GetMongoDBConnectionString(), tenantProfile.MongoDbDatabase);

            Database = mongoClient.Database;

        }

    }
}
