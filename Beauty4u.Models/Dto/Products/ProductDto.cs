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
        public string TaxType { get; set; } = null!;
        public bool Inventory { get; set; }
        public string Closed { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteUser { get; set; } = null!;
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; } = null!;

    }
}
