using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IPromotionRepository
    {
        Task<List<IProductPromotion>> GetProductPromotionsByPromoNoAsync(string promoNo);
        Task<List<IProductPromotion>> GetProductPromotionsBySkuAsync(string sku);
        Task<List<IPromotionDto>> SearchPromotionsAsync(IPromoSearchParams promoSearchParams);
    }
}