using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthService.Core.Exceptions;
using OAuthService.Core.Helpers;
using OAuthService.JWT.Models;
using OAuthService.Server.Domain;
using OAuthService.Server.Dto;
using OAuthService.Server.MapperProfiles;

namespace OAuthService.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly OAuthContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(OAuthContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;

            _passwordHasher = passwordHasher;
        }

        public async Task<string> CreateAsync(AddOrEditAccountDto dto)
        {
            if (await CheckDuplicationClientIdAsync(dto.UserName) != null)
                throw new BadRequestException($"This user already exists.");

            var client = await _context.Clients.SingleOrDefaultAsync(x => x.ClientId.Equals(dto.ClientId));

            if (client == null) throw new BadRequestException("Invalid client");

            var entity = dto.ToEntity();

            entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.Password);

            _context.Users.Add(entity);

            _context.UserClients.Add(new UserClient { UserId = entity.Id, ClientId = client.Id });

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<string> CreateOrEditAsync(AddOrEditAccountDto dto)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(x => x.ClientId.Equals(dto.ClientId));

            if (client == null) throw new BadRequestException("Invalid client");

            var entity = await CheckDuplicationClientIdAsync(dto.UserName);

            if (entity == null)
            {
                entity = dto.ToEntity();

                if (!string.IsNullOrEmpty(dto.Password))
                {
                    entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.Password);
                }

                _context.Users.Add(entity);

                _context.UserClients.Add(new UserClient { UserId = entity.Id, ClientId = client.Id });
            }
            else
            {
                var userClients = await _context.UserClients
                    .SingleOrDefaultAsync(x => x.UserId.Equals(entity.Id) && x.ClientId.Equals(client.Id));

                if (userClients == null)
                {
                    _context.UserClients.Add(new UserClient { UserId = entity.Id, ClientId = client.Id });
                }
            }

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdatePasswordAsync(string userName, string password)
        {
            var entity = await CheckDuplicationClientIdAsync(userName);

            if (entity != null)
            {
                entity.PasswordHash = _passwordHasher.HashPassword(entity, password);

                _context.Users.Update(entity);

                await _context.SaveChangesAsync();
            }
        }

        public async Task InviteUserAsync(InviteUserDto dto)
        {
            var entity = await CheckDuplicationClientIdAsync(dto.Email);

            if (entity == null)
            {
                entity = new User { UserName = dto.Email };

                _context.Users.Add(entity);

                await _context.SaveChangesAsync();
            }
        }

        public Task<AccountResult> VerifyAccountAsync(string username, string password, TokenRequest tokenRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountResult> VerifyAccountAsync(string clientId, string username, string password, TokenRequest tokenRequest)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(username));

            if (user == null) return new AccountResult(new { error = "Not found." });

            if (user.LockoutEnabled) return new AccountResult(new { error = "User locked." });

            var userClients = await _context.UserClients
                .Include(x => x.Client)
                .SingleOrDefaultAsync(x => x.UserId.Equals(user.Id) && x.ClientId.Equals(clientId));

            if (!user.IsSystem && userClients == null)
                return new AccountResult(new { error = "Invalid client." });

            if (!tokenRequest.Claims.Any(x => x.Type.Equals(ClaimKeyHelper.USER_ID)))
            {
                tokenRequest.Claims.Add(new CustomClaim(ClaimKeyHelper.USER_ID, user.Id));
            }

            tokenRequest.Claims.Add(new CustomClaim(ClaimKeyHelper.TENANT_ID, userClients.Client.ClientId));

            tokenRequest.Claims.Add(new CustomClaim(ClaimKeyHelper.TENANT_KEY, userClients.Client.Id));

            return new AccountResult(tokenRequest);
        }

        private async Task<User> CheckDuplicationClientIdAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(userName));
        }

        public async Task ChangeUserNameAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(email));

            if (user == null) return;

            user.UserName = email;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}
