using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Dto.Products;
using System.Data;

namespace Beauty4u.Models.DataAccess.B4u
{
    public interface IProductRepository
    {
        Task<List<IBulkProductResultDto>> BulkProductRegisterAsync(IBulkProductParams bulkProductRequest);
        Task BulkProductUpdateAsync(IBulkProductParams bulkProductRequest);
        Task<List<IBulkProductUpdatePreviewResult>> BulkUpdatePreview(DataTable bulkUpdateList);
        Task<List<ISearchProductResult>> ProductTransferSearchAsync(DateOnly startDate, DateOnly endDate);
        Task<List<ISearchProductResult>> ProductSearchByUPCListAsync(DataTable upcList);
        Task<List<IUPCValidateResult>> ValidateUPCListAsync(DataTable upcList);
        Task<List<IBulkProductUpdatePreviewResult>> TransferProductsAsync(ITransferProductParams transferProductParams);
        Task LogBulkProductRequestAsync(IBulkProductRequestParams bulkProductRequestParams);
        Task LogProductTransfersAsync(ILogProductTransfersParam logProductTransfersParam);
    }
}