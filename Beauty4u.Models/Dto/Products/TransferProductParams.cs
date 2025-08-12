using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Dto.Products;

namespace Beauty4u.Models.Dto.Products
{
    public class TransferProductParams : ITransferProductParams
    {
        public string UserCode { get; set; } = string.Empty;
        public DataTable ProductTransferDetails { get; set; } = new DataTable();
    }
}
