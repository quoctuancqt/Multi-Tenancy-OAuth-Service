namespace OAuthService.DistributedCache.Models
{
    public class TenantProfileModel
    {

        #region Tenant information

        public string Id { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public string ShortcutIconUri { get; set; }
        public string Secret { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public string ClientStatus { get; set; }
        public string TenantId { get; set; }
        public string SubDomain { get; set; }
        public bool IsVerifyPassword { get; set; } = true;
        public string OwnerId { get; set; }
        public string OwnerEmail { get; set; }
        public bool IsVerifyUser { get; set; }

        #endregion Tenant information

        #region Sql Server

        public string SqlServer { get; set; }
        public string SqlDatabase { get; set; }
        public string SqlUserName { get; set; }
        public string SqlPassword { get; set; }

        public string GetSqlConnectionString(string templete)
        {
            return string.Format(templete, SqlServer.Replace("@", "\\"), SqlDatabase, SqlUserName, SqlPassword);
        }

        #endregion Sql Server

        #region MongoDB

        public string MongoDbServer { get; set; }
        public string MongoDbDatabase { get; set; }
        public string MongoDbUserName { get; set; }
        public string MongoDbPassword { get; set; }

        public string GetMongoDBConnectionString()
        {
            if (string.IsNullOrEmpty(MongoDbUserName))
            {
                return $"mongodb://{MongoDbServer}/{MongoDbDatabase}";
            }

            return $"mongodb://{MongoDbUserName}:{MongoDbPassword}@{MongoDbServer}/{MongoDbDatabase}";
        }
        #endregion
    }
}
