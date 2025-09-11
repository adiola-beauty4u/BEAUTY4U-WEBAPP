using Beauty4u.Interfaces.Api.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Api.Scheduler
{
    public class ScheduledJobLogSearchParams : IScheduledJobLogSearchParams
    {
        public int ScheduledJobId { get; set; }
        public bool? IsSuccessful { get; set; }
        public DateTime? JobStart { get; set; }
        public DateTime? JobEnd { get; set; }
    }
}
