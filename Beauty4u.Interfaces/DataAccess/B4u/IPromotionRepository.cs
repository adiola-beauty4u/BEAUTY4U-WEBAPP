using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface IPromotionRepository
    {
        Task<List<IProductPromotion>> GetProductPromotionsBySkuAsync(string sku);
    }
}