using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Products
{
    public class ProductTransferRequest : IProductTransferRequest
    {
        public List<string> StoreCodes { get; set; } = new List<string>();
        public List<string> UPCList { get; set; } = new List<string>();
    }
}
