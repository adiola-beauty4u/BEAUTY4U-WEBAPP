using System.Data;

namespace Beauty4u.Models.Dto.Products
{
    public interface IBulkProductParams : IBulkProduct
    {
        DataTable BulkProducts { get; set; }
    }
}