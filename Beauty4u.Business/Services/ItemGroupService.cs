using Beauty4u.Interfaces.Common.Utilities;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.ItemGroup;
using Beauty4u.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public class ItemGroupService : IItemGroupService
    {
        private readonly IItemGroupRepository _itemGroupRepository;
        private readonly ILogger<StoreService> _logger;
        private readonly IMemoryCacheService _cache;
        public ItemGroupService(IItemGroupRepository itemGroupRepository, ILogger<StoreService> logger, IMemoryCacheService cache)
        {
            _itemGroupRepository = itemGroupRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Dictionary<string, IItemGroupDto>> GetActiveItemGroupsAsync()
        {
            var itemGroups = await _itemGroupRepository.GetActiveItemGroupsAsync();

            return itemGroups.ToDictionary(x => x.Code);
        }
    }
}
