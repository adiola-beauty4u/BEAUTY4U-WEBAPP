using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Interfaces.Services
{
    public interface IPromotionService
    {
        Task<ITableData> GetProductPromotionsBySkuAsync(string sku);
    }
}