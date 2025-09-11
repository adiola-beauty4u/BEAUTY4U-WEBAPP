using Beauty4u.Interfaces.Api.Scheduler;

namespace Beauty4u.Models.Api.Scheduler
{
    public class QueuedJob : IQueuedJob
    {
        public string JobName { get; set; } = string.Empty;
        public string JobGroup { get; set; } = string.Empty;
        public string TriggerName { get; set; } = string.Empty;
        public string TriggerGroup { get; set; } = string.Empty;
        public DateTime? NextFireTime { get; set; }
        public DateTime? PreviousFireTime { get; set; }
    }
}
