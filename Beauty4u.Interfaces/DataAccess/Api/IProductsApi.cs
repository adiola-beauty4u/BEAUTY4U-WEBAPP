using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Interfaces.DataAccess.Api
{
    public interface IProductsApi
    {
        Task<List<ISearchProductResult>> SearchProductBySkuFromApiAsync(string baseAddress, string jwtToken, List<string> skuList);
        Task<List<ISearchProductResult>> SearchProductByUpcFromApiAsync(string baseAddress, string jwtToken, List<string> upcList);
        Task<T> TransferProductsToApiAsync<T>(string baseAddress, string jwtToken, List<ISearchProductResult> transferList);
    }
}