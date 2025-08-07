using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Models.Dto.Products
{
    public class ProductFullWithPromotionCountsDto : ProductFullDto, IProductFullWithPromotionCountsDto, IProductFullDto
    {
        public int AllPromotions { get; set; }
        public int ActivePromotions { get; set; }
    }
}
