using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Models.Api.Table
{
    public class TableDataApi
    {
        public List<ColumnDataApi> Columns { get; set; } = new List<ColumnDataApi>();
        public List<RowDataApi> Rows { get; set; } = new List<RowDataApi>();
        public List<TableDataApi> TableGroups { get; set; } = new List<TableDataApi>();
        public string TableName { get; set; } = string.Empty;
    }
}
