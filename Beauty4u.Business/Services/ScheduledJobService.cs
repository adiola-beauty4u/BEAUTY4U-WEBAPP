using Beauty4u.DataAccess.B4u;
using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public class ScheduledJobService : IScheduledJobService
    {
        private readonly IScheduledJobsRepository _scheduledJobsRepository;
        public ScheduledJobService(IScheduledJobsRepository scheduledJobsRepository)
        {
            _scheduledJobsRepository = scheduledJobsRepository;
        }

        public async Task<List<IScheduledJobDto>> GetActiveJobsAsync()
        {
            return await _scheduledJobsRepository.usp_GetActiveJobsAsync();
        }
    }
}
