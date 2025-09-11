namespace Beauty4u.Interfaces.Api.Scheduler
{
    public interface IQueuedJob
    {
        string JobGroup { get; set; }
        string JobName { get; set; }
        DateTime? NextFireTime { get; set; }
        DateTime? PreviousFireTime { get; set; }
        string TriggerGroup { get; set; }
        string TriggerName { get; set; }
    }
}