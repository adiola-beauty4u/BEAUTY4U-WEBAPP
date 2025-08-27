using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Interfaces.Requests;
using System.Data;
using System.Reflection;

namespace Beauty4u.Common.Helpers
{
    public static class DataTableHelper
    {
        public static DataTable ToProductDataTable(IEnumerable<IBulkProductDataRequest> products)
        {
            var table = new DataTable();
            table.Columns.Add("Brand", typeof(string));
            table.Columns.Add("StyleCode", typeof(string));
            table.Columns.Add("StyleDesc", typeof(string));
            table.Columns.Add("Size", typeof(string));
            table.Columns.Add("Color", typeof(string));
            table.Columns.Add("Retail", typeof(decimal));
            table.Columns.Add("Cost", typeof(decimal));
            table.Columns.Add("ItmGroup", typeof(string));
            table.Columns.Add("UPC", typeof(string));

            foreach (var product in products)
            {
                table.Rows.Add(product.Brand,
                    product.StyleCode,
                    product.StyleDesc,
                    product.Size,
                    product.Color,
                    product.Retail,
                    product.Cost,
                    product.ItmGroup,
                    product.UPC);
            }

            return table;
        }

        public static DataTable ToProductDataTable(DataTable products)
        {
            var table = new DataTable();
            table.Columns.Add("Brand", typeof(string));
            table.Columns.Add("StyleCode", typeof(string));
            table.Columns.Add("StyleDesc", typeof(string));
            table.Columns.Add("Size", typeof(string));
            table.Columns.Add("Color", typeof(string));
            table.Columns.Add("Retail", typeof(decimal));
            table.Columns.Add("Cost", typeof(decimal));
            table.Columns.Add("ItmGroup", typeof(string));
            table.Columns.Add("UPC", typeof(string));

            foreach (DataRow product in products.Rows)
            {
                table.Rows.Add(product[0].ToString(),
                    product[1].ToString(),
                    product[2].ToString(),
                    product[3].ToString(),
                    product[4].ToString(),
                    product[5].ToString(),
                    product[6].ToString(),
                    product[7].ToString(),
                    product[8].ToString());
            }

            return table;
        }

        public static DataTable ToProductTransferDetailsDataTable(IEnumerable<ISearchProductResult> products)
        {
            var table = new DataTable();
            table.Columns.Add("VendorCode", typeof(string));
            table.Columns.Add("VendorName", typeof(string));
            table.Columns.Add("Brand", typeof(string));
            table.Columns.Add("StyleCode", typeof(string));
            table.Columns.Add("StyleDesc", typeof(string));
            table.Columns.Add("Size", typeof(string));
            table.Columns.Add("Color", typeof(string));
            table.Columns.Add("Retail", typeof(decimal));
            table.Columns.Add("Cost", typeof(decimal));
            table.Columns.Add("ItmGroup", typeof(string));
            table.Columns.Add("UPC", typeof(string));
            table.Columns.Add("Sku", typeof(string));
            table.Columns.Add("StoreCode", typeof(string));

            foreach (var product in products)
            {
                table.Rows.Add(product.VendorCode,
                    product.VendorName,
                    product.Brand,
                    product.StyleCode,
                    product.StyleDesc,
                    product.Size,
                    product.Color,
                    product.RetailPrice,
                    product.Cost,
                    product.ItmGroup,
                    product.UPC,
                    product.Sku,
                    product.Storecode);
            }

            return table;
        }

        public static List<T> DataTableToList<T>(DataTable table) where T : new()
        {
            var properties = typeof(T).GetProperties();
            var list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                var item = new T();

                foreach (var prop in properties)
                {
                    if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        // Get the underlying type if the property is nullable
                        Type propType = prop.PropertyType;
                        Type underlyingType = Nullable.GetUnderlyingType(propType) ?? propType;

                        // Handle nullable types
                        if (underlyingType == typeof(DateTime))
                        {
                            if (row[prop.Name] is DateTime dateValue)
                            {
                                prop.SetValue(item, (Nullable<DateTime>)dateValue);
                            }
                        }
                        else
                        {
                            // Handle other types normally
                            prop.SetValue(item, Convert.ChangeType(row[prop.Name], underlyingType));
                        }
                    }
                    else if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                    {
                        // Set nullable properties to null if the row value is DBNull
                        prop.SetValue(item, null);
                    }
                }

                list.Add(item);
            }
            return list;
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            // Get all properties of T
            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // Create DataTable columns
            foreach (var prop in props)
            {
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, propType);
            }

            // Add rows
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


        public static DataTable StringListParameter(List<string> stringList)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Value");
            foreach (var value in stringList)
            {
                var newRow = dataTable.NewRow();
                newRow["Value"]= value;
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }

        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();
                foreach (var prop in properties)
                {
                    if (table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    }
                }
                result.Add(obj);
            }

            return result;
        }

        public static List<string> ToStringList(this DataTable table, string columnName)
        {
            return table.AsEnumerable()
                .Where(row => row[columnName] != DBNull.Value)
                .Select(row => row[columnName].ToString()!)
                .ToList();
        }
    }
}
