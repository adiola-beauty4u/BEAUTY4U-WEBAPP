using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IPromotionRepository
    {
        Task CreatePromotions(IPromotionCreateRequest promotionCreateRequest);
        Task<List<IProductPromotion>> GetProductPromotionsByPromoNoAsync(string promoNo);
        Task<List<IProductPromotion>> GetProductPromotionsBySkuAsync(string sku);
        Task<List<IPromotionDto>> SearchPromotionsAsync(IPromoSearchParams promoSearchParams);
    }
}