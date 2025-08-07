namespace Beauty4u.Interfaces.Dto.Products
{
    public interface IProductFullWithPromotionCountsDto: IProductFullDto
    {
        int ActivePromotions { get; set; }
        int AllPromotions { get; set; }
    }
}