using Asp.Versioning;
using Azure.Core;
using Beauty4u.Jobs.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;

namespace Beauty4u.Jobs.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class JobSchedulerController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public JobSchedulerController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        [HttpGet("create-for-today")]
        public async Task<IActionResult> CreateForToday()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            // 1️⃣ Clear all scheduled jobs & triggers first
            await scheduler.Clear();

            // 2️⃣ Define job
            var jobKey = new JobKey("CreateScheduleJob", "ApiJobs");
            var job = JobBuilder.Create<CreateScheduleJob>()
                .WithIdentity(jobKey)
                .WithDescription("Runs the CreateScheduleJob immediately")
                .Build();

            // 3️⃣ Define trigger that starts immediately (no repeat)
            var trigger = TriggerBuilder.Create()
                .WithIdentity("CreateScheduleJob-trigger", "ApiJobs")
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x => x.WithRepeatCount(0)) // run once
                .Build();

            // 4️⃣ Schedule job
            await scheduler.ScheduleJob(job, trigger);

            return Ok(new { message = "✅ CreateScheduleJob cleared old jobs and started immediately." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobGroups = await scheduler.GetJobGroupNames();
            var jobs = new List<object>();

            foreach (var group in jobGroups)
            {
                var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

                foreach (var jobKey in jobKeys)
                {
                    var triggers = await scheduler.GetTriggersOfJob(jobKey);

                    foreach (var trigger in triggers)
                    {
                        jobs.Add(new
                        {
                            JobName = jobKey.Name,
                            JobGroup = jobKey.Group,
                            TriggerName = trigger.Key.Name,
                            TriggerGroup = trigger.Key.Group,
                            NextFireTime = trigger.GetNextFireTimeUtc()?.ToLocalTime(),
                            PreviousFireTime = trigger.GetPreviousFireTimeUtc()?.ToLocalTime()
                        });
                    }
                }
            }

            return Ok(jobs);
        }

        [HttpDelete("all")]
        public async Task<IActionResult> CancelAllJobs()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Clear();  // removes everything
            return Ok(new { message = "All scheduled jobs have been cancelled." });
        }
    }
}
