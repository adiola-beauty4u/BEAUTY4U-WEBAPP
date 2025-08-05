using Asp.Versioning;
using AutoMapper;
using Beauty4u.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SystemController : ControllerBase
    {
        private readonly ISystemSetupService _systemSetupService;
        private readonly IMapper _mapper;
        public SystemController(ISystemSetupService systemSetupService, IMapper mapper)
        {
            _systemSetupService = systemSetupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendors = await _systemSetupService.GetSystemSetupAsync();
            return Ok(vendors);
        }
    }
}
