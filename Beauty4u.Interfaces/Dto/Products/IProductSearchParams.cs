namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IProductSearchParams
    {
        string? Brand { get; set; }
        string? Category { get; set; }
        string? Color { get; set; }
        string? Size { get; set; }
        string? Sku { get; set; }
        string? StyleCode { get; set; }
        string? StyleDesc { get; set; }
        string? UPC { get; set; }
        string? VendorCode { get; set; }
        string? RetailPrice { get; set; }
    }
}