namespace Beauty4u.Interfaces.DataAccess.Api
{
    public interface IPromotionsApi
    {
        Task<T> SeachBySkuInApiAsync<T>(string baseAddress, string jwtToken, string sku);
    }
}