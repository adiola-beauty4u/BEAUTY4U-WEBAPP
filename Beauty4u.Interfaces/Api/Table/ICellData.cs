namespace Beauty4u.Interfaces.Api.Table
{
    public interface ICellData
    {
        object? RawValue { get; set; }
        string? TextValue { get; set; }
        bool IsValid { get; set; }
        string Tooltip { get; set; }
        string CssClass { get; set; }
        string CssIcon { get; set; }
        object? CommandParameter { get; set; }
    }
}