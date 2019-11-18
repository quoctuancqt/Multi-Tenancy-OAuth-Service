using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OAuthService.JWT.Helper;
using OAuthService.JWT.Proxy;
using OAuthService.TenantFactory;
using System.Threading.Tasks;

namespace OAuthService.WebApi.Controllers
{
    public class AccountsController : ApiBase
    {
        private readonly OAuthClient _oAuthClient;
        private readonly ITenantFactory _tenantFactory;

        public AccountsController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment,
            OAuthClient oAuthClient, ITenantFactory tenantFactory) : base(configuration, hostingEnvironment)
        {
            _oAuthClient = oAuthClient;
            _tenantFactory = tenantFactory;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] LoginDto dto)
        {
            var webSecurity = HttpContext.Request.Headers["WebSecurity"].ToString();

            string tenantId = _tenantFactory.GetTenantId(webSecurity);

            var secret = await _tenantFactory.GetSecretByTenantIdAsync(tenantId);

            var result = await _oAuthClient.EnsureApiTokenAsync(dto.Username,
                dto.Password, secret);

            return Ok(result);
        }
    }

    #region TO BE REMOVED
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Username);
            RuleFor(x => x.Password);
        }
    }
    #endregion
}
