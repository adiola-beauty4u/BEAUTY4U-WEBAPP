
using Beauty4u.Interfaces.Api.Scheduler;
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.DataAccess.B4u
{
    public interface IScheduledJobsRepository
    {
        Task CreateScheduledJobLogAsync(IScheduledJobLogDto scheduledJobLogDto);
        Task<List<IScheduledJobDto>> GetActiveJobsAsync();
        Task<List<IScheduledJobDto>> GetAllJobsAsync();
        Task<List<IScheduledJobLogDto>> SearchScheduledJobLogsAsync(IScheduledJobLogSearchParams scheduledJobLogSearchParams);
    }
}