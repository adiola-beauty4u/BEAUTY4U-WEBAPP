using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beauty4u.DataAccess.B4u;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto;
using Beauty4u.Interfaces.Services;

namespace Beauty4u.Business.Services
{
    public class SystemSetupService : ISystemSetupService
    {
        private readonly ISystemSetupRepository _systemRepository;
        public SystemSetupService(ISystemSetupRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        public async Task<Dictionary<string, ISystemSetupDto>> GetSystemSetupAsync()
        {
            var systemSetup = await _systemRepository.GetSystemSetupAsync();

            return systemSetup.ToDictionary(x => x.Code);
        }
        public async Task<List<ISysCodeDto>> GetSysCodesByClassAsync(string value)
        {
            var systemSetup = await _systemRepository.GetSysCodesByClassAsync(value);

            return systemSetup;
        }
    }
}
