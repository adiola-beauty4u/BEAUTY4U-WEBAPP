using System.Data;

namespace Beauty4u.Models.Common
{
    public interface IFileReadHelper
    {
        Task<List<T>> ReadCsvAsync<T>(Stream csvStream);
        Task<DataTable> ReadCsvToDataTableAsync(Stream csvStream);
    }
}