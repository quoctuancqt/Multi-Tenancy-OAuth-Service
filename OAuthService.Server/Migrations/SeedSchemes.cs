using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthService.Server.Domain;
using System.Linq;

namespace OAuthService.Server
{
    public class SeedSchemes
    {
        private readonly OAuthContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        public SeedSchemes(OAuthContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void MigrateSchemes()
        {
            _context.Database.Migrate();
        }

        public void InitData()
        {
            string clientId = "adminClient";

            var client = new Client
            {
                ClientId = clientId,
                ClientName = "Administrator",
                ClientUri = "http://localhost:9000",
                SubDomain = "admin",
                Enabled = true,
                ClientStatus = Enums.ClientStatus.Active,
                Secret = "aZ8Beo8QNtN89jPtkuJJokzT",
                IsSystem = true,
            };

            if (!_context.Clients.Any())
            {
                _context.Clients.Add(client);

                var user = new User
                {
                    UserName = "admin@yopmail.com",
                    LockoutEnabled = false,
                    IsSystem = true,
                };

                if (!_context.Users.Any())
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@1234");

                    _context.Users.Add(user);

                    _context.UserClients.Add(new UserClient
                    {
                        UserId = user.Id,
                        ClientId = client.Id,
                    });

                    _context.SaveChanges();
                }
            }
        }
    }
}
