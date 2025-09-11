using Asp.Versioning;
using Beauty4u.Jobs.Jobs;
using Beauty4u.Models.Api.Scheduler;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;

namespace Beauty4u.Jobs.Controllers.v1
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

            var jobGroups = await scheduler.GetJobGroupNames();
            var jobs = new List<object>();

            foreach (var group in jobGroups)
            {
                var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

                foreach (var jk in jobKeys)
                {
                    var triggers = await scheduler.GetTriggersOfJob(jk);

                    foreach (var trgr in triggers)
                    {
                        jobs.Add(new
                        {
                            JobName = jk.Name,
                            JobGroup = jk.Group,
                            TriggerName = trgr.Key.Name,
                            TriggerGroup = trgr.Key.Group,
                            NextFireTime = trgr.GetNextFireTimeUtc()?.ToLocalTime(),
                            PreviousFireTime = trgr.GetPreviousFireTimeUtc()?.ToLocalTime()
                        });
                    }
                }
            }

            return Ok(new { message = "✅ CreateScheduleJob cleared old jobs and started immediately.", jobs = jobs });
        }

        [HttpGet("queued-jobs")]
        public async Task<IActionResult> GetQueuedJobs()
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
        public async Task<IActionResult> CancelAllQueuedJobs()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Clear();  // removes everything
            return Ok(new { message = "All scheduled jobs have been cancelled." });
        }

        [HttpPost("create-execute-job")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobSchedule createJobSchedule)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();
                var jobKey = new JobKey($"Create{createJobSchedule.ScheduledJob}", "ApiJobs");
                var jobType = Type.GetType($"Beauty4u.Jobs.Jobs.{createJobSchedule.ScheduledJob}", throwOnError: false, ignoreCase: true);

                if (jobType == null || !typeof(IJob).IsAssignableFrom(jobType))
                    return BadRequest(new { error = $"Job class '{createJobSchedule.ScheduledJob}' not found or not an IJob." });

                var jobDataMap = new Dictionary<string, object>();
                jobDataMap.Add("JobParameters", createJobSchedule.JobParameters);
                var job = JobBuilder.Create(jobType)
                    .WithIdentity(jobKey)
                    .SetJobData(new JobDataMap((IDictionary<string, object>)jobDataMap))
                    .WithDescription($"Runs the {createJobSchedule.ScheduledJob}")
                    .Build();

                var schedule = createJobSchedule.Schedule.HasValue
                    ? createJobSchedule.Schedule.Value.ToLocalTime()
                    : DateTime.Now.AddMinutes(1);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{createJobSchedule.ScheduledJob}-trigger", "ApiJobs")
                    .ForJob(job)
                    .StartAt(schedule) // ✅ safer than DateBuilder.DateOf
                    .WithSimpleSchedule(x => x.WithRepeatCount(0))
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                return Ok(new { message = $"✅ {createJobSchedule.ScheduledJob} created and scheduled at {schedule}." });
            }
            catch (Exception ex)
            {
                // 🔑 log the error and return a useful message
                Console.WriteLine($"❌ CreateJob failed: {ex.Message}\n{ex.StackTrace}");

                return StatusCode(500, new
                {
                    error = "Job scheduling failed",
                    details = ex.Message
                });
            }
        }


    }
}
