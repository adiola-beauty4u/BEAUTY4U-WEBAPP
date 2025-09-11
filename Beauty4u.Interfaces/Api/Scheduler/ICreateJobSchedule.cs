using System.Text.Json;

namespace Beauty4u.Interfaces.Api.Scheduler
{
    public interface ICreateJobSchedule
    {
        string ScheduledJob { get; set; }
        DateTime? Schedule { get; set; }
        JsonElement? JobParameters { get; set; }
    }
}