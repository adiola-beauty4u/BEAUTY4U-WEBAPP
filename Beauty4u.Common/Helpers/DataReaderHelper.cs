using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Common.Helpers
{
    public static class DataReaderHelper
    {
        public static List<T> MapReaderToList<T>(SqlDataReader reader) where T : new()
        {
            var results = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columnNames = Enumerable.Range(0, reader.FieldCount)
                                         .Select(reader.GetName)
                                         .ToHashSet(StringComparer.OrdinalIgnoreCase);

            while (reader.Read())
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    if (columnNames.Contains(prop.Name) && !reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        var value = reader.GetValue(reader.GetOrdinal(prop.Name));
                        prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                    }
                }
                results.Add(obj);
            }

            return results;
        }
    }
}
