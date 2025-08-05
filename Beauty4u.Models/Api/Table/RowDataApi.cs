using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Models.Api.Table
{
    public class RowDataApi
    {
        public Dictionary<string, CellDataApi> Cells { get; set; } = new Dictionary<string, CellDataApi>();
        public bool IsValid { get; set; } = true;
        public string RowKey { get; set; } = null!;
        public string Tooltip { get; set; } = null!;
        public string CssClass { get; set; } = null!;
        public bool IsNew { get; set; }
        public bool IsChanged { get; set; }
    }
}
