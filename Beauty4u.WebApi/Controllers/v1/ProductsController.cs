using Asp.Versioning;
using AutoMapper;
using Beauty4u.Interfaces.Api.Auth;
using Beauty4u.Interfaces.Dto.Products;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api;
using Beauty4u.Models.Api.Products;
using Beauty4u.Models.Api.Table;
using Beauty4u.Models.Common;
using Beauty4u.Models.Dto.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileReadHelper _fileReadHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private ICurrentUserContext? _currentUserContext;
        public ProductsController(IProductService productService, IFileReadHelper fileReadHelper, IMapper mapper, ICurrentUserService currentUserService)
        {
            _productService = productService;
            _fileReadHelper = fileReadHelper;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _currentUserContext = _currentUserService.GetCurrentUser();
        }

        [HttpPost("bulk-register")]
        public async Task<IActionResult> BulkRegister([FromForm] BulkProductRequest bulkProductRequest)
        {
            if (bulkProductRequest.ProductFile == null || bulkProductRequest.ProductFile.Length == 0)
                return BadRequest("File is empty or missing.");

            var result = await _productService.BulkProductRegisterAsync(bulkProductRequest);

            return Ok(result);
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromForm] IFormFile productFile)
        {
            if (productFile == null || productFile.Length == 0)
                return BadRequest("File is empty or missing.");

            var result = await _productService.BulkProductUpdateAsync(productFile);

            return Ok(result);
        }

        [HttpPost("transfer-search")]
        public async Task<IActionResult> TransferSearch([FromBody] DateSearchRequest dateSearchRequest)
        {
            var result = await _productService.ProductTransferSearchAsync(dateSearchRequest.DateStart, dateSearchRequest.DateEnd);

            return Ok(result);
        }

        [HttpPost("bulk-register-preview")]
        public async Task<IActionResult> BulkRegisterPreview([FromForm] BulkProductRequest bulkProductRequest)
        {
            if (bulkProductRequest.ProductFile == null || bulkProductRequest.ProductFile.Length == 0)
                return BadRequest("File is empty or missing.");

            var result = await _productService.BulkRegisterPreviewAsync(bulkProductRequest);

            return Ok(result);
        }

        [HttpPost("bulk-update-preview")]
        public async Task<IActionResult> BulkUpdatePreview([FromForm] IFormFile productFile)
        {
            if (productFile == null || productFile.Length == 0)
                return BadRequest("File is empty or missing.");

            var result = await _productService.BulkUpdatePreviewAsync(productFile);

            return Ok(result);
        }

        [HttpPost("search-by-upc")]
        public async Task<IActionResult> SearchByUPC([FromBody] List<string> upcList)
        {
            var result = await _productService.ProductSearchByUPCListAsync(upcList);

            return Ok(result);
        }

        [HttpPost("transfer-preview")]
        public async Task<IActionResult> TransferPreview([FromBody] ProductTransferRequest productTransferRequest)
        {
            var result = await _productService.ProductTransferPreviewAsync(productTransferRequest);

            return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] List<SearchProductResult> productTransferRequest)
        {
            var result = await _productService.TransferProductsAsync(productTransferRequest.ToList<ISearchProductResult>());

            return Ok(new ProductTransferApiResult() { TableData = _mapper.Map<TableDataApi>(result.TableData), StartTime = result.StartTime, EndTime = result.EndTime, StoreCode = result.StoreCode, TransferResult = result.TransferResult.Cast<BulkProductUpdatePreviewResult>().ToList() });
        }

        [HttpPost("store-transfers")]
        public async Task<IActionResult> StoreTransfers([FromBody] ProductTransferRequest productTransferRequest)
        {
            var result = await _productService.ProductTransferToStoresAsync(productTransferRequest);

            return Ok(result);
        }

        [HttpPost("product-search")]
        public async Task<IActionResult> ProductSearch([FromBody] ProductSearchParams productSearchParams)
        {
            var result = await _productService.SearchProductsAsync(productSearchParams);
            return Ok(result);
        }
    }
}
