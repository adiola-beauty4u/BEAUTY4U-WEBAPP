using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Interfaces.Api.Products
{
    public interface IProductSearchResult
    {
        List<ISearchProductResult> Products { get; set; }
        ITableData TableData { get; set; }
    }
}