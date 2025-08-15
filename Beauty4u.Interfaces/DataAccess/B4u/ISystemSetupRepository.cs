using Beauty4u.Interfaces.Dto;

namespace Beauty4u.Interfaces.DataAccess.B4u
{
    public interface ISystemSetupRepository
    {
        Task<List<ISysCodeDto>> GetSysCodesByClassAsync(string value);
        Task<List<ISystemSetupDto>> GetSystemSetupAsync();
    }
}