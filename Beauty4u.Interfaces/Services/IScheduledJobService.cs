using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Interfaces.Services
{
    public interface IScheduledJobService
    {
        Task CreateScheduledJobLogAsync(IScheduledJobLogDto scheduledJobLogDto);
        Task<List<IScheduledJobDto>> GetActiveJobsAsync();
        Task<List<IScheduledJobDto>> GetAllJobsAsync();
    }
}