using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Models.Api.Table
{
    public class ColumnData : IColumnData
    {
        public string FieldName { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public ColumnDataType DataType { get; set; } = ColumnDataType.String;
    }
}
