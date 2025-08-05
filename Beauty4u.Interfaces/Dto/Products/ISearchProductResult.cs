namespace Beauty4u.Interfaces.Dto.Products
{
    public interface ISearchProductResult : IProductDto
    {
        string Sku { get; set; }
        string VendorCode { get; set; }
        string VendorName { get; set; }
        string Storecode { get; set; }
    }
}