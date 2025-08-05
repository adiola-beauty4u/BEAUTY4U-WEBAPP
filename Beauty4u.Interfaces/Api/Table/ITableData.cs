namespace Beauty4u.Interfaces.Api.Table
{
    public interface ITableData
    {
        List<IColumnData> Columns { get; set; }
        List<IRowData> Rows { get; set; }
        List<ITableData> TableGroups { get; set; }
        string TableName { get; set; }
    }
}