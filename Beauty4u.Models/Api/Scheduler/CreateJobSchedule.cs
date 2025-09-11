using Beauty4u.Interfaces.Api.Scheduler;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Beauty4u.Models.Api.Scheduler
{
    public class CreateJobSchedule : ICreateJobSchedule
    {
        public string ScheduledJob { get; set; } = string.Empty;
        public DateTime? Schedule { get; set; }
        public JsonElement? JobParameters { get; set; }
    }
}
