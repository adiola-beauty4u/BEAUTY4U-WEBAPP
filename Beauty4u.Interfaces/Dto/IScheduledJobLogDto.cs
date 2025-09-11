namespace Beauty4u.Interfaces.Dto
{
    public interface IScheduledJobLogDto
    {
        bool IsSuccessful { get; set; }
        DateTime JobEnd { get; set; }
        DateTime JobStart { get; set; }
        string Message { get; set; }
        int ScheduledJobId { get; set; }
        int ScheduledJobLogId { get; set; }
        string JobName { get; set; }
        string Description { get; set; }
    }
}