using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Interfaces.Services
{
    public interface IScheduledJobService
    {
        Task<List<IScheduledJobDto>> GetActiveJobsAsync();
    }
}