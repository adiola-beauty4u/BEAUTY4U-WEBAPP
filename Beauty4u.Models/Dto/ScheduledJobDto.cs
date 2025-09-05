using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Models.Dto
{
    public class ScheduledJobDto : IScheduledJobDto
    {
        public int ScheduledJobId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StartHour { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public Frequency Frequency { get; set; }
    }
}
