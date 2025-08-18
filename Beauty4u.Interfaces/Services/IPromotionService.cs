using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.Services
{
    public interface IPromotionService
    {
        Task<ITableData> GetProductPromotionsByPromoNoAsync(string promoNo);
        Task<ITableData> GetProductPromotionsBySkuAsync(string sku);
        Task<ITableData> SearchPromotionsAsync(IPromoSearchParams promoSearchParams);
    }
}