using Asp.Versioning;
using AutoMapper;
using Beauty4u.Models.Common;
using Beauty4u.Models.Api;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DataCheckController : ControllerBase
    {
        private readonly IFileReadHelper _fileReadHelper;
        public DataCheckController(IFileReadHelper fileReadHelper)
        {
            _fileReadHelper = fileReadHelper;
        }

        [HttpPost("read-csv")]
        public async Task<ActionResult> ReadCsv([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                DetectDelimiter = true
            });

            var records = new List<Dictionary<string, string>>();

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord!;

            while (csv.Read())
            {
                var row = new Dictionary<string, string>();
                foreach (var header in headers)
                {
                    row[header] = csv.GetField(header) ?? string.Empty;
                }
                records.Add(row);
            }

            return Ok(records);
        }
    }
}
