using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OAuthService.Core.Exceptions;
using OAuthService.DistributedCache;
using OAuthService.DistributedCache.Models;
using OAuthService.JWT.Services;
using OAuthService.Server.Domain;
using OAuthService.Server.Dto;
using OAuthService.Server.MapperProfiles;
using OAuthService.Server.Proxy;

namespace OAuthService.Server.Services
{
    public class ClientService : JWTClientService<Client>, IClientService
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly TenantConfig _tenantConfig;

        protected override DbSet<Client> _clients { get; set; }

        protected DbSet<ClientConfiguration> _clientConfiguration { get; set; }

        protected DbSet<User> _user { get; set; }

        protected DbSet<UserClient> _userClient { get; set; }

        private readonly OAuthContext _context;

        private readonly ProxyService _proxyService;

        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly IDistributedCacheService _distributedCache;

        public ClientService(OAuthContext context,
            IConfiguration configuration,
            IOptions<TenantConfig> tenantConfig,
            IWebHostEnvironment hostingEnvironment,
            IPasswordHasher<User> passwordHasher,
            ProxyService proxyService,
            IDistributedCacheService distributedCache)
        {
            _context = context;

            _clients = context.Clients;

            _clientConfiguration = context.ClientConfigurations;

            _user = context.Users;

            _userClient = context.UserClients;

            _passwordHasher = passwordHasher;

            _configuration = configuration;

            _tenantConfig = tenantConfig.Value;

            _hostingEnvironment = hostingEnvironment;

            _proxyService = proxyService;

            _distributedCache = distributedCache;
        }

        public async Task CreateAsync(AddOrEditClientDto dto)
        {
            string clientId = await CheckDuplicationClientIdAsync(dto.SubDomain);

            var entity = dto.ToEntity();

            entity.ClientId = clientId;

            entity.ClientUri = _tenantConfig.ParseClientUriBySubDomain(dto.SubDomain);

            _clients.Add(entity);

            AddClientConfiguration(entity);

            var user = new User { UserName = $"{dto.SubDomain}@{_tenantConfig.Domain}" };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@1234");

            _user.Add(user);

            _userClient.Add(new UserClient(user.Id, entity.Id));

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = _context.Database.BeginTransaction();

                await _context.SaveChangesAsync();

                await SetCacheTenantProfileAsync(entity, user);

                await _proxyService.CreateTenantAsync(entity.ClientId);

                transaction.Commit();
            });

        }

        public async Task UpdateAsync(string id, AddOrEditClientDto dto)
        {
            var entity = await _clients.SingleOrDefaultAsync(x => x.Id.Equals(id));

            var clientConfiguration = await _clientConfiguration.Where(x => x.ClientId.Equals(id)).ToListAsync();

            _clientConfiguration.RemoveRange(clientConfiguration);

            AddClientConfiguration(entity);

            var user = await _user.SingleOrDefaultAsync(x => x.UserName.Equals($"{entity.SubDomain}@tiagamma.com"));

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    await _context.SaveChangesAsync();

                    await SetCacheTenantProfileAsync(entity, user);

                    await _proxyService.UpdateTenantAsync(entity.ClientId);

                    transaction.Commit();
                }
            });
        }

        public async Task<string> GetSecretKeyBySubDomainAsync(string subDomain)
        {
            var client = await _clients.FirstOrDefaultAsync(x => x.SubDomain.Equals(subDomain));

            if (client == null) throw new BadRequestException($"Not found sub domain {subDomain}.");

            return Base64Encode($"{client.ClientId}:{client.Secret}");
        }

        public async Task<TenantProfileModel> RefreshCacheByClientIdAsync(string cliengId)
        {
            var entity = await _clients.SingleOrDefaultAsync(x => x.ClientId.Equals(cliengId));

            var user = await _user.SingleOrDefaultAsync(x => x.UserName.Equals($"{entity.SubDomain}@{_tenantConfig.Domain}"));

            return await SetCacheTenantProfileAsync(entity, user);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clients.ToListAsync();
        }

        private async Task<string> CheckDuplicationClientIdAsync(string subDomain)
        {
            string clientId = _tenantConfig.ParseClientIdBySubDomain(subDomain);

            if (await _clients.AnyAsync(x => x.ClientId.Equals(clientId)))
                throw new BadRequestException($"This sub domain already exists.");

            return clientId;
        }

        private async Task<TenantProfileModel> SetCacheTenantProfileAsync(Client entity, User user)
        {
            await _distributedCache.RemoveAsync(entity.ClientId);

            var tenant = new TenantProfileModel
            {
                Id = entity.Id,
                TenantId = entity.ClientId,
                ClientName = entity.ClientName,
                ClientUri = entity.ClientUri,
                LogoUri = entity.LogoUri,
                ShortcutIconUri = entity.ShortcutIconUri,
                IsVerifyUser = entity.IsVerifyUser,
                SubDomain = entity.SubDomain,
                OwnerId = user.Id,
                OwnerEmail = user.UserName,
                Secret = Base64Encode($"{entity.ClientId}:{entity.Secret}"),
                ClientPhone = entity.ClientPhone,
                ClientEmail = entity.ClientEmail,
                ClientAddress = entity.ClientAddress,
                ClientStatus = entity.ClientStatus.ToString(),
                SqlServer = _tenantConfig.SqlServer,
                SqlDatabase = _tenantConfig.GetTenantDbName(entity.SubDomain),
                SqlUserName = _tenantConfig.SqlUserName,
                SqlPassword = _tenantConfig.SqlPassword,
                MongoDbServer = _tenantConfig.MongoDbServer,
                MongoDbDatabase = _tenantConfig.GetTenantDbName(entity.SubDomain),
                MongoDbUserName = _tenantConfig.MongoDbUserName,
                MongoDbPassword = _tenantConfig.MongoDbPassword
            };

            await _distributedCache.SetAsync(entity.ClientId, tenant);

            return tenant;
        }

        private void AddClientConfiguration(Client entity)
        {
            #region SQl

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "SqlServer", _tenantConfig.SqlServer));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "SqlDatabase", _tenantConfig.GetTenantDbName(entity.SubDomain)));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "SqlUserName", _tenantConfig.SqlUserName));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "SqlPassword", _tenantConfig.SqlPassword));

            #endregion SQl

            #region MongoDB

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "MongoDbServer", _tenantConfig.MongoDbServer));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "MongoDbDatabase", _tenantConfig.GetTenantDbName(entity.SubDomain)));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "MongoDbUserName", _tenantConfig.MongoDbUserName));

            _clientConfiguration.Add(new ClientConfiguration(entity.Id, "MongoDbPassword", _tenantConfig.MongoDbPassword));

            #endregion MongoDB
        }

        public Task AddOrEditMongoContextAsync(AddOrEditMongoContextDto dto)
        {
            throw new NotImplementedException();
        }

        public Task AddOrEditSqlContextAsync(AddOrEditSqlContextDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(string id, EditClientCommonInfoDto dto)
        {
            var entity = await _clients.SingleOrDefaultAsync(x => x.Id.Equals(id));

            entity = dto.ToEntity(entity);

            _clients.Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
