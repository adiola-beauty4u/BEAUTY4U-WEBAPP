using Beauty4u.Interfaces.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class UPCValidateResult : IUPCValidateResult
    {
        public string UPC { get; set; } = string.Empty;
        public bool ExistsInB4u { get; set; }
        public bool ExistsInMIS { get; set; }
    }
}
