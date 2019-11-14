namespace OAuthService.Server.Dto
{
    public class AddOrEditAccountDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }

        public bool IsSystem { get; set; }
    }
}
