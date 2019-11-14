using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Linq;
using OAuthService.DistributedCache;
using OAuthService.DistributedCache.Models;
using OAuthService.Core.Exceptions;
using OAuthService.Core.Helper;
using OAuthService.Core.Helpers;

namespace OAuthService.ContextFactory.ContextFactory
{
    public class TenantFactory : ITenantFactory
    {
        private ConcurrentDictionary<string, DbContextOptionsBuilder<DbContext>> dbContextOptionsBuilders;

        private readonly IDistributedCacheService _distributedCache;

        private readonly IConfiguration _configuration;

        private readonly TenantConfig _tenantConfig;

        private readonly IPasswordHasher<User> _passwordHasher;

        public TenantFactory(IDistributedCacheService distributedCache,
            IConfiguration configuration,
            IOptions<TenantConfig> tenantConfig,
            IPasswordHasher<User> passwordHasher)
        {
            _distributedCache = distributedCache;

            _configuration = configuration;

            dbContextOptionsBuilders = new ConcurrentDictionary<string, DbContextOptionsBuilder<DbContext>>();

            _tenantConfig = tenantConfig.Value;

            _passwordHasher = passwordHasher;
        }

        public async Task<TenantProfileModel> GetTenantByTenantIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ForbiddenException();

            try
            {
                var tenant = await _distributedCache.GetAsync<TenantProfileModel>(id);

                if (tenant == null) throw new BadRequestException($"Not found tenant {id}");

                return tenant;

            }
            catch
            {
                return await GetTenantByHtppAsync(id);
            }

        }

        public T GetTenantContext<T>(string id) where T : DbContext
        {
            DbContextOptionsBuilder<DbContext> options;

            if (!dbContextOptionsBuilders.TryGetValue(id, out options))
            {
                var tenant = AsyncHelper.RunSync(() => GetTenantByTenantIdAsync(id));

                if (tenant == null) throw new BadRequestException($"Not found tenantId {id}");

                options = new DbContextOptionsBuilder<DbContext>();

                options.UseSqlServer(tenant.GetSqlConnectionString(_configuration.GetConnectionString("ConnectionString")));

                dbContextOptionsBuilders.TryAdd(id, options);
            }

            return (T)Activator.CreateInstance(typeof(T), options.Options);

        }

        public async Task<string> GenateSecurityKeyBySubDomainAsync(string subDomain)
        {
            var tenant = await GetTenantByTenantIdAsync(_tenantConfig.GetTenantIdBySubDomain(subDomain));

            if (tenant == null) throw new BadRequestException($"Not found");

            return CipherHelper.Encrypt(tenant.TenantId);
        }

        public string GetTenantId(string text)
        {
            return CipherHelper.Decrypt(text);
        }

        public async Task<string> GetSecretBySubDomainAsync(string subDomain)
        {
            var tenant = await GetTenantByTenantIdAsync(_tenantConfig.GetTenantIdBySubDomain(subDomain));

            return tenant.Secret;
        }

        public async Task<string> GetSecretByTenantIdAsync(string id)
        {
            var tenant = await GetTenantByTenantIdAsync(id);

            return tenant.Secret;
        }

        public IList<string> GetTenantIds()
        {
            return System.IO.File.ReadAllText(_tenantConfig.TenantStorage).JsonToObj<IList<string>>();
        }

        public async Task CreateAsync(string tenantId)
        {
            var tenant = await GetTenantByTenantIdAsync(tenantId);

            UpdateJsonTenantFile(tenantId);

            using (var context = GetTenantContext<IDentityDbCo>(tenantId))
            {
                context.Database.Migrate();

                string administratorRoleId = "5b549a27-78c9-474d-87d9-ed1439a9fd10";

                if (!await context.Roles.AnyAsync(x => x.Name.Equals(UserRoles.Administrator)))
                {
                    context.Roles.Add(new Role(administratorRoleId, UserRoles.Administrator, UserRoles.Administrator));
                }

                if (!await context.Roles.AnyAsync(x => x.Name.Equals(UserRoles.Presenter)))
                {
                    context.Roles.Add(new Role(UserRoles.Presenter, UserRoles.Presenter));
                }

                if (!await context.Roles.AnyAsync(x => x.Name.Equals(UserRoles.Participant)))
                {
                    context.Roles.Add(new Role(UserRoles.Participant, UserRoles.Participant));
                }

                if (!await context.Users.AnyAsync(x => x.Id.Equals(tenant.OwnerId)))
                {
                    var user = new User(tenant.OwnerId, tenant.OwnerEmail);

                    user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@1234");

                    context.Users.Add(user);

                    context.UserRoles.Add(new UserRole(tenant.OwnerId, administratorRoleId));
                }

                if (!await context.WorkTitles.AnyAsync())
                {
                    var data = TitleService.LoadData().JsonToObj<List<TitleModel>>();

                    context.WorkTitles.AddRange(data.Select(x => new WorkTitle
                    {
                        Id = x.Id,
                        Name = x.Name
                    }));
                }

                if (!await context.CityOrProvinces.AnyAsync())
                {
                    var cities = LocationService.LoadData().JsonToObj<IList<CityOrProvince>>();

                    context.CityOrProvinces.AddRange(cities);
                }
                
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(string tenantId)
        {
            await _distributedCache.RefreshAsync(tenantId);

            await CreateAsync(tenantId);
        }

        private void UpdateJsonTenantFile(string tenantId)
        {
            var list = System.IO.File.ReadAllText(_tenantConfig.TenantStorage).JsonToObj<IList<string>>();

            if (list == null) list = new List<string>();

            if (!list.Any(x => x.Equals(tenantId)))
            {
                list.Add(tenantId);

                System.IO.File.WriteAllText(_tenantConfig.TenantStorage, list.ObjToJson());
            }
        }

        private async Task<TenantProfileModel> GetTenantByHtppAsync(string tenantId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.GetSection("JWTSettings").GetValue<string>("Issuer"));

                var response = await client.GetAsync($"api/clients/refresh-cache/{tenantId}");

                return await response.Content.ReadAsAsync<TenantProfileModel>();
            }
        }
    }
}
