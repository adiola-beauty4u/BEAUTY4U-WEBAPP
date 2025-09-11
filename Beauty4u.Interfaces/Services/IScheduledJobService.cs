using Beauty4u.Interfaces.Api.Scheduler;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Interfaces.Services
{
    public interface IScheduledJobService
    {
        Task CancelAllJobsAsync();
        Task CreateExecuteJobAsync(ICreateJobSchedule createJobSchedule);
        Task CreateForTodayAsync();
        Task CreateScheduledJobLogAsync(IScheduledJobLogDto scheduledJobLogDto);
        Task<List<IScheduledJobDto>> GetActiveJobsAsync();
        Task<List<IScheduledJobDto>> GetAllJobsAsync();
        Task<ITableData> GetQueuedJobsAsync();
        Task<ITableData> GetScheduledJobLogsAsync(IScheduledJobLogSearchParams scheduledJobLogSearchParams);
    }
}