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
        public string Brand { get; set; } = null!;
        public string StyleCode { get; set; } = null!;
        public string StyleDesc { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal RetailPrice { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public string ItmGroup { get; set; } = null!;
        public string UPC { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string VendorCode { get; set; } = null!;
        public string VendorName { get; set; } = null!;

    }
}
