using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Models.Dto
{
    public class ScheduledJobLogDto : IScheduledJobLogDto
    {
        public int ScheduledJobLogId { get; set; }
        public int ScheduledJobId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public DateTime JobStart { get; set; }
        public DateTime JobEnd { get; set; }
    }
}
