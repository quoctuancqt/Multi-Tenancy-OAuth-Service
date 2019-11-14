using OAuthService.Core.Proxy;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuthService.Server.Proxy
{
    public class ProxyService : ProxyBase
    {
        private const string CREATE_TENANT = "/api/tenants/create";

        private const string UPDATE_TENANT = "/api/tenants/update";

        public ProxyService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> CreateTenantAsync(string tenantId)
        {
            var result = await PostAsJsonAsync($"{CREATE_TENANT}/{tenantId}", new { });

            result.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<HttpResponseMessage> UpdateTenantAsync(string tenantId)
        {
            var result = await PutAsJsonAsync($"{UPDATE_TENANT}/{tenantId}", new { });

            result.EnsureSuccessStatusCode();

            return result;
        }
    }
}
