using Beauty4u.Interfaces.Dto.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Promotions
{
    public class PromoSearchParams : IPromoSearchParams
    {
        public string PromoNo { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StoreCode { get; set; } = string.Empty;
        public string PromoType { get; set; } = string.Empty;
        public string PromoDescription { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
