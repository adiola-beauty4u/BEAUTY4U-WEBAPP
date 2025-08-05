using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Products;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto;

namespace Beauty4u.Models.Api.Products
{
    public class ProductSearchResult : IProductSearchResult
    {
        public ITableData TableData { get; set; } = new TableData();
        public List<ISearchProductResult> Products { get; set; } = new List<ISearchProductResult>();
    }
}
