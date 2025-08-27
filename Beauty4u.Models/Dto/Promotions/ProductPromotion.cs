using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Dto.Promotions;
using Beauty4u.Models.Dto.Products;

namespace Beauty4u.Models.Dto.Promotions
{
    public class ProductPromotion : ProductDto, IProductPromotion, IProductDto
    {
        public string PromoNo { get; set; } = string.Empty;
        public string PromoName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PromoType { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string IsActive { get; set; } = string.Empty;
        public decimal NewPrice { get; set; }
        public decimal CurrentRetailPrice { get; set; }
        public int? PromoRuleId { get; set; }
    }
}
