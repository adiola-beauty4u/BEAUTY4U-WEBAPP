using Asp.Versioning;
using AutoMapper;
using Beauty4u.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SystemController : ControllerBase
    {
        private readonly ISystemSetupService _systemSetupService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public SystemController(ISystemSetupService systemSetupService, IMapper mapper, IConfiguration configuration)
        {
            _systemSetupService = systemSetupService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var system = await _systemSetupService.GetSystemSetupAsync();
            return Ok(system);
        }

        [HttpGet("syscode-by-class")]
        public async Task<IActionResult> SysCodeByClass(string sysCodeClass)
        {
            var sysCodes = await _systemSetupService.GetSysCodesByClassAsync(sysCodeClass);
            return Ok(sysCodes);
        }

        [HttpGet("config-values")]
        public async Task<IActionResult> ConfigValues(string configSection)
        {
            var conn = _configuration.GetConnectionString("B4uConnection");
            var value = _configuration.GetSection(configSection).Get<string>();
            return Ok(new { connection = conn, sectionName = configSection, value = value });
        }
    }
}
