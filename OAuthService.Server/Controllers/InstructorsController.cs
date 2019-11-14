using Microsoft.AspNetCore.Mvc;
using System;

namespace OAuthService.Server.Controllers
{
    [Route("api/[controller]")]
    public class InstructorsController : Controller
    {
        private readonly SeedSchemes _dbSeedData;

        public InstructorsController(SeedSchemes dbSeedData)
        {
            _dbSeedData = dbSeedData;
        }

        [HttpGet("migrate-scheme")]
        public IActionResult MigrateScheme()
        {

            try
            {
                _dbSeedData.MigrateSchemes();
            }
            catch (Exception ex)
            {
                return BadRequest($"Message: {ex.Message} /<br/> StackTrace: {ex.StackTrace} /<br/> InnerException: {ex.InnerException}");
            }


            return Ok("Done");
        }

        [HttpGet("init-data")]
        public IActionResult initData()
        {
            try
            {
                _dbSeedData.InitData();
            }
            catch (Exception ex)
            {
                return BadRequest($"Message: {ex.Message} /<br/> StackTrace: {ex.StackTrace} /<br/> InnerException: {ex.InnerException}");
            }


            return Ok("Done");
        }
    }
}
