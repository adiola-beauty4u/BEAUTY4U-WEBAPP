using Beauty4u.Models.Common;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Common.Helpers
{
    public class FileReadHelper : IFileReadHelper
    {
        private readonly ILogger<FileReadHelper> _logger;

        public FileReadHelper(ILogger<FileReadHelper> logger)
        {
            _logger = logger;
        }

        public async Task<List<T>> ReadCsvAsync<T>(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                records.Add(record);
            }

            return records;
        }

        public async Task<DataTable> ReadCsvToDataTableAsync(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectColumnCountChanges = true,
                IgnoreBlankLines = true
            });

            await csv.ReadAsync();
            csv.ReadHeader();
            var headerRow = csv.HeaderRecord;

            var dataTable = new DataTable();

            foreach (var header in headerRow)
            {
                dataTable.Columns.Add(header);
            }

            while (await csv.ReadAsync())
            {
                var row = dataTable.NewRow();
                foreach (var header in headerRow)
                {
                    row[header] = csv.GetField(header);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
