using Beauty4u.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Promotions
{
    public class PromoTransferResult
    {
        public string StoreCode { get { return string.Join(", ", StoreCodes.Select(x => x.Name)); } }
        public List<NameValue<string>> StoreCodes { get; set; } = new List<NameValue<string>>();
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
