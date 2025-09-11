namespace Beauty4u.Interfaces.Api.Scheduler
{
    public interface IScheduledJobLogSearchParams
    {
        bool? IsSuccessful { get; set; }
        DateTime? JobEnd { get; set; }
        DateTime? JobStart { get; set; }
        int ScheduledJobId { get; set; }
    }
}