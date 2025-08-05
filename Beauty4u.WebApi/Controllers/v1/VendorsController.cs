using Asp.Versioning;
using AutoMapper;
using Beauty4u.Models.Common;
using Beauty4u.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Beauty4u.Interfaces.Services;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IMapper _mapper;
        public VendorsController(IVendorService vendorService, IMapper mapper)
        {
            _vendorService = vendorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendors = await _vendorService.GetVendorsAsync();
            return Ok(vendors);
        }
    }
}
