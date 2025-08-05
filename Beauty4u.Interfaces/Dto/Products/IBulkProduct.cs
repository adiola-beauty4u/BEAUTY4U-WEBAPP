namespace Beauty4u.Models.Dto.Products
{
    public interface IBulkProduct
    {
        string UserCode { get; set; }
        string VendorCode { get; set; }
        int VendorId { get; set; }
        string VendorName { get; set; }
    }
}