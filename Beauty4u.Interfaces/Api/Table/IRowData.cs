namespace Beauty4u.Interfaces.Api.Table
{
    public interface IRowData
    {
        Dictionary<string, ICellData> Cells { get; set; }
        bool IsValid { get; set; }
        string RowKey { get; set; }
        string Tooltip { get; set; }
        string CssClass { get; set; }
        bool IsNew { get; set; }
        bool IsChanged { get; set; }
        object? AdditionalData { get; set; }
    }
}