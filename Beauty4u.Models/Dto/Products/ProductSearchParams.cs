using Beauty4u.Interfaces.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class ProductSearchParams : IProductSearchParams
    {
        public string? Category { get; set; }
        public string? VendorCode { get; set; }
        public string? StyleCode { get; set; }
        public string? StyleDesc { get; set; }
        public string? Brand { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? UPC { get; set; }
        public string? Sku { get; set; }
        public string? RetailPrice { get; set; }
    }
}
