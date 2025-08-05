using Asp.Versioning;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;
        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stores = await _storeService.GetActiveStoresAsync();
            return Ok(stores);
        }

    }
}
