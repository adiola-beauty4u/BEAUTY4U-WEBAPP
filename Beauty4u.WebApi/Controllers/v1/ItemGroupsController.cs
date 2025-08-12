using Asp.Versioning;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Dto.ItemGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ItemGroupsController : ControllerBase
    {
        private readonly IItemGroupService _itemGroupService;
        public ItemGroupsController(IItemGroupService itemGroupService)
        {
            _itemGroupService = itemGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var itemGroups = await _itemGroupService.GetGroupedItemGroupsAsync();
            return Ok(itemGroups.OrderBy(x => x.Code).Cast<ItemGroupDto>());
        }
    }
}
