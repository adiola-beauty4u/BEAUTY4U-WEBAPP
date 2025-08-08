using Beauty4u.Interfaces.Common.Utilities;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.ItemGroup;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Dto.ItemGroup;
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

        public async Task<List<IItemGroupDto>> GetGroupedItemGroupsAsync()
        {
            var itemGroups = await _itemGroupRepository.GetActiveItemGroupsAsync();
            var level1 = itemGroups.Where(x => x.Code.Length == 1).Cast<ItemGroupDto>();
            foreach (var item in level1)
            {
                var level2 = itemGroups.Where(x => x.Level1Code == item.Code && x.Code.Length == 2).Cast<ItemGroupDto>();
                foreach(var item2 in level2)
                {
                    item2.ChildItemGroups = itemGroups.Where(x=>x.Code.Length == 3 && x.Level2Code == item2.Code).Cast<ItemGroupDto>().ToList();
                }
                item.ChildItemGroups = level2.ToList();
            }
            return level1.ToList<IItemGroupDto>();
        }
    }
}
