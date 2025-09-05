using Quartz;

namespace Beauty4u.Jobs.Jobs
{
    public class PromotionTransferJob : IJob
    {
        private readonly ILogger<PromotionTransferJob> _logger;
        public PromotionTransferJob(ILogger<PromotionTransferJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {

            throw new NotImplementedException();
        }
    }
}
