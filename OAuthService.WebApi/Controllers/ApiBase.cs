using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OAuthService.Core.Helpers;
using OAuthService.DistributedCache.Models;
using OAuthService.TenantFactory;

namespace OAuthService.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class ApiBase : ControllerBase
    {
        protected readonly IConfiguration _configuration;

        protected readonly IWebHostEnvironment _hostingEnvironment;

        protected string DisplayName => User.GetValue(ClaimKeyHelper.NAME);

        protected string RoleName => User.GetValue(ClaimKeyHelper.ROLE);

        protected string FirstName => User.GetValue(ClaimKeyHelper.FIRST_NAME);

        protected string UserId => User.GetValue(ClaimKeyHelper.USER_ID);

        protected string TenantId;

        protected TenantProfileModel TenantProfile => GetTenantProfile();

        private TenantProfileModel GetTenantProfile()
        {
            var tenantFactory = (ITenantFactory)HttpContext.RequestServices.GetService(typeof(ITenantFactory));

            string webCastSecurity = HttpContext.Request.Headers["WebSecurity"].ToString();

            TenantId = tenantFactory.GetTenantId(webCastSecurity);

            return AsyncHelper.RunSync(() => tenantFactory.GetTenantByTenantIdAsync(TenantId));
        }

        public ApiBase(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;

            _hostingEnvironment = hostingEnvironment;

        }

        protected OkObjectResult Success()
        {
            return Ok(new { success = true });
        }
    }
}
