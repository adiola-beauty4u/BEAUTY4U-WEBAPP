using Beauty4u.ApiAccess.Scheduler;
using Beauty4u.Common.Enums;
using Beauty4u.DataAccess.B4u;
using Beauty4u.Interfaces.Api.Scheduler;
using Beauty4u.Interfaces.Api.Table;
using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Scheduler;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Dto;
using Beauty4u.Models.Dto.Promotions;
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
        private readonly IJobSchedulerApi _jobSchedulerApi;
        private readonly ICurrentUserService _currentUserService;

        public ScheduledJobService(IScheduledJobsRepository scheduledJobsRepository, IJobSchedulerApi jobSchedulerApi, ICurrentUserService currentUserService)
        {
            _scheduledJobsRepository = scheduledJobsRepository;
            _jobSchedulerApi = jobSchedulerApi;
            _currentUserService = currentUserService;
        }

        public async Task<List<IScheduledJobDto>> GetActiveJobsAsync()
        {
            return await _scheduledJobsRepository.GetActiveJobsAsync();
        }
        public async Task CreateScheduledJobLogAsync(IScheduledJobLogDto scheduledJobLogDto)
        {
            await _scheduledJobsRepository.CreateScheduledJobLogAsync(scheduledJobLogDto);
        }
        public async Task<List<IScheduledJobDto>> GetAllJobsAsync()
        {
            return await _scheduledJobsRepository.GetAllJobsAsync();
        }
        public async Task CreateForTodayAsync()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                await _jobSchedulerApi.CreateForToday(currentUser.JwtToken!);
            }
        }
        public async Task CancelAllJobsAsync()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                await _jobSchedulerApi.CancelAllJobs(currentUser.JwtToken!);
            }
        }
        public async Task CreateExecuteJobAsync(ICreateJobSchedule createJobSchedule)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                await _jobSchedulerApi.CreateExecuteJob(currentUser.JwtToken!, createJobSchedule);
            }
        }
        public async Task<ITableData> GetQueuedJobsAsync()
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                var queuedJobs = await _jobSchedulerApi.GetQueuedJobs(currentUser.JwtToken!);

                var tableData = new TableData();
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Job Name",
                    FieldName = nameof(QueuedJob.JobName),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Job Group",
                    FieldName = nameof(QueuedJob.JobGroup),
                    DataType = ColumnDataType.String,
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Trigger Name",
                    FieldName = nameof(QueuedJob.TriggerName),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Trigger Group",
                    FieldName = nameof(QueuedJob.TriggerGroup),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Previous Fire Time",
                    FieldName = nameof(QueuedJob.PreviousFireTime),
                    DataType = ColumnDataType.Date
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Next Fire Time",
                    FieldName = nameof(QueuedJob.NextFireTime),
                    DataType = ColumnDataType.Date
                });

                foreach (var queuedJob in queuedJobs)
                {
                    var rowData = new RowData()
                    {
                        Cells = new Dictionary<string, ICellData>(),
                    };

                    rowData.Cells.Add(nameof(QueuedJob.JobName), new CellData()
                    {
                        RawValue = queuedJob.JobName,
                        TextValue = queuedJob.JobName,
                    });

                    rowData.Cells.Add(nameof(QueuedJob.JobGroup), new CellData()
                    {
                        RawValue = queuedJob.JobGroup,
                        TextValue = queuedJob.JobGroup,
                    });

                    rowData.Cells.Add(nameof(QueuedJob.TriggerName), new CellData()
                    {
                        RawValue = queuedJob.TriggerName,
                        TextValue = queuedJob.TriggerName,
                    });

                    rowData.Cells.Add(nameof(QueuedJob.TriggerGroup), new CellData()
                    {
                        RawValue = queuedJob.TriggerGroup,
                        TextValue = queuedJob.TriggerGroup,
                    });

                    rowData.Cells.Add(nameof(QueuedJob.NextFireTime), new CellData()
                    {
                        RawValue = queuedJob.NextFireTime,
                        TextValue = queuedJob.NextFireTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                    });
                    rowData.Cells.Add(nameof(QueuedJob.PreviousFireTime), new CellData()
                    {
                        RawValue = queuedJob.PreviousFireTime,
                        TextValue = queuedJob.PreviousFireTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                    });
                    tableData.Rows.Add(rowData);
                }
                return tableData;
            }

            return new TableData();
        }

        public async Task<ITableData> GetScheduledJobLogsAsync(IScheduledJobLogSearchParams scheduledJobLogSearchParams)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                var scheduledJobLogs = await _scheduledJobsRepository.SearchScheduledJobLogsAsync(scheduledJobLogSearchParams);
                var tableData = new TableData();
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Job Name",
                    FieldName = nameof(ScheduledJobLogDto.JobName),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Description",
                    FieldName = nameof(ScheduledJobLogDto.Description),
                    DataType = ColumnDataType.String,
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Message",
                    FieldName = nameof(ScheduledJobLogDto.Message),
                    DataType = ColumnDataType.String
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Successful",
                    FieldName = nameof(ScheduledJobLogDto.IsSuccessful),
                    DataType = ColumnDataType.Bool
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Job Start",
                    FieldName = nameof(ScheduledJobLogDto.JobStart),
                    DataType = ColumnDataType.Date
                });
                tableData.Columns.Add(new ColumnData()
                {
                    Header = "Job End",
                    FieldName = nameof(ScheduledJobLogDto.JobEnd),
                    DataType = ColumnDataType.Date
                });

                foreach (var jobLog in scheduledJobLogs)
                {
                    var rowData = new RowData()
                    {
                        Cells = new Dictionary<string, ICellData>(),
                    };

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.JobName), new CellData()
                    {
                        RawValue = jobLog.JobName,
                        TextValue = jobLog.JobName,
                    });

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.Description), new CellData()
                    {
                        RawValue = jobLog.Description,
                        TextValue = jobLog.Description,
                    });

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.IsSuccessful), new CellData()
                    {
                        RawValue = jobLog.IsSuccessful,
                        TextValue = jobLog.IsSuccessful.ToString(),
                    });

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.Message), new CellData()
                    {
                        RawValue = jobLog.Message,
                        TextValue = jobLog.Message,
                    });

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.JobStart), new CellData()
                    {
                        RawValue = jobLog.JobStart,
                        TextValue = jobLog.JobStart.ToString("yyyy-MM-dd HH:mm:ss"),
                    });

                    rowData.Cells.Add(nameof(ScheduledJobLogDto.JobEnd), new CellData()
                    {
                        RawValue = jobLog.JobEnd,
                        TextValue = jobLog.JobEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                    });
                    tableData.Rows.Add(rowData);
                }
                return tableData;
            }
            return new TableData();
        }
    }
}
