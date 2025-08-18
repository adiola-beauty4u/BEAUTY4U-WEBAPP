using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Dto.Products;
using Microsoft.AspNetCore.Http;

namespace Beauty4u.Interfaces.Services
{
    public interface IProductService
    {
        Task<ITableData> BulkProductRegisterAsync(IBulkProduct bulkProductRequest);
        Task<ITableData> BulkProductUpdateAsync(IFormFile productFile);
        Task<IBulkProductPreviewResult> BulkUpdatePreviewAsync(IFormFile productFile);
        Task<IProductSearchResult> ProductTransferSearchAsync(DateOnly dateStart, DateOnly dateEnd);
        Task<List<ISearchProductResult>> ProductSearchByUPCListAsync(List<string> upcList);
        Task<ITableData> ProductTransferPreviewAsync(IProductTransferRequest productTransferRequest);
        Task<IProductTransferResult> TransferProductsAsync(List<ISearchProductResult> productTransferRequest);
        Task<ITableData> ProductTransferToStoresAsync(IProductTransferRequest productTransferRequest);
        Task<IBulkProductPreviewResult> BulkRegisterPreviewAsync(IBulkProduct bulkProductRequest);
        Task<ITableData> SearchProductsAsync(IProductSearchParams searchParams);
        Task<List<ISearchProductResult>> ProductSearchBySkuListAsync(List<string> skuList);
    }
}