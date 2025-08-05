using Beauty4u.Interfaces.Requests;

namespace Beauty4u.Models.Requests
{
    public class BulkProductDataRequest : IBulkProductDataRequest
    {
        public string Brand { get; set; } = null!;
        public string StyleCode { get; set; } = null!;
        public string StyleDesc { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Retail { get; set; } = null!;
        public string Cost { get; set; } = null!;
        public string ItmGroup { get; set; } = null!;
        public string UPC { get; set; } = null!;
    }
}
