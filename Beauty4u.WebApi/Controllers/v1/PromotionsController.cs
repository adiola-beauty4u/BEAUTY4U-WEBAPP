using Asp.Versioning;
using AutoMapper;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.ItemGroup;
using Beauty4u.Models.Dto.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IMapper _mapper;
        public PromotionsController(IPromotionService promotionService, IMapper mapper)
        {
            _promotionService = promotionService;
            _mapper = mapper;
        }

        [HttpGet("search-by-sku")]
        public async Task<IActionResult> SearchBySku(string sku)
        {
            var output = await _promotionService.GetProductPromotionsBySkuAsync(sku);
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpGet("search-items-by-promono")]
        public async Task<IActionResult> SearchItemsByPromoNo(string promono)
        {
            var output = await _promotionService.GetProductPromotionsByPromoNoAsync(promono);
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpPost("search-promo")]
        public async Task<IActionResult> SearchPromo([FromBody] PromoSearchParams promoSearchParams)
        {
            var output = await _promotionService.SearchPromotionsAsync(promoSearchParams);
            return Ok(_mapper.Map<TableDataApi>(output));
        }
    }
}
