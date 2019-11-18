using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OAuthService.Core.Helpers;

namespace OAuthService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class ApiBase : ControllerBase
    {
        protected readonly IConfiguration _configuration;

        protected readonly IWebHostEnvironment _hostingEnvironment;

        protected string DisplayName => User.GetValue(ClaimKeyHelper.NAME);

        protected string RoleName => User.GetValue(ClaimKeyHelper.ROLE);

        protected string UserId => User.GetValue(ClaimKeyHelper.USER_ID);

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
