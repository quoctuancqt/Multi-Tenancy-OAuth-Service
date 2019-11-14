namespace OAuthService.Server.Dto
{
    public class AddOrEditSqlContextDto
    {
        public string SqlServer { get; set; }
        public string SqlDatabase { get; set; }
        public string SqlUserName { get; set; }
        public string SqlPassword { get; set; }
    }
}
