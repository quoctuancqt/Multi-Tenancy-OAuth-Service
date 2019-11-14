using System.Collections.Generic;
using System.Threading.Tasks;
using OAuthService.Server.Domain;
using OAuthService.Server.Dto;
using OAuthService.JWT.Services;
using OAuthService.DistributedCache.Models;

namespace OAuthService.Server.Services
{
    public interface IClientService : IJWTClientService
    {
        Task CreateAsync(AddOrEditClientDto dto);

        Task UpdateAsync(string id, AddOrEditClientDto dto);

        Task<TenantProfileModel> RefreshCacheByClientIdAsync(string cliengId);

        Task<string> GetSecretKeyBySubDomainAsync(string subdomain);

        Task<IEnumerable<Client>> GetAllAsync();

        Task AddOrEditMongoContextAsync(AddOrEditMongoContextDto dto);

        Task AddOrEditSqlContextAsync(AddOrEditSqlContextDto dto);

        Task UpdateAsync(string id, EditClientCommonInfoDto dto);
    }
}
