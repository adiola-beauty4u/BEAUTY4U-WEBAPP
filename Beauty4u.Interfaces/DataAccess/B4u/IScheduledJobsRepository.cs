
using Beauty4u.Interfaces.Dto;

namespace Beauty4u.DataAccess.B4u
{
    public interface IScheduledJobsRepository
    {
        Task<List<IScheduledJobDto>> usp_GetActiveJobsAsync();
    }
}