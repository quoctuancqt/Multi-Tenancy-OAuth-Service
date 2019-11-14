using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using OAuthService.Server.Dto;
using OAuthService.Server.Services;
using OAuthService.JWT.Proxy;

namespace OAuthService.Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : ApiBase
    {
        private readonly OAuthClient _client;

        private readonly IAccountService _accountService;

        public AccountsController(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            OAuthClient client,
            IAccountService accountService)
            : base(configuration, hostingEnvironment)
        {
            _client = client;

            _accountService = accountService;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] LoginDto dto)
        {
            var result = await _client.EnsureApiTokenAsync(dto.UserName,
                dto.Password,
                _configuration.GetValue<string>("Secret"));

            return Ok(result.Result);
        }

        [HttpGet("token/{refreshToken}")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var response = await _client.RefreshTokenAsync(refreshToken, _configuration.GetValue<string>("Secret"));

            if (response.Success)
            {
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(response.Result);
            }
        }

        [HttpPost("sync-password")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SyncPassword([FromBody] IDictionary<string, string> dic)
        {
            await _accountService.UpdatePasswordAsync(dic["userName"], dic["password"]);

            return Success();
        }

        [HttpPost("sync-user")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SyncPassword([FromBody] AddOrEditAccountDto dto)
        {
            return Ok(await _accountService.CreateOrEditAsync(dto));
        }

        [HttpPost("change-userName/{userName}")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ChangeUserName(string userName)
        {
            await _accountService.ChangeUserNameAsync(userName);

            return Success();
        }
    }
}
