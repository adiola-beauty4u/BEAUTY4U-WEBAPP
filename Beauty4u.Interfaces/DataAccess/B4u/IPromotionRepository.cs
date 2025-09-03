using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Models.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IPromotionRepository
    {
        Task CreatePromotionAsync(IPromotionRequest promotionCreateRequest);
        Task<List<IProductPromotion>> GetProductPromotionsByPromoNoAsync(string promoNo);
        Task<List<IProductPromotion>> GetProductPromotionsBySkuAsync(string sku);
        Task<IPromotionDto> GetByPromoNoAsync(string promono);
        Task<List<IPromotionDto>> SearchPromotionsAsync(IPromoSearchParams promoSearchParams);
        Task<List<IPromotionRuleDto>> GetPromoRulesByPromoNoAsync(string promono);
        Task UpdatePromotionAsync(IPromotionRequest promotionRequest);
        Task TransferPromoAsync(IPromotionRequest promotionRequest);
        Task UpdatePromoStoreAsync(IPromoTransferRequest promoTransferRequest);
    }
}