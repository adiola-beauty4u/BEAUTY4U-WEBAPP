using Beauty4u.Models.Common;

namespace Beauty4u.Interfaces.Api.Health
{
    public interface IHealthApi
    {
        Task<INameValue<bool>> HealthCheck(string storeCode, string baseAddress);
    }
}