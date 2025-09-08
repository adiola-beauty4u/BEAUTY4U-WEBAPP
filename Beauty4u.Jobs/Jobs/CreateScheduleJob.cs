using Beauty4u.Common.Enums;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Dto;
using Quartz;
using Quartz.Impl;
using static Quartz.Logging.OperationName;

namespace Beauty4u.Jobs.Jobs
{
    public class CreateScheduleJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IScheduledJobService _scheduledJobService;
        public CreateScheduleJob(IScheduledJobService scheduledJobService, ISchedulerFactory schedulerFactory)
        {
            _scheduledJobService = scheduledJobService;
            _schedulerFactory = schedulerFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var scheduledJobs = await _scheduledJobService.GetActiveJobsAsync();
            await scheduler.Start();

            foreach (ScheduledJobDto jobDto in scheduledJobs)
            {
                // 1. Resolve job type by class name
                var jobType = Type.GetType($"Beauty4u.Jobs.Jobs.{jobDto.Name}", throwOnError: false, ignoreCase: true);
                if (jobType == null || !typeof(IJob).IsAssignableFrom(jobType))
                    throw new InvalidOperationException($"Job class '{jobDto.Name}' not found or not an IJob.");

                var jobKey = new JobKey(jobDto.Name, "DynamicJobs");

                // 2. Define job
                var job = JobBuilder.Create(jobType)
                    .WithIdentity(jobKey)
                    .WithDescription(jobDto.Description)
                    .Build();

                // 3. Build trigger based on frequency
                ITrigger trigger;

                switch (jobDto.Frequency)
                {
                    case Frequency.Hourly:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Every2Hrs:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(2).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Every3Hrs:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(3).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Every4Hrs:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(4).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Every6Hrs:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(6).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Every12Hrs:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSimpleSchedule(x => x.WithIntervalInHours(12).RepeatForever())
                            .Build();
                        break;

                    case Frequency.Daily:
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"{jobDto.Name}-trigger")
                            .StartAt(DateBuilder.TodayAt(jobDto.StartHour, jobDto.Minute, 0))
                            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(jobDto.Hour, jobDto.Minute))
                            .Build();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // 4. Schedule job
                await scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
