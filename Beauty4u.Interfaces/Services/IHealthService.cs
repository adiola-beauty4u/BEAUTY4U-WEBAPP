using Beauty4u.Models.Common;

namespace Beauty4u.Interfaces.Services
{
    public interface IHealthService
    {
        Task<List<INameValue<bool>>> ServersUptimeCheckAsync();
    }
}