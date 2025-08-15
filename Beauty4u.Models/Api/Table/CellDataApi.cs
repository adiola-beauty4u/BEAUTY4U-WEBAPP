using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Table
{
    public class CellDataApi
    {
        public string? TextValue { get; set; } = string.Empty;
        public object? RawValue { get; set; }
        public object? CommandParameter { get; set; }
        public bool IsValid { get; set; } = true;
        public string Tooltip { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;
        public string CssIcon { get; set; } = string.Empty;
    }
}
