using Beauty4u.Models.Dto.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Products
{
    public class BulkProductRequest : BulkProduct
    {
        public IFormFile ProductFile { get; set; } = null!;
    }
}
