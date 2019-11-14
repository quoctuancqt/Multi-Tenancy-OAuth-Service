using Microsoft.EntityFrameworkCore;
using OAuthService.DistributedCache.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OAuthService.ContextFactory.ContextFactory
{
    public interface ITenantFactory
    {
        Task<TenantProfileModel> GetTenantByTenantIdAsync(string id);

        T GetTenantContext<T>(string id) where T : DbContext;

        IList<string> GetTenantIds();

        Task<string> GenateSecurityKeyBySubDomainAsync(string subDomain);

        Task<string> GetSecretBySubDomainAsync(string subDomain);

        string GetTenantId(string text);

        Task<string> GetSecretByTenantIdAsync(string id);

        Task CreateAsync(string tenantId);

        Task UpdateAsync(string tenantId);
    }
}
