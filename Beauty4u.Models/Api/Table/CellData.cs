using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Models.Api.Table
{
    public class CellData : ICellData
    {
        public string? TextValue { get; set; } = string.Empty;
        public object? RawValue { get; set; }
        public bool IsValid { get; set; } = true;
        public string Tooltip { get; set; } = null!;
        public string CssClass { get; set; } = null!;
        public string CssIcon { get; set; } = null!;
    }
}
