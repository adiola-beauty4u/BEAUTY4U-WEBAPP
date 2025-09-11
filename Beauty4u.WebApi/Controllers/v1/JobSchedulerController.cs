using Asp.Versioning;
using AutoMapper;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Scheduler;
using Beauty4u.Models.Api.Table;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class JobSchedulerController : ControllerBase
    {
        private readonly IScheduledJobService _scheduledJobService;
        private readonly ILogger<JobSchedulerController> _logger;
        private readonly IMapper _mapper;
        public JobSchedulerController(IScheduledJobService scheduledJobService, ILogger<JobSchedulerController> logger, IMapper mapper)
        {
            _scheduledJobService = scheduledJobService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("create-for-today")]
        public async Task<IActionResult> CreateForToday()
        {
            await _scheduledJobService.CreateForTodayAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetQueuedJobs()
        {
            var output = await _scheduledJobService.GetQueuedJobsAsync();
            return Ok(_mapper.Map<TableDataApi>(output));
        }

        [HttpDelete("all")]
        public async Task<IActionResult> CancelAllJobs()
        {
            await _scheduledJobService.CancelAllJobsAsync();
            return Ok();
        }

        [HttpPost("create-execute-job")]
        public async Task<IActionResult> CreateExecuteJob(CreateJobSchedule createJobSchedule)
        {
            await _scheduledJobService.CreateExecuteJobAsync(createJobSchedule);
            return Ok();
        }

        [HttpGet("get-active-jobs")]
        public async Task<IActionResult> GetScheduledJobs()
        {
            var output = await _scheduledJobService.GetActiveJobsAsync();

            return Ok(output);
        }
        [HttpPost("search-scheduled-job-logs")]
        public async Task<IActionResult> SearchScheduledJobLogs(ScheduledJobLogSearchParams scheduledJobLogSearchParams)
        {
            var output = await _scheduledJobService.GetScheduledJobLogsAsync(scheduledJobLogSearchParams);

            return Ok(_mapper.Map<TableDataApi>(output));
        }
    }
}
