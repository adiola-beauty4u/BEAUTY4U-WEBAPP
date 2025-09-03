using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.Api
{
    public interface IPromotionsApi
    {
        Task<T> SeachItemsByPromoNoInApiAsync<T>(string baseAddress, string jwtToken, string promoNo);
        Task<T> SeachBySkuInApiAsync<T>(string baseAddress, string jwtToken, string sku);
        Task<T> SearchPromoInApiAsync<T>(string baseAddress, string jwtToken, IPromoSearchParams promoSearchParams);
        Task<T> TransferPromoToStoreAsync<T>(string baseAddress, string jwtToken, IPromotionRequest promotionRequest);
        Task<T> UpdatePromoStoreAsync<T>(string baseAddress, string jwtToken, IPromoTransferRequest promotionRequest);
    }
}