using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Interfaces.Api.Products
{
    public interface IProductTransferResult
    {
        ITableData TableData { get; set; }
        List<IBulkProductUpdatePreviewResult> TransferResult { get; set; }
        string StoreCode { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
    }
}