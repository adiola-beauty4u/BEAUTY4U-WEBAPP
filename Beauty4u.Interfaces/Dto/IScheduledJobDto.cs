namespace Beauty4u.Interfaces.Dto
{
    public interface IScheduledJobDto
    {
        string Description { get; set; }
        int Hour { get; set; }
        int Minute { get; set; }
        string Name { get; set; }
        int ScheduledJobId { get; set; }
        int StartHour { get; set; }
    }
}