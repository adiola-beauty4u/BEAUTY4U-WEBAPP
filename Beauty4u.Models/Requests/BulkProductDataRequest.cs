using Beauty4u.Interfaces.Requests;

namespace Beauty4u.Models.Requests
{
    public class BulkProductDataRequest : IBulkProductDataRequest
    {
        public string Brand { get; set; } = string.Empty;
        public string StyleCode { get; set; } = string.Empty;
        public string StyleDesc { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Retail { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
        public string ItmGroup { get; set; } = string.Empty;
        public string UPC { get; set; } = string.Empty;
    }
}
