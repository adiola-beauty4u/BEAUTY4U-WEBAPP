using System.Data;
using Beauty4u.Interfaces.Api.Table;

namespace Beauty4u.Interfaces.Services
{
    public interface IDataValidationService
    {
        void ValidateObjectList<T>(ITableData tableData, List<T> dataList, string rowKeyField);
        void ValidateTable(ITableData tableData, DataTable data, string rowKeyField);
    }
}