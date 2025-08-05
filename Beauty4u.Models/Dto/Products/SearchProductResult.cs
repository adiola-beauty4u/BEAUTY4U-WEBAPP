using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Models.Dto.Products
{
    public class SearchProductResult : ProductDto, ISearchProductResult, IProductDto
    {
        public string VendorCode { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string Storecode { get; set; } = null!;
    }
}
