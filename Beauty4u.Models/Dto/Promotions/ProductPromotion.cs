using Beauty4u.Interfaces.Dto.Promotions;

namespace Beauty4u.Models.Dto.Promotions
{
    public class ProductPromotion : IProductPromotion
    {
        public string PromoNo { get; set; } = string.Empty;
        public string PromoName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string PromoType { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string IsActive { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal NewPrice { get; set; }
    }
}
