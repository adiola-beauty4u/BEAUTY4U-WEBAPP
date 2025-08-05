namespace Beauty4u.Interfaces.Requests
{
    public interface IBulkProductDataRequest
    {
        string Brand { get; set; }
        string Color { get; set; }
        string Cost { get; set; }
        string ItmGroup { get; set; }
        string Retail { get; set; }
        string Size { get; set; }
        string StyleCode { get; set; }
        string StyleDesc { get; set; }
        string UPC { get; set; }
    }
}