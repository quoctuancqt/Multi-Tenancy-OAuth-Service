using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OAuthService.TenantFactory;
using System;
using System.Threading.Tasks;

namespace OAuthService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ApiBase
    {
        private readonly ITenantFactory _tenantFactory;

        public TenantsController(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            ITenantFactory tenantFactory)
            : base(configuration, hostingEnvironment)
        {
            _tenantFactory = tenantFactory;
        }


        [HttpPost("create/{tenantId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string tenantId)
        {
            try
            {
                await _tenantFactory.CreateAsync(tenantId);

                return Success();
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} --- StackTrace: {ex.StackTrace.ToString()} --- InnerException: {ex.InnerException?.ToString()}";

                System.IO.File.WriteAllText(@"C:\inetpub\wwwroot\WebCast.Admin\logs\1.txt", message);

                throw ex;
            }
        }

        [HttpPut("update/{tenantId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(string tenantId)
        {
            await _tenantFactory.UpdateAsync(tenantId);

            return Success();
        }

        [HttpGet("security-key/{subdomain}")]
        [AllowAnonymous]
        public async Task<IActionResult> GenateSecurityKey(string subdomain)
        {
            return Ok(await _tenantFactory.GenateSecurityKeyBySubDomainAsync(subdomain));
        }

    }
}
