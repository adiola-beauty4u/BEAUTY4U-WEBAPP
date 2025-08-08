using Asp.Versioning;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Dto.ItemGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("search-by-sku")]
        public async Task<IActionResult> SearchBySku(string sku)
        {
            var output = await _promotionService.GetProductPromotionsBySkuAsync(sku);
            return Ok(output);
        }
    }
}
