namespace OAuthService.Server.Dto
{
    public class AddOrEditMongoContextDto
    {
        public string MongoDbServer { get; set; }
        public string MongoDbDatabase { get; set; }
        public string MongoDbUserName { get; set; }
        public string MongoDbPassword { get; set; }
    }
}
