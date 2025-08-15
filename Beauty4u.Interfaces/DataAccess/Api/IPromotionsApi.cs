using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Interfaces.DataAccess.Api
{
    public interface IPromotionsApi
    {
        Task<T> SeachItemsByPromoNoInApiAsync<T>(string baseAddress, string jwtToken, string promoNo);
        Task<T> SeachBySkuInApiAsync<T>(string baseAddress, string jwtToken, string sku);
        Task<T> SearchPromoInApiAsync<T>(string baseAddress, string jwtToken, IPromoSearchParams promoSearchParams);
    }
}