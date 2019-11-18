using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using OAuthService.Server.Dto;
using OAuthService.Server.Services;

namespace OAuthService.Server.Controllers
{
    public class ClientsController : ApiBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            IClientService clientService)
            : base(configuration, hostingEnvironment)
        {
            _clientService = clientService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] AddOrEditClientDto dto)
        {
            await _clientService.CreateAsync(dto);

            return Success();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AddOrEditClientDto dto)
        {
            await _clientService.UpdateAsync(id, dto);

            return Success();
        }

        [HttpPut("{id}/common")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCommon(string id, [FromBody] EditClientCommonInfoDto dto)
        {
            await _clientService.UpdateAsync(id, dto);

            return Success();
        }

        [HttpGet("secret/{subDomain}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSecretBySubDomain(string subDomain)
        {
            return Ok(await _clientService.GetSecretKeyBySubDomainAsync(subDomain));
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _clientService.GetAllAsync());
        }

        [HttpGet("refresh-cache/{id}")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RefreshCacheByClientId(string id)
        {
            return Ok(await _clientService.RefreshCacheByClientIdAsync(id));
        }
    }
}
