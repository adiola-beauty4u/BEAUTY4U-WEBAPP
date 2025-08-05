using Asp.Versioning;
using Beauty4u.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _healthService;
        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Ok");
        }

        [HttpGet("health-check")]
        public async Task<IActionResult> HealthCheck()
        {
            return Ok(await _healthService.ServersUptimeCheckAsync());
        }
    }
}
