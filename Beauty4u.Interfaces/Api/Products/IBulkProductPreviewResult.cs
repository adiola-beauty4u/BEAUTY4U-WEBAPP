using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Interfaces.Api.Products
{
    public interface IBulkProductPreviewResult
    {
        bool IsValid { get; set; }
        string PreviewResult { get; set; }
        ITableData TableData { get; set; }
    }
}