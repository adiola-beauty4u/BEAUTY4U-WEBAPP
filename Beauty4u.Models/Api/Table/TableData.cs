using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Models.Api.Table
{
    public class TableData : ITableData
    {
        public List<IColumnData> Columns { get; set; } = new List<IColumnData>();
        public List<IRowData> Rows { get; set; } = new List<IRowData>();
        public List<ITableData> TableGroups { get; set; } = new List<ITableData>();
        public string TableName { get; set; } = string.Empty;
    }
}
