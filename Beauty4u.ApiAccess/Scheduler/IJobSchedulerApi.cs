

using Beauty4u.Interfaces.Api.Scheduler;

namespace Beauty4u.ApiAccess.Scheduler
{
    public interface IJobSchedulerApi
    {
        Task CancelAllJobs(string jwtToken);
        Task CreateExecuteJob(string jwtToken, ICreateJobSchedule createJobSchedule);
        Task CreateForToday(string jwtToken);
        Task<List<IQueuedJob>> GetQueuedJobs(string jwtToken);
    }
}