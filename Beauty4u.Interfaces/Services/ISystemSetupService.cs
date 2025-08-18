using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Interfaces.Services
{
    public interface ISystemSetupService
    {
        Task<List<ISysCodeDto>> GetSysCodesByClassAsync(string value);
        Task<Dictionary<string, ISystemSetupDto>> GetSystemSetupAsync();
    }
}