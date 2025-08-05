using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto.Products;

namespace Beauty4u.Models.Api.Products
{
    public class ProductTransferApiResult
    {
        public string StoreCode { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TableDataApi TableData { get; set; } = new TableDataApi();
        public List<BulkProductUpdatePreviewResult> TransferResult { get; set; } = new List<BulkProductUpdatePreviewResult>();
    }
}
