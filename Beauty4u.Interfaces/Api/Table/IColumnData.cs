namespace Beauty4u.Interfaces.Api.Table
{
    public interface IColumnData
    {
        //ColumnDataType DataType { get; set; }
        string FieldName { get; set; }
        string Header { get; set; }
        bool IsSlideInColumn { get; set; }
        string SlideInCommand { get; set; }
        string SlideInTitle { get; set; }
        bool IsHidden { get; set; }
    }
}