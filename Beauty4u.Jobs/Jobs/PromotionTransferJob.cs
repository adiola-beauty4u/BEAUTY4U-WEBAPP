using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Api.Promotions;
using Beauty4u.Interfaces.DataAccess.Api;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Promotions;
using Beauty4u.Models.Api.Scheduler;
using Beauty4u.Models.Dto;
using Beauty4u.Models.Dto.Users;
using Newtonsoft.Json;
using Quartz;
using System.Text.Json;

namespace Beauty4u.Jobs.Jobs
{
    public class PromotionTransferJob : IJob
    {
        private readonly ILogger<PromotionTransferJob> _logger;
        private readonly IAuthService _authService;
        private readonly IScheduledJobService _scheduledJobService;
        private readonly IPromotionsApi _promotionsApi;
        private readonly string _hqApi;
        public PromotionTransferJob(IConfiguration configuration, ILogger<PromotionTransferJob> logger, IAuthService authService, IPromotionsApi promotionsApi, IScheduledJobService scheduledJobService)
        {
            _logger = logger;
            _authService = authService;
            _promotionsApi = promotionsApi;
            _scheduledJobService = scheduledJobService;
            _hqApi = configuration.GetValue<string>("HqSettings:HqApi")!;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            UserDto userDto = new UserDto();
            var jobs = await _scheduledJobService.GetActiveJobsAsync();

            var job = jobs.Where(x => x.Name == nameof(PromotionTransferJob)).FirstOrDefault();

            if (job != null)
            {

                ScheduledJobLogDto scheduledJobLogDto = new ScheduledJobLogDto();
                scheduledJobLogDto.ScheduledJobId = job.ScheduledJobId;
                scheduledJobLogDto.JobStart = DateTime.Now;
                try
                {
                    var schedulerToken = await _authService.CreateToken(userDto);
                    JobDataMap dataMap = context.MergedJobDataMap;
                    if (dataMap != null)
                    {
                        if (dataMap.ContainsKey("JobParameters"))
                        {
                            if (dataMap["JobParameters"] is JsonElement element)
                            {
                                var promoRequest = element.Deserialize<PromoTransferRequest>();
                                if (promoRequest != null)
                                {
                                    promoRequest.Schedule = null; // Remove schedule to execute immediately
                                    await _promotionsApi.TransferPromoToStoresAsync<PromoTransferResult>(
                                        _hqApi,
                                    schedulerToken.AccessToken,
                                        promoRequest
                                    );
                                }
                            }
                        }
                        else
                        {
                            await _promotionsApi.TransferAllPromosApiAsync<PromoTransferResult>(_hqApi, schedulerToken.AccessToken);
                        }
                    }

                    scheduledJobLogDto.JobEnd = DateTime.Now;
                    scheduledJobLogDto.IsSuccessful = true;
                    scheduledJobLogDto.Message = "Scheduled promo transfer to stores success.";

                    await _scheduledJobService.CreateScheduledJobLogAsync(scheduledJobLogDto);
                    _logger.LogInformation("Scheduled promo transfer done.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    scheduledJobLogDto.JobEnd = DateTime.Now;
                    scheduledJobLogDto.IsSuccessful = false;
                    scheduledJobLogDto.Message = ex.Message.Length > 500
                                                    ? ex.Message.Substring(0, 500)
                                                    : ex.Message;

                    await _scheduledJobService.CreateScheduledJobLogAsync(scheduledJobLogDto);
                }
            }
        }
    }
}
