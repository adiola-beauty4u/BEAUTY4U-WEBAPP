using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Models.Api.Table;

namespace Beauty4u.Models.Api.Products
{
    public class BulkProductPreviewResult : IBulkProductPreviewResult
    {
        public ITableData TableData { get; set; } = new TableData();
        public bool IsValid { get; set; } = false;
        public string PreviewResult { get; set; } = null!;
    }
}
