namespace OAuthService.Server.Dto
{
    public class EditClientCommonInfoDto
    {
        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public bool IsVerifyUser { get; set; }

        public string LogoUri { get; set; }

        public string ShortcutIconUri { get; set; }
    }
}
