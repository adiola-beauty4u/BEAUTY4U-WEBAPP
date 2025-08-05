using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Api.Table;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.VisualBasic.FileIO;

namespace Beauty4u.Business.Services
{
    public class DataValidationService : IDataValidationService
    {
        private readonly IStoreService _storeService;
        private readonly IConnectionRepository _connectionRepository;
        public DataValidationService(IStoreService storeService, IConnectionRepository connectionRepository)
        {
            _storeService = storeService;
            _connectionRepository = connectionRepository;
        }
        public void ValidateTable(ITableData tableData, DataTable data, string rowKeyField)
        {
            tableData.Rows = new List<IRowData>();

            var duplicates = data.AsEnumerable()
                                .GroupBy(row => row[rowKeyField]?.ToString())
                                .Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key))
                                .Select(g => g.Key!)
                                .ToList();

            foreach (DataRow row in data.Rows)
            {
                RowData rowData = new RowData();
                rowData.Cells = new Dictionary<string, ICellData>();
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    if (((ColumnData)tableData.Columns[i]).FieldName == rowKeyField)
                    {
                        rowData.RowKey = row[i].ToString()!;
                        if (duplicates.Contains(rowData.RowKey))
                        {
                            rowData.IsValid = false;
                            rowData.CssClass = "row-invalid";
                            rowData.Tooltip += $"Duplicate {rowKeyField} in file.\n";
                        }

                    }
                    CellData cellData = new CellData();
                    cellData.TextValue = row[i].ToString();
                    cellData.RawValue = row[i];
                    cellData.IsValid = IsValidValue(row[i], ((ColumnData)tableData.Columns[i]).DataType);
                    if (!cellData.IsValid)
                    {
                        rowData.IsValid = false;
                        rowData.CssClass = "row-invalid";
                        rowData.Tooltip += $"Invalid value on {tableData.Columns[i].Header}.\n";
                        cellData.Tooltip = $"Invalid value";
                        cellData.CssClass = "cell-invalid";
                    }

                    rowData.Cells.Add(((ColumnData)tableData.Columns[i]).FieldName, cellData);
                }
                tableData.Rows.Add(rowData);
            }
        }
        public void ValidateObjectList<T>(ITableData tableData, List<T> dataList, string rowKeyField)
        {
            tableData.Columns.Add(new ColumnData() { FieldName = "Result", Header = "Result" });
            tableData.Rows = new List<IRowData>();

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Detect duplicates based on key
            var duplicates = dataList
                .GroupBy(x => propertyInfos.First(p => p.Name == rowKeyField).GetValue(x)?.ToString())
                .Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key))
                .Select(g => g.Key!)
                .ToList();

            foreach (var item in dataList)
            {
                var rowData = new RowData
                {
                    Cells = new Dictionary<string, ICellData>(),
                    IsValid = true
                };

                var resultCellData = new CellData();

                foreach (var column in tableData.Columns.OfType<ColumnData>().Where(c => c.FieldName != "Result"))
                {
                    var prop = propertyInfos.FirstOrDefault(p => p.Name == column.FieldName);
                    if (prop == null) continue;

                    var value = prop.GetValue(item);
                    var textValue = value?.ToString();

                    if (column.FieldName == rowKeyField)
                    {
                        rowData.RowKey = textValue ?? string.Empty;

                        if (duplicates.Contains(rowData.RowKey))
                        {
                            rowData.IsValid = false;
                            resultCellData.TextValue += $"Duplicate {rowKeyField} in file.\n";
                        }
                    }

                    var cellData = new CellData
                    {
                        TextValue = textValue,
                        RawValue = value,
                        IsValid = IsValidValue(value, column.DataType)
                    };

                    if (!cellData.IsValid)
                    {
                        rowData.IsValid = false;
                        resultCellData.TextValue += $"Invalid value on {column.Header}.\n";
                    }

                    rowData.Cells[column.FieldName] = cellData;
                }

                rowData.Cells["Result"] = resultCellData;
                tableData.Rows.Add(rowData);
            }
        }
        public static bool IsValidValue(object value, ColumnDataType fieldType)
        {
            if (value == null && (fieldType != ColumnDataType.String || fieldType != ColumnDataType.Group)) return false;

            string strValue = value.ToString()?.Trim();

            switch (fieldType)
            {
                case ColumnDataType.String:
                    return true;
                case ColumnDataType.Int:
                    return BigInteger.TryParse(strValue, out _);
                case ColumnDataType.Money:
                    return decimal.TryParse(strValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out _);
                case ColumnDataType.Decimal:
                    return decimal.TryParse(strValue, NumberStyles.Number, CultureInfo.InvariantCulture, out _);
                case ColumnDataType.Group:
                    return true;
                case ColumnDataType.Bool:
                    return bool.TryParse(strValue, out _);
                default:
                    return false;
            }
        }
    }
}
