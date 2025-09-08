using Asp.Versioning;
using AutoMapper;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Promotions;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Common;
using Beauty4u.Models.Dto.ItemGroup;
using Beauty4u.Models.Dto.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IStoreService _storeService;
        private readonly ISystemSetupService _systemSetupService;
        private readonly IMapper _mapper;
        public PromotionsController(IPromotionService promotionService, IMapper mapper, ISystemSetupService systemSetupService, IStoreService storeService)
        {
            _promotionService = promotionService;
            _mapper = mapper;
            _systemSetupService = systemSetupService;
            _storeService = storeService;
        }

        [HttpGet("search-by-sku")]
        public async Task<IActionResult> SearchBySku(string sku)
        {
            var output = await _promotionService.GetProductPromotionsBySkuAsync(sku);
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpPost("search-items-by-promono")]
        public async Task<IActionResult> SearchItemsByPromoNo([FromBody] GetProductPromotionRequest getProductPromotionRequest)
        {
            var output = await _promotionService.GetProductPromotionsByPromoNoAsync(getProductPromotionRequest);
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpGet("get-by-promono")]
        public async Task<IActionResult> GetByPromoNo(string promono)
        {
            var output = await _promotionService.GetByPromoNoAsync(promono);
            if (output == null)
                return NoContent();
            return Ok(output);
        }

        [HttpPost("search-promo")]
        public async Task<IActionResult> SearchPromo([FromBody] PromoSearchParams promoSearchParams)
        {
            var output = await _promotionService.SearchPromotionsAsync(promoSearchParams);
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpPost("create-promo")]
        public async Task<IActionResult> CreatePromo([FromBody] PromotionRequest promotionCreateRequest)
        {
            await _promotionService.CreatePromotionAsync(promotionCreateRequest);
            return Ok();
        }

        [HttpPost("update-promo")]
        public async Task<IActionResult> UpdatePromo([FromBody] PromotionRequest promotionRequest)
        {
            await _promotionService.UpdatePromotionAsync(promotionRequest);
            return Ok();
        }

        [HttpPost("transfer-promo")]
        public async Task<IActionResult> TransferPromo([FromBody] PromotionRequest promotionRequest)
        {
            var systemSetup = await _systemSetupService.GetSystemSetupAsync();
            try
            {
                await _promotionService.TransferPromoAsync(promotionRequest);
                return Ok(new PromoTransferResult()
                {
                    IsSuccess = true,
                    Message = "Promo transfer success!",
                    StoreCodes = new List<NameValue<string>>() { new NameValue<string> { Name = systemSetup["B"].Description, Value = systemSetup["B"].Value } }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new PromoTransferResult()
                {
                    IsSuccess = false,
                    Message = "Promo transfer failed!",
                    StoreCodes = new List<NameValue<string>>() { new NameValue<string> { Name = systemSetup["B"].Description, Value = systemSetup["B"].Value } }
                });
            }
        }

        [HttpPost("transfer-promo-to-stores")]
        public async Task<IActionResult> TransferPromoToStores([FromBody] PromoTransferRequest promoTransferRequest)
        {
            var stores = await _storeService.GetAllStoresAsync();
            try
            {
                var updatedStores = await _promotionService.TransferPromoToStoresAsync(promoTransferRequest);

                var storeDict = stores.Where(x => updatedStores.Where(x => !string.IsNullOrWhiteSpace(x)).Contains(x.StoreAbbr)).ToDictionary(x => x.Code);
                return Ok(new PromoTransferResult()
                {
                    IsSuccess = true,
                    Message = "Promo transfer success!",
                    StoreCodes = storeDict.Values.Select(x => new NameValue<string>() { Name = x.StoreAbbr, Value = x.Code }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new PromoTransferResult() { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost("update-promo-store")]
        public async Task<IActionResult> UpdatePromoStore([FromBody] PromoTransferRequest promotionRequest)
        {
            try
            {
                await _promotionService.UpdatePromoStoreAsync(promotionRequest);
                return Ok(new PromoTransferResult() { IsSuccess = true, Message = "Promo update success!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new PromoTransferResult() { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("transfer-all-promo-to-stores")]
        public async Task<IActionResult> TransferAllPromoToStores()
        {
            try
            {
                await _promotionService.TransferAllPromoToStoresAsync();
                return Ok(new PromoTransferResult() { IsSuccess = true, Message = "Promo transfer success!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new PromoTransferResult() { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
