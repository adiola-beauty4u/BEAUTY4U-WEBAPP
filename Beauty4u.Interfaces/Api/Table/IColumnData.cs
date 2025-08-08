namespace Beauty4u.Interfaces.Api.Table
{
    public interface IColumnData
    {
        //ColumnDataType DataType { get; set; }
        string FieldName { get; set; }
        string Header { get; set; }
        bool IsCommand { get; set; }
        string CommandName { get; set; }
    }
}