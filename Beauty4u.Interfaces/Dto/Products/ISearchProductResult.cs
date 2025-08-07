namespace Beauty4u.Interfaces.Dto.Products
{
    public interface ISearchProductResult : IProductDto
    {
        string Storecode { get; set; }
    }
}