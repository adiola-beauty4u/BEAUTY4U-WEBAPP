
namespace Beauty4u.Interfaces.Dto
{
    public interface IProductDto
    {
        string Brand { get; set; }
        string Color { get; set; }
        decimal Cost { get; set; }
        string ItmGroup { get; set; }
        decimal RetailPrice { get; set; }
        string Size { get; set; }
        string StyleCode { get; set; }
        string StyleDesc { get; set; }
        string UPC { get; set; }
        string Sku { get; set; }
        string VendorCode { get; set; }
        string VendorName { get; set; }
    }
}