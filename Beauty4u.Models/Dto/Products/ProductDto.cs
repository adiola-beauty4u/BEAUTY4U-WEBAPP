using Beauty4u.Interfaces.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class ProductDto : IProductDto
    {
        public string Brand { get; set; } = string.Empty;
        public string StyleCode { get; set; } = string.Empty;
        public string StyleDesc { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal RetailPrice { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public string ItmGroup { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UPC { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;

    }
}
