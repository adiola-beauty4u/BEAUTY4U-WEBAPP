using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class BulkProductResultDto : BulkProductDataRequest, IBulkProductResultDto
    {
        public string Sku { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
    }
}
