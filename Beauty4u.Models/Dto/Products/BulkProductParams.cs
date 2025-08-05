using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Products
{
    public class BulkProductParams : BulkProduct, IBulkProductParams
    {
        public BulkProductParams()
        {
            BulkProducts = new DataTable();
        }
        public DataTable BulkProducts { get; set; } = new DataTable();
    }
}
