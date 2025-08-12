using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class ProductFullDto : ProductDto, IProductFullDto, IProductDto
    {
        public string TaxType { get; set; } = string.Empty;
        public bool Inventory { get; set; }
        public string Closed { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime? WriteDate { get; set; }
        public string WriteUser { get; set; } = string.Empty;
        public DateTime? LastUpdate { get; set; }
        public string LastUser { get; set; } = string.Empty;
    }
}
