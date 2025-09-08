using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Models.Api.Promotions;

namespace Beauty4u.Interfaces.Services
{
    public interface IPromotionService
    {
        Task CreatePromotionAsync(IPromotionRequest promotionCreateRequest);
        Task<IPromotionDto> GetByPromoNoAsync(string promoNo);
        Task<ITableData> GetProductPromotionsByPromoNoAsync(IGetProductPromotionRequest getProductPromotionRequest);
        Task<ITableData> GetProductPromotionsBySkuAsync(string sku);
        Task<ITableData> SearchPromotionsAsync(IPromoSearchParams promoSearchParams);
        Task TransferAllPromoToStoresAsync();
        Task TransferPromoAsync(IPromotionRequest promotionRequest);
        Task<List<string>> TransferPromoToStoresAsync(IPromoTransferRequest promoTransferRequest);
        Task UpdatePromoStoreAsync(IPromoTransferRequest promotionRequest);
        Task UpdatePromotionAsync(IPromotionRequest promotionRequest);
    }
}