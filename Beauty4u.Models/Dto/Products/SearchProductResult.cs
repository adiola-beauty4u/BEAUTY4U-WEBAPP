using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Models.Dto.Products
{
    public class SearchProductResult : ProductDto, ISearchProductResult, IProductDto
    {
        public string Storecode { get; set; } = string.Empty;
    }
}
